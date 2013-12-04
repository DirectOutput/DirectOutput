using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.LedStripFX
{
    /// <summary>
    /// Displays a defined part of a bitmap on a area of a ledstrip.
    /// </summary>
    public class LedStripBitmapEffect : LedStripEffectBase
    {

        private int _BitmapFrameNumber = 0;

        /// <summary>
        /// Gets or sets the number of the frame to be displayed.
        /// </summary>
        /// <value>
        /// The number of the frame to be displayed.
        /// </value>
        public int BitmapFrameNumber
        {
            get { return _BitmapFrameNumber; }
            set { _BitmapFrameNumber = value; }
        }

        private int _BitmapTop = 0;

        /// <summary>
        /// Gets or sets the top of the the part of the bitmap which is to be displayed.
        /// </summary>
        /// <value>
        /// The top of the the part of the bitmap which is to be displayed.
        /// </value>
        public int BitmapTop
        {
            get { return _BitmapTop; }
            set { _BitmapTop = value; }
        }

        private int _BitmapLeft = 0;

        /// <summary>
        /// Gets or sets the left boundary of the the part of the bitmap which is to be displayed.
        /// </summary>
        /// <value>
        /// The left boundary of the the part of the bitmap which is to be displayed.
        /// </value>
        public int BitmapLeft
        {
            get { return _BitmapLeft; }
            set { _BitmapLeft = value; }
        }

        private int _BitmapWidth = -1;

        /// <summary>
        /// Gets or sets the width of the the part of the bitmap which is to be displayed.
        /// </summary>
        /// <value>
        /// The width of the the part of the bitmap which is to be displayed.
        /// </value>
        public int BitmapWidth
        {
            get { return _BitmapWidth; }
            set { _BitmapWidth = value; }
        }

        private int _BitmapHeight = -1;

        /// <summary>
        /// Gets or sets the height of the the part of the bitmap which is to be displayed.
        /// </summary>
        /// <value>
        /// The height of the the part of the bitmap which is to be displayed.
        /// </value>
        public int BitmapHeight
        {
            get { return _BitmapHeight; }
            set { _BitmapHeight = value; }
        }




        public override void Trigger(Table.TableElementData TableElementData)
        {
            throw new NotImplementedException();
        }

        public override void Init(Table.Table Table)
        {
            base.Init(Table);
        }

        public override void Finish()
        {
            base.Finish();
        }
    }
}
