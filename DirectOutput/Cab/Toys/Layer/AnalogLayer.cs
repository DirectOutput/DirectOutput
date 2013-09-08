using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    public class AnalogLayer
    {
        public int Value;
        public int Alpha;

        public void SetLayer(int Value)
        {

            this.Value = Value;
            this.Alpha = (Value > 0 ? 255 : 0);
        }

        public void SetLayer(int Value, int Alpha)
        {
            this.Value = Value;
            this.Alpha = Alpha;
        }

        public AnalogLayer() { }
        public AnalogLayer(int Value) { SetLayer(Value); }
        public AnalogLayer(int Value, int Alpha) { SetLayer(Value, Alpha); }


    }


}
