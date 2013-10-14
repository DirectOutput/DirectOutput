using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.LWEquivalent
{
    /// <summary>
    /// The LEDWizEquivalent toy is only used by the framework when ini files are used for the configuration to determine which outputs should be controled by the columns in the ini files.<br />
    /// Do not target this type of toy with any effects.
    /// </summary>
    public class LedWizEquivalent:ToyBase,IToy
    {
        private LedWizEquivalentOutputList _Outputs=new LedWizEquivalentOutputList();




        /// <summary>
        /// Gets or sets the outputs of the LedWizEquivalent toy.
        /// </summary>
        /// <value>
        /// The outputs of the LedWizEquivalent toy.
        /// </value>
        public LedWizEquivalentOutputList Outputs
        {
            get { return _Outputs; }
            set { _Outputs = value; }
        }

        private int _LedWizNumber=-1;

        /// <summary>
        /// Gets or sets the number of the virtual LedWiz resp. ini file to be matched with the LedWizEquivalentToy.
        /// </summary>
        /// <value>
        /// The number of the virtual LedWiz resp. ini file to be matched with the LedWizEquivalentToy.
        /// </value>
        public int LedWizNumber
        {
            get { return _LedWizNumber; }
            set { _LedWizNumber = value; }
        }


        /// <summary>
        /// Initializes the LedwizEquivalent toy.
        /// </summary>
        /// <param name="Cabinet"><see cref="Cabinet" /> object to which the <see cref="LedWizEquivalent"/> belongs.</param>
        public override void Init(Cabinet Cabinet)
        {

            Outputs.Init(Cabinet); 
        }

        /// <summary>
        /// Resets all output to their default state.
        /// </summary>
        public override void Reset()
        {

        }

        /// <summary>
        ///Finishes the LedWizEquivalent toy.
        /// </summary>
        public override void Finish()
        {
            Outputs.Finish();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LedWizEquivalent"/> class.
        /// </summary>
        public LedWizEquivalent() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LedWizEquivalent"/> class.
        /// </summary>
        /// <param name="LedWiz">Reference to a LedWiz object used to configure the LedWizEquivalent.</param>
        public LedWizEquivalent(DirectOutput.Cab.Out.LW.LedWiz LedWiz)
        {
            this.LedWizNumber = LedWiz.Number;
            this.Name = "LedWizEquivalent {0}".Build(LedWiz.Number);
            foreach (DirectOutput.Cab.Out.IOutput O in LedWiz.Outputs)
            {
                Outputs.Add(new LedWizEquivalentOutput() {OutputName="{0}\\{1}".Build(LedWiz.Name,O.Name),LedWizEquivalentOutputNumber=((DirectOutput.Cab.Out.LW.LedWizOutput)O).LedWizOutputNumber});

            }
        }
       
    }
}
