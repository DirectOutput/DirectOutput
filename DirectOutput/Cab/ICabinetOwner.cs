using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.PinballSupport;

namespace DirectOutput.Cab
{
    public interface ICabinetOwner
    {
        /// <summary>
        /// Gets the configuration settings.
        /// This dict can contain settings which are used by the output controllers.
        /// </summary>
        /// <value>
        /// The configuration settings.
        /// </value>
        Dictionary<string, object> ConfigurationSettings { get; }

        /// <summary>
        /// Gets the AlarmHandler object  
        /// </summary>
        /// <value>
        /// The AlarmHandler object.
        /// </value>
        AlarmHandler Alarms { get; set; }
    }
}
