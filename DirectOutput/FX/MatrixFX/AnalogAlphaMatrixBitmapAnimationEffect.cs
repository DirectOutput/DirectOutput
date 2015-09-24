using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.BitmapHandling;
using DirectOutput.General;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Analog;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// Displays parts of a bitmap as a animation on a matrix of AnalogAlpha elements. 
    /// Check the docu on the other bitmap effects for more details on these effect types.
    /// </summary>
    public class AnalogAlphaMatrixBitmapAnimationEffect : MatrixBitmapAnimationEffectBase<AnalogAlpha>
    {
        /// <summary>
        /// Gets the value for a single element in the matrix.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <param name="Pixel">A pixel representing a element in the matrix.</param>
        /// <returns>The AnalogAlpha value for a element in the matrix</returns>
        protected override AnalogAlpha GetEffectValue(int TriggerValue, PixelData Pixel)
        {
            return new AnalogAlpha() { Value = (Pixel.Red + Pixel.Green + Pixel.Blue) / 3, Alpha = (int)((float)Pixel.Alpha * TriggerValue / 255) };

        }

    }
}
