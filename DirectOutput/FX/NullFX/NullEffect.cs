
/// <summary>
/// This namespace does only contain the NullEffect.
/// </summary>
namespace DirectOutput.FX.NullFX
{
    /// <summary>
    /// The NullEffect is a empty effect no doing anything.
    /// </summary>
    public class NullEffect:EffectBase
    {

        /// <summary>
        /// Triggers the NullEffect for the given TableElementData.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            //No work is done here
        }

        /// <summary>
        /// Init must do all necessary initialization work after the NullEffect object has been instanciated
        /// </summary>
        /// <param name="Pinball">Pinball object</param>
        public override void Init(Pinball Pinball)
        {
            //Nothing is happening here
        }
    }
}
