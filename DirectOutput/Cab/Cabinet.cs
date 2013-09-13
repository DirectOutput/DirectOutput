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
using DirectOutput.Cab.Color;


namespace DirectOutput.Cab
{
    /// <summary>
    /// The Cabinet object describes the parts of a pinball cabinet (toys, outputcontrollers, outputs and more). 
    /// </summary>
    public class Cabinet
    {
        /// <summary>
        /// This method finds all classes implementing the IAutoConfigOutputController interface and uses the member of this interface to detect and configure IOutputController objects automatically.
        /// </summary>
        public void AutoConfig()
        {
            Log.Write("Cabinet auto configuration started");

            General.TypeList Types = new General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IAutoConfigOutputController).IsAssignableFrom(p) && !p.IsAbstract));
            foreach (Type T in Types)
            {
                IAutoConfigOutputController AutoConfig = (IAutoConfigOutputController)Activator.CreateInstance(T);
                AutoConfig.AutoConfig(this);
            }



            Log.Write("Cabinet auto configuration finished");
        }



        #region Properties
        /// <summary>
        /// Gets the Pinball object to which the Cabinet pobject belongs.
        /// </summary>
        /// <value>
        /// The pinball object.
        /// </value>
        [XmlIgnore]
        public Pinball Pinball { get; private set; }

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






        private DirectOutput.Cab.Color.ColorList _Colors;

        /// <summary>
        /// List of Color objects used to set colors for toys. 
        /// </summary>
        [XmlElementAttribute(Order = 4)]
        public DirectOutput.Cab.Color.ColorList Colors
        {
            get { return _Colors; }
            set { _Colors = value; }
        }


        private bool _AutoConfigEnabled=true;
        /// <summary>
        /// Gets or sets a value indicating whether auto config is enabled.<br/>
        /// If auto config is enabled, the framework tries to detect and configure IOutputController objects and related IToy objects automatically.
        /// </summary>
        /// <value>
        ///   <c>true</c> enables auto config, <c>false</c> disables auto config.
        /// </value>
        [XmlElementAttribute(Order = 5)]
        public bool AutoConfigEnabled
        {
            get { return _AutoConfigEnabled; }
            set { _AutoConfigEnabled = value; }
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
        /// Tests a cabinet config in a XML file.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <returns>true is the file contains a valid config, otherwise false.</returns>
        public static bool TestCabinetConfigXmlFile(string FileName)
        {
            Cabinet C = null;
            try
            {
                C = GetCabinetFromConfigXmlFile(FileName);
            }
            catch
            {
                return false;
            }
            return C != null;

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
        /// <param name="Pinball">The Pinball object using the Cabinet instance.</param>
        public void Init(Pinball Pinball)
        {
            Log.Write("Initializing cabinet");
            this.Pinball = Pinball;
            OutputControllers.Init(this);
            Toys.Init(this);
    
            Log.Write("Cabinet initialized");
        }


        /// <summary>
        /// Calls the update method for toys and output controllers in the cabinet
        /// </summary>
        public void Update()
        {
            Toys.UpdateOutputs();
            OutputControllers.Update();
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
        /// <summary>
        /// Initializes a new instance of the <see cref="Cabinet"/> class.
        /// </summary>
        public Cabinet()
        {
            _OutputControllers = new Out.OutputControllerList();
            _Outputs = new CabinetOutputList(this);
            _Toys = new Toys.ToyList();
            _Colors = new ColorList();
        }
        #endregion

    }
}
