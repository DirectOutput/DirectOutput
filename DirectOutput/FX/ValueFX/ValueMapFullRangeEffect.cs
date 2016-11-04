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
        private int _PreviousState = 0;
        /// <summary>
        /// Triggers the effect with the given TableElementData.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            TableElementData.Value = (TableElementData.Value == 0 ? 0 : 255);

            if (TableElementData.Value != _PreviousState)
            { 
                TriggerTargetEffect(TableElementData);
                _PreviousState = TableElementData.Value;
            }
        }
    }
}
