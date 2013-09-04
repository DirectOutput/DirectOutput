using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.General.Generic;

namespace DirectOutput.Cab.Out
{

    /// <summary>
    /// Abstract OutputController base class to be used for IOutputController implementations.<br/>
    /// Implements IOutputController.
    /// </summary>
    /// <remarks>Remember that IOutputController does inherit INamedItem, so the members of that interface have to be implemented as well. The easiest way to achiev this is to inherit the NamedItemBase class.</remarks>
    public abstract class OutputControllerBase : NamedItemBase, IOutputController
    {
        




        private OutputList _Outputs=null;
        /// <summary>
        /// Contains the OutputList object for the outputs of the output controller.<br/>
        /// \remark This property is marked with the XMLIgnore attributte so its content does not get serialized, which means that output controller implementations inherting from this class have to create the output objects in the list.<br/>The XMLIgnore attribute is inherted to classes inheriting from this base class. This means that inherited classes do not serialize this property unless a specific serialization implementation is used in that class.
        /// </summary>
        [XmlIgnore]
        public virtual OutputList Outputs
        {
            get { return _Outputs; }
            set
            {
                if (_Outputs != null)
                {
                    _Outputs.OutputValueChanged -= new OutputList.OutputValueChangedEventHandler(Outputs_OutputValueChanged);
                }

                _Outputs = value;

                if (_Outputs != null)
                {
                    _Outputs.OutputValueChanged += new OutputList.OutputValueChangedEventHandler(Outputs_OutputValueChanged);

                }

            }
        }

        private void Outputs_OutputValueChanged(object sender, OutputEventArgs e)
        {
            OnOutputValueChanged(e.Output);
        }

        /// <summary>
        /// This method is called whenever the value of a output in the Outputs property changes its value.<br/>
        /// It doesn't do anything in this base class, but it can be overwritten (user override) in classes inherting the base class.
        /// </summary>
        /// <param name="Output">The output.</param>
        public virtual void OnOutputValueChanged(IOutput Output)
        {

        }





        

        /// <summary>
        /// Init must be overwritten and must initialize the ouput controller.<br />
        /// This method is called after the objects haven been instanciated.
        /// </summary>
        /// <param name="Cabinet">The Cabinet object which is using the IOutputController instance.</param>
        public abstract void Init(Cabinet Cabinet);

        /// <summary>
        /// Finish must be overwritten and must finish the ouput controller.<br/>
        /// All necessary cleanup tasks have to be implemented here und all physical outputs have to be turned off.
        /// </summary>
        public abstract void Finish();

        /// <summary>
        /// Update must update the physical outputs to the values defined in the Outputs list. 
        /// \remark Since communication with external components can be slow, it is a good practice to send the actual updates from a separate thread and to use this method only to notify the updater thread that data has to be sent.
        /// </summary>
        public abstract void Update();

    }
}
