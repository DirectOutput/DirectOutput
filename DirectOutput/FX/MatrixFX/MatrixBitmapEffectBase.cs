using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General;

using DirectOutput.General.BitmapHandling;
using System.IO;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// Outputs a defined part of a bitmap on a area of a matrix
    /// </summary>
    public abstract class MatrixBitmapEffectBase<MatrixElementType> : MatrixEffectBase<MatrixElementType>, IMatrixBitmapEffect
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

        protected PixelData[,] Pixels;

        private void OutputBitmap(int FadeValue)
        {
            if (FadeMode == FadeModeEnum.OnOff) FadeValue = (FadeValue < 1 ? 0 : 255);


            for (int y = 0; y < AreaHeight; y++)
            {
                int yd = y + AreaTop;
                for (int x = 0; x < AreaWidth; x++)
                {
                    int xd = x + AreaLeft;
                    MatrixLayer[xd, yd] = GetEffectValue(FadeValue, Pixels[x, y]);
                }
            }


        }

        /// <summary>
        /// Gets the value which is to be applied to all elements of the matrix area controlled by the effect.
        /// This methed must be overwritten.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <param name="Pixel">The pixel to be applied to the matrix element.</param>
        /// <returns>Returns the value which is to be applied to to elements of the matrix representing the Pixel.</returns>
        public abstract MatrixElementType GetEffectValue(int TriggerValue, PixelData Pixel);

        /// <summary>
        /// Triggers the effect with the given data.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (InitOK)
            {
                OutputBitmap(TableElementData.Value);
            }

        }

        private bool InitOK = false;

        private readonly List<string> ExtensionExclusion = new List<string>() { FastImage.RawImageExtension };

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
                FileInfo FI = BitmapFilePattern.GetFirstMatchingFile(Table.Pinball.GlobalConfig.GetReplaceValuesDictionary(), ExtensionExclusion);
                if (FI!=null && FI.Exists)
                {
                    FastImage BM;
                    try
                    {
                        BM = Table.Bitmaps[FI.FullName];
                    }
                    catch (Exception E)
                    {
                        Log.Exception("MatrixBitmapEffectBase {0} cant initialize.  Could not load file {1}.".Build(Name, FI.FullName), E);
                        return;
                    }

                    if (BM.Frames.ContainsKey(BitmapFrameNumber))
                    {
                        Log.Instrumentation("MX", "BitmapEffectBase. Grabbing image clip: W: {0}, H:{1}, BML: {2}, BMT: {3}, BMW: {4}, BMH: {5}".Build(new object[] { AreaWidth, AreaHeight, BitmapLeft, BitmapTop, BitmapWidth, BitmapHeight }));
                        Pixels = BM.Frames[BitmapFrameNumber].GetClip(AreaWidth, AreaHeight, BitmapLeft, BitmapTop, BitmapWidth, BitmapHeight, DataExtractMode, UseCache: Table.Pinball.GlobalConfig.FastInitialization).Pixels;

                    }
                    else
                    {
                        Log.Warning("MatrixBitmapEffectBase {0} cant initialize. Frame {1} does not exist in source image {2}.".Build(Name, BitmapFrameNumber, FI.FullName));

                    }
                }
                else
                {
                    Log.Warning("MatrixBitmapEffectBase {0} cant initialize. No file matches the BitmapFilePattern {1} is invalid".Build(Name, BitmapFilePattern.ToString()));
                }
            }
            else
            {
                Log.Warning("MatrixBitmapEffectBase {0} cant initialize. The BitmapFilePattern {1} is invalid".Build(Name, BitmapFilePattern.ToString()));
            }


            InitOK = (Pixels != null && MatrixLayer != null);

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
