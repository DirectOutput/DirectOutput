using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.BitmapHandling;
using DirectOutput.General;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.RGBAMatrixFX
{
    public class RGBAMatrixBitmapAnimationEffect : RGBAMatrixEffectBase
    {
        private RGBAMatrixAnimationDirection _AnimationDirection = RGBAMatrixAnimationDirection.Frame;

        public RGBAMatrixAnimationDirection AnimationDirection
        {
            get { return _AnimationDirection; }
            set { _AnimationDirection = value; }
        }

        private int _AnimationStepSize = 1;

        public int AnimationStepSize
        {
            get { return _AnimationStepSize; }
            set { _AnimationStepSize = value; }
        }


        private int _AnimationStepCount = 1;

        public int AnimationStepCount
        {
            get { return _AnimationStepCount; }
            set { _AnimationStepCount = value.Limit(1, int.MaxValue); }
        }



        private int _AnimationFrameDuration = 30;

        public int AnimationFrameDuration
        {
            get { return _AnimationFrameDuration; }
            set { _AnimationFrameDuration = value; }
        }



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

        private PixelData[][,] Pixels;


        private bool AnimationActive = false;
        private int AnimationStep = 0;
        private int AnimationFadeValue = 0;
        private void Animate()
        {
            float AlphaWeight = AnimationFadeValue.Limit(0, 255) / 255;

            for (int y = 0; y < AreaHeight; y++)
            {
                int yd = y + AreaTop;
                for (int x = 0; x < AreaWidth; x++)
                {
                    int xd = x + AreaLeft;
                    RGBAMatrixLayer[xd, yd].Red = Pixels[AnimationStep][x, y].Red;
                    RGBAMatrixLayer[xd, yd].Green = Pixels[AnimationStep][x, y].Green;
                    RGBAMatrixLayer[xd, yd].Blue = Pixels[AnimationStep][x, y].Blue;
                    RGBAMatrixLayer[xd, yd].Alpha = (int)(AlphaWeight * Pixels[AnimationStep][x, y].Alpha);
                }
            }

          

            AnimationStep++;
            if (AnimationStep >= Pixels.GetUpperBound(0))
            {
                AnimationStep = 0;
            }

        }

        private void Clear()
        {
            RGBAData Off = new RGBAData(); 

            for (int y = AreaTop; y <= AreaBottom; y++)
            {
               
                for (int x = AreaLeft; x <= AreaRight; x++)
                {
                    RGBAMatrixLayer[x, y] = Off;
                }
            }

        }

        private void ControlAnimation(int FadeValue)
        {
            if (FadeValue > 0)
            {
                //Start animation
                this.AnimationFadeValue = FadeValue;

                if (!AnimationActive)
                {
                    AnimationActive = true;
                    AnimationStep = 0;
                    Table.Pinball.Alarms.RegisterIntervalAlarm(AnimationFrameDuration, Animate);

                    Animate();
                }
            }
            else
            {
                //Stop animation
                StopAnimation();
            }



        }

        private void StopAnimation()
        {
            if (AnimationActive)
            {
                Table.Pinball.Alarms.UnregisterIntervalAlarm(Animate);

                AnimationActive = false;
                AnimationStep = 0;
                Clear();
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
                int FadeValue = TableElementData.Value;
                if (FadeMode == FadeModeEnum.OnOff) FadeValue = (FadeValue < 1 ? 0 : 255);
                ControlAnimation(FadeValue);

                
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
                        int StepCount = AnimationStepCount;
                        switch (AnimationDirection)
                        {
                            case RGBAMatrixAnimationDirection.Frame:
                                if ((BitmapFrameNumber + (StepCount * AnimationStepSize)) > BM.Frames.Count)
                                {
                                    StepCount = (BM.Frames.Count - BitmapFrameNumber) / AnimationStepSize;
                                }

                                Pixels = new PixelData[StepCount][,];

                                for (int s = 0; s < StepCount; s++)
                                {
                                    Pixels[s] = BM.Frames[BitmapFrameNumber + s].GetClip(AreaWidth, AreaHeight, BitmapLeft, BitmapTop, BitmapWidth, BitmapHeight, DataExtractMode).Pixels;
                                }



                                break;
                            case RGBAMatrixAnimationDirection.Right:
                                //TODO: Check if there should be a restriction of steps for this direction


                                Pixels = new PixelData[StepCount][,];

                                for (int s = 0; s < StepCount; s++)
                                {
                                    Pixels[s] = BM.Frames[BitmapFrameNumber].GetClip(AreaWidth, AreaHeight, BitmapLeft+s*AnimationStepSize, BitmapTop, BitmapWidth, BitmapHeight, DataExtractMode).Pixels;
                                }

                                break;
                            case RGBAMatrixAnimationDirection.Down:
                                //TODO: Check if there should be a restriction of steps for this direction
                                 Pixels = new PixelData[StepCount][,];

                                for (int s = 0; s < StepCount; s++)
                                {
                                    Pixels[s] = BM.Frames[BitmapFrameNumber].GetClip(AreaWidth, AreaHeight, BitmapLeft, BitmapTop + s * AnimationStepSize, BitmapWidth, BitmapHeight, DataExtractMode).Pixels;
                                }

                                break;
                            default:
                                StepCount = 1;
                                Pixels = new PixelData[StepCount][,];
                                for (int s = 0; s < StepCount; s++)
                                {
                                    Pixels[s] = BM.Frames[BitmapFrameNumber].GetClip(AreaWidth, AreaHeight, BitmapLeft, BitmapTop, BitmapWidth, BitmapHeight, DataExtractMode).Pixels;
                                }
                                break;
                        }



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
            StopAnimation();
            Pixels = null;
            base.Finish();
        }


    }
}
