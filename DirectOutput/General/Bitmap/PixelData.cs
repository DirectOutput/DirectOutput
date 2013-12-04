using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.General.Bitmap
{
    /// <summary>
    /// Struct holding the data for a single pixel in a bitmap.
    /// </summary>
    public struct PixelData
    {
        //TODO: Check order of bytes!
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;
    }
}
