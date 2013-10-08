using System;
namespace DirectOutput.Cab.Toys.Layer
{
    public interface IRGBAToy
    {
        void Finish();
        void Init(DirectOutput.Cab.Cabinet Cabinet);

        RGBALayerDictionary Layers { get; }
        string OutputNameBlue { get; set; }
        string OutputNameGreen { get; set; }
        string OutputNameRed { get; set; }
        void Reset();
    }
}
