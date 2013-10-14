using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Color
{
    /// <summary>
    /// Named RGBColor object
    /// </summary>
   public class RGBColorNamed:RGBColor, INamedItem
    {

        #region Name
        private string _Name;
        /// <summary>
        /// Name of the color.<br />
        /// </summary>
        /// <value>
        /// The name of the color.
        /// </value>
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
        /// Initializes a new instance of the <see cref="RGBColorNamed"/> class.
        /// </summary>
        public RGBColorNamed() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RGBColorNamed"/> class.
        /// </summary>
        /// <param name="Name">The name of the color.</param>
        /// <param name="BrightnessRed">The brightness for red.</param>
        /// <param name="BrightnessGreen">The brightness for green.</param>
        /// <param name="BrightnessBlue">The brightness for blue.</param>
        public RGBColorNamed(string Name, int BrightnessRed, int BrightnessGreen, int BrightnessBlue)
            : this()
        {
            this.Name = Name;
            SetColor(BrightnessRed, BrightnessGreen, BrightnessBlue);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RGBColorNamed"/> class.
        /// The parameter string <paramref name="Color"/> ist first parsed for hexadecimal color codes and afterwards checked for comma separated color values.
        /// </summary>
        /// <param name="Name">The name of the color.</param>
        /// <param name="Color">The color string.</param>
        public RGBColorNamed(string Name, string Color)
        {
            SetColor(Color);
            this.Name = Name;
        }

        #endregion
    }
}
