using System;
namespace DirectOutput.Cab.Toys
{
    public interface IRGBToy:IToy
    {
        int BrightnessBlue { get; set; }
        int BrightnessGreen { get; set; }
        int BrightnessRed { get; set; }
        void SetColor(Color Color);
        void SetColor(int Red, int Green, int Blue);
        void SetColor(string Color);
    }
}
