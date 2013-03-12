using System.ComponentModel.Composition;
using System.IO;
using DirectOutput;
using DirectOutput.GlobalConfig;

/// <summary>
/// DirectOutputPlugin is the namespace of the Dll implementing the actual plugin interface for the B2S Server.
/// </summary> 
namespace DirectOutputPlugin
{

    /// <summary>
    /// Plugin is IDirectPlugin interface implementation required by the B2S Server.
    /// </summary>
    [Export(typeof(B2S.IDirectPlugin))]
    public class Plugin:B2S.IDirectPlugin, B2S.IDirectPluginFrontend
    {

        
        #region IDirectPluginFrontEnd Member


        /// <summary>
        /// Shows the FrontEnd of the Plugin.<br/>
        /// The IDirectPluginFrontend interface requires the implementation of this method.
        /// </summary>
        public void PluginShowFrontend()
        {
            GlobalConfigEditor E = new GlobalConfigEditor();
            E.Show();
        }

        #endregion


        #region IDirectPlugin Members

        /// <summary>
        /// Gets the name of this IDirectPlugin.
        /// </summary>
        /// <value>
        /// The name of this IDirectPlugin (Name is DirectOutput (V: VersionNumber)).
        /// </value>
        public string Name
        {
            get
            {
                return "DirectOutput (V: {0})".Build(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            }
        }


        /// <summary>
        /// This method is called, when the property Pause of Pinmame gets set to false.<br/>
        /// The IDirectPlugin interface requires the implementation of this method.<br/>
        /// DirectOutput implements only a method stub for this method.
        /// </summary>
        public void PinMameContinue() {}

        /// <summary>
        /// This method is called, when new data from Pinmame becomes available.<br/>
        /// The IDirectPlugin interface requires the implementation of this method.
        /// </summary>
        /// <param name="TableElementTypeChar">Char representing the table element type (S=Solenoid, W=Switch, L=Lamp, M=Mech, G=GI).</param>
        /// <param name="Number">The number of the table element.</param>
        /// <param name="Value">The value of the table element.</param>
        public void PinMameDataReceive(char TableElementTypeChar, int Number, int Value)
        {
            Pinball.ReceivePinmameData(TableElementTypeChar, Number, Value);
        }

        /// <summary>
        /// This method is called, when the property Pause of Pinmame gets set to true.
        /// The IDirectPlugin interface requires the implementation of this method.<br/>
        /// DirectOutput implements only a method stub for this method.        /// </summary>
        public void PinMamePause() { }

        /// <summary>
        /// This method is called, when the Run method of PinMame gets called.
        /// The IDirectPlugin interface requires the implementation of this method.<br/>
        /// DirectOutput implements only a method stub for this method.        /// </summary>
        public void PinMameRun() { }

        /// <summary>
        /// This method is called, when the Stop method of Pinmame is called.
        /// The IDirectPlugin interface requires the implementation of this method.<br/>
        /// DirectOutput implements only a method stub for this method.
        /// </summary>
        public void PinMameStop() { }


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
            Pinball.Init(new FileInfo(TableFilename), RomName);
        }

        #endregion

        #region Properties

        

        private Pinball _Pinball=new Pinball();

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
