using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    public class RGBALayer
    {
        public int Red;
        public int Green;
        public int Blue;
        public int Alpha;

        public void Set(int Red, int Green, int Blue)
        {
            
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
            this.Alpha = (Red + Green + Blue > 0 ? 255 : 0);
        }

        public void Set(int Red, int Green, int Blue, int Alpha)
        {
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
            this.Alpha = Alpha;
        }


        public RGBAColor GetRGBAColor()
        {
            return new RGBAColor(Red, Green, Blue, Alpha);
        }

        public void Set(RGBAColor RGBA)
        {
            Set(RGBA.Red, RGBA.Green, RGBA.Blue, RGBA.Alpha);
        }

        public void Set(RGBColor RGB)
        {
            Set(RGB.Red, RGB.Green, RGB.Blue);
        }

        public RGBALayer() { }

        public RGBALayer(int Red, int Green, int Blue)
        {
            Set(Red, Green, Blue);
        }

        public RGBALayer(int Red, int Green, int Blue, int Alpha)
        {
            Set(Red, Green, Blue, Alpha);
        }

        public RGBALayer(RGBAColor RGBA)
        {
            Set(RGBA.Red, RGBA.Green, RGBA.Blue, RGBA.Alpha);
        }

        public  RGBALayer(RGBColor RGB)
        {
            Set(RGB.Red, RGB.Green, RGB.Blue);
        }
    }
}
