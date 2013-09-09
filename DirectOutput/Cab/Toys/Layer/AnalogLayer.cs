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

        public void SetLayer(AnalogAlphaValue AnalogAlphaValue)
        {
            this.Value = AnalogAlphaValue.Value;
            this.Alpha = AnalogAlphaValue.Alpha;
        }
       

        public void SetLayer(int Value)
        {
            this.Value=Value;
        }

        public void SetLayer(int Value, int Alpha)
        {
            this.Value=Value;
            this.Alpha=Alpha;
        }


        public AnalogAlphaValue GetAnalogAlphaValue() {
            return new AnalogAlphaValue(Value, Alpha);
        }

        public AnalogLayer() { }
        public AnalogLayer(int Value) { SetLayer(Value); }
        public AnalogLayer(int Value, int Alpha) { SetLayer(Value, Alpha); }
        public  AnalogLayer(AnalogAlphaValue AnalogAlphaValue) {SetLayer(AnalogAlphaValue);}


    }


}
