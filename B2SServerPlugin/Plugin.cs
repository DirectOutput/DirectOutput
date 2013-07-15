using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using B2SServerPluginInterface;
using DirectOutput;

/// <summary>
/// DirectOutputPlugin is the namespace of the Dll implementing the actual plugin interface for the B2S Server.
/// </summary> 
namespace B2SServerPlugin
{

    /// <summary>
    /// Plugin is IDirectPlugin interface implementation required by the B2S Server.
    /// </summary>
    [Export(typeof(IDirectPlugin))]
    public class Plugin : IDirectPlugin, IDirectPluginFrontend
    {

        #region IDirectPluginFrontend Member


        /// <summary>
        /// Shows the Frontend of the Plugin.<br/>
        /// The IDirectPluginFrontend interface requires the implementation of this method.
        /// </summary>
        public void PluginShowFrontend(System.Windows.Forms.Form Owner = null)
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

        #endregion


        #region IDirectPlugin Members

        /// <summary>
        /// Gets the name of this IDirectPlugin.<br/>
        /// \remark This property returns the version of the DirectOutput.dll, NOT the version of the B2SServer plugin.
        /// </summary>
        /// <value>
        /// The name of this IDirectPlugin (Name is DirectOutput (V: VersionNumber) af of TimeStamp).
        /// </value>
        public string Name
        {
            get
            {
                Version V = typeof(Pinball).Assembly.GetName().Version;
                DateTime BuildDate = new DateTime(2000, 1, 1).AddDays(V.Build).AddSeconds(V.Revision * 2);
                return "DirectOutput (V: {0} as of {1})".Build(V.ToString(), BuildDate.ToString("yyyy.MM.dd HH:mm"));
            }
        }



        /// <summary>
        /// This method is called, when new data from Pinmame becomes available.<br/>
        /// The IDirectPlugin interface requires the implementation of this method.
        /// </summary>
        /// <param name="TableElementTypeChar">Char representing the table element type (S=Solenoid, W=Switch, L=Lamp, M=Mech, G=GI, E=EMTable).</param>
        /// <param name="Number">The number of the table element.</param>
        /// <param name="Value">The value of the table element.</param>
        public void DataReceive(char TableElementTypeChar, int Number, int Value)
        {
            Pinball.ReceiveData(TableElementTypeChar, Number, Value);
        }




        /// <summary>
        /// Finishes the plugin.<br />
        /// This is the last method called, before a plugin is discared. This method is also called, after a undhandled exception has occured in a plugin.
        /// </summary>
        public void PluginFinish()
        {

            Pinball.Finish();
        }

        /// <summary>
        /// Initializes the Plugin.<br/>
        /// The IDirectPlugin interface requires the implementation of this method.<br/>
        /// </summary>
        /// <param name="TableFilename">The table filename.</param>
        /// <param name="RomName">Name of the rom.</param>
        public void PluginInit(string TableFilename, string RomName)
        {

            //Check config dir for global config file
            FileInfo F = new FileInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "config", "GlobalConfig_B2SServer.xml"));
            if (!F.Exists)
            {
                //Check if a shortcut to the config dir exists
                FileInfo LnkFile = new FileInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "config", "GlobalConfig_B2SServer.lnk"));
                if (LnkFile.Exists)
                {
                    string ConfigDirPath = ResolveShortcut(LnkFile);
                    if (Directory.Exists(ConfigDirPath))
                    {
                        F = new FileInfo(Path.Combine(ConfigDirPath, "GlobalConfig_B2SServer.xml"));
                    }
                }
                if (!F.Exists)
                {

                    //Check table dir for global config file
                    F = new FileInfo("GlobalConfig_B2SServer.xml");
                    if (!F.Exists)
                    {
                        //Check dll dir for global config file
                        F = new FileInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "GlobalConfig_B2SServer.xml"));
                        if (!F.Exists)
                        {
                            //if global config file does not exist, set filename to config directory.
                            F = new FileInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "config", "GlobalConfig_B2SServer.xml"));
                        }
                    }
                }
            }

            Pinball.Init(F.FullName, TableFilename, RomName);


            //PluginShowFrontend();


        }


        private string ResolveShortcut(FileInfo ShortcutFile)
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

        #endregion

        #region Properties



        private Pinball _Pinball = new Pinball();

        /// <summary>
        /// Gets or sets the Pinball object for the plugin.
        /// </summary>
        /// <value>
        /// The Pinball object for the plugin.
        /// </value>
        public Pinball Pinball
        {
            get { return _Pinball; }
            set { _Pinball = value; }
        }

        #endregion





        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        public Plugin()
        {


        }











    }
}
