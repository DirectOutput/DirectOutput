using System;
namespace DirectOutput.Cab.Toys
{
    public interface ILampToy:IAnalogToy
    {
        int Brightness { get; set; }
        void SetBrightness(int Brightness);
    }
}
