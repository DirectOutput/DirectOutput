using System;
using DirectOutput.General.Generic;
namespace DirectOutput.Cab.Out
{
    /// <summary>
    /// Common interface for all outputcontrollers. Only classes implementing this interface can be used as output controllers in the framework.<br/>
    /// The abstract OutputControllerBase class implements this interface and can be inherited for other output controller classes.<br/>
    /// It is important to ensure that all classes inherting this interface are XML serializeable.  
    /// </summary>
    public interface IOutputController : INamedItem
    {


        /// <summary>
        /// Must initialize the IOutputController.
        /// Is called after the objects have been instanciated.
        /// </summary>
        /// <param name="Cabinet">The cabinet object which is using the IOutputController instance.</param>
        void Init(Cabinet Cabinet);

        /// <summary>
        /// Must finish the IOutputController and do all necessary cleanup task.
        /// Must turn off the physical outputs of the IOutputController.
        /// </summary>
        void Finish();


        /// <summary>
        /// Name of the IOutputController.
        /// This property is fully implemented in the abstract OutputControllerBase class.
        /// </summary>
        /// <value>
        /// The name of the output controller.
        /// </value>
        new string Name { get; set; }

        /// <summary>
        /// OutputList containing the IOutput objects for a IOutputController.
        /// </summary>
        OutputList Outputs { get; set; }


        /// <summary>
        /// Must update resp. thrigger the update of the physical outputs of the IOutputController. <br/>
        /// \remark Since communication with external components can be slow, it is a good practice to send the actual updates from a separate thread and to use this method only to notify the updater thread that data has to be sent.
        /// </summary>
        void Update();
    }
}
