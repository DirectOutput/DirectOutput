using DirectOutput.Cab.Toys.LWEquivalent;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DirectOutput.Cab.Out.AdressableLedStrip
{
    public class UMXController : OutputControllerCompleteBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UMXController"/> class with a given unit number.
        /// </summary>
        /// <param name="Number">The number of the UMX controller.</param>
        public UMXController(int Number)
        {
            this.Number = Number;
        }


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
                lock (NumberUpdateLocker) {
                    // if the unit number changed, update it and attach to the new unit
                    if (_Number != value) {
                        // if we used a default name for the old unit number, change to the default
                        // name for the new unit number
                        if (Name.IsNullOrWhiteSpace() || Name == "UMX Controller {0:00}".Build(_Number)) {
                            Name = "UMX Controller {0:00}".Build(value);
                        }

                        // remember the new unit number
                        _Number = value;

                        // attach to the new device record for this unit number, updating the output list to match
                        this.Dev = UMXControllerAutoConfigurator.AllDevices().First(D => D.UnitNo() == Number);
                        this.OldOutputValues = Enumerable.Repeat((byte)255, this.Dev.NumOutputs()).ToArray();
                    }
                }
            }
        }

        #endregion

        byte[] OldOutputValues;

        protected override void ConnectToController()
        {
            throw new NotImplementedException();
        }

        protected override void DisconnectFromController()
        {
            throw new NotImplementedException();
        }

        protected override int GetNumberOfConfiguredOutputs()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateOutputs(byte[] OutputValues)
        {
            throw new NotImplementedException();
        }

        protected override bool VerifySettings()
        {
            throw new NotImplementedException();
        }

        #region Device
        public class Device
        {
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

            public Device(string path, string name, string serial, string comPort, ushort vendorID, ushort productID, short version)
            {
                // remember the settings
                this.path = path;
                this.name = name;
                this.serial = serial;
                this.comPort = comPort;
                this.vendorID = vendorID;
                this.productID = productID;
                this.version = version;

                //byte[] answer = null;

                ////Send HandShake
                //SendCommand(HIDReportType.RT_HANDSHAKE);
                //answer = ReadUSB().Skip(hidCommandPrefixSize).ToArray();
                //string handShake = Encoding.UTF8.GetString(answer).TrimEnd('\0');
                //Log.Write($"{name} says : {handShake}");

                ////Ask for Card Infos
                //SendCommand(HIDReportType.RT_INFOS);
                //answer = ReadUSB().Skip(hidCommandPrefixSize).ToArray();
                //Log.Write($"DudesCab Controller Informations : v{answer[0]}.{answer[1]}.{answer[2]}, unit #{answer[3]}, Max extensions {answer[4]}");
                //unitNo = answer[3];
                //MaxExtensions = answer[4];

                ////Ask for Pwm Configuration
                //SendCommand(HIDReportType.RT_PWM_GETEXTENSIONSINFOS);
                //answer = ReadUSB().Skip(hidCommandPrefixSize).ToArray();
                //PwmMaxOutputsPerExtension = answer[0];
                //PwmExtensionsMask = answer[1];
                //Log.Write($"    Pwm Informations : Max outputs per extensions {PwmMaxOutputsPerExtension}, Extesnion Mask {(int)PwmExtensionsMask:X2}");

                ////// presume we have the standard DudesCab complement of 128 outputs
                //this.numOutputs = 128;
            }

            ~Device()
            {
                //if (fp.ToInt32() != 0 && fp.ToInt32() != -1) {
                //    HIDImports.CloseHandle(fp);
                //    fp = IntPtr.Zero;
                //}
            }

            //public void AllOff()
            //{
            //    SendCommand(HIDReportType.RT_PWM_ALLOFF);
            //}

            //internal void SendCommand(HIDReportType command, byte[] parameters = null)
            //{
            //    byte[] data = new byte[0];
            //    if (parameters != null) {
            //        data = data.ToList().Concat(parameters).ToArray();
            //    }

            //    //Compute how many parts will be needed to send the command, based on the provided DudesCab caps.
            //    byte bufferOffset = 5;
            //    byte partSize = (byte)(caps.OutputReportByteLength - bufferOffset);
            //    byte nbParts = (byte)((data.Length / partSize) + 1);

            //    //Write the command to USB , splitted into chuncks of caps.OutputReportByteLength
            //    for (byte i = 0; i < nbParts; i++) {
            //        byte[] sendData = new byte[bufferOffset];
            //        sendData[0] = RID_OUTPUTS;
            //        sendData[1] = (byte)command;
            //        sendData[2] = i;
            //        sendData[3] = nbParts;
            //        byte toSend = (byte)(data.Length > partSize ? partSize : data.Length);
            //        sendData[4] = toSend;
            //        var dataPart = data.Take(toSend).ToArray();
            //        sendData = sendData.Concat(dataPart).ToArray();
            //        data = data.Skip(toSend).ToArray();
            //        WriteUSB(sendData);
            //    }
            //}

            public string path;
            public string name;
            public string serial;
            public string comPort;
            public ushort vendorID;
            public ushort productID;
            public short version;
            public short unitNo;
            public int numOutputs;
            public int MaxExtensions = 0;
            public int PwmMaxOutputsPerExtension = 0;
            public byte PwmExtensionsMask = 0;
        }

        // my device
        private Device Dev;
        #endregion
    }

    public class UMXControllerAutoConfigurator : IAutoConfigOutputController
    {
        #region Device enumeration

        public static readonly List<Tuple<string, string>> compatibleSerialCards = new List<Tuple<string, string>>() {
            new Tuple<string, string> ("1A86", "7523"),   //D1 Mini
            new Tuple<string, string> ("10C4", "EA60"),   //D1 Mini Pro
            new Tuple<string, string> ("303A", "80C2"),   //S2 Mini
            new Tuple<string, string> ("16C0", "0483"),   //Teensy 4.0
        };

        static UMXControllerAutoConfigurator()
        {
            Devices = FindDevices();
        }

        private static List<UMXController.Device> Devices;

        /// <summary>
        /// Get the list of all DudesCab devices discovered in the system from the Windows USB device scan.
        /// </summary>
        public static List<UMXController.Device> AllDevices()
        {
            return Devices;
        }


        static string RetrieveComportName(string deviceID)
        {
            RegistryKey rk1 = Registry.LocalMachine;
            RegistryKey rk2 = rk1.OpenSubKey($"SYSTEM\\CurrentControlSet\\Enum\\{deviceID}\\Device Parameters");
            if ( rk2 != null ) {
                return rk2.GetValue("PortName") as string;
            }

            return string.Empty;
        }

        // Search the Windows USB HID device set for DudesCab controllers
        private static List<UMXController.Device> FindDevices()
        {
            // set up an empty return list
            List<UMXController.Device> devices = new List<UMXController.Device>();

            // get the list of devices matching the HID class GUID
            //            Guid guid = new Guid("{88BAE032-5A81-49f0-BC3D-A4FF138216D6}"); ;
        Guid guid = new Guid("{86E0D1E0-8089-11D0-9CE4-08003E301F73}");
        //public static readonly Guid GuidForPortsClass = new Guid("{4D36E978-E325-11CE-BFC1-08002BE10318}");
        //public static readonly Guid GuidForUsbHub = new Guid("{F18A0E88-C30C-11D0-8815-00A0C906BED8}");
        //            HIDImports.HidD_GetHidGuid(out guid);
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

                    string devicePath = diDetail.DevicePath.ToUpper();

                    //Check Compatible devices 
                    if (devicePath.Contains("VID_") && devicePath.Contains("PID_")) {
                        var vendorID = devicePath.Substring(devicePath.IndexOf("VID_") + "VID_".Length, 4);
                        var productID = devicePath.Substring(devicePath.IndexOf("PID_") + "PID_".Length, 4);

                        if (compatibleSerialCards.Any(C=>C.Item1.Equals(vendorID, StringComparison.InvariantCultureIgnoreCase) &&
                                C.Item2.Equals(productID, StringComparison.InvariantCultureIgnoreCase))) {
                            //Retrieve com port

                        }
                    }

//                    // read the attributes
//                    HIDImports.HIDD_ATTRIBUTES attrs = new HIDImports.HIDD_ATTRIBUTES();
//                    attrs.Size = Marshal.SizeOf(attrs);
//                    if (HIDImports.HidD_GetAttributes(fp, ref attrs)) {
//                        // presume this is a UMX Controller, then look for reasons it's not
//                        bool ok = true;

//                        // read the product name string
//                        String name = "<not available>";
//                        byte[] nameBuf = new byte[128];
//                        if (HIDImports.HidD_GetProductString(fp, nameBuf, 128))
//                            name = System.Text.Encoding.Unicode.GetString(nameBuf).TrimEnd('\0');

//                        // read the product serial number
//                        String serial = "<not available>";
//                        byte[] serialBuf = new byte[128];
//                        if (HIDImports.HidD_GetSerialNumberString(fp, serialBuf, 128))
//                            serial = System.Text.Encoding.Unicode.GetString(serialBuf).TrimEnd('\0');

//                        // If the vendor and product ID match a DudesCab and
//                        // product name matches "DudesCab Outputs", which id the specific Hid entrypoint for DirectOutput
//                        // and does not have the same serial number as an already enumerated DudesCab (dual cards in enumeration happened)
//                        // then it's a DudesCab controller.
//                        bool isUMX = compatibleCards.Any(C => C.Item1 == attrs.VendorID && C.Item2 == attrs.ProductID);
//                        ok &= isUMX && !devices.Any(D => string.Compare(D.serial, serial, StringComparison.InvariantCultureIgnoreCase) == 0);

//                        // If we passed all tests, this is the output controller interface for
//                        // a DudesCab controller device, so add the device to our list.
//                        if (ok) {
//                            //Check handshake to see if it's the correct UMX COntroller firmware
//                            SerialPort serialPort = new SerialPort();
//                            serialPort.BaudRate = 115200;
//                            serialPort.Parity = Parity.None;
//                            serialPort.DataBits = 8;
//                            serialPort.StopBits = StopBits.One;
//                            serialPort.ReadTimeout = 1000;
//                            serialPort.WriteTimeout = 1000;
//                            serialPort.DtrEnable = false;

//                            serialPort.PortName = RetrieveComportName(diDetail.DevicePath);

//                            // add the device to our list
////                            devices.Add(new UMXController.Device(fp, diDetail.DevicePath, name, serial, attrs.VendorID, attrs.ProductID, attrs.VersionNumber));

//                            // the device list object owns the handle now
//                            fp = System.IntPtr.Zero;
//                        }
//                    }

                    //// done with the file handle
                    //if (fp.ToInt32() != 0 && fp.ToInt32() != -1)
                    //    HIDImports.CloseHandle(fp);
                }
            }

            return devices;
        }

        #endregion

        /// <summary>
        /// This method detects and configures DudesCab output controllers automatically.
        /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoConfig(Cabinet Cabinet)
        {
            const int UnitBias = 89;
            List<int> Preconfigured = new List<int>(Cabinet.OutputControllers.Where(OC => OC is UMXController).Select(C => ((UMXController)C).Number));
            IEnumerable<int> Numbers = UMXControllerAutoConfigurator.AllDevices().Select(d => d.UnitNo());
            foreach (int N in Numbers) {
                if (!Preconfigured.Contains(N)) {
                    UMXController umxC = new UMXController(N);
                    if (!Cabinet.OutputControllers.Contains(umxC.Name)) {
                        Cabinet.OutputControllers.Add(umxC);
                        Log.Write("Detected and added DudesCab Controller Nr. {0} with name {1}".Build(umxC.Number, umxC.Name));

                        if (!Cabinet.Toys.Any(T => T is LedWizEquivalent && ((LedWizEquivalent)T).LedWizNumber == umxC.Number + UnitBias)) {
                            LedWizEquivalent LWE = new LedWizEquivalent();
                            LWE.LedWizNumber = umxC.Number + UnitBias;
                            LWE.Name = "{0} Equivalent".Build(umxC.Name);

                            //for (int i = 1; i <= umxC.NumberOfOutputs; i++) {
                            //    LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(umxC.Name, i), LedWizEquivalentOutputNumber = i };
                            //    LWE.Outputs.Add(LWEO);
                            //}

                            //if (!Cabinet.Toys.Contains(LWE.Name)) {
                            //    Cabinet.Toys.Add(LWE);
                            //    Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for DudesCab Controller Nr. {2}".Build(
                            //        LWE.LedWizNumber, LWE.Name, umxC.Number) + ", {0}".Build(umxC.NumberOfOutputs));
                            //}
                        }
                    }
                }
            }
        }
    }

}
