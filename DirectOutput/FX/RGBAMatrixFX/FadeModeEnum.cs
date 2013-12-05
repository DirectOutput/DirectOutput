using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.RGBAMatrixFX
{
    /// <summary>
    /// Defines the fading behaviour.
    /// </summary>
    public enum FadeModeEnum
    {
        /// <summary>
        /// Fading is enabled.
        /// </summary>
        Fade,
        /// <summary>
        /// No fading. There will be a simple on/off behaviour depending on the triggering value.
        /// </summary>
        OnOff
    }
}
