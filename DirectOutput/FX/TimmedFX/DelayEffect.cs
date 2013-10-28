using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Table;

namespace DirectOutput.FX.TimmedFX
{
    /// <summary>
    /// The effect fires a assigned target effect after a specified delay.<br/>
    /// The original values supplied when the effect is triggered are forwarded to the target effect.<br/>
    /// \image html FX_Delay.png "Delay effect"
    /// </summary>
    public class DelayEffect:EffectEffectBase
    {
        
        private int _DelayMs=0;

        /// <summary>
        /// Gets or sets the delay in milliseconds.
        /// </summary>
        /// <value>
        /// The delay in milliseconds.
        /// </value>
        public int DelayMs
        {
            get { return _DelayMs; }
            set { _DelayMs = value; }
        }


        /// <summary>
        /// Triggers the effect.<br/>
        /// If the TargetEffect throws a exception, it will be deactivated.
        /// </summary>
        /// <param name="TableElementData">The TableElementData object for the TableElement which has triggered the effect.</param>
        public override void Trigger(TableElementData TableElementData)
        {
            if (TargetEffect != null)
            {
                Table.Pinball.Alarms.RegisterAlarm(DelayMs, TriggerTargetEffect, TableElementData, true);
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
                    Log.Exception("The target effect {0} of the DelayEffect {1} has trown a exception.".Build(TargetEffectName, Name), E);
                    TargetEffect = null;
                }
            }
        }

      
        /// <summary>
        /// Initializes the DelayEffect.
        /// </summary>
        /// <param name="Table">The table which contains the DelayEffect.</param>
        public override void Init(Table.Table Table)
        {

            base.Init(Table);
        }

        /// <summary>
        /// Finishes the DelayEffect.
        /// </summary>
        public new void Finish()
        {
            Table.Pinball.Alarms.UnregisterAlarm(TriggerTargetEffect);
         
            base.Finish();
        }

    }
}
