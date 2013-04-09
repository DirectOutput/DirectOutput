using System;
namespace DirectOutput.Cab.Toys
{
    public interface ILampToy:IAnalogToy
    {
        int Brightness { get;  }
        void SetBrightness(int Brightness);
    }
}
