using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Out
{

    /// <summary>
    /// EventArgs for events of IOutput objects.
    /// </summary>
    public class OutputEventArgs : EventArgs
    {

        /// <summary>
        /// IOutput which has triggered the event.
        /// </summary>
        public IOutput Output { get; set; }

        /// <summary>
        /// Value of the IOutput which has triggered the event.
        /// </summary>
        public int Value { get { return Output.Value; } }

        /// <summary>
        /// Name of the IOutput which has triggered the event
        /// </summary>
        public string Name { get { return Output.Name; } }

        public OutputEventArgs(IOutput Output)
        {
            this.Output = Output;
        }

        public OutputEventArgs() { }
    }
}
