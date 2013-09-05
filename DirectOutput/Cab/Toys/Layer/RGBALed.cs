using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Out;

namespace DirectOutput.Cab.Toys.Layer
{
    public class RGBALed : ToyBaseUpdatable
    {
        public RGBALayerDictionary Layers { get; private set; }

        public RGBALayer SetColor(int Layer, int Red, int Green, int Blue)
        {
            return Layers.SetColor(Layer,Red,Green,Blue);
        }

        public RGBALayer SetColor(int Layer, int Red, int Green, int Blue, int Alpha)
        {
            return Layers.SetColor(Layer, Red, Green, Blue, Alpha);
        }

        public RGBALayer SetColor(int Layer, RGBAColor RGBA)
        {
            return Layers.SetColor(Layer, RGBA);
        }


        public RGBALayer SetColor(int Layer, RGBColor RGB)
        {
            return Layers.SetColor(Layer, RGB);
        }


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
        /// Initializes the RGBLed toy.
        /// </summary>
        /// <param name="Cabinet"><see cref="Cabinet"/> object to which the <see cref="RGBALed"/> belongs.</param>
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


        public RGBALed()
        {
            Layers = new RGBALayerDictionary();
        }



    }
}
