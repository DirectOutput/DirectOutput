using DirectOutput.Cab.Out.AdressableLedStrip;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.FX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static DirectOutput.Cab.Out.AdressableLedStrip.UMXDevice;
using static DirectOutput.Cab.Out.DudesCab.DudesCab.Device;
using static DirectOutput.General.Curve;

namespace DirectOutput.Cab.Out.DudesCab
{
    public class UMXDudesCabDevice : UMXDevice
    {
        public const byte CompressionRatioVersion = 7;

        public override void Initialize()
        {
            byte[] answer = null;

            try {
                //Handshake
                _device.SendCommand(HIDReportTypeMx.RT_UMXHANDSHAKE);
                answer = _device.ReadUSB((byte)HIDReportTypeMx.RT_UMXHANDSHAKE).ToArray();
                answer = answer.Skip(hidCommandPrefixSize).ToArray();
                string handShake = Encoding.UTF8.GetString(answer).Trim('\0');
                Log.Write($"UMX Handshake : {handShake}");
                name = $"UMXDudesCab[{_device.name}]";
            } catch (Exception ex) {
                throw new Exception($"Exception during Handshake of UMXDudesCabDevice (answer size {answer.Length} bytes): {ex.Message}");
            }

            try {
                //Ask for Informations
                _device.SendCommand(HIDReportTypeMx.RT_MX_GETINFOS);
                answer = _device.ReadUSB((byte)HIDReportTypeMx.RT_MX_GETINFOS).ToArray();
                answer = answer.Skip(hidCommandPrefixSize).ToArray();

                int index = 0;
                umxVersion = new Version(ReadByte(answer, ref index), ReadByte(answer, ref index), ReadByte(answer, ref index));
                maxDataLines = ReadByte(answer, ref index);
                maxNbLeds = ReadShort(answer, ref index);
            } catch (Exception ex) {
                throw new Exception($"Exception during GetInfos of UMXDudesCabDevice {_device.name} (answer [{string.Join(",", answer)}]) : {ex.Message}");
            }

            try { 
                //Ask for Configuration
                _device.SendCommand(HIDReportTypeMx.RT_MX_GETCONFIG);
                answer = _device.ReadUSB((byte)HIDReportTypeMx.RT_MX_GETCONFIG).ToArray();
                answer = answer.Skip(hidCommandPrefixSize).ToArray();
                totalLeds = 0;
                int index = 0;
                enabled = ReadBool(answer, ref index);
                ledChipset = (LedChipset)ReadByte(answer, ref index);
                ledWizEquivalent = ReadByte(answer, ref index); ;
                testOnReset = (TestMode)ReadByte(answer, ref index);
                testOnResetDuration = ReadByte(answer, ref index);
                testOnConnect = (TestMode)ReadByte(answer, ref index);
                testOnConnectDuration = ReadByte(answer, ref index);
                testBrightness = ReadByte(answer, ref index);
                if (_device.configVersion >= CompressionRatioVersion)
                    compressionRatio = ReadByte(answer, ref index);
                var nbLedstrips = ReadByte(answer, ref index);
                LedStrips.Clear();
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
            } catch (Exception ex) {
                throw new Exception($"Exception during GetConfig of UMXDudesCabDevice {_device.name} (answer size {answer.Length} bytes): {ex.Message}");
            }
        }

        public override void SendCommand(UMXCommand command, byte[] parameters = null)
        {
            var dudeCommand = UMXToDudeCommand(command);
            _device.SendCommand(dudeCommand, parameters);
        }

        private HIDReportTypeMx UMXToDudeCommand(UMXCommand command)
        {
            switch (command) {
                case UMXCommand.UMX_SendStripsData:
                    return HIDReportTypeMx.RT_MX_OUTPUTS;

                case UMXCommand.UMX_StartTest:
                    return HIDReportTypeMx.RT_MX_RUNTEST;

                case UMXCommand.UMX_AllOff:
                    return HIDReportTypeMx.RT_MX_ALLOFF;

                case UMXCommand.UMX_Handshake:
                    return HIDReportTypeMx.RT_UMXHANDSHAKE;

                case UMXCommand.UMX_GetInfos:
                    return HIDReportTypeMx.RT_MX_GETINFOS;

                case UMXCommand.UMX_GetConfig:
                    return HIDReportTypeMx.RT_MX_GETCONFIG;

                default:
                    throw new Exception($"Invalid UMX to Dude command remap [{command}]");
            }
        }

        public override void WaitAck(byte command)
        {
            return;
            byte[] answer = _device.ReadUSB((byte)UMXToDudeCommand((UMXCommand)command)).ToArray();
            if (answer.Length != hidCommandPrefixSize+1) {
                throw new Exception($"The {this.GetType().ToString()} did not send the expected {hidCommandPrefixSize + 1} bytes containing the acknowledge. Received {answer.Length} bytes instead. Will not send data to the controller");
            }
            if (answer[hidCommandPrefixSize] != 'A') {
                throw new Exception($"The {this.GetType().ToString()} did not send a ACK. Will not send data to the controller");
            }
            Log.Instrumentation("DudesCab,Mx", $"Received Ack");
        }

        DudesCab.Device _device = null;

        public DudesCab.Device Device
        {
            get { return _device; }
            set { _device = value; }
        }
    }
}
