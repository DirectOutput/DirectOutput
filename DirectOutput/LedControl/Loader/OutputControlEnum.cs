
namespace DirectOutput.LedControl.Loader
{
    /// <summary>
    /// Used the specify how the output for a setting is controled.
    /// </summary>
    public enum OutputControlEnum
    {
        /// <summary>
        /// Is always on (triggered with static effects).
        /// </summary>
        FixedOn,
        /// <summary>
        /// Is always off.
        /// </summary>
        FixedOff,
        /// <summary>
        /// Is controlled by a table element.
        /// </summary>
        Controlled
    }
}
