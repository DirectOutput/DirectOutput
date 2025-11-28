using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.FX;

using DirectOutput.General.BitmapHandling;
using DirectOutput.FX.MatrixFX.BitmapShapes;


// <summary>
// The Table namespace contains all table specific classes like the Table class itself, TableElement and effect assigment classes. 
// </summary>
namespace DirectOutput.Table
{

    /// <summary>
    /// Holds all table specific information and handles all TableElements
    /// </summary>
    public class Table
    {
        #region Properties
        /// <summary>
        /// Lists the TableElement objects for the Table.<br/>
        /// This list is automaticaly extend with new TableElement objects if updates for non existing elements are received.
        /// </summary>
        public TableElementList TableElements { get; set; }

        /// <summary>
        /// Gets the pinball object to which the Table object belongs.
        /// </summary>
        /// <value>
        /// The pinball object to which the Table object belongs.
        /// </value>
        [XmlIgnoreAttribute]
        public Pinball Pinball { get; private set; }


        private FastImageList _Bitmaps = new FastImageList();

        /// <summary>
        /// Gets or sets the list of bitmaps
        /// </summary>
        /// <value>
        /// The bitmaps list.
        /// </value>
        [XmlIgnoreAttribute]
        public FastImageList Bitmaps
        {
            get { return _Bitmaps; }
            private set { _Bitmaps = value; }
        }

        private ShapeDefinitions _ShapeDefinitions;

        /// <summary>
        /// Gets or sets the shape definitions.
        /// </summary>
        /// <value>
        /// The shape definitions.
        /// </value>
        [XmlIgnore]
        public ShapeDefinitions ShapeDefinitions
        {
            get { return _ShapeDefinitions; }
            set { _ShapeDefinitions = value; }
        }







        #region TableName
        private string _TableName;
        /// <summary>
        /// Name of the Table.<br/>
        /// Triggers TableNameChanged if value is changed.
        /// </summary>    
        public string TableName
        {
            get { return _TableName; }
            set
            {
                if (_TableName != value)
                {
                    _TableName = value;
                    if (TableNameChanged != null)
                    {
                        TableNameChanged(this, new EventArgs());
                    }
                }
            }
        }
        /// <summary>
        /// Event is fired if the value of the property TableName is changed.
        /// </summary>
        public event EventHandler<EventArgs> TableNameChanged;
        #endregion

        #region RomName
        private string _RomName;
        /// <summary>
        /// Name of the table rom.<br/>
        /// Triggers RomNameChanged if value is changed.
        /// </summary>    
        [XmlIgnoreAttribute]
        public string RomName
        {
            get { return _RomName; }
            set
            {
                if (_RomName != value)
                {
                    _RomName = value;
                    if (RomNameChanged != null)
                    {
                        RomNameChanged(this, new EventArgs());
                    }
                }
            }
        }
        /// <summary>
        /// Event is fired if the value of the property RomName is changed.
        /// </summary>
        public event EventHandler<EventArgs> RomNameChanged;
        #endregion

        /// <summary>
        /// Gets or sets the filename of the table.
        /// </summary>
        /// <value>
        /// The filename of the table.
        /// </value>
        [XmlIgnoreAttribute]
        public string TableFilename { get; set; }

        /// <summary>
        /// Gets or sets the table configuration filename.
        /// </summary>
        /// <value>
        /// The table configuration filename.
        /// </value>
        [XmlIgnoreAttribute]
        public string TableConfigurationFilename { get; set; }


        private bool _AddLedControlConfig = false;

        /// <summary>
        /// Gets or sets a value indicating whether configurations from ledcontrol files should be added to the table config from a xml table config file.
        /// </summary>
        /// <value>
        /// <c>true</c> will add ledcontrol configs, <c>false</c> (default) will ingnore ledcontrol configs.
        /// </value>
        public bool AddLedControlConfig
        {
            get { return _AddLedControlConfig; }
            set { _AddLedControlConfig = value; }
        }


        private TableConfigSourceEnum _ConfigurationSource = TableConfigSourceEnum.Unknown;

        /// <summary>
        /// Gets or sets the configuration source.
        /// </summary>
        /// <value>
        /// The configuration source.
        /// </value>
        [XmlIgnoreAttribute]
        public TableConfigSourceEnum ConfigurationSource
        {
            get { return _ConfigurationSource; }
            set { _ConfigurationSource = value; }
        }




        private EffectList _Effects;
        /// <summary>
        /// List of table specific effects.
        /// </summary>
        public EffectList Effects
        {
            get { return _Effects; }
            set { _Effects = value; }
        }

        private AssignedEffectList _AssignedStaticEffects;
        /// <summary>
        /// Gets or sets the static effects list for the table.<br/>
        /// AssignedEffects contained in AssignedStaticEffects are triggered when the Table is started. The Trigger method is called with null as the TableElement parameter.<br/>
        /// </summary>
        /// <value>
        /// The static effects list.
        /// </value>
        public AssignedEffectList AssignedStaticEffects
        {
            get
            {
                return _AssignedStaticEffects;
            }
            set
            {
                _AssignedStaticEffects = value;
            }
        }

