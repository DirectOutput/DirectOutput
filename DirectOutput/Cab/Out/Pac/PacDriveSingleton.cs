using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DirectOutput.Cab.Out.Pac
{

    /// <summary>
    /// Singleton version of the PacDrive class found in the PacDrive SDK.
    /// </summary>
    public sealed class PacDriveSingleton
    {
        public enum DeviceType
        {
            Unknown,
            PacDrive,
            UHID,
            PacLED64
        };

        public enum FlashSpeed : byte
        {
            AlwaysOn = 0,
            Seconds_2 = 1,
            Seconds_1 = 2,
            Seconds_0_5 = 3
        };

        #region DLL imports 32-bit

        [DllImport("PacDrive32.dll", EntryPoint = "PacSetCallbacks", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacSetCallbacks32(PAC_ATTACHED_CALLBACK pacAttachedCallback, PAC_REMOVED_CALLBACK pacRemovedCallback);

        [DllImport("PacDrive32.dll", EntryPoint = "PacInitialize", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacInitialize32();

        [DllImport("PacDrive32.dll", EntryPoint = "PacShutdown", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacShutdown32();

        [DllImport("PacDrive32.dll", EntryPoint = "PacSetLEDStates", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacSetLEDStates32(int id, ushort data);

        [DllImport("PacDrive32.dll", EntryPoint = "PacSetLEDState", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacSetLEDState32(int id, int port, [MarshalAs(UnmanagedType.Bool)] bool state);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64SetLEDStates", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDStates32(int id, int group, byte data);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64SetLEDState", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDState32(int id, int group, int port, [MarshalAs(UnmanagedType.Bool)] bool state);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64SetLEDStatesRandom", CallingConvention = CallingConvention.StdCall)]
        private static extern bool Pac64SetLEDStatesRandom32(int id);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64SetLEDIntensities", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDIntensities32(int id, byte[] data);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64SetLEDIntensity", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDIntensity32(int id, int port, byte intensity);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64SetLEDFadeTime", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDFadeTime32(int id, byte fadeTime);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64SetLEDFlashSpeeds", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDFlashSpeeds32(int id, byte flashSpeed);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64SetLEDFlashSpeed", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDFlashSpeed32(int id, int port, byte flashSpeed);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64StartScriptRecording", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64StartScriptRecording32(int id);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64StopScriptRecording", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64StopScriptRecording32(int id);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64SetScriptStepDelay", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetScriptStepDelay32(int id, byte stepDelay);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64RunScript", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64RunScript32(int id);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64ClearFlash", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64ClearFlash32(int id);

        [DllImport("PacDrive32.dll", EntryPoint = "Pac64SetDeviceId", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetDeviceId32(int id, int newId);

        [DllImport("PacDrive32.dll", EntryPoint = "PacGetDeviceType", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacGetDeviceType32(int id);

        [DllImport("PacDrive32.dll", EntryPoint = "PacGetVendorId", CallingConvention = CallingConvention.StdCall)]
		private static extern int PacGetVendorId32(int id);

        [DllImport("PacDrive32.dll", EntryPoint = "PacGetProductId", CallingConvention = CallingConvention.StdCall)]
		private static extern int PacGetProductId32(int id);

        [DllImport("PacDrive32.dll", EntryPoint = "PacGetVersionNumber", CallingConvention = CallingConvention.StdCall)]
		private static extern int PacGetVersionNumber32(int id);

        [DllImport("PacDrive32.dll", EntryPoint = "PacGetVendorName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		private static extern void PacGetVendorName32(int id, StringBuilder vendorName);

        [DllImport("PacDrive32.dll", EntryPoint = "PacGetProductName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		private static extern void PacGetProductName32(int id, StringBuilder productName);

        [DllImport("PacDrive32.dll", EntryPoint = "PacGetSerialNumber", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		private static extern void PacGetSerialNumber32(int id, StringBuilder serialNumber);

        [DllImport("PacDrive32.dll", EntryPoint = "PacGetDevicePath", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		private static extern void PacGetDevicePath32(int id, StringBuilder devicePath);

        [DllImport("PacDrive32.dll", EntryPoint = "PacProgramUHid", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacProgramUHid32(int id, StringBuilder fileName);

        [DllImport("PacDrive32.dll", EntryPoint = "PacSetServoStik4Way", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacSetServoStik4Way32();

        [DllImport("PacDrive32.dll", EntryPoint = "PacSetServoStik8Way", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacSetServoStik8Way32();

        [DllImport("PacDrive32.dll", EntryPoint = "USBButtonConfigurePermanent", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool USBButtonConfigurePermanent32(int id, byte[] data);

        [DllImport("PacDrive32.dll", EntryPoint = "USBButtonConfigureTemporary", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool USBButtonConfigureTemporary32(int id, byte[] data);

        [DllImport("PacDrive32.dll", EntryPoint = "USBButtonConfigureColor", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool USBButtonConfigureColor32(int id, byte red, byte green, byte blue);

        [DllImport("PacDrive32.dll", EntryPoint = "USBButtonGetState", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool USBButtonGetState32(int id, [MarshalAs(UnmanagedType.Bool)] ref bool state);

        #endregion

        #region DLL imports 64-bit

        [DllImport("PacDrive64.dll", EntryPoint = "PacSetCallbacks", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacSetCallbacks64(PAC_ATTACHED_CALLBACK pacAttachedCallback, PAC_REMOVED_CALLBACK pacRemovedCallback);

        [DllImport("PacDrive64.dll", EntryPoint = "PacInitialize", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacInitialize64();

        [DllImport("PacDrive64.dll", EntryPoint = "PacShutdown", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacShutdown64();

        [DllImport("PacDrive64.dll", EntryPoint = "PacSetLEDStates", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacSetLEDStates64(int id, ushort data);

        [DllImport("PacDrive64.dll", EntryPoint = "PacSetLEDState", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacSetLEDState64(int id, int port, [MarshalAs(UnmanagedType.Bool)] bool state);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64SetLEDStates", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDStates64(int id, int group, byte data);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64SetLEDState", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDState64(int id, int group, int port, [MarshalAs(UnmanagedType.Bool)] bool state);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64SetLEDStatesRandom", CallingConvention = CallingConvention.StdCall)]
        private static extern bool Pac64SetLEDStatesRandom64(int id);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64SetLEDIntensities", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDIntensities64(int id, byte[] data);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64SetLEDIntensity", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDIntensity64(int id, int port, byte intensity);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64SetLEDFadeTime", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDFadeTime64(int id, byte fadeTime);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64SetLEDFlashSpeeds", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDFlashSpeeds64(int id, byte flashSpeed);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64SetLEDFlashSpeed", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDFlashSpeed64(int id, int port, byte flashSpeed);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64StartScriptRecording", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64StartScriptRecording64(int id);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64StopScriptRecording", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64StopScriptRecording64(int id);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64SetScriptStepDelay", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetScriptStepDelay64(int id, byte stepDelay);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64RunScript", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64RunScript64(int id);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64ClearFlash", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64ClearFlash64(int id);

        [DllImport("PacDrive64.dll", EntryPoint = "Pac64SetDeviceId", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetDeviceId64(int id, int newId);

        [DllImport("PacDrive64.dll", EntryPoint = "PacGetDeviceType", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacGetDeviceType64(int id);

        [DllImport("PacDrive64.dll", EntryPoint = "PacGetVendorId", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacGetVendorId64(int id);

        [DllImport("PacDrive64.dll", EntryPoint = "PacGetProductId", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacGetProductId64(int id);

        [DllImport("PacDrive64.dll", EntryPoint = "PacGetVersionNumber", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacGetVersionNumber64(int id);

        [DllImport("PacDrive64.dll", EntryPoint = "PacGetVendorName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		private static extern void PacGetVendorName64(int id, StringBuilder vendorName);

        [DllImport("PacDrive64.dll", EntryPoint = "PacGetProductName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		private static extern void PacGetProductName64(int id, StringBuilder productName);

        [DllImport("PacDrive64.dll", EntryPoint = "PacGetSerialNumber", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		private static extern void PacGetSerialNumber64(int id, StringBuilder serialNumber);

        [DllImport("PacDrive64.dll", EntryPoint = "PacGetDevicePath", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		private static extern void PacGetDevicePath64(int id, StringBuilder devicePath);

        [DllImport("PacDrive64.dll", EntryPoint = "PacProgramUHid", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacProgramUHid64(int id, StringBuilder fileName);

        [DllImport("PacDrive64.dll", EntryPoint = "PacSetServoStik4Way", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacSetServoStik4Way64();

        [DllImport("PacDrive64.dll", EntryPoint = "PacSetServoStik8Way", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacSetServoStik8Way64();

        [DllImport("PacDrive64.dll", EntryPoint = "USBButtonConfigurePermanent", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool USBButtonConfigurePermanent64(int id, byte[] data);

        [DllImport("PacDrive64.dll", EntryPoint = "USBButtonConfigureTemporary", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool USBButtonConfigureTemporary64(int id, byte[] data);

        [DllImport("PacDrive64.dll", EntryPoint = "USBButtonConfigureColor", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool USBButtonConfigureColor64(int id, byte red, byte green, byte blue);

        [DllImport("PacDrive64.dll", EntryPoint = "USBButtonGetState", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool USBButtonGetState64(int id, [MarshalAs(UnmanagedType.Bool)] ref bool state);
		

		#endregion

		#region DLL API

		private static void PacSetCallbacks(PAC_ATTACHED_CALLBACK pacAttachedCallback, PAC_REMOVED_CALLBACK pacRemovedCallback)
		{
			if (IntPtr.Size == 8) {
				PacSetCallbacks64(pacAttachedCallback, pacRemovedCallback);
			} else {
				PacSetCallbacks32(pacAttachedCallback, pacRemovedCallback);
			}
		}

		private static int PacInitialize()
		{
			 return IntPtr.Size == 8 ? PacInitialize64() : PacInitialize32();
		}

        private static void PacShutdown()
		{
			if (IntPtr.Size == 8) {
				PacShutdown64();
			} else {
				PacShutdown32();
			}
		}

        private static bool PacSetLEDStates(int Index, ushort data)
		{
			return IntPtr.Size == 8 ? PacSetLEDStates64(Index, data) : PacSetLEDStates32(Index, data);
		}

        private static bool PacSetLEDState(int Index, int port, bool state)
		{
			return IntPtr.Size == 8 ? PacSetLEDState64(Index, port, state) : PacSetLEDState32(Index, port, state);
		}

        private static bool Pac64SetLEDStates(int Index, int group, byte data)
		{
			return IntPtr.Size == 8 ? Pac64SetLEDStates64(Index, group, data) : Pac64SetLEDStates32(Index, group, data);
		}

        private static bool Pac64SetLEDState(int Index, int group, int port, bool state)
		{
			return IntPtr.Size == 8 ? Pac64SetLEDState64(Index, group, port, state) : Pac64SetLEDState32(Index, group, port, state);
		}

        private static bool Pac64SetLEDStatesRandom(int Index)
		{
			return IntPtr.Size == 8 ? Pac64SetLEDStatesRandom64(Index) : Pac64SetLEDStatesRandom32(Index);
		}

        private static bool Pac64SetLEDIntensities(int Index, byte[] data)
		{
			return IntPtr.Size == 8 ? Pac64SetLEDIntensities64(Index, data) : Pac64SetLEDIntensities32(Index, data);
		}

        private static bool Pac64SetLEDIntensity(int Index, int port, byte intensity)
		{
			return IntPtr.Size == 8 ? Pac64SetLEDIntensity64(Index, port, intensity) : Pac64SetLEDIntensity32(Index, port, intensity);
		}

        private static bool Pac64SetLEDFadeTime(int Index, byte fadeTime)
		{
			return IntPtr.Size == 8 ? Pac64SetLEDFadeTime64(Index, fadeTime) : Pac64SetLEDFadeTime32(Index, fadeTime);
		}

        private static bool Pac64SetLEDFlashSpeeds(int Index, byte flashSpeed)
		{
			return IntPtr.Size == 8 ? Pac64SetLEDFlashSpeeds64(Index, flashSpeed) : Pac64SetLEDFlashSpeeds32(Index, flashSpeed);
		}

        private static bool Pac64SetLEDFlashSpeed(int Index, int port, byte flashSpeed)
		{
			return IntPtr.Size == 8 ? Pac64SetLEDFlashSpeed64(Index, port, flashSpeed) : Pac64SetLEDFlashSpeed32(Index, port, flashSpeed);
		}

        private static bool Pac64StartScriptRecording(int Index)
		{
			return IntPtr.Size == 8 ? Pac64StartScriptRecording64(Index) : Pac64StartScriptRecording32(Index);
		}

        private static bool Pac64StopScriptRecording(int Index)
		{
			return IntPtr.Size == 8 ? Pac64StopScriptRecording64(Index) : Pac64StopScriptRecording32(Index);
		}

        private static bool Pac64SetScriptStepDelay(int Index, byte stepDelay)
		{
			return IntPtr.Size == 8 ? Pac64SetScriptStepDelay64(Index, stepDelay) : Pac64SetScriptStepDelay32(Index, stepDelay);
		}

        private static bool Pac64RunScript(int Index)
		{
			return IntPtr.Size == 8 ? Pac64RunScript64(Index) : Pac64RunScript32(Index);
		}

        private static bool Pac64ClearFlash(int Index)
		{
			return IntPtr.Size == 8 ? Pac64ClearFlash64(Index) : Pac64ClearFlash32(Index);
		}

        private static bool Pac64SetDeviceId(int Index, int newId)
		{
			return IntPtr.Size == 8 ? Pac64SetDeviceId64(Index, newId) : Pac64SetDeviceId32(Index, newId);
		}

        private static int PacGetDeviceType(int Index)
		{
			return IntPtr.Size == 8 ? PacGetDeviceType64(Index) : PacGetDeviceType32(Index);
		}

        private static int PacGetVendorId(int Index)
		{
			return IntPtr.Size == 8 ? PacGetVendorId64(Index) : PacGetVendorId32(Index);
		}

        private static int PacGetProductId(int Index)
		{
			return IntPtr.Size == 8 ? PacGetProductId64(Index) : PacGetProductId32(Index);
		}

        private static int PacGetVersionNumber(int Index)
		{
			return IntPtr.Size == 8 ? PacGetVersionNumber64(Index) : PacGetVersionNumber32(Index);
		}

        private static void PacGetVendorName(int Index, StringBuilder VendorName)
		{
			if (IntPtr.Size == 8) {
				PacGetVendorName64(Index, VendorName);
			} else {
				PacGetVendorName32(Index, VendorName);
			}
		}

        private static void PacGetProductName(int Index, StringBuilder ProductName)
		{
			if (IntPtr.Size == 8) {
				PacGetProductName64(Index, ProductName);
			} else {
				PacGetProductName32(Index, ProductName);
			}
		}

        private static void PacGetSerialNumber(int Index, StringBuilder SerialNumber)
		{
			if (IntPtr.Size == 8) {
				PacGetSerialNumber64(Index, SerialNumber);
			} else {
				PacGetSerialNumber32(Index, SerialNumber);
			}
		}

        private static void PacGetDevicePath(int Index, StringBuilder DevicePath)
		{
			if (IntPtr.Size == 8) {
				PacGetDevicePath64(Index, DevicePath);
			} else {
				PacGetDevicePath32(Index, DevicePath);
			}
		}

        private static bool PacProgramUHID(int Index, StringBuilder FileName)
		{
			return IntPtr.Size == 8 ? PacProgramUHid64(Index, FileName) : PacProgramUHid32(Index, FileName);
		}

        #endregion

        private delegate void PAC_ATTACHED_CALLBACK(int Index);
        private delegate void PAC_REMOVED_CALLBACK(int Index);

        public delegate void PacAttachedDelegate(int Index);
        public delegate void PacRemovedDelegate(int Index);

        public event PacAttachedDelegate OnPacAttached = null;
        public event PacRemovedDelegate OnPacRemoved = null;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        PAC_ATTACHED_CALLBACK PacAttachedCallbackPtr = null;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        PAC_REMOVED_CALLBACK PacRemovedCallbackPtr = null;


        private int m_numDevices = 0;


        private static readonly Lazy<PacDriveSingleton> lazy = new Lazy<PacDriveSingleton>(() => new PacDriveSingleton());

        public static PacDriveSingleton Instance { get { return lazy.Value; } }


        private PacDriveSingleton()
        {


            PacAttachedCallbackPtr = new PAC_ATTACHED_CALLBACK(PacAttachedCallback);
            PacRemovedCallbackPtr = new PAC_REMOVED_CALLBACK(PacRemovedCallback);

            PacSetCallbacks(PacAttachedCallbackPtr, PacRemovedCallbackPtr);
            Initialize();
        }

        private void PacAttachedCallback(int Index)
        {
            m_numDevices++;

            if (OnPacAttached != null)
                OnPacAttached(Index);
        }

        private void PacRemovedCallback(int Index)
        {
            m_numDevices--;

            if (OnPacRemoved != null)
                OnPacRemoved(Index);
        }

        private void Initialize()
        {
            m_numDevices = PacInitialize();


        }



        public void Shutdown()
        {
            PacShutdown();
        }

        #region PacDrive & UHID Led methods

        public bool PacDriveUHIDSetLEDStates(int Index, ushort Data)
        {
            return PacSetLEDStates(Index, Data);
        }

        public bool PacDriveUHIDSetLEDState(int Index, int Port, bool State)
        {
            return PacSetLEDState(Index, Port, State);
        }

        public bool PacDriveUHIDSetLEDStates(int Index, bool[] Data)
        {
            ushort dataSend = 0;

            for (int i = 0; i < Data.Length; i++)
                if (Data[i]) dataSend |= (ushort)(1 << i);

            return PacSetLEDStates(Index, dataSend);
        }
        #endregion

        #region PacLed64 methods
        public bool PacLed64SetLEDState(int Index, int Group, int Port, bool State)
        {
            return Pac64SetLEDState(Index, Group, Port, State);
        }

        public bool PacLed64SetLEDStates(int Index, int Group, byte Data)
        {
            return Pac64SetLEDStates(Index, Group, Data);
        }


        public bool PacLed64SetLEDStates(int Index, byte[] Data)
        {
            bool OK = true;
            for (int i = 0; i < 8; i++)
            {
                OK &= Pac64SetLEDStates(Index, i + 1, Data[i]);
            }
            return OK;
        }

        public bool PacLed64SetLEDStates(int Index, int Group, bool[] Data)
        {
            byte dataSend = 0;

            for (int i = 0; i < Data.Length; i++)
                if (Data[i]) dataSend |= (byte)(1 << i);

            return Pac64SetLEDStates(Index, Group, dataSend);
        }

        public bool PacLed64SetLEDStatesRandom(int Index)
        {
            return Pac64SetLEDStatesRandom(Index);
        }

        public bool PacLed64SetLEDIntensities(int Index, byte[] Data)
        {
            return Pac64SetLEDIntensities(Index, Data);
        }

        public bool PacLed64SetLEDIntensity(int Index, int Port, byte Intensity)
        {
            return Pac64SetLEDIntensity(Index, Port, Intensity);
        }

        public bool PacLed64SetLEDFadeTime(int Index, byte FadeTime)
        {
            return Pac64SetLEDFadeTime(Index, FadeTime);
        }

        public bool PacLed64SetLEDFlashSpeeds(int Index, FlashSpeed FlashSpeed)
        {
            return Pac64SetLEDFlashSpeeds(Index, (byte)FlashSpeed);
        }

        public bool PacLed64SetLEDFlashSpeed(int Index, int Port, FlashSpeed FlashSpeed)
        {
            return Pac64SetLEDFlashSpeed(Index, Port, (byte)FlashSpeed);
        }

        public bool PacLed64StartScriptRecording(int Index)
        {
            return Pac64StartScriptRecording(Index);
        }

        public bool PacLed64StopScriptRecording(int Index)
        {
            return Pac64StopScriptRecording(Index);
        }

        public bool PacLed64SetScriptStepDelay(int Index, byte StepDelay)
        {
            return Pac64SetScriptStepDelay(Index, StepDelay);
        }

        public bool PacLed64RunScript(int Index)
        {
            return Pac64RunScript(Index);
        }

        public bool PacLed64ClearFlash(int Index)
        {
            return Pac64ClearFlash(Index);
        }

        public bool PacLed64SetDeviceId(int Index, int NewId)
        {
            return Pac64SetDeviceId(Index, NewId);
        }

        public int PacLed64GetDeviceId(int Index)
        {
            string S = GetDevicePath(Index);
            //\\?\hid#vid_d209&pid_1402#7&f3bb0d5&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}

            int IdPos = S.IndexOf("&pid_140");
  
            if (IdPos <=0)
            {
                return -1;
            }
            IdPos += 8;
            int Id = 0;
            if (int.TryParse(S.Substring(IdPos, 1), out Id))
            {
                return Id;
            }
            return -1;
        }


        public int PacLed64GetIndexForDeviceId(int Id)
        {

            for (int i = 0; i < NumDevices; i++)
            {
                if (PacLed64GetDeviceId(i) == Id)
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion

        public DeviceType GetDeviceType(int Index)
        {
            return (DeviceType)PacGetDeviceType(Index);
        }

        public int GetVendorId(int Index)
        {
            return PacGetVendorId(Index);
        }

        public int GetProductId(int Index)
        {
            return PacGetProductId(Index);
        }

        public int GetVersionNumber(int Index)
        {
            return PacGetVersionNumber(Index);
        }

        public string GetVendorName(int Index)
        {
            StringBuilder sb = new StringBuilder(256);

            PacGetVendorName(Index, sb);

            return sb.ToString();
        }

        public string GetProductName(int Index)
        {
            StringBuilder sb = new StringBuilder(256);

            PacGetProductName(Index, sb);

            return sb.ToString();
        }

        public string GetSerialNumber(int Index)
        {
            StringBuilder sb = new StringBuilder(256);

            PacGetSerialNumber(Index, sb);

            return sb.ToString();
        }

        public string GetDevicePath(int Index)
        {
            StringBuilder sb = new StringBuilder(256);

            PacGetDevicePath(Index, sb);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the Ids of the PacLed64 controllers which are connected to the system.
        /// </summary>
        /// <returns>List of PacLed64 Ids</returns>
        public List<int> PacLed64GetIdList()
        {
            List<int> L = new List<int>();


            for (int i = 0; i < NumDevices; i++)
            {
                if (GetDeviceType(i) == DeviceType.PacLED64)
                {
                    L.Add(PacLed64GetDeviceId(i));
                }
            }


            return L;
        }


        /// <summary>
        /// Gets the Ids of the first PacDrive controller which is connected to the system.
        /// </summary>
        /// <returns>List of PacDrive Ids</returns>
        public int PacDriveGetIndex()
        {
            List<int> L = new List<int>();


            for (int i = 0; i < NumDevices; i++)
            {
                if (GetDeviceType(i) == DeviceType.PacDrive)
                {
                    return i;
                }
            }


            return -1;
        }



        /// <summary>
        /// Gets the count of attached devices
        /// </summary>
        /// <value>
        /// The count of devices.
        /// </value>
        public int NumDevices
        {
            get { return m_numDevices; }
        }
    }
}
