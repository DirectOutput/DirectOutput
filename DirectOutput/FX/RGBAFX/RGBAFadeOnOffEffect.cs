using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.RGBAFX
{
    /// <summary>
    /// This RGBA effect fades the color of a RGBA toys towards a defined target color based on the state (not 0, 0 or null) of the triggering table element (see Trigger method for details).
    /// </summary>
    public class RGBAFadeOnOffEffect : RGBAEffectBase
    {
        private const int FadingRefreshIntervalMs = 30;

        private RetriggerBehaviourEnum _RetriggerBehaviour;

        /// <summary>
        /// Gets or sets the retrigger behaviour.<br/>
        /// The setting defines the behaviour of the effect if it is retriggered while it is still active (e.g. already fading towards the ActiveColor and getting another trigger call with a active table element value).<br/>
        /// This settings is only relevant, if the effect can be called from more than one table element.
        /// </summary>
        /// <value>
        /// Valid values are RestartEffect or IgnoreRetrigger.
        /// </value>
        public RetriggerBehaviourEnum RetriggerBehaviour
        {
            get { return _RetriggerBehaviour; }
            set { _RetriggerBehaviour = value; }
        }

        private FadeModeEnum _FadeMode = FadeModeEnum.DefinedToDefined;

        /// <summary>
        /// Gets or sets the fading mode.<br/>
        /// This determines if one of the colors specified in the effect settings or the current color of the layer are used for the start of the fading.
        /// </summary>
        /// <value>
        /// CurrentToDefinedColor or DefinedColor
        /// </value>
        public FadeModeEnum FadeMode
        {
            get { return _FadeMode; }
            set { _FadeMode = value; }
        }

        //TODO: Maybe split fademode into fademodeactive and fademode inactive

        private int _FadeInactiveDurationMs = 500;

        /// <summary>
        /// Gets or sets the duration for the fading when the effect is inactive resp triggered with a table element value =0.
        /// </summary>
        /// <value>
        /// The fading duration in milliseconds.
        /// </value>
        public int FadeInactiveDurationMs
        {
            get { return _FadeInactiveDurationMs; }
            set { _FadeInactiveDurationMs = value; }
        }


        private int _FadeActiveDurationMs = 500;

        /// <summary>
        /// Gets or sets the duration for the fading when the effect is active resp triggered with a table element value !=0.
        /// </summary>
        /// <value>
        /// The fading duration in milliseconds.
        /// </value>
        public int FadeActiveDurationMs
        {
            get { return _FadeActiveDurationMs; }
            set { _FadeActiveDurationMs = value; }
        }



        float[] Current = new float[4];
        float[] Step = new float[4];
        float[] Target = new float[4];
        bool IsFading = false;

        private void StartFading(bool Active)
        {
            Table.Pinball.Alarms.UnregisterAlarm(FadingStep);

            RGBAColor TargetColor = (Active ? ActiveColor : InactiveColor);

            int Duration = (Active ? FadeActiveDurationMs : FadeInactiveDurationMs);
            int Steps = Duration / FadingRefreshIntervalMs;

            if (Steps > 0)
            {
                IsFading = true;

                RGBAColor CurrentColor;
                switch (FadeMode)
                {
                    case FadeModeEnum.CurrentToDefined:
                        CurrentColor = RGBAToy.Layers[Layer].GetRGBAColor();
                        break;
                    case FadeModeEnum.DefinedToDefined:
                    default:
                        CurrentColor = (!Active ? ActiveColor.Clone() : InactiveColor.Clone());
                        break;
                }

                Current[0] = CurrentColor.Red;
                Current[1] = CurrentColor.Green;
                Current[2] = CurrentColor.Blue;
                Current[3] = CurrentColor.Alpha;

                Target[0] = TargetColor.Red;
                Target[1] = TargetColor.Green;
                Target[2] = TargetColor.Blue;
                Target[3] = TargetColor.Alpha;

                for (int i = 0; i < 4; i++)
                {
                    Step[i] = (Target[i] - Current[i]) / Steps;
                }

                FadingStep();
            }
            else
            {
                RGBAToy.Layers[Layer].Set( TargetColor);
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
                    if (Current[i] > Target[i] && Current[i] > 0)
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

            RGBAToy.Layers[Layer].Set( (int)Current[0], (int)Current[1], (int)Current[2], (int)Current[3]);

            if (ContinueFading)
            {
                Table.Pinball.Alarms.RegisterAlarm(FadingRefreshIntervalMs, FadingStep);
                IsFading = true;
            }
            else
            {
                IsFading = false;
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



        bool LastTriggerState = false;
        /// <summary>
        /// Triggers the effect with the given TableElementData.<br/>
        /// If the TableElementData is null, the effect acts as a static effect and will fade towards the ActiveColor when it is triggered.<br/>
        /// If TableElementData is not null, the effect will fade the specified layer towards the ActiveColor if the TableElementData value is not 0. For 0 the layer will fade to the InactiveColor.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect or null.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (RGBAToy != null)
            {
                bool TriggerState = (TableElementData == null || TableElementData.Value != 0);

                if (TriggerState != LastTriggerState || IsFading == false || RetriggerBehaviour == RetriggerBehaviourEnum.RestartEffect)
                {
                    StartFading(TriggerState);
                }

                LastTriggerState = TriggerState;
            }
        }

        /// <summary>
        /// Finishes the effect (stops fading, removes the layer, releases references).
        /// </summary>
        public override void Finish()
        {
            Table.Pinball.Alarms.UnregisterAlarm(FadingStep);
            if (RGBAToy != null)
            {
                RGBAToy.Layers.Remove(Layer);
            }
            base.Finish();
        }

    }
}
