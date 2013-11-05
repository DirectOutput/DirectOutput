using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.AnalogToyFX
{
    /// <summary>
    /// A effect fading the output value of a AnalogToy object to a active or inactive value. The fading is controlled by the value property (0, not 0) of the TableElementData parameter of the Trigger method.
    /// \image html FX_FadeOnOff.png "FadeOnOff effect"
    /// </summary>
    public class AnalogToyFadeOnOffEffect : AnanlogToyEffectBase
    {
        private const int FadingRefreshIntervalMs = 30;

        private RetriggerBehaviourEnum _RetriggerBehaviour;

        /// <summary>
        /// Gets or sets the retrigger behaviour.<br/>
        /// The setting defines the behaviour of the effect if it is retriggered while it is still active (e.g. already fading towards the ActiveValue and getting another trigger call with a active table element value).<br/>
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
        /// This determines if one of the values specified in the effect settings or the current value of the layer are used for the start of the fading.
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




        private AnalogAlphaValue _ActiveValue = new AnalogAlphaValue(255, 255);

        /// <summary>
        /// Gets or sets the value which is set on the specified layer of the referenced AnalogToy object if this effect is triggered with a TableElementData instance having a Value which is not zero.
        /// </summary>
        /// <value>
        /// The active value between 0 and 255.
        /// </value>
        public AnalogAlphaValue ActiveValue
        {
            get { return _ActiveValue; }
            set { _ActiveValue = value; }
        }

        private AnalogAlphaValue _InactiveValue = new AnalogAlphaValue(0, 0);

        /// <summary>
        /// Gets or sets the value which is set on the specified layer of the referenced AnalogToy object if this effect is triggered with a TableElementData instance having a Value which is zero.
        /// </summary>
        /// <value>
        /// The inactive value between 0 and 255.
        /// </value>
        public AnalogAlphaValue InactiveValue
        {
            get { return _InactiveValue; }
            set { _InactiveValue = value; }
        }




        float[] Current = new float[2];
        float[] Step = new float[2];
        float[] Target = new float[2];
        bool IsFading = false;

        private void StartFading(bool Active)
        {
            Table.Pinball.Alarms.UnregisterAlarm(FadingStep);

            AnalogAlphaValue TargetValue = (Active ? ActiveValue : InactiveValue);

            int Duration = (Active ? FadeActiveDurationMs : FadeInactiveDurationMs);
            int Steps = Duration / FadingRefreshIntervalMs;

            if (Steps > 0)
            {

                IsFading = true;

                AnalogAlphaValue CurrentAnalogAlphaValue;
                switch (FadeMode)
                {
                    case FadeModeEnum.CurrentToDefined:
                        CurrentAnalogAlphaValue = Toy.Layers[Layer].GetAnalogAlphaValue();
                        break;
                    case FadeModeEnum.DefinedToDefined:
                    default:
                        CurrentAnalogAlphaValue = (!Active ? ActiveValue : InactiveValue);
                        break;
                }

                Current[0] = CurrentAnalogAlphaValue.Value;
                Current[1] = CurrentAnalogAlphaValue.Alpha;

                Target[0] = TargetValue.Value;
                Target[1] = TargetValue.Alpha;


                for (int i = 0; i < 2; i++)
                {
                    Step[i] = (Target[i] - Current[i]) / Steps;
                }
                FadingStep();
            }
            else
            {
                Toy.Layers[Layer].Set(TargetValue);
            }

        }

        private void FadingStep()
        {
            bool ContinueFading = false;
            for (int i = 0; i < 2; i++)
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

            Toy.Layers[Layer].Set((int)Current[0], (int)Current[1]);

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

        bool LastTriggerState = false;
        /// <summary>
        /// Triggers the effect with the given TableElementData.<br/>
        /// If the Value property of the TableElementData parameter is not 0, the value of the specified layer of the referenced AnalogToy fades towards the value specified in the ActiveValue property.<br/>
        /// If the Value property of the TableElementData parameter equals 0, the value of the specified layer of the referenced AnalogToy fades towards the value specified in the InactiveValue property.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (Toy != null)
            {

                bool TriggerState = TableElementData.Value != 0;

                if (TriggerState != LastTriggerState || IsFading == false || RetriggerBehaviour == RetriggerBehaviourEnum.RestartEffect)
                {
                    StartFading(TriggerState);
                }

                LastTriggerState = TriggerState;
            }
        }


        /// <summary>
        /// Initializes the effect.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public override void Init(Table.Table Table)
        {
            base.Init(Table);
        }

        /// <summary>
        /// Finish does all necessary cleanupwork before the effect is discarded.
        /// </summary>
        public override void Finish()
        {
            try
            {
                Table.Pinball.Alarms.UnregisterAlarm(FadingStep);
            }
            catch { }
            if (Toy != null)
            {
                Toy.Layers.Remove(Layer);
            }
            base.Finish();
        }
    }
}
