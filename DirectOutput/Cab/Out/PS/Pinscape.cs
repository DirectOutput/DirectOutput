using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Text.RegularExpressions;

namespace DirectOutput.Cab.Out.PS
{
    /// <summary>
    /// The <a href="https://developer.mbed.org/users/mjr/code/Pinscape_Controller/">Pinscape Controller</a> is an open-source
    /// software/hardware project based on the inexpensive and powerful Freescale FRDM-KL25Z microcontroller development platform.
    /// It provides a full set of virtual pinball cabinet I/O features, including analog plunger, accelerometer nudging, key/button
    /// input, and a flexible array of PWM outputs.
    ///
    /// For DOF purposes, we're only interested in the output controller features; all of the input features are handled through
    /// the standard Windows USB joystick drivers.  The output controller emulates an LedWiz, so legacy LedWiz-aware software can
    /// access its basic functionality.  However, the Pinscape controller has expanded functionality that the LedWiz protocol
    /// can't access due to its inherent design limits.  To allow access to the expanded functionality, the Pinscape controller 
    /// uses custom extensions to the LedWiz protocol.  This DirectOutput framework module lets DOF use the extended protocol to
    /// take full advantage of the extended features.  First and most importantly, the Pinscape controller can support many more
    /// output channels than a real LedWiz.  In fact, there's no hard limit to the number of channels that could be attached
    /// to one controller, although the practical limit is probably about 200, and the reference hardware design provides
    /// up to about 60.  The extended protocol allows for about 130 channels, which is hopefully well beyond what anyone will
    /// be motivated to actually build in hardware.  Second, the extended protocol provides 8-bit PWM resolution, whereas the
    /// LedWiz protocol is limited to 49 levels (about 5-1/2 bit resolution).  DOF uses 8-bit resolution internally, so this
    /// lets devices show the full range of brightness levels that DOF can represent internally, for smoother fades and more
    /// precise color control in RGB devices (or more precise speed control in motors, intensity control in solenoids, etc).
    ///
    /// DOF uses the extended protocol, so it can fully access all of the expanded features.
    /// Legacy software that uses only the original LedWiz protocol (e.g., Future Pinball) can still recognize the device and
    /// access the first 32 output ports, using 49-level PWM resolution.
    /// 
    /// DOF can automatically detect connected Pinscape controllers and configure them for use with the framework.
    ///
    /// The Pinscape Controller project can be found on <a href="https://developer.mbed.org/users/mjr/code/Pinscape_Controller/">mbed.org</a>.
    /// </summary>
    public class Pinscape : OutputControllerFlexCompleteBase
    {
        #region Number


        private readonly object NumberUpdateLocker = new object();
        private int _Number = -1;

