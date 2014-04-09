using DirectOutput.General.BitmapHandling;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// Displays a defined part of a bitmap on a area of a RGBAtoy Matrix.
    /// </summary>
    public class RGBAMatrixBitmapEffect : MatrixBitmapEffectBase<RGBAData>
    {

        /// <summary>
        /// Gets the value for a single element in the matrix.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <param name="Pixel">A pixel representing a element in the matrix.</param>
        /// <returns>The RGBAData for a element in the matrix</returns>
        public override RGBAData GetEffectValue(int TriggerValue, PixelData Pixel)
        {
            RGBAData D=Pixel.GetRGBAData();

            D.Alpha = (int)((float)Pixel.Alpha * TriggerValue / 255);

            return D;
            
        }
    }
}
