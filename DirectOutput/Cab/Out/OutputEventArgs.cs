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
        /// IOutput objec^t which has triggered the event.
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

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputEventArgs"/> class.
        /// </summary>
        /// <param name="Output">The IOutput object triggering the event.</param>
        public OutputEventArgs(IOutput Output)
        {
            this.Output = Output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputEventArgs"/> class.
        /// </summary>
        public OutputEventArgs() { }
    }
}
