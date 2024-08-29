﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using DirectOutput.General;


namespace DirectOutput.GlobalConfiguration
{

    /// <summary>
    /// Global configuration for the DirectOutput framework.<br/> 
    /// </summary>
    public class GlobalConfig
    {

        

        #region IniFiles



        private int _LedWizDefaultMinCommandIntervalMs = 10;

        /// <summary>
        /// Gets or sets the minimal interval between commands sent to the LedWiz.
        /// This works around a design defect in the LedWiz that makes it misinterpret
        /// commands if USB packets are sent too quickly.  See LedWiz.cs for details.
        /// </summary>
        /// <value>
        /// The minimum interval between commands sent to LedWiz units, in milliseconds.
        /// </value>
        public int LedWizDefaultMinCommandIntervalMs
        {
            get { return _LedWizDefaultMinCommandIntervalMs; }
            set { _LedWizDefaultMinCommandIntervalMs = value.Limit(0, 1000); }
        }


        private int _LedControlMinimumEffectDurationMs = 60;

        /// <summary>
        /// Gets or sets the minimum duration in milliseconds for LedControl effects occupying one output (e.g. contactors).<br/>
        /// This settings has no effect if a duration or blinking is defined for the LedControlEffect.<br/>
        /// If this value is not specified in the global config file, 60 milliseconds will be used by default.
        /// </summary>
        /// <value>
        /// The minimum effect duration in milliseconds.
        /// </value>
        public int LedControlMinimumEffectDurationMs
        {
            get { return _LedControlMinimumEffectDurationMs; }
            set { _LedControlMinimumEffectDurationMs = value; }
        }

        private int _LedControlMinimumRGBEffectDurationMs = 120;

        /// <summary>
        /// Gets or sets the minimum duration in milliseconds for LedControl effects controlling RGB LEDs.<br/>
        /// This settings has no effect if a duration or blinking is defined for the LedControlEffect.
        /// If this value is not specified in the global config file, 120 milliseconds will be used by default.
        /// </summary>
        /// <value>
        /// The minimum effect duration in milliseconds.
        /// </value>
        public int LedControlMinimumRGBEffectDurationMs
        {
            get { return _LedControlMinimumRGBEffectDurationMs; }
            set { _LedControlMinimumRGBEffectDurationMs = value; }
        }


        private int _PacLedDefaultMinCommandIntervalMs = 10;

        /// <summary>
        /// Gets or sets the minimal interval between commands sent to the PacLed64.
        /// The PacLed appears to have a design defect similar to the LedWiz that requires
        /// a similar workaround of spacing out commands sent to the device.
        /// </summary>
        /// <value>
        /// The minimum interval between commands sent to PacLed units, in milliseconds.
        /// </value>
        public int PacLedDefaultMinCommandIntervalMs
        {
            get { return _PacLedDefaultMinCommandIntervalMs; }
            set { _PacLedDefaultMinCommandIntervalMs = value.Limit(0, 1000); }
        }




        private string _IniFilesPath="";

        /// <summary>
        /// Gets or sets the path to the ini files used for table configurations
        /// </summary>
        /// <value>
        /// The path to the directory containing the ini files used for table configurations.
        /// </value>
        public string IniFilesPath
        {
            get { return _IniFilesPath; }
            set { _IniFilesPath = value; }
        }


