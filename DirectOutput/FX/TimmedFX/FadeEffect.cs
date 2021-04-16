using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.TimmedFX
{
    /// <summary>
    /// This effect fades towards the value passed to the effect in the TableElementData of the trigger methods.
    /// It is calling the target effect repeatedly with the changing values.
    /// </summary>
    public class FadeEffect:EffectEffectBase
    {
        private const int FadingRefreshIntervalMs = 30;


        private int _FadeUpDuration = 300;

        /// <summary>
        /// Gets or sets the duration for fading up.
        /// </summary>
        /// <value>
        /// The duration for fading up.
        /// </value>
        public int FadeUpDuration
        {
            get { return _FadeUpDuration; }
            set { _FadeUpDuration = value; }
        }

        private int _FadeDownDuration = 300;

        /// <summary>
        /// Gets or sets the duration for the fading down.
        /// </summary>
        /// <value>
        /// The duration for the fading down.
        /// </value>
        public int FadeDownDuration
        {
            get { return _FadeDownDuration; }
            set { _FadeDownDuration = value; }
        }


        private FadeEffectDurationModeEnum _FadeDurationMode = FadeEffectDurationModeEnum.CurrentToTarget;
        /// <summary>
        /// Gets or sets the fade duration mode.
        /// </summary>
        /// <value>
        /// The fade duration mode.<br/>
        /// Depending on the FadeDurationMode the transition from the current to the target value will use one of the duration values directly or use the duration values to determine how long it would take to fade through the whole possible value range and the effective fading duration will depend on the defference between the current and the target value.
        /// </value>
        public FadeEffectDurationModeEnum FadeDurationMode
        {
            get { return _FadeDurationMode; }
            set { _FadeDurationMode = value; }
        }
        


        float TargetValue = -1;
        float CurrentValue = 0;
        float StepValue = 0;
        int LastTargetTriggerValue = -1;
        Table.TableElementData TableElementData;

        /// <summary>
        /// Triggers the effect with the given TableElementData.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (TargetEffect!=null && TableElementData.Value != TargetValue)
            {
                TargetValue = TableElementData.Value.Limit(0,255);

                this.TableElementData = TableElementData;

                double Duration = (CurrentValue < TargetValue ? FadeUpDuration : FadeDownDuration);
                if (FadeDurationMode == FadeEffectDurationModeEnum.FullValueRange)
                {
                    Duration = Duration / 255 * (TargetValue - CurrentValue).Abs();
                }
                int Steps = (int)(Duration>0?(Duration / FadingRefreshIntervalMs):0);

                if (Steps > 0)
                {
                    StepValue = (float)(TargetValue - CurrentValue) / Steps;
                    LastTargetTriggerValue = -1;
                    FadingStep();

                }
                else
                {
                    Table.Pinball.Alarms.UnregisterAlarm( FadingStep);

                    CurrentValue=TargetValue;
                    LastTargetTriggerValue = -1;
                    TriggerTargetEffect(TableElementData);
                }

            }
        }

        private void FadingStep()
        {
            CurrentValue += StepValue;

            if ((CurrentValue < TargetValue && StepValue > 0) || (CurrentValue > TargetValue && StepValue < 0))
            {
                //Continue fading
                Table.Pinball.Alarms.RegisterIntervalAlarm(FadingRefreshIntervalMs, FadingStep);
            }
            else
            {
                Table.Pinball.Alarms.UnregisterIntervalAlarm(FadingStep);
                CurrentValue = TargetValue;
            }

            if (LastTargetTriggerValue != (int)CurrentValue)
            {
                LastTargetTriggerValue = (int)CurrentValue;
                TableElementData.Value = LastTargetTriggerValue;
                TriggerTargetEffect(TableElementData);
            }
        }

    }
}
