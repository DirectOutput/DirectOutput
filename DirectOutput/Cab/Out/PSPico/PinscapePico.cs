using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using PerCederberg.Grammatica.Runtime.RE;

namespace DirectOutput.Cab.Out.PSPico
{
	/// <summary>
	/// The <a href="https://gihub.com/mjrgh/PinscapePico/">Pinscape Pico</a> controller is an open-source
	/// software/hardware project based on the Raspberry Pi Pico.  Pinscape Pico is a sequel to the original
	/// Pinscape Controller for KL25Z, providing a full set of pinball cabinet I/O features, including analog
	/// plunger, accelerometer nudging, key/button input, and a flexible array of PWM outputs.
	///
	/// DOF is only concerned with the Pinscape Pico's output controller features.  Pinscape Pico provides a
	/// custom HID interface for the output controller functions, which DOF uses to send commands to the
	/// device.  Refer to USBProtocol/FeedbackControllerInterface.h in the Pinscape Pico source repository
	/// for documentation on the protocol.
	/// 
	/// DOF can automatically detect connected Pinscape Pico units (by scanning the set of live HID instances)
	/// and configure them for use with the framework.  No manual user configuration is required to use these
	/// devices.
	/// </summary>
	public class PinscapePico : OutputControllerFlexCompleteBase
	{
		#region Number

		private readonly object NumberUpdateLocker = new object();
		private int _Number = -1;

