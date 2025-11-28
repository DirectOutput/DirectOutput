using System.Collections.Generic;
using System.Linq;
using DirectOutput.Cab;
using DirectOutput.Cab.Toys;
using DirectOutput.GlobalConfiguration;
using DirectOutput.Table;
using DirectOutput.Cab.Toys.LWEquivalent;
using System.IO;
namespace DirectOutput.LedControl.Loader
{

    /// <summary>
    /// List of LedControlConfig objects loaded from LedControl.ini files.
    /// </summary>
    public class LedControlConfigList : List<LedControlConfig>
    {
        /// <summary>
        /// Gets a dictionary of table configs for a specific romname from the loaded ini file data.
        /// </summary>
        /// <param name="RomName">Name of the rom.</param>
        /// <returns></returns>
        public Dictionary<int, TableConfig> GetTableConfigDictionary(string RomName)
        {
            Dictionary<int, TableConfig> D = new Dictionary<int, TableConfig>();

            foreach (LedControlConfig LCC in this)
            {
                TableConfig TC = LCC.TableConfigurations.GetBestMatchingConfig(RomName);
                if (TC != null) {
                    D.Add(LCC.LedWizNumber, TC);
                }
            }

            return D;
        }


        /// <summary>
        /// Determines whether a config for the specified RomName exists in the configs.
        /// </summary>
        /// <param name="RomName">Name of the rom.</param>
        /// <returns>
        ///   <c>true</c> if the specified config exists; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsConfig(string RomName)
        {
            return GetTableConfigDictionary(RomName).Count > 0;

        }


        /// <summary>
        /// Loads a list of ledcontrol.ini files.
        /// </summary>
        /// <param name="LedControlFilenames">The list of ledcontrol.ini files</param>
        /// <param name="RomName">Specify a rom name at loading stage to ignore parsing of non matching lines</param>
        /// <param name="GlobalConfig">The current global configuration.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> throw exceptions on errors.</param>
        public void LoadLedControlFiles(IList<string> LedControlFilenames, string RomName, GlobalConfig GlobalConfig, bool ThrowExceptions = false)
        {
            for (int i = 0; i < LedControlFilenames.Count; i++)
            {
                LoadLedControlFile(LedControlFilenames[i], i + 1, RomName, GlobalConfig, ThrowExceptions);
            }
        }

        /// <summary>
        /// Loads a list of ledcontrol.ini files.
        /// </summary>
        /// <param name="LedControlIniFiles">The dictionary of ini files to be loaded.</param>
        /// <param name="RomName">Specify a rom name at loading stage to ignore parsing of non matching lines</param>
        /// <param name="GlobalConfig">The current global configuration.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> throw exceptions on errors.</param>
        public void LoadLedControlFiles(Dictionary<int, FileInfo> LedControlIniFiles, string RomName, GlobalConfig GlobalConfig, bool ThrowExceptions = false)
        {
            foreach (KeyValuePair<int,FileInfo> F in LedControlIniFiles)
            {
                LoadLedControlFile(F.Value.FullName, F.Key, RomName, GlobalConfig, ThrowExceptions);
            }
        }


        /// <summary>
        /// Loads a single ledcontrol.ini file.
        /// </summary>
        /// <param name="LedControlFilename">The ledcontrol.ini filename.</param>
        /// <param name="LedWizNumber">The number of the LedWizEquivalent to be used for the output of the configuration in the file.</param>
        /// <param name="RomName">Specify a rom name at loading stage to ignore parsing of non matching lines</param>
        /// <param name="GlobalConfig">The current global configuration.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> throws exceptions on errors.</param>
        public void LoadLedControlFile(string LedControlFilename, int LedWizNumber, string RomName, GlobalConfig GlobalConfig, bool ThrowExceptions = false)
        {
            Log.Write("Loading LedControl file {0}".Build(LedControlFilename));

            LedControlConfig LCC = new LedControlConfig(LedControlFilename, LedWizNumber, RomName, GlobalConfig, ThrowExceptions);
            Add(LCC);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LedControlConfigList"/> class.
        /// </summary>
        public LedControlConfigList() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LedControlConfigList"/> class.
        /// </summary>
        /// <param name="LedControlFilenames">The filenames of the ledcontrol.ini files to be loaded.</param>
        /// <param name="RomName">Specify a rom name at loading stage to ignore parsing of non matching lines</param>
        /// <param name="GlobalConfig">The current global configuration.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> exceptions on loading the files are shown.</param>
        public LedControlConfigList(IList<string> LedControlFilenames, string RomName, GlobalConfig GlobalConfig, bool ThrowExceptions = false)
            : this()
        {
            LoadLedControlFiles(LedControlFilenames, RomName, GlobalConfig, ThrowExceptions);
        }



    }
}
