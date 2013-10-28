
namespace DirectOutput.Table
{
    /// <summary>
    /// Enum used to specify the source of a table configuration
    /// </summary>
    public enum TableConfigSourceEnum
    {
        /// <summary>
        /// Source of the table configuration is unknown.
        /// </summary>
        Unknown,
        /// <summary>
        /// Table configuration has been loaded from a table config file.
        /// </summary>
        TableConfigurationFile,
        /// <summary>
        /// Table configuration has been loaded from a directoutputconfig.ini or a ledconbtrol.ini file.
        /// </summary>
        IniFile,
        /// <summary>
        /// The table configurations is a combination from a table configuration file and a ini file.
        /// </summary>
        TabbleConfigurationFileAndIniFile
    }
}
