using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Out.LW
{

    /// <summary>
    /// Output class for LedWiz output controllers.
    /// </summary>
    public class LedWizOutput : Output, IOutput
    {

        private int _LedWizOutputNumber;
        /// <summary>
        /// Number of the Output of the LedWiz.
        /// </summary>
        /// <value>
        /// The LedWiz output number.
        /// </value>
        /// <exception cref="System.Exception">LedWiz output numbers must be in the range of 1-32. The supplied number {0} is out of range.</exception>
        public int LedWizOutputNumber
        {
            get
            {
                return _LedWizOutputNumber;
            }
            set
            {
                if (_LedWizOutputNumber != value)
                {
                    if (!value.IsBetween(1, 32))
                    {
                        throw new Exception("LedWiz output numbers must be in the range of 1-32. The supplied number {0} is out of range.".Build(value));
                    }
                    if (Name.IsNullOrWhiteSpace() || Name == "LedWizOutput {0:00}".Build(_LedWizOutputNumber))
                    {
                        Name = "LedWizOutput {0:00}".Build(value);
                    }
                    _LedWizOutputNumber = value;
                }
            }
        }


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LedWizOutput"/> class.
        /// </summary>
        public LedWizOutput()
        {

        }

        /// <summary>
        /// LedWizOutput instance with the specified LedWizOutputNumber
        /// </summary>
        public LedWizOutput(int LedWizOutputNumber)
        {

            this.LedWizOutputNumber = LedWizOutputNumber;
            //TODO: Generate unique output names
            this.Name = string.Format("LedWizOutput {0:00}", LedWizOutputNumber);
            this.Value = 0;
        }
        #endregion
    }
}
