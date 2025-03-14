using DirectOutput.Cab.Out.AdressableLedStrip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using static DirectOutput.Cab.Out.DudesCab.DudesCab.Device;

namespace DirectOutput.Cab.Out.DudesCab
{
    /// <summary>
    /// The <a href="https://shop.arnoz.com/en/">Dude's Cab Controller</a> is a all-in-one virtual pinball controller
    /// 
    /// It's based on the RP2040 processor, provide 32 inputs, accelerometer, plunger &amp; Dof support.
    /// The Dof support provides 128 PWM outputs through up to 8 extension boards.
    /// Maximum intensity, Flipper logic, Chime logic &amp; Gamma Correction are supported for all outputs
    /// Each outputs can also have its intensity lowered independently from the Dof values
    /// As for the Pinscape controller, the communication is fully supported through Hid In/Out protocol with multipart messages support
    /// The Dude's Cab controllers are registered in the Dof config tool from unit #90 to 94
    /// 
    /// </summary>
    public class DudesCab : OutputControllerFlexCompleteBase
    {
        public static readonly ushort VendorID = 0x2E8A;
        public static readonly ushort ProductID = 0x106F;

        #region Number


        private object NumberUpdateLocker = new object();
        private int _Number = -1;

        /// <summary>
        /// Gets or sets the unit number of the controller.<br />
        /// The unit number must be unique.<br />
        /// Setting changes the Name property, if it is blank or if the Name coresponds to "DudesCab Controller {0}".
        /// </summary>
        /// <value>
        /// The unique unit number of the controller (Range 1-5).
        /// </value>
        /// <exception cref="System.Exception">
        /// Dude's Cab Unit Numbers must be between 1-5. The supplied number {0} is out of range.
        /// </exception>
        public int Number
        {
            get { return _Number; }
            set {
                if (!value.IsBetween(1, 5)) {
                    throw new Exception("DudesCab Unit Numbers must be between 1-5. The supplied number {0} is out of range.".Build(value));
                }
                lock (NumberUpdateLocker) {
                    // if the unit number changed, update it and attach to the new unit
                    if (_Number != value) {
                        // if we used a default name for the old unit number, change to the default
                        // name for the new unit number
                        if (Name.IsNullOrWhiteSpace() || Name == "DudesCab Controller {0:00}".Build(_Number)) {
                            Name = "DudesCab Controller {0:00}".Build(value);
                        }

                        // remember the new unit number
                        _Number = value;

                        // attach to the new device record for this unit number, updating the output list to match
                        this.Dev = Devices.First(D => D.UnitNo() == Number);
                        this.NumberOfOutputs = this.Dev.NumOutputs();
                        this.OldOutputValues = Enumerable.Repeat((byte)255, this.NumberOfOutputs).ToArray();
                    }
                }
            }
        }

        #endregion


        #region MinCommandIntervalMs property core parts
        private int _MinCommandIntervalMs = 1;
        private bool MinCommandIntervalMsSet = false;

        /// <summary>
        /// Gets or sets the mininimal interval between command in miliseconds (Default: 1ms).
        ///
        /// We recommend using the default interval of 1 ms, and only increasing this if problems occur (Toys which are sometimes not reacting, random
        /// knocks of replay knocker or solenoids).  Better yet, any such problems should be investigated first on the DudesCab controller side to see if
        /// they can be addressed more cleanly there.
        /// </summary>
        /// <value>
        /// The mininimal interval between command in miliseconds.  The default is 1ms, which is also the minimum, since it's
        /// the fastest that USB allows at the hardware protocol level.
        /// </value>
        public int MinCommandIntervalMs
        {
            get { return _MinCommandIntervalMs; }
            set {
                _MinCommandIntervalMs = value.Limit(0, 1000);
                MinCommandIntervalMsSet = true;
            }
        }

        #endregion

        #region IOutputcontroller implementation

