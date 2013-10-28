


/// <summary>
/// DirectOutput is the root namespace for the DirectOutput framework. 
/// </summary>
using System.IO;
using System.Reflection;
using System;
namespace DirectOutput
{
    /// <summary>
    /// Static class providing simple access to the DirectOutput framework functionality.
    /// </summary>
    public static class DirectOutputHandler
    {

        /// <summary>
        /// Gets the version of the DirectOutput framework.
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Receives data to be processed by the DirectOutput framework.<br/>
        /// This function has to be called whenever the state of a table element changes in the hosting application.
        /// </summary>
        /// <param name="TableElementTypeChar">The table element type char.</param>
        /// <param name="Number">The number of the table element.</param>
        /// <param name="Value">The value of the table element.</param>
        /// <exception cref="System.Exception">
        /// Could not extract the first char of the TableElementTypeChar parameter
        /// or
        /// You must call Init before passing data to the DirectOutput framework
        /// or
        /// A exception occured when passing in data (TableElementTypeChar: {0}, Number: {1}, Value: {2})
        /// </exception>
        public static void DataReceive(string TableElementTypeChar, int Number, int Value)
        {
            char C;
            try
            {
                C = TableElementTypeChar[0];
            }
            catch (Exception E)
            {

                throw new Exception("Could not extract the first char of the TableElementTypeChar parameter", E);
            }
            try
            {
                Pinball.ReceiveData(C, Number, Value);
            }
            catch (Exception E)
            {
                if (Pinball == null)
                {
                    throw new Exception("You must call Init before passing data to the DirectOutput framework");
                }
                else
                {
                    throw new Exception("A exception occured when passing in data (TableElementTypeChar: {0}, Number: {1}, Value: {2})".Build(C, Number, Value), E);
                }
            }
        }


        /// <summary>
        /// Finishes the DirectOutput framework.<br/>
        /// The is the last methods to be called on the framework.
        /// </summary>
        public static void Finish()
        {
            if (Pinball != null)
            {
                Pinball.Finish();
                Pinball = null;
            }

        }

        /// <summary>
        /// Initializes the DirectOutput framework.<br/>
        /// The method has to be called before any data is sent to DOF.<br/>
        /// It loads all necessary configuration data and starts all internal processes.
        /// </summary>
        /// <param name="HostingApplicationName">Name of the hosting application.</param>
        /// <param name="TableFilename">The table filename (specify a dummy filename of no table file is available).</param>
        /// <param name="RomName">Name of the rom (If thhere is no rom name of a table, specify your own unique name for the game).</param>
        /// <exception cref="System.Exception">Object has already been initialized. You must call Finish() before initializing again.</exception>
        public static void Init(string HostingApplicationName, string TableFilename, string RomName)
        {
            if (Pinball == null)
            {
                //Check config dir for global config file

                string HostAppFilename = HostingApplicationName.Replace(".", "");

                foreach (char C in Path.GetInvalidFileNameChars())
                {
                    HostAppFilename = HostAppFilename.Replace(C.ToString(), "");
                }

                foreach (char C in Path.GetInvalidPathChars())
                {
                    HostAppFilename = HostAppFilename.Replace(C.ToString(), "");
                }


                HostAppFilename = "GlobalConfig_{0}".Build(HostAppFilename);

                //Check config dir for global config file
                FileInfo F = new FileInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "config", "GlobalConfig_{0}.xml".Build(HostAppFilename)));
                if (!F.Exists)
                {
                    //Check if a shortcut to the config dir exists
                    FileInfo LnkFile = new FileInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "config", "GlobalConfig_{0}.lnk".Build(HostAppFilename)));
                    if (LnkFile.Exists)
                    {
                        string ConfigDirPath = ResolveShortcut(LnkFile);
                        if (Directory.Exists(ConfigDirPath))
                        {
                            F = new FileInfo(Path.Combine(ConfigDirPath, "GlobalConfig_{0}.xml".Build(HostAppFilename)));
                        }
                    }
                    if (!F.Exists)
                    {

                        //Check default dir for global config file
                        F = new FileInfo("GlobalConfig_{0}.xml".Build(HostAppFilename));
                        if (!F.Exists)
                        {
                            //Check dll dir for global config file
                            F = new FileInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "GlobalConfig_{0}.xml".Build(HostAppFilename)));
                            if (!F.Exists)
                            {
                                //if global config file does not exist, set filename to config directory.
                                F = new FileInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "config", "GlobalConfig_{0}.xml".Build(HostAppFilename)));
                                if (!F.Directory.Exists)
                                {
                                    //If the config dir does not exist set the dll dir for the config
                                    F = new FileInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "GlobalConfig_{0}.xml".Build(HostAppFilename)));
                                }
                            }
                        }
                    }
                }


                Pinball = new DirectOutput.Pinball();
                Pinball.Init((F.Exists ? F.FullName : ""), TableFilename, RomName);
            }
            else
            {
                throw new Exception("Object has already been initialized. You must call Finish() before initializing again.");
            }
        }


        /// <summary>
        /// Shows the frontend of the DirectOutput framework.
        /// </summary>
        /// <exception cref="System.Exception">Init has to be called before the frontend is opend.</exception>
        public static void ShowFrontend()
        {
            if (Pinball != null)
            {
                try
                {
                    DirectOutput.Frontend.MainMenu.Open(Pinball);
                }
                catch (Exception E)
                {
                    System.Windows.Forms.MessageBox.Show("Could not show DirectOutput frontend.\n The following exception occured:\n{0}".Build(E.Message), "DirectOutput");
                }

            }
            else
            {
                throw new Exception("Init has to be called before the frontend is opend.");
            }
        }




        private static string ResolveShortcut(FileInfo ShortcutFile)
        {
            string TargetPath = "";
            try
            {
                Type WScriptShell = Type.GetTypeFromProgID("WScript.Shell");
                object Shell = Activator.CreateInstance(WScriptShell);
                object Shortcut = WScriptShell.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, Shell, new object[] { ShortcutFile.FullName });
                TargetPath = (string)Shortcut.GetType().InvokeMember("TargetPath", BindingFlags.GetProperty, null, Shortcut, null);
                Shortcut = null;
                Shell = null;
            }
            catch
            {

            }

            try
            {
                if (Directory.Exists(TargetPath))
                {
                    return TargetPath;
                }
                else if (File.Exists(TargetPath))
                {
                    return TargetPath;
                }
                else
                {
                    return "";
                }

            }
            catch
            {
                return "";
            }
        }

        #region Properties



        private static Pinball _Pinball;

        /// <summary>
        /// Gets the \ref DirectOutput.Pinball object.
        /// </summary>
        /// <value>
        /// The \ref DirectOutput.Pinball object.
        /// </value>
        public static Pinball Pinball
        {
            get { return _Pinball; }
            private set { _Pinball = value; }
        }

        #endregion

    }
}
