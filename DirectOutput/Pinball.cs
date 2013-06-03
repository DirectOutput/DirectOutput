using System;
using System.IO;
using System.Reflection;
using DirectOutput.Cab;
using DirectOutput.FX;
using DirectOutput.GlobalConfiguration;
using DirectOutput.LedControl;
using DirectOutput.Scripting;
using System.Threading;
using DirectOutput.Table;
using DirectOutput.PinballSupport;

namespace DirectOutput
{
    /// <summary>
    /// Pinball is the main object of the DirectOutput framework.<br/>
    /// It holds all objects required to process Pinmame data, trigger the necessary effects and update toys and output controllers.
    /// </summary>
    public class Pinball
    {

        #region Properties

        private ScriptList _Scripts = new ScriptList();
        /// <summary>
        /// Gets the list of loaded scripts.
        /// </summary>
        /// <value>
        /// The list of loaded scripts.
        /// </value>
        public ScriptList Scripts
        {
            get { return _Scripts; }
            private set { _Scripts = value; }
        }

        private CombinedEffectList _Effects = new CombinedEffectList();

        public CombinedEffectList Effects
        {
            get { return _Effects; }
            private set { _Effects = value; }
        }



        private Table.Table _Table = new Table.Table();

        /// <summary>
        /// Gets the table object for the Pinball object.
        /// </summary>
        /// <value>
        /// The table object for the Pinball object.
        /// </value>
        public Table.Table Table
        {
            get { return _Table; }
            private set { _Table = value; }
        }
        private Cabinet _Cabinet = new Cabinet();

        /// <summary>
        /// Gets the Cabinet object for the Pinball object.
        /// </summary>
        /// <value>
        /// The cabinet object for the Pinball object.
        /// </value>
        public Cabinet Cabinet
        {
            get { return _Cabinet; }
            private set { _Cabinet = value; }
        }



        private AlarmHandler _Alarms = new AlarmHandler();

        /// <summary>
        /// Gets the AlarmHandler object for the Pinball object.
        /// </summary>
        /// <value>
        /// The AlarmHandler object for the Pinball object.
        /// </value>
        public AlarmHandler Alarms
        {
            get { return _Alarms; }
            private set { _Alarms = value; }
        }

        private GlobalConfig _GlobalConfig = new GlobalConfig();

        /// <summary>
        /// Gets the global config for the Pinnball object.
        /// </summary>
        /// <value>
        /// The global config for the Pinball object.
        /// </value>
        public GlobalConfig GlobalConfig
        {
            get { return _GlobalConfig; }
            private set { _GlobalConfig = value; }
        }



        #endregion


