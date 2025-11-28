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

        internal void Colorize(RGBAColor ActiveColor, RGBAColor InactiveColor)
        {
            double Brightness = ((double)(Red + Green + Blue) / 3).Limit(0, 255);

            Red = (byte)(InactiveColor.Red + (int)((float)(ActiveColor.Red - InactiveColor.Red) * Brightness / 255)).Limit(0, 255);
            Green = (byte)(InactiveColor.Green + (int)((float)(ActiveColor.Green - InactiveColor.Green) * Brightness / 255)).Limit(0, 255);
            Blue = (byte)(InactiveColor.Blue + (int)((float)(ActiveColor.Blue - InactiveColor.Blue) * Brightness / 255)).Limit(0, 255);
            Alpha = (byte)(InactiveColor.Alpha + (int)((float)(ActiveColor.Alpha - InactiveColor.Alpha) * Brightness / 255)).Limit(0, 255);
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
