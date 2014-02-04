using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Color;

namespace DirectOutput.FX.RGBAFX
{
    /// <summary>
    /// The effects sets the color of a RGBAToy based on the trigger value.
    /// 
    /// Depending on the setting of the FadeMode property, the effect uses the active or inactive color or a mix of those colors.
    /// </summary>
   public class RGBAColorEffect:RGBAEffectBase
    {

        private RGBAColor _ActiveColor = new RGBAColor(0xff, 0xff, 0xff, 0xff);

        /// <summary>
        /// Gets or sets the active color.
        /// The ColorSetMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The active color.
        /// </value>
        public RGBAColor ActiveColor
        {
            get { return _ActiveColor; }
            set { _ActiveColor = value; }
        }

        private RGBAColor _InactiveColor = new RGBAColor(0, 0, 0, 0);

        /// <summary>
        /// Gets or sets the inactive color.
        /// The ColorSetMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The inactive color.
        /// </value>
        public RGBAColor InactiveColor
        {
            get { return _InactiveColor; }
            set { _InactiveColor = value; }
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

                Layer.Red = InactiveColor.Red + (int)((float)(ActiveColor.Red - InactiveColor.Red) * FadeValue / 255).Limit(0, 255);
                Layer.Green = InactiveColor.Green + (int)((float)(ActiveColor.Green - InactiveColor.Green) * FadeValue / 255).Limit(0, 255);
                Layer.Blue = InactiveColor.Blue + (int)((float)(ActiveColor.Blue - InactiveColor.Blue) * FadeValue / 255).Limit(0, 255);
                Layer.Alpha = InactiveColor.Alpha + (int)((float)(ActiveColor.Alpha - InactiveColor.Alpha) * FadeValue / 255).Limit(0, 255);

            }
        }


        /// <summary>
        /// Initializes the effect.<br />
        /// Resolves the name of the RGBA toy.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public override void Init(Table.Table Table)
        {
            base.Init(Table);
        }

        /// <summary>
        /// Finishes the effect.
        /// </summary>
        public override void Finish()
        {
            base.Finish();
        }

    }
}
