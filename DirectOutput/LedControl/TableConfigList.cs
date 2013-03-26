using System;
using System.Collections.Generic;

namespace DirectOutput.LedControl
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
        /// Parses a line of LedControl data containg the config for a table.
        /// </summary>
        /// <param name="TableConfigDataLineFromLedControlIni">The table config data line from led control ini.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <exception cref="System.Exception">
        /// Could not load table config from data line: {0}.Build(TableConfigDataLineFromLedControlIni)
        /// or
        /// Table with ShortRomName {0} has already been loaded..Build(TC.ShortRomName)
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
                if (ThrowExceptions)
                {
                    throw new Exception("Could not load table config from data line: {0}".Build(TableConfigDataLineFromLedControlIni),E);
                }
                
            };
            if (TC != null)
            {
                if (ThrowExceptions && Contains(TC.ShortRomName))
                {
                    throw new Exception("Table with ShortRomName {0} has already been loaded.".Build(TC.ShortRomName));
                }
                Add(TC);
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
