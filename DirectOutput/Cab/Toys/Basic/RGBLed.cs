
using System.Xml.Serialization;
using DirectOutput.Cab.Out;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Color;

namespace DirectOutput.Cab.Toys.Basic
{

    /// <summary>
    /// \deprecated The use of this toy is depreceated. Use the new RGBAToy instead.
    /// 
    /// RGB led toy controlls a multicolor led.
    /// Implement IToy, inherits Toy.
    /// </summary>
    public class RGBLed : ToyBase, IRGBToy
    {
        private Cabinet _Cabinet;
        #region Init
        /// <summary>
        /// Initializes the RGBLed toy.
        /// </summary>
        /// <param name="Cabinet"><see cref="Cabinet"/> object to which the <see cref="RGBLed"/> belongs.</param>
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


        private int _BrightnessRed;

        /// <summary>
        /// Red brightness.
        /// </summary>
        [XmlIgnoreAttribute]
        public int Red
        {
            get { return _BrightnessRed; }
            private set
            {
                _BrightnessRed = value.Limit(0, 255);
                if (_OutputRed != null)
                {
                    _OutputRed.Value = (byte)_BrightnessRed;
                }
            }
        }
        private int _BrightnessGreen;

        /// <summary>
        /// Green brightness
        /// </summary>
        [XmlIgnoreAttribute]
        public int Green
        {
            get { return _BrightnessGreen; }
            private set
            {
                _BrightnessGreen = value.Limit(0, 255);
                if (_OutputGreen != null)
                {
                    _OutputGreen.Value = (byte)_BrightnessGreen;
                }
            }
        }
        private int _BrightnessBlue;

        /// <summary>
        /// Blue brightness
        /// </summary>
        [XmlIgnoreAttribute]
        public int Blue
        {
            get { return _BrightnessBlue; }
            private set
            {
                _BrightnessBlue = value.Limit(0, 255);
                if (_OutputBlue != null)
                {
                    _OutputBlue.Value = (byte)_BrightnessBlue;
                }
            }
        }




        /// <summary>
        /// Resets the RGBLed toy.
        /// Turns of the light.
        /// </summary>
        public override void Reset()
        {
            Red = 0;
            Blue = 0;
            Green = 0;

        }

        /// <summary>
        /// Sets the color of the RGBLed toy.
        /// </summary>
        /// <param name="Red">Red brightness</param>
        /// <param name="Green">Green brightness</param>
        /// <param name="Blue">Blue brightness</param>
        public void SetColor(int Red, int Green, int Blue)
        {
            this.Red = Red;
            this.Blue = Blue;
            this.Green = Green;

        }


        /// <summary>
        /// Sets the color of the RGBLed toy.
        /// </summary>
        /// <param name="Color">Color object containg the brightness values for the color.</param>
        public void SetColor(RGBColor Color)
        {
            SetColor(Color.Red, Color.Green, Color.Blue);
        }


        /// <summary>
        /// Sets the color of the RGBLed toy. <br/>
        /// The Alpha part of the RGBAColor is ignored.
        /// </summary>
        /// <param name="Color">RGBAColor object containg the brightness values for the color.</param>
        public void SetColor(RGBAColor Color)
        {
            SetColor(Color.Red, Color.Green, Color.Blue);
        }

        /// <summary>
        /// Sets the color of the RGB led toy. 
        /// </summary>
        /// <param name="Color">Hexadecimal color (e.g. \#ff0000 for red), comma separated color (e.g. 0,255,0 for green) or color name as defined in Cabinet.Colors.</param>
        public void SetColor(string Color)
        {
            if (_Cabinet.Colors.Contains(Color))
            {
                SetColor(_Cabinet.Colors[Color]);
            }
            else
            {
                SetColor(new RGBColor(Color));
            }
        }
    }
}
