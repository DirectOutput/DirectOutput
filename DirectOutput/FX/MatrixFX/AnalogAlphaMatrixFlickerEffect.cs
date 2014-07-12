using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Analog;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// Does create random flickering with a defineable density, durations and value within the spefied area of a matrix.
    /// </summary>
    public class AnalogAlphaMatrixFlickerEffect : MatrixFlickerEffectBase<AnalogAlpha>
    {
        private const int RefreshIntervalMs = 30;

        private AnalogAlpha _ActiveValue = new AnalogAlpha(0xff, 0xff);

        /// <summary>
        /// Gets or sets the active value.
        /// The FadeMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The active value.
        /// </value>
        public AnalogAlpha ActiveValue
        {
            get { return _ActiveValue; }
            set { _ActiveValue = value; }
        }

        private AnalogAlpha _InactiveValue = new AnalogAlpha(0, 0);

        /// <summary>
        /// Gets or sets the inactive value.
        /// The FadeMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The inactive value.
        /// </value>
        public AnalogAlpha InactiveValue
        {
            get { return _InactiveValue; }
            set { _InactiveValue = value; }
        }


        /// <summary>
        /// Gets the effect value by mixinging Active and InactiveValue based on the TriggerValue.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <returns>AnalogAlpha value representing a mix of InactiveValue and ActiveValue.</returns>
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
