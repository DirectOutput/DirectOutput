using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DirectOutput.Cab.Out.AdressableLedStrip
{
    public abstract class UMXDevice
    {
        public enum UMXCommand
        {
            UMX_Handshake,
            UMX_GetInfos,
            UMX_AllOff,
            UMX_SendStripsData
        }

        public class LedStripSplit
        {
            public int DataLine { get; set; } = 0;
            public int NbLeds { get; set; } = 0;
        }

        public class LedStripDescriptor
        {
            public string Name { get; set; } = string.Empty;
            public int Width { get; set; } = 0;
            public int Height { get; set; } = 0;
            public int NbLeds => Width * Height;
            public int FirstLedIndex { get; set; } = 0;
            public byte DofOutputNum { get; set; } = 0;

            public Curve.CurveTypeEnum FadeMode { get; set; } = Curve.CurveTypeEnum.SwissLizardsLedCurve;
            public LedStripArrangementEnum Arrangement { get; set; } = LedStripArrangementEnum.LeftRightTopDown;
            public RGBOrderEnum ColorOrder { get; set; } = RGBOrderEnum.RBG;
            public byte Brightness { get; set; } = 100;
            public List<LedStripSplit> Splits { get; set; } = new List<LedStripSplit>();
            public int FirstDataline => Splits.Count == 0 ? 0 : Splits.Min(S => S.DataLine);
        }

        public List<LedStripDescriptor> LedStrips = new List<LedStripDescriptor>();

        public override string ToString()
        {
            return $"{name} (unit {unitNo}, ledWiz {ledWizEquivalent}, {numOutputs} outputs)";
        }

        public int UnitNo()
        {
            return unitNo;
        }

        public int NumOutputs()
        {
            return numOutputs;
        }

        protected void ComputeNumOutputs()
        {
            if (numOutputs > 0)
                return;
            numOutputs = 0;
            foreach (var ledstrip in LedStrips) {
                numOutputs += ledstrip.Width * ledstrip.Height * 3;
            }
        }

        internal bool VerifySettings()
        {
            return true;
        }

        internal class DataLine
        {
            public int NbLeds = 0;
            public int NbValues = 0;
            public byte[] OldValues;
            public byte[] Values;
        }

        DataLine[] DataLines = null;

        public void ResetDataLines()
        {
            foreach (DataLine line in DataLines) {
                line.OldValues = Enumerable.Repeat((byte)255, line.NbValues).ToArray();
            }
        }

        public void CreateDataLines()
        {
            DataLines = new DataLine[maxDataLines];
            for (int num = 0; num < maxDataLines; num++) {
                DataLines[num] = new DataLine();
            }

            foreach(var ledstrip in LedStrips) {
                foreach(var split in ledstrip.Splits) {
                    DataLines[split.DataLine].NbLeds += split.NbLeds;
                }
            }

            foreach (var line  in DataLines) {
                line.NbValues = line.NbLeds * 3;
                line.OldValues = Enumerable.Repeat((byte)(255), line.NbValues).ToArray();
            }
        }

        internal void UpdateOutputs(byte[] outputValues)
        {
            if (!enabled) 
                return;

            //Clear current values
            foreach(DataLine line in DataLines) {
                if (line.NbValues > 0) {
                    line.Values = new byte[0];
                }
            }

            //Fill data line values with ledstrip corresponding ones
            int curOutputIndex = 0;
            foreach(var ledstrip in LedStrips) {
                foreach (var split in ledstrip.Splits) {
                    ref var line = ref DataLines[split.DataLine];
                    var nbValues = split.NbLeds * 3;
                    line.Values = line.Values.Concat(outputValues.Skip(curOutputIndex).Take(nbValues).ToArray());
                    curOutputIndex += nbValues;
                }
            }

            //prepare updated datalines buffer to send
            List<byte> sendBuffer = new List<byte>();
            byte nbChangedLines = 0;
            for(int numLine = 0; numLine < DataLines.Length; numLine++) {
                var line = DataLines[numLine];
                if (line.NbLeds > 0) {
                    if (!line.Values.SequenceEqual(line.OldValues)) {
                        nbChangedLines++;
                        sendBuffer.Add((byte)numLine);
                        List<byte> compressedLine = CompressDataLineValues(line.Values);
                        if (compressedLine.Count < (int)((float)line.Values.Length * 0.75f)) {
                            sendBuffer.Add((byte)1);
                            sendBuffer.Add((byte)(compressedLine.Count & 0xFF));
                            sendBuffer.Add((byte)(compressedLine.Count >> 8));
                            sendBuffer.Add((byte)(line.NbLeds & 0xFF));
                            sendBuffer.Add((byte)(line.NbLeds >> 8));
                            sendBuffer.AddRange(compressedLine);
                        } else {
                            sendBuffer.Add((byte)0);
                            sendBuffer.Add((byte)(line.NbLeds & 0xFF));
                            sendBuffer.Add((byte)(line.NbLeds >> 8));
                            sendBuffer.AddRange(line.Values);
                        }
                    }
                }
            }

            if (nbChangedLines > 0 && sendBuffer.Count > 0) {
                byte[] buffer = new byte[] { nbChangedLines };
                buffer = buffer.Concat(sendBuffer).ToArray();
                SendCommand(UMXCommand.UMX_SendStripsData, buffer);
            }
        }

        private List<byte> CompressDataLineValues(byte[] values)
        {
            var newList = new List<byte>();
            int num = 0;
            try {
                while(num < values.Length) {
                    if (values.Length - num <= 3) {
                        newList.Add(1);
                        newList.Add(0);
                        newList.Add((byte)values[num++]);
                        newList.Add((byte)values[num++]);
                        newList.Add((byte)values[num++]);
                        break;
                    } else {
                        byte r = (byte)values[num++];
                        byte g = (byte)values[num++];
                        byte b = (byte)values[num++];
                        int color = (r << 16) | (g << 8) | b;
                        int sameColCount = 1;
                        while (((values[num] << 16) | (values[num + 1] << 8) | values[num + 2]) == color) {
                            sameColCount++;
                            num += 3;
                            if (num >= values.Length) {
                                break;
                            }
                        }
                        newList.Add((byte)(sameColCount & 0xFF));
                        newList.Add((byte)(sameColCount >> 8));
                        newList.Add(r);
                        newList.Add(g);
                        newList.Add(b);
                    }
                }
            } catch {
                newList.Clear();
            }
            return newList;
        }

        public abstract void Initialize();
        public abstract void SendCommand(UMXCommand command, byte[] parameters = null);

        public string name;
        public short unitNo;
        public bool enabled;
        public byte maxDataLines = 0;
        public int maxNbLeds = int.MaxValue;
        public int nbLedsUpdatePerLine = int.MaxValue;
        public int ledWizEquivalent;
        public int numOutputs = 0;
        public bool testOnReset = false;
        public bool testButton = false;
        public byte testBrightness = 100;
        public bool activityLed = false;
    }
}
