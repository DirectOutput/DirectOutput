using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General;
using DirectOutput.General.Bitmap;

namespace DirectOutput.FX.RGBAMatrixFX
{
    /// <summary>
    /// Displays a defined part of a bitmap on a area of a ledstrip.
    /// </summary>
    public class RGBAMatrixBitmapEffect : RGBAMatrixEffectBase
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


        private FastBitmapDataExtractModeEnum _DataExtractMode = FastBitmapDataExtractModeEnum.SinglePixelCenter;

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

        private FadeModeEnum _FadeMode = FadeModeEnum.Fade;

        /// <summary>
        /// Gets or sets the fade mode.
        /// </summary>
        /// <value>
        /// Fade (active and inactive color will fade depending on trigger value) or OnOff (actvice color is used for triger values >0, otherwise inactive color will be used).
        /// </value>
        public FadeModeEnum FadeMode
        {
            get { return _FadeMode; }
            set { _FadeMode = value; }
        }

        private FilePattern _BitmapFilePattern;

        /// <summary>
        /// Gets or sets the file pattern which is used to load the bitmap file for the effect.
        /// </summary>
        /// <value>
        /// The bitmap file pattern which is used to load the bitmap file for the effect.
        /// </value>
        public FilePattern BitmapFilePattern
        {
            get { return _BitmapFilePattern; }
            set { _BitmapFilePattern = value; }
        }

        private PixelData[,] Pixels;

        private void DisplayBitmap(int FadeValue)
        {
            if (FadeMode == FadeModeEnum.OnOff) FadeValue = (FadeValue < 1 ? 0 : 255); 

            float AlphaWeight = 255 / FadeValue.Limit(0, 255);
            for (int y = 0; y < AreaHeight; y++)
            {
                int yd = y * AreaTop;
                for (int x = 0; x < AreaWidth; x++)
                {
                    int xd = x + AreaLeft;
                    RGBAMatrixLayer[xd, yd].Red = Pixels[x, y].Red;
                    RGBAMatrixLayer[xd, yd].Green = Pixels[x, y].Green;
                    RGBAMatrixLayer[xd, yd].Blue = Pixels[x, y].Blue;
                    RGBAMatrixLayer[xd, yd].Alpha = (int)(AlphaWeight * Pixels[x, y].Alpha);
                }
            }


        }



        /// <summary>
        /// Triggers the effect with the given data.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (InitOK)
            {
                DisplayBitmap(TableElementData.Value);
            }

        }

        private bool InitOK = false;


        /// <summary>
        /// Initializes the effect.
        /// Resolves object references, extracts source image data.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public override void Init(Table.Table Table)
        {
            InitOK = false;
            Pixels = null;
            base.Init(Table);

            //TODO: Insert replace values for file pattern
            if (BitmapFilePattern.IsValid)
            {

                string Filename = BitmapFilePattern.GetFirstMatchingFile().FullName;
                if (!Filename.IsNullOrWhiteSpace())
                {
                    FastImage BM;
                    try
                    {
                        BM = Table.Bitmaps[Filename];
                    }
                    catch (Exception E)
                    {
                        Log.Exception("LedStripBitmapEffect {0} cant initialize.  Could not load file {1}.".Build(Name, Filename), E);
                        return;
                    }

                    if (BM.Frames.ContainsKey(BitmapFrameNumber))
                    {
                        Pixels = BM.Frames[BitmapFrameNumber].GetClip(AreaWidth, AreaHeight, BitmapLeft, BitmapTop, BitmapWidth, BitmapHeight, DataExtractMode).Pixels;

                    }
                    else
                    {
                        Log.Warning("LedStripBitmapEffect {0} cant initialize. Frame {1} does not exist in source image {2}.".Build(Name, BitmapFrameNumber, Filename));

                    }
                }
                else
                {
                    Log.Warning("LedStripBitmapEffect {0} cant initialize. No file matches the BitmapFilePattern {1} is invalid".Build(Name, BitmapFilePattern.ToString()));
                }
            }
            else
            {
                Log.Warning("LedStripBitmapEffect {0} cant initialize. The BitmapFilePattern {1} is invalid".Build(Name, BitmapFilePattern.ToString()));
            }


            InitOK = (Pixels != null && RGBAMatrixLayer != null);

        }

        /// <summary>
        /// Finishes the effect and releases object references
        /// </summary>
        public override void Finish()
        {
            Pixels = null;
            base.Finish();
        }
    }
}
