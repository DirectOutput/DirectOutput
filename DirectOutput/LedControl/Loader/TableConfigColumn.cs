﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace DirectOutput.LedControl.Loader
{

    /// <summary>
    /// Column in a LedControl.ini file.<br />
    /// Is a list of TableConfigSettingObjects for that column.
    /// </summary>
    public class TableConfigColumn : List<TableConfigSetting>
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
        /// Gets or sets the number of the first output for this column.
        /// </summary>
        /// <value>
        /// The number of the first output for this column.
        /// </value>
        public int FirstOutputNumber
        {
            get { return _FirstOutputNumber; }
            set { _FirstOutputNumber = value; }
        }


        ///// <summary>
        ///// Gets a value indicating whether the settings for this column require a analog output.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if analog output is required; otherwise, <c>false</c>.
        ///// </value>
        //public bool AnalogOutputRequired
        //{
        //    get
        //    {
        //        foreach (TableConfigSetting TCS in this)
        //        {
        //            if (TCS.Intensity != 0 && TCS.Intensity != 48 && TCS.OutputType != OutputTypeEnum.RGBOutput)
        //            {
        //                return true;

        //            }

        //        };
        //        return false;
        //    }
        //}



        /// <summary>
        /// Gets the number of required outputs for the column.
        /// </summary>
        /// <value>
        /// The number of required outputs for the column.
        /// </value>
        public int RequiredOutputCount
        {
            get
            {
                return (this.Any(S => S.OutputType == OutputTypeEnum.RGBOutput) ? 3 : 1);
            }
        }


        /// <summary>
        /// Gets a value indicating whether any setting in the column has area values.
        /// </summary>
        /// <value>
        ///   <c>true</c> if any area settings exists is area; otherwise, <c>false</c>.
        /// </value>
        public bool IsArea
        {
            get
            {
                return this.Any(S => S.IsArea);
            }

        }


        #region Parser

        /// <summary>
        /// Parses data for a column from a LedControl.ini file.
        /// </summary>
        /// <param name="ColumnData">The column data.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> exceptions are thrown on errors.</param>
        /// <returns>true if all settings have been parsed successfully, false if a exception occurred during parsing.</returns>
        /// <exception cref="System.Exception">Could not parse setting {0} in column data {1}.</exception>
        public bool ParseColumnData(string ColumnData, bool ThrowExceptions = false)
        {
            bool ExceptionOccurred = false;
            List<string> ColumnConfigs = new List<string>(SplitSettings(ColumnData));

            foreach (string CC in ColumnConfigs)
            {
                if (!CC.IsNullOrWhiteSpace())
                {
                    try
                    {
                        TableConfigSetting TCS = new TableConfigSetting(CC);
                        if (TCS.OutputControl != OutputControlEnum.FixedOff)
                        {
                            Add(TCS);
                        }
                    }
                    catch (Exception E)
                    {
                        ExceptionOccurred = true;
                        Log.Exception("Could not parse setting {0} in column data {1}.".Build(CC, ColumnData), E);
                        if (ThrowExceptions)
                        {
                            throw new Exception("Could not parse setting {0} in column data {1}.".Build(CC, ColumnData), E);
                        }
                    }
                }
            }
            return !ExceptionOccurred;
        }


        private string[] SplitSettings(string ConfigData)
        {
            List<string> L = new List<string>();

            int BracketCount = 0;

            int LP = 0;

            for (int P = 0; P < ConfigData.Length; P++)
            {
                if (ConfigData[P] == '(')
                {
                    BracketCount++;
                }
                else if (ConfigData[P] == ')')
                {
                    BracketCount--;
                } if (ConfigData[P] == '/' && BracketCount <= 0)
                {
                    L.Add(ConfigData.Substring(LP, P - LP));
                    LP = P + 1;
                    BracketCount = 0;
                }

            }

            if (LP < ConfigData.Length)
            {
                L.Add(ConfigData.Substring(LP));
            }

            return L.ToArray();
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
