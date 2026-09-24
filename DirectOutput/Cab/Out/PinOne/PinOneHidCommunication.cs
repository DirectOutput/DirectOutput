using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using DirectOutput.Cab.Out;

namespace DirectOutput.Cab.Out.PinOne
{
    /// <summary>
    /// Wraps an open Win32 file handle to the PinOne board's USB HID vendor
    /// channel (the raw command/output interface added when the firmware
    /// moved off USB CDC serial). Write-only: DOF only ever sends output
    /// bank updates, it never needs to read config/state data back.
    /// </summary>
    public class PinOneHidDevice
    {
        public IntPtr fp;
        public string path;
        public ushort vendorID;
        public ushort productID;

        public PinOneHidDevice(IntPtr fp, string path, ushort vendorID, ushort productID)
        {
            this.fp = fp;
            this.path = path;
            this.vendorID = vendorID;
            this.productID = productID;
        }

        ~PinOneHidDevice()
        {
            Close();
        }

        public void Close()
        {
            if (fp != IntPtr.Zero && fp.ToInt32() != -1)
            {
                HIDImports.CloseHandle(fp);
                fp = IntPtr.Zero;
            }
        }

        private System.Threading.NativeOverlapped ov;

        /// <summary>
        /// Sends a 9-byte PinOne protocol packet (matching the format
        /// firmware's Communication.cpp expects: [0, admin/bank byte,
        /// data0..data6]) wrapped in a full 64-byte HID OUTPUT report
        /// (report ID 6 + the 9 payload bytes, zero-padded).
        /// </summary>
        public bool WriteUSB(byte[] payload)
        {
            byte[] buf = new byte[PinOneHidCommunication.ReportSize];
            buf[0] = PinOneHidCommunication.ReportId;
            Array.Copy(payload, 0, buf, 1, Math.Min(payload.Length, buf.Length - 1));

            for (int tries = 0; tries < 3; ++tries)
            {
                if (HIDImports.WriteFile(fp, buf, (uint)buf.Length, out uint actual, ref ov) == 0)
                {
                    if (TryReopenHandle())
                        continue;

                    Log.Error("PinOne Controller USB error sending request to device: " + GetLastWin32ErrMsg());
                    return false;
                }
                else if (actual != buf.Length)
                {
                    Log.Error("PinOne Controller USB error sending request: not all bytes sent");
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryReopenHandle()
        {
            // if the last error is 6 ("invalid handle"), try re-opening it
            if (Marshal.GetLastWin32Error() == 6)
            {
                Log.Error("PinOne Controller: invalid handle on write; trying to reopen handle");
                IntPtr fp2 = HIDImports.CreateFile(
                    path, HIDImports.GENERIC_READ_WRITE, HIDImports.SHARE_READ_WRITE,
                    IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                if (fp2 != IntPtr.Zero && fp2.ToInt32() != -1)
                {
                    fp = fp2;
                    return true;
                }
            }

            return false;
        }

        private String GetLastWin32ErrMsg()
        {
            int errNo = Marshal.GetLastWin32Error();
            return String.Format("{0} (Win32 error {1})",
                                 new System.ComponentModel.Win32Exception(errNo).Message, errNo);
        }
    }

    /// <summary>
    /// Locates the PinOne board's USB HID vendor channel. This is the
    /// current-firmware (VID 0x39F5) counterpart to the legacy CDC serial
    /// path in PinOneCommunication.cs/NamedPipeServer.cs, which remains
    /// for boards still on the old firmware (VID 0x0E8F).
    ///
    /// Modeled closely on Pinscape.cs's HID device enumeration (same
    /// SetupAPI + HidP_GetCaps pattern is needed here because the physical
    /// device exposes multiple HID collections - gamepad, consumer
    /// control, keyboard, and this vendor channel - and only the vendor
    /// one accepts PinOne protocol packets), but scans fresh on every call
    /// rather than caching once at class load, since PinOne only ever
    /// supports a single unit (unlike Pinscape's multi-unit numbering).
    /// </summary>
    public static class PinOneHidCommunication
    {
        // Must match firmware UsbHid.cpp: USB.VID(0x39F5) / USB.PID(0x9208),
        // and the USBHIDVendor(63, true) channel's report ID
        // (HID_REPORT_ID_VENDOR = 6) on usage page 0xFF00 (Vendor-Defined),
        // usage 0x01 - the same constants used in the config tool's
        // hidBoard.ts.
        public const ushort VendorID = 0x39F5;
        public const ushort ProductID = 0x9208;
        public const ushort UsagePage = 0xFF00;
        public const ushort Usage = 0x01;
        public const byte ReportId = 6;
        public const int ReportSize = 64; // report ID byte + 63 payload bytes

        /// <summary>
        /// Returns the first PinOne HID vendor device found, or null if
        /// none is connected.
        /// </summary>
        public static PinOneHidDevice FindDevice()
        {
            List<PinOneHidDevice> devices = FindDevices();
            return devices.Count > 0 ? devices[0] : null;
        }

        /// <summary>
        /// Scans all HID devices in the system for PinOne's vendor
        /// command/output channel (VID/PID + usage page/usage match).
        /// </summary>
        public static List<PinOneHidDevice> FindDevices()
        {
            List<PinOneHidDevice> devices = new List<PinOneHidDevice>();

            HIDImports.HidD_GetHidGuid(out Guid guid);
            IntPtr hDevice = HIDImports.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, HIDImports.DIGCF_DEVICEINTERFACE);

            HIDImports.SP_DEVICE_INTERFACE_DATA diData = new HIDImports.SP_DEVICE_INTERFACE_DATA();
            diData.cbSize = Marshal.SizeOf(diData);

            for (uint i = 0;
                 HIDImports.SetupDiEnumDeviceInterfaces(hDevice, IntPtr.Zero, ref guid, i, ref diData);
                 ++i)
            {
                HIDImports.SetupDiGetDeviceInterfaceDetail(hDevice, ref diData, IntPtr.Zero, 0, out UInt32 size, IntPtr.Zero);

                HIDImports.SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new HIDImports.SP_DEVICE_INTERFACE_DETAIL_DATA();
                diDetail.cbSize = (IntPtr.Size == 8) ? (uint)8 : (uint)5;
                if (!HIDImports.SetupDiGetDeviceInterfaceDetail(hDevice, ref diData, ref diDetail, size, out size, IntPtr.Zero))
                    continue;

                IntPtr fp = HIDImports.CreateFile(
                    diDetail.DevicePath, HIDImports.GENERIC_READ_WRITE, HIDImports.SHARE_READ_WRITE,
                    IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                bool keep = false;
                try
                {
                    if (fp == IntPtr.Zero || fp.ToInt32() == -1)
                        continue;

                    HIDImports.HIDD_ATTRIBUTES attrs = new HIDImports.HIDD_ATTRIBUTES();
                    attrs.Size = Marshal.SizeOf(attrs);
                    if (!HIDImports.HidD_GetAttributes(fp, ref attrs))
                        continue;

                    if (attrs.VendorID != VendorID || attrs.ProductID != ProductID)
                        continue;

                    // The device exposes several HID collections (gamepad,
                    // consumer control, keyboard, and this vendor channel).
                    // Only the one matching our vendor usage page/usage
                    // accepts PinOne protocol packets.
                    if (!HIDImports.HidD_GetPreparsedData(fp, out IntPtr preparsedData))
                        continue;

                    try
                    {
                        HIDImports.HIDP_CAPS caps = new HIDImports.HIDP_CAPS();
                        HIDImports.HidP_GetCaps(preparsedData, ref caps);

                        if (caps.UsagePage != UsagePage || caps.Usage != Usage)
                            continue;
                    }
                    finally
                    {
                        HIDImports.HidD_FreePreparsedData(preparsedData);
                    }

                    devices.Add(new PinOneHidDevice(fp, diDetail.DevicePath, attrs.VendorID, attrs.ProductID));
                    keep = true;
                }
                finally
                {
                    if (!keep && fp != IntPtr.Zero && fp.ToInt32() != -1)
                        HIDImports.CloseHandle(fp);
                }
            }

            return devices;
        }
    }
}
