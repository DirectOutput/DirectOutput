using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.General.BitmapHandling
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

        public DirectOutput.Cab.Toys.Layer.RGBAData GetRGBAData()
        {
            DirectOutput.Cab.Toys.Layer.RGBAData D;
            D.Red = Red;
            D.Green = Green;
            D.Blue = Blue;
            D.Alpha = Alpha;
            return D;
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
