using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Color
{
   public class RGBColorNamed:RGBColor, INamedItem
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
        public RGBColorNamed() { }
        public RGBColorNamed(string Name, int BrightnessRed, int BrightnessGreen, int BrightnessBlue)
            : this()
        {
            this.Name = Name;
            SetColor(BrightnessRed, BrightnessGreen, BrightnessBlue);
        }


        public RGBColorNamed(string Name, string Color)
        {
            SetColor(Color);
            this.Name = Name;
        }

        #endregion
    }
}
