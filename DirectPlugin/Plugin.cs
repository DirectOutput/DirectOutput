using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using DirectPluginInterface;
using DirectOutput;

/// <summary>
/// DirectPlugin is the namespace of the Dll implementing the actual plugin interface.
/// </summary> 
namespace DirectPlugin
{

    /// <summary>
    /// DirectOutputPlugin is the IPlugin interface implementation required by generic the PluginInterface.dll.
    /// </summary>
    [Export(typeof(IDirectPlugin))]
    public class DirectOutputPlugin : IDirectPlugin
    {
        
        #region IPlugin Members

        /// <summary>
        /// Gets the name of this IPlugin.<br/>
        /// \remark This property returns the version of the DirectOutput.dll, NOT the version of the DirectOutputPlugin plugin.
        /// </summary>
        /// <value>
        /// The name of this IPlugin (Name is DirectOutput (V: VersionNumber) af of TimeStamp).
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
        /// This method is called, when the hosting application sends new data to DirectOutput.<br />
        /// The IPlugin interface requires the implementation of this method.<br/>
        /// </summary>
        /// <param name="TableElementTypeChar">Char identifying the type of the table element. For a list of valid char, check to docu on the TableElementTypeEnum in DirectOutput.</param>
        /// <param name="Number">The received Number.</param>
        /// <param name="Value">The received value.</param>
        public void DataReceive(char TableElementTypeChar, int Number, int Value)
        {

            Pinball.ReceiveData(TableElementTypeChar, Number, Value);
            
        }




        /// <summary>
        /// Finishes the plugin.<br />
        /// This is the last method called, before a plugin is discared. This method is also called, after a undhandled exception has occured in a plugin.<br/>
        /// The IPlugin interface requires the implementation of this method.<br/>
        /// </summary>
        public void PluginFinish()
        {
         
                Pinball.Finish();
      
        }

        /// <summary>
        /// Initializes the Plugin.<br/>
        /// The IPlugin interface requires the implementation of this method.<br/>
        /// </summary>
        /// <param name="HostingApplicationName">Name of the hosting application.</param>
        /// <param name="TableFilename">The table filename.<br>If no table filename is available, it is best to provide a path and name of a non existing dummy file, since the plugins might use this path to identify the directories where the store logs, load configs from and so on.</br></param>
        /// <param name="GameName">Name of the game.<br/>If the game is a SS pinball table it is highly recommanded to provide the name of the game rom, otherwise any other name which identifiey to game uniquely will be fine as well.</param>
        public void PluginInit(string HostingApplicationName, string TableFilename, string GameName)
        {
            string HostAppFilename = HostingApplicationName.Replace(".", "");
            foreach (char C in Path.GetInvalidFileNameChars() )
            {
                HostAppFilename=HostAppFilename.Replace(""+C,"");
            }
            foreach (char C in Path.GetInvalidPathChars())
            {
                HostAppFilename = HostAppFilename.Replace("" + C, "");
            }
            HostAppFilename = "GlobalConfig_{0}".Build(HostAppFilename);

            //Check config dir for global config file
            FileInfo F = new FileInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "config", HostAppFilename+".xml"));
            if (!F.Exists)
            {
                //Check if a shortcut to the config dir exists
                FileInfo LnkFile = new FileInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "config", HostAppFilename + ".lnk"));
                if (LnkFile.Exists)
                {
                    string ConfigDirPath = ResolveShortcut(LnkFile);
                    if (Directory.Exists(ConfigDirPath))
                    {
                        F = new FileInfo(Path.Combine(ConfigDirPath, HostAppFilename+".xml"));
                    }
                }
               
            }

            Pinball.Init(F.FullName,TableFilename,GameName );

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
        /// Initializes a new instance of the <see cref="DirectOutputPlugin"/> class.
        /// </summary>
        public DirectOutputPlugin()
        {


        }











    }
}
