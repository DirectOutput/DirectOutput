using System;
namespace DirectOutput.Cab.Color
{
    public interface IRGBColor
    {
        int Blue { get; set; }
        int Green { get; set; }
        string HexColor { get; set; }
        int Red { get; set; }
    }
}
