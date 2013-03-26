using System;
using System.Collections.Generic;
using System.IO;

namespace DirectOutput.LedControl
{
    /// <summary>
    /// Ledcontrol configuration read from a ledcontrol.ini file.
    /// </summary>
    public class LedControlConfig
    {
        private int _LedWizNumber=0;

        /// <summary>
        /// Gets or sets the number of the LedWiz resp. LedWizEquivalent to which the settings in this config will be applied.
        /// </summary>
        /// <value>
        /// The number of the LedWiz resp. LedWizEquivalent to which the settings in this config will be applied (1-16).
        /// </value>
        public int LedWizNumber
        {
            get { return _LedWizNumber; }
            set { _LedWizNumber = value; }
        }
        


        private TableConfigList _TableConfigurations; 

        /// <summary>
        /// Gets or sets the list of table configurations in the ledcontrol.ini.
        /// </summary>
        /// <value>
        /// The table configurations.
        /// </value>
        public TableConfigList TableConfigurations
        {
            get { return _TableConfigurations; }
            set { _TableConfigurations = value; }
        }

        private ColorConfigList _ColorConfigurations;

        /// <summary>
        /// Gets or sets the list of color configurations in the ledcontrol.ini.
        /// </summary>
        /// <value>
        /// The color configurations.
        /// </value>
        public ColorConfigList ColorConfigurations
        {
            get { return _ColorConfigurations; }
            set { _ColorConfigurations = value; }
        }



        /// <summary>
        /// Parses the ledcontrol.ini file.
        /// </summary>
        /// <param name="LedControlIniFile">The ledcontrol.ini FileInfo object.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <exception cref="System.Exception">
        /// File {0} does not contain data.
        /// or
        /// Could not find {0} section in file {1}.
        /// or
        /// File {1} does not contain data in the {0} section.
        /// or
        /// Section {0} of file {1} does not have the same number of columns in all lines.
        /// </exception>
        private void ParseLedControlIni(FileInfo LedControlIniFile, bool ThrowExceptions = false)
        {
            const string ColorStartString = "[Colors LedWiz]";
            const string OutStartString = "[Config outs]";

            //Read the file
            string Data="";
            try
            {
                Data = General.FileReader.ReadFileToString(LedControlIniFile);
            }
            catch (Exception E)
            {
                if (ThrowExceptions)
                {
                    throw new Exception("Could not read file {0}.".Build(LedControlIniFile), E);
                }
            }
            if (Data.IsNullOrWhiteSpace())
            {
                throw new Exception("File {0} does not contain data.".Build(LedControlIniFile));
            }

            //Find starting positions of both sections
            int ColorStart = Data.IndexOf(ColorStartString, StringComparison.InvariantCultureIgnoreCase);
            int OutStart = Data.IndexOf(OutStartString, StringComparison.InvariantCultureIgnoreCase);

            if (ColorStart < 0)
            {
                if (ThrowExceptions)
                {
                    throw new Exception("Could not find {0} section in file {1}.".Build(ColorStartString, LedControlIniFile));
                }
                return;
            }

            if (OutStart < 0)
            {
                if (ThrowExceptions)
                {
                    throw new Exception("Could not find {0} section in file {1}.".Build(OutStartString, LedControlIniFile));
                }
                return;
            }

            string[] ColorData;
            string[] OutData;

            //Extract data of the sections, split into string arrays and remove empty lines
            if (ColorStart < OutStart)
            {
                ColorData = Data.Substring(ColorStart + ColorStartString.Length, OutStart - (ColorStart + ColorStartString.Length)).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                OutData = Data.Substring(OutStart + OutStartString.Length).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                OutData = Data.Substring(OutStart + OutStartString.Length, ColorStart - (OutStart + OutStartString.Length)).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                ColorData = Data.Substring(ColorStart + ColorStartString.Length).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            }


            if (OutData.Length == 0)
            {
                if (ThrowExceptions)
                {
                    throw new Exception("File {1} does not contain data in the {0} section.".Build(OutStartString, LedControlIniFile));
                }
                return;
            }

            if (ColorData.Length == 0)
            {
                if (ThrowExceptions)
                {
                    throw new Exception("File {1} does not contain data in the {0} section.".Build(ColorStartString, LedControlIniFile));
                }
                return;
            }

            int ColumnCount = OutData[0].Split(new char[] { ',' }).Length;
            foreach (string OutLine in OutData)
            {
                if (OutLine.Split(new char[] { ',' }).Length != ColumnCount)
                {
                    if (ThrowExceptions)
                    {
                        throw new Exception("Section {0} of file {1} does not have the same number of columns in all lines.".Build(OutStartString, LedControlIniFile));
                    }
                    
                }
            }

            ColorConfigurations.ParseLedControlData(ColorData, ThrowExceptions);

            TableConfigurations.ParseLedcontrolData(OutData, ThrowExceptions);

            ResolveOutputNumbers();
            ResolveRGBColors();
        }