        /// <summary>
        /// Gets or sets the unit number of the controller.<br />
        /// The unit number must be unique.<br />
        /// Setting changes the Name property, if it is blank or if the Name corresponds to "Pinscape Controller {Number}".
        /// </summary>
        /// <value>
        /// The unique unit number of the controller (Range 1-16).
        /// </value>
        /// <exception cref="System.Exception">
        /// Pinscape Unit Numbers must be between 1-16. The supplied number {0} is out of range.
        /// </exception>
        public int Number
        {
            get { return _Number; }
            set
            {
                if (!value.IsBetween(1, 16))
                {
                    throw new Exception("Pinscape Unit Numbers must be between 1-16. The supplied number {0} is out of range.".Build(value));
                }
                lock (NumberUpdateLocker)
                {
                    // if the unit number changed, update it and attach to the new unit
                    if (_Number != value)
                    {
                        // if we used a default name for the old unit number, change to the default
                        // name for the new unit number
                        if (Name.IsNullOrWhiteSpace() || Name == "Pinscape Controller {0:00}".Build(_Number))
                        {
                            Name = "Pinscape Controller {0:00}".Build(value);
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
		/// Gets or sets the minimum interval between command in milliseconds (default: 1 ms).<br/>
		/// This was originally added as a stab-in-the-dark workaround for problems that were not understood
		/// at the time, but which were ultimately traced to bugs in the firmware of genuine LedWiz devices.
		/// It was incorrectly believed at the time that the bug was related to the Windows USB HID driver,
		/// so a global message rate throttle was added as a general feature of all HID-based devices.  The
		/// only device that ever actually needed it was the LedWiz, because the bug never had anything to 
		/// do with DOF or the Windows HID driver or anything else on the PC side.  It's still in here as a
		/// legacy, but devices other than genuine LedWiz's don't need it.
		/// 
		/// Note that the bug we're referring only affects GENUINE LedWiz devices.  The Pinscape LedWiz
		/// emulation doesn't have the bug, nor do any of the open-source LedWiz clone projects for
		/// Arduino or other devices.  The timing workaround wouldn't be needed for a Pinscape unit EVEN
		/// IF we talked to it in LedWiz mode, which we don't (DOF uses Pinscape's custom extended protocol).
		/// </summary>
		/// <value>
		/// The minimal interval between command in milliseconds.  The default is 1ms, which is also the minimum, since it's
		/// the fastest that USB allows at the hardware protocol level.
		/// </value>
		public int MinCommandIntervalMs
        {
            get { return _MinCommandIntervalMs; }
            set
            {
                _MinCommandIntervalMs = value.Limit(0, 1000);
                MinCommandIntervalMsSet = true;
            }
        }

        #endregion
        
        #region IOutputController implementation

        /// <summary>
        /// Initializes the Pinscape object.<br />
        /// This method also starts the worker thread which does the actual update work when Update() is called.<br />
        /// This method should only be called once. Subsequent calls have no effect.
        /// </summary>
        /// <param name="Cabinet">The Cabinet object which is using the Pinscape instance.</param>
        public override void Init(Cabinet Cabinet)
        {
            // get the minimum update interval from the global config
            if (!MinCommandIntervalMsSet
                && Cabinet.Owner.ConfigurationSettings.ContainsKey("PinscapeDefaultMinCommandIntervalMs")
				&& Cabinet.Owner.ConfigurationSettings["PinscapeDefaultMinCommandIntervalMs"] is int minInterval)
                MinCommandIntervalMs = minInterval;

            // do the base class work
            base.Init(Cabinet);
        }

        /// <summary>
        /// Finishes the Pinscape object.<br/>
        /// Finish also terminates the worker thread for updates.
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

        /// <summary>
        /// Send updated outputs to the physical device.
        /// </summary>
        protected override void UpdateOutputs(byte[] NewOutputValues)
        {
            // The extended protocol lets us update outputs in blocks of 7.
            // Run through our output list and send an update for each bank
            // that's changed.  The extended protocol message starts with
            // a byte set to 200+B, where B is the bank number - B=0 for
            // outputs 1-7, B=1 for outputs 8-14, etc.
            //
            // Note that, unlike the LedWiz protocol, the extended protocol
            // uses ONLY the brightness value to control each output.  There's
            // no separate on/off state.  "Off" is simply a brightness of 0.
            byte pfx = 200;
            for (int i = 0 ; i < NumberOfOutputs ; i += 7, ++pfx)
            {
                // look for a change among this bank's 7 outputs
                int lim = Math.Min(i + 7, NumberOfOutputs);
                for (int j = i ; j < lim ; ++j)
                {
                    // if this output has changed, flush the bank
                    if (NewOutputValues[j] != OldOutputValues[j])
                    {
                        // found a change - send the bank
                        UpdateDelay();
                        byte[] buf = new byte[9];
                        buf[0] = 0;             // USB report ID - always 0
                        buf[1] = pfx;			// message prefix
                        Array.Copy(NewOutputValues, i, buf, 2, lim - i);
                        Dev.WriteUSB(buf);

                        // the new values are now the current values on the device
                        Array.Copy(NewOutputValues, i, OldOutputValues, i, lim - i);

                        // we've sent this whole bank of 7 - move on to the next
                        break;
                    }
                }
            }
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

            public Device(IntPtr fp, string path, string name, ushort vendorID, ushort productID, short version, uint inputReportByteLength)
            {
                // remember the settings
                this.fp = fp;
                this.path = path;
                this.name = name;
                this.vendorID = vendorID;
                this.productID = productID;
                this.version = version;
                this.plungerEnabled = true;
                this.inputReportByteLength = inputReportByteLength;

                // presume we have the standard LedWiz-compatible complement of 32 outputs
                this.numOutputs = 32;

                // If we're using the LedWiz vendor/product ID, the unit number is encoded in the product
                // ID (it's the bottom 4 bits of the ID value - this is zero-based, so add one to get our
                // 1-based value for UI reporting).  If we're using our private vendor/product ID, the unit
                // number must be obtained from the configuration report below.
                if (vendorID == 0xFAFA && (productID & 0xFFF0) == 0x00F0)
                    this.unitNo = (short)((productID & 0x000f) + 1);
                else
                    this.unitNo = 1;

                // read a status report
                byte[] buf = ReadUSB();
                if (buf != null)
                {
                    // parse the response
                    this.plungerEnabled = (buf[1] & 0x01) != 0;
                }

				// Request a configuration report (special request type 4)
				SpecialRequest(4);
				for (int i = 0 ; i < 16 ; ++i)
				{
					// read a report - if it's our configuration reply, parse it, otherwise
					// skip it (there might be one or more joystick status reports buffered)
					buf = ReadUSB();
					if (buf != null && (buf[2] & 0xF8) == 0x88)
					{
						// get the number of outputs configured on the hardware side
						this.numOutputs = (int)buf[3] | (((int)buf[4]) << 8);

						// get the unit number
						unitNo = (short)(((ushort)buf[5] | (((ushort)buf[6]) << 8)) + 1);
						break;
					}
				}
			}

            ~Device()
            {
				if (fp.ToInt32() != 0 && fp.ToInt32() != -1)
				{
                    HIDImports.CloseHandle(fp);
					fp = IntPtr.Zero;
				}
            }

			// Does this device emulate a given LedWiz unit?  The unit is identified by
			// the nominal user-visible numbering scheme where the first unit at VID/PID
			// FAFA/00F0 is LedWiz #1.
			public bool IsLedWizEmulator(int unitNum)
			{
				return (ushort)vendorID == 0xFAFA && productID == 0x00F1 + unitNum;
			}

			private System.Threading.NativeOverlapped ov;
			public byte[] ReadUSB()
			{
				for (int tries = 0 ; tries < 3 ; ++tries)
				{
					byte[] buf = new byte[inputReportByteLength];
					buf[0] = 0x00;
					if (HIDImports.ReadFile(fp, buf, inputReportByteLength, out uint actual, ref ov) == 0)
					{
						// if the error is 6 ("invalid handle"), try re-opening the device
						if (TryReopenHandle())
							continue;

						Log.Error("Pinscape Controller USB error reading from device: " + GetLastWin32ErrMsg());
						return null;
					}
					else if (actual != inputReportByteLength)
					{
						Log.Error("Pinscape Controller USB error reading from device: not all bytes received");
						return null;
					}
					else
						return buf;
				}

				// don't retry more than a few times
				return null;
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
				if (Marshal.GetLastWin32Error() == 6)
				{
					// try opening a new handle on the device path
					Log.Error("Pinscape Controller: invalid handle on read; trying to reopen handle");
					IntPtr fp2 = OpenFile();

					// if that succeeded, replace the old handle with the new one and retry the read
					if (fp2 != null)
					{
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
				int errNo = Marshal.GetLastWin32Error();
				return String.Format("{0} (Win32 error {1})",
									 new System.ComponentModel.Win32Exception(errNo).Message, errNo);
			}

			public void AllOff()
			{
				// send the All Outputs Off request (special request 5)
				SpecialRequest(5);
			}

			public bool SpecialRequest(byte id)
			{
				byte[] buf = new byte[9];
				buf[0] = 0x00;  // report ID - always 0
				buf[1] = 0x41;  // 0x41 -> Pinscape special request
				buf[2] = id;    // special request type
				return WriteUSB(buf);
			}

			public bool WriteUSB(byte[] buf)
			{
				for (int tries = 0; tries < 3; ++tries)
				{
					if (HIDImports.WriteFile(fp, buf, 9, out uint actual, ref ov) == 0)
					{
						// try re-opening the handle, if it's an "invalid handle" error
						if (TryReopenHandle())
							continue;

						Log.Error("Pinscape Controller USB error sending request to device: " + GetLastWin32ErrMsg());
						return false;
					}
					else if (actual != 9)
					{
						Log.Error("Pinscape Controller USB error sending request: not all bytes sent");
						return false;
					}
					else
					{
						return true;
					}
				}

				// maximum retries exceeded - return failure
				return false;
			}

			public IntPtr fp;
			public string path;
			public string name;
			public ushort vendorID;
			public ushort productID;
			public short version;
			public short unitNo;
			public bool plungerEnabled;
			public int numOutputs;
            public uint inputReportByteLength;
		}

		#endregion


		#region Device enumeration

		/// <summary>
		/// Get the list of all Pinscape devices discovered in the system from the Windows USB device scan.
		/// </summary>
		public static List<Device> AllDevices()
		{
			return Devices;
		}

		// Search the Windows USB HID device set for Pinscape controllers
		private static List<Device> FindDevices()
		{
			// set up an empty return list
			List<Device> devices = new List<Device>();

			// get the list of devices matching the HID class GUID
			HIDImports.HidD_GetHidGuid(out Guid guid);
			IntPtr hDevice = HIDImports.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, HIDImports.DIGCF_DEVICEINTERFACE);

			// set up the attribute structure buffer
			HIDImports.SP_DEVICE_INTERFACE_DATA diData = new HIDImports.SP_DEVICE_INTERFACE_DATA();
			diData.cbSize = Marshal.SizeOf(diData);

			// read the devices in our list
			for (uint i = 0;
				 HIDImports.SetupDiEnumDeviceInterfaces(hDevice, IntPtr.Zero, ref guid, i, ref diData);
				 ++i)
			{
				// get the size of the detail data structure
				HIDImports.SetupDiGetDeviceInterfaceDetail(hDevice, ref diData, IntPtr.Zero, 0, out UInt32 size, IntPtr.Zero);

				// now actually read the detail data structure
				HIDImports.SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new HIDImports.SP_DEVICE_INTERFACE_DETAIL_DATA();
				diDetail.cbSize = (IntPtr.Size == 8) ? (uint)8 : (uint)5;
				if (HIDImports.SetupDiGetDeviceInterfaceDetail(hDevice, ref diData, ref diDetail, size, out size, IntPtr.Zero))
				{
					// create a file handle to access the device
					IntPtr fp = HIDImports.CreateFile(
						diDetail.DevicePath, HIDImports.GENERIC_READ_WRITE, HIDImports.SHARE_READ_WRITE,
						IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

					// read the attributes
					HIDImports.HIDD_ATTRIBUTES attrs = new HIDImports.HIDD_ATTRIBUTES();
					attrs.Size = Marshal.SizeOf(attrs);
					if (HIDImports.HidD_GetAttributes(fp, ref attrs))
					{
                        // presume this is a Pinscape Controller, then look for reasons it's not
                        bool ok = true;

                        // read the product name string
                        String name = "<not available>";
                        byte[] nameBuf = new byte[128];
                        if (HIDImports.HidD_GetProductString(fp, nameBuf, 128))
                            name = System.Text.Encoding.Unicode.GetString(nameBuf).TrimEnd('\0');

                        // If the vendor and product ID match an LedWiz OR our private ID, and the
                        // product name contains "pinscape", and it's product version 7 or higher,
                        // it's a Pinscape controller with the extended protocol features.
                        bool isLW = ((ushort)attrs.VendorID == 0xFAFA && (attrs.ProductID >= 0x00F0 && attrs.ProductID <= 0x00FF));
                        bool isPS = ((ushort)attrs.VendorID == 0x1209 && ((ushort)attrs.ProductID == 0xEAEA));
                        ok &= ((isLW || isPS)
                                && Regex.IsMatch(name, @"\b(?i)pinscape\b")
                                && attrs.VersionNumber >= 7);

						// Newer versions of the device software can present multiple USB HID
						// interfaces, including Keyboard (usage page 1, usage 6) and Media
						// Control (volume up/down/mute buttons) (usage page 12, usage 1).
						// The output controller is always part of the Joystick interface
						// (usage page 1, usage 4) OR a vendor-specific "undefined" interface
						// (usage page 1, usage 0).  HidP_GetCaps() returns the USB usage
						// information for the first HID report descriptor associated with
						// the interface, so we can determine which interface we're looking
						// at by checking this information.  Start by getting the preparsed
						// data from the Windows HID driver.
                        uint inputReportByteLength = 0;
                        if (ok && HIDImports.HidD_GetPreparsedData(fp, out IntPtr preparsedData))
                        {
                            // get the device caps
                            HIDImports.HIDP_CAPS caps = new HIDImports.HIDP_CAPS();
                            HIDImports.HidP_GetCaps(preparsedData, ref caps);

							// This Pinscape interface accepts output controller commands only
							// if it's the joystick type (usage page 1 == generic desktop, usage
							// 4 == joystick).  If it doesn't match, it must be a secondary HID
							// interface on the same device, such as the keyboard or media
							// controller interface.  Skip those interfaces, as they don't
							// accept the output controller commands.
                            ok &= (caps.UsagePage == 1 && (caps.Usage == 4 || caps.Usage == 0));

                            // remember the input report byte length
                            inputReportByteLength = caps.InputReportByteLength;

                            // done with the preparsed data
                            HIDImports.HidD_FreePreparsedData(preparsedData);
                        }

                        // If we passed all tests, this is the output controller interface for
                        // a Pinscape controller device, so add the device to our list.
                        if (ok)
						{
							// add the device to our list
							devices.Add(new Device(fp, diDetail.DevicePath, name, attrs.VendorID, attrs.ProductID, attrs.VersionNumber, inputReportByteLength));

							// the device list object owns the handle now
							fp = System.IntPtr.Zero;
						}
					}

					// done with the file handle
                    if (fp.ToInt32() != 0 && fp.ToInt32() != -1)
                        HIDImports.CloseHandle(fp);
                }
            }

            // return the device list
            return devices;
        }

        #endregion

        // list of Pinscape controller devices discovered in Windows USB HID scan
        private static readonly List<Device> Devices;

        // my device
        private Device Dev;

        #region Constructor

        /// <summary>
        /// Initializes the Pinscape class.
        /// </summary>
        static Pinscape()
        {
            // scan the Windows USB HID device set for installed Pinscape Controller devices,
            // and save the list statically in the class
            Devices = FindDevices();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pinscape"/> class with no unit number set.
        /// The unit number must be set before use (via the Number property).
        /// </summary>
        public Pinscape()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pinscape"/> class with a given unit number.
        /// </summary>
        /// <param name="Number">The number of the Pinscape controller (1-16).</param>
        public Pinscape(int Number)
        {
            this.Number = Number;
        }

        #endregion
    }
}
