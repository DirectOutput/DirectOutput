using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DirectOutput.Cab.Out.Pac
{
   
    class PacDrive
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
        
        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacSetCallbacks(PAC_ATTACHED_CALLBACK pacAttachedCallback, PAC_REMOVED_CALLBACK pacRemovedCallback);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacInitialize();

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacShutdown();

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacSetLEDStates(int id, ushort data);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacSetLEDState(int id, int port, [MarshalAs(UnmanagedType.Bool)] bool state);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDStates(int id, int group, byte data);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDState(int id, int group, int port, [MarshalAs(UnmanagedType.Bool)] bool state);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool Pac64SetLEDStatesRandom(int id);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDIntensities(int id, byte[] data);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDIntensity(int id, int port, byte intensity);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDFadeTime(int id, byte fadeTime);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDFlashSpeeds(int id, byte flashSpeed);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDFlashSpeed(int id, int port, byte flashSpeed);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64StartScriptRecording(int id);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64StopScriptRecording(int id);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetScriptStepDelay(int id, byte stepDelay);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64RunScript(int id);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64ClearFlash(int id);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetDeviceId(int id, int newId);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacGetDeviceType(int id);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacGetVendorId(int Id);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacGetProductId(int Id);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacGetVersionNumber(int Id);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacGetVendorName(int Id, StringBuilder VendorName);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacGetProductName(int Id, StringBuilder ProductName);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacGetSerialNumber(int Id, StringBuilder SerialNumber);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacGetDevicePath(int Id, StringBuilder DevicePath);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacProgramUHid(int id, StringBuilder FileName);

        private delegate void PAC_ATTACHED_CALLBACK(int id);
        private delegate void PAC_REMOVED_CALLBACK(int id);

        public delegate void PacAttachedDelegate(int id);
        public delegate void PacRemovedDelegate(int id);

        public event PacAttachedDelegate OnPacAttached = null;
        public event PacRemovedDelegate OnPacRemoved = null;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        PAC_ATTACHED_CALLBACK PacAttachedCallbackPtr = null;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        PAC_REMOVED_CALLBACK PacRemovedCallbackPtr = null;

        private Control m_ctrl;
        private int m_numDevices = 0;

        public PacDrive(Control ctrl)
        {
            m_ctrl = ctrl;

            PacAttachedCallbackPtr = new PAC_ATTACHED_CALLBACK(PacAttachedCallback);
            PacRemovedCallbackPtr = new PAC_REMOVED_CALLBACK(PacRemovedCallback);

            PacSetCallbacks(PacAttachedCallbackPtr, PacRemovedCallbackPtr);
        }

        void PacAttachedCallback(int id)
        {
            m_numDevices++;

            if (OnPacAttached != null)
                m_ctrl.BeginInvoke(OnPacAttached, id);
        }

        void PacRemovedCallback(int id)
        {
            m_numDevices--;

            if (OnPacRemoved != null)
                m_ctrl.BeginInvoke(OnPacRemoved, id);
        }

        public int Initialize()
        {
            m_numDevices = PacInitialize();

            return m_numDevices;
        }

        public void Shutdown()
        {
            PacShutdown();
        }

        public bool SetLEDStates(int Id, ushort Data)
        {
            return PacSetLEDStates(Id, Data);
        }

        public bool SetLEDState(int Id, int Port, bool State)
        {
            return PacSetLEDState(Id, Port, State);
        }

        public bool SetLEDStates(int Id, bool[] Data)
        {
            ushort dataSend = 0;

            for (int i = 0; i < Data.Length; i++)
                if (Data[i]) dataSend |= (ushort)(1 << i);

            return PacSetLEDStates(Id, dataSend);
        }

        public bool SetLED64State(int id, int Group, int Port, bool State)
        {
            return Pac64SetLEDState(id, Group, Port, State);
        }

        public bool SetLED64States(int Id, int Group, byte Data)
        {
            return Pac64SetLEDStates(Id, Group, Data);
        }



        public bool SetLED64States(int Id, int Group, bool[] Data)
        {
            byte dataSend = 0;

            for (int i = 0; i < Data.Length; i++)
                if (Data[i]) dataSend |= (byte)(1 << i);

            return Pac64SetLEDStates(Id, Group, dataSend);
        }

        public bool SetLED64StatesRandom(int Id)
        {
            return Pac64SetLEDStatesRandom(Id);
        }

        public bool SetLED64Intensities(int Id, byte[] Data)
        {
            return Pac64SetLEDIntensities(Id, Data);
        }

        public bool SetLED64Intensity(int Id, int Port, byte Intensity)
        {
            return Pac64SetLEDIntensity(Id, Port, Intensity);
        }

        public bool SetLED64FadeTime(int Id, byte FadeTime)
        {
            return Pac64SetLEDFadeTime(Id, FadeTime);
        }

        public bool SetLED64FlashSpeeds(int Id, FlashSpeed FlashSpeed)
        {
            return Pac64SetLEDFlashSpeeds(Id, (byte)FlashSpeed);
        }

        public bool SetLED64FlashSpeed(int Id, int Port, FlashSpeed FlashSpeed)
        {
            return Pac64SetLEDFlashSpeed(Id, Port, (byte)FlashSpeed);
        }

        public bool StartScriptRecording(int Id)
        {
            return Pac64StartScriptRecording(Id);
        }

        public bool StopScriptRecording(int Id)
        {
            return Pac64StopScriptRecording(Id);
        }

        public bool SetScriptStepDelay(int Id, byte StepDelay)
        {
            return Pac64SetScriptStepDelay(Id, StepDelay);
        }

        public bool RunScript(int Id)
        {
            return Pac64RunScript(Id);
        }

        public bool ClearFlash(int Id)
        {
            return Pac64ClearFlash(Id);
        }

        public bool SetDeviceId(int Id, int NewId)
        {
            return Pac64SetDeviceId(Id, NewId);
        }

        public DeviceType GetDeviceType(int Id)
        {
            return (DeviceType) PacGetDeviceType(Id);
        }

        public int GetVendorId(int Id)
        {
            return PacGetVendorId(Id);
        }

        public int GetProductId(int Id)
        {
            return PacGetProductId(Id);
        }

        public int GetVersionNumber(int Id)
        {
            return PacGetVersionNumber(Id);
        }

        public string GetVendorName(int Id)
        {
            StringBuilder sb = new StringBuilder(256);

            PacGetVendorName(Id, sb);

            return sb.ToString();
        }

        public string GetProductName(int Id)
        {
            StringBuilder sb = new StringBuilder(256);

            PacGetProductName(Id, sb);

            return sb.ToString();
        }

        public string GetSerialNumber(int Id)
        {
            StringBuilder sb = new StringBuilder(256);

            PacGetSerialNumber(Id, sb);

            return sb.ToString();
        }

        public string GetDevicePath(int Id)
        {
            StringBuilder sb = new StringBuilder(256);

            PacGetDevicePath(Id, sb);

            return sb.ToString();
        }

        public bool ProgramUHid(int Id, string FileName)
        {
            StringBuilder sb = new StringBuilder(FileName);

            return PacProgramUHid(Id, sb);
        }

        public int NumDevices
        {
            get { return m_numDevices; }
        }
    }
}
