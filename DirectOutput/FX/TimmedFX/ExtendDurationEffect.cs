using System;
using DirectOutput.Table;

namespace DirectOutput.FX.TimmedFX
{
    /// <summary>
    /// The extend duration effect triggers another effect for a duration which is extebnded by the number of milliseconds specified in DurationMs.<br/>
    /// This is done by forwarding triggers calls which are seting the effect to active directly to the target effect and delaying the forwarding of calls which set the effect to inactive by the number of milliseconds specified in DurationMs.<br/>
    /// \image html FX_ExtendDuration.png "ExtendDuration effect"
    /// </summary>
    public class ExtendDurationEffect : EffectEffectBase
    {
        private int _DurationMs = 500;

        /// <summary>
        /// Gets or sets the duration in milliseconds which will is added to the duration during which the effect is triggered/active.
        /// </summary>
        /// <value>
        /// The extended duration in milliseconds.
        /// </value>
        public int DurationMs
        {
            get { return _DurationMs; }
            set { _DurationMs = value.Limit(0, 500); }
        }



        /// <summary>
        /// Triggers the effect with the given TableElementData.<br/>
        /// Trigger calls of a TableElemenData value which is not equal 0, are forwarded directly to the target effect.<br/>
        /// Calls with a TableElementData Value of 0, are delayed by the Duration specifed in DurationMs before they are forwarded to the target effect.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (TargetEffect != null)
            {
                if (TableElementData.Value != 0)
                {
                    TargetEffect.Trigger(TableElementData);
                }
                else
                {
                    if (DurationMs > 0)
                    {
                        Table.Pinball.Alarms.RegisterAlarm(DurationMs, TriggerTargetEffect, TableElementData, true);
                    }
                    else
                    {
                        TargetEffect.Trigger(TableElementData);
                    }
                }

            }
        }


        private void TriggerTargetEffect(object AlarmParameter)
        {
            if (TargetEffect != null)
            {
                try
                {
                    TargetEffect.Trigger((TableElementData)AlarmParameter);
                }
                catch (Exception E)
                {
                    Log.Exception("The target effect {0} of the ExtendDurationEffect {1} has trown a exception.".Build(TargetEffectName, Name), E);
                    TargetEffect = null;
                }
            }

        }


        /// <summary>
        /// Inititializes the ExtendDurationEffect.
        /// </summary>
        /// <param name="Table">The table which uses the effect.</param>
        public override void Init(Table.Table Table)
        {
            base.Init(Table);
        }

        /// <summary>
        /// Finishes the ExtendDurationEffect.<br/>
        /// Clears all pending/delayed calls.
        /// </summary>
        public override void Finish()
        {
            Table.Pinball.Alarms.UnregisterAlarm(TriggerTargetEffect);
            base.Finish();
        }

    }
}
