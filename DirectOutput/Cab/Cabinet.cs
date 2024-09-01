using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.Cab.Out;
using DirectOutput.General;
using DirectOutput.General.Color;
using DirectOutput.PinballSupport;
using DirectOutput.Cab.Schedules;
using DirectOutput.Cab.Sequencer;
using DirectOutput.Cab.Overrides;

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

                try
                {
                    IAutoConfigOutputController AutoConfig = (IAutoConfigOutputController)Activator.CreateInstance(T);
                    AutoConfig.AutoConfig(this);

                }
                catch (Exception E)
                {
                    Log.Exception("A exception occurred during auto configuration for output controller(s) of type {0}.".Build(T.Name), E);                    
                }
            }



            Log.Write("Cabinet auto configuration finished");
        }



        #region Properties
        ///// <summary>
        ///// Gets the Pinball object to which the Cabinet pobject belongs.
        ///// </summary>
        ///// <value>
        ///// The pinball object.
        ///// </value>
        //[XmlIgnore]
        //public Pinball Pinball { get; private set; }


        /// <summary>
        /// Gets or sets the owner or the cabinet.
        /// </summary>
        /// <value>
        /// The owner of the cabinet.
        /// </value>
        [XmlIgnore]
        public ICabinetOwner Owner { get; set; }


        

        /// <summary>
        /// Gets the AlarmHandler object for the cabinet object.
        /// </summary>
        /// <value>
        /// The AlarmHandler object for the cabinet object.
        /// </value>
        [XmlIgnore]
        public AlarmHandler Alarms
        {
            get { return Owner.Alarms; }
        }


        /// <summary>
        /// Name of the Cabinet.
        /// </summary>

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

        public DirectOutput.Cab.Toys.ToyList Toys
        {
            get { return _Toys; }
            set { _Toys = value; }

        }






        private ColorList _Colors = new ColorList();

        /// <summary>
        /// List of Color objects used to set colors for toys. 
        /// </summary>

        public ColorList Colors
        {
            get { return _Colors; }
            set { _Colors = value; }
        }

        private CurveList _Curves=new CurveList();

        /// <summary>
        /// List of named curve objects used to set Curves for toys. 
        /// </summary>

        public CurveList Curves
        {
            get { return _Curves; }
            set { _Curves = value; }
        }

        private bool _AutoConfigEnabled=true;
        /// <summary>
        /// Gets or sets a value indicating whether auto config is enabled.<br/>
        /// If auto config is enabled, the framework tries to detect and configure IOutputController objects and related IToy objects automatically.
        /// </summary>
        /// <value>
        ///   <c>true</c> enables auto config, <c>false</c> disables auto config.
        /// </value>

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

        public Out.OutputControllerList OutputControllers
        {
            get { return _OutputControllers; }
            set { _OutputControllers = value; }
        }

        /// <summary>
        /// List of scheduled settings. Outputs can be disabled using start-end clock regions.
        /// This getter/setter is only used for XML-parsing into ScheduledSettings class, and settings can be accessed using ScheduledSettings.Instance from that point on.
        /// </summary>
        public ScheduledSettings ScheduledSettings {
            get { return ScheduledSettings.Instance; }
            set { ScheduledSettings.Instance = value; }
        }

        /// <summary>
        /// List of sequencial output settings. Outputs can be forwarded to others during fast retriggers to compensate for latency.
        /// This getter/setter is only used for XML-parsing into SequentialOutputSettings class, and settings can be accessed using SequentialOutputSettings.Instance from that point on.
        /// </summary>
        public SequentialOutputSettings SequentialOutputSettings {
            get { return SequentialOutputSettings.Instance; }
            set { SequentialOutputSettings.Instance = value; }
        }

        /// <summary>
        /// List of table overrides. Outputs can be disabled using table filenames or rom names.
        /// This getter/setter is only used for XML-parsing into TableOverrideSettings class, and settings can be accessed using TableOverrideSettings.Instance from that point on.
        /// </summary>
        public TableOverrideSettings TableOverrideSettings {
            get { return TableOverrideSettings.Instance; }
            set { TableOverrideSettings.Instance = value; }
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

                // For debugging only: copy the contents of the cabinet.xml file to the log, to help diagnose file sourcing issues
                // Log.Write("Read cabinet definition from \"" + FileName + "\"; file contents follow:\n====\n" + Xml + "\n====\n\n");
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
        /// <param name="CabinetOwner">The ICabinetOwner object for the cabinet instance.</param>
        /// </summary>
        public void Init(ICabinetOwner CabinetOwner)
        {
            Log.Write("Initializing cabinet");
            this.Owner = CabinetOwner;
            OutputControllers.Init(this);
            Toys.Init(this);
    
            Log.Write("Cabinet initialized");
        }


        /// <summary>
        /// Calls the update method for toys and output controllers in the cabinet
        /// </summary>
        public void Update()
        {
            //Log.Write("Cabinet.Update... ");
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
