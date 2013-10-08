using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.Cab.Color;

namespace DirectOutput.FX.RGBAFX
{
    /// <summary>
    /// A basic RBA effect which sets the color of a layer of a RGBA toy to a specified color based on the state (not 0, 0) of the triggering table element.
    /// </summary>
    public class RGBAOnOffEffect : RGBAEffectBase
    {

        private RGBAColor _ActiveColor=new RGBAColor(0,0,0,0);

        /// <summary>
        /// Gets or sets the RGBA color which is set when the effect is triggered with a table element value which is not equal 0.
        /// </summary>
        /// <value>
        /// The RGBA color to be used when the effect is active.
        /// </value>
        public RGBAColor ActiveColor
        {
            get { return _ActiveColor; }
            set { _ActiveColor = value; }
        }


        private RGBAColor _InactiveColor = new RGBAColor(0, 0, 0, 0);

        /// <summary>
        /// Gets or sets the RGBA color which is set when the effect is triggered with a table element value which is 0.
        /// </summary>
        /// <value>
        /// The RGBA color to be used when the effect is inactive.
        /// </value>
        public RGBAColor InactiveColor
        {
            get { return _InactiveColor; }
            set { _InactiveColor = value; }
        }


        
        /// <summary>
        /// Triggers the effect with the given TableElementData.<br/>
        /// If TableElementData is not null, the effect will set the specified layer to the ActiveColor if the TableElementData value is not 0. For 0 the layer will be set to the InActiveColor.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect or null.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (RGBAToy != null)
            {
                if (TableElementData == null || TableElementData.Value != 0)
                {
                    RGBAToy.Layers[Layer].Set(ActiveColor);
                }
                else
                {
                    RGBAToy.Layers[Layer].Set(InactiveColor);
                }
            }
        }


        public override void Init(Table.Table Table)
        {
            
            base.Init(Table);
        }

          public override void Finish()
          {

              base.Finish();
          }

    }
}
