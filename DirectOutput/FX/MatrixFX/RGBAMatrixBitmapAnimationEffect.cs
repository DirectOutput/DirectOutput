using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.BitmapHandling;
using DirectOutput.General;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Color;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// The RGBAMatrixBitmapAnimationEffect displays a anmation which is based on a image file on the defineable part of a matrix of rgb toys (e.g. adressable ledstrip).
    /// 
    /// The properties of the effect allow you to specify the position, frame and size of the first image part to be displayed on the matrix. In addition you can define how the effect steps forward through the source picture for the further animation frames.
    /// 
    /// To get a better idea, have a look at the following video and the picture below it.
    /// 
    /// \htmlinclude 61_FX_BuiltIn_RGBAMatrixBitmapAnimationVideo.html
    /// 
    /// \image html RGBAMatrixBitmapAnimationEffectWave.png Source Image
    /// 
    /// In this example DOF extracts a area of 10x100pixels for every frame of the animation. For every frame of the animation it steps 3 pixels to the right, so we slowly progress through the whole image.
    ///  
    /// </summary>
    public class RGBAMatrixBitmapAnimationEffect : MatrixBitmapAnimationEffectBase<RGBAColor>
    {
        /// <summary>
        /// Gets the value for a single element in the matrix.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <param name="Pixel">A pixel representing a element in the matrix.</param>
        /// <returns>The RGBAData for a element in the matrix</returns>
        protected override RGBAColor GetEffectValue(int TriggerValue, PixelData Pixel)
        {
            RGBAColor D = Pixel.GetRGBAColor();

            D.Alpha = (int)((float)Pixel.Alpha * TriggerValue / 255);

            return D;

        }

    }
}
