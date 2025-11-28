using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.BitmapHandling;
using DirectOutput.General;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.MatrixFX
{
    public abstract class MatrixBitmapAnimationEffectBase<MatrixElementType> : MatrixEffectBase<MatrixElementType>, IMatrixBitmapEffect
    {
        private MatrixAnimationStepDirectionEnum _AnimationStepDirection = MatrixAnimationStepDirectionEnum.Frame;

        /// <summary>
        /// Gets or sets the animation direction.
        /// </summary>
        /// <value>
        /// The direction in which the effect will step formward through the source image to get the next frame of the animation. 
        /// </value>
        public MatrixAnimationStepDirectionEnum AnimationStepDirection
        {
            get { return _AnimationStepDirection; }
            set { _AnimationStepDirection = value; }
        }

        private int _AnimationStepSize = 1;

        /// <summary>
        /// Gets or sets the size of the step in pixels or frames (depending on the \ref AnimationStepDirection) to the next frame of the animation.
        /// </summary>
        /// <value>
        /// The size of the step in pixels or frames (depending on the \ref AnimationStepDirection) to the next frame of the animation.
        /// </value>
        public int AnimationStepSize
        {
            get { return _AnimationStepSize; }
            set { _AnimationStepSize = value; }
        }


        private int _AnimationFrameCount = 1;

        /// <summary>
        /// Gets or sets the number of frames for the whole animation.
        /// </summary>
        /// <value>
        /// The number of frames for the whole animation.
        /// </value>
        public int AnimationFrameCount
        {
            get { return _AnimationFrameCount; }
            set { _AnimationFrameCount = value.Limit(1, int.MaxValue); }
        }

        private AnimationBehaviourEnum _AnimationBehaviour= AnimationBehaviourEnum.Loop;

        /// <summary>
        /// Gets or sets the animation behaviour.
        /// </summary>
        /// <value>
        /// The animation behaviour defines if a animation should run only once, run in a loop or continue at its last position when triggered.
        /// </value>
        public AnimationBehaviourEnum AnimationBehaviour
        {
            get { return _AnimationBehaviour; }
            set { _AnimationBehaviour = value; }
        }
        

        private int _AnimationFrameDurationMs = 30;

        /// <summary>
        /// Gets or sets the animation frame duration in ms.
        /// </summary>
        /// <value>
        /// The animation frame duration in miliseconds. Defaults to 30ms if not set.
        /// </value>
        public int AnimationFrameDurationMs
        {
            get { return _AnimationFrameDurationMs; }
            set { _AnimationFrameDurationMs = value.Limit(1,int.MaxValue); }
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
            set { _BitmapTop = value.Limit(0,int.MaxValue); }
        }

        private int _BitmapLeft = 0;

        /// <summary>
        /// Gets or sets the left boundary of the the part of the bitmap which is to be displayed.
        /// </summary>
        /// <value>
        /// The left boundary in pixels of the the part of the bitmap which is to be displayed.
        /// </value>
        public int BitmapLeft
        {
            get { return _BitmapLeft; }
            set { _BitmapLeft = value.Limit(0, int.MaxValue); }
        }

        private int _BitmapWidth = -1;

        /// <summary>
        /// Gets or sets the width of the the part of the bitmap which is to be displayed.
        /// </summary>
        /// <value>
        /// The width in pixels of the the part of the bitmap which is to be displayed. -1 selects the fully width resp. the remaining width from the BitMapLeft position.
        /// </value>
        public int BitmapWidth
        {
            get { return _BitmapWidth; }
            set { _BitmapWidth = value.Limit(-1, int.MaxValue); }
        }

        private int _BitmapHeight = -1;

        /// <summary>
        /// Gets or sets the height of the the part of the bitmap which is to be displayed.
        /// </summary>
        /// <value>
        /// The height of the the part of the bitmap which is to be displayed. -1 selects the fully height resp. the remaining height from the BitMapTop position.
        /// </value>
        public int BitmapHeight
        {
            get { return _BitmapHeight; }
            set { _BitmapHeight = value.Limit(-1, int.MaxValue); }
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

        protected PixelData[][,] Pixels;


        private bool AnimationActive = false;
        private int AnimationStep = 0;
        private int AnimationFadeValue = 0;
        private void Animate()
        {

            if (AnimationStep <= Pixels.GetUpperBound(0)) {
                for (int y = 0; y < AreaHeight; y++) {
                    int yd = y + AreaTop;
                    for (int x = 0; x < AreaWidth; x++) {
                        int xd = x + AreaLeft;
                        MatrixLayer[xd, yd] = GetEffectValue(AnimationFadeValue, Pixels[AnimationStep][x, y]);
                    }
                }
                AnimationStep++;
                if (AnimationBehaviour != AnimationBehaviourEnum.Once) {
                    AnimationStep = AnimationStep % (Pixels.GetUpperBound(0) + 1);
                }
            } else {
                AnimationStep = 0;
                if (AnimationBehaviour == AnimationBehaviourEnum.Once) {
                    StopAnimation();
                }
            }
        }

        private void Clear()
        {
            MatrixElementType Off = GetEffectValue(0, new PixelData());

            for (int y = AreaTop; y <= AreaBottom; y++)
            {
               
                for (int x = AreaLeft; x <= AreaRight; x++)
                {
                    MatrixLayer[x, y] = Off;
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
        protected abstract MatrixElementType GetEffectValue(int TriggerValue, PixelData Pixel);


        private void ControlAnimation(int FadeValue)
        {
            if (FadeValue > 0)
            {
                //Start animation
                this.AnimationFadeValue = FadeValue;

                if (!AnimationActive)
                {
                    AnimationActive = true;

                    if (AnimationBehaviour != AnimationBehaviourEnum.Continue)
                    {
                        AnimationStep = 0;
                    }
                    Table.Pinball.Alarms.RegisterIntervalAlarm(AnimationFrameDurationMs, Animate);

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
 
                try
                {
                    Table.Pinball.Alarms.UnregisterIntervalAlarm(Animate);
                }
                catch { };

                AnimationActive = false;
                
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

                string Filename = BitmapFilePattern.GetFirstMatchingFile(Table.Pinball.GlobalConfig.GetReplaceValuesDictionary(), ExtensionExclusion)?.FullName ?? string.Empty;
                if (!Filename.IsNullOrWhiteSpace())
                {
                    FastImage BM;
                    try
                    {
                        BM = Table.Bitmaps[Filename];
                    }
                    catch (Exception E)
                    {
                        Log.Exception("MatrixBitmapAnimationEffectBase {0} cant initialize.  Could not load file {1}.".Build(Name, Filename), E);
                        return;
                    }

                    if (BM.Frames.ContainsKey(BitmapFrameNumber))
                    {
                        int StepCount = AnimationFrameCount;
                        switch (AnimationStepDirection)
                        {
                            case MatrixAnimationStepDirectionEnum.Frame:
                                if ((BitmapFrameNumber + (StepCount * AnimationStepSize)) > BM.Frames.Count)
                                {
                                    StepCount = (BM.Frames.Count - BitmapFrameNumber) / AnimationStepSize;
                                }

                                Pixels = new PixelData[StepCount][,];


                                for (int s = 0; s < StepCount; s++)
                                {
                                    
                                    Pixels[s] = BM.Frames[BitmapFrameNumber + s].GetClip(AreaWidth, AreaHeight, BitmapLeft, BitmapTop, BitmapWidth, BitmapHeight, DataExtractMode, UseCache: Table.Pinball.GlobalConfig.FastInitialization).Pixels;
                                }



                                break;
                            case MatrixAnimationStepDirectionEnum.Right:
                                //TODO: Check if there should be a restriction of steps for this direction


                                Pixels = new PixelData[StepCount][,];

                                for (int s = 0; s < StepCount; s++)
                                {
                                    Pixels[s] = BM.Frames[BitmapFrameNumber].GetClip(AreaWidth, AreaHeight, BitmapLeft+s*AnimationStepSize, BitmapTop, BitmapWidth, BitmapHeight, DataExtractMode, UseCache: Table.Pinball.GlobalConfig.FastInitialization).Pixels;
                                }

                                break;
                            case MatrixAnimationStepDirectionEnum.Down:
                                //TODO: Check if there should be a restriction of steps for this direction
                                 Pixels = new PixelData[StepCount][,];

                                for (int s = 0; s < StepCount; s++)
                                {
                                    Pixels[s] = BM.Frames[BitmapFrameNumber].GetClip(AreaWidth, AreaHeight, BitmapLeft, BitmapTop + s * AnimationStepSize, BitmapWidth, BitmapHeight, DataExtractMode, UseCache: Table.Pinball.GlobalConfig.FastInitialization).Pixels;
                                }

                                break;
                            default:
                                StepCount = 1;
                                Pixels = new PixelData[StepCount][,];
                                for (int s = 0; s < StepCount; s++)
                                {
                                    Pixels[s] = BM.Frames[BitmapFrameNumber].GetClip(AreaWidth, AreaHeight, BitmapLeft, BitmapTop, BitmapWidth, BitmapHeight, DataExtractMode, UseCache: Table.Pinball.GlobalConfig.FastInitialization).Pixels;
                                }
                                break;
                        }

                        Log.Instrumentation("MX", "BitmapAnimationEffectBase. Grabbed image clips: W: {0}, H:{1}, BML: {2}, BMT: {3}, BMW: {4}, BMH: {5}, Steps: {6}".Build(new object[] { AreaWidth, AreaHeight, BitmapLeft, BitmapTop, BitmapWidth, BitmapHeight, StepCount }));


                    }
                    else
                    {
                        Log.Warning("MatrixBitmapAnimationEffectBase {0} cant initialize. Frame {1} does not exist in source image {2}.".Build(Name, BitmapFrameNumber, Filename));

                    }
                }
                else
                {
                    Log.Warning("MatrixBitmapAnimationEffectBase {0} cant initialize. No file matches the BitmapFilePattern {1} is invalid".Build(Name, BitmapFilePattern.ToString()));
                }
            }
            else
            {
                Log.Warning("MatrixBitmapAnimationEffectBase {0} cant initialize. The BitmapFilePattern {1} is invalid".Build(Name, BitmapFilePattern.ToString()));
            }


            InitOK = (Pixels != null && MatrixLayer != null);

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