        /// <summary>
        /// Initializes the DudesCab object.<br />
        /// This method does also start the workerthread which does the actual update work when Update() is called.<br />
        /// This method should only be called once. Subsequent calls have no effect.
        /// </summary>
        /// <param name="Cabinet">The Cabinet object which is using the DudesCab instance.</param>
        public override void Init(Cabinet Cabinet)
        {
            // get the minimum update interval from the global config
            if (!MinCommandIntervalMsSet
                && Cabinet.Owner.ConfigurationSettings.ContainsKey("DudesCabDefaultMinCommandIntervalMs")
                && Cabinet.Owner.ConfigurationSettings["DudesCabDefaultMinCommandIntervalMs"] is int)
                MinCommandIntervalMs = (int)Cabinet.Owner.ConfigurationSettings["DudesCabDefaultMinCommandIntervalMs"];

            // do the base class work
            base.Init(Cabinet);
        }

        /// <summary>
        /// Finishes the DudesCab object.<br/>
        /// Finish does also terminate the workerthread for updates.
        /// </summary>
        public override void Finish()
        {
            Dev.AllOff();
            base.Finish();
        }
        #endregion


        #region OutputControllerFlexCompleteBase implementation

        /// <summary>
        /// Verify settings.  Returns true if settings are valid, false otherwise.  In the current implementation,
        /// there's nothing to check; we simply return true unconditionally.
        /// </summary>
        protected override bool VerifySettings()
        {
            return true;
        }

        protected List<byte> OutputBuffer = new List<byte>();

        private void Instrumentation(string Message)
        {
            Log.Instrumentation("DudesCab", Message);
        }

        /// <summary>
        /// Send updated outputs to the physical device.
        /// </summary>
        protected override void UpdateOutputs(byte[] NewOutputValues)
        {
            if (NewOutputValues.All(x => x == 0)) {
                Dev.AllOff();
            } else {
                OutputBuffer.Clear();
                OutputBuffer.Add(0); //extension mask
                int nbValuesToSend = 0;

                byte extMask = 0;
                byte oldExtMask = 0xFF;
                int outputMaskOffset = 0;
                ushort outputMask = 0;

                for (int numDofOutput = 0; numDofOutput < NewOutputValues.Length; numDofOutput++) {
                    if (NewOutputValues[numDofOutput] != OldOutputValues[numDofOutput]) {
                        byte extNum = (byte)(numDofOutput / Dev.PwmMaxOutputsPerExtension);
                        byte outputNum = (byte)(numDofOutput % Dev.PwmMaxOutputsPerExtension);
                        Instrumentation($"Prepare Dof Value to send : DOF #{numDofOutput} {OldOutputValues[numDofOutput]} => {NewOutputValues[numDofOutput]}, Extension #{extNum}, Output #{outputNum}");
                        extMask |= (byte)(1 << extNum);
                        if (oldExtMask != extMask) {
                            //New extension add output masks placeholders
                            oldExtMask = extMask;
                            //Set previous outputmask if available
                            if (outputMask != 0) {
                                OutputBuffer[outputMaskOffset] = (byte)(outputMask & 0xFF);
                                OutputBuffer[outputMaskOffset + 1] = (byte)((outputMask >> 8) & 0xFF);
                                Instrumentation($"        Changed OutputMask 0x{outputMask:X4}");
                                outputMask = 0;
                            }
                            Instrumentation($"    Extension {extNum} has changes");
                            outputMaskOffset = OutputBuffer.Count;
                            OutputBuffer.Add(0);//Low bits of output mask
                            OutputBuffer.Add(0);//High bits of output mask
                        }

                        outputMask |= (ushort)(1 << outputNum);
                        OutputBuffer.Add(NewOutputValues[numDofOutput]);
                        nbValuesToSend++;
                    }
                }

                //set last outputmask & extmask
                if (outputMask != 0) {
                    OutputBuffer[outputMaskOffset] = (byte)(outputMask & 0xFF);
                    OutputBuffer[outputMaskOffset + 1] = (byte)((outputMask >> 8) & 0xFF);
                    Instrumentation($"        Changed OutputMask 0x{outputMask:X4}");
                }
                OutputBuffer[0] = extMask;
                Instrumentation($"    ExtenstionMask 0x{OutputBuffer[0]:X2}");

                Instrumentation($"{nbValuesToSend} Dof Values to send to Dude's cab");
                Dev.SendCommand(Device.HIDReportType.RT_PWM_OUTPUTS, OutputBuffer.ToArray());
            }

            Array.Copy(NewOutputValues, OldOutputValues, OldOutputValues.Length);
        }

