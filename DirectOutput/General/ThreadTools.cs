using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DirectOutput.General
{
    public class ThreadTools
    {
        [DllImport("kernel32.dll")]
        public extern static int GetCurrentProcessorNumber();

        


    }
}
