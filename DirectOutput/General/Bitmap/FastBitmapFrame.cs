using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.General.Bitmap
{
    public class FastBitmapFrame
    {
        private PixelData[,] _Pixels = new PixelData[0, 0];
        /// <summary>
        /// The pixel data array of the frame. <br />
        /// Dimension 0 if the array is the x/horizontal direction.
        /// Dimension 1 of the array is the y/vertical direction.
        /// </summary>
        /// <value>
        /// The pixels array of the frame.
        /// </value>
        private PixelData[,] Pixels
        {
            get
            {
                return _Pixels;
            }
            private set
            {
                _Pixels = value;
            }
        }


        /// <summary>
        /// Gets the PixelData for the specified pixel of the frame.<br/>
        /// For positions outside the frame, the method will return PixelData for a fully transparent black pixel.
        /// </summary>
        /// <param name="X">The X position of the pixel.</param>
        /// <param name="Y">The Y position of the pixel.</param>
        /// <returns>PixelData for the specified pixel.</returns>
        public PixelData GetPixel(int X, int Y)
        {
            try
            {
                return Pixels[X, Y];
            }
            catch { }
            return new PixelData();
        }

        /// <summary>
        /// Sets the size (width/height) of the frame.<br/>
        /// Setting the framesize will discard all existing pixel data of the frame.
        /// </summary>
        /// <param name="Width">The width of the frame.</param>
        /// <param name="Height">The height of the frame.</param>
        public void SetFrameSize(int Width, int Height)
        {
            Pixels = new PixelData[Width.Limit(0, int.MaxValue), Height.Limit(0, int.MaxValue)];
        }


        /// <summary>
        /// Gets the width of the frame.<br/>

        /// </summary>
        /// <value>
        /// The width of the frame.
        /// </value>
        public int Width
        {
            get
            {
                return Pixels.GetLength(0);
            }
        }

        /// <summary>
        /// Get the height of the frame.<br/>
        /// </summary>
        /// <value>
        /// The height of the frame.
        /// </value>
        public int Height
        {
            get
            {
                return Pixels.GetLength(1);
            }
        }

    }
}
