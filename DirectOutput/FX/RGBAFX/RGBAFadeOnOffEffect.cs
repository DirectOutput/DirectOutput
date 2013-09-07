using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.RGBAFX
{
    public class RGBAFadeOnOffEffect : RGBAEffectBase
    {
        private const int FadingRefreshIntervalMs=30;

        public enum FadeModeEnum
        {
            CurrentToDefinedColors,
            DefinedColors
        }

        private FadeModeEnum _FadeMode=FadeModeEnum.DefinedColors;

        public FadeModeEnum FadeMode
        {
            get { return _FadeMode; }
            set { _FadeMode = value; }
        }
        


        private int _FadeInactiveDurationMs=500;

        public int FadeInactiveDurationMs
        {
            get { return _FadeInactiveDurationMs; }
            set { _FadeInactiveDurationMs = value; }
        }
        

        private int _FadeActiveDurationMs=500;

        public int FadeActiveDurationMs
        {
            get { return _FadeActiveDurationMs; }
            set { _FadeActiveDurationMs = value; }
        }



        public float[] Current = new float[4];
        public float[] Step = new float[4];
        public float[] Target = new float[4];

        public void StartFading(Table.TableElementData TableElementData)
        {
            RGBAColor CurrentColor;
            switch (FadeMode)
            {
                case FadeModeEnum.CurrentToDefinedColors:
                    CurrentColor = RGBAToy.Layers[Layer].GetRGBAColor();
                    break;
                case FadeModeEnum.DefinedColors:
                default:
                    CurrentColor=(TableElementData.Value==0?ActiveColor.Clone():InactiveColor.Clone());
                    break;
            }

            Current[0] = CurrentColor.Red;
            Current[1] = CurrentColor.Green;
            Current[2] = CurrentColor.Blue;
            Current[3] = CurrentColor.Alpha;

            RGBAColor TargetColor = (TableElementData.Value != 0 ? ActiveColor.Clone() : InactiveColor.Clone());
            Target[0] = TargetColor.Red;
            Target[1] = TargetColor.Green;
            Target[2] = TargetColor.Blue;
            Target[3] = TargetColor.Alpha;

            int Duration=(TableElementData.Value!=0?FadeActiveDurationMs:FadeInactiveDurationMs);
            int Steps=Duration/FadingRefreshIntervalMs;

            for (int i = 0; i < 4; i++)
            {
                Step[i] = (Target[i] - Current[i]) / Steps;
            }
        }

        private void FadingStep()
        {
            bool ContinueFading = false;
            for (int i = 0; i < 4; i++)
            {
                if (Step[i] > 0)
                {
                    Current[i] += Step[i];
                    if (Current[i] < Target[i] && Current[i] < 255)
                    {
                        ContinueFading = true;
                    }
                    else
                    {
                        Current[i] = Target[i];
                        Step[i] = 0;
                    }
                }
                else if (Step[i] < 0)
                {
                    Current[i] += Step[i];
                    if (Current[i] > Target[i] && Current[i] >0)
                    {
                        ContinueFading = true;
                    }
                    else
                    {
                        Current[i] = Target[i];
                        Step[i] = 0;
                    }
                }
            }

            RGBAToy.SetLayer(Layer, (int)Current[0], (int)Current[1], (int)Current[2], (int)Current[3]);

            if (ContinueFading)
            {
                Table.Pinball.Alarms.RegisterAlarm(FadingRefreshIntervalMs,FadingStep);
            }
        }




        private RGBAColor _ActiveColor = new RGBAColor(0, 0, 0, 0);

        /// <summary>
        /// Gets or sets the RGBA color which is the target for the fading when the effect is triggered with a table element value which is not equal 0 or if the effect is triggered as a static effect (table element data = 0).
        /// </summary>
        /// <value>
        /// The RGBA color to be used when the effect is active.
        /// </value>
        public RGBAColor ActiveColor
        {
            get { return _ActiveColor; }
            set { _ActiveColor = value; }
        }


        private RGBAColor _InactiveColor = new RGBAColor(0, 0, 0, 0);

        /// <summary>
        /// Gets or sets the RGBA color which is the target for the fading when the effect is triggered with a table element value which is 0.
        /// </summary>
        /// <value>
        /// The RGBA color to be used when the effect is inactive.
        /// </value>
        public RGBAColor InactiveColor
        {
            get { return _InactiveColor; }
            set { _InactiveColor = value; }
        }



        /// <summary>
        /// Triggers the effect with the given TableElementData.<br/>
        /// If the TableElementData is null, the effect acts as a static effect and will set the ActiveColor when it is triggered.<br/>
        /// If TableElementData is not null, the effect will set the specified layer to the ActiveColor of the TableElementData value is not 0. For 0 the layer will be set to the InActiveColor.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect or null.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (RGBAToy != null)
            {
                if (TableElementData == null || TableElementData.Value != 0)
                {
                    RGBAToy.SetLayer(Layer, ActiveColor);
                }
                else
                {
                    RGBAToy.SetLayer(Layer, InactiveColor);
                }
            }
        }
        public override void Finish()
        {
            RGBAToy.Layers.Remove(Layer);
            base.Finish();
        }

    }
}
