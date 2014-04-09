using System;
namespace DirectOutput.Cab.Toys.Layer
{
    public interface ILayerToy<LayerElementType>
        where LayerElementType:struct
    {
        LayerDictionary<LayerElementType> Layers { get; }
    }
}
