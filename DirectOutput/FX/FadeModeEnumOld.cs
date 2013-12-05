using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX
{
    /// <summary>
    /// This enum describes the possible fading modes.
    /// </summary>
    public enum FadeModeEnumOld
    {
        /// <summary>
        /// Fading starts with the current value and fades towards a specified target value (depending on the trigger value either active or inactive value).
        /// </summary>
        CurrentToDefined,
        /// <summary>
        /// Fadding starts with a defined value (depending on the tigger value either active or in active vallue) and fades towards a specified target value (the other of the 2 defined active resp. inactive values).
        /// </summary>
        DefinedToDefined
    }
}
