using System;
namespace DirectOutput.Cab.Toys.Layer
{
    public interface IRGBAMatrix
    {
        RGBAData[,] GetLayer(int LayerNr);
        int Height { get; set; }
        int Width { get; set; }
    }
}
