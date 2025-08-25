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
    /// <summary>
    /// The UMXDevice is a base class for any implementation of the UMX (Unified MX) protocol for adressable leds effects.
    /// It's meant to be the common facade for the <see cref="UMXController"/>.
    /// 
    /// It has all the basis to build <see cref="LedStripDescriptor"/> toys from the configuration sent by the implemented device.
    /// The big difference from usual Adressable leds controllers is that the device has its own configuration so there is no need for cabinet.xml file anymore.
    /// You only need to implement the 3 main methods to have a functionnal UMX Device:
    ///    - <see cref="Initialize"/> : will retrieve the config and build the LedstripDescriptors list
    ///    - <see cref="SendCommand"/> : will send the UMXCommand to the UMXDevice
    ///    - <see cref="WaitAck"/> : wait for the acknowledgment from the UMXDevice after sending a command
    ///    
    /// UMXDevices protocol commands (command numbers can be remapped by the implementation) : 
    /// The 3 first one are not yet used by the <see cref="UMXDevice"/> base class, it's up to the implementation to manage them in the Initialize method.
    ///    - UMXHandshake, UMXGetInfos, UMXGetConfig 
    /// UMX_AllOff: reset the device leds, called only when disconnecting the device
    /// UMX_SendStripsData: will send either uncompressed or compressed data depending on the compressionRatio set by the config
    ///    For each line with changed data (all words or ints are little endian) : 
    ///    - line number (1 byte)
    ///    Depending on line compression
    ///         - Uncompressed data : 0 (1 byte), nb leds (1 word), byte[nbleds*3]
    ///         - Compressed data : 1 (1 byte), compressed size (1 word), nb leds (1 word)
    ///             - bytes[compressed size] : nb leds (1 word), byte[3] color to duplicate
    /// UMX_StartTest : will start a test using the <see cref="TestMode"/> enums with a duration in seconds
    /// 
    /// </summary>
    public abstract class UMXDevice
    {
        public enum UMXCommand
        {
            UMX_Handshake,
            UMX_GetInfos,
            UMX_GetConfig,
            UMX_AllOff,
            UMX_SendStripsData,
            UMX_StartTest
        }

        public enum TestMode
        {
            None,
            RGB,
            Colors,
            RGBLaser
        }

        public enum LedChipset
        {
            WS2811,
            WS2812,
            WS2812B,
            WS2813,
            WS2815,
            SK6812
        }

        public static readonly List<Tuple<LedChipset, int>> LedsCaps = new List<Tuple<LedChipset, int>>() {
            new Tuple<LedChipset, int>(LedChipset.WS2811, 30000),
            new Tuple<LedChipset, int>(LedChipset.WS2812, 30000),
            new Tuple<LedChipset, int>(LedChipset.WS2812B, 30000),
            new Tuple<LedChipset, int>(LedChipset.WS2813, 30000),
            new Tuple<LedChipset, int>(LedChipset.SK6812, 30000),
        };

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

        public bool Enabled => enabled;
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

        public class DataLine
        {
            public int NbLeds = 0;
            public int NbValues = 0;
            public byte[] OldValues;
            public byte[] Values;
        }

        public DataLine[] DataLines = null;

        public int LongestDataLineNbLeds = 0;

        public void ResetDataLines()
        {
            foreach (DataLine line in DataLines) {
                line.OldValues = Enumerable.Repeat((byte)255, line.NbValues).ToArray();
            }
        }

        public void CreateDataLines()
        {
            LongestDataLineNbLeds = 0;

            DataLines = new DataLine[maxDataLines];
            for (int num = 0; num < maxDataLines; num++) {
                DataLines[num] = new DataLine();
            }

            foreach(var ledstrip in LedStrips) {
                foreach(var split in ledstrip.Splits) {
                    DataLines[split.DataLine].NbLeds += split.NbLeds;
                    LongestDataLineNbLeds = Math.Max(LongestDataLineNbLeds, DataLines[split.DataLine].NbLeds);
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
                        float compRatio = (float)compressedLine.Count / line.Values.Length;
                        if (compRatio < compressionRatio * 0.01f) {
                            Log.Instrumentation("UMX", $"Send compressed Mx Data (ratio {compRatio * 100:F2}%)");
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

                        line.Values.CopyTo(line.OldValues, 0);
                    }
                }
            }

            if (nbChangedLines > 0 && sendBuffer.Count > 0) {
                byte[] buffer = new byte[] { nbChangedLines };
                buffer = buffer.Concat(sendBuffer).ToArray();
                SendCommand(UMXCommand.UMX_SendStripsData, buffer);
                WaitAck((byte)UMXCommand.UMX_SendStripsData);
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

        #region Abstract Methods
        public abstract void Initialize();
        public abstract void SendCommand(UMXCommand command, byte[] parameters = null);
        public abstract void WaitAck(byte command);
        #endregion

        public string name;
        public short unitNo;
        public bool enabled;
        public Version umxVersion;
        public byte maxDataLines = 0;
        public int maxNbLeds = int.MaxValue;
        public LedChipset ledChipset = LedChipset.WS2812B;
        public int ledWizEquivalent;
        public int numOutputs = 0;
        public TestMode testOnReset = TestMode.None;
        public byte testOnResetDuration = 0;
        public TestMode testOnConnect = TestMode.None;
        public byte testOnConnectDuration = 0;
        public byte testBrightness = 100;
        public byte compressionRatio = 75;
        public int totalLeds = 0;
        public bool activityLed = false;
    }
}
