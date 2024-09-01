using System;
using System.Collections.Generic;
using System.Text;

namespace DirectOutput.LedControl.Loader
{
    /// <summary>
    /// List of columns of a LedCOntrol.ini file.<br/>
    /// Inherits List&gt;T&lt;.
    /// </summary>
    public class TableConfigColumnList : List<TableConfigColumn>
    {

        /// <summary>
        /// Parses a line of LedControl data.
        /// </summary>
        /// <param name="LedControlData">LedControl data to parse.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> exceptions will be thrown on errors.</param>
        /// <exception cref="System.Exception">
        /// No data to parse found in LedControlData: {0}
        /// or
        /// Exception(s) accorud when parsing {0}
        /// </exception>
        public void ParseControlData(string LedControlData, bool ThrowExceptions = false)
        {
            string[] Cols = SplitColumns(LedControlData);
            if (Cols.Length < 2)
            {
                Log.Warning("No data to parse found in LedControlData: {0}".Build(LedControlData));
                if (ThrowExceptions)
                {
                    throw new Exception("No data to parse found in LedControlData: {0}".Build(LedControlData));
                }
                return;
            }
            int FirstOutputNumber = 1;
            int LastColumnWithData = -1;
            for (int i = 1; i < Cols.Length; i++)
            {
                if (!Cols[i].IsNullOrWhiteSpace())
                {
                    LastColumnWithData = i;
                }
            }
            for (int i = 1; i <= LastColumnWithData; i++)
            {
                TableConfigColumn C = new TableConfigColumn();
                C.Number = i;
                C.FirstOutputNumber = FirstOutputNumber;
                bool ParseOK=C.ParseColumnData(Cols[i], false);
                if (!ParseOK)
                {
                    Log.Warning("Previous exceptions occurred in the line {0} of the ledcontrol file".Build(LedControlData));
                    if (ThrowExceptions)
                    {
                        throw new Exception("Exception(s) occurred when parsing {0}".Build(LedControlData));
                    }
                }



                Add(C);

                FirstOutputNumber += C.RequiredOutputCount;
               
            }
            
        }


        private string[] SplitColumns(string ConfigData)
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
                } if (ConfigData[P] == ',' && BracketCount <= 0)
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


        /// <summary>
        /// Sorts the elements in the list on the column number.
        /// </summary>
        public new void Sort()
        {
          base.Sort((a, b) => a.Number.CompareTo(b.Number));
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfigColumnList"/> class.
        /// </summary>
        public TableConfigColumnList() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfigColumnList"/> class and parses a line of LedCOntrolData.
        /// </summary>
        /// <param name="LedControlData">LedControl data to parse.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> exceptions will be thrown on errors.</param>
        /// <exception cref="System.Exception">No data to parse found in LedControlData: {0}</exception>
        public TableConfigColumnList(string LedControlData, bool ThrowExceptions = false) {
            ParseControlData(LedControlData, ThrowExceptions);
        }

    }
}
