using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using DirectOutput.Cab;
using DirectOutput.FX;
using DirectOutput.General;
using DirectOutput.GlobalConfiguration;
using DirectOutput.LedControl;
using DirectOutput.PinballSupport;
using DirectOutput.Scripting;
using DirectOutput.Table;
using DirectOutput.General.Statistics;

namespace DirectOutput
{
    /// <summary>
    /// Pinball is the main object of the DirectOutput framework.<br/>
    /// It holds all objects required to process Pinmame data, trigger the necessary effects and update toys and output controllers.
    /// </summary>
    public class Pinball
    {

        #region Properties


        public ThreadInfoList ThreadInfoList { get; private set; }
        public TimeSpanStatisticsList TimeSpanStatistics { get; private set; }


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
        /// <param name="GlobalConfigFilename">The global config filename.</param>
        /// <param name="TableFilename">The table filename.</param>
        /// <param name="RomName">Name of the rom.</param>
        public void Init(string GlobalConfigFilename = "", string TableFilename = "", string RomName = "")
        {
            bool GlobalConfigLoaded = true;
            //Load the global config

            if (!GlobalConfigFilename.IsNullOrWhiteSpace())
            {
                FileInfo GlobalConfigFile = new FileInfo(GlobalConfigFilename);


                GlobalConfig = GlobalConfig.GetGlobalConfigFromConfigXmlFile(GlobalConfigFile.FullName);
                if (GlobalConfig == null)
                {
                    GlobalConfigLoaded = false;

                    //set new global config object if it config could not be loaded from the file.
                    GlobalConfig = new GlobalConfig();
                }
                GlobalConfig.GlobalConfigFilename = GlobalConfigFile.FullName;
            }
            else
            {
                GlobalConfig = new GlobalConfig();
                GlobalConfig.GlobalConfigFilename = GlobalConfigFilename;
            }


            if (GlobalConfig.EnableLogging)
            {
                Log.Filename = GlobalConfig.GetLogFilename((!TableFilename.IsNullOrWhiteSpace() ? new FileInfo(TableFilename).FullName : ""), RomName);
                Log.Init();
            }

            if (GlobalConfigLoaded)
            {
                Log.Write("Global config loaded from: ".Build(GlobalConfigFilename));
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
            if (!TableFilename.IsNullOrWhiteSpace())
            {
                Scripts.LoadAndAddScripts(GlobalConfig.GetTableScriptFiles(new FileInfo(TableFilename).FullName));
            }
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
            Table.AddLedControlConfig = true;

            if (!TableFilename.IsNullOrWhiteSpace())
            {
                FileInfo TableFile = new FileInfo(TableFilename);
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
                    if (Table.AddLedControlConfig)
                    {
                        Log.Write("Table config allows mix with ledcontrol configs.");
                    }
                }
                else
                {
                    Log.Warning("No table config file found. Will try to load config from LedControl file(s).");
                }
            }
            else
            {
                Log.Write("No TableFilename specified, will use empty tableconfig");
            }
            if (Table.AddLedControlConfig)
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
                    else
                    {
                        bool FoundIt = false;
                        List<string> LookupPaths = new List<string>();
                        if (!TableFilename.IsNullOrWhiteSpace())
                        {
                            LookupPaths.Add(new FileInfo(TableFilename).Directory.FullName);
                        }
                        LookupPaths.AddRange(new string[] { GlobalConfig.GetGlobalConfigDirectory().FullName, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) });

                        LedControlIniFileList LedControlIniFiles = new LedControlIniFileList();

                        foreach (string P in LookupPaths)
                        {

                            if (File.Exists(Path.Combine(P, "ledcontrol.ini")))
                            {
                                Log.Write("Found ledcontrol.ini in {0} Will try to load config data.".Build(P));



                                LedControlIniFiles.Add(new LedControlIniFile(Path.Combine(P, "ledcontrol.ini"), 1));

                                for (int LedWizNr = 2; LedWizNr <= 16; LedWizNr++)
                                {
                                    if (File.Exists(Path.Combine(P, "ledcontrol{0}.ini").Build(LedWizNr)))
                                    {
                                        LedControlIniFiles.Add(new LedControlIniFile(Path.Combine(P, "ledcontrol{0}.ini").Build(LedWizNr), LedWizNr));
                                    }
                                    else if (File.Exists(Path.Combine(P, "ledcontrol{0:00}.ini").Build(LedWizNr)))
                                    {
                                        LedControlIniFiles.Add(new LedControlIniFile(Path.Combine(P, "ledcontrol{0:00}.ini").Build(LedWizNr), LedWizNr));
                                    }

                                    FoundIt = true;
                                }
                                break;

                            }
                        }
                        if (FoundIt)
                        {
                            L.LoadLedControlFiles(LedControlIniFiles, false);
                            Log.Write("{0} ledcontrol files loaded.".Build(LedControlIniFiles.Count));
                        }
                        else
                        {
                            Log.Write("No ledcontrol files found. No ledcontrol configs will be loaded.");
                        }
                    }
                    if (!L.ContainsConfig(RomName))
                    {
                        Log.Write("No config for table found in LedControl data for RomName {0}.".Build(RomName));
                    }
                    else
                    {
                        Log.Write("Config for RomName {0} exists in LedControl data. Updating table config.".Build(RomName));
                        L.UpdateTableConfig(Table, RomName, Cabinet);
                    }
                }
                else
                {

                    Log.Write("Cant load config from LedControl file(s) since no RomName was supplied. No ledcontrol config will be loaded.");
                }

            }
            if (Table.TableName.IsNullOrWhiteSpace())
            {
                if (!TableFilename.IsNullOrWhiteSpace())
                {
                    Table.TableName = Path.GetFileNameWithoutExtension(new FileInfo(TableFilename).FullName);
                }
                else if (!RomName.IsNullOrWhiteSpace())
                {
                    Table.TableName = RomName;
                }
            }
            if (!TableFilename.IsNullOrWhiteSpace())
            {
                Table.TableFilename = new FileInfo(TableFilename).FullName;
            }
            if (!RomName.IsNullOrWhiteSpace())
            {
                Table.RomName = RomName;
            }
            Log.Write("Table config loading finished");

            Effects = new CombinedEffectList(new EffectList[] { Table.Effects, Cabinet.Effects });

            Log.Write("Pinball parts loaded");

            Log.Write("Starting processes");
            InitStatistics();
            Cabinet.Init(this);
            Table.Init(this);
            Alarms.Init(this);
            Table.TriggerStaticEffects();
            Cabinet.OutputControllers.Update();

            //Add the thread initializing the framework to the threadinfo list
            ThreadInfoList.HeartBeat("External caller");

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

            WriteStatisticsToLog();

            ThreadInfoList.ThreadTerminates();

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

            ThreadInfoList.HeartBeat("DirectOutput");
            try
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
                            DateTime StartTime = DateTime.Now;
                            Table.UpdateTableElement(D);
                            UpdateRequired |= true;
                            UpdateTableElementStatistics(D, (StartTime - DateTime.Now));
                        }
                        catch (Exception E)
                        {
                            Log.Exception("A unhandled exception occured while processing data for table element {0} {1} with value {2}".Build(D.TableElementType, D.Number, D.Value), E);
                            ThreadInfoList.RecordException(E);

                        }
                    }

                    if (KeepMainThreadAlive)
                    {
                        try
                        {
                            //Executed all alarms which have been scheduled for the current time
                            UpdateRequired |= Alarms.ExecuteAlarms(DateTime.Now.AddMilliseconds(1));
                        }
                        catch (Exception E)
                        {
                            Log.Exception("A unhandled exception occured while executing timer events.", E);
                            ThreadInfoList.RecordException(E);
                        }
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
                            ThreadInfoList.RecordException(E);
                        }
                    }

                    if (KeepMainThreadAlive)
                    {
                        ThreadInfoList.HeartBeat();
                        //Sleep until we get more input data and/or a timer expires.
                        DateTime NextAlarm = Alarms.GetNextAlarmTime();

                        lock (MainThreadLocker)
                        {
                            while (InputQueue.Count == 0 && NextAlarm > DateTime.Now && KeepMainThreadAlive)
                            {
                                int TimeOut = ((int)(NextAlarm - DateTime.Now).TotalMilliseconds).Limit(1, 50);

                                Monitor.Wait(MainThreadLocker, TimeOut);  // Lock is released while we’re waiting
                                ThreadInfoList.HeartBeat();
                            }
                        }
                    }


                }
            }
            catch (Exception E)
            {
                Log.Exception("A unexpected exception occured in the DirectOutput MainThread", E);
                ThreadInfoList.RecordException(E);
            }

            ThreadInfoList.ThreadTerminates();
        }
        #endregion


        private Dictionary<TableElementTypeEnum, TimeSpanStatisticsItem> TableElementCallStatistics = new Dictionary<TableElementTypeEnum, TimeSpanStatisticsItem>();


        private void InitStatistics()
        {
            TimeSpanStatisticsItem TSI;

            TableElementCallStatistics = new Dictionary<TableElementTypeEnum, TimeSpanStatisticsItem>();
            foreach (TableElementTypeEnum T in Enum.GetValues(typeof(TableElementTypeEnum)))
            {
                 TSI=new TimeSpanStatisticsItem() {Name="{0} calls".Build(T.ToString()),GroupName="Pinball - Table element update calls"};
                TableElementCallStatistics.Add(T, TSI);
                TimeSpanStatistics.Add(TSI);
            }

         
        }

        public void UpdateTableElementStatistics(TableElementData D, TimeSpan Duration)
        {
            try
            {
                TableElementCallStatistics[D.TableElementType].AddDuration(Duration);
            }
            catch (Exception E)
            {
                Log.Exception("Could not update TimeSpanStatistics for Pinball table element type {0} ({1})".Build(D.ToString(), D), E);
            }
        }


        public void WriteStatisticsToLog()
        {
            Log.Write("Duration statistics:");

            TimeSpanStatistics.Sort();
            string CurrentGroupName = "";
            foreach (TimeSpanStatisticsItem TSI in TimeSpanStatistics)
            {
                if (CurrentGroupName != TSI.GroupName)
                {
                    Log.Write("  {0}".Build(TSI.GroupName));
                }
                Log.Write("    - {0}".Build(TSI.Name));
            }


            foreach (KeyValuePair<TableElementTypeEnum, TimeSpanStatisticsItem> KV in TableElementCallStatistics)
            {
                Log.Write("  {0}, {1}".Build(KV.Key.ToString(), KV.Value.ToString()));
            }
        }

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
            ThreadInfoList.HeartBeat("Data delivery");
        }

        /// <summary>
        /// Receives the table element data from the calling app.<br />
        /// The received data is put in a queue and the internal thread of the framework is notified about the availability of new data.
        /// </summary>
        /// <param name="TableElementData">The table element data to be received.</param>
        public void ReceiveData(TableElementData TableElementData)
        {
            InputQueue.Enqueue(TableElementData);
            MainThreadSignal();
            ThreadInfoList.HeartBeat("Data delivery");
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

            ThreadInfoList = new ThreadInfoList();
            TimeSpanStatistics = new TimeSpanStatisticsList();

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Pinball" /> class and calls the Init method with the specified parameters.
        /// </summary>
        /// <param name="GlobalConfigFilename">The global config filename.</param>
        /// <param name="TableFilename">The table filename.</param>
        /// <param name="RomName">Name of the rom.</param>
        public Pinball(string GlobalConfigFilename = "", string TableFilename = "", string RomName = "")
            : this()
        {
            Init(GlobalConfigFilename, TableFilename, RomName);
        }
        #endregion


    }
}
