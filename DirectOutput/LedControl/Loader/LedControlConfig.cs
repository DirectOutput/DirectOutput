using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using DirectOutput.GlobalConfiguration;

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

        public Version MinDOFVersion { get; set; }



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


        private FileInfo _LedControlIniFile;

        /// <summary>
        /// Gets the led control ini file.
        /// </summary>
        /// <value>
        /// The led control ini file.
        /// </value>
        public FileInfo LedControlIniFile
        {
            get { return _LedControlIniFile; }
            private set { _LedControlIniFile = value; }
        }






        /// <summary>
        /// Parses the ledcontrol.ini file.
        /// </summary>
        /// <param name="LedControlIniFile">The ledcontrol.ini FileInfo object.</param>
        /// <param name="RomName">Specify a rom name at loading stage to ignore parsing of non matching lines</param>
        /// <param name="GlobalConfig">The current global configuration.</param>
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
        private void ParseLedControlIni(FileInfo LedControlIniFile, string RomName, GlobalConfig GlobalConfig, bool ThrowExceptions = false)
        {
            string[] ColorStartStrings = { "[Colors DOF]", "[Colors LedWiz]" };
            string[] OutStartStrings = { "[Config DOF]", "[Config outs]" };
            string[] VariableStartStrings = { "[Variables DOF]" };
            string[] VersionStartStrings = { "[version]" };
            string[] TableVariableStartStrings = { "[TableVariables]" }; ;
            string FileData = "";

            #region Read file
            try
            {
                FileData = General.FileReader.ReadFileToString(LedControlIniFile);
            }
            catch (Exception E)
            {
                Log.Exception($"Could not read file {LedControlIniFile}.", E);
                if (ThrowExceptions)
                {

                    throw new Exception($"Could not read file {LedControlIniFile}.", E);
                }
            }
            if (FileData.IsNullOrWhiteSpace())
            {
                Log.Warning($"File {LedControlIniFile} does not contain data.");
                if (ThrowExceptions)
                {
                    throw new Exception($"File {LedControlIniFile} does not contain data.");
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
                                while (Sections.ContainsKey($"{SectionHeader} {Cnt}"))
                                {
                                    Cnt++;
                                    if (Cnt > 999) { throw new Exception($"Section header {SectionHeader} exists to many times."); }
                                }
                                SectionHeader = $"{SectionHeader} {Cnt}";
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
                    while (Sections.ContainsKey($"{SectionHeader} {Cnt}"))
                    {
                        Cnt++;
                        if (Cnt > 999) { throw new Exception($"Section header {SectionHeader} exists to many times."); }
                    }
                    SectionHeader = $"{SectionHeader} {Cnt}";
                }
                Sections.Add(SectionHeader, SectionData);
                SectionData = new List<string>();
            }
            SectionData = null;
            #endregion

            FileData = null;

            List<string> ColorData = GetSection(Sections, ColorStartStrings);
            List<string> OutData = GetSection(Sections, OutStartStrings);
            List<string> VariableData = GetSection(Sections, VariableStartStrings);
            List<string> VersionData = GetSection(Sections, VersionStartStrings);
            List<string> TableVariableData = GetSection(Sections, TableVariableStartStrings);

            if (VersionData != null && VersionData.Count > 0)
            {
                MinDOFVersion = null;

                string MinDofVersionLine = VersionData.FirstOrDefault(S => S.ToLowerInvariant().StartsWith("mindofversion="));

                if (MinDofVersionLine != null)
                {
                    string MinDofVersionString = MinDofVersionLine.Substring("mindofversion=".Length);

                    try
                    {
                        MinDOFVersion = new Version(MinDofVersionString);
                    }
                    catch (Exception)
                    {
                        Log.Exception("Could not parse line {1} from file {0}".Build(LedControlIniFile, MinDofVersionLine));
                        MinDOFVersion = null;
                    }
                    if (MinDOFVersion != null)
                    {
                        Log.Write("Min DOF Version is {0} for file {1}".Build(MinDOFVersion.ToString(), LedControlIniFile.Name));
                    }

                }
                else
                {
                    Log.Warning("No DOF version information found in file {0}.".Build(LedControlIniFile));
                }

            }
            else
            {
                Log.Warning($"No version section found in file {LedControlIniFile}.");
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
                    throw new Exception($"Could not find table config section section in file {LedControlIniFile}.");
                }
                return;
            }
            else if (OutData.Count < 1)
            {
                Log.Warning($"File {LedControlIniFile} does not contain data in the table config section.");
                if (ThrowExceptions)
                {
                    throw new Exception($"File {LedControlIniFile} does not contain data in the table config section");
                }
                return;
            }

            //Resolve tables variables first in case they override global variables (like custom flasher mx shapes)
            if (TableVariableData != null) {
                Log.Write($"Resolving Tables Variables ({LedControlIniFile})");
                ResolveTableVariables(OutData, TableVariableData);
            }

            if (VariableData != null)
            {
                Log.Write($"Resolving Global Variables ({LedControlIniFile})");
                ResolveVariables(OutData, VariableData);
            }

            Log.Write($"Parsing Color Configurations ({LedControlIniFile})");
            ColorConfigurations.ParseLedControlData(ColorData, ThrowExceptions);
            Log.Write($"{ColorConfigurations.Count} Color Configurations parsed.");

            Log.Write($"Parsing Tables Configurations ({LedControlIniFile})");
            TableConfigurations.ParseLedcontrolData(OutData, RomName, GlobalConfig, ThrowExceptions);
            Log.Write($"{TableConfigurations.Count} Tables Configurations parsed.");

            //ResolveOutputNumbers();
            ResolveRGBColors();

            this.LedControlIniFile = LedControlIniFile;
        }

        private List<string> GetSection(Dictionary<string, List<string>> Sections, IEnumerable<string> SectionStartStrings)
        {
            foreach (string StartString in SectionStartStrings)
            {
                if (Sections.ContainsKey(StartString))
                {
                    return Sections[StartString];
                }
            }

            return null;
        }


        private void ResolveTableVariables(List<String> DataToResolve, List<string> VariableData)
        {

            TableVariablesDictionary VD = new TableVariablesDictionary(VariableData);


            for (int i = 0; i < DataToResolve.Count ; i++)
            {
                string D = DataToResolve[i].Trim();
                bool Updated = false;
                if (!D.IsNullOrWhiteSpace())
                {
                    int TP = D.IndexOf(",");
                    if (TP > 0)
                    {
                        string TableName = D.Substring(0, TP).Trim();
                        if (VD.ContainsKey(TableName))
                        {
                            foreach (KeyValuePair<string, string> KV in VD[TableName])
                            {
                                string N = KV.Key;
                                if (!N.StartsWith("@")) N = "@" + N;
                                if (!N.EndsWith("@")) N += "@";
                                D = D.Replace(N, KV.Value);
                                Updated = true;
                            }
                        }
                    }
                }
                if (Updated)
                {
                    DataToResolve[i] = D;
                  
                }
            }

        }


        private void ResolveVariables(List<String> DataToResolve, List<string> VariableData)
        {
            VariablesDictionary VD = new VariablesDictionary(VariableData);
            foreach (KeyValuePair<string, string> KV in VD)
            {
                string N = KV.Key;
                if (!N.StartsWith("@")) N = "@" + N;
                if (!N.EndsWith("@")) N += "@";

                for (int i = 0; i < DataToResolve.Count; i++)
                {
                    DataToResolve[i] = DataToResolve[i].Replace(N, KV.Value);
                }
            }

        }

        private void ResolveRGBColors()
        {

            foreach (TableConfig TC in TableConfigurations)
            {
                foreach (TableConfigColumn C in TC.Columns)
                {
                    foreach (TableConfigSetting S in C)
                    {
                        if (ColorConfigurations.Any(CC=>CC.Name.Equals(S.ColorName,StringComparison.InvariantCultureIgnoreCase)))
                        {
                            S.ColorConfig = ColorConfigurations.First(CC=>CC.Name.Equals(S.ColorName,StringComparison.InvariantCultureIgnoreCase));

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
        /// <param name="RomName">Specify a rom name at loading stage to ignore parsing of non matching lines</param>
        /// <param name="GlobalConfig">The current global configuration.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <exception cref="System.Exception">File {0} does not contain data.
        /// or
        /// Could not find {0} section in file {1}.
        /// or
        /// File {1} does not contain data in the {0} section.
        /// or
        /// Section {0} of file {1} does not have the same number of columns in all lines.</exception>
        public LedControlConfig(string LedControlIniFilename, int LedWizNumber, string RomName, GlobalConfig GlobalConfig, bool ThrowExceptions = false)
            : this()
        {
            ParseLedControlIni(new FileInfo(LedControlIniFilename), RomName, GlobalConfig, ThrowExceptions);
            this.LedWizNumber = LedWizNumber;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LedControlConfig" /> class.
        /// Parses the ledcontrol.ini file.
        /// </summary>
        /// <param name="LedControlIniFile">The ledcontrol.ini FileInfo object.</param>
        /// <param name="LedWizNumber">The number of the LedWizEquivalent to be used.</param>
        /// <param name="RomName">Specify a rom name at loading stage to ignore parsing of non matching lines</param>
        /// <param name="GlobalConfig">The current global configuration.</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <exception cref="System.Exception">File {0} does not contain data.
        /// or
        /// Could not find {0} section in file {1}.
        /// or
        /// File {1} does not contain data in the {0} section.
        /// or
        /// Section {0} of file {1} does not have the same number of columns in all lines.</exception>
        public LedControlConfig(FileInfo LedControlIniFile, int LedWizNumber, string RomName, GlobalConfig GlobalConfig, bool ThrowExceptions = false)
            : this()
        {
            ParseLedControlIni(LedControlIniFile, RomName, GlobalConfig, ThrowExceptions);
            this.LedWizNumber = LedWizNumber;
        }

    }
}
