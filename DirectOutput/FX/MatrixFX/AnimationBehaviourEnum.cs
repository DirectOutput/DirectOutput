using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// This enum describes the different supported behaviours for animations.
    /// </summary>
    public enum AnimationBehaviourEnum
    {

        /// <summary>
        /// The animation restarts when it is triggered, is shown once and stops after the last frame
        /// </summary>
        Once='O',
        /// <summary>
        /// The animation restarts when it is triggered and is shown in a loop
        /// </summary>
        Loop='L',
        /// <summary>
        /// The animation continues with the next frame when triggered and is shown in a loop
        /// </summary>
        Continue='C'
    }
}
