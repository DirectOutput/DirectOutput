using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace DirectOutput.GlobalConfiguration
{

    /// <summary>
    /// Global configuration for the DirectOutput framework.<br/> 
    /// </summary>
    public class GlobalConfig
    {

        #region Led Control 


        private int _LedControlMinimumEffectDurationMs = 60;

        /// <summary>
        /// Gets or sets the minimum duration in milliseconds for LedControl effects occupying one output (e.g. contactors).<br/>
        /// This settings has no effect if a duration or blinking is defined for the LedControlEffect.<br/>
        /// If this value is not specified in the globalconfig file, 60 miliseconds will be used by default.
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
        /// Gets or sets the minimum duration in milliseconds for LedControl effects controlling RGB leds.<br/>
        /// This settings has no effect if a duration or blinking is defined for the LedControlEffect.
        /// If this value is not specified in the globalconfig file, 120 miliseconds will be used by default.
        /// </summary>
        /// <value>
        /// The minimum effect duration in milliseconds.
        /// </value>
        public int LedControlMinimumRGBEffectDurationMs
        {
            get { return _LedControlMinimumRGBEffectDurationMs; }
            set { _LedControlMinimumRGBEffectDurationMs = value; }
        }


        private LedControlIniFileList _LedControlIniFiles = new LedControlIniFileList();

        /// TODO: Check serialization of the property.
        /// <summary>
        /// Gets or sets the list of LedControl.ini files.<br/>
        /// </summary>
        /// <value>The list of LedControl.ini files.
        /// </value>
        public LedControlIniFileList LedControlIniFiles
        {
            get { return _LedControlIniFiles; }
            set { _LedControlIniFiles = value; }
        }

        #endregion


        #region Cabinet

        #region Cabinet config file
        private FilePatternList _CabinetConfigFilePatterns = new FilePatternList();


        /// <summary>
        /// Gets or sets the cabinet config file pattern.
        /// </summary>
        /// <value>
        /// The cabinet config file pattern.
        /// </value>
        public FilePatternList CabinetConfigFilePatterns
        {
            get { return _CabinetConfigFilePatterns; }
            set { _CabinetConfigFilePatterns = value; }
        }


        /// <summary>
        ///  FileInfo object for the file containing the configuration of the cabinet (outputs, toys and so on). 
        /// </summary>
        /// <returns>FileInfo object for the file containing the configuration of the cabinet or null if no file has been specified.</returns>
        public FileInfo GetCabinetConfigFile()
        {
            if (CabinetConfigFilePatterns != null)
            {
                return CabinetConfigFilePatterns.GetFirstMatchingFile(GetReplaceValuesDictionary());
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

        #region Cabinet Scripts file patterns
        private FilePatternList _CabinetScriptFilePatterns = new FilePatternList();

        /// <summary>
        /// Gets or sets the search patterns for cabinet scripts files.
        /// </summary>
        /// <value>
        /// The search patterns for cabinet scripts files.
        /// </value>
        public FilePatternList CabinetScriptFilePatterns
        {
            get { return _CabinetScriptFilePatterns; }
            set { _CabinetScriptFilePatterns = value; }
        }


        /// <summary>
        /// Gets a list of FileInfo objects representing the cabinet script files.
        /// </summary>
        /// <returns>A list of FileOnfo objects or a empty list of the files have been found.</returns>
        public List<FileInfo> GetCabinetScriptFiles()
        {
            if (CabinetScriptFilePatterns != null)
            {
                return CabinetScriptFilePatterns.GetMatchingFiles(GetReplaceValuesDictionary());
            }
            return new List<FileInfo>();
        }


        #endregion



        #endregion


        #region Table



        #region Table script files
        private FilePatternList _TableScriptFilePatterns = new FilePatternList();

        /// <summary>
        /// Gets or sets the table script file patterns.
        /// </summary> 
        /// <value>
        /// The table script file patterns.
        /// </value>
        public FilePatternList TableScriptFilePatterns
        {
            get { return _TableScriptFilePatterns; }
            set { _TableScriptFilePatterns = value; }
        }

        /// <summary>
        /// Gets the list of table script files.
        /// </summary>
        /// <param name="FullTableFilename">The filename, inckuding path, to the table.</param>
        /// <returns>
        /// The list of table script files or a empty list if no table script files have been found.
        /// </returns>
        public List<FileInfo> GetTableScriptFiles(string FullTableFilename)
        {
            if (TableScriptFilePatterns != null)
            {
                return TableScriptFilePatterns.GetMatchingFiles(GetReplaceValuesDictionary(FullTableFilename));
            }
            return new List<FileInfo>();
        }
        #endregion


        #region Table Config
        private FilePatternList _TableConfigFilePatterns = new FilePatternList();

        /// <summary>
        /// Gets or sets the config file patterns used to looup the table configuration.
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
        /// The file is lookued up using the list of the property TableConfigFilePatterns.
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
        private bool _EnableLog = false;


        /// <summary>
        /// Gets or sets a value indicating whether impotant events in the framework are logged to a file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if logging is enabled, <c>false</c> if logging is disabled.
        /// </value>
        public bool EnableLogging
        {
            get { return _EnableLog; }
            set { _EnableLog = value; }
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



        private Dictionary<string, string> GetReplaceValuesDictionary(string TableFileName = null, string RomName = "")
        {
            Dictionary<string, string> D = new Dictionary<string, string>();
            if (GetGlobalConfigFile() != null)
            {
                D.Add("GlobalConfigDirectory", GetGlobalConfigDirectory().FullName);
                D.Add("GlobalConfigDir", GetGlobalConfigDirectory().FullName);
            }

            FileInfo FI = new FileInfo(Assembly.GetExecutingAssembly().Location);
            D.Add("DllDirectory", FI.Directory.FullName);
            D.Add("DllDir", FI.Directory.FullName);
            D.Add("AssemblyDirectory", FI.Directory.FullName);
            D.Add("AssemblyDir", FI.Directory.FullName);
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
            if (!RomName.IsNullOrWhiteSpace())
            {
                D.Add("RomName", RomName);
            }


            return D;

        }



        #region Global config properties
        /// <summary>
        /// Gets an list of FileInfo objects for the global script files.<br/>
        /// If no script files are found or if a error occurs when looking for the script files a empty list is returned.<br/>
        /// The script files are looked up using the value of the property GlobalScriptFilePatterns.
        /// </summary>
        /// <returns>List of FileInfo objects for the global script files.</returns>
        public List<FileInfo> GetGlobalScriptFiles()
        {
            return GlobalScriptFilePatterns.GetMatchingFiles();
        }


        private FilePatternList _GlobalScriptFilePatterns = new FilePatternList();

        /// <summary>
        /// Gets or sets the script file patterns used to lookup the global scripts.
        /// </summary>
        /// <value>
        /// The global script file patterns.
        /// </value>
        public FilePatternList GlobalScriptFilePatterns
        {
            get { return _GlobalScriptFilePatterns; }
            set { _GlobalScriptFilePatterns = value; }
        }

        /// <summary>
        /// Path to the directory where the global config is stored (readonly).
        /// </summary>
        /// <returns>string containing to the global config directory.</returns>
        public string GlobalConfigDirectoryName()
        {
            if (GlobalConfigFilename.IsNullOrWhiteSpace()) { return null; }
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Config");
        }

        /// <summary>
        /// Gets a DirectoryInfo object for the global config directory.
        /// </summary>
        /// <returns>
        /// The DirectoryInfo object for the global config directory or null if no GlobalConfigFilename is defined.
        /// </returns>
        public DirectoryInfo GetGlobalConfigDirectory()
        {

            if (GlobalConfigFilename.IsNullOrWhiteSpace()) { return null; }
            return GetGlobalConfigFile().Directory;

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

                    GlobalConfig GC = GetGlobalConfigFromGlobalConfigXml(Xml);
                    if (GC != null)
                    {
                        GC.GlobalConfigFilename = GlobalConfigFileName;
                    }
                    return GC;

                }
                else
                {

                    return null;
                }
            }
            catch
            {

                return null;
            }
        }


        /// <summary>
        /// Instanciates a GlobalConfig object from a global configuration in a XML string.
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
