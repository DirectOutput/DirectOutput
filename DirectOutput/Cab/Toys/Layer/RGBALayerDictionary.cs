using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    public class RGBALayerDictionary : SortedDictionary<int, RGBALayer>
    {

        public RGBALayer SetLayer(int Layer, int Red, int Green, int Blue)
        {
            RGBALayer L = null;
            if (ContainsKey(Layer))
            {
                this[Layer].SetLayer(Red, Green, Blue);
            }
            else
            {
                L = new RGBALayer(Red, Green, Blue);
                Add(Layer, L);
            }
            return L;
        }

        public RGBALayer SetLayer(int Layer, int Red, int Green, int Blue, int Alpha)
        {
            RGBALayer L = null;
            if (ContainsKey(Layer))
            {
                this[Layer].SetLayer(Red, Green, Blue, Alpha);
            }
            else
            {
                L = new RGBALayer(Red, Green, Blue, Alpha);
                Add(Layer, L);
            }
            return L;
        }

        public RGBALayer SetLayer(int Layer, RGBAColor RGBA)
        {
            RGBALayer L = null;
            if (ContainsKey(Layer))
            {
                this[Layer].SetLayer(RGBA);
            }
            else
            {
                L = new RGBALayer(RGBA);
                Add(Layer, L);
            }
            return L;
        }


        public RGBALayer SetLayer(int Layer, RGBColor RGB)
        {
            RGBALayer L = null;
            if (ContainsKey(Layer))
            {
                this[Layer].SetLayer(RGB);
            }
            else
            {
                L = new RGBALayer(RGB);
                Add(Layer, L);
            }
            return L;
        }



        public RGBColor GetResultingColor()
        {
            if (Count > 0)
            {
                float Red = 0;
                float Green = 0;
                float Blue = 0;
                foreach (KeyValuePair<int, RGBALayer> KV in this)
                {
                    int Alpha = KV.Value.Alpha;
                    if (Alpha != 0)
                    {
                        int NegAlpha = 255 - Alpha;
                        Red = AlphaMappingTable.AlphaMapping[NegAlpha, (int)Red] + AlphaMappingTable.AlphaMapping[Alpha, KV.Value.Red];
                        Green = AlphaMappingTable.AlphaMapping[NegAlpha, (int)Green] + AlphaMappingTable.AlphaMapping[Alpha, KV.Value.Green];
                        Blue = AlphaMappingTable.AlphaMapping[NegAlpha, (int)Blue] + AlphaMappingTable.AlphaMapping[Alpha, KV.Value.Blue];
                    }
                }

                return new RGBColor((int)Red, (int)Green, (int)Blue);
            }
            else
            {
                return new RGBColor(0, 0, 0);
            }
        }
    }
}
