using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Color;

namespace DirectOutput.Cab.Toys.Virtual
{
    /// <summary>
    /// This toys allows the grouping of several RGBA toys (e.g. <see cref="RGBAToy"/> or <see cref="RGBLed"/>) into a matrix, which can be controlled by the matrix effects.
    /// 
    /// \note Be sure to define this toy in the config file before the toys, which are listed in the ToyNames array.
    /// </summary>
    public class RGBAToyGroup : ToyGroupBase<RGBAColor>
    {

    }
}
