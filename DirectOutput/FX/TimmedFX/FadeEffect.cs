using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.TimmedFX
{
    public class FadeEffect:EffectEffectBase
    {

        private int _FadeUpDuration = 300;

        public int FadeUpDuration
        {
            get { return _FadeUpDuration; }
            set { _FadeUpDuration = value; }
        }

        private int _FadeDownDuration = 300;

        public int FadeDownDuration
        {
            get { return _FadeDownDuration; }
            set { _FadeDownDuration = value; }
        }


        float TargetValue = 0;
        float CurrentValue = 0;

        /// <summary>
        /// Triggers the effect with the given TableElementData.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (TargetEffect!=null && TableElementData.Value != TargetValue && TargetValue!=CurrentValue)
            {
                TargetValue = TableElementData.Value.Limit(0,255);




            }
        }
    }
}
