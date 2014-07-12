using System;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Color;
namespace DirectOutput.Cab.Toys
{
    /// <summary>
    /// Common interface for RGB toys supporting several layers of color with alpha value.
    /// </summary>
    public interface IRGBOutputToy: IToy
    {
        /// <summary>
        /// Gets or sets the output name for blue.
        /// </summary>
        /// <value>
        /// The output name for blue.
        /// </value>
        string OutputNameBlue { get; set; }
        /// <summary>
        /// Gets or sets the output name for green.
        /// </summary>
        /// <value>
        /// The output name for green.
        /// </value>
        string OutputNameGreen { get; set; }
        /// <summary>
        /// Gets or sets the output name for red.
        /// </summary>
        /// <value>
        /// The output name for red.
        /// </value>
        string OutputNameRed { get; set; }


    }
}
