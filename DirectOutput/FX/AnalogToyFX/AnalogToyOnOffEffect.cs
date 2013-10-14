using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.AnalogToyFX
{
    /// <summary>
    /// A basic effect setting the output of a AnalogToy object to a active or inactive value, based on value property (0, not 0) of the TableElementData parameter of the Trigger method.
    /// </summary>
    public class AnalogToyOnOffEffect: AnanlogToyEffectBase
    {
        private AnalogAlphaValue _ActiveValue = new AnalogAlphaValue(255,255);

        /// <summary>
        /// Gets or sets the value which is set on the specified layer of the referenced AnalogToy object if this effect is triggered with a TableElementData instance having a Value which is not zero.
        /// </summary>
        /// <value>
        /// The active value between 0 and 255.
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
        /// The inactive value between 0 and 255.
        /// </value>
        public AnalogAlphaValue InactiveValue
        {
            get { return _InactiveValue; }
            set { _InactiveValue = value; }
        }

        /// <summary>
        /// Triggers the effect with the given TableElementData.<br/>
        /// If the Value property of the TableElementData parameter is not 0, the value of the specified layer of the referenced AnalogToy is set to the value specified in the ActiveValue property.<br/>
        /// If the Value property of the TableElementData parameter equals 0, the value of the specified layer of the referenced AnalogToy is set to the value specified in the InactiveValue property.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
    

            if (Toy != null)
            {
                if (TableElementData == null || TableElementData.Value != 0)
                {
                    Toy.Layers[Layer].Set(ActiveValue);
                }
                else
                {
                    Toy.Layers[Layer].Set( InactiveValue);
                }
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
            if (Toy != null)
            {
                Toy.Layers.Remove(Layer);
            }
            base.Finish();
        }
    }
}
