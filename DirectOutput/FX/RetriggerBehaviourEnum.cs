
namespace DirectOutput.FX
{
    /// <summary>
    /// This enum describes the different retrigger behaviours for the effects.<br/>
    /// Retriggering means that a effect is getting another Trigger call with the same table element value as the last call, while it is still active.
    /// </summary>
    public enum RetriggerBehaviourEnum
    {
        /// <summary>
        /// The effect or its behaviour gets restarted in a retrigger situation.
        /// </summary>
        Restart,

        /// <summary>
        /// Retrigger calls are ignored. The effect or its behaviour is not being restarted.
        /// </summary>
        Ignore
    }
}
