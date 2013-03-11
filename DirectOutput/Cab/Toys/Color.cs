using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.General.Generic;

namespace DirectOutput.Cab.Toys
{

    /// <summary>
    /// This class stores information on colors used for toys (e.g. RGBLed).
    /// </summary>
    public class Color:NamedItemBase,INamedItem
    {
        

        private int _BrightnessRed;

        /// <summary>
        /// Brightness for Red. 
        /// </summary>
        /// <value>Brightness between 0 and 255.</value>
        [XmlIgnoreAttribute]
        public int BrightnessRed
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
        public int BrightnessGreen
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
        public int BrightnessBlue
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
                return "#{0:X2}{1:X2}{2:X2}".Build(BrightnessRed, BrightnessBlue, BrightnessGreen);
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
            BrightnessRed = Red;
            BrightnessBlue = Blue;
            BrightnessGreen = Green;
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
        public Color() { }
        public Color(string Name, int BrightnessRed, int BrightnessGreen, int BrightnessBlue)
            : this()
        {
            this.Name = Name;
            SetColor(BrightnessRed, BrightnessGreen, BrightnessBlue);
        }
        public Color(int BrightnessRed, int BrightnessGreen, int BrightnessBlue)
            : this("", BrightnessRed, BrightnessGreen, BrightnessBlue)
        { }
        public Color(string Color):this()
        {
            SetColor(Color);
        }
        public Color(string Name, string Color):this(Color)
        {
            this.Name = Name;
        }

        #endregion
    }
}
