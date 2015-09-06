using System;
namespace DirectOutput.FX.MatrixFX
{
    public interface IMatrixBitmapEffect : IMatrixEffect
    {
        DirectOutput.General.FilePattern BitmapFilePattern { get; set; }
        int BitmapFrameNumber { get; set; }
        int BitmapHeight { get; set; }
        int BitmapLeft { get; set; }
        int BitmapTop { get; set; }
        int BitmapWidth { get; set; }
        DirectOutput.General.BitmapHandling.FastBitmapDataExtractModeEnum DataExtractMode { get; set; }
    }
}
