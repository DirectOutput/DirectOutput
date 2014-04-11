using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Analog;

namespace DirectOutput.FX.MatrixFX
{
    public class AnalogAlphaMatrixShiftEffect : MatrixShiftEffectBase<AnalogAlpha>
    {
        private const int RefreshIntervalMs = 30;

        private AnalogAlpha _ActiveValue = new AnalogAlpha(0xff, 0xff);

        /// <summary>
        /// Gets or sets the active Value.
        /// The FadeMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The active Value.
        /// </value>
        public AnalogAlpha ActiveValue
        {
            get { return _ActiveValue; }
            set { _ActiveValue = value; }
        }

        private AnalogAlpha _InactiveValue = new AnalogAlpha(0, 0);

        /// <summary>
        /// Gets or sets the inactive Value.
        /// The FadeMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The inactive Value.
        /// </value>
        public AnalogAlpha InactiveValue
        {
            get { return _InactiveValue; }
            set { _InactiveValue = value; }
        }



        /// <summary>
        /// Gets the effect Value by mixinging Active and InactiveValue based on the TriggerValue.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <returns>AnalogAlpha representing a mix of InactiveValue and ActiveValue.</returns>
        protected override AnalogAlpha GetEffectValue(int TriggerValue)
        {
            AnalogAlpha D = new AnalogAlpha();

            int V = TriggerValue.Limit(0, 255);
            D.Value = InactiveValue.Value + (int)((float)(ActiveValue.Value - InactiveValue.Value) * V / 255).Limit(0, 255);
            D.Alpha = InactiveValue.Alpha + (int)((float)(ActiveValue.Alpha - InactiveValue.Alpha) * V / 255).Limit(0, 255);
            return D;
        }


    }
}
