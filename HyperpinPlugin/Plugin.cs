using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using HyperpinPluginInterface;
using DirectOutput;

/// <summary>
/// DirectOutputHyperpinPlugin is the namespace of the Dll implementing the actual plugin interface for Hyperpin.
/// </summary> 
namespace DirectOutputHyperpinPlugin
{

    /// <summary>
    /// DirectOutputPlugin is IHyperpinPlugin interface implementation required by Hyperpin.
    /// </summary>
    [Export(typeof(IHyperpinPlugin))]
    public class DirectOutputPlugin : IHyperpinPlugin
    {






        #region IHyperpinPlugin Members

        /// <summary>
        /// Gets the name of this IHyperpinPlugin.<br/>
        /// \remark This property returns the version of the DirectOutput.dll, NOT the version of the DirectOutputPlugin plugin.
        /// </summary>
        /// <value>
        /// The name of this IHyperpinPlugin (Name is DirectOutput (V: VersionNumber) af of TimeStamp).
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
        /// This method is called, when Hyperpin sends new data to DirectOutput.<br/>
        /// HyperPin does only send a IDNumber and a value. These values are fed to DirectOutput as updates on EMTable elements (TableElementType character E).<br/>
        /// The IHyperpinPlugin interface requires the implementation of this method.
        /// </summary>
        /// <param name="Number">The received IdNumber.</param>
        /// <param name="Value">The received value.</param>
        public void DataReceive(int Number, int Value)
        {
   
                Pinball.ReceiveData('E', Number, Value);
            
        }




        /// <summary>
        /// Finishes the plugin.<br />
        /// This is the last method called, before a plugin is discared. This method is also called, after a undhandled exception has occured in a plugin.<br/>
        /// The IHyperpinPlugin interface requires the implementation of this method.<br/>
        /// </summary>
        public void PluginFinish()
        {
         
                Pinball.Finish();
      
        }

        /// <summary>
        /// Initializes the Plugin.<br/>
        /// The IHyperpinPlugin interface requires the implementation of this method.<br/>
        /// DirectOutput likes to receive a global config filename, a table file name and a romname when it is initialized. <br/>
        /// The global config file for the HyperPinPlugin is called GlobalConfig_Hyperpin.xml and should reside in the config subdirectory of the directory containing the DirectOutput.dll or there can be a shortcut named config pointing to the directory containg the config file.<br/>
        /// For the tablename the name of a file named Hyperpin.tmp located in the config dir of DirectOutput is supplied to DirectOutput. This file does/must not exist.<br/>
        /// For the RomName the value Hyperpin is provided to DOF to allow for configuratiojn through LedControl files.
        /// </summary>
        public void PluginInit()
        {

            //Check config dir for global config file
            FileInfo F = new FileInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "config", "GlobalConfig_Hyperpin.xml"));
            if (!F.Exists)
            {
                //Check if a shortcut to the config dir exists
                FileInfo LnkFile = new FileInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "config", "GlobalConfig_Hyperpin.lnk"));
                if (LnkFile.Exists)
                {
                    string ConfigDirPath = ResolveShortcut(LnkFile);
                    if (Directory.Exists(ConfigDirPath))
                    {
                        F = new FileInfo(Path.Combine(ConfigDirPath, "GlobalConfig_Hyperpin.xml"));
                    }
                }
               
            }

            Pinball.Init(F.FullName, Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "config", "Hyperpin.tmp"), "Hyperpin");

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