        /// <summary>
        /// Gets the a dictionary containing all ini files (file) and their number (key).
        /// </summary>
        /// <param name="TableFilename">The table filename.</param>
        /// <returns>Dictionary of ini files. Key is the ini file number, value is the ini file.</returns>
        public Dictionary<int, FileInfo> GetIniFilesDictionary(string TableFilename = "")
        {
            // Build the array of possible paths for the ini files
            List<string> LookupPaths = new List<string>();

            // start with the configured path list
            if (!IniFilesPath.IsNullOrWhiteSpace())
            {
                try
                {
                    DirectoryInfo DI = new DirectoryInfo(IniFilesPath);
                    if (DI.Exists)
                        LookupPaths.Add(DI.FullName);
                }
                catch (Exception E) 
                {
                    Log.Exception("The specified IniFilesPath {0} could not be used due to a exception.".Build(IniFilesPath), E);
                }
            }

            // add the table file folder, if available
            if (!TableFilename.IsNullOrWhiteSpace())
            {
                try
                {
                    if (new FileInfo(TableFilename).Directory.Exists)
                        LookupPaths.Add(new FileInfo(TableFilename).Directory.FullName);
                }
                catch { }
            }

            // add the global config folder
			if (GetGlobalConfigDirectory() != null)
				LookupPaths.Add(GetGlobalConfigDirectory().FullName);

            // add the working directory
			LookupPaths.Add(Directory.GetCurrentDirectory());

            // add the root DOF install folder
            var InstallFolder = DirectOutputHandler.GetInstallFolder();
			if (InstallFolder != null)
				LookupPaths.Add(InstallFolder);

            // Build the dictionary of ini files
            Dictionary<int, FileInfo> IniFiles = new Dictionary<int, FileInfo>();

            bool FoundIt = false;
            string[] LedControlFilenames = { "directoutputconfig", "ledcontrol" };

            foreach (string LedControlFilename in LedControlFilenames)
            {
                foreach (string P in LookupPaths) 
                {
                    DirectoryInfo DI = new DirectoryInfo(P);

                    List<FileInfo> Files = new List<FileInfo>();
                    foreach (FileInfo FI in DI.EnumerateFiles())
                    {
                        if (FI.Name.ToLower().StartsWith(LedControlFilename.ToLower()) && FI.Name.ToLower().EndsWith(".ini"))
                        {
                            Files.Add(FI);
                        }
                    }


                    foreach (FileInfo FI in Files)
                    {
                        if (string.Equals(FI.Name, "{0}.ini".Build(LedControlFilename), StringComparison.OrdinalIgnoreCase))
                        {
                            if (!IniFiles.ContainsKey(1))
                            {
                                IniFiles.Add(1, FI);
                                FoundIt = true;
                            }
                            else
                            {
                                Log.Warning("Found more than one ini file with for number 1. Likely you have a ini file without a number and and a second one with number 1.");
                            }
                        }
                        else
                        {
                            string F = FI.Name.Substring(LedControlFilename.Length, FI.Name.Length - LedControlFilename.Length - 4);
                            if (F.IsInteger())
                            {
                                int LedWizNr = -1;
                                if (int.TryParse(F, out LedWizNr))
                                {
                                    if (!IniFiles.ContainsKey(LedWizNr))
                                    {
                                        IniFiles.Add(LedWizNr, FI);
                                        FoundIt = true;
                                    }
                                    else
                                    {
                                        Log.Warning("Found more than one ini file with number {0}.".Build(LedWizNr));
                                    }
 
                                }

                            }

                        }
                    };
                    if (FoundIt) break;
                }
                if (FoundIt) break;
            }



            return IniFiles;

        }

        /// <summary>
        /// Gets a FileInfo object pointing to the table mapping file or null if no table mapping file exists.
        /// </summary>
        /// <param name="TableFilename">The table filename (optional).</param>
        /// <returns>FileInfo object pointing to the table mapping file or null if no table mapping file exists</returns>
        public FileInfo GetTableMappingFile(string TableFilename="")
        {
            Dictionary<int,FileInfo> IniFileDict = GetIniFilesDictionary(TableFilename);

            if (IniFileDict != null && IniFileDict.Count > 0)
            {
                DirectoryInfo DI = IniFileDict.First().Value.Directory;

                FileInfo FI = DI.GetFiles("tablemappings.*").FirstOrDefault();

                return FI;
            }
            else
            {
                return null;
            }



        }


        #endregion


        private FilePattern _ShapeDefinitionFilePattern = null;

        /// <summary>
        /// Gets or sets the path and name for the file containing shape definitions.
        /// </summary>
        /// <value>
        /// The path and name of the file containing shape definitions.
        /// </value>
        public FilePattern ShapeDefinitionFilePattern
        {
            get { return _ShapeDefinitionFilePattern; }
            set { _ShapeDefinitionFilePattern = value; }
        }

        /// <summary>
        ///  Gets the FileInfo object for the xml file defining the shapes to be used by DOF.
        /// </summary>
        /// <returns>FileInfo object for the xml file defining the shapes to be used by DOF</returns>
        public FileInfo GetShapeDefinitionFile(string TableFilename="", string RomName="")
        {
            if (ShapeDefinitionFilePattern!=null && !ShapeDefinitionFilePattern.Pattern.IsNullOrWhiteSpace() && ShapeDefinitionFilePattern.IsValid)
            {
                return ShapeDefinitionFilePattern.GetFirstMatchingFile(GetReplaceValuesDictionary(TableFilename, RomName));
            }
            Dictionary<int, FileInfo> IniFilesDict = GetIniFilesDictionary(TableFilename);
            if (IniFilesDict.Count > 0)
            {
                FileInfo FI=new FileInfo(Path.Combine(IniFilesDict.Select(KV=>KV.Value).First().Directory.FullName,"DirectOutputShapes.xml"));
                if (FI.Exists)
                {
                    return FI;
                }
            }
            FileInfo FII = new FilePattern("{DllDir}\\DirectOutputShapes.xml").GetFirstMatchingFile(GetReplaceValuesDictionary(TableFilename, RomName));
            if (FII != null && FII.Exists)
            {
                return FII;
            }


            return null;
        }




