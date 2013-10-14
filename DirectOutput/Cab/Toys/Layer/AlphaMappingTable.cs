using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// Static class containg a mapping table which is used for faster alpha mapping of toys supporting layers with alpha channels.
    /// </summary>
    public static class AlphaMappingTable
    {
        /// <summary>
        /// The alpha mapping table.
        /// </summary>
        public static float[,] AlphaMapping = new float[256, 256];


        static AlphaMappingTable()
        {
            for (int A = 0; A < 256; A++)
            {
                for (int V = 0; V < 256; V++)
                {
                    AlphaMapping[A, V] = (float)((double)V / 255 * (double)A);
                }
            }

        }
    }
}
