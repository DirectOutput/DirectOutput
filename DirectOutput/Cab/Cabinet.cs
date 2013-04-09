using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;
using DirectOutput.FX;
using DirectOutput.Cab.Out.LW;
using DirectOutput.Cab.Toys;
using DirectOutput.Cab.Out;
using DirectOutput.Cab.Toys.LWEquivalent;


namespace DirectOutput.Cab
{

    /// <summary>
    /// Cabinet objects are describing the parts of a cabinet. 
    /// </summary>
    public class Cabinet
    {
        /// <summary>
        /// If no IOutputController objects of type LedWiz and no IToy objects of type LedWizEquivalent are configured for the Cabinet,
        /// this method will add a LedWiz and a LedWizEquivalent object for every Ledwiz connected to the system.<br/>
        /// </summary>
        public void AutoConfig()
        {
            Log.Write("Cabinet auto configuration started");
            if (!OutputControllers.Any(OC => OC is LedWiz))
            {
                Log.Write("No predefined LedWiz controllers found in cabinet config. Will try to located LedWiz units.");
        
                List<int> Numbers = LedWiz.GetLedwizNumbers();
                foreach (int N in Numbers)
                {
                    LedWiz LW = new LedWiz(N);
                    LW.AddMissingOutputs();
                    OutputControllers.Add(LW);
                    Log.Write("Added LedWiz Nr. {0}".Build(LW.Number));
        
                }
            }
            if (!Toys.Any(Toy => Toy is LedWizEquivalent))
            {
                Log.Write("No predefined LedWizequivalent toys found in cabinet config. Will add a LedWizEquivalent toy for every LedWiz unit connected to the system.");
        
                foreach (IOutputController OC in OutputControllers.Where(OC => OC is LedWiz))
                {
                    Toys.Add(new LedWizEquivalent((LedWiz)OC));
                    Log.Write("Added LedWizEquivalent for LedWiz Nr. {0}".Build(((LedWiz)OC).Number));
        
                }
            }
            Log.Write("Cabinet auto configuration finished");
        }



        #region Properties
        /// <summary>
        /// Name of the Cabinet.
        /// </summary>
        [XmlElementAttribute(Order = 1)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the filename from which the cabiet configuration was loaded.
        /// </summary>
        /// <value>
        /// The filename of the cabinet configuration file.
        /// </value>
        [XmlIgnoreAttribute]
        public string CabinetConfigurationFilename { get; set; }

        private Toys.ToyList _Toys;

        /// <summary>
        /// List of IToy objects describing the toys in the cabinet.
        /// </summary>
        [XmlElementAttribute(Order = 3)]
        public DirectOutput.Cab.Toys.ToyList Toys
        {
            get { return _Toys; }
            set { _Toys = value; }

        }


        private EffectList _Effects = new EffectList();

        /// <summary>
        /// List of cabinet specific effects.
        /// </summary>
        [XmlElementAttribute(Order = 4)]
        public EffectList Effects
        {
            get { return _Effects; }
            set { _Effects = value; }
        }



        private Toys.ColorList _Colors;

        /// <summary>
        /// List of Color objects used to set colors for toys. 
        /// </summary>
        [XmlElementAttribute(Order = 5)]
        public Toys.ColorList Colors
        {
            get { return _Colors; }
            set { _Colors = value; }
        }


        private CabinetOutputList _Outputs;
        /// <summary>
        /// List of IOutput objects representing all outputs of all all output controllers in the cabinet.
        /// </summary>
        [XmlIgnoreAttribute]
        public CabinetOutputList Outputs
        {
            get
            {
                return _Outputs;

            }
        }

        private Out.OutputControllerList _OutputControllers;

        /// <summary>
        /// List of IOutputController objects representing the output controllers in the cabinet.
        /// </summary>
        [XmlElementAttribute(Order = 2)]
        public Out.OutputControllerList OutputControllers
        {
            get { return _OutputControllers; }
            set { _OutputControllers = value; }
        }
        #endregion

        #region Serialization

        /// <summary>
        /// Returns a serialized XML representation of the cabinet configuration.
        /// </summary>
        /// <returns>XMLString</returns>
        public string GetConfigXml()
        {
            string Xml = "";
            using (MemoryStream ms = new MemoryStream())
            {
                new XmlSerializer(typeof(Cabinet)).Serialize(ms, this);
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
        /// Serializes the cabinet configuration to a XML file.
        /// </summary>
        /// <param name="FileName">Name of the XML file.</param>
        public void SaveConfigXmlFile(string FileName)
        {
            GetConfigXml().WriteToFile(FileName);
        }


        /// <summary>
        /// Instanciates a Cabinet object from a cabinet configuration in a XML file.
        /// </summary>
        /// <param name="FileName">Name of the XML file.</param>
        /// <returns>Cabinet object</returns>
        public static Cabinet GetCabinetFromConfigXmlFile(string FileName)
        {
            string Xml;
            try
            {
                Xml = General.FileReader.ReadFileToString(FileName);
            }
            catch (Exception E)
            {
                Log.Exception("Could not load cabinet config from {0}.".Build(FileName), E);
                throw new Exception("Could not read cabinet config file {0}.".Build(FileName), E);
            }

            return GetCabinetFromConfigXml(Xml);
        }

        /// <summary>
        /// Instanciates a Cabinet object from a cabinet configuration in a XML file.
        /// </summary>
        /// <param name="CabinetConfigFile">FileInfo object for the config file.</param>
        /// <returns>Cabinet object</returns>
        public static Cabinet GetCabinetFromConfigXmlFile(FileInfo CabinetConfigFile)
        {
            return GetCabinetFromConfigXmlFile(CabinetConfigFile.FullName);
        }

        /// <summary>
        /// Instanciates a Cabinet object from a cabinet configuration in a XML string.
        /// </summary>
        /// <param name="ConfigXml">XML string</param>
        /// <returns>Cabinet object</returns>
        public static Cabinet GetCabinetFromConfigXml(string ConfigXml)
        {
            byte[] xmlBytes = Encoding.Default.GetBytes(ConfigXml);
            using (MemoryStream ms = new MemoryStream(xmlBytes))
            {
                try
                {
                    return (Cabinet)new XmlSerializer(typeof(Cabinet)).Deserialize(ms);
                }
                catch (Exception E)
                {
                    
                    Exception Ex = new Exception("Could not deserialize the cabinet config from XML data.", E);
                    Ex.Data.Add("XML Data", ConfigXml);
                    Log.Exception("Could not load cabinet config from XML data.", Ex);
                    throw Ex;
                }
            }
        }
        #endregion


        /// <summary>
        /// Initializes the cabinet.
        /// </summary>
        public void Init(Pinball Pinball)
        {
            Log.Write("Initializing cabinet");
            OutputControllers.Init();
            Toys.Init(Pinball);
            Effects.Init(Pinball);
            Log.Write("Cabinet initialized");
        }




        /// <summary>
        /// Finishes the cabinet
        /// </summary>
        public void Finish()
        {
            Log.Write("Finishing cabinet");
            Toys.Finish();
            OutputControllers.Finish();
            Log.Write("Cabinet finished");
        }



        #region Constructor
        public Cabinet()
        {
            _OutputControllers = new Out.OutputControllerList();
            _Outputs = new CabinetOutputList(this);
            _Toys = new Toys.ToyList();
            _Effects = new EffectList();
            _Colors = new Toys.ColorList();
        }
        #endregion

    }
}
