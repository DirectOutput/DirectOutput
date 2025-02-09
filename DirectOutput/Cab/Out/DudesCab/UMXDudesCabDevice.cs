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
        public override void Initialize()
        {
            byte[] answer = null;

            //Handshak
            _device.SendCommand(HIDReportTypeMx.RT_UMXHANDSHAKE);
            answer = _device.ReadUSB().ToArray();
            answer = answer.Skip(hidCommandPrefixSize).ToArray();
            string handShake = Encoding.UTF8.GetString(answer).Trim('\0');
            Log.Write($"UMX Handshake : {handShake}");
            name = $"UMXDudesCab[{_device.name}]";

            //Ask for Configuration
            _device.SendCommand(HIDReportTypeMx.RT_MX_GETINFOS);
            answer = _device.ReadUSB().ToArray();
            answer = answer.Skip(hidCommandPrefixSize).ToArray();
            try {
                int index = 0;
                enabled = ReadBool(answer, ref index);
                maxDataLines = ReadByte(answer, ref index);
                maxNbLeds = ReadLong(answer, ref index);
                nbLedsUpdatePerLine = ReadLong(answer, ref index);
                ledWizEquivalent = ReadByte(answer, ref index); ;
                testOnReset = ReadBool(answer, ref index);
                testBrightness = ReadByte(answer, ref index);
                Log.Write($"Name: {name}, Enabled: {enabled}, MaxDataLines: {maxDataLines}, MaxNbLeds: {maxNbLeds}, Max Parallel Led Updates: {nbLedsUpdatePerLine}/s");
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
                Log.Write($"{LedStrips.Count} ledstrips ({totalLeds} leds, {numOutputs} outputs) configured");
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }

        public override void SendCommand(UMXCommand command, byte[] parameters = null)
        {
            switch (command) {
                case UMXCommand.UMX_SendStripsData:
                    _device.SendCommand(HIDReportTypeMx.RT_MX_OUTPUTS, parameters);
                    break;

                case UMXCommand.UMX_AllOff:
                    _device.SendCommand(HIDReportTypeMx.RT_MX_ALLOFF, parameters);
                    break;

                case UMXCommand.UMX_Handshake:
                    _device.SendCommand(HIDReportTypeMx.RT_UMXHANDSHAKE, parameters);
                    break;

                case UMXCommand.UMX_GetInfos:
                    _device.SendCommand(HIDReportTypeMx.RT_MX_GETINFOS, parameters);
                    break;
            }
        }

        DudesCab.Device _device = null;

        public DudesCab.Device Device
        {
            get { return _device; }
            set { _device = value; }
        }
    }
}
