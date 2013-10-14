using System;
namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// Common interface for toys supporting analog alpha layers.
    /// </summary>
    public interface IAnalogAlphaToy
    {
        /// <summary>
        /// Finishes the toy.
        /// </summary>
        void Finish();
        /// <summary>
        /// Initializes the toy.
        /// </summary>
        /// <param name="Cabinet">The cabinet object containing the toy.</param>
        void Init(DirectOutput.Cab.Cabinet Cabinet);
        /// <summary>
        /// Gets the dictionary of AnalogAlphaLayers.
        /// </summary>
        /// <value>
        /// The layers dictionary.
        /// </value>
        AnalogLayerDictionary Layers { get; }

        /// <summary>
        /// Gets or sets the name of the output of the toy.
        /// </summary>
        /// <value>
        /// The name of the output.
        /// </value>
        //TODO: CHeck if this property should really be part of this interface.
        string OutputName { get; set; }
        /// <summary>
        /// Resets the toy.
        /// </summary>
        void Reset();

    }
}
