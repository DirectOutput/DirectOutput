using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    public struct AnalogAlphaData
    {
        /// <summary>
        /// The analog value (0-255).
        /// </summary>
        public int Value;
        /// <summary>
        /// The alpha value for the value (0-255).
        /// </summary>
        public int Alpha;
    }
}
