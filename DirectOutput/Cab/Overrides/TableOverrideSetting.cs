using DirectOutput.General.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Overrides {
    public class TableOverrideSetting : INamedItem{

        public TableOverrideSetting() { }

        //public ScheduledSetting (string Name, bool Enabled, int ClockStart, int ClockEnd, NamedItemList<ScheduledSetting> ScheduledSettingDevice) :this() {
        public TableOverrideSetting(string Name, bool Enabled) : this() {
            Log.Write("TableOverrideSetting constructor...name=" + Name);
            //Log.Write("ScheduledSetting constructor...ScheduledSettingDevice=" + ScheduledSettingDevice);
            /*this.Name = Name;
            this.Enabled = Enabled;
            this.ClockStart = ClockStart;
            this.ClockEnd = ClockEnd;*/
            //this.ScheduledSettingDevice = ScheduledSettingDevice;
        }

        /// <summary>
        /// Specifies whether this setting is in use in this instance / table, false by default.
        /// Gets set to either true or false after checking for rom/tablenames first time, and if false gets ignored during processing / triggers.
        /// </summary>
        public bool activeSetting = false;


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

        private NamedItemList<TableOverrideSettingDevice> _TableOverrideSettingDeviceList;
        /// <summary>
        /// List of devices and its outputs, and how to affect.<br/>
        /// </summary>    
        public NamedItemList<TableOverrideSettingDevice> TableOverrideSettingDeviceList {
            get { return _TableOverrideSettingDeviceList; }
            set { _TableOverrideSettingDeviceList = value; }
        }


        private string _Roms;
        /// <summary>
        /// Comma seperated list of roms identified by rom name to affect. Gets parsed into a list of strings that can be checked at runtime when outputs do get triggered. <br/>
        /// </summary>    
        public string Roms{
            get { return _Roms; }
            set {
                _Roms = value;

                //split string into an array, convert / cast all entries to int, return as list
                RomList = new List<string>(value.Split(','));
            }
        }

        /// <summary>
        /// Parsed rom files that can be checked at runtime when outputs do get triggered. <br/>
        /// </summary>    
        public List<string> RomList = new List<string>();



        private string _Tables;
        /// <summary>
        /// Comma seperated list of tables identified by actual filename to affect. Gets parsed into a list of strings that can be checked at runtime when outputs do get triggered. <br/>
        /// </summary>    
        public string Tables {
            get { return _Tables; }
            set {
                _Tables = value;

                //split string into an array, convert / cast all entries to int, return as list
                TableList = new List<string>(value.Split(','));
            }
        }

        /// <summary>
        /// Parsed tables that can be checked at runtime when outputs do get triggered. <br/>
        /// </summary>    
        public List<string> TableList = new List<string>();


    }
}
