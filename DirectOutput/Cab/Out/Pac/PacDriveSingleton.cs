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

        #region DLL imports

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacSetCallbacks(PAC_ATTACHED_CALLBACK pacAttachedCallback, PAC_REMOVED_CALLBACK pacRemovedCallback);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacInitialize();

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacShutdown();

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacSetLEDStates(int Index, ushort data);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacSetLEDState(int Index, int port, [MarshalAs(UnmanagedType.Bool)] bool state);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDStates(int Index, int group, byte data);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDState(int Index, int group, int port, [MarshalAs(UnmanagedType.Bool)] bool state);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool Pac64SetLEDStatesRandom(int Index);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDIntensities(int Index, byte[] data);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDIntensity(int Index, int port, byte intensity);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDFadeTime(int Index, byte fadeTime);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDFlashSpeeds(int Index, byte flashSpeed);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetLEDFlashSpeed(int Index, int port, byte flashSpeed);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64StartScriptRecording(int Index);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64StopScriptRecording(int Index);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetScriptStepDelay(int Index, byte stepDelay);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64RunScript(int Index);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64ClearFlash(int Index);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Pac64SetDeviceId(int Index, int newId);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacGetDeviceType(int Index);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacGetVendorId(int Index);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacGetProductId(int Index);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int PacGetVersionNumber(int Index);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacGetVendorName(int Index, StringBuilder VendorName);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacGetProductName(int Index, StringBuilder ProductName);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacGetSerialNumber(int Index, StringBuilder SerialNumber);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void PacGetDevicePath(int Index, StringBuilder DevicePath);

        [DllImport("PacDrive.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PacProgramUHID(int Index, StringBuilder FileName);


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


        public bool PacLed64SetLEDStates(int Index, byte[] Data) {
            bool OK=true;
            for (int i = 0; i < 8; i++)
            {
              OK&=Pac64SetLEDStates(Index,i+1,Data[i]);
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
            if (IdPos >= 0)
            {
                IdPos += 8;
            }
            int Id = 0;
            int.TryParse(S.Substring(IdPos,1), out Id);
            return Id;
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
