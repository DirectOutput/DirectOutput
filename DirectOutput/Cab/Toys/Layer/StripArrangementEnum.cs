using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// Enum used to specify the arrangement of the ledstripe(s).
    ///  \image html LedStripArrangementEnum.jpg Supported led string arrangements
    /// 
    /// </summary>
    public enum LedStripArrangementEnum
    {
        LeftRightTopDown,
        LeftRightBottomUp,
        RightLeftTopDown,
        RightLeftBottomUp,
        TopDownLeftRight,
        TopDownRightLeft,
        BottomUpLeftRight,
        BottomUpRightLeft,
        LeftRightAlternateTopDown,
        LeftRightAlternateBottomUp,
        RightLeftAlternateTopDown,
        RightLeftAlternateBottomUp,
        TopDownAlternateLeftRight,
        TopDownAlternateRightLeft,
        BottomUpAlternateLeftRight,
        BottomUpAlternateRightLeft
    }
}
