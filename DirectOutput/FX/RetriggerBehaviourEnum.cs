
namespace DirectOutput.FX
{
    /// <summary>
    /// This enum describes the different retrigger behaviours for the effects.<br/>
    /// Retriggering means that a effect is getting another Trigger call with the same table element value as the last call, while it is still active.
    /// </summary>
    public enum RetriggerBehaviourEnum
    {
        /// <summary>
        /// The effect gets restarted in a retrigger situation.
        /// </summary>
        RestartEffect,

        /// <summary>
        /// Retrigger calls are ignored. The effect is not being restarted.
        /// </summary>
        IgnoreRetrigger
    }
}
