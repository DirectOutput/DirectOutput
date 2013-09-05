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

        public void SetColor(int Red, int Green, int Blue)
        {
            
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
            this.Alpha = (Red + Green + Blue > 0 ? 255 : 0);
        }

        public void SetColor(int Red, int Green, int Blue, int Alpha)
        {
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
            this.Alpha = Alpha;
        }

        public void SetColor(RGBAColor RGBA)
        {
            SetColor(RGBA.Red, RGBA.Green, RGBA.Blue, RGBA.Alpha);
        }

        public void SetColor(RGBColor RGB)
        {
            SetColor(RGB.Red, RGB.Green, RGB.Blue);
        }

        public RGBALayer() { }

        public RGBALayer(int Red, int Green, int Blue)
        {
            SetColor(Red, Green, Blue);
        }

        public RGBALayer(int Red, int Green, int Blue, int Alpha)
        {
            SetColor(Red, Green, Blue, Alpha);
        }

        public RGBALayer(RGBAColor RGBA)
        {
            SetColor(RGBA.Red, RGBA.Green, RGBA.Blue, RGBA.Alpha);
        }

        public  RGBALayer(RGBColor RGB)
        {
            SetColor(RGB.Red, RGB.Green, RGB.Blue);
        }
    }
}
