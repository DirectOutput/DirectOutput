using System;
namespace DirectOutput.Cab.Toys.Layer
{
    public interface ILayerToy<LayerElementType>:IToy
        where LayerElementType:new()
    {
        LayerDictionary<LayerElementType> Layers { get; }
    }
}