        byte[] OldOutputValues;

        private DateTime LastUpdate = DateTime.Now;
        private void UpdateDelay()
        {
            int Ms = (int)DateTime.Now.Subtract(LastUpdate).TotalMilliseconds;
            if (Ms < MinCommandIntervalMs)
                Thread.Sleep((MinCommandIntervalMs - Ms).Limit(0, MinCommandIntervalMs));
            LastUpdate = DateTime.Now;
        }

        /// <summary>
        /// Connect to the controller.
        /// </summary>
        protected override void ConnectToController()
        {
        }

        /// <summary>
        /// Disconnect from the controller.
        /// </summary>
        protected override void DisconnectFromController()
        {
            // if we've generated any updates, send an All Off signal
            if (InUseState == InUseStates.Running) {
                if (Log.HasInstrumentations("DudesCabLog")) {
                    Log.Write($"Clear the DudesCab forced LogLevel");
                    Dev.SendCommand(HIDCommonReportType.RT_FORCELOGLEVEL, new byte[] { (byte)DudesCabLogLevel.None });
                }
                Dev.AllOff();
            }
        }

        #endregion

        #region USB Communications

        public class Device
        {
            public enum RIDType : byte
            {
                None = 0,
                RIDOutputs = 3,
                RIDOutputsMx = 5
            };

            public enum HIDCommonReportType : byte
            {
                RT_HANDSHAKE = 1,
                RT_SETADMIN,
                RT_VERSION,
                RT_GETSTATUS,
                RT_FORCELOGLEVEL,
                RT_MAX,
                RT_GETINFOS_OLDPROTOCOL = 4
            };

            public enum DudesCabLogLevel : byte
            {
                None = 0,
                Errors,
                Warnings,
                Infos,
                Debug
            };

            public enum HIDReportType : byte
            {
                //PWM
                RT_PWM_GETINFOS = HIDCommonReportType.RT_MAX,
                RT_PWM_ALLOFF,
                RT_PWM_OUTPUTS,

                RT_MAX
            };

            public enum HIDReportTypeMx : byte
            {
                //MX 
                RT_UMXHANDSHAKE = HIDCommonReportType.RT_MAX,
                RT_MX_GETINFOS,
                RT_MX_GETCONFIG,
                RT_MX_ALLOFF,
                RT_MX_OUTPUTS,
                RT_MX_RUNTEST,

                RT_MAX
            }

            public override string ToString()
            {
                return $"{devicename} (name: {name} unit:{UnitNo()})";
            }

            public int UnitNo()
            {
                return unitNo;
            }

            public static bool ReadBool(byte[] data, ref int index)
            {
                return data[index++] > 0 ? true : false;
            }
            public static byte ReadByte(byte[] data, ref int index)
            {
                return data[index++];
            }
            public static short ReadShort(byte[] data, ref int index)
            {
                return (short)(data[index++] | (data[index++] << 8));
            }
            public static int ReadLong(byte[] data, ref int index)
            {
                return (data[index++] | (data[index++] << 8) | (data[index++] << 16) | (data[index++] << 24));
            }
            public static string ReadString(byte[] data, ref int index)
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


            private int _NumOutputs = 128;

            public int NumOutputs() => _NumOutputs;

            static public readonly int hidCommandPrefixSize = 5;

