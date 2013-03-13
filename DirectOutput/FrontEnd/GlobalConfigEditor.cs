using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.GlobalConfig;
using System.IO;
using System.Reflection;

namespace DirectOutput.Frontend
{
    public partial class GlobalConfigEditor : Form
    {


        private bool _Modified = false;

        public bool Modified
        {
            get { return _Modified; }
            set
            {
                _Modified = value;
                UpdateFormTitle();
            }
        }


        private Config _GlobalConfig = new Config();

        public Config GlobalConfig
        {
            get { return _GlobalConfig; }
            set { _GlobalConfig = value; }
        }

        private string _GlobalConfigFilename = "";

        public string GlobalConfigFilename
        {
            get { return _GlobalConfigFilename; }
            set { _GlobalConfigFilename = value; }
        }


        private void GlobalConfigEditor_Load(object sender, EventArgs e)
        {
            Init();
        }


        private void Init()
        {


            TableConfigFilePattern.DataBindings.Clear();
            TableConfigFilePattern.DataBindings.Add(new Binding("Text", GlobalConfig.TableConfigFilePattern, "Pattern", true, DataSourceUpdateMode.OnPropertyChanged));
            CabinetConfigFilePattern.DataBindings.Clear();
            CabinetConfigFilePattern.DataBindings.Add(new Binding("Text", GlobalConfig.CabinetConfigFilePattern, "Pattern", true, DataSourceUpdateMode.OnPropertyChanged));

            LedControlIniFilesBindingSource.DataSource = GlobalConfig;
            CabinetScriptFilePatternsBindingSource.DataSource = GlobalConfig;
            TableScriptFilePatternsBindingSource.DataSource = GlobalConfig;
            RemoveLedControlIniFile.Enabled = (GlobalConfig.LedControlIniFiles.Count != 0);
            Modified = false;
            UpdateFormTitle();
        }

        private void UpdateFormTitle()
        {
            this.Text = "Global Configuration Editor{1}{0}".Build((!GlobalConfigFilename.IsNullOrEmpty() ? ": {0}".Build(GlobalConfigFilename) : ""), (Modified ? " (not saved)" : ""));
            GlobalConfigEditorStatus.Text = "{1}{0}".Build(GlobalConfigFilename, (Modified ? " (not saved)" : ""));
        }

        #region Drag Drop for LedControlIniFiles control

        //Code from http://stackoverflow.com/questions/1620947/how-could-i-drag-and-drop-datagridview-rows-under-each-other
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;

