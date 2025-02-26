using DirectOutput.Cab.Out.AdressableLedStrip;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.FX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            //Handshake
            _device.SendCommand(HIDReportTypeMx.RT_UMXHANDSHAKE);
            answer = _device.ReadUSB().ToArray();
            answer = answer.Skip(hidCommandPrefixSize).ToArray();
            string handShake = Encoding.UTF8.GetString(answer).Trim('\0');
            Log.Write($"UMX Handshake : {handShake}");
            name = $"UMXDudesCab[{_device.name}]";

            //Ask for Informations
            _device.SendCommand(HIDReportTypeMx.RT_MX_GETINFOS);
            answer = _device.ReadUSB().ToArray();
            answer = answer.Skip(hidCommandPrefixSize).ToArray();
            try {
                int index = 0;
                umxVersion = new Version(ReadByte(answer, ref index), ReadByte(answer, ref index), ReadByte(answer, ref index));
                maxDataLines = ReadByte(answer, ref index);
                maxNbLeds = ReadShort(answer, ref index);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

            //Ask for Configuration
            _device.SendCommand(HIDReportTypeMx.RT_MX_GETCONFIG);
            answer = _device.ReadUSB().ToArray();
            answer = answer.Skip(hidCommandPrefixSize).ToArray();
            totalLeds = 0;
            try {
                int index = 0;
                enabled = ReadBool(answer, ref index);
                ledChipset = (LedChipset)ReadByte(answer, ref index);
                ledWizEquivalent = ReadByte(answer, ref index); ;
                testOnReset = (TestMode)ReadByte(answer, ref index);
                testOnResetDuration = ReadByte(answer, ref index);
                testOnConnect = (TestMode)ReadByte(answer, ref index);
                testOnConnectDuration = ReadByte(answer, ref index);
                testBrightness = ReadByte(answer, ref index);
                var nbLedstrips = ReadByte(answer, ref index);
                int curLedIndex = 0;
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
                    LedStrips.Add(ledstrip);
                }
                ComputeNumOutputs();
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

                case UMXCommand.UMX_StartTest:
                    _device.SendCommand(HIDReportTypeMx.RT_MX_RUNTEST, parameters);
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

                case UMXCommand.UMX_GetConfig:
                    _device.SendCommand(HIDReportTypeMx.RT_MX_GETCONFIG, parameters);
                    break;

                default:
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
