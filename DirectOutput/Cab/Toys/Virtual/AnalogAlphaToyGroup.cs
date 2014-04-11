using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Analog;

namespace DirectOutput.Cab.Toys.Virtual
{
    /// <summary>
    /// This toys allows the grouping of several AnalogAlpha toys (e.g. <see cref="AnalogAlphaToy"/> or <see cref="Lamp"/>) into a matrix, which can be controlled by the matrix effects.
    /// 
    /// \note: Be sure to define this toy in the config file before the toys, which are listed in the ToyNames array.
    /// </summary>
    public class AnalogAlphaToyGroup : ToyGroupBase<AnalogAlpha>
    {

    }
}
