using System;
using DirectOutput.Cab.Toys.Layer;
namespace DirectOutput.Cab.Toys
{
    /// <summary>
    /// Common interface for toys supporting analog alpha layers.
    /// </summary>
    public interface IAnalogAlphaToy : IToy, ILayerToy<AnalogAlphaData>
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
        LayerDictionary<AnalogAlphaData> Layers { get; }


        /// <summary>
        /// Resets the toy.
        /// </summary>
        void Reset();

    }
}
