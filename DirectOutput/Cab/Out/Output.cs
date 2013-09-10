using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Out
{

    /// <summary>
    /// Basic IOutput implementation.
    /// </summary>
    public class Output : NamedItemBase, IOutput
    {

        private object ValueChangeLocker = new object();
        private byte _Value = 0;

        /// <summary>
        /// Value of the Output.<br/>
        /// Valid value range is 0-255.
        /// </summary>
        [XmlIgnoreAttribute]
        public byte Value
        {
            get { return _Value; }
            set
            {
                byte OldValue;
                lock (ValueChangeLocker)
                {
                    OldValue = _Value;
                    _Value = value;
                }
                if (OldValue!=value) OnValueChanged();
            }
        }

        #region Events
        #region "ValueChanged Event"
        protected void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, new OutputEventArgs(this));
            }
        }

        /// <summary>
        /// Event fires if the Value property of the Ouput is changed 
        /// </summary>
        public event ValueChangedEventHandler ValueChanged;

        public delegate void ValueChangedEventHandler(object sender, OutputEventArgs e);



        #endregion

        #endregion



    }
}
