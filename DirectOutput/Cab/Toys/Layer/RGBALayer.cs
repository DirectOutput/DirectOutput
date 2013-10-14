using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Color;

namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// Layer object for toys supporting several layers of RGB colors with alpha channel.
    /// </summary>
    public class RGBALayer
    {
        /// <summary>
        /// The red color component.
        /// </summary>
        public int Red;
        /// <summary>
        /// The green color component.
        /// </summary>
        public int Green;
        /// <summary>
        /// The blue color component.
        /// </summary>
        public int Blue;
        /// <summary>
        /// The alpha value for the color.
        /// </summary>
        public int Alpha;

        /// <summary>
        /// Sets the specified color for the layer.<br/>
        /// If all color components are set to 0, the alpha value will be set to 0 (fully transparent), otherwise the alpha value will be set to 255 (compeltely opaque).
        /// </summary>
        /// <param name="Red">The red color component.</param>
        /// <param name="Green">The green color component.</param>
        /// <param name="Blue">The blue color component.</param>
        public void Set(int Red, int Green, int Blue)
        {
            
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
            this.Alpha = (Red + Green + Blue > 0 ? 255 : 0);
        }

        /// <summary>
        /// Sets the specified color and alpha value
        /// </summary>
        /// <param name="Red">The red color component.</param>
        /// <param name="Green">The green color component.</param>
        /// <param name="Blue">The blue color component.</param>
        /// <param name="Alpha">The alpha value.</param>
        public void Set(int Red, int Green, int Blue, int Alpha)
        {
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
            this.Alpha = Alpha;
        }


        /// <summary>
        /// Gets a RGBAColor object representing the color and alpha values of the layer.
        /// </summary>
        /// <returns></returns>
        public RGBAColor GetRGBAColor()
        {
            return new RGBAColor(Red, Green, Blue, Alpha);
        }

        /// <summary>
        /// Sets the layers color and alpha values based on the specified RGBAColor object.
        /// </summary>
        /// <param name="RGBA">The RGBAColor object.</param>
        public void Set(RGBAColor RGBA)
        {
            Set(RGBA.Red, RGBA.Green, RGBA.Blue, RGBA.Alpha);
        }

        /// <summary>
        /// Sets the layers color values based on the specified RGBColor object.<br/>
        /// The alpha value is set to 0, if all color components are set to 0, otherwise the alpha value will be set to 255.
        /// </summary>
        /// <param name="RGB">The RGBColor object.</param>
        public void Set(RGBColor RGB)
        {
            Set(RGB.Red, RGB.Green, RGB.Blue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBALayer"/> class.
        /// </summary>
        public RGBALayer() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBALayer"/> class.
        /// </summary>
        /// <param name="Red">The red color component.</param>
        /// <param name="Green">The green color component.</param>
        /// <param name="Blue">The blue color component.</param>
        public RGBALayer(int Red, int Green, int Blue)
        {
            Set(Red, Green, Blue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBALayer"/> class.
        /// </summary>
        /// <param name="Red">The red color component.</param>
        /// <param name="Green">The green color component.</param>
        /// <param name="Blue">The blue color component.</param>
        /// <param name="Alpha">The alpha value.</param>
        public RGBALayer(int Red, int Green, int Blue, int Alpha)
        {
            Set(Red, Green, Blue, Alpha);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBALayer"/> class.
        /// </summary>
        /// <param name="RGBA">The RGBAColor object.</param>
        public RGBALayer(RGBAColor RGBA)
        {
            Set(RGBA.Red, RGBA.Green, RGBA.Blue, RGBA.Alpha);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBALayer"/> class.<br/>
        /// The alpha value is set to 0, if all color components are set to 0, otherwise the alpha value will be set to 255.
        /// </summary>
        /// <param name="RGB">The RGBColor object.</param>
        public  RGBALayer(RGBColor RGB)
        {
            Set(RGB.Red, RGB.Green, RGB.Blue);
        }
    }
}
