using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    public class AnalogAlphaValue
    {
        public int Value;
        public int Alpha;

        public void Set(int Value, int Alpha)
        {
            this.Value = Value;
            this.Alpha = Alpha;
        }

        public void Set(int Value)
        {
            this.Value = Value;
            this.Alpha = (Value != 0 ? 255 : 0);
        }

        public AnalogAlphaValue(int Value, int Alpha)
        {
            this.Value = Value;
            this.Alpha = Alpha;
        }

        public AnalogAlphaValue(int Value)
        {
            this.Value = Value;
            this.Alpha = (Value != 0 ? 255 : 0);
        }


    }
}
