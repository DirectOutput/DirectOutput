using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using DirectOutput.Cab;
using DirectOutput.FX;
using DirectOutput.General;
using DirectOutput.GlobalConfiguration;
using DirectOutput.LedControl.Loader;
using DirectOutput.PinballSupport;
using System.Linq;
using DirectOutput.Table;
using System.Runtime.InteropServices;

using System.Diagnostics;
using DirectOutput.Cab.Overrides;

namespace DirectOutput
{
    /// <summary>
    /// Pinball is the main object of the DirectOutput framework.<br/>
    /// It holds all objects required to process PinMame data, trigger the necessary effects and update toys and output controllers.
    /// </summary>
    public class Pinball
    {

        #region Properties






        private Table.Table _Table = new Table.Table();

        /// <summary>
        /// Gets or sets the table object for the Pinball object.
        /// </summary>
        /// <value>
        /// The table object for the Pinball object.
        /// </value>
        public Table.Table Table
        {
            get { return _Table; }
            set { _Table = value; }
        }
        private Cabinet _Cabinet = new Cabinet();

        /// <summary>
        /// Gets or sets the Cabinet object for the Pinball object.
        /// </summary>
        /// <value>
        /// The cabinet object for the Pinball object.
        /// </value>
        public Cabinet Cabinet
        {
            get { return _Cabinet; }
            set { _Cabinet = value; }
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
        /// Gets the global config for the Pinball object.
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
        /// Configures the Pinball object.<br/>
        /// Loads the global config, table config and cabinet config
        /// </summary>
        /// <param name="GlobalConfigFilename">The global config filename.</param>
        /// <param name="TableFilename">The table filename.</param>
        /// <param name="RomName">Name of the rom.</param>
        public void Setup(string GlobalConfigFilename = "", string TableFilename = "", string RomName = "")
        {
            try
            {
                if (!GlobalConfigFilename.IsNullOrWhiteSpace())
                {
					Log.Write("Loading global configuration from {0}".Build(GlobalConfigFilename));
					FileInfo GlobalConfigFile = new FileInfo(GlobalConfigFilename);
                    GlobalConfig = GlobalConfig.GetGlobalConfigFromConfigXmlFile(GlobalConfigFile.FullName);
                    if (GlobalConfig != null)
                    {
						Log.Write("Global config successfully loaded from {0}".Build(GlobalConfigFilename));
					}
                    else
                    {
						// failed to load - use a default config
						Log.Write("No global config file loaded");
						GlobalConfig = new GlobalConfig();
                    }

                    GlobalConfig.GlobalConfigFilename = GlobalConfigFile.FullName;
                }
                else
                {
					Log.Write("Global config filename is unknown; using defaults");
					GlobalConfig = new GlobalConfig();
                    GlobalConfig.GlobalConfigFilename = GlobalConfigFilename;
                }
            }
            catch (Exception E)
            {
                throw new Exception("DirectOutput framework could not initialize global config.\n Inner exception: {0}".Build(E.Message), E);
            }

            if (GlobalConfig.EnableLogging)
            {
                if (GlobalConfig.ClearLogOnSessionStart)
                {
                    try
                    {

                        FileInfo LF = new FileInfo(GlobalConfig.GetLogFilename((!TableFilename.IsNullOrWhiteSpace() ? new FileInfo(TableFilename).FullName : ""), RomName));
                        if (LF.Exists)
                        {
                            LF.Delete();
                        }
                    }
                    catch { }
                }
                try
                {
                    Log.Filename = GlobalConfig.GetLogFilename((!TableFilename.IsNullOrWhiteSpace() ? new FileInfo(TableFilename).FullName : ""), RomName);
                    Log.Instrumentations = GlobalConfig.Instrumentation;
                    Log.Init();
                }
                catch (Exception E)
                {
                    Console.WriteLine(E.StackTrace);
                    throw new Exception("DirectOutput framework could not initialize the log file.\n Inner exception: {0}".Build(E.Message), E);
                }
            }

            // finish logger initialization
            Log.AfterInit();

            try
            {
                Log.Write("Loading Pinball parts");

                Log.Write("Loading cabinet");
                //Load cabinet config
                Cabinet = null;
                FileInfo CCF = GlobalConfig.GetCabinetConfigFile();
                if (CCF != null)
                {
                    if (CCF.Exists)
                    {
                        Log.Write("Will load cabinet config file: {0}".Build(CCF.FullName));
                        try
                        {
                            Cabinet = Cabinet.GetCabinetFromConfigXmlFile(CCF);

                            Log.Write("{0} output controller definitions and {1} toy definitions loaded from cabinet config.".Build(Cabinet.OutputControllers.Count,Cabinet.Toys.Count));

                            Cabinet.CabinetConfigurationFilename = CCF.FullName;
                            if (Cabinet.AutoConfigEnabled)
                            {
                                Log.Write("Cabinet config file has AutoConfig feature enabled. Calling AutoConfig.");
                                try
                                {
                                    Cabinet.AutoConfig();
                                }
                                catch (Exception E)
                                {
                                    Log.Exception("An exception occurred during cabinet auto configuration", E);
                                }
                                Log.Write("Auto-config complete.");
                            }
                            Log.Write("Cabinet config loaded successfully from {0}".Build(CCF.FullName));
                        }
                        catch (Exception E)
                        {
                            Log.Exception("A exception occurred when loading cabinet config file: {0}".Build(CCF.FullName), E);
                        }
                    }
                    else
                    {
                        Log.Warning("Cabinet config file {0} does not exist.".Build(CCF.FullName));
                    }
                }
                if (Cabinet == null)
                {
                    Log.Warning("No cabinet config file loaded. Will use AutoConfig.");
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
                            Log.Exception("A exception occurred when loading table config: {0}".Build(TCF.FullName), E);
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
                        Log.Write("Will try to load configs from DirectOutput.ini or LedControl.ini file(s) for RomName {0}".Build(RomName));
                        //Load ledcontrol

                        Dictionary<int, FileInfo> LedControlIniFiles = GlobalConfig.GetIniFilesDictionary(TableFilename);


                        LedControlConfigList L = new LedControlConfigList();
                        if (LedControlIniFiles.Count > 0)
                        {
                            L.LoadLedControlFiles(LedControlIniFiles, RomName, GlobalConfig, false);
                            Log.Write("{0} directoutputconfig.ini or ledcontrol.ini files loaded.".Build(LedControlIniFiles.Count));
                        }
                        else
                        {
                            Log.Write("No directoutputconfig.ini or ledcontrol.ini files found.");
                        }

                        if (!L.ContainsConfig(RomName))
                        {
                            Log.Write("No config for table found in LedControl data for RomName {0}.".Build(RomName));
                        }
                        else
                        {
                            Log.Write("Config for RomName {0} exists in LedControl data. Updating cabinet and config.".Build(RomName));

                            DirectOutput.LedControl.Setup.Configurator C = new DirectOutput.LedControl.Setup.Configurator();
                            C.EffectMinDurationMs = GlobalConfig.LedControlMinimumEffectDurationMs;
                            C.EffectRGBMinDurationMs = GlobalConfig.LedControlMinimumRGBEffectDurationMs;
                            C.Setup(L, Table, Cabinet, RomName);
                            C = null;
                            //                        L.UpdateTableConfig(Table, RomName, Cabinet);

                            //Check DOF Version
                            Version DOFVersion = typeof(Pinball).Assembly.GetName().Version;
                            
                            if(L.Any(LC=>LC.MinDOFVersion!=null && LC.MinDOFVersion.CompareTo(DOFVersion)>0)) {

                                Version MaxVersion = null;
                                foreach (LedControlConfig LC in L)
                                {
                                    if(LC.MinDOFVersion!=null && (MaxVersion==null || MaxVersion.CompareTo(LC.MinDOFVersion)>0)) {
                                        MaxVersion = LC.MinDOFVersion;
                                    }
                                }


                                Log.Warning("UPDATE DIRECT OUTPUT FRAMEWORK!");
                                if (MaxVersion != null)
                                {
                                    Log.Warning("Current DOF version is {0}, but DOF version {1} or later is required by one or several config files.".Build(DOFVersion, MaxVersion));
                                }
                                try
                                {
                                    
                                    Process.Start(Path.Combine(Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location),"UpdateNotification.exe"));
                                }
                                catch (Exception E)
                                {
                                    Log.Exception("A exception occurred when displaying the update notification", E);
                                }
                            }




                        }
                        L = null;
                    }
                    else
                    {
                        Log.Warning("Cant load config from directoutput.ini or ledcontrol.ini file(s) since no RomName was supplied. No ledcontrol config will be loaded.");
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
                Log.Write("Table config loading finished: romname="+RomName+", tablename="+Table.TableName);


                //update table overrider with romname and tablename references, and activate valid overrides
                TableOverrideSettings.Instance.activeromName = RomName;
                TableOverrideSettings.Instance.activetableName = Table.TableName;
                TableOverrideSettings.Instance.activateOverrides();

                Log.Write("Pinball parts loaded");
            }
            catch (Exception E)
            {
                Log.Exception("DirectOutput framework has encountered a exception during setup.", E);
                throw new Exception("DirectOutput framework has encountered a exception during setup.\n Inner exception: {0}".Build(E.Message), E);
            }

            
        }

        /// <summary>
        /// Initializes/starts the Pinball object
        /// </summary>
        public void Init()
        {

            try
            {

                Log.Write("Starting processes");

                CabinetOwner CO = new CabinetOwner();
				CO.Alarms = this.Alarms;
                CO.ConfigurationSettings.Add("LedControlMinimumEffectDurationMs",GlobalConfig.LedControlMinimumEffectDurationMs);
				CO.ConfigurationSettings.Add("LedWizDefaultMinCommandIntervalMs",GlobalConfig.LedWizDefaultMinCommandIntervalMs);
                CO.ConfigurationSettings.Add("PacLedDefaultMinCommandIntervalMs", GlobalConfig.PacLedDefaultMinCommandIntervalMs);
                Cabinet.Init(CO);

                Table.Init(this);
                Alarms.Init(this);
                Table.TriggerStaticEffects();
                Cabinet.Update();

                //Add the thread initializing the framework to the threadinfo list
                ThreadInfo TI = new ThreadInfo(Thread.CurrentThread);
                TI.HeartBeatTimeOutMs = 10000;
                TI.HostName = "External caller";
                TI.HeartBeat();
                //ThreadInfoList.Add(TI);



                InitMainThread();
                Log.Write("Framework initialized.");
                Log.Write("Have fun! :)");


            }
            catch (Exception E)
            {
                Log.Exception("DirectOutput framework has encountered a exception during initialization.", E);
                throw new Exception("DirectOutput framework has encountered a exception during initialization.\n Inner exception: {0}".Build(E.Message), E);
            }
        }

        /// <summary>
        /// Finishes the Pinball object.
        /// </summary>
        public void Finish()
        {
            try
            {
                Log.Write("Finishing framework");
                FinishMainThread();

                Alarms.Finish();
                Table.Finish();
                Cabinet.Finish();


                //         WriteStatisticsToLog();

                //ThreadInfoList.ThreadTerminates();

                Log.Write("DirectOutput framework finished.");
                Log.Write("Bye and thanks for using!");

            }
            catch (Exception E)
            {
                Log.Exception("A exception occurred while finishing the DirectOutput framework.", E);
                throw new Exception("DirectOutput framework has encountered while finishing.\n Inner exception: {0}".Build(E.Message), E);
            }
        }

        #endregion



        #region MainThread
        /// <summary>
        /// Initializes the main thread.
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
        /// <exception cref="System.Exception">A error occurred during termination of DirectOutput MainThread</exception>
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
                    Log.Exception("A error occurred during termination of DirectOutput MainThread", E);
                    throw new Exception("A error occurred during termination of DirectOutput MainThread", E);
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
        public void MainThreadSignal()
        {
            lock (MainThreadLocker)
            {
                MainThreadDoWork = true;
                Monitor.Pulse(MainThreadLocker);
            }
        }


        private Thread MainThread { get; set; }
        private object MainThreadLocker = new object();
        private bool KeepMainThreadAlive = true;
        private bool MainThreadDoWork = false;
        //TODO: Maybe this should be a config option
        const int MaxInputDataProcessingTimeMs = 10;


        /// <summary>
        /// This method is constantly being executed by the main thread of the framework.<br/>
        /// Don't call this method directly. Use the Init and FinishMainThread methods.
        /// </summary>
        //TODO: Think about implementing something which does really check on value changes on tableelements or triggered effects before setting update required.
        private void MainThreadDoIt()
        {

            //ThreadInfoList.HeartBeat("DirectOutput");
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
                            //Log.Write("Pinball.MainThreadDoIt...d.name="+D.Name+", value="+D.Value+", d="+D.Number+", d="+D);
                            Table.UpdateTableElement(D);
                            UpdateRequired |= true;
                      
                        }
                        catch (Exception E)
                        {
                            Log.Exception("A unhandled exception occurred while processing data for table element {0} {1} with value {2}".Build(D.TableElementType, D.Number, D.Value), E);
                            //ThreadInfoList.RecordException(E);

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
                            Log.Exception("A unhandled exception occurred while executing timer events.", E);
                            //ThreadInfoList.RecordException(E);
                        }
                    }


