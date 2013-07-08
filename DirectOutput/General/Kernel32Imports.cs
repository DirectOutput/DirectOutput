using System.Runtime.InteropServices;
using System;

namespace DirectOutput.General
{
    /// <summary>
    /// This class contains import of the kernel32.dll<br/>
    /// </summary>
    public class Kernel32Imports
    {
        /// <summary>
        /// Gets the current processor number.<br/>
        /// 
        /// </summary>
        /// <returns>The current processor number.</returns>
        /// TODO: Find a way to get current processor number if running on xp.
        [DllImport("kernel32.dll")]
        public extern static int GetCurrentProcessorNumber();


        private static bool GetCurrentProcessorNumberIsAvailableResult = false;
        private static bool GetCurrentProcessorNumberIsAvailableChecked=false;
        private static object GetCurrentProcessorNumberIsAvailableLocker = new object();
        /// <summary>
        /// Detects if the GetCurrentProcessorNumber call is working on the current system.
        /// </summary>
        /// <returns><c>true</c> if the function is available, otherwise <c>false</c></returns>
        public static bool GetCurrentProcessorNumberIsAvailable
        {
            get
            {
                lock (GetCurrentProcessorNumberIsAvailableLocker)
                {
                    if (GetCurrentProcessorNumberIsAvailableChecked) return GetCurrentProcessorNumberIsAvailableResult;
                    try
                    {
                        Kernel32Imports.GetCurrentProcessorNumber();
                        GetCurrentProcessorNumberIsAvailableResult = true;
                    }
                    catch (Exception)
                    {
                        GetCurrentProcessorNumberIsAvailableResult = false;
                    }
                }
                return GetCurrentProcessorNumberIsAvailableResult;
            }
        }

    }
}
