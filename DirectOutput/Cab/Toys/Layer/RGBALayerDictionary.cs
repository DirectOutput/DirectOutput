using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Color;

namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// Dictionary for RGBALayer objects.
    /// </summary>
    public class RGBALayerDictionary : SortedDictionary<int, RGBALayer>
    {
        /// <summary>
        /// Gets or sets the <see cref="RGBALayer"/> for the specified layer nr.<br/>
        /// If no layer exists for the specified number, a new layer will be created for the layer nr.
        /// </summary>
        /// <value>
        /// The <see cref="RGBALayer"/> with the specified layer nr.
        /// </value>
        /// <param name="LayerNr">The layer nr.</param>
        /// <returns></returns>
        public new RGBALayer this[int LayerNr]
        {
            get
            {
                try
                {
                    return base[LayerNr];
                }
                catch
                {
                    RGBALayer L = new RGBALayer();
                    Add(LayerNr, L);
                    return L;
                }
            }
            set
            {
                base[LayerNr] = value;
            }
        }


        /// <summary>
        /// Get the RGBColor resulting from the colors and alpha values in the layers.
        /// </summary>
        /// <returns></returns>
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
