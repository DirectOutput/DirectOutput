using DirectOutput.Cab.Out.AdressableLedStrip;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.FX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DirectOutput.Cab.Out.AdressableLedStrip.UMXDevice;
using static DirectOutput.Cab.Out.DudesCab.DudesCab.Device;
using static DirectOutput.General.Curve;

namespace DirectOutput.Cab.Out.DudesCab
{
    public class UMXDudesCabDevice : UMXDevice
    {
        internal bool ReadBool(byte[] data, ref int index)
        {
            return data[index++] > 0 ? true : false;
        }
        internal byte ReadByte(byte[] data, ref int index)
        {
            return data[index++];
        }
        internal short ReadShort(byte[] data, ref int index)
        {
            return (short)(data[index++] | (data[index++] << 8));
        }
        internal int ReadLong(byte[] data, ref int index)
        {
            return (data[index++] | (data[index++] << 8) | (data[index++] << 16) | (data[index++] << 24));
        }

        internal string ReadString(byte[] data, ref int index)
        {
            string strRead = string.Empty;
            byte len = data[index++];
            if (len > 0) {
                for (int i = 0; i < len; i++) {
                    strRead += (char)data[index++];
                }
            }
            return strRead;
        }

        public override void Initialize()
        {
            byte[] answer = null;

            //Handshak
            _device.SendCommand(HIDReportType.RT_MX_HANDSHAKE);
            answer = _device.ReadUSB().ToArray();
            answer = answer.Skip(hidCommandPrefixSize).ToArray();
            string handShake = Encoding.UTF8.GetString(answer).Trim('\0');
            Log.Write($"UMX Handshake : {handShake}");
            name = $"UMXDudesCab[{_device.name}]";

            //Ask for Configuration
            _device.SendCommand(HIDReportType.RT_MX_GETINFOS);
            answer = _device.ReadUSB().ToArray();
            answer = answer.Skip(hidCommandPrefixSize).ToArray();
            try {
                int index = 0;
                enabled = ReadBool(answer, ref index);
                maxNbLeds = ReadLong(answer, ref index);
                nbLedsUpdatePerLine = ReadLong(answer, ref index);
                ledWizEquivalent = ReadByte(answer, ref index); ;
                testOnReset = ReadBool(answer, ref index);
                testBrightness = ReadByte(answer, ref index);
                Log.Write($"Name: {name}, Enabled: {enabled}, MaxNbLeds: {maxNbLeds}, Max Parallel Led Updates: {nbLedsUpdatePerLine}/s");
                Log.Write($"LedWizEquivalent: {ledWizEquivalent}, TestOnReset: {testOnReset}, TestBrightness: {testBrightness}");
                var nbLedstrips = ReadByte(answer, ref index);
                Log.Write($"{nbLedstrips} ledstrips :");
                int curLedIndex = 0;
                int totalLeds = 0;
                for (int numStrip = 0; numStrip < nbLedstrips; numStrip++) {
                    var ledstrip = new LedStripDescriptor() {
                        Name = ReadString(answer, ref index),
                        Width = ReadShort(answer, ref index),
                        Height = ReadShort(answer, ref index),
                        FirstLedIndex = curLedIndex,
                        DofOutputNum = ReadByte(answer, ref index),
                        FadeMode = (CurveTypeEnum)ReadByte(answer, ref index),
                        Arrangement = (LedStripArrangementEnum)ReadByte(answer, ref index),
                        ColorOrder = (RGBOrderEnum)ReadByte(answer, ref index),
                        Brightness = ReadByte(answer, ref index)
                    };
                    var nbSplits = ReadByte(answer, ref index);
                    for (int numSplit = 0; numSplit < nbSplits; numSplit++) {
                        ledstrip.Splits.Add(new LedStripSplit() {
                            DataLine = ReadByte(answer, ref index),
                            NbLeds = ReadShort(answer, ref index)
                        });
                    }
                    curLedIndex += ledstrip.NbLeds;
                    totalLeds += ledstrip.NbLeds;
                    Log.Write($"\t{numStrip} => W/H:{ledstrip.Width}*{ledstrip.Height} ({ledstrip.NbLeds} leds), FirstLed: {ledstrip.FirstLedIndex}, Dof:{ledstrip.DofOutputNum}, Brightness:{ledstrip.Brightness} [{ledstrip.FadeMode},{ledstrip.Arrangement},{ledstrip.ColorOrder}]");
                    if (nbSplits == 1) {
                        Log.Write($"\t\t1 split : {ledstrip.Splits.Last().NbLeds} leds on line {ledstrip.Splits.Last().DataLine}");
                    } else {
                        Log.Write($"\t\t{nbSplits} splits :");
                        foreach(var split in ledstrip.Splits) {
                            Log.Write($"\t\t\t{split.NbLeds} leds on line {split.DataLine}");
                        }
                    }
                    LedStrips.Add(ledstrip);
                }
                ComputeNumOutputs();
                BuildDataLines();
                Log.Write($"{LedStrips.Count} ledstrips ({totalLeds} leds, {numOutputs} outputs) configured");
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }

        private void BuildDataLines()
        {
            
        }

        public override void SendCommand(UMXCommand command, byte[] parameters = null)
        {
            throw new NotImplementedException();
        }

        DudesCab.Device _device = null;

        public DudesCab.Device Device
        {
            get { return _device; }
            set { _device = value; }
        }
    }
}
