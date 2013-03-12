using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.FX;


/// <summary>
/// The Table namespace contains all table specific classes like the Table class itself, TableElement and effect assigment classes. 
/// </summary>
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
        public string TableFilename { get; set; }

        /// <summary>
        /// Gets or sets the table configuration filename.
        /// </summary>
        /// <value>
        /// The table configuration filename.
        /// </value>
        public string TableConfigurationFilename { get; set; }


        

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
        /// AssignedEffects contained in AssignedStaticEffects are triggered when the Table is started. The Trigger method is called with a TableElement containing a Swith with number -1 and a Value of -1.<br/>
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
        public void UpdateTableElement(DirectOutput.PinmameHandling.PinmameData Data)
        {
            TableElements.UpdateState(Data.TableElementType, Data.Number, Data.Value);
        }


        public void TriggerStaticEffects()
        {
            AssignedStaticEffects.Trigger(null);
        }

        /// <summary>
        /// Initializes the table and the contained objects(Effects, TableElements).
        /// </summary>
        /// <param name="Pinball">Pinball object which runs the table</param>
        public void Init(Pinball Pinball)
        {
            Effects.Init(Pinball);

            TableElements.InitAssignedEffects(Pinball);
            AssignedStaticEffects.Init(Pinball);
        }

        /// <summary>
        /// Finishes the table and the contained objects (Effects, TableElements)
        /// </summary>
        public void Finish()
        {
            AssignedStaticEffects.Finish();
            TableElements.FinishAssignedEffects();
            Effects.Finish();
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
            //TODO: Error handling einbauen

            string Xml = General.FileReader.ReadFileToString(FileName);

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
            //TODO: Error handling einbauen
            byte[] xmlBytes = Encoding.Default.GetBytes(ConfigXml);
            using (MemoryStream ms = new MemoryStream(xmlBytes))
            {
                return (Table)new XmlSerializer(typeof(Table)).Deserialize(ms);
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
