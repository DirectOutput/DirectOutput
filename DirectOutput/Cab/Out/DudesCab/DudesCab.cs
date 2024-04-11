﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DirectOutput.Cab.Out.DudesCab
{
    /// <summary>
    /// The <a href="https://shop.arnoz.com/en/">Dude's Cab Controller</a> is a all-in-one virtual pinball controller
    /// 
    /// It's based on the RP2040 processor, provide 32 inputs, accelerometer, plunger & Dof support.
    /// The Dof support provides 128 PWM outputs through up to 8 extension boards.
    /// Maximum intensity, Flipper logic, Chime logic & Gamma Correction are supported for all outputs
    /// Each outputs can also have its intensity lowered independently from the Dof values
    /// As for the Pinscape controller, the communication is fully suported through Hid In/Out protocol with multipart messages support
    /// The Dude's Cab controllers are registered in the Dof config tool from unit #90 to 94
    /// 
    /// </summary>
    public class DudesCab : OutputControllerFlexCompleteBase
    {
        public static readonly ushort VendorID = 0x2E8A;
        public static readonly ushort ProductID = 0x106F;

        public static readonly bool DebugCommunication = false;

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

        private bool firstInit = true;

        /// <summary>
        /// Send updated outputs to the physical device.
        /// </summary>
        protected override void UpdateOutputs(byte[] NewOutputValues)
        {
            byte extensionChangeMask = 0;
            ushort[] outputsChangeMask = new ushort[Dev.MaxExtensions];
            List<byte> changedOutputs = new List<byte>();

            List<byte> outputBuffer = new List<byte>();
            outputBuffer.Add(0); //extension mask

            for (int numExt = 0; numExt < Dev.MaxExtensions; numExt++) {
                outputsChangeMask[numExt] = 0;
                int maskOffset = outputBuffer.Count;
                outputBuffer.Add(0);//Low bits of output mask
                outputBuffer.Add(0);//High bits of output mask

                for (int numOuput = 0; numOuput < Dev.PwmMaxOutputsPerExtension; numOuput++) {
                    int outputOffset = numExt * Dev.PwmMaxOutputsPerExtension + numOuput;
                    if (NewOutputValues[outputOffset] != OldOutputValues[outputOffset]) {
                        //If the DudesCab has a configured extension for this output
                        if ((Dev.PwmExtensionsMask & (byte)(1 << numExt)) != 0) {
                            extensionChangeMask |= (byte)(1 << numExt);
                            outputsChangeMask[numExt] |= (ushort)(1 << numOuput);
                            outputBuffer.Add(NewOutputValues[outputOffset]);
                            if (DebugCommunication)
                                Log.Debug($"Output {outputOffset + 1} ({OldOutputValues[outputOffset]}=>{NewOutputValues[outputOffset]}) is sent to an extension ({numExt + 1})");
                        } else {
                            if (!firstInit) {
                                Log.Warning($"Output {outputOffset + 1} ({OldOutputValues[outputOffset]}=>{NewOutputValues[outputOffset]}) is sent to an extension ({numExt + 1}) which wasn't configured on the DudesCab Controller, Please check your Controller or Dof settings");
                            }
                        }
                    }
                }

                if (outputsChangeMask[numExt] != 0) {
                    if (DebugCommunication)
                        Log.Debug($"Extenstion {numExt + 1} OutputsMask {(int)outputsChangeMask[numExt]:X4}");
                    outputBuffer[maskOffset] = (byte)(outputsChangeMask[numExt] & 0xFF);
                    outputBuffer[maskOffset + 1] = (byte)((outputsChangeMask[numExt] >> 8) & 0xFF);
                } else {
                    outputBuffer.RemoveRange(outputBuffer.Count - 2, 2);
                }
            }

            if (extensionChangeMask != 0) {
                outputBuffer[0] = extensionChangeMask;
                if (DebugCommunication)
                    Log.Debug($"ExtenstionMask {outputBuffer[0]:X2}");
                Dev.SendCommand(Device.HIDReportType.RT_PWM_OUTPUTS, outputBuffer.ToArray());
            }
            firstInit = false;
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
            if (InUseState == InUseStates.Running)
                Dev.AllOff();
        }

        #endregion

        #region USB Communications

        public class Device
        {
            static byte RID_OUTPUTS = 3;
            internal enum HIDReportType
            {
                RT_HANDSHAKE = 1,
                RT_INFOS,        

                //PWM
                RT_PWM_GETEXTENSIONSINFOS, 
                RT_PWM_ALLOFF, 
                RT_PWM_OUTPUTS, 

                //MX (not implemented yet)
                RT_MX_GETEXTENSIONSINFOS, 
                RT_MX_ALLOFF, 
                RT_MX_OUTPUTS,

                RT_MAX
            };

            public override string ToString()
            {
                return name + " (unit " + UnitNo() + ")";
            }

            public int UnitNo()
            {
                return unitNo;
            }

            public int NumOutputs()
            {
                return numOutputs;
            }

            static readonly int hidCommandPrefixSize = 5;

            public Device(IntPtr fp, string path, string name, string serial, ushort vendorID, ushort productID, short version)
            {
                // remember the settings
                this.fp = fp;
                this.path = path;
                this.name = name;
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
                SendCommand(HIDReportType.RT_HANDSHAKE);
                answer = ReadUSB().Skip(hidCommandPrefixSize).ToArray();
                string handShake = Encoding.UTF8.GetString(answer).TrimEnd('\0');
                Log.Write($"{name} says : {handShake}");

                //Ask for Card Infos
                SendCommand(HIDReportType.RT_INFOS);
                answer = ReadUSB().Skip(hidCommandPrefixSize).ToArray();
                Log.Write($"DudesCab Controller Informations : v{answer[0]}.{answer[1]}.{answer[2]}, unit #{answer[3]}, Max extensions {answer[4]}");
                unitNo = answer[3];
                MaxExtensions = answer[4];

                //Ask for Pwm Configuration
                SendCommand(HIDReportType.RT_PWM_GETEXTENSIONSINFOS);
                answer = ReadUSB().Skip(hidCommandPrefixSize).ToArray();
                PwmMaxOutputsPerExtension = answer[0];
                PwmExtensionsMask = answer[1];
                Log.Write($"    Pwm Informations : Max outputs per extensions {PwmMaxOutputsPerExtension}, Extesnion Mask {(int)PwmExtensionsMask:X2}");

                //// presume we have the standard DudesCab complement of 128 outputs
                this.numOutputs = 128;
            }

            private System.Threading.NativeOverlapped ov;
            public byte[] ReadUSB()
            {
                for (int tries = 0; tries < 3; ++tries) {
                    uint rptLen = caps.InputReportByteLength;
                    byte[] buf = new byte[rptLen];
                    buf[0] = 0x00;
                    uint actual;
                    if (HIDImports.ReadFile(fp, buf, rptLen, out actual, ref ov) == 0) {
                        // if the error is 6 ("invalid handle"), try re-opening the device
                        if (TryReopenHandle())
                            continue;

                        Log.Write("DudesCab Controller USB error reading from device: " + GetLastWin32ErrMsg());
                        return null;
                    } else if (actual != rptLen) {
                        Log.Write("DudesCab Controller USB error reading from device: not all bytes received");
                        return null;
                    } else
                        return buf;
                }

                // don't retry more than a few times
                return null;
            }

            public bool WriteUSB(byte[] buf)
            {
                for (int tries = 0; tries < 3; ++tries) {
                    UInt32 actual;
                    if (HIDImports.WriteFile(fp, buf, caps.OutputReportByteLength, out actual, ref ov) == 0) {
                        // try re-opening the handle, if it's an "invalid handle" error
                        if (TryReopenHandle())
                            continue;

                        Log.Write("DudesCab Controller USB error sending request to device: " + GetLastWin32ErrMsg());
                        return false;
                    } else if (actual != caps.OutputReportByteLength) {
                        Log.Write("DudesCab Controller USB error sending request: not all bytes sent");
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
                    Log.Write("DudesCab Controller: invalid handle on read; trying to reopen handle");
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

            internal void SendCommand(HIDReportType command, byte[] parameters = null)
            {
                byte[] data = new byte[0];
                if (parameters != null) {
                    data = data.ToList().Concat(parameters).ToArray();
                }

                if (DebugCommunication)
                    Log.Write($"DudesCab SendCommand: {command}, [{string.Join(",", data.ToArray())}]");

                //Compute how many parts will be needed to send the command, based on the provided DudesCab caps.
                byte bufferOffset = 5;
                byte partSize = (byte)(caps.OutputReportByteLength - bufferOffset);
                byte nbParts = (byte)((data.Length / partSize) + 1);

                //Write the command to USB , splitted into chuncks of caps.OutputReportByteLength
                for (byte i = 0; i < nbParts; i++) {
                    byte[] sendData = new byte[bufferOffset];
                    sendData[0] = RID_OUTPUTS;
                    sendData[1] = (byte)command;
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
            public string name;
            public string serial;
            public ushort vendorID;
            public ushort productID;
            public short version;
            public short unitNo;
            public int numOutputs;
            internal HIDImports.HIDP_CAPS caps = new HIDImports.HIDP_CAPS();
            public int MaxExtensions = 0;
            public int PwmMaxOutputsPerExtension = 0;
            public byte PwmExtensionsMask = 0;
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
                        // presume this is a DudesCab Controller, then look for reasons it's not
                        bool ok = true;

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
                        // product name matches "DudesCab Outputs", which id the specific Hid entrypoint for DirectOutput
                        // and does not have the same serial number as an already enumerated DudesCab (dual cards in enumeration happened)
                        // then it's a DudesCab controller.
                        bool isDude = (attrs.VendorID == DudesCab.VendorID && attrs.ProductID == DudesCab.ProductID);
                        ok &= (isDude && Regex.IsMatch(name, @"DudesCab Outputs"));
                        ok &= !devices.Any(D => string.Compare(D.serial, serial, StringComparison.InvariantCultureIgnoreCase) == 0);

                        // If we passed all tests, this is the output controller interface for
                        // a DudesCab controller device, so add the device to our list.
                        if (ok) {
                            // add the device to our list
                            devices.Add(new Device(fp, diDetail.DevicePath, name, serial, attrs.VendorID, attrs.ProductID, attrs.VersionNumber));

                            // the device list object owns the handle now
                            fp = System.IntPtr.Zero;
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