        #region Cabinet

        #region Cabinet config file


        private FilePattern _CabinetConfigFilePattern=new FilePattern();

        /// <summary>
        /// Gets or sets the path and name of the cabinet config file.
        /// </summary>
        /// <value>
        /// The path and name of the cabinet config file.
        /// </value>
        public FilePattern CabinetConfigFilePattern
        {
            get { return _CabinetConfigFilePattern; }
            set { _CabinetConfigFilePattern = value; }
        }
        



        /// <summary>
        ///  FileInfo object for the file containing the configuration of the cabinet (outputs, toys and so on). 
        /// </summary>
        /// <returns>FileInfo object for the file containing the configuration of the cabinet or null if no file has been specified.</returns>
        public FileInfo GetCabinetConfigFile()
        {
            if (!CabinetConfigFilePattern.Pattern.IsNullOrWhiteSpace() && CabinetConfigFilePattern.IsValid)
            {
                return CabinetConfigFilePattern.GetFirstMatchingFile(GetReplaceValuesDictionary());
            }
   
            return null;
        }


        /// <summary>
        /// Gets the cabinet config directory.
        /// </summary>
        /// <returns>The DirectoryInfo object for the cabinet config directory or null if no CabinetConfigFile is available.</returns>
        public DirectoryInfo GetCabinetConfigDirectory()
        {
            FileInfo CC = GetCabinetConfigFile();
            if (CC != null)
            {
                return CC.Directory;
            }
            return null;
        }
        #endregion


        #endregion


        #region Table






        #region Table Config
        private FilePatternList _TableConfigFilePatterns = new FilePatternList();

        /// <summary>
        /// Gets or sets the config file patterns used to lookup the table configuration.
        /// </summary>
        /// <value>
        /// The table config file patterns.
        /// </value>
        public FilePatternList TableConfigFilePatterns
        {
            get { return _TableConfigFilePatterns; }
            set { _TableConfigFilePatterns = value; }
        }

        /// <summary>
        /// Gets a FileInfo object for the table config file.<br/>
        /// The file is looked up using the list of the property TableConfigFilePatterns.
        /// If more than one file matches the search patterns, only the first file is returned.
        /// </summary>
        /// <param name="FullTableFilename">The table filename (The *.vpt file for the table, not the config file).</param>
        /// <returns>A FileInfo object for the table config file or null if no matching file was found.</returns>
        public FileInfo GetTableConfigFile(string FullTableFilename)
        {
            return TableConfigFilePatterns.GetFirstMatchingFile(GetReplaceValuesDictionary(FullTableFilename));
        }

        #endregion


        #endregion



        #region Logging
        private bool _EnableLog = true;


        /// <summary>
        /// Gets or sets a value indicating whether important events in the framework are logged to a file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if logging is enabled, <c>false</c> if logging is disabled.
        /// </value>
        public bool EnableLogging
        {
            get { return _EnableLog; }
            set { _EnableLog = value; }
        }

        private bool _ClearLogOnSessionStart=true;

        /// <summary>
        /// Gets or sets a value indicating whether DOF clears the log file on session start.
        /// </summary>
        /// <value>
        /// <c>true</c> if DOF should clear the log file on session start; otherwise, <c>false</c>.
        /// </value>
        public bool ClearLogOnSessionStart
        {
            get { return _ClearLogOnSessionStart; }
            set { _ClearLogOnSessionStart = value; }
        }
        



        private FilePattern _LogFilePattern = new FilePattern(".\\DirectOutput.log");


        /// <summary>
        /// Gets or sets the log file pattern.<br/>
        /// The log file pattern supports the following placeholders:
        /// 
        /// - {GlobalConfigDir}
        /// - {DllDir}
        /// - {TableDir}
        /// - {TableName}
        /// - {RomName}
        /// - {DateTime}
        /// - {Date}
        /// - {Time}
        /// 
        /// Note that {DllDir} is a misnomer; it's actually the installation folder.
        /// In the newer install configuration, the DLLs are in subfolders that separate
        /// them by architecture (x86\, x64\).  Since {DllDir} is used in user-editable
        /// config files for locating assets (e.g., the predefined .png files for matrix
        /// effects), we have to preserve its semantic meaning of the folder containing
        /// the asset files.  It's still the parent folder of the DLLs as long as you
        /// think of the DLL name as a relative path starting with the the x86\ or x64\ 
        /// subfolder name.
        /// 
        /// </summary>
        /// <value>
        /// The log file pattern.
        /// </value>
        public FilePattern LogFilePattern
        {
            get { return _LogFilePattern; }
            set { _LogFilePattern = value; }
        }

