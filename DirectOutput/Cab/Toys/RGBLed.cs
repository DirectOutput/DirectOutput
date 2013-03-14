using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DirectOutput.Cab.Out;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
namespace DirectOutput.Cab.Toys
{

    /// <summary>
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
        /// <param name="Cabinet">Cabinet to which the RGBLed belongs.</param>
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
        public int BrightnessRed
        {
            get { return _BrightnessRed; }
            set
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
        public int BrightnessGreen
        {
            get { return _BrightnessGreen; }
            set
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
        public int BrightnessBlue
        {
            get { return _BrightnessBlue; }
            set
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
            BrightnessRed = 0;
            BrightnessBlue = 0;
            BrightnessGreen = 0;
          
        }

        /// <summary>
        /// Sets the color of the RGBLed toy.
        /// </summary>
        /// <param name="Red">Red brightness</param>
        /// <param name="Green">Green brightness</param>
        /// <param name="Blue">Blue brightness</param>
        public void SetColor(int Red, int Green, int Blue)
        {
            BrightnessRed = Red;
            BrightnessBlue = Blue;
            BrightnessGreen = Green;

        }


        /// <summary>
        /// Sets the color of the RGBLed toy.
        /// </summary>
        /// <param name="Color">Color object containg the brightness values for the color.</param>
        public void SetColor(Color Color) {
            SetColor(Color.BrightnessRed,BrightnessGreen,BrightnessBlue);
        }


        /// <summary>
        /// Sets the color of the RGBLed toy. 
        /// </summary>
        /// <param name="Color">Hexadecimal color (e.g. \#ff0000 for red), comma separated color (e.g. 0,255,0 for green) or color name as defined in Cabinet.Colors.</param>
        public void SetColor(string Color)
        {
            SetColor(_Cabinet.Colors[Color]);
        }
    }
}