        #region Init & Finish
        /// <summary>
        /// Configures and initializes/starts and configures the Pinball object
        /// </summary>
        /// <param name="GlobalConfigFile">The global config file.</param>
        /// <param name="TableFile">The table file.</param>
        /// <param name="RomName">Name of the rom.</param>
        public void Init(FileInfo GlobalConfigFile, FileInfo TableFile, string RomName = "")
        {
            bool GlobalConfigLoaded = true;
            //Load the global config
            GlobalConfig = GlobalConfig.GetGlobalConfigFromConfigXmlFile(GlobalConfigFile.FullName);
            if (GlobalConfig == null)
            {
                GlobalConfigLoaded = false;

                //set new global config object if it config could not be loaded from the file.
                GlobalConfig = new GlobalConfig();
            }
            GlobalConfig.GlobalConfigFilename = GlobalConfigFile.FullName;

            Log.Filename = GlobalConfig.GetLogFilename(TableFile.FullName, RomName);

            if (GlobalConfig.EnableLogging)
            {
                Log.Init();
            }
            if (GlobalConfigLoaded)
            {
                Log.Write("Global config loaded from: ".Build(GlobalConfigFile.FullName));
            }
            else
            {
                Log.Write("No GlobalConfig file loaded. Using new inanciated GlobalConfig object instead.");
            }



            Log.Write("Loading Pinball parts");



            //Load global script files
            Log.Write("Loading script files");
            Scripts.LoadAndAddScripts(GlobalConfig.GetGlobalScriptFiles());

            //Load cabinet script files
            Scripts.LoadAndAddScripts(GlobalConfig.GetCabinetScriptFiles());

            //Load table script files
            Scripts.LoadAndAddScripts(GlobalConfig.GetTableScriptFiles(TableFile.FullName));

            Log.Write("Script files loaded");


            Log.Write("Loading cabinet");
            //Load cabinet config
            Cabinet = null;
            FileInfo CCF = GlobalConfig.GetCabinetConfigFile();
            if (CCF != null)
            {
                Log.Write("Will load cabinet config file: {0}".Build(CCF.FullName));
                try
                {
                    Cabinet = Cabinet.GetCabinetFromConfigXmlFile(CCF);
                    Cabinet.CabinetConfigurationFilename = CCF.FullName;
                    Log.Write("Cabinet config loaded successfully from {0}".Build(CCF.FullName));
                }
                catch (Exception E)
                {
                    Log.Exception("A exception occured when load cabinet config file: {0}".Build(CCF.FullName), E);


                }
            }
            if (Cabinet == null)
            {
                Log.Write("No cabinet config file loaded. Will use AutoConfig.");
                //default to a new cabinet object if the config cant be loaded
                Cabinet = new Cabinet();
                Cabinet.AutoConfig();
            }
            Log.Write("Cabinet loaded");

            Log.Write("Loading table config");
            
            //Load table config
            
            Table = new DirectOutput.Table.Table();
            Table.MixWithLedControlConfig = true;

            FileInfo TCF = GlobalConfig.GetTableConfigFile(TableFile.FullName);
            if (TCF != null)
            {
                Log.Write("Will load table config from {0}".Build(TCF.FullName));
                try
                {
                    Table = DirectOutput.Table.Table.GetTableFromConfigXmlFile(GlobalConfig.GetTableConfigFile(TableFile.FullName));
                    Table.TableConfigurationFilename = GlobalConfig.GetTableConfigFile(TableFile.FullName).FullName;
                    Log.Write("Table config loaded successfully from {0}".Build(TCF.FullName));
                }
                catch (Exception E)
                {
                    Log.Exception("A exception occured when loading table config: {0}".Build(TCF.FullName), E);
                }
                if (Table.MixWithLedControlConfig)
                {
                    Log.Write("Table config allows mix with ledcontrol configs.");
                }
            }
            else
            {
                Log.Warning("No table config file found. Will try to load config from LedControl file(s).");
            }

            if (Table.MixWithLedControlConfig)
            {
                if (!RomName.IsNullOrWhiteSpace())
                {
                    Log.Write("Will try to load configs from LedControl file(s) for RomName {0}".Build(RomName));
                    //Load ledcontrol
                    LedControlConfigList L = new LedControlConfigList();
                    if (GlobalConfig.LedControlIniFiles.Count > 0)
                    {
                        Log.Write("Will try to load table config from LedControl  file(s) specified in global config.");
                        L.LoadLedControlFiles(GlobalConfig.LedControlIniFiles, false);
                    }
                    else if (File.Exists(Path.Combine(TableFile.Directory.FullName, "ledcontrol.ini")))
                    {
                        Log.Write("Will try to load table config from LedControl.ini file in the table directory {0}".Build(TableFile.Directory.FullName));
                        L.LoadLedControlFile(Path.Combine(TableFile.Directory.FullName, "ledcontrol.ini"), 1, false);
                    }
                    else if (File.Exists(Path.Combine(GlobalConfig.GetGlobalConfigDirectory().FullName, "ledcontrol.ini")))
                    {
                        Log.Write("Will try to load table config from LedControl.ini file in the global config directory {0}".Build(GlobalConfig.GetGlobalConfigDirectory().FullName));
                        L.LoadLedControlFile(Path.Combine(GlobalConfig.GetGlobalConfigDirectory().FullName, "ledcontrol.ini"), 1, false);
                    }
                    else if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ledcontrol.ini")))
                    {
                        Log.Write("Will try to load table config from LedControl.ini file in the DirectOutput directory {0}".Build(Assembly.GetExecutingAssembly().Location));
                        L.LoadLedControlFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ledcontrol.ini"), 1, false);
                    }
                    if (!L.ContainsConfig(RomName))
                    {
                        Log.Write("No config for table found in LedControl data for RomName {0}.".Build(RomName));
                    }
                    else
                    {
                        Log.Write("Config for RomName {0} exists in LedControl data. Updating table config.".Build(RomName));
                        L.UpdateTableConfig(Table,RomName, Cabinet);
                    }
                }
                else
                {
                    
                    Log.Write("Cant load config from LedControl file(s) since no RomName was supplied. No ledcontrol config will be loaded.");
                }
                
            }
            if (Table.TableFilename.IsNullOrWhiteSpace())
            {
                Table.TableName = Path.GetFileNameWithoutExtension(TableFile.FullName);
            }
            Table.TableFilename = TableFile.FullName;
            Table.RomName = RomName;

            Log.Write("Table config loading finished");

            Effects = new CombinedEffectList(new EffectList[] { Table.Effects, Cabinet.Effects });

            Log.Write("Pinball parts loaded");

            Log.Write("Initializing framework");
            Cabinet.Init(this);
            Table.Init(this);
            Alarms.Init();
            Table.TriggerStaticEffects();
            Cabinet.OutputControllers.Update();
            InitMainThread();
            Log.Write("Framework initialized.");
            Log.Write("Have fun! :)");

        }

