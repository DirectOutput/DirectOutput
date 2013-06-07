using System;
using System.IO;
using System.Windows.Forms;
using DirectOutput.LedControl;

namespace LedControlFileTester
{
    /// <summary>
    /// This is the main form of the LedControlFileTester application. <br/>
    /// It contains all functionality of the tester.
    /// </summary>
    public partial class LedControlFileTestWizard : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LedControlFileTestWizard"/> class.
        /// </summary>
        public LedControlFileTestWizard()
        {
            InitializeComponent();
        }

        private void SelectLedControlFile_Click(object sender, EventArgs e)
        {
            string Filename="";
            if (OpenLedControlFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                     Filename = OpenLedControlFile.FileName;
                    LedControlFileName.Text = Filename;

                    ParsingResults.Rows.Clear();

                    string TempLogFile = Path.GetTempFileName();
                    DirectOutput.Log.Filename = TempLogFile;
                    DirectOutput.Log.Init();

                    LedControlConfig L = new LedControlConfig(Filename, 1);

                    DirectOutput.Log.Finish();

                    string LedControlLoadingLog = DirectOutput.General.FileReader.ReadFileToString(TempLogFile);


                    File.Delete(TempLogFile);

                    foreach (string LogLine in LedControlLoadingLog.Split(new[] { '\r', '\n' }))
                    {
                        if (LogLine.Split('\t').Length > 1)
                        {
                            int RowIndex = ParsingResults.Rows.Add();
                            ParsingResults[0, RowIndex].Value = LogLine.Split('\t')[0];
                            ParsingResults[1, RowIndex].Value = LogLine.Split('\t')[1];
                        }
                    }

                }
                catch (Exception E)
                {
                    ParsingResults.Rows.Clear();
                    MessageBox.Show("A error has occured when trying to load and parse the ledcontrol file: \n{0}\n\nException:\n{1}".Build(Filename, E.Message));
                }
                

            }
        }
    }
}
