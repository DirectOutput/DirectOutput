using System.Xml.Serialization;
using DirectOutput.Cab.Out;

namespace DirectOutput.Cab.Toys.LWEquivalent
{
    /// <summary>
    /// LEDWizEquivalentOutput is the output object for the LedWizEquivalent IToy.
    /// </summary>
    public class LedWizEquivalentOutput
    {
        /// <summary>
        /// Gets or sets the name of the IOutput object beeing controlled by the LedWizEquivalenOutput.
        /// </summary>
        /// <value>
        /// The name of the IOutput object beeing controlled by the LedWizEquivalenOutput.
        /// </value>
        public string OutputName { get; set; }

        /// <summary>
        /// Gets or sets the number of the LedWizEquivalentOutput.
        /// </summary>
        /// <value>
        /// The number of the LedWizEquivalentOutput.
        /// </value>
        public int LedWizEquivalentOutputNumber { get; set; }

        /// <summary>
        /// Gets or sets the value of the output.<br/>
        /// Valid values must be in the range of 0-48.
        /// </summary>
        /// <value>
        /// The value of the output.
        /// </value>
        [XmlIgnoreAttribute]
        public int Value
        {
            get
            {
                if (_Output != null)
                {
                    return (int)(_Output.Value/5.3125);
                }
                return 0;
            }
            set
            {
                if (_Output != null)
                {
                    _Output.Value = (byte)(value.Limit(0, 48) * 5.3125);
                }
            }
        }
        private IOutput _Output;

        /// <summary>
        /// Initalizes the LedWizEquivalentOutput.
        /// </summary>
        /// <param name="Cabinet">The cabinet to which the LedWizEquivalentOutput belongs.</param>
        public void Init(Cabinet Cabinet)
        {
            if (Cabinet.Outputs.Contains(OutputName))
            {
                _Output = Cabinet.Outputs[OutputName];
            }
            else
            {
                _Output = null;
            }
        }

        /// <summary>
        /// Resets the output to its default value.
        /// </summary>
        public void Reset()
        {
            Value = 0;
        }

        /// <summary>
        /// Finishes this instance and resets the output to its default value.
        /// </summary>
        public void Finish()
        {
            Value = 0;
            _Output = null;
        }
    }
}
