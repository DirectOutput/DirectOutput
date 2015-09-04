using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using DracLabs;
using FuzzyStrings;
using System.Reflection;
using PinballX.Table2RomMapping;


namespace PinballX
{

    /// <summary>
    /// This is the base calls for PinballX plugins.
    /// It contains the public methods which are called from PinballX when it is using the plugin.
    /// </summary>
    public class Plugin
    {

        DOFManager DM = new DOFManager();


        int LastPBXState = -1;
        private void UpdatePBXState(int State)
        {
            if (State != LastPBXState)
            {
                string StateName = GetPBXStateName(LastPBXState);
                if (StateName != null)
                {
                    DM.UpdateNamedTableElement(StateName, 0);
                }

                StateName = GetPBXStateName(State);
                if (StateName != null)
                {
                    DM.UpdateNamedTableElement(StateName, 1);
                }

                LastPBXState = State;

            }

        }


        private string GetPBXStateName(int State)
        {
            switch (State)
            {
                case 10:
                    return "PBXWheel";
                case 20:
                    return "PBXMenu";
                default:
                    break;
            }
            return null;
        }

        private enum InputActionEnum
        {
            Quit,
            Left,
            Right,
            Select,
            PageLeft,
            PageRight,
            Instructions,
            NoInput
        }

        private Dictionary<int, InputActionEnum> KeyInputs = new Dictionary<int, InputActionEnum>();
        private Dictionary<int, InputActionEnum> JoyInputs = new Dictionary<int, InputActionEnum>();
        private bool OneClickLaunch = false;

        private void AddToDict(Dictionary<int, InputActionEnum> D, string Code, InputActionEnum Action)
        {
            int C = 0;
            if (int.TryParse(Code, out C))
            {
                if (!D.ContainsKey(C))
                {
                    D.Add(C, Action);
                }
            }
        }

        private void ReadPBXConfig()
        {
            Log("Loading PBX config data");
            string IniFileName = Path.Combine(new DirectoryInfo(".").FullName, "config\\PinballX.ini");

            if (File.Exists(IniFileName))
            {
                IniFile I = new IniFile();
                I.Load(IniFileName, false);

                AddToDict(KeyInputs, I.GetKeyValue("KeyCodes", "quit"), InputActionEnum.Quit);
                AddToDict(KeyInputs, I.GetKeyValue("KeyCodes", "left"), InputActionEnum.Left);
                AddToDict(KeyInputs, I.GetKeyValue("KeyCodes", "right"), InputActionEnum.Right);
                AddToDict(KeyInputs, I.GetKeyValue("KeyCodes", "select"), InputActionEnum.Select);
                AddToDict(KeyInputs, I.GetKeyValue("KeyCodes", "pageleft"), InputActionEnum.PageLeft);
                AddToDict(KeyInputs, I.GetKeyValue("KeyCodes", "pageright"), InputActionEnum.PageRight);
                AddToDict(KeyInputs, I.GetKeyValue("KeyCodes", "instructions"), InputActionEnum.Instructions);


                AddToDict(JoyInputs, I.GetKeyValue("JoyCodes", "quit"), InputActionEnum.Quit);
                AddToDict(JoyInputs, I.GetKeyValue("JoyCodes", "left"), InputActionEnum.Left);
                AddToDict(JoyInputs, I.GetKeyValue("JoyCodes", "right"), InputActionEnum.Right);
                AddToDict(JoyInputs, I.GetKeyValue("JoyCodes", "select"), InputActionEnum.Select);
                AddToDict(JoyInputs, I.GetKeyValue("JoyCodes", "instructions"), InputActionEnum.Instructions);

                try
                {
                    OneClickLaunch = I.GetKeyValue("Interface", "OneClickLaunch").Equals("True", StringComparison.InvariantCultureIgnoreCase);

                }
                catch
                {

                    OneClickLaunch = false;
                }

                Log("PBX config data loaded");
            }
            else
            {
                Log("No PBX copnfig data found");
                //No ini file found
            }
        }

        List<string> TableRomNames = new List<string>();
        TableNameMappings AllTableMappings = new TableNameMappings();
        // Dictionary<string, string> ConfiguredRomMap = new Dictionary<string, string>();
        //Dictionary<string, string> AllRomMap = new Dictionary<string, string>();


