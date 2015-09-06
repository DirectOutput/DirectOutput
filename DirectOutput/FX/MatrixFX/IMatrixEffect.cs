using System;
namespace DirectOutput.FX.MatrixFX
{
    public interface IMatrixEffect:IEffect
    {
        DirectOutput.FX.FadeModeEnum FadeMode { get; set; }
        float Height { get; set; }
        int LayerNr { get; set; }
        float Left { get; set; }
        float Top { get; set; }
        string ToyName { get; set; }
        float Width { get; set; }
    }
}
