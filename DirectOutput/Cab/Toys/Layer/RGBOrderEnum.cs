using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// Enum used to define the order of the colors of a multicolor element (e.g. RGB led).
    /// Depending on the connection of the multi color element, the order of the colors does maybe not match the default Red - Green - Blue order (e.g. addressable WS2812 led chips are using Green - Red - Blue).
    /// </summary>
    public enum RGBOrderEnum
    {
        /// <summary>
        /// Red-Green-Blue (usual color order)
        /// </summary>
        RGB = 1,
        /// <summary>
        /// Red - Blue - Green
        /// </summary>
        RBG = 2,
        /// <summary>
        /// Green - Red - Blue (WS2812 led chips are using the scheme)
        /// </summary>
        GRB = 3,
        /// <summary>
        /// WS2812 led chip (uses green - red - blue color order)
        /// </summary>
        WS2812 = 3,
        /// <summary>
        /// Green - Blue - Red
        /// </summary>
        GBR = 4,
        /// <summary>
        /// Green - Blue - Red
        /// </summary>
        BRG = 5,
        /// <summary>
        /// Blue - Green - Red
        /// </summary>
        BGR = 6
    }
}
