using System;
using System.Collections.Generic;

namespace DirectOutput.LedControl.Loader
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

       

        /// <summary>
        /// Gets the number of required outputs for the column.
        /// </summary>
        /// <value>
        /// The number of required outputs for the column.
        /// </value>
        public int RequiredOutputCount
        {
            get {
                int RequiredOutputCount = 1;
                foreach (TableConfigSetting S in this)
                {
                    if (S.OutputType == OutputTypeEnum.RGBOutput)
                    {
                        RequiredOutputCount = 3;
                    }
                }
                
                return RequiredOutputCount; }
            
        }
        



        #region Parser

        /// <summary>
        /// Parses data for a column from a LedControl.ini file.
        /// </summary>
        /// <param name="ColumnData">The column data.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> exceptions are thrown on errors.</param>
        /// <returns>true if all settings have been parsed successfully, fals if a exception occured during parsing.</returns>
        /// <exception cref="System.Exception">Could not parse setting {0} in column data {1}..Build(CC, ColumnData)</exception>
        public bool ParseColumnData(string ColumnData, bool ThrowExceptions = false)
        {
            bool ExceptionOccured = false;
            List<string> ColumnConfigs = new List<string>(ColumnData.Split(new char[] { '/' }, StringSplitOptions.None));

            foreach (string CC in ColumnConfigs)
            {
                if (!CC.IsNullOrWhiteSpace())
                {
                    try
                    {
                        TableConfigSetting TCS = new TableConfigSetting(CC);
                        Add(TCS);
                    }
                    catch (Exception E)
                    {
                        ExceptionOccured = true;
                        Log.Exception("Could not parse setting {0} in column data {1}.".Build(CC, ColumnData), E);
                        if (ThrowExceptions)
                        {
                            throw new Exception("Could not parse setting {0} in column data {1}.".Build(CC, ColumnData), E);
                        }
                    }
                }
            }
            return !ExceptionOccured;
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
