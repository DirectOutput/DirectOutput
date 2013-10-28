using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.General.Generic;

namespace DirectOutput.Cab.Color
{

    /// <summary>
    /// This class stores information on colors used for toys (e.g. RGBLed).
    /// </summary>
    public class RGBAColorNamed : RGBAColor, INamedItem
    {


        #region Name
        private string _Name;
        /// <summary>
        /// Name of the Named item.<br/>
        /// Triggers BeforeNameChange before a new Name is set.<br/>
        /// Triggers AfterNameChanged after a new name has been set.
        /// </summary>    
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    string OldName = _Name;
                    if (BeforeNameChange != null)
                    {
                        BeforeNameChange(this, new NameChangeEventArgs(OldName, value));
                    }

                    _Name = value;

                    if (AfterNameChanged != null)
                    {
                        AfterNameChanged(this, new NameChangeEventArgs(OldName, value));
                    }
                }
            }
        }
        /// <summary>
        /// Event is fired after the value of the property Name has changed.
        /// </summary>
        public event EventHandler<NameChangeEventArgs> AfterNameChanged;

        /// <summary>
        /// Event is fired before the value of the property Name is changed.
        /// </summary>
        public event EventHandler<NameChangeEventArgs> BeforeNameChange;
        #endregion

        #region Contructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RGBAColorNamed"/> class.
        /// </summary>
        public RGBAColorNamed() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RGBAColorNamed"/> class.
        /// If all color components are set to 0, the alpha value will be set to 0, otherwise the alpha value will be set to 255.
        /// </summary>
        /// <param name="Name">The name of the color.</param>
        /// <param name="BrightnessRed">The brightness for red.</param>
        /// <param name="BrightnessGreen">The brightness for green.</param>
        /// <param name="BrightnessBlue">The brightness for blue.</param>
        public RGBAColorNamed(string Name, int BrightnessRed, int BrightnessGreen, int BrightnessBlue)
            : this()
        {
            this.Name = Name;
            SetColor(BrightnessRed, BrightnessGreen, BrightnessBlue);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RGBAColorNamed"/> class.
        /// </summary>
        /// <param name="Name">The name of the color.</param>
        /// <param name="BrightnessRed">The brightness for red.</param>
        /// <param name="BrightnessGreen">The brightness for green.</param>
        /// <param name="BrightnessBlue">The brightness for blue.</param>
        /// <param name="Alpha">The alpha value for the color.</param>
        public RGBAColorNamed(string Name, int BrightnessRed, int BrightnessGreen, int BrightnessBlue,int Alpha)
            : this()
        {
            this.Name = Name;
            SetColor(BrightnessRed, BrightnessGreen, BrightnessBlue,Alpha);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RGBAColorNamed"/> class.
        /// The parameter string <paramref name="Color"/> is first parsed for hexadecimal color codes and afterwards checked for comma separated color values.<br/>
        /// The following values are accepted:<br/>
        /// 
        /// * Hexadecimal color code including alpha channel (e.g. &#35;ff0000FF for fully opaque red).<br/>
        /// * Hexadecimal color code without alpha channel (e.g. &#35;ff0000 for red). If all color components are set to 0 (that equals black) the alpha value is set to 0 (fully transparent), otherwise to 255 (fully opaque).
        /// * Comma separated color components including alpha channel (e.g. 255,0,0,128 for half transparent red).
        /// * Comma separated color components without alpha channel (e.g. 255,0,0 for for fully opque red). If all color components are set to 0 (that equals black) the alpha value is set to 0 (fully transparent), otherwise to 255 (fully opaque).
        /// </summary>
        /// <param name="Name">The name of the color.</param>
        /// <param name="Color">The color string.</param>
        public RGBAColorNamed(string Name, string Color)
        {
            SetColor(Color);
            this.Name = Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBAColorNamed"/> class.
        /// The Alpha value is set to 0 if all color components are set to 0, otherwise the Alpha value will be set to 255.
        /// </summary>
        /// <param name="Name">The name of the color.</param>
        /// <param name="RGBColor">RGBColor object.</param>
        public RGBAColorNamed(string Name, RGBColor RGBColor)
            
        {
            SetColor(RGBColor);
            this.Name = Name;
        }


        #endregion
    }
}