                    //Call update on output controllers if necessary
                    if (UpdateRequired && KeepMainThreadAlive)
                    {
                        try
                        {
                            Cabinet.Update();
                        }
                        catch (Exception E)
                        {
                            Log.Exception("A unhandled exception occurred while updating the output controllers", E);
                            //ThreadInfoList.RecordException(E);
                        }
                    }

                    if (KeepMainThreadAlive)
                    {
                        //ThreadInfoList.HeartBeat();
                        //Sleep until we get more input data and/or a timer expires.
                        DateTime NextAlarm = Alarms.GetNextAlarmTime();

                        lock (MainThreadLocker)
                        {
                            while (InputQueue.Count == 0 && NextAlarm > DateTime.Now && !MainThreadDoWork && KeepMainThreadAlive)
                            {
                                int TimeOut = ((int)(NextAlarm - DateTime.Now).TotalMilliseconds).Limit(1, 50);

                                Monitor.Wait(MainThreadLocker, TimeOut);  // Lock is released while we’re waiting
                                //ThreadInfoList.HeartBeat();
                            }
                        }
                        MainThreadDoWork = false;
                    }


                }
            }
            catch (Exception E)
            {
                Log.Exception("A unexpected exception occurred in the DirectOutput MainThread", E);
                //ThreadInfoList.RecordException(E);
            }

            //ThreadInfoList.ThreadTerminates();
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
            //ThreadInfoList.HeartBeat("Data delivery");

        }


        /// <summary>
        /// Receives  data for named table elements.
        /// The received data is put in a queue and the internal thread of the framework is notified about the availability of new data.
        /// </summary>
        /// <param name="TableElementName">Name of the table element.</param>
        /// <param name="Value">The value of the table element.</param>
        public void ReceiveData(string TableElementName, int Value)
        {
            //Log.Write("TableName:"+TableElementName);
            //Log.Write("Update {0}: {1}".Build(TableElementName, Value));
            InputQueue.Enqueue(TableElementName, Value);
            MainThreadSignal();

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
            //ThreadInfoList.HeartBeat("Data delivery");
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
            S += "    Table name: " + Table.TableName + "\n";
            S += "    Table filename: " + Table.TableFilename + "\n";
            S += "    RomName: " + Table.RomName + "\n";
            S += "    Table config source: " + Table.ConfigurationSource + "\n";
            S += "    Table config filename: " + Table.TableConfigurationFilename + "\n";
            S += "    Table Elements count: " + Table.TableElements.Count + "\n";
            S += "    Table Effects count: " + Table.Effects.Count + "\n";
            S += "  }\n";
            S += "  Cabinet {\n";
            S += "     Cabinet config filename: " + Cabinet.CabinetConfigurationFilename + "\n";
            S += "     Output controllers count: " + Cabinet.OutputControllers.Count + "\n";
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


        // <summary>
        // Initializes a new instance of the <see cref="Pinball" /> class and calls the Init method with the specified parameters.
        // </summary>
        // <param name="GlobalConfigFilename">The global config filename.</param>
        // <param name="TableFilename">The table filename.</param>
        // <param name="RomName">Name of the rom.</param>
        //public Pinball(string GlobalConfigFilename = "", string TableFilename = "", string RomName = "")
        //    : this()
        //{
        //    Init(GlobalConfigFilename, TableFilename, RomName);
        //}
        #endregion


    }
}
