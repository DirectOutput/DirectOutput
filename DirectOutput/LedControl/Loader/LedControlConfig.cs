using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DirectOutput.LedControl.Loader
{
    /// <summary>
    /// Ledcontrol configuration read from a ledcontrol.ini file.
    /// </summary>
    public class LedControlConfig
    {
        private int _LedWizNumber = 0;

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
            string[] ColorStartStrings = { "[Colors DOF]", "[Colors LedWiz]" };
            string[] OutStartStrings = { "[Config DOF]", "[Config outs]" };

            string FileData = "";

            #region Read file
            try
            {
                FileData = General.FileReader.ReadFileToString(LedControlIniFile);
            }
            catch (Exception E)
            {
                Log.Exception("Could not read file {0}.".Build(LedControlIniFile), E);
                if (ThrowExceptions)
                {

                    throw new Exception("Could not read file {0}.".Build(LedControlIniFile), E);
                }
            }
            if (FileData.IsNullOrWhiteSpace())
            {
                Log.Warning("File {0} does not contain data.".Build(LedControlIniFile));
                if (ThrowExceptions)
                {
                    throw new Exception("File {0} does not contain data.".Build(LedControlIniFile));
                }
            }
            #endregion

            Dictionary<string, List<string>> Sections = new Dictionary<string, List<String>>();

            #region Read sections

            List<string> SectionData = new List<string>();
            String SectionHeader = null;
            foreach (string RawIniLine in FileData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string IniLine = RawIniLine.Trim();
                if (IniLine.Length > 0 && !IniLine.StartsWith("#"))
                {
                    if (IniLine.StartsWith("[") && IniLine.EndsWith("]") && IniLine.Length > 2)
                    {
                        //This is a section header
                        if (!SectionHeader.IsNullOrWhiteSpace())
                        {
                            if (Sections.ContainsKey(SectionHeader))
                            {
                                int Cnt = 2;
                                while (Sections.ContainsKey("{0} {1}".Build(SectionHeader, Cnt)))
                                {
                                    Cnt++;
                                    if (Cnt > 999) { throw new Exception("Section header {0} exists to many times.".Build(SectionHeader)); }
                                }
                                SectionHeader = "{0} {1}".Build(SectionHeader, Cnt);
                            }
                            Sections.Add(SectionHeader, SectionData);
                            SectionData = new List<string>();
                        }
                        SectionHeader = IniLine;
                    }
                    else
                    {
                        //Its a data line
                        SectionData.Add(IniLine);
                    }
                }
            }

            if (!SectionHeader.IsNullOrWhiteSpace())
            {
                if (Sections.ContainsKey(SectionHeader))
                {
                    int Cnt = 2;
                    while (Sections.ContainsKey("{0} {1}".Build(SectionHeader, Cnt)))
                    {
                        Cnt++;
                        if (Cnt > 999) { throw new Exception("Section header {0} exists to many times.".Build(SectionHeader)); }
                    }
                    SectionHeader = "{0} {1}".Build(SectionHeader, Cnt);
                }
                Sections.Add(SectionHeader, SectionData);
                SectionData = new List<string>();
            }
            SectionData = null;
            #endregion

            FileData = null;

            List<string> ColorData = null;
            List<string> OutData = null;

            foreach (string StartString in ColorStartStrings)
            {
                if (Sections.ContainsKey(StartString))
                {
                    ColorData = Sections[StartString];
                    break;
                }
            }

            foreach (string StartString in OutStartStrings)
            {
                if (Sections.ContainsKey(StartString))
                {
                    OutData = Sections[StartString];
                    break;
                }
            }

            if (ColorData == null)
            {
                Log.Warning("Could not find color definition section in file {0}.".Build(LedControlIniFile));
                if (ThrowExceptions)
                {
                    throw new Exception("Could not find  color definition section in file {0}.".Build(LedControlIniFile));
                }
                return;
            }
            else if (ColorData.Count < 1)
            {
                Log.Warning("File {0} does not contain data in the color definition section.".Build(LedControlIniFile));
                if (ThrowExceptions)
                {
                    throw new Exception("File {0} does not contain data in the color definition section.".Build(LedControlIniFile));
                }
                return;
            }

            if (OutData == null)
            {
                Log.Warning("Could not find table config section in file {0}.".Build(LedControlIniFile));
                if (ThrowExceptions)
                {
                    throw new Exception("Could not find table config section section in file {1}.".Build(LedControlIniFile));
                }
                return;
            }
            else if (OutData.Count < 1)
            {
                Log.Warning("File {0} does not contain data in the table config section.".Build(LedControlIniFile));
                if (ThrowExceptions)
                {
                    throw new Exception("File {0} does not contain data in the table config section".Build(LedControlIniFile));
                }
                return;
            }







            ColorConfigurations.ParseLedControlData(ColorData, ThrowExceptions);

            TableConfigurations.ParseLedcontrolData(OutData, ThrowExceptions);

            //ResolveOutputNumbers();
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
                            S.ColorConfig = ColorConfigurations[S.ColorName];

                        }
                    }
                }
            }
        }


        //private void ResolveOutputNumbers()
        //{
        //    //Get the number of required outputs per column
        //    Dictionary<int, int> RequiredOutputs = new Dictionary<int, int>();
        //    foreach (TableConfig TC in TableConfigurations)
        //    {
        //        TC.Columns.Sort();
        //        foreach (TableConfigColumn C in TC.Columns)
        //        {
        //            foreach (TableConfigSetting S in C)
        //            {
        //                int Cnt = (S.OutputType == OutputTypeEnum.RGBOutput ? 3 : 1);
        //                if (RequiredOutputs.ContainsKey(C.Number))
        //                {

        //                    if (RequiredOutputs[C.Number] < Cnt)
        //                    {
        //                        RequiredOutputs[C.Number] = Cnt;
        //                    }
        //                }
        //                else
        //                {
        //                    RequiredOutputs.Add(C.Number, Cnt);
        //                }
        //            }

        //        }

        //    }


        //    //Dump();
        //}


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
        public LedControlConfig(string LedControlIniFilename, int LedWizNumber, bool ThrowExceptions = false)
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
