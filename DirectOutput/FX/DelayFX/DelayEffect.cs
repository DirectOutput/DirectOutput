using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Table;

namespace DirectOutput.FX.DelayFX
{
    /// <summary>
    /// The effect fires a assigned effect after a specified delay.
    /// </summary>
    public class DelayEffect:AssignedEffect
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
        /// Triggers the assigned Effect.
        /// \remark If the assigned effect throws a exception the effect will be deactivated.
        /// </summary>
        /// <param name="TableElementData">The TableElementData object for the TableElement which has triggered the effect.</param>
        public new void Trigger(TableElementData TableElementData)
        {
            Pinball.Alarms.RegisterAlarm(DelayMs, TriggerAssignedEffect, TableElementData, true);
        }


        private void TriggerAssignedEffect(object AlarmParameter)
        {
            base.Trigger((TableElementData)AlarmParameter);
        }

        private Pinball Pinball;
        /// <summary>
        /// Initializes the DelayEffect.
        /// </summary>
        /// <param name="Pinball">The Pinball object to which the effect belongs.</param>
        public new void Init(Pinball Pinball)
        {
            this.Pinball = Pinball;
            base.Init(Pinball);
        }

        /// <summary>
        /// Finishes the DelayEffect.
        /// </summary>
        public new void Finish()
        {
            Pinball.Alarms.UnregisterAlarm(TriggerAssignedEffect);
            Pinball = null;
            base.Finish();
        }

    }
}
