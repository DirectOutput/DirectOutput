using DirectOutput.GlobalConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectOutput.LedControl.Loader
{
    /// <summary>
    /// A list of table configs from a ini file.
    /// </summary>
    public class TableConfigList:List<TableConfig>
    {
        /// <summary>
        /// Parses several lines of LedControlData.
        /// </summary>
        /// <param name="TableConfigDataFromLedControlIni">The table config data from led control ini.</param>
        /// <param name="RomName">Specify a rom name at loading stage to ignore parsing of non matching lines</param>
        /// <param name="GlobalConfig">The current global configuration.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> [throw exceptions].</param>
        public void ParseLedcontrolData(IEnumerable<string> TableConfigDataFromLedControlIni, string RomName, GlobalConfig GlobalConfig, bool ThrowExceptions = true)
        {
            foreach (string Data in TableConfigDataFromLedControlIni)
            {
                if (!Data.IsNullOrWhiteSpace())
                {
                    ParseLedcontrolData(Data, RomName, GlobalConfig, ThrowExceptions);
                }
            }
        }


        /// <summary>
        /// Parses a line of LedControl data contaning the config for a table.
        /// </summary>
        /// <param name="TableConfigDataLineFromLedControlIni">The table config data line from led control ini.</param>
        /// <param name="RomName">Specify a rom name at loading stage to ignore parsing of non matching lines</param>
        /// <param name="GlobalConfig">The current global configuration.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <exception cref="System.Exception">
        /// Could not load table config from data line: {0}
        /// or
        /// Table with ShortRomName {0} has already been loaded.
        /// </exception>
        public void ParseLedcontrolData(string TableConfigDataLineFromLedControlIni, string RomName, GlobalConfig GlobalConfig, bool ThrowExceptions = true)
        {
            TableConfig TC=null;
            try
            {
                TC = new TableConfig(TableConfigDataLineFromLedControlIni, RomName, GlobalConfig, ThrowExceptions);

            }
            catch (Exception E)
            {
                Log.Exception("Could not load table config from data line: {0}".Build(TableConfigDataLineFromLedControlIni), E);
                if (ThrowExceptions)
                {
                    throw new Exception("Could not load table config from data line: {0}".Build(TableConfigDataLineFromLedControlIni),E);
                }
                
            };
            if (TC != null && !TC.ShortRomName.IsNullOrEmpty())
            {
                if (Contains(TC.ShortRomName))
                {
                    Log.Exception("Table with ShortRomName {0} has already been loaded (Exists more than once in ledcontrol file).".Build(TC.ShortRomName));
                    if (ThrowExceptions)
                    {
                        throw new Exception("Table with ShortRomName {0} has already been loaded.".Build(TC.ShortRomName));
                    }
                }
                {
                    Add(TC);
                }
            }
        }

        /// <summary>
        /// Determines whether the list contains the specified short rom name.
        /// </summary>
        /// <param name="ShortRomName">Short name of the rom.</param>
        /// <returns>
        ///   <c>true</c> if the list contains the specified short rom name, otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string ShortRomName)
        {
            foreach (TableConfig TC in this)
            {
                if (TC.ShortRomName.Equals(ShortRomName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the best matching config for the provided romname
        /// </summary>
        /// <param name="RomName">Name of the rom.</param>
        /// <returns>
        ///   The TableConfig if there is one matching, otherwise, <c>null</c>.
        /// </returns>
        internal TableConfig GetBestMatchingConfig(string RomName)
        {
            TableConfig bestMatchingConfig = this.FirstOrDefault(TC=>string.Compare(TC.ShortRomName, RomName, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (bestMatchingConfig == null) 
            {
                bestMatchingConfig = this.FirstOrDefault(TC => RomName.StartsWith("{0}_".Build(TC.ShortRomName), StringComparison.InvariantCultureIgnoreCase));

                if (bestMatchingConfig == null) {
                    bestMatchingConfig = this.FirstOrDefault(TC => RomName.StartsWith(TC.ShortRomName, StringComparison.InvariantCultureIgnoreCase));
                }
            }
            return bestMatchingConfig;
        }
    }
}
