using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    public class AnalogLayerDictionary : SortedDictionary<int, AnalogLayer>
    {

        public new AnalogLayer this[int key]
        {
            get
            {
                try
                {
                    return base[key];
                }
                catch
                {
                    AnalogLayer L = new AnalogLayer();
                    Add(key, L);
                    return L;
                }
            }
            set
            {
                base[key] = value;
            }
        }


        public AnalogLayer SetLayer(int Layer, int Value)
        {
            AnalogLayer L;

            try
            {
                L = this[Layer];
            }
            catch
            {
                L = new AnalogLayer();
                Add(Layer, L);
            }
            L.SetLayer(Value);

            return L;
        }




        public AnalogLayer SetLayer(int Layer, int Value, int Alpha)
        {
            AnalogLayer L;

            try
            {
                L = this[Layer];
            }
            catch
            {
                L = new AnalogLayer();
                Add(Layer, L);
            }
            L.SetLayer(Value, Alpha);

            return L;
        }

        public AnalogLayer SetLayer(int Layer, AnalogAlphaValue AnalogAlphaValue)
        {
            AnalogLayer L;

            try
            {
                L = this[Layer];
            }
            catch
            {
                L = new AnalogLayer();
                Add(Layer, L);
            }
            L.SetLayer(AnalogAlphaValue);

            return L;
        }


        public int GetResultingValue()
        {
            if (Count > 0)
            {
                float Value = 0;

                foreach (KeyValuePair<int, AnalogLayer> KV in this)
                {
                    int Alpha = KV.Value.Value ;
                    if (Alpha != 0)
                    {
                        Value = AlphaMappingTable.AlphaMapping[255 - Alpha, (int)Value] + AlphaMappingTable.AlphaMapping[Alpha, KV.Value.Value];
                    }
                }

                return (int)Value;
            }
            else
            {
                return 0;
            }
        }



    }

}
