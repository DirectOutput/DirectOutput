using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutput.Cab.Out.AdressableLedStrip
{
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

        private static List<UMXDevice> Devices;

        public static void AddUMXDevice(UMXDevice dev)
        {
            if (!Devices.Contains(dev)) {
                dev.unitNo = (short)(Devices.Count + 1);
                Devices.Add(dev);
            }
        }

        /// <summary>
        /// Get the list of all DudesCab devices discovered in the system from the Windows USB device scan.
        /// </summary>
        public static List<UMXDevice> AllDevices()
        {
            return Devices;
        }


        static string RetrieveComportName(string deviceID)
        {
            RegistryKey rk1 = Registry.LocalMachine;
            RegistryKey rk2 = rk1.OpenSubKey($"SYSTEM\\CurrentControlSet\\Enum\\{deviceID}\\Device Parameters");
            if (rk2 != null) {
                return rk2.GetValue("PortName") as string;
            }

            return string.Empty;
        }

        // Search the Windows USB HID device set for DudesCab controllers
        private static List<UMXDevice> FindDevices()
        {
            // set up an empty return list
            List<UMXDevice> devices = new List<UMXDevice>();

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

                        if (compatibleSerialCards.Any(C => C.Item1.Equals(vendorID, StringComparison.InvariantCultureIgnoreCase) &&
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
            List<int> Preconfigured = new List<int>(Cabinet.OutputControllers.Where(OC => OC is UMXController).Select(C => ((UMXController)C).Number));
            foreach (var device in UMXControllerAutoConfigurator.AllDevices()) {
                if (!Preconfigured.Contains(device.UnitNo())) {
                    UMXController umxC = new UMXController(device.UnitNo());
                    if (!Cabinet.OutputControllers.Contains(umxC.Name)) {
                        Cabinet.OutputControllers.Add(umxC);
                        Log.Write("Detected and added UMX Controller Nr. {0} with name {1}".Build(umxC.Number, umxC.Name));
                        umxC.UpdateCabinetFromConfig(Cabinet);
                    }
                }
            }
        }
    }
}
