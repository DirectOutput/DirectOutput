using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace DirectOutputConfigTester
{
    public class Settings
    {
        private string _LastRomName = "";

        public string LastRomName
        {
            get { return _LastRomName; }
            set { _LastRomName = value; }
        }
        private string _LastTableFilename = "";

        public string LastTableFilename
        {
            get { return _LastTableFilename; }
            set { _LastTableFilename = value; }
        }
        private string _LastGlobalConfigFilename = "";

        public string LastGlobalConfigFilename
        {
            get { return _LastGlobalConfigFilename; }
            set { _LastGlobalConfigFilename = value; }
        }

        private List<string> _RomNames = new List<string>();

        public List<string> RomNames
        {
            get { return _RomNames; }
            set { _RomNames = value; }
        }


        private List<string> _TableFilenames = new List<string>();

        public List<string> TableFilenames
        {
            get { return _TableFilenames; }
            set { _TableFilenames = value; }
        }


        private List<string> _GlobalConfigFilenames = new List<string>();

        public List<string> GlobalConfigFilenames
        {
            get { return _GlobalConfigFilenames; }
            set { _GlobalConfigFilenames = value; }
        }

        private int _PulseDurationMs = 100;

        public int PulseDurationMs
        {
            get { return _PulseDurationMs; }
            set { _PulseDurationMs = value; }
        }


        public void SaveSettings()
        {
            try
            {
                string Xml = "";
                using (MemoryStream ms = new MemoryStream())
                {
                    new XmlSerializer(typeof(Settings)).Serialize(ms, this);
                    ms.Position = 0;

                    using (StreamReader sr = new StreamReader(ms, Encoding.Default))
                    {
                        Xml = sr.ReadToEnd();

                    }

                }
                new DirectoryInfo("config").CreateDirectoryPath();
                Xml.WriteToFile("Config\\DirectOutputTesterSettings.xml");
            }
            catch
            {

            }
        }


        public static Settings LoadSettings()
        {
            try
            {
                string SettingsXML = DirectOutput.General.FileReader.ReadFileToString("Config\\DirectOutputTesterSettings.xml");
                
                using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(SettingsXML)))
                {
                    Settings S = (Settings)new XmlSerializer(typeof(Settings)).Deserialize(ms);
                    
                    return S;
                }
            }
            catch 
            {
                return new Settings();
            }
        }

    }
}
