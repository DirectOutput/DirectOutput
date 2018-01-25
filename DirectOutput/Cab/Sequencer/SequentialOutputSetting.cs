using DirectOutput.General.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Sequencer {
    public class SequentialOutputSetting: INamedItem{

        public SequentialOutputSetting() { }

        //public ScheduledSetting (string Name, bool Enabled, int ClockStart, int ClockEnd, NamedItemList<ScheduledSetting> ScheduledSettingDevice) :this() {
        public SequentialOutputSetting(string Name, bool Enabled) : this() {
            Log.Write("SequentialOutputSetting constructor...name=" + Name);
            //Log.Write("ScheduledSetting constructor...ScheduledSettingDevice=" + ScheduledSettingDevice);
            /*this.Name = Name;
            this.Enabled = Enabled;
            this.ClockStart = ClockStart;
            this.ClockEnd = ClockEnd;*/
            //this.ScheduledSettingDevice = ScheduledSettingDevice;
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

        private bool _Enabled;
        /// <summary>
        /// Specifies if setting is enabled or disabled.<br/>
        /// </summary>    
        public bool Enabled {
            get { return _Enabled; }
            set { _Enabled = value; }
        }

        private NamedItemList<SequentialOutputDevice> _SequentialOutputDeviceList;
        /// <summary>
        /// List of devices and its outputs, and how to affect.<br/>
        /// </summary>    
        public NamedItemList<SequentialOutputDevice> SequentialOutputDeviceList {
            get { return _SequentialOutputDeviceList; }
            set { _SequentialOutputDeviceList = value; }
        }



    }
}
