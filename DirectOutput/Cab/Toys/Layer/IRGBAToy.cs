using System;
namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// Common interface for RGB toys supporting several layers of color with alpha value.
    /// </summary>
    public interface IRGBAToy
    {
        /// <summary>
        /// Finishes the toy.
        /// </summary>
        void Finish();
        /// <summary>
        /// Inits the the toy.
        /// </summary>
        /// <param name="Cabinet">The cabinet to which the toy belongs.</param>
        void Init(Cabinet Cabinet);

        /// <summary>
        /// Gets the dictionary of RGBALayers.
        /// </summary>
        /// <value>
        /// The layers dictionary.
        /// </value>
        RGBALayerDictionary Layers { get; }
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
        /// <summary>
        /// Resets the toy.
        /// </summary>
        void Reset();

    }
}
