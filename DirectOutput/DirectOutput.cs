


/// <summary>
/// DirectOutput is the root namespace for the DirectOutput framework. 
/// </summary>
using System.IO;
using System.Reflection;
using System;
namespace DirectOutput
{
    public static class DirectOutputHandler
    {

        public static string GetVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

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


        public static void Finish()
        {
            if (Pinball != null)
            {
                Pinball.Finish();
                Pinball = null;
            }

        }

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
