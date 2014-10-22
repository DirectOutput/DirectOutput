using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab
{
    public class CabinetOwner : ICabinetOwner
    {

        #region ICabinetOwner Member

        Dictionary<string, object> _ConfigurationSettings = new Dictionary<string, object>();
        public Dictionary<string, object> ConfigurationSettings
        {
            get { return _ConfigurationSettings; }
            set { _ConfigurationSettings = value; }
        }

        public PinballSupport.AlarmHandler Alarms{get;set;}

        #endregion
    }
}