        private void SetupRomNameLinks()
        {
            Log("Loading Table/Romname mappings");
            AllTableMappings = new TableNameMappings();

            //AllTableMappings.Add(new Mapping() { TableName = "xxx", RomName = "yyy" });
            //AllTableMappings.Add(new Mapping() { TableName = "aaa", RomName = "bbb" });
            
            //AllTableMappings.SaveTableMappings(".\\mappings.xml");

               string TableMappingFileName = DM.GetTableMappingFilename();
            Log("Table mapping filename: " + TableMappingFileName);
            if (!string.IsNullOrEmpty(TableMappingFileName))
            {
                AllTableMappings = TableNameMappings.LoadTableMappings(TableMappingFileName);
            }

            Log(AllTableMappings.ToString() + " TableMappings loaded");

            string[] L=DM.GetConfiguredTableElmentDescriptors();
            foreach (string R in L)
            {
                if (R.StartsWith("$"))
                {
                    TableRomNames.Add(R.Substring(1).Trim().ToUpper());
                }

            }

            if (TableRomNames.Count > 0)
            {
                Log("The following " + TableRomNames.Count + " RomNames are configured in DOF: " + string.Join(",", TableRomNames.ToArray()));
            }
            else
            {
                Log("No romnames are configured in DOF.");
                //return;
            }
            Log("Table/RomName mapping loaded");
           

  



           

        }



        Dictionary<string, string> RomLookupCache = new Dictionary<string, string>();

        private string GetRom(string GameDecriptionShort)
        {
            string GameDesc = CleanGameDescription(GameDecriptionShort.ToUpper());


            if (TableRomNames.Contains(GameDecriptionShort)) return GameDesc;


            if (RomLookupCache.ContainsKey(GameDesc)) return RomLookupCache[GameDesc];


            Mapping MatchMapping = null;
            double MatchValue = 0;

            foreach (Mapping Mapping in AllTableMappings)
            {
                double M = FuzzyStrings.FuzzyText.DiceCoefficient(Mapping.TableName.ToUpper(), GameDesc);

                if (M > MatchValue)
                {
                    MatchValue = M;
                    MatchMapping = Mapping;
                }
            }

            if(MatchMapping!=null) {
                Log("Best match for " + GameDesc + " is " + MatchMapping.TableName.ToUpper() + " (" + MatchMapping.RomName + "). Match Value: " + MatchValue.ToString());
            }
            if (MatchValue > 0.55 && MatchMapping!=null)
            {
                string Rom = MatchMapping.RomName;


                string UseTableRom = null;

                foreach (string TableRomName in TableRomNames)
                {
                    if (TableRomName.Equals(Rom,StringComparison.InvariantCultureIgnoreCase))
                    {
                        UseTableRom = TableRomName;
                        break;
                    };
                };



                if (!RomLookupCache.ContainsKey(GameDesc))
                {
                    RomLookupCache.Add(GameDesc, UseTableRom);
                }

                if (UseTableRom != null)
                {

                    return UseTableRom;
                }
            }


            return null;
        }

