using System;
namespace DirectOutput.Cab.Toys.Layer
{
    public interface IAnalogAlphaToy
    {
        void Finish();
        void Init(DirectOutput.Cab.Cabinet Cabinet);
        AnalogLayerDictionary Layers { get; }
        string OutputName { get; set; }
        void Reset();
        void UpdateOutputs();
    }
}
