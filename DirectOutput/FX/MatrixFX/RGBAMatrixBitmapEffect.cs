using DirectOutput.General.BitmapHandling;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Color;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// Displays a defined part of a bitmap on a area of a RGBAtoy Matrix.
    /// </summary>
    public class RGBAMatrixBitmapEffect : MatrixBitmapEffectBase<RGBAColor>
    {

        /// <summary>
        /// Gets the value for a single element in the matrix.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <param name="Pixel">A pixel representing a element in the matrix.</param>
        /// <returns>The RGBAData for a element in the matrix</returns>
        public override RGBAColor GetEffectValue(int TriggerValue, PixelData Pixel)
        {
     

            RGBAColor D = Pixel.GetRGBAColor();

            D.Alpha = (int)((float)Pixel.Alpha * TriggerValue / 255);

            return D;
            
        }
    }
}