		/// <summary>
		/// Gets or sets the unit number of the controller.<br />
		/// The unit number must be unique.<br />
		/// Setting changes the Name property, if it is blank or if the Name corresponds to "Pinscape Pico {Number}".
		/// </summary>
		/// <value>
		/// The unique unit number of the controller (Range 1-16).
		/// </value>
		/// <exception cref="System.Exception">
		/// Pinscape Pico Unit Numbers must be between 1-16. The supplied number {0} is out of range.
		/// </exception>
		public int Number
		{
			get { return _Number; }
			set
			{
				if (!value.IsBetween(1, 16))
					throw new Exception("Pinscape Pico Unit Numbers must be between 1-16. The supplied number {0} is out of range.".Build(value));

				lock (NumberUpdateLocker)
				{
					// if the unit number changed, update it and attach to the new unit
					if (_Number != value)
					{
						// if we used a default name for the old unit number, change to the default
						// name for the new unit number
						if (Name.IsNullOrWhiteSpace() || Name == "Pinscape Pico {0:00}".Build(_Number))
							Name = "Pinscape Pico {0:00}".Build(value);

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

		/// <summary>
		/// Gets or sets the minimum interval between command in milliseconds (default: 1 ms).<br/>
		/// This was originally added as a stab-in-the-dark workaround for problems that were not understood
		/// at the time, but which were ultimately traced to bugs in the firmware of genuine LedWiz devices.
		/// It was incorrectly believed at the time that the bug was related to the Windows USB HID driver,
		/// so a global message rate throttle was added as a general feature of all HID-based devices.  The
		/// only device that ever actually needed it was the LedWiz, because the bug never had anything to 
		/// do with DOF or the Windows HID driver or anything else on the PC side.  It's still in here as a
		/// legacy, but devices other than genuine LedWiz's don't need it.
		/// </summary>
		/// <value>
		/// The minimal interval between command in milliseconds.  The default is 1ms, which is also the minimum, since it's
		/// the fastest that USB allows at the hardware protocol level.
		/// </value>
		public int MinCommandIntervalMs
		{
			get { return _MinCommandIntervalMs; }
			set { _MinCommandIntervalMs = value.Limit(0, 1000); }
		}

		#endregion

		#region IOutputController implementation

		/// <summary>
		/// Initializes the Pinscape Pico object.<br />
		/// This method also starts the worker thread which does the actual update work when Update() is called.<br />
		/// This method should only be called once. Subsequent calls have no effect.
		/// </summary>
		/// <param name="Cabinet">The Cabinet object which is using the device instance.</param>
		public override void Init(Cabinet Cabinet)
		{
			// Note - this device doesn't have a configurable minimum command interval,
			// so there's no "PinscapePicoDefaultMinCommandIntervalMs" setting in the
			// cab config file (as there is for the original KL25Z Pinscape).  The
			// parameter is only present in older devices because it was incorrectly
			// thought at the time that it was needed to address a Windows HID driver
			// bug, which turned out to have nothing to do with Windows and everything
			// to do with the genuine LedWiz firmware.  Devices other than LedWiz's
			// don't need it, so we're leaving it out for this newer device.

			// do the base class work
			base.Init(Cabinet);
		}

		/// <summary>
		/// Finishes the Pinscape Pico object.<br/>
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

		// output list group - common base class for the random and contiguous formats
		abstract class OutputListGroup
		{
			// get the number of USB packets represented by this group
			public abstract int PacketCount { get; }

			// get the number of ports updated by this group
			public abstract int PortCount { get; }

			// add a port to the group list
			public abstract void Add(int port, int value, bool changed);

			// send the packets
			public abstract void Send(PinscapePico psp);
		}

		// random access output list
		class RandomAccessOutputList
		{
			public List<KeyValuePair<int, int>> outputs = new List<KeyValuePair<int, int>>();
			public bool Add(int port, int value)
			{
				// up to 30 ports can be accessed randomly
				if (outputs.Count == 30)
					return false;

				outputs.Add(new KeyValuePair<int, int>(port, value));
				return true;
			}
		}

		class RandomAccessOutputListGroup : OutputListGroup
		{ 
			readonly List<RandomAccessOutputList> list = new List<RandomAccessOutputList>();
			RandomAccessOutputList cur = null;
			public override int PacketCount {  get { return list.Count; } }	
			public override int PortCount {  get { return list.Sum(r => r.outputs.Count); } }
			public override void Add(int port, int value, bool changed)
			{
				if (changed)
				{
					// value changed, so we have to add it to a list - make sure a list is open
					if (cur == null)
						AddList();

					// add the value to the open list
					if (!cur.Add(port, value))
					{
						// out of space in the old list; add a new one
						AddList();
						cur.Add(port, value);
					}
				}
			}

			private void AddList()
			{
				cur = new RandomAccessOutputList();
				list.Add(cur);
			}
			public override void Send(PinscapePico psp)
			{
				// send each packet in the list
				foreach (var l in list)
				{
					// construct the output report:
					//
					//   [0] -> HID report ID prefix
					//   [1] -> command ID 0x22, SET OUTPUT PORTS
					//   [2] -> number of ports in packet
					//   [3] -> first port number
					//   [4] -> first port value
					//   [5] -> second port number
					//   [6] -> second port value
					//   ...
					//   [61] -> 30th port number
					//   [62] -> 30th port value
					//   [63] -> unused
					//
					byte[] buf = new byte[psp.Dev.outputReportByteLength];
					buf[0] = psp.Dev.outputReportId;
					buf[1] = 0x22;
					buf[2] = (byte)l.outputs.Count;
					int idx = 3;
					foreach (var output in l.outputs)
					{
						buf[idx++] = (byte)output.Key;
						buf[idx++] = (byte)output.Value;
					}

					// send the report
					psp.Dev.WriteUSB("SET OUTPUT PORTS", buf, 100);
				}
			}
		}

		// contiguous access output list
		class ContiguousOutputList
		{
			public ContiguousOutputList(int firstPortNum) { this.firstPortNum = firstPortNum; }
			public int firstPortNum = 0;
			public int numToSend = 0;
			public List<int> outputs = new List<int>();
			public bool Add(int value, bool changed)
			{
				// up to 60 ports can be accessed consecutively
				if (outputs.Count == 60)
					return false;

				// add the value to the list
				outputs.Add(value);

				// If this value has changed, we need to include it in the
				// report sent to the device, so we have to send the whole
				// list up to the list point.  Values that haven't changed
				// don't need to be sent, even though we collect them all
				// here in case we find a value that has changed further
				// down the line.
				if (changed)
					numToSend = outputs.Count;

				// success
				return true;
			}
		}

		class ContiguousOutputListGroup : OutputListGroup
		{
			readonly List<ContiguousOutputList> list = new List<ContiguousOutputList>();
			ContiguousOutputList cur = null;
			public override int PacketCount { get { return list.Count; } }
			public override int PortCount { get { return list.Sum(r => r.numToSend); } }
			public override void Add(int port, int value, bool changed)
			{
				if (changed)
				{
					// value changed, so we have to add it to a list - make sure a list is open
					if (cur == null)
						AddList(port);

					// add the value to the open list
					if (!cur.Add(value, changed))
					{
						// out of space in the old list; add a new one
						AddList(port);
						cur.Add(port, changed);
					}
				}
				else if (cur != null)
				{
					// Value hasn't changed, but a list is open, so we have to add it to keep
					// the contiguous range going, in case we encounter another item further
					// along that has been changed.  If the list is full, though, we can simply
					// close it out.
					if (cur != null && !cur.Add(value, changed))
						cur = null;
				}
			}
			private void AddList(int startingPort)
			{
				cur = new ContiguousOutputList(startingPort);
				list.Add(cur);
			}
			public override void Send(PinscapePico psp)
			{
				// send each packet in the list
				foreach (var l in list)
				{
					// construct the output report:
					//
					//   [0] -> HID report ID prefix
					//   [1] -> command ID 0x21, SET OUTPUT PORT BLOCK
					//   [2] -> number of ports in packet
					//   [3] -> first port value
					//   [4] -> second port value
					//   ...
					//   [63] -> 60th port value
					//
					// Note that we only have to send 'numToSend' elements, since this
					// records the last element in the list with changes to send.  The
					// list might have additional unchanged items past the end, which
					// we don't have to send.
					byte[] buf = new byte[psp.Dev.outputReportByteLength];
					buf[0] = psp.Dev.outputReportId;
					buf[1] = 0x21;
					buf[2] = (byte)l.numToSend;
					int idx = 3;
					foreach (var output in l.outputs)
						buf[idx++] = (byte)output;

					// send the report
					psp.Dev.WriteUSB("SET OUTPUT PORT BLOCK", buf, 100);
				}
			}
		}

		/// <summary>
		/// Send updated outputs to the physical device.
		/// </summary>
		protected override void UpdateOutputs(byte[] NewOutputValues)
		{
			// The Pinscape Pico output controller protocol lets us send
			// single-USB-packet updates that address up to 60 contiguously
			// numbered ports, or up to 30 randomly addressed ports.  To
			// minimize USB traffic, figure it both ways, and see which
			// way gives us fewer packets.  (Note that we only consider
			// the all-random or all-contiguous options.  It might be
			// possible in some cases to do better still with a mix, but
			// that's harder to analyze, and doesn't seem worth the effort.
			// 60 ports is enough that we'll probably be able to pack
			// everything into a single contiguous-range message in
			// practically every case, and when we can't do that, it's
			// hard to imagine needing more than two of them, since most
			// cabs don't have more than about 80 ports.  The only case
			// where we might find more ports is addressable light strips,
			// but Pinscape Pico doesn't handle those.)
			var contiguousGroup = new ContiguousOutputListGroup();
			var randomGroup = new RandomAccessOutputListGroup();
			int firstChangedIndex = -1;
			int lastChangedIndex = -1;
			for (int i = 0; i < NumberOfOutputs; ++i)
			{
				// get the value, note if it's changed
				int val = NewOutputValues[i];
				bool changed = (NewOutputValues[i] != OldOutputValues[i]);

				// add it to each list
				contiguousGroup.Add(i, val, changed);
				randomGroup.Add(i, val, changed);

				// update the change range
				if (changed)
				{
					if (firstChangedIndex < 0) firstChangedIndex = i;
					lastChangedIndex = i;
				}
			}

			// Pick the access style (contiguous or random) that requires
			// sending the fewest USB packets.  If the packet count is
			// the same, choose according to which updates the fewest
			// ports on the device side.  (Individual port updates cost
			// practically nothing, since they run on the Pico side and
			// just write the new level to a data structure in RAM, so
			// there's no real need to optimize for reduced port update
			// count.  But we have the information, so we  might as well
			// use it.)
			if (contiguousGroup.PacketCount <= randomGroup.PacketCount
				|| (contiguousGroup.PacketCount <= randomGroup.PacketCount && contiguousGroup.PortCount < randomGroup.PortCount))
				contiguousGroup.Send(this);
			else
				randomGroup.Send(this);

			// the new values are now the current values on the device
			if (firstChangedIndex >= 0)
			{
				Array.Copy(NewOutputValues, firstChangedIndex, OldOutputValues, firstChangedIndex,
					lastChangedIndex - firstChangedIndex + 1);
			}
		}

		// Output values as of the last send.  On each device update, we compare
		// the new values against the old values to optimize the USB traffic to
		// the device, sending the minimum number of packets needed to communicate
		// just the changes since last time.
		byte[] OldOutputValues;

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

			// construction
			public Device(IntPtr fp, string path, string name, ushort vendorID, ushort productID, short productVersion,
				int protocolVersion, byte inputReportId, byte inputReportLength, byte outputReportID, byte outputReportLength)
			{
				// remember the settings
				this.fp = fp;
				this.path = path;
				this.name = name;
				this.vendorID = vendorID;
				this.productID = productID;
				this.productVersion = productVersion;
				this.protocolVersion = protocolVersion;
				this.inputReportId = inputReportId;
				this.outputReportId = outputReportID;
				this.inputReportByteLength = inputReportLength;
				this.outputReportByteLength = outputReportLength;

				// we don't yet know the device's unit number or output port count
				unitNo = 0;
				numOutputs = 0;
				unitName = "NoName";

				// request an identification report (command 0x01, QUERY DEVICE IDENTIFICATION)
				DeviceRequest("QUERY DEVICE ID", 0x01);
				bool identified = false;
				for (int i = 0; i < 16; ++i)
				{
					// Read a report; skip it if it's not the identification report.
					// Identification reports have byte 0x01 in the command byte (the
					// second byte of the buffer, after the HID report ID prefix byte).
					var buf = ReadUSB(100);
					if (buf != null && buf[1] == 0x01)
					{
						// Identification report:
						//
						//   [0]       = HID report ID
						//   [1]       = command code 0x01
						//   [2]       = Pinscape Pico unit number
						//   [3..34]   = Pinscape Pico unit name (null-PADDED string, not necessarily null-terminated)
						//   [35..36]  = Protocol version, UINT16, little-endian
						//   [37..44]  = Pico hardware ID (opaque 8-byte binary string)
						//   [45..46]  = Number of output ports, UINT16, little-endian
						//   [47..48]  = Plunger type, UINT16, little-endian; possible values are defined in the interface spec
						//
						unitNo = buf[2];
						unitName = Encoding.GetEncoding("ISO-8859-1").GetString(buf, 3, 32).TrimEnd('\0');
						protocolVersion = ParseReportU16(buf, 35);
						hardwareID = string.Join("", new ArraySegment<byte>(buf, 37, 8).Select(b => b.ToString("X2")));
						numOutputs = ParseReportU16(buf, 45);
						plungerType = ParseReportU16(buf, 47);

						// report it in the log
						Log.Write("Pinscape Pico discovery found unit #{0} ({1}), {2} output ports, Pico hardware ID {3}, protocol version {4}".Build(
							unitNo, unitName, numOutputs, hardwareID, protocolVersion));

						// As long as we're setting up communications with the device, send it
						// the current wall-clock time, which the firmware can use to implement
						// time-of-day features.  The device depends on the host to send it the
						// current time, since the Pico doesn't have its own real-time clock;
						// and HID doesn't let the device send queries to the host, so the Pico
						// has to depend on applications sending this information incidentally,
						// which is what we're doing here.
						var now = DateTime.Now;

						// SET WALL CLOCK TIME command (see Pinscape Pico source -> USBProtocol/FeedbackControllerProtocol.h)
						// <0x14:BYTE> <Year:UINT16> <Month:BYTE> <Day:BYTE> <Hour:BYTE> <Minute:BYTE> <Second:BYTE>
						var timeBuf = new byte[outputReportByteLength];
						timeBuf[0] = outputReportID;
						timeBuf[1] = 0x14;             // SET WALL CLOCK TIME command code
						timeBuf[2] = (byte)(now.Year & 0xFF);
						timeBuf[3] = (byte)((now.Year >> 8) & 0xFF);
						timeBuf[4] = (byte)now.Month;
						timeBuf[5] = (byte)now.Day;
						timeBuf[6] = (byte)now.Hour;
						timeBuf[7] = (byte)now.Minute;
						timeBuf[8] = (byte)now.Second;
						if (!WriteUSB("SET WALL CLOCK TIME", timeBuf, 100))
							Log.Warning("Pinscape Pico: error setting wall clock time");

						// we found the response, so we can stop searching for it
						identified = true;
						break;
					}
				}

				// make sure we identified the device
				if (!identified)
				{
					Log.Error(("Pinscape Pico VID/PID {X4/X4} did not respond to identification query; "
						+ "this device's output port configuration is unknown, so no port updates will be sent "
						+ "to it during this session").Build(
						vendorID, productID));
				}
			}

			// Parse a UINT16 from a HID report.  All integers in Pinscape Pico feedback
			// controller interface reports are encoded in little-endian format.
			ushort ParseReportU16(byte[] buf, int index)
			{
				return (ushort)((ushort)buf[index] | (((ushort)buf[index + 1]) << 8));
			}

			// destruction
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

			// Overlapped I/O struct
			private System.Threading.NativeOverlapped ov = new NativeOverlapped();
			private readonly EventWaitHandle hOverlappedEvent = new EventWaitHandle(false, EventResetMode.ManualReset);

			public byte[] ReadUSB(uint timeout_ms)
			{
				// Initialize a buffer of the input report byte length.  The Windows
				// HID driver requires that we initialize the first byte of the buffer
				// with the HID report ID of the report type we wish to read.
				byte[] buf = new byte[inputReportByteLength];
				buf[0] = inputReportId;

				// retry the read a few times, since we might be able to correct
				// certain types of errors and retry
				uint actual = 0;
				for (int tries = 0; tries < 3; ++tries)
				{
					// set up the OVERLAPPED struct
					ov.EventHandle = hOverlappedEvent.SafeWaitHandle.DangerousGetHandle();

					// read a report - on successful completion, stop looping
					if (HIDImports.ReadFile(fp, buf, inputReportByteLength, out actual, ref ov) != 0)
						break;

					// if the error is 6 ("invalid handle"), try re-opening the device
					if (TryReopenHandle())
						continue;

					// if it's "pending", wait for up to the timeout; if that succeeds, stop looping
					if (GetLastWin32ErrorCode() == ERROR_IO_PENDING
						&& HIDImports.GetOverlappedResultEx(fp, ref ov, out actual, timeout_ms, true))
						break;

					// read or wait failed - log the error and return failure
					Log.Error("Pinscape Pico: USB error reading from device: " + GetLastWin32ErrMsg());
					return null;
				}

				// check that the actual byte length read matches the request
				if (actual != inputReportByteLength)
				{
					Log.Error("Pinscape Pico: USB error reading from device: not all bytes received");
					return null;
				}
					
				// success - return the buffer
				return buf;
			}

			public bool WriteUSB(string desc, byte[] buf, uint timeout_ms)
			{
				// retry the write a few times, since we might be able to correct
				// certain types of errors and retry
				uint actual = 0;
				for (int tries = 0; tries < 3; ++tries)
				{
					// set up the OVERLAPPED struct
					ov.EventHandle = hOverlappedEvent.SafeWaitHandle.DangerousGetHandle();

					// read a report - on successful completion, stop looping
					if (HIDImports.WriteFile(fp, buf, outputReportByteLength, out actual, ref ov) != 0)
						break;

					// if the error is 6 ("invalid handle"), try re-opening the device
					if (TryReopenHandle())
						continue;

					// if it's "pending", wait for up to the timeout; if that succeeds, stop looping
					if (GetLastWin32ErrorCode() == ERROR_IO_PENDING
						&& HIDImports.GetOverlappedResultEx(fp, ref ov, out actual, timeout_ms, true))
						break;

					// write or wait failed - log the error and return failure
					Log.Error("Pinscape Pico: USB error sending to device ({0}): {1}".Build(desc, GetLastWin32ErrMsg()));
					return false;
				}

				// check that the actual byte length written matches the request
				if (actual != outputReportByteLength)
				{
					Log.Error("Pinscape Pico: USB error sending to device ({0}): not all bytes sent".Build(desc));
					return false;
				}

				// success
				return true;
			}

			private IntPtr OpenFile()
			{
				return HIDImports.CreateFile(
					path, HIDImports.GENERIC_READ_WRITE, HIDImports.SHARE_READ_WRITE,
					IntPtr.Zero, FileMode.Open, HIDImports.EFileAttributes.Overlapped, IntPtr.Zero);
			}

			private bool TryReopenHandle()
			{
				// if the last error is 6 ("invalid handle"), try re-opening it
				if (Marshal.GetLastWin32Error() == ERROR_INVALID_HANDLE)
				{
					// try opening a new handle on the device path
					Log.Error("Pinscape Pico: invalid handle on read; trying to reopen handle");
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
				int errNum = Marshal.GetLastWin32Error();
				return String.Format("{0} (Win32 error {1})",
					new System.ComponentModel.Win32Exception(errNum).Message, errNum);
			}

			public int GetLastWin32ErrorCode()
			{
				return Marshal.GetLastWin32Error();
			}

			// internally useful Win32 error codes
			const int ERROR_IO_PENDING = 997;
			const int ERROR_INVALID_HANDLE = 6;

			public void AllOff()
			{
				// send the ALL PORTS OFF command (command code 0x20)
				DeviceRequest("ALL PORTS OFF", 0x20);
			}

			// Send a request to the device
			public bool DeviceRequest(string desc, byte commandId)
			{
				// set up the output report
				byte[] buf = new byte[outputReportByteLength];
				buf[0] = outputReportId;  // HID report ID
				buf[1] = commandId;       // command code
				return WriteUSB(desc, buf, 100);
			}

			public IntPtr fp;                       // file handle to USB device
			public string path;                     // device file system path
			public string name;                     // product name string reported by device
			public ushort vendorID;                 // USB VID
			public ushort productID;                // USB PID
			public short productVersion;            // product version reported by device
			public int protocolVersion;             // Pinscape Pico Feedback Controller protocol version, from interface signature string
			public ushort unitNo;                   // Pinscape Pico unit number, from device info query
			public String unitName;                 // Pinscape Pico unit name, from device info query
			public String hardwareID;               // Pico hardware ID, as a 16-hex-digit number encoded from the binary byte string
			public int plungerType;                 // plunger type code
			public int numOutputs;                  // number of output ports, from device info query
			public byte inputReportId;              // HID report ID for input report (device to host)
			public byte outputReportByteLength;     // HID input report byte length (including HID report ID prefix byte)
			public byte outputReportId;             // HID report ID for output report (host to device)
			public byte inputReportByteLength;      // HID output report byte length (including HID report ID prefix byte)
		}

		#endregion


		#region Device enumeration

		/// <summary>
		/// Get the list of all Pinscape Pico devices discovered in the system from the Windows USB device scan.
		/// </summary>
		public static List<Device> AllDevices()
		{
			return Devices;
		}

		// Search the Windows USB HID device set for Pinscape Pico units
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

			// read the devices in the HID list
			for (uint i = 0; HIDImports.SetupDiEnumDeviceInterfaces(hDevice, IntPtr.Zero, ref guid, i, ref diData); ++i)
			{
				// get the size of the detail data structure
				HIDImports.SetupDiGetDeviceInterfaceDetail(hDevice, ref diData, IntPtr.Zero, 0, out UInt32 size, IntPtr.Zero);

				// allocate a buffer and read the detail data structure, which gives us a virtual
				// file system path to the device interface
				HIDImports.SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new HIDImports.SP_DEVICE_INTERFACE_DETAIL_DATA();
				diDetail.cbSize = (IntPtr.Size == 8) ? (uint)8 : (uint)5;
				if (!HIDImports.SetupDiGetDeviceInterfaceDetail(hDevice, ref diData, ref diDetail, size, out size, IntPtr.Zero))
					continue;

				// open a handle to the device, which allows us to read and write it like a file
				IntPtr fp = HIDImports.CreateFile(
					diDetail.DevicePath, HIDImports.GENERIC_READ_WRITE, HIDImports.SHARE_READ_WRITE,
					IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

				// read the interface's HID attributes
				HIDImports.HIDD_ATTRIBUTES attrs = new HIDImports.HIDD_ATTRIBUTES();
				attrs.Size = Marshal.SizeOf(attrs);
				if (HIDImports.HidD_GetAttributes(fp, ref attrs))
				{
					// read the HID product name string
					String name = "<not available>";
					byte[] nameBuf = new byte[128];
					if (HIDImports.HidD_GetProductString(fp, nameBuf, 128))
						name = System.Text.Encoding.Unicode.GetString(nameBuf).TrimEnd('\0');

					// read the HID preparsed data, to get details on the report structure
					if (HIDImports.HidD_GetPreparsedData(fp, out IntPtr preparsedData))
					{
						// get the device caps
						HIDImports.HIDP_CAPS caps = new HIDImports.HIDP_CAPS();
						HIDImports.HidP_GetCaps(preparsedData, ref caps);

						// The Feedback Controller interface is defined as a private
						// vendor-defined interface, Usage Page 0x06 (Generic Device),
						// usage 0x00 (Undefined), and it has 64-byte IN/OUT reports.
						if (caps.UsagePage == 0x06 && caps.Usage == 0x00
							&& caps.InputReportByteLength == 64 && caps.OutputReportByteLength == 64)
						{
							// It's a match so far, but Generic/Undefined is too vague
							// to positively identify a Pinscape Pico; this usage code
							// just means that it's special to the device, so other,
							// unrelated devices could just as well have their own
							// special interfaces.  To positively identify a Pinscape
							// Pico Feedback Interface, we need to check the Usage
							// String on its input interface, which is a unique string
							// that identifies this specific interface.
							var bc = new HIDImports.HIDP_BUTTON_CAPS();
							ushort numButtonCaps = 1;
							uint gbStatus;
							if ((gbStatus = HIDImports.HidP_GetButtonCaps(HIDImports.HidP_Input, ref bc, ref numButtonCaps, preparsedData)) == HIDImports.HIDP_STATUS_SUCCESS
								&& numButtonCaps == 1 && bc.IsStringRange == 0 && bc.StringMin != 0)
							{
								// retrieve the indexed string
								byte[] strBuf = new byte[128];
								if (HIDImports.HidD_GetIndexedString(fp, bc.StringMin, strBuf, (uint)strBuf.Length))
								{
									// match the string against the unique interface signature
									var str = Encoding.Unicode.GetString(strBuf).TrimEnd('\0');
									var m = Regex.Match(str, @"PinscapeFeedbackController/(\d+)");
									if (m.Success)
									{
										// extract the protocol version from the string
										int protocolVersion = int.Parse(m.Groups[1].Value);

										// add the device to our list
										devices.Add(new Device(fp, diDetail.DevicePath, name,
											attrs.VendorID, attrs.ProductID, attrs.VersionNumber, protocolVersion,
											bc.ReportID, (byte)caps.InputReportByteLength,
											bc.ReportID, (byte)caps.OutputReportByteLength));

										// the device list object owns the handle now, so forget it locally
										// (so that we don't close it as we clean up resources for this
										// iteration)
										fp = System.IntPtr.Zero;
									}
								}
							}
						}

						// done with the preparsed data
						HIDImports.HidD_FreePreparsedData(preparsedData);
					}
				}

				// if we didn't assign ownership of the device handle to the list,
				// we're done with the file handle; close it
				if (fp.ToInt32() != 0 && fp.ToInt32() != -1)
					HIDImports.CloseHandle(fp);
			}

			// return the device list
			return devices;
		}

		#endregion

		// list of Pinscape Pico controller devices discovered in Windows USB HID scan
		private static readonly List<Device> Devices;

		// my device
		private Device Dev;

		#region Constructor

		/// <summary>
		/// Initializes the Pinscape Pico class.
		/// </summary>
		static PinscapePico()
		{
			// scan the Windows USB HID device set for installed Pinscape Pico devices,
			// and save the list statically in the class
			Devices = FindDevices();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PinscapePico"/> class with no unit number set.
		/// The unit number must be set before use (via the Number property).
		/// </summary>
		public PinscapePico()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PinscapePico"/> class with a given unit number.
		/// </summary>
		/// <param name="Number">The number of the Pinscape Pico (1-16).</param>
		public PinscapePico(int Number)
		{
			this.Number = Number;
		}

		#endregion
	}
}
