using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    public class AnalogAlphaLayer
    {

        public int Value;
        public int Alpha;

        public void Set(AnalogAlphaValue AnalogAlphaValue)
        {
            this.Value = AnalogAlphaValue.Value;
            this.Alpha = AnalogAlphaValue.Alpha;
        }
       

        public void Set(int Value)
        {
            this.Value=Value;
            this.Alpha = (Value == 0 ? 0 : 255);
        }

        public void Set(int Value, int Alpha)
        {
            this.Value=Value;
            this.Alpha=Alpha;
        }


        public AnalogAlphaValue GetAnalogAlphaValue() {
            return new AnalogAlphaValue(Value, Alpha);
        }

        public AnalogAlphaLayer() { }
        public AnalogAlphaLayer(int Value) { Set(Value); }
        public AnalogAlphaLayer(int Value, int Alpha) { Set(Value, Alpha); }
        public  AnalogAlphaLayer(AnalogAlphaValue AnalogAlphaValue) {Set(AnalogAlphaValue);}


    }


}
