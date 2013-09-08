using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.AnalogToyFX
{
    /// <summary>
    /// A basic effect controlling AnalogToy object based on value property (0, not 0 or null) of the TableElementData parameter of the Trigger method.
    /// </summary>
    public class AnalogToyOnOffEffect: AnanlogToyEffectBase
    {
        private int _ActiveValue=255;

        /// <summary>
        /// Gets or sets the value which is set on the specified layer of the referenced AnalogToy object if this effect is triggered with a TableElementData instance having a Value which is not zero of if the Effect is triggered with a null value for the TableElementData paramter.
        /// </summary>
        /// <value>
        /// The active value between 0 and 255.
        /// </value>
        public int ActiveValue
        {
            get { return _ActiveValue; }
            set { _ActiveValue = value.Limit(0,255); }
        }

        private int _InactiveValue=0;

        /// <summary>
        /// Gets or sets the value which is set on the specified layer of the referenced AnalogToy object if this effect is triggered with a TableElementData instance having a Value which is zero.
        /// </summary>
        /// <value>
        /// The inactive value between 0 and 255.
        /// </value>
        public int InactiveValue
        {
            get { return _InactiveValue; }
            set { _InactiveValue = value.Limit(0, 255); }
        }

        /// <summary>
        /// Triggers the effect with the given TableElementData.<br>
        /// If the Value property of the TableElementData parameter is not 0 or if the TableElementData parameter is null, the value of the specified layer of the referenced AnalogToy is set to the value specified in the ActiveValue property.<br/>
        /// If the Value property of the TableElementData parameter equals 0, the value of the specified layer of the referenced AnalogToy is set to the value specified in the InactiveValue property.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (Toy != null)
            {
                if (TableElementData == null || TableElementData.Value != 0)
                {
                    Toy.SetLayer(Layer, ActiveValue);
                }
                else
                {
                    Toy.SetLayer(Layer, InactiveValue);
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