            public Device(RIDType rid, IntPtr fp, string path, string name, string serial, ushort vendorID, ushort productID, short version)
            {
                // remember the settings
                this.deviceRid = rid;
                this.fp = fp;
                this.path = path;
                this.devicename = name;
                this.serial = serial;
                this.vendorID = vendorID;
                this.productID = productID;
                this.version = version;

                IntPtr ppdata;
                if (HIDImports.HidD_GetPreparsedData(fp, out ppdata)) {
                    // get the device caps
                    HIDImports.HidP_GetCaps(ppdata, ref caps);
                    // done with the preparsed data
                    HIDImports.HidD_FreePreparsedData(ppdata);
                }

                byte[] answer = null;

                //Send HandShake
                SendCommand(HIDCommonReportType.RT_HANDSHAKE);
                answer = ReadUSB().Skip(hidCommandPrefixSize).ToArray();
                string handShake = Encoding.UTF8.GetString(answer).TrimEnd('\0');
                var splits = handShake.Split('|');
                if (splits.Length > 1) {
                    this.name = splits[0];
                    handShake = splits[1];
                } else {
                    Log.Warning($"Old Dude's Cab handshake, you should update your firmware");
                }
                Log.Write($"{this.name} says : {handShake}");

                //Ask for Card Infos
                SendCommand(HIDCommonReportType.RT_VERSION);
                answer = ReadUSB().Skip(hidCommandPrefixSize).ToArray();
                Log.Write($"DudesCab Controller Informations : Device [{this.devicename},RID:{this.deviceRid}] Name [{this.name}], v{answer[0]}.{answer[1]}.{answer[2]}, unit #{answer[3]}, Max extensions {answer[4]}");
                unitNo = answer[3];
                MaxExtensions = answer[4];
                firmwareVersion = new Version(answer[0], answer[1], answer[2]);

                //Force Logging from the card if there is a DudesCab Instrumentation
                if (Log.HasInstrumentations("DudesCabLog")) {
                    Log.Write($"Forcing DudesCab LogLevel to DEBUG due to Instrumentation activation");
                    SendCommand(HIDCommonReportType.RT_FORCELOGLEVEL, new byte[] { (byte)DudesCabLogLevel.Debug });
                }
            }

            private System.Threading.NativeOverlapped ov;
            public byte[] ReadUSB()
            {
                byte[] incomingData = new byte[0];
                byte receivedCommand = 0;
                DateTime startRead = DateTime.Now;
                while (DateTime.Now - startRead < TimeSpan.FromSeconds(10)) {
                    uint rptLen = caps.InputReportByteLength;
                    byte[] buf = new byte[rptLen];
                    buf[0] = 0x00;
                    uint actual;
                    if (HIDImports.ReadFile(fp, buf, rptLen, out actual, ref ov) == 0) {
                        // if the error is 6 ("invalid handle"), try re-opening the device
                        if (TryReopenHandle())
                            continue;

                        Log.Error("DudesCab Controller USB error reading from device: " + GetLastWin32ErrMsg());
                        return null;
                    } else if (actual != rptLen) {
                        Log.Error("DudesCab Controller USB error reading from device: not all bytes received");
                        return null;
                    } else {
                        byte rid = buf[0];

                        if (rid == (byte)deviceRid) {
                            byte numpart = buf[2];
                            if (numpart == 0)
                                receivedCommand = buf[1];
                            byte nbparts = buf[3];
                            byte received = buf[4];

                            if (receivedCommand == buf[1]) {
                                if (numpart == 0) {
                                    incomingData = buf.Take(received + hidCommandPrefixSize).ToArray();
                                } else {
                                    incomingData = incomingData.Concat(buf.Skip(hidCommandPrefixSize).Take(received).ToArray());
                                }
                                if (numpart == nbparts - 1) {
                                    return incomingData;
                                }
                            }
                        }
                    }
                }

                // don't retry more than a few times
                return null;
            }

            public bool WriteUSB(byte[] buf)
            {
                DateTime startWrite = DateTime.Now;
                while (DateTime.Now - startWrite < TimeSpan.FromSeconds(10)) {
                    UInt32 actual;
                    if (HIDImports.WriteFile(fp, buf, caps.OutputReportByteLength, out actual, ref ov) == 0) {
                        // try re-opening the handle, if it's an "invalid handle" error
                        if (TryReopenHandle())
                            continue;

                        Log.Error("DudesCab Controller USB error sending request to device: " + GetLastWin32ErrMsg());
                        return false;
                    } else if (actual != caps.OutputReportByteLength) {
                        Log.Error("DudesCab Controller USB error sending request: not all bytes sent");
                        return false;
                    } else {
                        return true;
                    }
                }

                // maximum retries exceeded - return failure
                return false;
            }

