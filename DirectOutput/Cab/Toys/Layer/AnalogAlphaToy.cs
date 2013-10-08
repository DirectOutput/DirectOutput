using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Out;

namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// This toy handles analog values (0-255) in a layer structure including alpha value (0=completely transparent, 255=fully opaque) and outputs the belended result of the layers on a single output.
    /// </summary>
    public class AnalogAlphaToy : ToyBaseUpdatable, IAnalogAlphaToy
    {

        /// <summary>
        /// Gets the layers dictionary.
        /// </summary>
        /// <value>
        /// The layers dictionary.
        /// </value>
        [System.Xml.Serialization.XmlIgnore]
        public AnalogLayerDictionary Layers { get; private set; }




        #region Outputs

        private IOutput Output;


        /// <summary>
        /// Gets or sets the name of the IOutput object of the toy.
        /// </summary>
        /// <value>
        /// The name of the output.
        /// </value>
        public string OutputName { get; set; }


        #endregion

        private Cabinet Cabinet;


        /// <summary>
        /// Initializes the toy.
        /// </summary>
        /// <param name="Cabinet"><see cref="Cabinet" /> object  to which the <see cref="IToy" /> belongs.</param>
        public override void Init(Cabinet Cabinet)
        {
            this.Cabinet = Cabinet;
            InitOutputs(Cabinet);
        }

        private void InitOutputs(Cabinet Cabinet)
        {
            if (Cabinet.Outputs.Contains(OutputName))
            {
                Output = Cabinet.Outputs[OutputName];
            }
            else
            {
                Output = null;
            }
        }




        /// <summary>
        /// Updates the output of the toy.
        /// </summary>
        public override void UpdateOutputs()
        {
            if (Output != null)
            {
                //Output.Value = (byte)FadeCurve[Layers.GetResultingValue()];
                Output.Value = (byte)Layers.GetResultingValue();
            }
        }


        /// <summary>
        /// Resets the toy and releases all references
        /// </summary>
        public override void Finish()
        {
            Reset();
            Output = null;
            Cabinet = null;
            base.Finish();
        }

        /// <summary>
        /// Resets the toy.<br/>
        /// Clears the Layers object and turn off the output (if available).
        /// Method must be overwritten.
        /// </summary>
        public override void Reset()
        {
            Layers.Clear();
            if (Output != null)
            {
                Output.Value = 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogAlphaToy"/> class.
        /// </summary>
        public AnalogAlphaToy()
        {
            Layers = new AnalogLayerDictionary();
        }
    }
}
