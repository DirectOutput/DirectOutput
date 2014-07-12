using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.ValueFX
{
    /// <summary>
    /// Inverts the trigger value of the effect before the target effect is called (e.g. 0 becomes 255, 255 becomes 0, 10 becomes 245).
    /// </summary>
    public class ValueInvertEffect:EffectEffectBase
    {
        /// <summary>
        /// Triggers the effect with the given TableElementData.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            TableElementData.Value = 255 - TableElementData.Value;
            TriggerTargetEffect(TableElementData);

        }
    }
}