        private void ResolveRGBColors()
        {

            foreach (TableConfig TC in TableConfigurations)
            {
                foreach (TableConfigColumn C in TC.Columns)
                {
                    foreach (TableConfigSetting S in C)
                    {
                        if (ColorConfigurations.Contains(S.ColorName))
                        {
                            ColorConfig CC = ColorConfigurations[S.ColorName];
                            if (CC != null)
                            {
                                S.ColorConfig = CC;
                            }
                        }
                    }
                }
            }
        }


        private void ResolveOutputNumbers()
        {
            //Get the number of required outputs per column
            Dictionary<int, int> RequiredOutputs = new Dictionary<int, int>();
            foreach (TableConfig TC in TableConfigurations)
            {
                TC.Columns.Sort();
                foreach (TableConfigColumn C in TC.Columns)
                {
                    foreach (TableConfigSetting S in C)
                    {
                        int Cnt = (S.OutputType == OutputTypeEnum.RGBOutput ? 3 : 1);
                        if (RequiredOutputs.ContainsKey(C.Number))
                        {

                            if (RequiredOutputs[C.Number] < Cnt)
                            {
                                RequiredOutputs[C.Number] = Cnt;
                            }
                        }
                        else
                        {
                            RequiredOutputs.Add(C.Number, Cnt);
                        }
                    }
                    
                }
                
            }

            //Assign number of required outputs to columns
            foreach (TableConfig TC in TableConfigurations)
            {
                foreach (TableConfigColumn C in TC.Columns)
                {
                    if (RequiredOutputs.ContainsKey(C.Number))
                    {
                        C.RequiredOutputCount = RequiredOutputs[C.Number];
                    }
                    else
                    {
                        C.RequiredOutputCount = 1;
                    }
                }
            }

            //Set LedWiz output numbers to columns
            foreach (TableConfig TC in TableConfigurations)
            {
                
                int FirstOutputNumber = 1;
                foreach (TableConfigColumn C in TC.Columns)
                {
                    C.FirstOutputNumber = FirstOutputNumber;
                    FirstOutputNumber += C.RequiredOutputCount;
                }
            }
            //Dump();
        }


        //private void Dump()
        //{
        //    string A = "";
        //    foreach (TableConfig TC in TableConfigurations)
        //    {
        //        A+="Cols: {0:##}, ".Build( TC.Columns.Count);
        //        foreach (TableConfigColumn C in TC.Columns)
        //        {
        //            A+="({0:00} {1} {2:00})".Build( C.Number, C.RequiredOutputCount, C.FirstOutputNumber);
        //        }
        //        A += "\n";
        //    }
        //    A.WriteToFile("c:\\parseresult.txt");

        //}



        /// <summary>
        /// Initializes a new instance of the <see cref="LedControlConfig"/> class.
        /// </summary>
        public LedControlConfig()
        {
            this.TableConfigurations = new TableConfigList();
            this.ColorConfigurations = new ColorConfigList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LedControlConfig" /> class.
        /// Parses the ledcontrol.ini file.
        /// </summary>
        /// <param name="LedControlIniFilename">The ledcontrol.ini filename.</param>
        /// <param name="LedWizNumber">The number of the LedWizEquivalent to be used.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <exception cref="System.Exception">File {0} does not contain data.
        /// or
        /// Could not find {0} section in file {1}.
        /// or
        /// File {1} does not contain data in the {0} section.
        /// or
        /// Section {0} of file {1} does not have the same number of columns in all lines.</exception>
        public LedControlConfig(string LedControlIniFilename,int LedWizNumber, bool ThrowExceptions=false)
            : this()
        {
            ParseLedControlIni(new FileInfo(LedControlIniFilename), ThrowExceptions);
            this.LedWizNumber = LedWizNumber;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LedControlConfig" /> class.
        /// Parses the ledcontrol.ini file.
        /// </summary>
        /// <param name="LedControlIniFile">The ledcontrol.ini FileInfo object.</param>
        /// <param name="LedWizNumber">The number of the LedWizEquivalent to be used.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <exception cref="System.Exception">File {0} does not contain data.
        /// or
        /// Could not find {0} section in file {1}.
        /// or
        /// File {1} does not contain data in the {0} section.
        /// or
        /// Section {0} of file {1} does not have the same number of columns in all lines.</exception>
        public LedControlConfig(FileInfo LedControlIniFile, int LedWizNumber, bool ThrowExceptions = false)
            : this()
        {
            ParseLedControlIni(LedControlIniFile, ThrowExceptions);
            this.LedWizNumber = LedWizNumber;
        }

    }
}
