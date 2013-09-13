using System;
namespace DirectOutput.Cab.Color
{
    public interface IRGBAColor
    {
        int Alpha { get; set; }
        int Blue { get; set; }
        int Green { get; set; }
        string HexColor { get; set; }
        int Red { get; set; }
        bool SetColor(int Red, int Green, int Blue, int Alpha);
        bool SetColor(int Red, int Green, int Blue);
        bool SetColor(IRGBColor Color);
        bool SetColor(string Color);
        RGBAColor GetRGBAColor();

    }
}