        /// <summary>
        /// Finishes the Pinball object.
        /// </summary>
        public void Finish()
        {
            Log.Write("Finishing framework");
            FinishMainThread();

            Alarms.Finish();
            Table.Effects.Finish();
            Cabinet.Effects.Finish();
            Cabinet.Toys.Finish();
            Cabinet.OutputControllers.Finish();
            Log.Write("DirectOutput framework finished.");
            Log.Write("Bye and thanks for using!");
        }

        #endregion



        #region MainThread
        /// <summary>
        /// Inits the main thread.
        /// </summary>
        /// <exception cref="System.Exception">DirectOutput MainThread could not start.</exception>
        private void InitMainThread()
        {

            if (!MainThreadIsActive)
            {
                KeepMainThreadAlive = true;
                try
                {
                    MainThread = new Thread(MainThreadDoIt);
                    MainThread.Name = "DirectOutput MainThread ";
                    MainThread.Start();
                }
                catch (Exception E)
                {
                    Log.Exception("DirectOutput MainThread could not start.", E);
                    throw new Exception("DirectOutput MainThread could not start.", E);
                }
            }
        }

        /// <summary>
        /// Finishes the main thread.
        /// </summary>
        /// <exception cref="System.Exception">A error occured during termination of DirectOutput MainThread</exception>
        private void FinishMainThread()
        {
            if (MainThread != null)
            {
                try
                {
                    KeepMainThreadAlive = false;
                    lock (MainThreadLocker)
                    {
                        Monitor.Pulse(MainThreadLocker);
                    }
                    if (!MainThread.Join(1000))
                    {
                        MainThread.Abort();
                    }
                    MainThread = null;
                }
                catch (Exception E)
                {
                    Log.Exception("A error occured during termination of DirectOutput MainThread", E);
                    throw new Exception("A error occured during termination of DirectOutput MainThread", E);
                }
            }
        }


