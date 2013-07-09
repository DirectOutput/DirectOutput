using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Out
{
    /// <summary>
    /// Interface for classes used to automatically detect and configure output controllers
    /// </summary>
    public interface IAutoConfigOutputController
    {
        /// <summary>
        /// This method has to detect configure IOutputController object automatically.<br/>
        /// \note: Make sure that implementations of the method do also work correctly if a IoutputController has allready been added through config files. If a IOutputController does already exist, the method should skip it and try to find other controllers.
        /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        void AutoDetect(Cabinet Cabinet);

    }
}
