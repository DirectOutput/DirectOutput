using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Out;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Toys
{
    /// <summary>
    /// Implementation of a generic digital toy.
    /// Implements IToy.
    /// </summary>
    public class GenericDigitalToy : ToyBase, IToy, DirectOutput.Cab.Toys.IDigitalToy
    {

        /// <summary>
        /// Initalizes the GenericDigitalToy.
        /// </summary>
        /// <param name="Cabinet">Cabinet to which GenericDigitalToy the belongs.</param>
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
        /// Name of the Output for the GenericDigitalToy
        /// </summary>
        public string OutputName { get; set; }

        private bool _State;
        /// <summary>
        /// State of the GenericDigitalToy.
        /// False equals off, true equals on.
        /// </summary>
        [XmlIgnoreAttribute]
        public bool State
        {
            get { return _State; }
            set
            {
                _State = value;
                if (_Output != null)
                {
                    _Output.Value = (State ? (byte)255 : (byte)0);
                }
            }
        }

        /// <summary>
        /// Sets the State property the GenericDigitalToy
        /// </summary>
        /// <param name="State">False equals off, true equals on.</param>
        public void SetState(bool State)
        {
            this.State = State;
        }

        /// <summary>
        /// Resets the GenericDigitalToy
        /// </summary>
        public override void Reset()
        {
            this.State = false;
        }
    }
}