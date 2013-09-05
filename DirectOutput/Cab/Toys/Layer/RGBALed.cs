using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Out;

namespace DirectOutput.Cab.Toys.Layer
{
    public class RGBALed : ToyBaseUpdatable, IRGBToy
    {


        #region IRGBToy Member

        public int Blue
        {
            get
            {
                if (_OutputBlue != null)
                {
                    return _OutputBlue.Value;
                }
                return 0;
            }
        }

        public int Green
        {
            get
            {
                if (_OutputGreen != null)
                {
                    return _OutputGreen.Value;
                }
                return 0;
            }
        }

        public int Red
        {
            get
            {
                if (_OutputRed != null)
                {
                    return _OutputRed.Value;
                }
                return 0;
            }
        }

        public void SetColor(RGBColor Color)
        {
            SetLayer(int.MaxValue, Color);
        }

        public void SetColor(int Red, int Green, int Blue)
        {
            SetLayer(int.MaxValue, Red, Green, Blue);
        }

        public void SetColor(string Color)
        {
            if (_Cabinet.Colors.Contains(Color))
            {
                SetLayer(int.MaxValue, _Cabinet.Colors[Color]);
            }
            else
            {
                SetLayer(int.MaxValue, new RGBAColor(Color));
            }
        }

        #endregion



        #region Layers
        public RGBALayerDictionary Layers { get; private set; }

        public RGBALayer SetLayer(int Layer, int Red, int Green, int Blue)
        {
            return Layers.SetLayer(Layer, Red, Green, Blue);
        }

        public RGBALayer SetLayer(int Layer, int Red, int Green, int Blue, int Alpha)
        {
            return Layers.SetLayer(Layer, Red, Green, Blue, Alpha);
        }

        public RGBALayer SetLayer(int Layer, RGBAColor RGBA)
        {
            return Layers.SetLayer(Layer, RGBA);
        }


        public RGBALayer SetLayer(int Layer, RGBColor RGB)
        {
            return Layers.SetLayer(Layer, RGB);
        }

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