        /// <summary>
        /// Indicates if the MainThread of DirectOutput is active
        /// </summary>
        public bool MainThreadIsActive
        {
            get
            {
                if (MainThread != null)
                {
                    if (MainThread.IsAlive)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Signals the main thread to continue its work (if currently sleeping).
        /// </summary>
        private void MainThreadSignal()
        {
            lock (MainThreadLocker)
            {
                Monitor.Pulse(MainThreadLocker);
            }
        }


        private Thread MainThread { get; set; }
        private object MainThreadLocker = new object();
        private bool KeepMainThreadAlive = true;

        //TODO: Maybe this should be a config option
        const int MaxInputDataProcessingTimeMs = 10;


        /// <summary>
        /// This method is constantly beeing executed by the main thread of the framework.<br/>
        /// Dont call this method directly. Use the Init and FinishMainThread methods.
        /// </summary>
        //TODO: Think about implement something which does really check on value changes on tableelements or triggered effects before setting update required.
        private void MainThreadDoIt()
        {


            while (KeepMainThreadAlive)
            {
                bool UpdateRequired = false;
                DateTime Start = DateTime.Now;

                //Consume the tableelement data delivered from the calling application
                while (InputQueue.Count > 0 && (DateTime.Now - Start).Milliseconds <= MaxInputDataProcessingTimeMs && KeepMainThreadAlive)
                {
                    TableElementData D;

                    D = InputQueue.Dequeue();
                    try
                    {
                        Table.UpdateTableElement(D);
                        UpdateRequired |= true;
                    }
                    catch (Exception E)
                    {
                        Log.Exception("A unhandled exception occured while processing data for table element {0} {1} with value {2}".Build(D.TableElementType, D.Number, D.Value), E);
                    }
                }

                if (KeepMainThreadAlive)
                {
                   //Executed all alarms which have been scheduled for the current time
                    UpdateRequired|=Alarms.ExecuteAlarms(DateTime.Now.AddMilliseconds(1));
                }           


                //Call update on output controllers if necessary
                if (UpdateRequired && KeepMainThreadAlive)
                {
                    try
                    {
                        Cabinet.OutputControllers.Update();
                    }
                    catch (Exception E)
                    {
                         Log.Exception("A unhandled exception occured while updating the output controllers", E);
                        
                    }
                }

                if (KeepMainThreadAlive)
                {
                    //Sleep until we get more input data and/or a timer expires.
                    DateTime NextAlarm = Alarms.GetNextAlarmTime();

                    lock (MainThreadLocker)
                    {
                        while (InputQueue.Count == 0 && NextAlarm>DateTime.Now && KeepMainThreadAlive)
                        {
                            int TimeOut = ((int)(NextAlarm - DateTime.Now).TotalMilliseconds).Limit(1,50);
                            
                            Monitor.Wait(MainThreadLocker, TimeOut);  // Lock is released while we’re waiting
                        }
                    }
                }


            }


        }
        #endregion

        private InputQueue InputQueue = new InputQueue();

        /// <summary>
        /// Receives the table element data from the calling app (e.g. B2S.Server providing data through the plugin interface).<br/>
        /// The received data is put in a queue and the internal thread of the framework is notified about the availability of new data.
        /// </summary>
        /// <param name="TableElementTypeChar">The table element type char as specified in the TableElementTypeEnum.</param>
        /// <param name="Number">The number of the TableElement.</param>
        /// <param name="Value">The value of the TableElement.</param>
        public void ReceiveData(char TableElementTypeChar, int Number, int Value)
        {
            InputQueue.Enqueue(TableElementTypeChar, Number, Value);
            MainThreadSignal();
        }




        #region ToString
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string S = this.GetType().FullName + " {\n";
            S += "  GlobalConfig {\n";
            S += "   Global Config filename:" + GlobalConfig.GlobalConfigFilename + "\n";
            S += "  }\n";
            S += "  Table {\n";
            S += "    Tablename: " + Table.TableName + "\n";
            S += "    Tablefileename: " + Table.TableFilename + "\n";
            S += "    RomName: " + Table.RomName + "\n";
            S += "    Table config source: " + Table.ConfigurationSource + "\n";
            S += "    Table config fileename: " + Table.TableConfigurationFilename + "\n";
            S += "    Table Elements count: " + Table.TableElements.Count + "\n";
            S += "    Table Effects count: " + Table.Effects.Count + "\n";
            S += "  }\n";
            S += "  Cabinet {\n";
            S += "     Cabinet config filename: " + Cabinet.CabinetConfigurationFilename + "\n";
            S += "     Outputcontrollers count: " + Cabinet.OutputControllers.Count + "\n";
            S += "     Output toys count: " + Cabinet.Toys.Count + "\n";
            S += "  }\n";

            S += "}\n";
            return S;

        }
        #endregion


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Pinball"/> class.
        /// </summary>
        public Pinball()
        {




        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Pinball"/> class and calls the Init method with the specified parameters.
        /// </summary>
        /// <param name="TableFile">The table file.</param>
        /// <param name="RomName">Name of the rom.</param>
        public Pinball(FileInfo GlobalConfigFile, FileInfo TableFile, string RomName = "")
            : this()
        {
            Init(GlobalConfigFile, TableFile, RomName);
        }
        #endregion


    }
}