        private string CleanGameDescription(string GameName)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 ]");
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);


            return regex.Replace(rgx.Replace(GameName, ""), @" ");

        }

        Config Config = null;

        private void LoadConfig()
        {
            Log("Loading plugin config");
            try
            {
                Config = Config.GetConfigFromXmlFile();
            }
            catch { }
            if (Config == null)
            {
                Log("Plugin config loading failed. Using default values.");
                Config = new Config();

            }
            Log("Plugin config loaded");
        }


        private void Log(string Text)
        {
            if (Config != null && Config.EnableLogging)
            {
                TextWriter tw = null;
                try
                {
                    tw = new StreamWriter(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "PinballX DirectOutput Plugin.log"), true);

                    tw.WriteLine("{1:yy.MM.dd hh:mm:ss.fff} {0}", Text, DateTime.Now);


                    tw.Close();
                }
                catch
                {

                }
            }
        }
        /// <summary>
        /// Initializes the plugin.
        /// This method is called when PinballX loads and initializes the plugin.
        /// Be sure to return false if a exception ocurres during plugin initialization (plugin will be disabled).
        /// </summary>
        /// <param name="InfoPtr">The InfoPTR for the PinballXInfo struct.</param>
        /// <returns><c>true</c> if the plugins has been initialized succesfully, otherwise <c>false</c></returns>
        public bool Initialize(IntPtr InfoPtr)
        {
            PinballXInfo Info = (PinballXInfo)Marshal.PtrToStructure(InfoPtr, typeof(PinballXInfo));

            bool InitOK = true;
            try
            {
                LoadConfig();
                
                Config.EnableLogging = true;

                Log("Initializing plugin");

                ReadPBXConfig();



                Log("Initializing DOF");
                DM.Init();
                Log("DOF initialized");


                SetupRomNameLinks();

                Log("Sending initial PBX state to DOF");
                UpdatePBXState(10);

                DM.UpdateNamedTableElement("PBXScreenSaver", 0);
                Log("Init complete");
            }
            catch (Exception E)
            {
                Log("Init failed: " + E.Message);
                InitOK = false;
            }


            return InitOK;
        }

        /// <summary>
        /// The method is called when the configure button in the plugin manager of PinballX is clicked.
        /// Open configs windows in the method.
        /// </summary>
        public void Configure()
        {
            Config CO = new Config(); ;
            if (File.Exists(Config.ConfigFileName))
            {
                try
                {
                    CO = Config.GetConfigFromXmlFile();


                }
                catch { }

            }

            Configure C = new Configure(CO);

            if (C.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                C.Config.SaveConfigXmlFile();
            }


        }


        /// <summary>
        /// This method is called when PinballX exits
        /// </summary>
        public void Event_App_Exit()
        {
            try
            {
                if (!string.IsNullOrEmpty(LastGameSelect))
                {
                    DM.UpdateNamedTableElement(LastGameSelect, 0);
                }
                UpdatePBXState(-1);

                DM.Finish();
                DM = null;

                Log("Exiting PBX");

            }
            catch (Exception E)
            {

                Log("App_Exit failed: " + E.Message);
            }
        }



        /// <summary>
        /// This method is called when a game is selected in PinballX.
        /// </summary>
        /// <param name="InfoPtr">The InfoPTR to the GameInfo structure.</param>

        private string LastGameSelect = null;
        public void Event_GameSelect(IntPtr InfoPtr)
        {
            try
            {
                GameInfo Info = (GameInfo)Marshal.PtrToStructure(InfoPtr, typeof(GameInfo));

                if (!string.IsNullOrEmpty(LastGameSelect))
                {
                    DM.UpdateNamedTableElement(LastGameSelect, 0);
                }
                SendAction("PBXGameSelect");
                LastGameSelect = GetRom(Info.GameShortDescription);
                if (!string.IsNullOrEmpty(LastGameSelect))
                {
                    DM.UpdateNamedTableElement(LastGameSelect, 1);
                }

                Log("Game selected " + Info.GameShortDescription + " (" + (string.IsNullOrEmpty(LastGameSelect) ? "No update sent" : "Update sent for " + LastGameSelect) + ")");
            }
            catch (Exception E)
            {
                Log("GameSelect failed: " + E.Message);
            }
        }

        /// <summary>
        /// This method is called before a game is launched.
        /// Return true to tell PinballX process the event
        /// Return false to tell PinballX NOT to process the event
        /// </summary>
        /// <param name="InfoPtr">The InfoPTR to the GameInfo structure.</param>
        /// <returns><c>true</c> if the game has to be launched, otherwise <c>false</c>.</returns>
        public bool Event_GameRun(IntPtr InfoPtr)
        {
            try
            {
                GameInfo Info = (GameInfo)Marshal.PtrToStructure(InfoPtr, typeof(GameInfo));
                if (!string.IsNullOrEmpty(LastGameSelect))
                {
                    DM.UpdateNamedTableElement(LastGameSelect, 0);
                }
                UpdatePBXState(-1);
                DM.Finish();

                Log("Running " + Info.GameShortDescription);
            }
            catch (Exception E)
            {
                Log("GameRun failed: " + E.Message);
            }

            return true;
        }





        /// <summary>
        /// This method is after PinballX has built the command line parameters.
        /// You can return a modified command line from this method.
        /// To do nothing return an empty string or the same command line 
        /// </summary>
        /// <param name="InfoPtr">The InfoPTR to the GameInfo structure.</param>
        /// <returns>Returns the parameters for the command line (modified if required) or a emty string to preserve to or command line paras.</returns>
        public string Event_Parameters(IntPtr InfoPtr)
        {

            GameInfo Info = (GameInfo)Marshal.PtrToStructure(InfoPtr, typeof(GameInfo));
            string CmdLine = Info.Parameters;

            return CmdLine;
        }


        /// <summary>
        /// This method is called when the emulator exits from a game.
        /// </summary>
        /// <param name="InfoPtr">The InfoPTR to the GameInfo structure.</param>
        public void Event_GameExit(IntPtr InfoPtr)
        {
            try
            {
                GameInfo Info = (GameInfo)Marshal.PtrToStructure(InfoPtr, typeof(GameInfo));


                DM.Init();
                UpdatePBXState(10);
                DM.UpdateNamedTableElement("PBXScreenSaver", 0);

                Log("Exiting game " + Info.GameShortDescription);
            }
            catch (Exception E)
            {
                Log("GameExit failed: " + E.Message);
            }
        }

        /// <summary>
        /// This method is called when PinballX receives input from keyboard or buttons
        /// </summary>
        /// <param name="Input_Keys">The input keys.</param>
        /// <param name="Input_Buttons">The input buttons.</param>
        /// <param name="PinballXStatus">The PinballX status.</param>
        /// <returns></returns>
        public bool Event_Input(bool[] Input_Keys, bool[] Input_Buttons, int PinballXStatus)
        {
            Log("Event_Input");
            try
            {

                InputActionEnum InputAction = GetInputAction(KeyInputs, Input_Keys);
                if (InputAction == InputActionEnum.NoInput)
                {
                    InputAction = GetInputAction(JoyInputs, Input_Buttons);
                }

                string PBXAction = null;
                if (InputAction != InputActionEnum.NoInput)
                {

                    switch (PinballXStatus)
                    {
                        case 10:
                            //Wheel mode
                            switch (InputAction)
                            {
                                case InputActionEnum.Quit:
                                    PBXAction = "PBXQuit";
                                    break;
                                case InputActionEnum.Left:
                                    PBXAction = "PBXWheelRight";
                                    break;
                                case InputActionEnum.Right:
                                    PBXAction = "PBXWheelLeft";
                                    break;
                                case InputActionEnum.Select:
                                    if (!OneClickLaunch)
                                    {
                                        PBXAction = "PBXMenuOpen";
                                    }
                                    break;
                                case InputActionEnum.PageLeft:
                                    PBXAction = "PBXWheelPageRight";
                                    break;
                                case InputActionEnum.PageRight:
                                    PBXAction = "PBXWheelPageLeft";
                                    break;
                                case InputActionEnum.Instructions:
                                    PBXAction = "PBXInstructions";
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case 20:
                            //Menu
                            switch (InputAction)
                            {
                                case InputActionEnum.Quit:
                                    PBXAction = "PBXMenuQuit";
                                    break;
                                case InputActionEnum.Left:
                                    PBXAction = "PBXMenuUp";
                                    break;
                                case InputActionEnum.Right:
                                    PBXAction = "PBXMenuDown";
                                    break;
                                case InputActionEnum.Select:
                                    PBXAction = "PBXMenuSelect";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:

                            break;
                    }


                }



                SendAction(PBXAction);
                UpdatePBXState(PinballXStatus);
            }
            catch (Exception E)
            {

                Log("Event_Input failed: " + E.Message);
            }


            return true;
        }


        private InputActionEnum GetInputAction(Dictionary<int, InputActionEnum> InputDefs, bool[] Input)
        {
            foreach (int K in InputDefs.Keys)
            {
                if (Input[K])
                {
                    return InputDefs[K];
                }
            }

            return InputActionEnum.NoInput;



        }

        private void SendAction(string Action)
        {
            if (!string.IsNullOrEmpty(Action))
            {
                DM.SignalNamedTableElement(Action);
                Log("Action: " + Action);
            }
        }


        /// <summary>
        /// This event is called when PinballX starts or quits the screen saver. 
        /// </summary>
        /// <param name="Type">The type of the call (Start ScreenSaver=1, End Screensaver=2).</param>
        /// <returns>Return true to tell PinballX process the event. Return false to tell PinballX NOT to process the event</returns>
        public bool Event_ScreenSaver(int Type)
        {

            try
            {
                //Start = 1,
                //End = 2,
                switch (Type)
                {

                    case 1:
                        SendAction("PBXScreenSaverStart");
                        DM.UpdateNamedTableElement("PBXcreenSaver", 1);
                        break;

                    case 2:
                        SendAction("PBXScreenSaverQuit");
                        DM.UpdateNamedTableElement("PBXScreenSaver", 0);

                        break;
                }
            }
            catch (Exception E)
            {

                Log("Event screensaver failed: " + E.Message);
            }
            return true;
        }



        public bool Event_DisableDMD()
        {
            return false;
        }

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        public Plugin()
        {
        }

        #endregion
        #region Dispose


        /// <summary>
        /// Releases unmanaged and managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            //// remove this from gc finalizer list
        }

        private bool disposed = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            //
            if (!(this.disposed))
            {

                //// called from Dispose
                if ((disposing))
                {

                    // Clean up managed resources

                    // Clean up unmanaged resources here.

                }
            }


            disposed = true;
        }

        #endregion

        #region PluginInfo Properties

        // Usually it is not necessary to modfy the code for the following properties.
        // Instead set the correct values for those properties in the PlufinInfo structure.

        public string Name
        {
            get { return PluginInfo.Name; }
        }

        public string Version
        {
            get { return PluginInfo.Version; }
        }

        public string Author
        {
            get { return PluginInfo.Author; }
        }

        public string Description
        {
            get { return PluginInfo.Description; }
        }

        public string PluginVersion
        {
            get { return PluginInfo.PluginVersion; }
        }
        #endregion

    }

}



