using System;
using System.Collections.Generic;

namespace DirectOutput.LedControl.Loader
{
    public class TableConfigList:List<TableConfig>
    {
        public void ParseLedcontrolData(string[] TableConfigDataFromLedControlIni, bool ThrowExceptions = true)
        {
            foreach (string Data in TableConfigDataFromLedControlIni)
            {
                if (!Data.IsNullOrWhiteSpace())
                {
                    ParseLedcontrolData(Data, ThrowExceptions);
                }
            }
        }


        /// <summary>
        /// Parses a line of LedControl data contaning the config for a table.
        /// </summary>
        /// <param name="TableConfigDataLineFromLedControlIni">The table config data line from led control ini.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <exception cref="System.Exception">
        /// Could not load table config from data line: {0}
        /// or
        /// Table with ShortRomName {0} has already been loaded.
        /// </exception>
        public void ParseLedcontrolData(string TableConfigDataLineFromLedControlIni, bool ThrowExceptions = true)
        {
            TableConfig TC=null;
            try
            {
                TC = new TableConfig(TableConfigDataLineFromLedControlIni, ThrowExceptions);

            }
            catch (Exception E)
            {
                Log.Exception("Could not load table config from data line: {0}".Build(TableConfigDataLineFromLedControlIni), E);
                if (ThrowExceptions)
                {
                    throw new Exception("Could not load table config from data line: {0}".Build(TableConfigDataLineFromLedControlIni),E);
                }
                
            };
            if (TC != null)
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
    
    
    }
}
