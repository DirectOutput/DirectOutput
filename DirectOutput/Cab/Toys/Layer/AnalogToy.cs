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
    public class AnalogToy : ToyBaseUpdatable
    {
        //private static readonly byte[] FadeCurve = { 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16, 17, 17, 18, 18, 19, 19, 20, 20, 21, 21, 22, 22, 23, 23, 24, 24, 25, 25, 26, 27, 28, 29, 30, 30, 31, 32, 33, 34, 35, 36, 36, 37, 38, 39, 40, 41, 42, 43, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 102, 103, 104, 105, 106, 107, 108, 109, 110, 112, 113, 114, 115, 116, 117, 119, 120, 121, 122, 123, 124, 126, 127, 128, 129, 130, 132, 133, 134, 135, 136, 138, 139, 140, 141, 142, 144, 145, 146, 147, 149, 150, 151, 152, 154, 155, 156, 158, 159, 160, 161, 162, 164, 165, 167, 168, 169, 171, 172, 173, 174, 176, 177, 178, 180, 181, 182, 184, 185, 187, 188, 189, 191, 192, 193, 195, 196, 197, 199, 200, 202, 203, 204, 206, 207, 208, 210, 211, 213, 214, 216, 217, 218, 220, 221, 223, 224, 226, 227, 228, 230, 231, 233, 234, 236, 237, 239, 240, 242, 243, 245, 246, 248, 249, 251, 252, 254, 255 };

        /// <summary>
        /// Gets the layers dictionary.
        /// </summary>
        /// <value>
        /// The layers dictionary.
        /// </value>
        public AnalogLayerDictionary Layers { get; private set; }




        #region Outputs
        /// <summary>
        /// Gets the output for the toy.
        /// </summary>
        /// <value>
        /// The output of the toy.
        /// </value>
        public IOutput Output { get; private set; }


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
        /// Initializes a new instance of the <see cref="AnalogToy"/> class.
        /// </summary>
        public AnalogToy()
        {
            Layers = new AnalogLayerDictionary();
        }
    }
}