        /// <summary>
        /// Gets the log filename based on the LogFilePattern with replaced placeholders.
        /// </summary>
        /// <param name="TableFilename">The table filename.</param>
        /// <param name="RomName">Name of the rom.</param>
        /// <returns></returns>
        public string GetLogFilename(string TableFilename = "", string RomName = "")
        {
            Dictionary<string, string> R = GetReplaceValuesDictionary(TableFilename, RomName);
            R.Add("DateTime", DateTime.Now.ToString("yyyyMMdd_hhmmss"));
            R.Add("Date", DateTime.Now.ToString("yyyyMMdd"));
            R.Add("Time", DateTime.Now.ToString("hhmmss"));

            return LogFilePattern.ReplacePlaceholders(R);
        }

        #endregion



        internal Dictionary<string, string> GetReplaceValuesDictionary(string TableFileName = null, string RomName = "")
        {
            Dictionary<string, string> D = new Dictionary<string, string>();

            // add the global config folder
            if (GetGlobalConfigFile() != null)
            {
                D.Add("GlobalConfigDirectory", GetGlobalConfigDirectory().FullName);
                D.Add("GlobalConfigDir", GetGlobalConfigDirectory().FullName);
            }

			// The {DllDir} and {AssemblyDir} variables have actually always been
            // semantically the install folder, since the main use in user-facing
            // config files is as the location for predefined asset files (e.g.,
            // the matrix effect .png files).  In the new x86/x64 combined install
            // configuration, the literal DLL folder is an architecture-specific
            // subfolder of the install folder.
			var InstallDir = DirectOutputHandler.GetInstallFolder();
			FileInfo FI = new FileInfo(InstallDir.IsNullOrEmpty() ? Directory.GetCurrentDirectory() : InstallDir);
            D.Add("DllDirectory", FI.Directory.FullName);
            D.Add("DllDir", FI.Directory.FullName);
            D.Add("AssemblyDirectory", FI.Directory.FullName);
            D.Add("AssemblyDir", FI.Directory.FullName);

            // add the table folder
            if (!TableFileName.IsNullOrWhiteSpace())
            {
                FI = new FileInfo(TableFileName);
                if (FI.Directory.Exists)
                {
                    D.Add("TableDirectory", FI.Directory.FullName);
                    D.Add("TableDir", FI.Directory.FullName);
                    D.Add("TableDirectoryName", FI.Directory.Name);
                    D.Add("TableDirName", FI.Directory.Name);
                }

                D.Add("TableName", Path.GetFileNameWithoutExtension(FI.FullName));
            }
            
            // add the ROM name
            if (!RomName.IsNullOrWhiteSpace())
                D.Add("RomName", RomName);

            return D;
        }



        #region Global config properties


        /// <summary>
        /// Path to the directory where the global config is stored (readonly).
        /// </summary>
        /// <returns>string containing to the global config directory.</returns>
        public string GlobalConfigDirectoryName()
        {
            DirectoryInfo DI = GetGlobalConfigDirectory();
            if (DI == null) return null;
            return DI.FullName;
        }

        /// <summary>
        /// Gets a DirectoryInfo object for the global config directory.
        /// </summary>
        /// <returns>
        /// The DirectoryInfo object for the global config directory or null if no GlobalConfigFilename is defined.
        /// </returns>
        public DirectoryInfo GetGlobalConfigDirectory()
        {
            FileInfo FI = GetGlobalConfigFile();
            if (FI == null) return null;
            return FI.Directory;

        }


        private string _GlobalConfigFilename = "";

        /// <summary>
        /// Gets or sets the global config filename.
        /// </summary>
        /// <value>
        /// The global config filename.
        /// </value>
        [XmlIgnore]
        public string GlobalConfigFilename
        {
            get { return _GlobalConfigFilename; }
            set { _GlobalConfigFilename = value; }
        }


        /// <summary>
        /// Gets a FileInfo object for the global config file.
        /// </summary>
        /// <returns>FileInfo object for the global config file or null if no filename has been set.</returns>
        public FileInfo GetGlobalConfigFile()
        {
            if (GlobalConfigFilename.IsNullOrWhiteSpace()) { return null; }
            return new FileInfo(GlobalConfigFilename);
        }
        #endregion


