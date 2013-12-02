using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    public class LedStripLayerDictionary : SortedDictionary<int, RGBAData[,]>
    {

        public new RGBAData[,] this[int LayerNr]
        {
            get
            {
                try
                {
                    return base[LayerNr];
                }
                catch
                {
                    RGBAData[,] L = new RGBAData[Parent.Width, Parent.Height];

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



    }
}
