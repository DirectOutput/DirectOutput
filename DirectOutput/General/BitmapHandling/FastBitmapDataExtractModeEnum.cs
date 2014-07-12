using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.General.BitmapHandling
{
    /// <summary>
    /// The enum defines how the pixels are extracted from the source image.
    /// </summary>
    public enum FastBitmapDataExtractModeEnum
    {
        SinglePixelTopLeft,
        SinglePixelCenter,
        BlendPixels
    }
}