            ~Device()
            {
                if (fp.ToInt32() != 0 && fp.ToInt32() != -1) {
                    HIDImports.CloseHandle(fp);
                    fp = IntPtr.Zero;
                }
            }

            public void ReadPwmOutputsConfig()
            {
                //// presume we have the standard DudesCab complement of 128 outputs
                this._NumOutputs = 128;

                byte[] answer = null;
                //Ask for Pwm Configuration
                SendCommand(HIDReportType.RT_PWM_GETINFOS);
                answer = ReadUSB().ToArray();
                var answersize = answer[hidCommandPrefixSize - 1];
                answer = answer.Skip(hidCommandPrefixSize).ToArray();
                PwmMaxOutputsPerExtension = answer[0];
                PwmExtensionsMask = answer[1];
                Log.Write($"    Pwm Informations : Max outputs per extensions {PwmMaxOutputsPerExtension}, Extension Mask 0x{(int)PwmExtensionsMask:X2}");
                if (answersize > 2) {
                    //Get Outputmasks and process remaps
                    var nbMasks = answer[2];
                    var maskSize = answer[3];
                    var masks = new ushort[nbMasks];
                    for (int mask = 0; mask < nbMasks; mask++) {
                        masks[mask] = (ushort)(answer[4 + (2 * mask)] + (answer[4 + (2 * mask) + 1] << 8));
                    }
                    var curMask = 0;

                    this._NumOutputs = 0;
                    for (var ext = 0; ext < MaxExtensions; ext++) {
                        if ((PwmExtensionsMask & (byte)(1 << ext)) != 0) {
                            for (var output = 0; output < PwmMaxOutputsPerExtension; output++) {
                                if ((masks[curMask] & (ushort)(1 << output)) != 0) {
                                    this._NumOutputs = Math.Max(this._NumOutputs, (ext * PwmMaxOutputsPerExtension) + output + 1);
                                }
                            }
                            curMask++;
                        }
                    }
                    Log.Write($"    Output configuration received, highest configured output is #{this._NumOutputs}");
                } else {
                    Log.Warning($"No output configuration received, {this._NumOutputs} will be used, you should update your DudesCab firmware");
                }
            }

