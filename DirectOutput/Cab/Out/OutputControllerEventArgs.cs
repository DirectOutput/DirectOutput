using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Out
{

    /// <summary>
    /// EventArgs for events of IOutputControllers
    /// </summary>
    public class OutputControllerEventArgs : EventArgs
    {

        /// <summary>
        /// IOutputControler which has triggered the event
        /// </summary>
        public IOutputController OutputController { get; set; }


        /// <summary>
        /// Name of the IOutputController which has triggered the event
        /// </summary>
        public string Name { get { return OutputController.Name; } }

        public OutputControllerEventArgs(IOutputController OutputController)
        {
            this.OutputController = OutputController;
        }

        public OutputControllerEventArgs() { }
    }
}
