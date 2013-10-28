using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    public class LedStripLayerDictionary : SortedDictionary<int, LedStripLayer>
    {

        public new LedStripLayer this[int LayerNr]
        {
            get
            {
                try
                {
                    return base[LayerNr];
                }
                catch
                {
                    LedStripLayer L = new LedStripLayer();
                    L.NumberOfLeds = Parent.NumberOfLeds;
                    Add(LayerNr, L);
                    return L;
                }
            }
            set
            {
                base[LayerNr] = value;
            }
        }

        private LedStrip Parent;



        public int[,] GetResultingValue()
        {
            if (Count > 0)
            {
                float[,] Value = new float[Parent.NumberOfLeds, 3];



                foreach (KeyValuePair<int, LedStripLayer> KV in this)
                {
                    int[,] D = KV.Value.RGBALedData;

                    for (int i = 0; i < Parent.NumberOfLeds; i++)
                    {
                        int Alpha = D[i, 3];
                        if (Alpha != 0)
                        {
                            Value[i, 0] = AlphaMappingTable.AlphaMapping[255 - Alpha, (int)Value[i, 0]] + AlphaMappingTable.AlphaMapping[Alpha, D[i, 0]];
                            Value[i, 1] = AlphaMappingTable.AlphaMapping[255 - Alpha, (int)Value[i, 1]] + AlphaMappingTable.AlphaMapping[Alpha, D[i, 1]];
                            Value[i, 2] = AlphaMappingTable.AlphaMapping[255 - Alpha, (int)Value[i, 2]] + AlphaMappingTable.AlphaMapping[Alpha, D[i, 2]];
                        }
                    }
                }
                int[,] Result = new int[Parent.NumberOfLeds, 3]; 
                for (int i = 0; i < Parent.NumberOfLeds; i++)
                {
                    Result[i, 0] = (int)Value[i, 0];
                    Result[i, 1] = (int)Value[i, 1];
                    Result[i, 2] = (int)Value[i, 2];
                }
                return Result;

            }
            else
            {
                return new int[0, 3];
            }
        }

    }
}