            private IntPtr OpenFile()
            {
                return HIDImports.CreateFile(
                    path, HIDImports.GENERIC_READ_WRITE, HIDImports.SHARE_READ_WRITE,
                    IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            }

            private bool TryReopenHandle()
            {
                // if the last error is 6 ("invalid handle"), try re-opening it
                if (Marshal.GetLastWin32Error() == 6) {
                    // try opening a new handle on the device path
                    Log.Error($"DudesCab Controller ({name}): invalid handle on read; trying to reopen handle");
                    IntPtr fp2 = OpenFile();

                    // if that succeeded, replace the old handle with the new one and retry the read
                    if (fp2 != null) {
                        // replace the handle
                        fp = fp2;

                        // tell the caller to try again
                        return true;
                    }
                }

                // we didn't successfully reopen the handle
                return false;
            }

            public String GetLastWin32ErrMsg()
            {
                int errno = Marshal.GetLastWin32Error();
                return String.Format("{0} (Win32 error {1})",
                                     new System.ComponentModel.Win32Exception(errno).Message, errno);
            }

            public void AllOff()
            {
                SendCommand(HIDReportType.RT_PWM_ALLOFF);
            }

            internal void SendCommand(HIDCommonReportType command, byte[] paramaters = null)
            {
                SendCommand(deviceRid, (byte)command, paramaters);
            }
            internal void SendCommand(HIDReportType command, byte[] paramaters = null)
            {
                SendCommand(deviceRid, (byte)command, paramaters);
            }

            internal void SendCommand(HIDReportTypeMx command, byte[] paramaters = null)
            {
                if (SupportMx())
                    SendCommand(deviceRid, (byte)command, paramaters);
            }

            private void SendCommand(RIDType rid, byte command, byte[] parameters = null)
            {
                byte[] data = new byte[0];
                if (parameters != null) {
                    data = data.ToList().Concat(parameters).ToArray();
                }

                if (rid == RIDType.RIDOutputs) {
                    Log.Instrumentation("DudesCab", $"DudesCab SendCommand: {command}, [{string.Join(",", data.ToArray())}]");
                } else {
                    Log.Instrumentation("DudesCab,Mx", $"DudesCab SendCommand: {command}, [{string.Join(",", data.ToArray())}]");
                }

                //Compute how many parts will be needed to send the command, based on the provided DudesCab caps.
                byte bufferOffset = 5;
                byte partSize = (byte)(caps.OutputReportByteLength - bufferOffset);
                byte nbParts = (byte)((data.Length / partSize) + 1);

                //Write the command to USB , splitted into chuncks of caps.OutputReportByteLength
                for (byte i = 0; i < nbParts; i++) {
                    byte[] sendData = new byte[bufferOffset];
                    sendData[0] = (byte)rid;
                    sendData[1] = command;
                    sendData[2] = i;
                    sendData[3] = nbParts;
                    byte toSend = (byte)(data.Length > partSize ? partSize : data.Length);
                    sendData[4] = toSend;
                    var dataPart = data.Take(toSend).ToArray();
                    sendData = sendData.Concat(dataPart).ToArray();
                    data = data.Skip(toSend).ToArray();
                    WriteUSB(sendData);
                }
            }

            public IntPtr fp;
            public string path;
            public string devicename;
            public string name;
            public string serial;
            public ushort vendorID;
            public ushort productID;
            public short version;
            public short unitNo;
            internal HIDImports.HIDP_CAPS caps = new HIDImports.HIDP_CAPS();
            public RIDType  deviceRid = RIDType.RIDOutputs;
            public int MaxExtensions = 0;
            public int PwmMaxOutputsPerExtension = 0;
            public byte PwmExtensionsMask = 0;
            public Version firmwareVersion = new Version(0,0,0);

            private readonly Version minimalMxVersion = new Version(1,9,0);

            internal bool SupportMx()
            {
                return firmwareVersion >= minimalMxVersion;
            }
        }

        #endregion


        #region Device enumeration

        /// <summary>
        /// Get the list of all DudesCab devices discovered in the system from the Windows USB device scan.
        /// </summary>
        public static List<Device> AllDevices()
        {
            return Devices;
        }

        // Search the Windows USB HID device set for DudesCab controllers
        private static List<Device> FindDevices()
        {
            // set up an empty return list
            List<Device> dudedevices = new List<Device>();
            List<Device> devices = new List<Device>();

            // get the list of devices matching the HID class GUID
            Guid guid;
            HIDImports.HidD_GetHidGuid(out guid);
            IntPtr hdev = HIDImports.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, HIDImports.DIGCF_DEVICEINTERFACE);

            // set up the attribute structure buffer
            HIDImports.SP_DEVICE_INTERFACE_DATA diData = new HIDImports.SP_DEVICE_INTERFACE_DATA();
            diData.cbSize = Marshal.SizeOf(diData);

