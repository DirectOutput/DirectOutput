using System;
using System.Collections.Generic;

namespace DirectOutput.LedControl
{

    /// <summary>
    /// Column in a LedControl.ini file.<br />
    /// Is a list of <see cref="TableConfigSettingObjects" /> for that column.
    /// </summary>
    public class TableConfigColumn:List<TableConfigSetting>
    {
        /// <summary>
        /// Gets or sets the number of the column.
        /// </summary>
        /// <value>
        /// The number of the column.
        /// </value>
        public int Number { get; set; }


        private int _FirstOutputNumber;

        /// <summary>
        /// Gets or sets the number of the first ouput for this column.
        /// </summary>
        /// <value>
        /// The number of the first output for this column.
        /// </value>
        public int FirstOutputNumber
        {
            get { return _FirstOutputNumber; }
            set { _FirstOutputNumber = value; }
        }

        private int _RequiredOutputCount;

        /// <summary>
        /// Gets or sets the number of required outputs for the column.
        /// </summary>
        /// <value>
        /// The number of required outputs for the column.
        /// </value>
        public int RequiredOutputCount
        {
            get { return _RequiredOutputCount; }
            set { _RequiredOutputCount = value; }
        }
        



        #region Parser

        /// <summary>
        /// Parses data for a column from a LedControl.ini file.
        /// </summary>
        /// <param name="ColumnData">The column data.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> exceptions are thrown on errors.</param>
        public void ParseColumnData(string ColumnData, bool ThrowExceptions = false)
        {
            List<string> ColumnConfigs = new List<string>(ColumnData.Split(new char[] { '/' }, StringSplitOptions.None));

            foreach (string CC in ColumnConfigs)
            {
                if (!CC.IsNullOrWhiteSpace())
                {
                    try
                    {
                        TableConfigSetting TCS = new TableConfigSetting(CC, true);
                        Add(TCS);
                    }
                    catch (Exception E)
                    {
                        if (ThrowExceptions)
                        {
                            throw new Exception("Could not a table config setting {0} (likely due to a parse error).".Build(CC), E);
                        }
                    }
                }
            }
        } 
        #endregion


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfigColumn" /> class and parses data for a column.
        /// </summary>
        /// <param name="ColumnNumber">The column number.</param>
        /// <param name="ColumnData">The column data.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> exceptions are thrown on errors.</param>
        public TableConfigColumn(int ColumnNumber, string ColumnData, bool ThrowExceptions = false)
        {
            this.Number = ColumnNumber;
            ParseColumnData(ColumnData, ThrowExceptions);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfigColumn"/> class.
        /// </summary>
        public TableConfigColumn() { } 
        #endregion


    }
}
