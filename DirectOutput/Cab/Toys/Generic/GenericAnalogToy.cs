using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Out;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Toys.Generic
{
    /// <summary>
    /// Implementation of a generic analog toy.
    /// Implements IToy.
    /// </summary>
    public class GenericAnalogToy : ToyBase, IToy, DirectOutput.Cab.Toys.IAnalogToy
    {

        /// <summary>
        /// Initalizes the GenericAnalogToy.
        /// </summary>
         /// <param name="Cabinet"><see cref="Cabinet"/> object to which the <see cref="GenericAnalogToy"/> belongs.</param>
        public override void Init(Cabinet Cabinet)
        {
            InitOutput(Cabinet);
        }

        private void InitOutput(Cabinet Cabinet)
        {
            if (Cabinet.Outputs.Contains(OutputName))
            {
                _Output = Cabinet.Outputs[OutputName];
            }
            else
            {
                _Output = null;
            }
        }

        private IOutput _Output;
        /// <summary>
        /// Name of the Output for the GenericAnalogToy.
        /// </summary>
        public string OutputName { get; set; }




        private int _Value;
        /// <summary>
        /// Value of the GenericAnalogToy.
        /// </summary>
        [XmlIgnoreAttribute]
        public int Value
        {
            get { return _Value; }
            protected set
            {
                _Value = value.Limit(0, 255);
                if (_Output != null)
                {
                    _Output.Value = (byte)_Value;
                }
            }
        }

        /// <summary>
        /// Resets the GenericAnalogToy.
        /// </summary>
        public override void Reset()
        {
            this.Value = 0;
        }


        /// <summary>
        /// Sets the Value property for the GenericAnalogToy.
        /// </summary>
        /// <param name="Value">Value for the GenericAnalogToy.</param>
        public void SetValue(int Value)
        {
            this.Value = Value;
        }
    }
}
