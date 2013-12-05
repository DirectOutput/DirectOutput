using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.AnalogToyFX
{
    /// <summary>
    /// This effect controlls sets the value and alpha channel of a analog alpha toy based on the trigger value.
    /// 
    /// Dependinging on the FadeMode property the effect sets the value of the target layer either to the active inactive value in OnOff mode or a mix of the value in Fade mode.
    /// </summary>
    public class AnalogToyValueEffect : AnanlogToyEffectBase
    {

        private AnalogAlphaValue _ActiveValue = new AnalogAlphaValue(255, 255);

        /// <summary>
        /// Gets or sets the value which is set on the specified layer of the referenced AnalogToy object if this effect is triggered with a TableElementData instance having a Value which is not zero.
        /// </summary>
        /// <value>
        /// The active value and alpha channel between 0 and 255.
        /// </value>
        public AnalogAlphaValue ActiveValue
        {
            get { return _ActiveValue; }
            set { _ActiveValue = value; }
        }

        private AnalogAlphaValue _InactiveValue = new AnalogAlphaValue(0, 0);

        /// <summary>
        /// Gets or sets the value which is set on the specified layer of the referenced AnalogToy object if this effect is triggered with a TableElementData instance having a Value which is zero.
        /// </summary>
        /// <value>
        /// The inactive value and alpha channel between 0 and 255.
        /// </value>
        public AnalogAlphaValue InactiveValue
        {
            get { return _InactiveValue; }
            set { _InactiveValue = value; }
        }


        private FadeModeEnum _FadeMode = FadeModeEnum.Fade;

        /// <summary>
        /// Gets or sets the fade mode.
        /// </summary>
        /// <value>
        /// Fade (active and inactive color will fade depending on trigger value) or OnOff (actvice color is used for triger values >0, otherwise inactive color will be used).
        /// </value>
        public FadeModeEnum FadeMode
        {
            get { return _FadeMode; }
            set { _FadeMode = value; }
        }


        /// <summary>
        /// Triggers the effect with the given TableElementData.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (Layer != null)
            {
                int FadeValue = TableElementData.Value;
                if (FadeMode == FadeModeEnum.OnOff && FadeValue > 0) { FadeValue = 255; }

                Layer.Value = InactiveValue.Value + (int)((float)(ActiveValue.Value - InactiveValue.Value) * FadeValue / 255).Limit(0, 255);
                Layer.Alpha = InactiveValue.Alpha + (int)((float)(ActiveValue.Alpha - InactiveValue.Alpha) * FadeValue / 255).Limit(0, 255);

            }
        }


        /// <summary>
        /// Initializes the effect.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public override void Init(Table.Table Table)
        {
            base.Init(Table);
        }

        /// <summary>
        /// Finish does all necessary cleanupwork before the effect is discarded.
        /// </summary>
        public override void Finish()
        {
            base.Finish();
        }

    }

}
