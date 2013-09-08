using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Out;

namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// 
    /// </summary>
    public class AnalogToy: ToyBaseUpdatable
    {


        public AnalogLayerDictionary Layers { get; private set; }


        public void SetLayer(int Layer, int Value)
        {
            Layers.SetLayer(Layer, Value);
        }

        public void SetLayer(int Layer, int Value, int Alpha)
        {
            Layers.SetLayer(Layer,Value, Alpha);
        }


        #region Outputs
        /// <summary>
        /// Gets the output for the toy.
        /// </summary>
        /// <value>
        /// The output of the toy.
        /// </value>
        public IOutput Output{get;private set;}


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
        /// Initializes a new instance of the <see cref="AnalogToy"/> class.
        /// </summary>
        public AnalogToy()
        {
            Layers = new AnalogLayerDictionary();
        }

        public override void UpdateOutputs()
        {
            if (Output != null)
            {
                Output.Value = (byte)Layers.GetResultingValue();
            }
        }



        public override void Reset()
        {
            Layers.Clear();
            Output.Value = 0;
        }
    }
}
