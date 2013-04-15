using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Out.NullOutputController
{

    /// <summary>
    /// This is a dummy output controller not doing anthing with the data it receives.<br/>
    /// It is mainly thought as a sample how to implement a output controller.<br/>
    /// <remarks>Be sure to check the OutputControllerBase class and the IOutputController interface for a better understanding.</remarks>
    /// </summary>
    public class NullOutputController: OutputControllerBase, IOutputController
    {
        

        /// <summary>
        /// Init initializes the ouput controller.<br/>
        /// This method is called after the objects haven been instanciated.
        /// </summary>
        public  void Init() {
        }

        /// <summary>
        /// Finishes the ouput controller.<br/>
        /// All necessary cleanup tasks have to be implemented here und all physical outputs have to be turned off.
        /// </summary>
        public  void Finish() {
            
        }

        /// <summary>
        /// Update must update the physical outputs to the values defined in the Outputs list. 
        /// </summary>
        public  void Update() {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullOutputController"/> class.
        /// </summary>
        public NullOutputController()
        {

        }

    }
}
