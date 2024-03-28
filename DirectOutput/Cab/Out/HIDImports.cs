//////////////////////////////////////////////////////////////////////////////////
//	HIDImports.cs
//	For more information: http://wiimotelib.codeplex.com/
//////////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace DirectOutput.Cab.Out
{
	public class HIDImports
	{
		// Flags controlling what is included in the device information set built by SetupDiGetClassDevs
		internal const int DIGCF_DEFAULT = 0x00000001;       // only valid with DIGCF_DEVICEINTERFACE
		internal const int DIGCF_PRESENT = 0x00000002;
		internal const int DIGCF_ALLCLASSES = 0x00000004;
		internal const int DIGCF_PROFILE = 0x00000008;
		internal const int DIGCF_DEVICEINTERFACE = 0x00000010;

		[Flags]
		internal enum EFileAttributes : uint
		{
			Readonly = 0x00000001,
			Hidden = 0x00000002,
			System = 0x00000004,
			Directory = 0x00000010,
			Archive = 0x00000020,
			Device = 0x00000040,
			Normal = 0x00000080,
			Temporary = 0x00000100,
			SparseFile = 0x00000200,
			ReparsePoint = 0x00000400,
			Compressed = 0x00000800,
			Offline = 0x00001000,
			NotContentIndexed = 0x00002000,
			Encrypted = 0x00004000,
			Write_Through = 0x80000000,
			Overlapped = 0x40000000,
			NoBuffering = 0x20000000,
			RandomAccess = 0x10000000,
			SequentialScan = 0x08000000,
			DeleteOnClose = 0x04000000,
			BackupSemantics = 0x02000000,
			PosixSemantics = 0x01000000,
			OpenReparsePoint = 0x00200000,
			OpenNoRecall = 0x00100000,
			FirstPipeInstance = 0x00080000
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct SP_DEVINFO_DATA
		{
			public uint cbSize;
			public Guid ClassGuid;
			public uint DevInst;
			public IntPtr Reserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct SP_DEVICE_INTERFACE_DATA
		{
			public int cbSize;
			public Guid InterfaceClassGuid;
			public int Flags;
			public IntPtr RESERVED;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
		{
			public UInt32 cbSize;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string DevicePath;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct HIDD_ATTRIBUTES
		{
			public int Size;
			public ushort VendorID;
			public ushort ProductID;
			public short VersionNumber;
		}

		[DllImport(@"hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern void HidD_GetHidGuid(out Guid gHid);

		[DllImport("hid.dll")]
		internal static extern Boolean HidD_GetAttributes(IntPtr HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

		[DllImport("hid.dll")]
		internal extern static bool HidD_SetOutputReport(
			IntPtr HidDeviceObject,
			byte[] lpReportBuffer,
			uint ReportBufferLength);

		[DllImport("hid.dll")]
		internal extern static bool HidD_GetInputReport(
			IntPtr HidDeviceObject,
			byte[] lpREportBuffer,
			uint ReportBufferLength);

		[DllImport("hid.dll")]
		internal extern static bool HidD_GetProductString(
			IntPtr HidDeviceObject,
			byte[] Buffer,
			uint BufferLength);

		[DllImport("hid.dll")]
		internal extern static bool HidD_GetSerialNumberString(
			IntPtr HidDeviceObject, 
			byte[] Buffer, 
			int BufferLength);

		[DllImport("hid.dll")]
		internal extern static bool HidD_GetManufacturerString(
			IntPtr HidDeviceObject,
			byte[] Buffer,
			uint BufferLength);

		[DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr SetupDiGetClassDevs(
			ref Guid ClassGuid,
			[MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
			IntPtr hwndParent,
			UInt32 Flags
			);

		[DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern Boolean SetupDiEnumDeviceInterfaces(
			IntPtr hDevInfo,
			IntPtr devInvo,
			ref Guid interfaceClassGuid,
			UInt32 memberIndex,
			ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
			);

		[DllImport(@"setupapi.dll", SetLastError = true)]
		internal static extern Boolean SetupDiGetDeviceInterfaceDetail(
			IntPtr hDevInfo,
			ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
			IntPtr deviceInterfaceDetailData,
			UInt32 deviceInterfaceDetailDataSize,
			out UInt32 requiredSize,
			IntPtr deviceInfoData
			);

		[DllImport(@"setupapi.dll", SetLastError = true)]
		internal static extern Boolean SetupDiGetDeviceInterfaceDetail(
			IntPtr hDevInfo,
			ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
			ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
			UInt32 deviceInterfaceDetailDataSize,
			out UInt32 requiredSize,
			IntPtr deviceInfoData
			);

		[DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern UInt16 SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern IntPtr CreateFile(
			string fileName,
			[MarshalAs(UnmanagedType.U4)] UInt32 fileAccess,
			[MarshalAs(UnmanagedType.U4)] UInt32 fileShare,
			IntPtr securityAttributes,
			[MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
			[MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
			IntPtr template);

		public const uint GENERIC_READ =  0x80000000;
		public const uint GENERIC_WRITE = 0x40000000;
		public const uint GENERIC_READ_WRITE = GENERIC_READ | GENERIC_WRITE;

		public const uint SHARE_NONE = 0;
		public const uint SHARE_READ = 1;
		public const uint SHARE_WRITE = 2;
		public const uint SHARE_READ_WRITE = SHARE_READ | SHARE_WRITE;
		public const uint SHARE_DELETE = 4;

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern int WriteFile(
			IntPtr hFile,
			byte[] lpBuffer,
			UInt32 bytesToWrite,
			out UInt32 bytesWritten,
			ref System.Threading.NativeOverlapped lpOverlapped);

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool LockFile(
			IntPtr hFile,
			UInt32 dwFileOffsetLow,
			UInt32 dwFileOffsetHigh,
			UInt32 nNumberOfBytesToLockLow,
			UInt32 nNumberOfBytesToLockHigh);

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern int ReadFile(
			IntPtr hFile,
			byte[] lpBuffer,
			UInt32 bytesToRead,
			out UInt32 bytesRead,
			ref System.Threading.NativeOverlapped lpOverlapped);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr hObject);

		[StructLayout(LayoutKind.Sequential)]
		internal struct HIDP_PREPARSED_DATA
		{
			public int cbSize;
			public Guid InterfaceClassGuid;
			public int Flags;
			public IntPtr RESERVED;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct HIDP_CAPS
		{
			public ushort Usage;
			public ushort UsagePage;
			public ushort InputReportByteLength;
			public ushort OutputReportByteLength;
			public ushort FeatureReportByteLength;
			public ushort Reserved0;
			public ushort Reserved1;
			public ushort Reserved2;
			public ushort Reserved3;
			public ushort Reserved4;
			public ushort Reserved5;
			public ushort Reserved6;
			public ushort Reserved7;
			public ushort Reserved8;
			public ushort Reserved9;
			public ushort Reserved10;
			public ushort Reserved11;
			public ushort Reserved12;
			public ushort Reserved13;
			public ushort Reserved14;
			public ushort Reserved15;
			public ushort Reserved16;
			public ushort NumberLinkCollectionNodes;
			public ushort NumberInputButtonCaps;
			public ushort NumberInputValueCaps;
			public ushort NumberInputDataIndices;
			public ushort NumberOutputButtonCaps;
			public ushort NumberOutputValueCaps;
			public ushort NumberOutputDataIndices;
			public ushort NumberFeatureButtonCaps;
			public ushort NumberFeatureValueCaps;
			public ushort NumberFeatureDataIndices;        
		}

		[DllImport("Hid.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern bool HidD_GetPreparsedData(
			IntPtr hFile,
			out IntPtr pp);

		[DllImport("Hid.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern bool HidD_FreePreparsedData(
			IntPtr pp);

		[DllImport("Hid.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern bool HidP_GetCaps(
			IntPtr pp,
			ref HIDP_CAPS caps);


	}
}

