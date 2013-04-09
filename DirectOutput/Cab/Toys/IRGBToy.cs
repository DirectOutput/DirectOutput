using System;
namespace DirectOutput.Cab.Toys
{
    public interface IRGBToy:IToy
    {
        int BrightnessBlue { get;  }
        int BrightnessGreen { get;  }
        int BrightnessRed { get;  }
        void SetColor(Color Color);
        void SetColor(int Red, int Green, int Blue);
        void SetColor(string Color);
    }
}
