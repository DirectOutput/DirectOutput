using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DirectOutput.Cab.Out
{
    //TODO: We can ldrop this class. It is not required.
    public class OutputNameConverter : StringConverter
    {

        /// <summary>
        /// Gets or sets the cabinet from which the type convert will extract the output names.<br/>
        /// </summary>
        /// <remarks>
        /// The use of a static property for this datasource is a ugly, but working ack! Dont try this at home! ;)
        /// </remarks>
        /// <value>
        /// The cabinet object from which the output name will be extracted.
        /// </value>
        public static Cabinet Cabinet { get; set; }


        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            
            if (Cabinet != null)
            {
                return new StandardValuesCollection(Cabinet.Outputs.Select(O=>O.Name).ToArray());
            }
            else
            {
                return new StandardValuesCollection(new string[] {});
            }


        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
