using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.General.Generic;

namespace DirectOutput.General.Color
{

    /// <summary>
    /// This class stores information on RGB colors used for toys and effects (e.g. RGBLed).
    /// </summary>
    public class RGBColor 
    {



 

        private int _BrightnessRed;

        /// <summary>
        /// Brightness for Red. 
        /// </summary>
        /// <value>Brightness between 0 and 255.</value>
        [XmlIgnoreAttribute]
        public int Red
        {
            get { return _BrightnessRed; }
            set { _BrightnessRed = value.Limit(0,255); }
        }

        private int _BrightnessGreen;

        /// <summary>
        /// Brightness for Green.
        /// </summary>
        /// <value>Brightness between 0 and 255.</value>
        [XmlIgnoreAttribute]
        public int Green
        {
            get { return _BrightnessGreen; }
            set { _BrightnessGreen = value.Limit(0, 255); }
        }

        private int _BrightnessBlue;
        /// <summary>
        /// Brightness for Blue.
        /// </summary>
        /// <value>Brightness between 0 and 255.</value>
        [XmlIgnoreAttribute]
        public int Blue
        {
            get { return _BrightnessBlue; }
            set { _BrightnessBlue = value.Limit(0, 255); }
        }


        /// <summary>
        /// Returns the hexadecimal code for the color.
        /// </summary>
        /// <value>6 digit hexadecimal color code with leading  &#35;(e.g. &#35;ff0000 for red).</value>
        public string HexColor
        {
            get
            {
                return "#{0:X2}{1:X2}{2:X2}".Build(Red,  Green,Blue);
            }
            set {
                SetColor(value);
            }
        }

        /// <summary>
        /// Sets the RGB components of the Color.
        /// </summary>
        /// <param name="Red">Red brightness</param>
        /// <param name="Green">Green brightness</param>
        /// <param name="Blue">Blue brightness</param>
        /// <returns>true</returns>
        public bool SetColor(int Red, int Green, int Blue)
        {
            this.Red = Red;
            this.Blue = Blue;
            this.Green = Green;
            return true;
        }

        /// <summary>
        /// Sets the RGB components of the Color.<br/>
        /// The parameter string <paramref name="Color"/> ist first parsed for hexadecimal color codes and afterwards checked for comma separated color values.
        /// </summary>
        /// <param name="Color">Hexadecimal color code (e.g. &#35;ff0000 for red) or comma separated color (e.g. 0,255,0 for green).</param>
        /// <returns>true if the parameter string contained a valid color codes, otherwise false.</returns>
        public bool SetColor(string Color)
        {
            if ((Color.Length == 6 && Color.IsHexString()) || (Color.Length == 7 && Color.StartsWith("#") && Color.IsHexString(1, 6)))
            {
                int Offset;
                if (Color.StartsWith("#"))
                {
                    Offset = 1;
                }
                else
                {
                    Offset = 0;
                }
                SetColor(Color.Substring(0 + Offset, 2).HexToInt(), Color.Substring(2 + Offset, 2).HexToInt(), Color.Substring(4 + Offset, 2).HexToInt());
                return true;
            };

            string[] SplitColors = Color.Split(',');
            if (SplitColors.Length == 3)
            {
                bool ColorsOK = true;
                foreach (string C in SplitColors)
                {
                    if (C.IsInteger())
                    {
                        ColorsOK = false;
                    }
                }
                if (ColorsOK)
                {
                    SetColor(int.Parse(SplitColors[0]), int.Parse(SplitColors[1]), int.Parse(SplitColors[2]));
                    return true;
                }
            }
            return false;
        }

        #region Contructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RGBColor"/> class.
        /// </summary>
        public RGBColor() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBColor"/> class.
        /// </summary>
        /// <param name="BrightnessRed">The brightness for red.</param>
        /// <param name="BrightnessGreen">The brightness for green.</param>
        /// <param name="BrightnessBlue">The brightness for blue.</param>
        public RGBColor(int BrightnessRed, int BrightnessGreen, int BrightnessBlue){
            SetColor(BrightnessRed, BrightnessGreen, BrightnessBlue);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RGBColor"/> class.
        /// The parameter string <paramref name="Color"/> ist first parsed for hexadecimal color codes and afterwards checked for comma separated color values.
        /// </summary>
        /// <param name="Color">The color string.</param>
        public RGBColor(string Color)
        {
            SetColor(Color);
        }


        #endregion
    }
}
