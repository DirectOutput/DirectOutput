using System;
namespace DirectOutput.Cab.Toys.Layer
{
    public interface ILayerToy<LayerElementType>
        where LayerElementType:new()
    {
        LayerDictionary<LayerElementType> Layers { get; }
    }
}
