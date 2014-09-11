using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Out.AdressableLedStrip
{
    /// <summary>
    /// The WS2811StripController class is just a simple wrapper around the DirectStripController class. It is only here to allow the use of old configs.  
    /// Use the DirectStripController class for your configs.
    /// \deprecated The use of this class is deprecated. Please use the DirectStripController class instead.
    /// </summary>
    [Obsolete("Use the the DirectStripController class instead.")]
    public class WS2811StripController : DirectStripController
    {
    }
}
