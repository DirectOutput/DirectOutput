using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// A dictionary for AnalogAlphaLayer objects.
    /// </summary>
    public class AnalogLayerDictionary : SortedDictionary<int, AnalogAlphaLayer>
    {

        /// <summary>
        /// Gets or sets the <see cref="AnalogAlphaLayer"/> with the specified layer number.
        /// If a layer does not exist for the specified number, a new layer will be created for the LayerNr.
        /// </summary>
        /// <value>
        /// The <see cref="AnalogAlphaLayer"/> for the specified LayerNr.
        /// </value>
        /// <param name="LayerNr">The number of the layer.</param>
        /// <returns></returns>
        public new AnalogAlphaLayer this[int LayerNr]
        {
            get
            {
                try
                {
                    return base[LayerNr];
                }
                catch
                {
                    AnalogAlphaLayer L = new AnalogAlphaLayer();
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
        /// Gets the analog value which results from the analog values and alpha values in the dirctionary.
        /// </summary>
        /// <returns>A analog value.</returns>
        public int GetResultingValue()
        {
            if (Count > 0)
            {
                float Value = 0;

                foreach (KeyValuePair<int, AnalogAlphaLayer> KV in this)
                {
                    int Alpha = KV.Value.Alpha ;
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
