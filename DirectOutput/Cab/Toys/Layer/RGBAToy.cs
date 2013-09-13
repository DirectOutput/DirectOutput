using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Out;
using DirectOutput.Cab.Color;

namespace DirectOutput.Cab.Toys.Layer
{
    public class RGBAToy : ToyBaseUpdatable, IRGBAToy
    {





        #region Layers
        public RGBALayerDictionary Layers { get; private set; }

       

        #endregion



        #region Outputs
        private IOutput _OutputRed;

        /// <summary>
        /// Name of the IOutput for red.
        /// </summary>
        public string OutputNameRed { get; set; }

        private IOutput _OutputGreen;

        /// <summary>
        /// Name of the IOutput for green.
        /// </summary>
        public string OutputNameGreen { get; set; }

        private IOutput _OutputBlue;

        /// <summary>
        /// Name of the IOutput for blue.
        /// </summary>
        public string OutputNameBlue { get; set; }

        #endregion


        private Cabinet _Cabinet;
        #region Init
        /// <summary>
        /// Initializes the RGBALed toy.
        /// </summary>
        /// <param name="Cabinet"><see cref="Cabinet"/> object to which the <see cref="RGBAToy"/> belongs.</param>
        public override void Init(Cabinet Cabinet)
        {
            _Cabinet = Cabinet;
            InitOutputs(Cabinet);
        }

        private void InitOutputs(Cabinet Cabinet)
        {
            if (Cabinet.Outputs.Contains(OutputNameRed))
            {
                _OutputRed = Cabinet.Outputs[OutputNameRed];
            }
            else
            {
                _OutputRed = null;
            }
            if (Cabinet.Outputs.Contains(OutputNameGreen))
            {
                _OutputGreen = Cabinet.Outputs[OutputNameGreen];
            }
            else
            {
                _OutputGreen = null;
            }
            if (Cabinet.Outputs.Contains(OutputNameBlue))
            {
                _OutputBlue = Cabinet.Outputs[OutputNameBlue];
            }
            else
            {
                _OutputBlue = null;
            }
        }
        #endregion

        #region Finish

        /// <summary>
        /// Finishes the RGBLed toy.<br/>
        /// Resets the the toy and releases all references.
        /// </summary>
        public override void Finish()
        {
            Reset();
            _Cabinet = null;
            _OutputRed = null;
            _OutputGreen = null;
            _OutputBlue = null;
        }
        #endregion




        public override void UpdateOutputs()
        {
            RGBColor RGB = Layers.GetResultingColor();
            if (_OutputRed != null)
            {
                _OutputRed.Value = (byte)RGB.Red;
            }
            if (_OutputGreen != null)
            {
                _OutputGreen.Value = (byte)RGB.Green;
            }
            if (_OutputBlue != null)
            {
                _OutputBlue.Value = (byte)RGB.Blue;
            }
        }


        public override void Reset()
        {
            Layers.Clear();
            _OutputRed.Value = 0;
            _OutputGreen.Value = 0;
            _OutputBlue.Value = 0;
        }


        public RGBAToy()
        {
            Layers = new RGBALayerDictionary();
        }





    }
}
