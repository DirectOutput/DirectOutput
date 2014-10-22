using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;

namespace PinballX
{



    public class Config
    {

        public bool EnableLogging { get; set; }





        #region Serialization


        public static string ConfigFileName
        {
            get
            {
                FileInfo PluginAssemblyFileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
                return PluginAssemblyFileInfo.FullName.Substring(0, PluginAssemblyFileInfo.FullName.Length - PluginAssemblyFileInfo.Extension.Length) + ".xml";
            }
        }

        /// <summary>
        /// Returns a serialized XML representation of the configuration.
        /// </summary>
        /// <returns>XMLString</returns>
        public string GetConfigXml()
        {
            string Xml = "";
            using (MemoryStream ms = new MemoryStream())
            {
                new XmlSerializer(typeof(Config)).Serialize(ms, this);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms, Encoding.Default))
                {
                    Xml = sr.ReadToEnd();
                    sr.Dispose();
                }
            }

            return Xml;
        }


        /// <summary>
        /// Serializes the configuration to a XML file.
        /// </summary>
        /// <param name="FileName">Name of the XML file or null to use the default config filename.</param>
        public void SaveConfigXmlFile(string FileName=null)
        {

            string C = GetConfigXml();

            TextWriter tw = null;
            try
            {
                tw = new StreamWriter((FileName == null ? ConfigFileName : FileName), false);
                tw.Write(C);
                tw.Close();
            }
            catch (Exception e)
            {

                if (tw != null)
                {
                    tw.Close();
                }
                throw new Exception("Could not save PinballX DirectOutput Plugin config to " + (FileName == null ? ConfigFileName : FileName), e);
            }

            tw = null;

        }


        /// <summary>
        /// Instanciates a config object from a cabinet configuration in a XML file.
        /// </summary>
        /// <param name="FileName">Name of the XML file or null to use the default config filename.</param>
        /// <returns>Config object</returns>
        public static Config GetConfigFromXmlFile(string FileName=null)
        {
            string Xml;

            try
            {
                using (StreamReader streamReader = new StreamReader((FileName==null?ConfigFileName:FileName)))
                {
                    Xml = streamReader.ReadToEnd();
                    streamReader.Close();
                }
            }
            catch (Exception E)
            {
                throw new Exception("Could not read PinballX DirectOutput Plugin config file" + (FileName == null ? ConfigFileName : FileName), E);
            }

            return GetConfigFromXml(Xml);
        }



        /// <summary>
        /// Instanciates a Cabinet object from a cabinet configuration in a XML string.
        /// </summary>
        /// <param name="ConfigXml">XML string</param>
        /// <returns>Cabinet object</returns>
        public static Config GetConfigFromXml(string ConfigXml)
        {
            byte[] xmlBytes = Encoding.Default.GetBytes(ConfigXml);
            using (MemoryStream ms = new MemoryStream(xmlBytes))
            {
                try
                {
                    return (Config)new XmlSerializer(typeof(Config)).Deserialize(ms);
                }
                catch (Exception E)
                {

                    Exception Ex = new Exception("Could not deserialize the PinballX DirectOutput Plugin config from XML data.", E);
                    Ex.Data.Add("XML Data", ConfigXml);
                 
                    throw Ex;
                }
            }
        }
        #endregion



    }

}
