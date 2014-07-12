using DirectOutput.General.BitmapHandling;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Color;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// The RGBAMatrixBitmapEffect displays a defined part of a bitmap on a area of a RGBAtoy Matrix.
    /// 
    /// The properties of the effect allow you to select the part of the bitmap to display as well as the area of the matrix on which the bitmap is displayed. Dempending on the size of your bitmap you might choose different modes for the image extraction.
    /// 
    /// The effect supports numerous imahe formats, inluding png, gif (also animated) and jpg.
    /// 
    /// The image extraction takes place upon initalization of the framework. While the framework is active, it only outputs the previously extracted and scaled data to allow for better performance. 
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
