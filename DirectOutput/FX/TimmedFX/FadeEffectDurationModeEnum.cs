using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.TimmedFX
{
    /// <summary>
    /// Options which define how the durations of the FadeEffect are used.
    /// </summary>
    public enum FadeEffectDurationModeEnum
    {
        /// <summary>
        /// The duration(s) specify whoe long it will take to fade from the current value to the target value.
        /// </summary>
        CurrentToTarget,
        /// <summary>
        /// The duration(s) specify how long it would take to fade through the whole possible value range (0-255) for the target value. The effective fading duration will depend on the difference between the current and the target value.
        /// </summary>
        FullValueRange
    }
}
