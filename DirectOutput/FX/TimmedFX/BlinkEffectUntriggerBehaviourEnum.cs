using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.TimmedFX
{
    /// <summary>
    /// Defines the untrigger behaviours for the blink effect.
    /// </summary>
    public enum BlinkEffectUntriggerBehaviourEnum
    {
        /// <summary>
        /// Blinking stops immediately
        /// </summary>
        Immediate,
        /// <summary>
        /// Completes the high cycle of the blinking before stopping. 
        /// </summary>
        CompleteHigh
    }
}
