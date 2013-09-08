using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    public class AnalogLayerDictionary : SortedDictionary<int, AnalogLayer>
    {
        public void SetLayer(int Layer, int Value)
        {
            this[Layer].SetLayer(Value);
        }

        public void SetLayer(int Layer, int Value, int Alpha)
        {
            this[Layer].SetLayer(Value,Alpha);
        }



        public int GetResultingValue()
        {
            if (Count > 0)
            {
                float Value = 0;

                foreach (KeyValuePair<int, AnalogLayer> KV in this)
                {
                    int Alpha = KV.Value.Alpha;
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
