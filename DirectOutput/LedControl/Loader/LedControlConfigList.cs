using System.Collections.Generic;
using System.Linq;
using DirectOutput.Cab;
using DirectOutput.Cab.Toys;
using DirectOutput.FX.LedControlFX;
using DirectOutput.GlobalConfiguration;
using DirectOutput.Table;
using DirectOutput.Cab.Toys.LWEquivalent;
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
        public Dictionary<int, TableConfig> GetTableConfigDictonary(string RomName)
        {
            Dictionary<int, TableConfig> D = new Dictionary<int, TableConfig>();

            bool FoundMatch = false;
            foreach (LedControlConfig LCC in this)
            {

                foreach (TableConfig TC in LCC.TableConfigurations)
                {
                    if (RomName.ToUpper() == TC.ShortRomName.ToUpper())
                    {
                        D.Add(LCC.LedWizNumber, TC);
                        FoundMatch = true;
                        break;
                    }
                }
            }

            if (FoundMatch) return D;

            foreach (LedControlConfig LCC in this)
            {

                foreach (TableConfig TC in LCC.TableConfigurations)
                {
                    if (RomName.ToUpper().StartsWith("{0}_".Build(TC.ShortRomName.ToUpper())))
                    {
                        D.Add(LCC.LedWizNumber, TC);
                        FoundMatch = true;
                        break;
                    }
                }
            }

            if (FoundMatch) return D;

            foreach (LedControlConfig LCC in this)
            {

                foreach (TableConfig TC in LCC.TableConfigurations)
                {
                    if (RomName.StartsWith(TC.ShortRomName))
                    {
                        D.Add(LCC.LedWizNumber, TC);

                        break;
                    }
                }
            }
            return D;
        }


        /// <summary>
        /// Determines whether a config for the spcified RomName exists in the configs.
        /// </summary>
        /// <param name="RomName">Name of the rom.</param>
        /// <returns>
        ///   <c>true</c> if the specified config exists; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsConfig(string RomName)
        {
            return GetTableConfigDictonary(RomName).Count > 0;

        }


        /// <summary>
        /// Loads a list of ledcontrol.ini files.
        /// </summary>
        /// <param name="LedControlFilenames">The list of ledcontrol.ini files</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> throw exceptions on errors.</param>
        public void LoadLedControlFiles(IList<string> LedControlFilenames, bool ThrowExceptions = false)
        {
            for (int i = 0; i < LedControlFilenames.Count; i++)
            {
                LoadLedControlFile(LedControlFilenames[i], i + 1, ThrowExceptions);
            }
        }

        /// <summary>
        /// Loads a list of ledcontrol.ini files.
        /// </summary>
        /// <param name="LedControlIniFiles">The list of ini files to be loaded.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> throw exceptions on errors.</param>
        public void LoadLedControlFiles(LedControlIniFileList LedControlIniFiles, bool ThrowExceptions = false)
        {
            foreach (LedControlIniFile F in LedControlIniFiles)
            {
                LoadLedControlFile(F.Filename, F.LedWizNumber, ThrowExceptions);
            }
        }


        /// <summary>
        /// Loads a single ledcontrol.ini file.
        /// </summary>
        /// <param name="LedControlFilename">The ledcontrol.ini filename.</param>
        /// <param name="LedWizNumber">The number of the LedWizEquivalent to be used for the output of the configuration in the file.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> throws exceptions on errors.</param>
        public void LoadLedControlFile(string LedControlFilename, int LedWizNumber, bool ThrowExceptions = false)
        {
            Log.Write("Loading LedControl file {0}".Build(LedControlFilename));

            LedControlConfig LCC = new LedControlConfig(LedControlFilename, LedWizNumber, ThrowExceptions);
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
        /// <param name="ThrowExceptions">if set to <c>true</c> exceptions on loading the files are shown.</param>
        public LedControlConfigList(IList<string> LedControlFilenames, bool ThrowExceptions = false)
            : this()
        {
            LoadLedControlFiles(LedControlFilenames, ThrowExceptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LedControlConfigList"/> class.
        /// </summary>
        /// <param name="LedControlIniFiles">The list of ini files to be loaded.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> exceptions on loading the files are shown.</param>
        public LedControlConfigList(LedControlIniFileList LedControlIniFiles, bool ThrowExceptions = false)
            : this()
        {
            LoadLedControlFiles(LedControlIniFiles, ThrowExceptions);
        }

    }
}
