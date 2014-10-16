using System;
using System.Runtime.InteropServices;
using System.IO;

namespace PinballX
{

    /// <summary>
    /// This is the base calls for PinballX plugins.
    /// It contains the public methods which are called from PinballX when it is using the plugin.
    /// </summary>
    public class Plugin
    {



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

            return true;
        }

        /// <summary>
        /// The method is called when the configure button in the plugin manager of PinballX is clicked.
        /// Open configs windows in the method.
        /// </summary>
        public void Configure()
        {

        }


        /// <summary>
        /// This method is called when PinballX exits
        /// </summary>
        public void Event_App_Exit()
        {



        }



        /// <summary>
        /// This method is called when a game is selected in PinballX.
        /// </summary>
        /// <param name="InfoPtr">The InfoPTR to the GameInfo structure.</param>
        public void Event_GameSelect(IntPtr InfoPtr)
        {
            GameInfo Info = (GameInfo)Marshal.PtrToStructure(InfoPtr, typeof(GameInfo));

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
            GameInfo Info = (GameInfo)Marshal.PtrToStructure(InfoPtr, typeof(GameInfo));


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
            GameInfo Info = (GameInfo)Marshal.PtrToStructure(InfoPtr, typeof(GameInfo));
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

            //if (Input_Keys[System.Windows.Forms.Keys.LShiftKey])
            //{
            //    // Left Shift Key Down
            //}

            //if (Input_Buttons[0])
            //{
            //    // Gamepad buttton 1 down
            //}
            return true;
        }


        /// <summary>
        /// This event is called when PinballX starts or quits the screen saver. 
        /// </summary>
        /// <param name="Type">The type of the call (Start ScreenSaver=1, End Screensaver=2).</param>
        /// <returns>Return true to tell PinballX process the event. Return false to tell PinballX NOT to process the event</returns>
        public bool Event_ScreenSaver(int Type)
        {

            //Start = 1,
            //End = 2,
            switch (Type)
            {

                case 1:
                    break;

                case 2:

                    break;
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



