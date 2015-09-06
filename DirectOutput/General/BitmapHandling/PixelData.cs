using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Color;

namespace DirectOutput.General.BitmapHandling
{
    /// <summary>
    /// Struct holding the data for a single pixel in a bitmap.
    /// </summary>
    public struct PixelData
    {
        //TODO: Check order of bytes!

        //Attention!
        //The order of bytes is important. Dont change this without beeing sure that this will yield the desired results.
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;



        public RGBAColor GetRGBAColor()
        {
            return new RGBAColor(Red, Green, Blue, Alpha);
        }


        public PixelData(byte Red, byte Green, byte Blue, byte Alpha)
        {
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
            this.Alpha = Alpha;
        }

    }
}
