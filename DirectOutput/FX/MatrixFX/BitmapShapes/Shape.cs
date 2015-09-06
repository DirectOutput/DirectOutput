using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;
using DirectOutput.General.BitmapHandling;

namespace DirectOutput.FX.MatrixFX.BitmapShapes
{
    public class Shape : NamedItemBase
    {
  

        private int _BitmapFrameNumber = 0;

        /// <summary>
        /// Gets or sets the number of the frame to be used. This is usefull if you work with animated gifs.
        /// </summary>
        /// <value>
        /// The number of the frame to be used (for animated gifs).
        /// </value>
        public int BitmapFrameNumber
        {
            get { return _BitmapFrameNumber; }
            set { _BitmapFrameNumber = value; }
        }

        private int _BitmapTop = 0;

        /// <summary>
        /// Gets or sets the top of the the part of the bitmap which is to be used.
        /// </summary>
        /// <value>
        /// The top of the the part of the bitmap which is to be used.
        /// </value>
        public int BitmapTop
        {
            get { return _BitmapTop; }
            set { _BitmapTop = value; }
        }

        private int _BitmapLeft = 0;

        /// <summary>
        /// Gets or sets the left boundary of the the part of the bitmap which is to be used.
        /// </summary>
        /// <value>
        /// The left boundary of the the part of the bitmap which is to be used.
        /// </value>
        public int BitmapLeft
        {
            get { return _BitmapLeft; }
            set { _BitmapLeft = value; }
        }

        private int _BitmapWidth = -1;

        /// <summary>
        /// Gets or sets the width of the the part of the bitmap which is to be used.
        /// </summary>
        /// <value>
        /// The width of the the part of the bitmap which is to be used.
        /// </value>
        public int BitmapWidth
        {
            get { return _BitmapWidth; }
            set { _BitmapWidth = value; }
        }

        private int _BitmapHeight = -1;

        /// <summary>
        /// Gets or sets the height of the the part of the bitmap which is to be used.
        /// </summary>
        /// <value>
        /// The height of the the part of the bitmap which is to be used.
        /// </value>
        public int BitmapHeight
        {
            get { return _BitmapHeight; }
            set { _BitmapHeight = value; }
        }


        private FastBitmapDataExtractModeEnum _DataExtractMode = FastBitmapDataExtractModeEnum.BlendPixels;

        /// <summary>
        /// Gets or sets the mode how data is extracted from the source bitmap.
        /// </summary>
        /// <value>
        /// The data extract mode which defines how the data is extracted from the source bitmap.
        /// </value>
        public FastBitmapDataExtractModeEnum DataExtractMode
        {
            get { return _DataExtractMode; }
            set { _DataExtractMode = value; }
        }






    }
}
