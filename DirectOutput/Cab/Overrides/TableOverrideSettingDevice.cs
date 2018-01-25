using DirectOutput.General.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Overrides {
    public class TableOverrideSettingDevice : INamedItem {

        public TableOverrideSettingDevice() { }

        public TableOverrideSettingDevice(string Name, int ConfigPostfixID, string Outputs, int OutputPercent) :this() {
            Log.Write("TableOverrideSettingDevice constructor...name=" + Name);
            this.Name = Name;
            this.ConfigPostfixID = ConfigPostfixID;
            this.Outputs = Outputs;
            this.OutputPercent = OutputPercent;
        }
            

        private string _Name;
        /// <summary>
        /// Name of the Named item.<br/>
        /// </summary>    
        public string Name {
            get { return _Name; }
            set {
                if (_Name != value) {
                    string OldName = _Name;
                    if (BeforeNameChanged != null) {
                        BeforeNameChanged(this, new NameChangeEventArgs(OldName, value));
                    }

                    _Name = value;

                    if (AfterNameChanged != null) {
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
        public event EventHandler<NameChangeEventArgs> BeforeNameChanged;

        private int _ConfigPostfixID;
        /// <summary>
        /// Device ID as used for config XML postfix (27=UIO).<br/>
        /// </summary>    
        public int ConfigPostfixID {
            get { return _ConfigPostfixID; }
            set { _ConfigPostfixID = value; }
        }

        private string _Outputs;
        /// <summary>
        /// Comma seperated list of outputs 1-> to affect. Gets parsed into a list of ints that can be checked at runtime when outputs do get triggered. <br/>
        /// </summary>    
        public string Outputs {
            get { return _Outputs; }
            set {
                _Outputs = value;

                //split string into an array, convert / cast all entries to int, return as list
                OutputList = new List<int>(Array.ConvertAll(value.Split(','), int.Parse));
            }
        }

        /// <summary>
        /// Parsed Outputs of ints that can be checked at runtime when outputs do get triggered. <br/>
        /// </summary>    
        public List<int> OutputList = new List<int>();

        private int _OutputPercent;
        /// <summary>
        /// Output value in percent 0-100. to devices specified in OutputList. 0 = disabled / turned off completely. 100 = untouched (input strength).<br/>
        /// </summary>    
        public int OutputPercent {
            get { return _OutputPercent; }
            set { _OutputPercent = value; }
        }


    }
}
