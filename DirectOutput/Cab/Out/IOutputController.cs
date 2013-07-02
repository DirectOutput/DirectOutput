using System;
using DirectOutput.General.Generic;
namespace DirectOutput.Cab.Out
{
    /// <summary>
    /// Common interface for all outputcontrollers. Only classes implementing this interface can be used as output controllers in the framework.<br/>
    /// The abstract OutputControllerBase class implements this interface and can be inherited for other output controller classes.<br/>
    /// It is important to ensure that all classes inherting this interface are XML serializeable.  
    /// </summary>
    //TODO: Invent some common, proper interface for the AutoConfig of Output controllers which support automatic detection and configuration.
    //TODO: Revisit IOutputController interface to determine wether it would be a good idea to use a more generric approch for the outputs collection.
    public interface IOutputController : INamedItem
    {

        /// <summary>
        /// Must initialize the IOutputController.
        /// Is called after the objects have been instanciated.
        /// </summary>
        /// <param name="Pinball">The pinball object which is using the IOutputController instance.</param>
        void Init(Pinball Pinball);

        /// <summary>
        /// Must finish the IOutputController and do all necessary cleanup task.
        /// Must turn off the physical outputs of the IOutputController.
        /// </summary>
        void Finish();


        /// <summary>
        /// Name of the IOutputController.
        /// This property is fully implemented in the abstract OutputControllerBase class.
        /// </summary>
        new string Name { get; set; }

        /// <summary>
        /// OutputList containing the IOutput objects for a IOutputController.
        /// </summary>
        OutputList Outputs { get; set; }


        /// <summary>
        /// Must update the physical outputs of the IOutputController. 
        /// </summary>
        void Update();
    }
}
