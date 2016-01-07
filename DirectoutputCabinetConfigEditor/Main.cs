using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.Cab;

namespace DirectoutputCabinetConfigEditor
{
    public partial class Main : Form
    {

        #region Cabinet property of type Cabinet with events
        #region Cabinet property core parts
        private Cabinet _Cabinet = null;

        /// <summary>
        ///  Cabinet property of type Cabinet
        /// </summary>
        public Cabinet Cabinet
        {
            get { return _Cabinet; }
            set
            {
                if (_Cabinet != value)
                {
                    OnCabinetChanging();
                    _Cabinet = value;
                    OnCabinetChanged();
                }
            }
        }

        /// <summary>
        /// Fires when the Cabinet property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> CabinetChanging;

        /// <summary>
        /// Fires when the Cabinet property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> CabinetChanged;
        #endregion

        /// <summary>
        /// Is called when the Cabinet property is about to change its value and fires the CabinetChanging event
        /// </summary>
        protected void OnCabinetChanging()
        {
            if (CabinetChanging != null) CabinetChanging(this, new EventArgs());

            //Insert more logic to execute before the Cabinet property changes here
        }

        /// <summary>
        /// Is called when the Cabinet property has changed its value and fires the CabinetChanged event
        /// </summary>
        protected void OnCabinetChanged()
        {
            //Insert more logic to execute after the Cabinet property has changed here

            if (CabinetChanged != null) CabinetChanged(this, new EventArgs());
        }

        #endregion
        public Main()
        {
            InitializeComponent();
            Cabinet = new Cabinet();
        }

        private void LoadCabinetConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadCabinetConfigDialog.ShowDialog(this) == DialogResult.OK)
            {
                Cabinet C;

                try
                {
                    C = Cabinet.GetCabinetFromConfigXmlFile(LoadCabinetConfigDialog.FileName);

                }
                catch (Exception E)
                {
                    MessageBox.Show("Cant load the cabinet configuration from file\n{0}.\nException Message:\n{0}".Build(LoadCabinetConfigDialog.FileName, string.Join(", ", E.GetNestedMessages())));
                    return;
                }

                Cabinet = C;

            }
        }



        
    }
}
