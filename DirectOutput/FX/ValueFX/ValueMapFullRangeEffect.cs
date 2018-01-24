using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.ValueFX
{
    /// <summary>
    /// This effects maps the trigger value to the full range of 0 - 255.
    /// If the trigger value is 0, the mapped trigger value for the target effect is also 0.
    /// If the trigger value is >0, the mapped trigger value for the target effect is 255.
    /// </summary>
    public class ValueMapFullRangeEffect : EffectEffectBase
    {
        // Previous state map.  We use this to keep track of the table
        // element data for the last event trigger, so that we can detect
        // when there's an actual change on a subsequent event, as opposed
        // to a repeated trigger caused by a PWM update from VPinMAME.
        private Dictionary<string, int> _PreviousState = new Dictionary<string, int>();

        /// <summary>
        /// Triggers the effect with the given TableElementData.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            // Convert the element data from a PWM level to a boolean value: PWM 
            // level 0 is bool OFF, any non-zero PWM level is bool ON.  We
            // represent the bool values as 0/255 for OFF/ON.
            var newval = TableElementData.Value = (TableElementData.Value == 0 ? 0 : 255);

            // check to see if the converted boolean value has changed since
            // the last update
            var key = TableElementData.TableElementType.ToString() + TableElementData.Number.ToString();
            if (!_PreviousState.ContainsKey(key) || TableElementData.Value != newval)
            {
                // trigger the effect
                TriggerTargetEffect(TableElementData);

                // remember the update
                _PreviousState[key] = newval;
            }
        }
    }
}
