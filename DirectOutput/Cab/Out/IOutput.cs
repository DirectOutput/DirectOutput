using System;
using DirectOutput.General.Generic;
namespace DirectOutput.Cab.Out
{

    /// <summary>
    /// Common interface for outputs of any output controller.
    /// The Output class implements this interface and can be inherited for the implementation of other output types.
    /// </summary>
    public interface IOutput:INamedItem
    {


        /// <summary>
        /// Value of the output.<br/>
        /// \remark The implementation of this property has to be thread safe
        /// </summary>
        byte Value { get; set; }


        /// <summary>
        /// Gets or sets the number of the Output object.
        /// </summary>
        /// <value>
        /// The number of the Numbered object.
        /// </value>
        int Number { get; set; }


        /// <summary>
        /// This event has to fire if the Value of the output is changed 
        /// </summary>
        event Output.ValueChangedEventHandler ValueChanged;
    }
}
