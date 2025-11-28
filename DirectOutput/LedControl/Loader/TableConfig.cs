using DirectOutput.GlobalConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectOutput.LedControl.Loader
{
    /// <summary>
    /// A table config from a ini file.
    /// </summary>
    public class TableConfig
    {
        /// <summary>
        /// Gets or sets the short name of the rom.
        /// </summary>
        /// <value>
        /// The short name of the rom.
        /// </value>
        public string ShortRomName
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the columns in the table table config.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public TableConfigColumnList Columns { get; set; }

        /// <summary>
        /// Return if the table config is patching a specified rom name
        /// </summary>
        /// <param name="RomName">The rom name to match with</param>
        /// <returns>true if it matches, false otherwise</returns>
        public bool IsRomNameMatching(string RomName)
        {
            return (
                RomName.IsNullOrEmpty() ||
                string.Compare(RomName, ShortRomName, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                RomName.StartsWith(ShortRomName, StringComparison.InvariantCultureIgnoreCase) ||
                RomName.StartsWith("{0}_".Build(ShortRomName), StringComparison.InvariantCultureIgnoreCase)
                );
        }

        /// <summary>
        /// Gets the table config from led control data.<br />
        /// Parses one line of a Ledcontrol.ini file, it should look something like<br />
        /// mm,L88 Blink I44,L87,ON Red,S37,S7,S48,S46/S1/S2,S10/S8,S11,S4 300 I32/S8 300 I32/W15 300 2/W25 300 2,S3/S4/S8/S33,S4/S9/S14/S26/S35,S4/S5/S12/S13/S34/S36,S4/S16/S27/S28,S7 300/S4 1500/S8 300/S33 300/S35 300,S7 White/S17 Red/S18 Red/S19 Red/W26 Magenta,S7 White/S24 Green/S25 Green/W15 Lime/W44 Green,S7 White/S22 Red/S24 Green,S7 White/S21 Red/S12 Magenta/S13 Magenta/W25 Lime,S7 White/S20 Red/S23 Red/S14 Magenta/W17 Magenta<br />
        /// (Looks cool for Medival Madness)
        /// </summary>
        /// <param name="LedControlData">One line of data from led control ini.</param>
        /// <param name="RomName">Specify a rom name at loading stage to ignore parsing of non matching lines</param>
        /// <param name="GlobalConfig">The current global configuration.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> the parser will throw exceptions if something wrong is found in the data, otherwise exceptions are ignored.</param>
        /// <exception cref="System.Exception">
        /// No usable data found in line {0}.
        /// or
        /// No short Rom name found in line {0}.
        /// </exception>
        public void ParseLedControlDataLine(string LedControlData, string RomName, GlobalConfig GlobalConfig, bool ThrowExceptions)
        {
            //mm,L88 Blink I44,L87,ON Red,S37,S7,S48,S46/S1/S2,S10/S8,S11,S4 300 I32/S8 300 I32/W15 300 2/W25 300 2,S3/S4/S8/S33,S4/S9/S14/S26/S35,S4/S5/S12/S13/S34/S36,S4/S16/S27/S28,S7 300/S4 1500/S8 300/S33 300/S35 300,S7 White/S17 Red/S18 Red/S19 Red/W26 Magenta,S7 White/S24 Green/S25 Green/W15 Lime/W44 Green,S7 White/S22 Red/S24 Green,S7 White/S21 Red/S12 Magenta/S13 Magenta/W25 Lime,S7 White/S20 Red/S23 Red/S14 Magenta/W17 Magenta

            //Split columns
            List<string> DataColumns = new List<string>(LedControlData.Split(new char[] { ',' }, StringSplitOptions.None));
            if (DataColumns.Count < 1)
            {
                Log.Warning("No data found in line {0}".Build(LedControlData));
                if (ThrowExceptions)
                {
                    throw new Exception("No usable data found in line {0}".Build(LedControlData));
                }
                return;
            }
            //Check if first column contains data (Romname)
            if (DataColumns[0].IsNullOrWhiteSpace())
            {
                Log.Warning("No short Rom name found in line {0}".Build(LedControlData));
                if (ThrowExceptions)
                {
                    throw new Exception("No short Rom name found in line {0}".Build(LedControlData));
                }
                return;
            }
            //Assign Romname from first column
            ShortRomName = DataColumns[0];
            //If Romname was specified check if it matches, otherwize clear ShortRomName
            if ((GlobalConfig?.FastInitialization ?? true) && !IsRomNameMatching(RomName)) {
                ShortRomName = string.Empty;
                return;
            }

            Columns = new TableConfigColumnList();

            Columns.ParseControlData(LedControlData, ThrowExceptions);

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfig"/> class.
        /// </summary>
        public TableConfig()
        {
            Columns = new TableConfigColumnList();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfig"/> class.
        /// Gets the table config from led control data.<br />
        /// Parses one line of a Ledcontrol.ini file, it should look something like<br />
        /// mm,L88 Blink I44,L87,ON Red,S37,S7,S48,S46/S1/S2,S10/S8,S11,S4 300 I32/S8 300 I32/W15 300 2/W25 300 2,S3/S4/S8/S33,S4/S9/S14/S26/S35,S4/S5/S12/S13/S34/S36,S4/S16/S27/S28,S7 300/S4 1500/S8 300/S33 300/S35 300,S7 White/S17 Red/S18 Red/S19 Red/W26 Magenta,S7 White/S24 Green/S25 Green/W15 Lime/W44 Green,S7 White/S22 Red/S24 Green,S7 White/S21 Red/S12 Magenta/S13 Magenta/W25 Lime,S7 White/S20 Red/S23 Red/S14 Magenta/W17 Magenta<br />
        /// (Looks cool for Medival Madness)
        /// </summary>
        /// <param name="LedControlData">One line of data from led control ini.</param>
        /// <param name="RomName">Specify a rom name at loading stage to ignore parsing of non matching lines</param>
        /// <param name="GlobalConfig">The current global configuration.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> the parser will throw exceptions if something wrong is found in the data, otherwise exceptions are ignored.</param>
        /// 0
        /// <exception cref="System.Exception">
        /// No usable data found in line {0}.
        /// or
        /// No short Rom name found in line {0}.
        /// </exception>
        public TableConfig(string LedControlData, string RomName, GlobalConfig GlobalConfig, bool ThrowExceptions)
            : this()
        {
            ParseLedControlDataLine(LedControlData, RomName, GlobalConfig, ThrowExceptions);
        }




    }
}
