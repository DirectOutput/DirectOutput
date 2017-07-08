using DirectOutput.General.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Sequencer {
    public class SequentialOutputDevice: INamedItem {

        public SequentialOutputDevice() { }

        public SequentialOutputDevice(string Name, int ConfigPostfixID, string Outputs, int OutputMaxTime) :this() {
            Log.Write("SequentialOutputDevice constructor...name=" + Name);
            this.Name = Name;
            this.ConfigPostfixID = ConfigPostfixID;
            this.Outputs = Outputs;
            this.OutputMaxTime = OutputMaxTime;
            this.OutputTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            this.OutputIndex = 0;
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


        private int _OutputMaxTime;
        /// <summary>
        /// Max life of active output in milliseconds until triggering next device in OutputList.<br/>
        /// </summary>    
        public int OutputMaxTime {
            get { return _OutputMaxTime; }
            set { _OutputMaxTime = value; }
        }

        private long _OutputTimestamp;
        /// <summary>
        /// Last timestamp value in milliseconds.<br/>
        /// </summary>    
        public long OutputTimestamp {
            get { return _OutputTimestamp; }
            set { _OutputTimestamp = value; }
        }

        private int _OutputIndex;
        /// <summary>
        /// Last output index used.<br/>
        /// </summary>    
        public int OutputIndex {
            get { return _OutputIndex; }
            set { _OutputIndex = value; }
        }

        /// <summary>
        /// Sets next OutputIndex, and returns value for that index. If beyond length of outputs, reset back to first / primary output.
        /// </summary>
        /// <returns></returns>
        public int getsetnextoutputValue() {
            int nextValue = 0;

            OutputIndex++;

            if (OutputIndex == OutputList.Count) {
                OutputIndex = 0;
            }

            nextValue = OutputList[OutputIndex];

            return nextValue;
        }

    }
}
