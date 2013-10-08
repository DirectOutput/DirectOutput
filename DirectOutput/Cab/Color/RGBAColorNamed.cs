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
        public RGBAColorNamed() { }
        public RGBAColorNamed(string Name, int BrightnessRed, int BrightnessGreen, int BrightnessBlue)
            : this()
        {
            this.Name = Name;
            SetColor(BrightnessRed, BrightnessGreen, BrightnessBlue);
        }
        public RGBAColorNamed(string Name, int BrightnessRed, int BrightnessGreen, int BrightnessBlue,int Alpha)
            : this()
        {
            this.Name = Name;
            SetColor(BrightnessRed, BrightnessGreen, BrightnessBlue,Alpha);
        }
        

        public RGBAColorNamed(string Name, string Color)
        {
            SetColor(Color);
            this.Name = Name;
        }

        public RGBAColorNamed(string Name, RGBColor RGBColor)
            
        {
            SetColor(RGBColor);
            this.Name = Name;
        }


        #endregion
    }
}