        #endregion
        /// <summary>
        /// Updates the TableElements list with data received from Pinmame.
        /// </summary>
        /// <param name="Data">Data received from Pinmame and handled by the PinMameInputManger</param>
        public void UpdateTableElement(TableElementData Data)
        {

            TableElements.UpdateState(Data);
        }


        /// <summary>
        /// Triggers the static effects for the table.
        /// </summary>
        public void TriggerStaticEffects()
        {
            AssignedStaticEffects.Trigger(new TableElementData(TableElementTypeEnum.Unknown, 0, 1));
        }

        /// <summary>
        /// Initializes the table and the contained objects(Effects, TableElements).
        /// </summary>
        /// <param name="Pinball">The Pinball object containing the Table.</param>
        public void Init(Pinball Pinball)
        {
            this.Pinball = Pinball;
            Bitmaps.UseCache = this.Pinball.GlobalConfig.FastInitialization;

            FileInfo ShapeDefinitionFile = Pinball.GlobalConfig.GetShapeDefinitionFile();
            if (ShapeDefinitionFile != null && ShapeDefinitionFile.Exists)
            {
                Log.Write("Loading shape definition file: {0}".Build(ShapeDefinitionFile.FullName));
                try
                {
                    ShapeDefinitions = ShapeDefinitions.GetShapeDefinitionsFromShapeDefinitionsXmlFile(ShapeDefinitionFile);
                }
                catch (Exception E)
                {
                    Log.Exception("Loading shape definition file {0} failed.".Build(ShapeDefinitionFile.FullName), E);
                }
                ShapeDefinitions.BitmapFilePattern = new General.FilePattern(ShapeDefinitionFile.FullName.Substring(0, ShapeDefinitionFile.FullName.Length - ShapeDefinitionFile.Extension.Length) + ".png");
            }
            else
            {
                if (ShapeDefinitionFile == null)
                {
                    Log.Warning("Could not determine name of shape definition file");
                }
                else
                {
                    Log.Warning("Shape definition file {0} does not exist");
                }
                ShapeDefinitions = new ShapeDefinitions();
            }

            Effects.Init(this);

            TableElements.InitAssignedEffects(this);
            AssignedStaticEffects.Init(this);
        }

        /// <summary>
        /// Finishes the table and the contained objects (Effects, TableElements)
        /// </summary>
        public void Finish()
        {
            AssignedStaticEffects.Finish();
            TableElements.FinishAssignedEffects();
            Effects.Finish();
            Pinball = null;
        }


        #region Serialization

        /// <summary>
        /// Returns a serialized XML representation of the Table configuration.
        /// </summary>
        /// <returns>XMLString</returns>
        public string GetConfigXml()
        {
            string Xml = "";
            using (MemoryStream ms = new MemoryStream())
            {
                new XmlSerializer(typeof(Table)).Serialize(ms, this);
                ms.Position = 0;

                using (StreamReader sr = new StreamReader(ms, Encoding.Default))
                {
                    Xml = sr.ReadToEnd();

                }

            }
            return Xml;
        }


        /// <summary>
        /// Serializes the Table configuration to a XML file.
        /// </summary>
        /// <param name="FileName">Name of the XML file.</param>
        public void SaveConfigXmlFile(string FileName)
        {
            GetConfigXml().WriteToFile(FileName);
        }


        /// <summary>
        /// Instanciates a Table object from a Table configuration in a XML file.
        /// </summary>
        /// <param name="FileName">Name of the XML file.</param>
        /// <returns>Table object</returns>
        public static Table GetTableFromConfigXmlFile(string FileName)
        {
            string Xml;

            try
            {
                Xml = General.FileReader.ReadFileToString(FileName);



            }
            catch (Exception E)
            {
                Log.Exception("Could not read Table Config file {0}".Build(FileName), E);
                throw new Exception("Could not read Table Config file {0}".Build(FileName), E);
            }
            return GetTableFromConfigXml(Xml);
        }

        /// <summary>
        /// Instanciates a Table object from a Table configuration in a XML file.
        /// </summary>
        /// <param name="TableConfigFile">FileInfo object for the config file.</param>
        /// <returns>Table object</returns>
        public static Table GetTableFromConfigXmlFile(FileInfo TableConfigFile)
        {
            return GetTableFromConfigXmlFile(TableConfigFile.FullName);
        }

        /// <summary>
        /// Instanciates a Table object from a Table configuration in a XML string.
        /// </summary>
        /// <param name="ConfigXml">XML string</param>
        /// <returns>Table object</returns>
        public static Table GetTableFromConfigXml(string ConfigXml)
        {

            try
            {
                byte[] xmlBytes = Encoding.Default.GetBytes(ConfigXml);
                using (MemoryStream ms = new MemoryStream(xmlBytes))
                {
                    Table T = (Table)new XmlSerializer(typeof(Table)).Deserialize(ms);
                    T.ConfigurationSource = TableConfigSourceEnum.TableConfigurationFile;
                    return T;
                }
            }
            catch (Exception E)
            {

                Exception Ex = new Exception("Could not deserialize Table config from XML Data", E);
                Ex.Data.Add("XMLData", ConfigXml);
                Log.Exception(Ex);
                throw Ex;
            }
        }
        #endregion



        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        public Table()
        {
            Effects = new FX.EffectList();
            TableElements = new TableElementList();
            AssignedStaticEffects = new AssignedEffectList();
        }



    }
}
