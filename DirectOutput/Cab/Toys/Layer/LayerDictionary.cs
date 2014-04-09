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
    public class LayerDictionary<LayerElementType> : SortedDictionary<int, LayerElementType>
        where LayerElementType : struct
    {
        /// <summary>
        /// Gets or sets the data for the specified layer nr.<br/>
        /// If no layer exists for the specified number, a new layer will be created for the layer nr.
        /// </summary>
        /// <value>
        /// The struct with the specified layer nr.
        /// </value>
        /// <param name="LayerNr">The layer nr.</param>
        /// <returns>Struct with the specified layer nr.</returns>
        public new LayerElementType this[int LayerNr]
        {
            get
            {
                try
                {
                    return base[LayerNr];
                }
                catch
                {
                    LayerElementType L = new LayerElementType();
                    Add(LayerNr, L);
                    return L;
                }
            }
            set
            {
                base[LayerNr] = value;
            }
        }




    }
}
