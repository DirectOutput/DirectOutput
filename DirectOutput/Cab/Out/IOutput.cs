using System;
using DirectOutput.General.Generic;
namespace DirectOutput.Cab.Out
{

    /// <summary>
    /// Common interface for outputs of any output controller.
    /// The Output class implements this interface and can be inherited for the implementation of other output types.
    /// It is important to ensure that all classes inherting this interface are XML serializeable.
    /// </summary>
    public interface IOutput:INamedItem
    {


        /// <summary>
        /// Value of the output
        /// The implementation of this property has to be thread safe
        /// </summary>
        byte Value { get; set; }


        /// <summary>
        /// This event has to fire if the Value of the output is changed 
        /// </summary>
        event Output.ValueChangedEventHandler ValueChanged;
    }
}
