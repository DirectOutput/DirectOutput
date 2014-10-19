using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PinballX
{
    public partial class Configure : Form
    {

        public Configure(Config Config)
        {
            InitializeComponent();
            this.Config = Config;
            Populate();
        }

        public Configure()
            : this(new Config())
        {
        }




        public Config Config { get; set; }


        private void Populate()
        {

            PopulateDOFState();


        }


        private void PopulateDOFState()
        {
            DOFManager DM = new DOFManager();

            string DllPath = "";
            bool DOFLoaded = false;

            string DOFVersion = "";

            try
            {
                DM.Load();
                DOFLoaded = true;

            }
            catch
            {
                DOFLoaded = false;
            }


            if (DOFLoaded)
            {
                try
                {
                    DllPath = DM.GetDllPath();
                    DOFVersion = DM.GetVersion();
                }
                catch { }
            }


            try
            {
                DM.Unload();
            }
            catch { }

            DOFPathText.Text = DllPath;
            DOFVersionText.Text = DOFVersion;
            DOFStateText.Text = (DOFLoaded ? "OK. DirectOutput framework found" : "Error! DirectOutput framework not found. Make sure the DOF com object is registered.");


        }


    }
}
