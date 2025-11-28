using System;
using System.IO;
using System.Windows.Forms;
using DirectOutput.LedControl.Loader;
using System.Threading;

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


        public LedControlFileTestWizard(string Filename):this()
        {
            if (CheckLedControlFile(Filename))
            {
                Console.WriteLine("LedControl file {0} seems to be ok.", Filename);
                this.Refresh();
                
                Thread.Sleep(1500);
                this.Close();
            }
            else
            {
                Console.WriteLine("WARNING: Found some issues in ledcontrol file: {0}", Filename);
            }
        }



        private void SelectLedControlFile_Click(object sender, EventArgs e)
        {
            string Filename = "";
            if (OpenLedControlFile.ShowDialog() == DialogResult.OK)
            {
                    Filename = OpenLedControlFile.FileName;
                    CheckLedControlFile(Filename);



            }
        }

        private bool CheckLedControlFile(string Filename)
        {
            try
            {
                bool OK = true;
                LedControlFileName.Text = Filename;

                ParsingResults.Rows.Clear();

                string TempLogFile = Path.GetTempFileName();
                DirectOutput.Log.Filename = TempLogFile;
                DirectOutput.Log.Init();

                LedControlConfig L = new LedControlConfig(Filename, 1, string.Empty, null);

                DirectOutput.Log.Finish();

                string LedControlLoadingLog = DirectOutput.General.FileReader.ReadFileToString(TempLogFile);


                File.Delete(TempLogFile);

                foreach (string LogLine in LedControlLoadingLog.Split(new[] { '\r', '\n' }))
                {
                    string[] Parts = LogLine.Split('\t');
                    if (Parts.Length > 1)
                    {
                        if (Parts[1].ToLowerInvariant().Contains("exception") || Parts[1].ToLowerInvariant().Contains("warning") || Parts[1].ToLowerInvariant().Contains("error"))
                        {
                            OK = false;
                        }

                        int RowIndex = ParsingResults.Rows.Add();
                        ParsingResults[0, RowIndex].Value = Parts[0];
                        ParsingResults[1, RowIndex].Value = Parts[1];
                    }
                }
                return OK;

            }
            catch (Exception E)
            {
                ParsingResults.Rows.Clear();
                MessageBox.Show("A error has occurred when trying to load and parse the ledcontrol file: \n{0}\n\nException:\n{1}".Build(Filename, E.Message));
                return false;
            }
        }

 
    }
}