            // read the devices in our list
            for (uint i = 0;
                 HIDImports.SetupDiEnumDeviceInterfaces(hdev, IntPtr.Zero, ref guid, i, ref diData);
                 ++i) {
                // get the size of the detail data structure
                UInt32 size = 0;
                HIDImports.SetupDiGetDeviceInterfaceDetail(hdev, ref diData, IntPtr.Zero, 0, out size, IntPtr.Zero);

                // now actually read the detail data structure
                HIDImports.SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new HIDImports.SP_DEVICE_INTERFACE_DETAIL_DATA();
                diDetail.cbSize = (IntPtr.Size == 8) ? (uint)8 : (uint)5;
                if (HIDImports.SetupDiGetDeviceInterfaceDetail(hdev, ref diData, ref diDetail, size, out size, IntPtr.Zero)) {
                    // create a file handle to access the device
                    IntPtr fp = HIDImports.CreateFile(
                        diDetail.DevicePath, HIDImports.GENERIC_READ_WRITE, HIDImports.SHARE_READ_WRITE,
                        IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                    // read the attributes
                    HIDImports.HIDD_ATTRIBUTES attrs = new HIDImports.HIDD_ATTRIBUTES();
                    attrs.Size = Marshal.SizeOf(attrs);
                    if (HIDImports.HidD_GetAttributes(fp, ref attrs)) {
                        // read the product name string
                        String name = "<not available>";
                        byte[] nameBuf = new byte[128];
                        if (HIDImports.HidD_GetProductString(fp, nameBuf, 128))
                            name = System.Text.Encoding.Unicode.GetString(nameBuf).TrimEnd('\0');

                        // read the product serial number
                        String serial = "<not available>";
                        byte[] serialBuf = new byte[128];
                        if (HIDImports.HidD_GetSerialNumberString(fp, serialBuf, 128))
                            serial = System.Text.Encoding.Unicode.GetString(serialBuf).TrimEnd('\0');

                        // If the vendor and product ID match a DudesCab and
                        // product name matches "DudesCab Outputs" or "DudesCab MX Outputs", which are the specific Hid entrypoints for DirectOutput
                        // then it's a DudesCab or DudesCabMX controller.
                        bool isDude = (attrs.VendorID == DudesCab.VendorID && attrs.ProductID == DudesCab.ProductID);
                        if (isDude) {
                            var deviceRid = Device.RIDType.None;
                            if (string.Compare(name, "DudesCab Outputs", StringComparison.InvariantCultureIgnoreCase) == 0)
                                deviceRid = Device.RIDType.RIDOutputs;
                            else if (string.Compare(name, "DudesCab Outputs MX", StringComparison.InvariantCultureIgnoreCase) == 0)
                                deviceRid = Device.RIDType.RIDOutputsMx;
                            if (deviceRid != Device.RIDType.None && 
                                !dudedevices.Any(D =>
                                    D.deviceRid == deviceRid &&
                                    string.Compare(D.serial, serial, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                                    string.Compare(D.devicename, name, StringComparison.InvariantCultureIgnoreCase) == 0
                                    )) {
                                var newDevice = new Device(deviceRid, fp, diDetail.DevicePath, name, serial, attrs.VendorID, attrs.ProductID, attrs.VersionNumber);
                                dudedevices.Add(newDevice);
                                if (deviceRid == Device.RIDType.RIDOutputs) {
                                    newDevice.ReadPwmOutputsConfig();
                                    devices.Add(newDevice);
                                } else if (deviceRid == Device.RIDType.RIDOutputsMx) {
                                    if (newDevice.SupportMx()) {
                                        // Add a new UMXDevice
                                        var dudeUMX = new UMXDudesCabDevice();
                                        dudeUMX.Device = newDevice;
                                        UMXControllerAutoConfigurator.AddUMXDevice(dudeUMX);
                                    }
                                }
                                // the device list object owns the handle now
                                fp = System.IntPtr.Zero;
                            }
                        }
                    }

                    // done with the file handle
                    if (fp.ToInt32() != 0 && fp.ToInt32() != -1)
                        HIDImports.CloseHandle(fp);
                }
            }

            return devices;
        }

        #endregion

            // list of DudesCab controller devices discovered in Windows USB HID scan
        private static List<Device> Devices;

        // my device
        private Device Dev;

        #region Constructor

        /// <summary>
        /// Initializes the DudesCab class.
        /// </summary>
        static DudesCab()
        {
            // scan the Windows USB HID device set for installed DudesCab Controller devices,
            // and save the list statically in the class
            Devices = FindDevices();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DudesCab"/> class with no unit number set.
        /// The unit number must be set before use (via the Number property).
        /// </summary>
        public DudesCab()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DudesCab"/> class with a given unit number.
        /// </summary>
        /// <param name="Number">The number of the DudesCab controller (1-5).</param>
        public DudesCab(int Number)
        {
            this.Number = Number;
        }

        #endregion
    }
}
