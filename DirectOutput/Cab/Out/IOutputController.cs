using System;
using DirectOutput.General.Generic;
namespace DirectOutput.Cab.Out
{
    //TODO: Invent some common, proper interface for the AutoConfig of Output controllers which support automatic detection and configuration.
    /// <summary>
    /// Common interface for all outputcontrollers.
    /// The abstract OutputController class implements this interface and can be inherited for other output controller classes.
    /// It is important to ensure that all classes inherting this interface are XML serializeable.  
    /// </summary>
    public interface IOutputController: INamedItem
    {

        /// <summary>
        /// Must initialize the IOutputController.
        /// Is called after the objects have been instanciated.
        /// </summary>
        void Init();

        /// <summary>
        /// Must finish the IOutputController and do all necessary cleanup task.
        /// Must turn off the physical outputs of the IOutputController.
        /// </summary>
        void Finish();


        /// <summary>
        /// Name of the IOutputController.
        /// This property is fully implemented in the abstract OutputController class.
        /// </summary>
        new string Name { get; set; }

        /// <summary>
        /// OutputList containing the IOutput objects for a IOutputController.
        /// </summary>
        OutputList Outputs { get; set; }


        /// <summary>
        /// Must update the physical outputs if the IOutputController. 
        /// </summary>
        void Update();
    }
}
