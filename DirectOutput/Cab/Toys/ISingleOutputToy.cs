using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys
{
    public interface ISingleOutputToy:IToy
    {


        /// <summary>
        /// Gets or sets the name of the output of the toy.
        /// </summary>
        /// <value>
        /// The name of the output.
        /// </value>
       
        string OutputName { get; set; }
    }
}
