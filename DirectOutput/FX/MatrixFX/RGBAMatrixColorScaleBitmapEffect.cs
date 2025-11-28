using DirectOutput.General.BitmapHandling;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Color;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// The RGBAMatrixBitmapEffect displays a defined part of a bitmap in the given colors on a area of a RGBAtoy Matrix. The effect take the overall brightness of the pixels of the bitmap to control the brightness of the specified colors for each pixel.
    /// 
    /// The properties of the effect allow you to select the part of the bitmap to display as well as the area of the matrix on which the bitmap is displayed. Depending on the size of your bitmap you might choose different modes for the image extraction.
    /// 
    /// The effect supports numerous image formats, inluding png, gif (also animated) and jpg.
    /// 
    /// The image extraction takes place upon initalization of the framework. While the framework is active, it only outputs the previously extracted and scaled data to allow for better performance. 
    /// </summary>
    public class RGBAMatrixColorScaleBitmapEffect : MatrixBitmapEffectBase<RGBAColor>
    {

        private RGBAColor _ActiveColor = new RGBAColor(0xff, 0xff, 0xff, 0xff);

        /// <summary>
        /// Gets or sets the active color.
        /// The FadeMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The active color.
        /// </value>
        public RGBAColor ActiveColor
        {
            get { return _ActiveColor; }
            set { _ActiveColor = value; }
        }

        private RGBAColor _InactiveColor = new RGBAColor(0, 0, 0, 0);

        /// <summary>
        /// Gets or sets the inactive color.
        /// The FadeMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The inactive color.
        /// </value>
        public RGBAColor InactiveColor
        {
            get { return _InactiveColor; }
            set { _InactiveColor = value; }
        }



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

        public override void Init(Table.Table Table)
        {
            base.Init(Table);

            if (Pixels != null)
            {
                for (int Y = 0; Y <= Pixels.GetUpperBound(1); Y++)
                {
                    for (int X = 0; X <= Pixels.GetUpperBound(0); X++)
                    {
                        Pixels[X, Y].Colorize(ActiveColor, InactiveColor);
                    }
                }
            }
        }
    }
}
