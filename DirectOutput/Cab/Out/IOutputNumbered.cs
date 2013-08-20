
namespace DirectOutput.Cab.Out
{
    /// <summary>
    /// Common interface for numbered outputs.
    /// </summary>
    public interface IOutputNumbered: IOutput
    {
        /// <summary>
        /// Gets or sets the number of the IOutput.
        /// </summary>
        /// <value>
        /// The number of the IOutput.
        /// </value>
        int Number { get; set; }

    }
}
