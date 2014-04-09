using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.BitmapHandling;
using DirectOutput.General;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.MatrixFX
{
    public class RGBAMatrixBitmapAnimationEffect : MatrixBitmapAnimationEffectBase<RGBAData>
    {
        /// <summary>
        /// Gets the value for a single element in the matrix.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <param name="Pixel">A pixel representing a element in the matrix.</param>
        /// <returns>The RGBAData for a element in the matrix</returns>
        public override RGBAData GetEffectValue(int TriggerValue, PixelData Pixel)
        {
            RGBAData D = Pixel.GetRGBAData();

            D.Alpha = (int)((float)Pixel.Alpha * TriggerValue / 255);

            return D;

        }

    }
}
