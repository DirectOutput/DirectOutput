using DirectOutput.General.BitmapHandling;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Analog;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// Displays a defined part of a bitmap on a area of a AnalogAlpha Matrix.
    /// </summary>
    public class AnalogAlphaMatrixBitmapEffect : MatrixBitmapEffectBase<AnalogAlpha>
    {

        /// <summary>
        /// Gets the value for a single element in the matrix.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <param name="Pixel">A pixel representing a element in the matrix.</param>
        /// <returns>The AnalogAlpha for a element in the matrix</returns>
        public override AnalogAlpha GetEffectValue(int TriggerValue, PixelData Pixel)
        {

            return new AnalogAlpha() { Value = (Pixel.Red + Pixel.Green + Pixel.Blue) / 3, Alpha = (int)((float)Pixel.Alpha * TriggerValue / 255) };
            
        }
    }
}