        #region Serialization
        /// <summary>
        /// Returns a serialized XML representation of the global configuration.
        /// </summary>
        /// <returns>XMLString</returns>
        public string GetGlobalConfigXml()
        {

            XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add(string.Empty, string.Empty);
            XmlSerializer Serializer = new XmlSerializer(typeof(GlobalConfig));
            MemoryStream Stream = new MemoryStream();
            XmlWriterSettings Settings = new XmlWriterSettings();
            Settings.Indent = true;
            Settings.NewLineOnAttributes = true;

            XmlWriter Writer = XmlWriter.Create(Stream, Settings);
            Writer.WriteStartDocument();
            Writer.WriteComment("Global configuration for the DirectOutput framework.");
            Writer.WriteComment("Saved by DirectOutput Version {1}: {0}".Build(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"), System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            Serializer.Serialize(Writer, this, Namespaces);
            Writer.WriteEndDocument();
            Writer.Flush();
            Stream.Position = 0;
            string XML;
            using (StreamReader sr = new StreamReader(Stream, Encoding.Default))
            {
                XML = sr.ReadToEnd();
                sr.Dispose();
            }
            Stream.Dispose();

            return XML;
        }



        /// <summary>
        /// Instanciates a GlobalConfig object from a global configuration in a XML file.<br/>
        /// If the global config file does not exist or can not be loaded, null will be returned.
        /// </summary>
        /// <param name="GlobalConfigFileName">Name of the global config XML file.</param>
        /// <returns>GlobalConfig object or null.</returns>
        public static GlobalConfig GetGlobalConfigFromConfigXmlFile(string GlobalConfigFileName)
        {

            try
            {
                if (File.Exists(GlobalConfigFileName))
                {

                    string Xml = General.FileReader.ReadFileToString(GlobalConfigFileName);

                    // For debug purposes only: copy the contents to the log to debug file sourcing issues
                    // Log.Write("Read global config from \"" + GlobalConfigFileName + "\", contents follow:\n====\n"
                    //    + Xml + "\n====");

                    GlobalConfig GC = GetGlobalConfigFromGlobalConfigXml(Xml);
                    if (GC != null)
                    {
                        GC.GlobalConfigFilename = GlobalConfigFileName;
                    }
                    return GC;

                }
                else
                {
                    Log.Write("Global config file \"" + GlobalConfigFileName + "\" does not exist; no global config loaded");
                    return null;
                }
            }
            catch
            {

                return null;
            }
        }


        /// <summary>
        /// Instantiates a GlobalConfig object from a global configuration in a XML string.
        /// </summary>
        /// <param name="ConfigXml">XML string</param>
        /// <returns>GlobalConfig object for the specified ConfigXML or null if the XML data can not be deserialized.</returns>
        public static GlobalConfig GetGlobalConfigFromGlobalConfigXml(string ConfigXml)
        {
            try
            {

                byte[] xmlBytes = Encoding.Default.GetBytes(ConfigXml);
                using (MemoryStream ms = new MemoryStream(xmlBytes))
                {
                    return (GlobalConfig)new XmlSerializer(typeof(GlobalConfig)).Deserialize(ms);
                }
            }
            catch { return null; }

        }

        /// <summary>
        /// Saves the GlobalConfig to the file specified in GlobalConfigFilename.<br />
        /// Before saving the current global config file is backed up.
        /// </summary>
        /// <param name="GlobalConfigFilename">(Optional)Global config filename. If no value is supplied the value of the property GlobalConfigFilename will be used.</param>
        public void SaveGlobalConfig(string GlobalConfigFilename = "")
        {
            string GCFileName = (GlobalConfigFilename.IsNullOrWhiteSpace() ? this.GlobalConfigFilename : GlobalConfigFilename);
            if (GCFileName.IsNullOrWhiteSpace())
            {
                ArgumentException Ex = new ArgumentException("No filename for GlobalConfig file has been supplied. Looking up the filename from the property GlobalConfigFilename failed as well");
                throw Ex;
            }
            if (File.Exists(GCFileName))
            {
                //Create a backup of the current global config file
                File.Copy(GCFileName, Path.Combine(Path.GetDirectoryName(GCFileName), "{1} old (replaced {0}){2}".Build(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"), Path.GetFileNameWithoutExtension(GCFileName), Path.GetExtension(GCFileName))));
            };
            DirectoryInfo GCDirectory = new FileInfo(GCFileName).Directory;

            GCDirectory.CreateDirectoryPath();
            GetGlobalConfigXml().WriteToFile(GCFileName, false);

        }
        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalConfig"/> class.
        /// </summary>
        public GlobalConfig()
        {

         

        }

    }
}
