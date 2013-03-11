using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DirectOutput.General.Generic;


/// <summary>
/// The DirectOutput.FX.ListFX namepsace contains the classes for the ListEffect.
/// </summary>
namespace DirectOutput.FX.ListFX
{

    /// <summary>
    /// IEffect class which handles a list of other IEffect objects.<br/>
    /// Attention! Be careful not to add ListEffect objects which finnaly contain a reference to the instance you're working with. This will create a recursive loop which never exit!.
    /// </summary>
    public class ListEffect : EffectBase
    {
       

        private AssignedEffectList _Effects;

        /// <summary>
        /// Gets or sets the list of effects assigned to the ListEffect.
        /// </summary>
        /// <value>
        /// The list of effects assigned to the ListEffect.
        /// </value>
        public AssignedEffectList Effects
        {
            get { return _Effects; }
            set { _Effects = value; }
        }
        

        /// <summary>
        /// Triggers all effects in the ListEffect.
        /// </summary>
        /// <param name="TableElement">TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElement TableElement)
        {

            Effects.Trigger(TableElement);
        }


        /// <summary>
        /// Initializes the ListEffect.
        /// </summary>
        public override void Init(Pinball Pinball)
        {
            Effects.Init(Pinball);
        }


        /// <summary>
        /// Fnishes the ListEffect.
        /// </summary>
        public override void Finish()
        {
            Effects.Finish();

        }



    }
}