        private void LedControlIniFiles_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = LedControlIniFiles.DoDragDrop(
                    LedControlIniFiles.Rows[rowIndexFromMouseDown],
                    DragDropEffects.Move);
                }
            }
        }

        private void LedControlIniFiles_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = LedControlIniFiles.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void LedControlIniFiles_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void LedControlIniFiles_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = LedControlIniFiles.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop =
                LedControlIniFiles.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                DataGridViewRow rowToMove = e.Data.GetData(
                    typeof(DataGridViewRow)) as DataGridViewRow;

                LedControlIniFileList L = new LedControlIniFileList();
                int Cnt = 0;
                int Number = 1;
                foreach (LedControlIniFile I in GlobalConfig.LedControlIniFiles)
                {
                    if (Cnt != rowIndexFromMouseDown)
                    {
                        I.LedWizNumber = Number;
                        L.Add(I);
                        Number++;
                    }
                    if (Cnt == rowIndexOfItemUnderMouseToDrop)
                    {
                        LedControlIniFile I1 = GlobalConfig.LedControlIniFiles[rowIndexFromMouseDown];
                        I1.LedWizNumber = Number;
                        L.Add(I1);
                    }
                    Cnt++;
                }
                GlobalConfig.LedControlIniFiles = L;
                this.LedControlIniFiles.DataSource = L;
                this.LedControlIniFiles.Refresh();

                Modified = true;

            }
        }




        #endregion



        #region LedControl.ini files

        private void LedControlIniFiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex.IsBetween(0, GlobalConfig.LedControlIniFiles.Count - 1))
            {
                this.LedControlIniFileDialog.FileName = GlobalConfig.LedControlIniFiles[e.RowIndex].Filename;
                if (this.LedControlIniFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (GlobalConfig.LedControlIniFiles[e.RowIndex].Filename != this.LedControlIniFileDialog.FileName)
                    {
                        Modified = true;
                    }

                    GlobalConfig.LedControlIniFiles[e.RowIndex].Filename = this.LedControlIniFileDialog.FileName;


                }
            }
        }



        private void AddLedControlIniFile_Click(object sender, EventArgs e)
        {
            if (GlobalConfig.LedControlIniFiles.Count == 0)
            {
                if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ledcontrol.ini")))
                {
                    this.LedControlIniFileDialog.FileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ledcontrol.ini");
                    this.LedControlIniFileDialog.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                }
                else
                {
                    this.LedControlIniFileDialog.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                }
            }
            else
            {
                this.LedControlIniFileDialog.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            if (this.LedControlIniFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                GlobalConfig.LedControlIniFiles.Add(this.LedControlIniFileDialog.FileName);
                Modified = true;
            }
            RemoveLedControlIniFile.Enabled = (GlobalConfig.LedControlIniFiles.Count != 0);
        }

        private void RemoveLedControlIniFile_Click(object sender, EventArgs e)
        {
            if (GlobalConfig.LedControlIniFiles.Count > 0)
            {
                if (MessageBox.Show(this, "Do you really want to remove the following file: \n{0}".Build(GlobalConfig.LedControlIniFiles[this.LedControlIniFiles.CurrentRow.Index].Filename), "Remove ledcontrol.ini file?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    GlobalConfig.LedControlIniFiles.RemoveAt(this.LedControlIniFiles.CurrentRow.Index);
                    GlobalConfig.LedControlIniFiles.Renumber();
                    Modified = true;
                }
            }
            RemoveLedControlIniFile.Enabled = (GlobalConfig.LedControlIniFiles.Count != 0);
        }

        #endregion

        private void InsertPlaceholderFromContextMenu(ToolStripItem MenuItem)
        {

            ContextMenuStrip Owner = MenuItem.Owner as ContextMenuStrip;
            if (Owner != null)
            {
                Control C = Owner.SourceControl;

                string T = (string)MenuItem.Tag;
                if (!T.IsNullOrWhiteSpace())
                {
                    if (C is TextBox)
                    {
                        int Position;
                        TextBox TB = (TextBox)C;
                        if (TB.Focused)
                        {
                            Position = TB.SelectionStart;
                        }
                        else
                        {
                            Position = TB.Text.Length;
                        }
                        TB.Text = TB.Text.Insert(Position, T);
                        TB.Focus();
                        TB.SelectionStart = Position + T.Length;
                        Modified = true;
                    }
                }
            }
        }


        private void CabinetConfigFileContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            InsertPlaceholderFromContextMenu(e.ClickedItem);
        }

        private void TableConfigFileContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            InsertPlaceholderFromContextMenu(e.ClickedItem);
        }

        private void CabinetConfigFilePattern_TextChanged(object sender, EventArgs e)
        {
            //            GlobalConfig.TableConfigFilePattern.Pattern = TableConfigFilePattern.Text;
            Modified = true;
        }
        private void TableConfigFilePattern_Validating(object sender, CancelEventArgs e)
        {
            if (new FilePattern(TableConfigFilePattern.Text).IsValid)
            {
                TableConfigFilePatternErrorProvider.SetError(TableConfigFilePatternLabel, String.Empty);

            }
            else
            {
                TableConfigFilePatternErrorProvider.SetError(TableConfigFilePatternLabel, "Invalid file pattern");
                e.Cancel = true;
            }
        }

        private void CabinetConfigFilePattern_Validating(object sender, CancelEventArgs e)
        {
            if (new FilePattern(CabinetConfigFilePattern.Text).IsValid)
            {
                TableConfigFilePatternErrorProvider.SetError(CabinetConfigFilePatternLabel, String.Empty);
            }
            else
            {
                TableConfigFilePatternErrorProvider.SetError(CabinetConfigFilePatternLabel, "Invalid file pattern");
                e.Cancel = true;
            }

        }



        private void TableConfigFilePattern_TextChanged(object sender, EventArgs e)
        {
            //GlobalConfig.CabinetConfigFilePattern.Pattern = CabinetConfigFilePattern.Text;
            Modified = true;
        }
        private void CabinetScriptFilePatternsBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new FilePattern();
            Modified = true;
        }

        private void TableScriptFilePatternsBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new FilePattern();
            Modified = true;
        }

        private void TableScriptFilePatterns_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (TableScriptFilePatterns.Rows[e.RowIndex].IsNewRow) { return; }

            if (new FilePattern(e.FormattedValue.ToString()).IsValid)
            {
                TableScriptFilePatterns.Rows[e.RowIndex].ErrorText = "";

            }
            else
            {
                TableScriptFilePatterns.Rows[e.RowIndex].ErrorText = "Invalid file pattern";
                e.Cancel = true;
            }


        }

        private void CabinetScriptFilePatterns_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (CabinetScriptFilePatterns.Rows[e.RowIndex].IsNewRow) { return; }

            if (new FilePattern(e.FormattedValue.ToString()).IsValid)
            {
                CabinetScriptFilePatterns.Rows[e.RowIndex].ErrorText = "";

            }
            else
            {
                CabinetScriptFilePatterns.Rows[e.RowIndex].ErrorText = "Invalid file pattern";
                e.Cancel = true;
            }
        }

        #region Menu
        private void MenuSave_Click(object sender, EventArgs e)
        {
            GlobalConfig.SaveGlobalConfig();
            Modified = false;
        }

        private void MenuClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string LastPath = "";
        private void MenuSaveAlternate_Click(object sender, EventArgs e)
        {
            string Path = (LastPath.IsNullOrWhiteSpace() ? Config.GlobalConfigDirectoryName : LastPath);

            SaveGlobalConfigDialog.InitialDirectory = Path;
            if (SaveGlobalConfigDialog.ShowDialog() == DialogResult.OK)
            {
                GlobalConfig.SaveGlobalConfig(SaveGlobalConfigDialog.FileName);
                LastPath = new FileInfo(SaveGlobalConfigDialog.FileName).Directory.FullName;
                Modified = false;
            }

        }

        private void MenuOpen_Click(object sender, EventArgs e)
        {
            if (Modified)
            {
                if (MessageBox.Show("The global config editor contains unsaved changes.\nIf you load the default global config file, those changes will be lost.\nDo you want to proceed?", "Unsaved changes", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                {
                    return;
                }
            }
            Config GC = Config.GetGlobalConfigFromConfigXmlFile();
            if (GC != null)
            {
                GlobalConfig = GC;
                GlobalConfigFilename = Config.GlobalConfigFilename;
                Init();
                MessageBox.Show("Default global config file loaded from \n{0}".Build(Config.GlobalConfigFilename), "Global config loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Could not load global config from file\n{0}.".Build(Config.GlobalConfigFilename), "Global config load error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void MenuOpenAlternate_Click(object sender, EventArgs e)
        {
            if (Modified)
            {
                if (MessageBox.Show("The global config editor contains unsaved changes.\nIf you load another global config file, those changes will be lost.\nDo you want to proceed?", "Unsaved changes", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                {
                    return;
                }
            }
            string Path = (LastPath.IsNullOrWhiteSpace() ? Config.GlobalConfigDirectoryName : LastPath);
            OpenGlobalConfigDialog.InitialDirectory = Path;
            if (OpenGlobalConfigDialog.ShowDialog(this) == DialogResult.OK)
            {
                LastPath = new FileInfo(OpenGlobalConfigDialog.FileName).Directory.FullName;
                Config GC = Config.GetGlobalConfigFromConfigXmlFile(OpenGlobalConfigDialog.FileName);
                if (GC != null)
                {
                    GlobalConfig = GC;
                    GlobalConfigFilename = OpenGlobalConfigDialog.FileName;
                    Init();
                    MessageBox.Show("Global config file loaded from \n{0}".Build(Config.GlobalConfigFilename), "Global config loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Could not load global config from\n{0}".Build(Config.GlobalConfigFilename), "Global config load error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }     
        private void MenuNew_Click(object sender, EventArgs e)
        {
            if (Modified)
            {
                if (MessageBox.Show("The global config editor contains unsaved changes.\nDo you want to proceed?", "Unsaved changes", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                {
                    return;
                }
                GlobalConfig = new Config();
                Modified = false;
                GlobalConfigFilename = "";
                Init();
            }
        }
        #endregion

        private void GlobalConfigEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                switch (MessageBox.Show("The settings in this form have been modified.\nIf you close the fowm all changes will be lost.\nDo you want to save your changes?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Yes:
                        GlobalConfig.SaveGlobalConfig();
                        MessageBox.Show("Config saved to {0}.".Build(Config.GlobalConfigFilename), "Changes saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void TableScriptFilePatterns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Modified = true;
        }

        private void CabinetScriptFilePatterns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Modified = true;

        }

        public GlobalConfigEditor()
            : this(new Config())
        { ; }

        public GlobalConfigEditor(string GlobalConfigFilename)
            : this(Config.GetGlobalConfigFromConfigXmlFile(GlobalConfigFilename))
        { this.GlobalConfigFilename = GlobalConfigFilename; }

        public GlobalConfigEditor(bool LoadDefaultGlobalConfig)
            : this(Config.GetGlobalConfigFromConfigXmlFile())
        { }

        public GlobalConfigEditor(Config GlobalConfig)
        {
            InitializeComponent();
            this.GlobalConfig = GlobalConfig;
            this.GlobalConfigFilename = "";
        }





    }
}
