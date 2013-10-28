

/// <summary>
/// The DirectOutput.FX.ListFX namepsace contains the classes for the ListEffect.
/// </summary>
namespace DirectOutput.FX.ListFX
{

    /// <summary>
    /// This effect triggers a list of other effect when it is triggered.<br/>
    ///  
    /// \warning Be careful not to add ListEffect objects which finnaly contain a reference to the instance you're working with. This will create a recursive loop which never exit!.
    /// </summary>
    public class ListEffect : EffectBase
    {
       

        private AssignedEffectList _AssignedEffects;

        /// <summary>
        /// Gets or sets the list of effects assigned to the ListEffect.
        /// </summary>
        /// <value>
        /// The list of effects assigned to the ListEffect.
        /// </value>
        public AssignedEffectList AssignedEffects
        {
            get { return _AssignedEffects; }
            set { _AssignedEffects = value; }
        }
        

        /// <summary>
        /// Triggers all effects assigned to the ListEffect.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            AssignedEffects.Trigger(TableElementData);
        }


        /// <summary>
        /// Initializes the ListEffect.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public override void Init(Table.Table Table)
        {
            AssignedEffects.Init(Table);
        }


        /// <summary>
        /// Fnishes the ListEffect.
        /// </summary>
        public override void Finish()
        {
            AssignedEffects.Finish();

        }



    }
}
