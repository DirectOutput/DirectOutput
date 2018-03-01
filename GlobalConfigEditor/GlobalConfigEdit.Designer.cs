namespace GlobalConfigEditor
{
    partial class GlobalConfigEdit
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalConfigEdit));
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectIniFilePathButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.IniFilesPath = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.SelectCabinetConfigFileButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.CabinetFilename = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ClearLogOnSessionStart = new System.Windows.Forms.CheckBox();
            this.LoggingEnabled = new System.Windows.Forms.CheckBox();
            this.SelectLogFileButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.LogFilename = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LedWizDefaultMinCommandIntervalMs = new System.Windows.Forms.NumericUpDown();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.SelectIniFileDirectoryDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SelectCabinetConfigFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SelectLogFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SaveGlobalConfigDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenGlobalConfigDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.PacLedDefaultMinCommandIntervalMs = new System.Windows.Forms.NumericUpDown();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.Menu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LedWizDefaultMinCommandIntervalMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PacLedDefaultMinCommandIntervalMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // Menu
            // 
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(584, 24);
            this.Menu.TabIndex = 0;
            this.Menu.Text = "MainMenu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(584, 338);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.SelectIniFilePathButton);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.IniFilesPath);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(576, 312);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "IniFiles";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(431, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(139, 45);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(8, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(417, 124);
            this.label3.TabIndex = 3;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // SelectIniFilePathButton
            // 
            this.SelectIniFilePathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectIniFilePathButton.Location = new System.Drawing.Point(449, 147);
            this.SelectIniFilePathButton.Name = "SelectIniFilePathButton";
            this.SelectIniFilePathButton.Size = new System.Drawing.Size(121, 23);
            this.SelectIniFilePathButton.TabIndex = 2;
            this.SelectIniFilePathButton.Text = "Select Ini Files Path";
            this.SelectIniFilePathButton.UseVisualStyleBackColor = true;
            this.SelectIniFilePathButton.Click += new System.EventHandler(this.SelectIniFilePathButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ini Files Path:";
            // 
            // IniFilesPath
            // 
            this.IniFilesPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IniFilesPath.Location = new System.Drawing.Point(75, 149);
            this.IniFilesPath.Name = "IniFilesPath";
            this.IniFilesPath.Size = new System.Drawing.Size(368, 20);
            this.IniFilesPath.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Transparent;
            this.tabPage2.Controls.Add(this.pictureBox2);
            this.tabPage2.Controls.Add(this.SelectCabinetConfigFileButton);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.CabinetFilename);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(576, 312);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Cabinet Config";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(431, 6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(139, 45);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // SelectCabinetConfigFileButton
            // 
            this.SelectCabinetConfigFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectCabinetConfigFileButton.Location = new System.Drawing.Point(502, 72);
            this.SelectCabinetConfigFileButton.Name = "SelectCabinetConfigFileButton";
            this.SelectCabinetConfigFileButton.Size = new System.Drawing.Size(69, 23);
            this.SelectCabinetConfigFileButton.TabIndex = 8;
            this.SelectCabinetConfigFileButton.Text = "Select File";
            this.SelectCabinetConfigFileButton.UseVisualStyleBackColor = true;
            this.SelectCabinetConfigFileButton.Click += new System.EventHandler(this.SelectCabinetConfigFileButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Cabinet config file:";
            // 
            // CabinetFilename
            // 
            this.CabinetFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CabinetFilename.Location = new System.Drawing.Point(100, 74);
            this.CabinetFilename.Name = "CabinetFilename";
            this.CabinetFilename.Size = new System.Drawing.Size(396, 20);
            this.CabinetFilename.TabIndex = 6;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Transparent;
            this.tabPage3.Controls.Add(this.pictureBox3);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.ClearLogOnSessionStart);
            this.tabPage3.Controls.Add(this.LoggingEnabled);
            this.tabPage3.Controls.Add(this.SelectLogFileButton);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.LogFilename);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(576, 312);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Logging";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(431, 6);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(139, 45);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 9;
            this.pictureBox3.TabStop = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(12, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(556, 40);
            this.label4.TabIndex = 8;
            this.label4.Text = "Setting this option will clear the contents of your log file on every new start o" +
    "f the framework.";
            // 
            // ClearLogOnSessionStart
            // 
            this.ClearLogOnSessionStart.AutoSize = true;
            this.ClearLogOnSessionStart.Location = new System.Drawing.Point(12, 103);
            this.ClearLogOnSessionStart.Name = "ClearLogOnSessionStart";
            this.ClearLogOnSessionStart.Size = new System.Drawing.Size(143, 17);
            this.ClearLogOnSessionStart.TabIndex = 7;
            this.ClearLogOnSessionStart.Text = "Clear log on session start";
            this.ClearLogOnSessionStart.UseVisualStyleBackColor = true;
            // 
            // LoggingEnabled
            // 
            this.LoggingEnabled.AutoSize = true;
            this.LoggingEnabled.Location = new System.Drawing.Point(12, 34);
            this.LoggingEnabled.Name = "LoggingEnabled";
            this.LoggingEnabled.Size = new System.Drawing.Size(96, 17);
            this.LoggingEnabled.TabIndex = 6;
            this.LoggingEnabled.Text = "Enable logging";
            this.LoggingEnabled.UseVisualStyleBackColor = true;
            // 
            // SelectLogFileButton
            // 
            this.SelectLogFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectLogFileButton.Location = new System.Drawing.Point(452, 60);
            this.SelectLogFileButton.Name = "SelectLogFileButton";
            this.SelectLogFileButton.Size = new System.Drawing.Size(121, 23);
            this.SelectLogFileButton.TabIndex = 5;
            this.SelectLogFileButton.Text = "Select Log File";
            this.SelectLogFileButton.UseVisualStyleBackColor = true;
            this.SelectLogFileButton.Click += new System.EventHandler(this.SelectLogFileButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Log File:";
            // 
            // LogFilename
            // 
            this.LogFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogFilename.Location = new System.Drawing.Point(76, 62);
            this.LogFilename.Name = "LogFilename";
            this.LogFilename.Size = new System.Drawing.Size(368, 20);
            this.LogFilename.TabIndex = 3;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.LedWizDefaultMinCommandIntervalMs);
            this.tabPage4.Controls.Add(this.pictureBox4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(576, 312);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "LedWiz";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(271, 34);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "ms";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(10, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(560, 58);
            this.label7.TabIndex = 13;
            this.label7.Text = resources.GetString("label7.Text");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(191, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "LedWiz Default Min Command Interval:";
            // 
            // LedWizDefaultMinCommandIntervalMs
            // 
            this.LedWizDefaultMinCommandIntervalMs.Location = new System.Drawing.Point(207, 32);
            this.LedWizDefaultMinCommandIntervalMs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LedWizDefaultMinCommandIntervalMs.Name = "LedWizDefaultMinCommandIntervalMs";
            this.LedWizDefaultMinCommandIntervalMs.Size = new System.Drawing.Size(58, 20);
            this.LedWizDefaultMinCommandIntervalMs.TabIndex = 11;
            this.LedWizDefaultMinCommandIntervalMs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.LedWizDefaultMinCommandIntervalMs.ThousandsSeparator = true;
            this.LedWizDefaultMinCommandIntervalMs.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(431, 6);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(139, 45);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 10;
            this.pictureBox4.TabStop = false;
            // 
            // SelectIniFileDirectoryDialog
            // 
            this.SelectIniFileDirectoryDialog.Description = "Select the path where you ini files are stored";
            // 
            // SelectCabinetConfigFileDialog
            // 
            this.SelectCabinetConfigFileDialog.DefaultExt = "xml";
            this.SelectCabinetConfigFileDialog.Filter = "XML-Files (*.xml)|*.xml|All files (*.*)|*.*";
            this.SelectCabinetConfigFileDialog.Title = "Select your cabinet config file";
            // 
            // SelectLogFileDialog
            // 
            this.SelectLogFileDialog.DefaultExt = "log";
            this.SelectLogFileDialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
            this.SelectLogFileDialog.OverwritePrompt = false;
            this.SelectLogFileDialog.Title = "Select or enter a name for the log file";
            // 
            // SaveGlobalConfigDialog
            // 
            this.SaveGlobalConfigDialog.DefaultExt = "xml";
            this.SaveGlobalConfigDialog.Filter = "GlobalConfig files (GlobalConfig_*.xml)|GlobalConfig_*.xml|XML files (*.xml)|*.xm" +
    "l|All files (*.*)|*.*";
            this.SaveGlobalConfigDialog.Title = "Select or enter a name for your global config file";
            // 
            // OpenGlobalConfigDialog
            // 
            this.OpenGlobalConfigDialog.DefaultExt = "xml";
            this.OpenGlobalConfigDialog.Filter = "GlobalConfig files (GlobalConfig_*.xml)|GlobalConfig_*.xml|XML files (*.xml)|*.xm" +
    "l|All files (*.*)|*.*";
            this.OpenGlobalConfigDialog.Title = "Select the global config file to open";
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage5.Controls.Add(this.label9);
            this.tabPage5.Controls.Add(this.label10);
            this.tabPage5.Controls.Add(this.label11);
            this.tabPage5.Controls.Add(this.PacLedDefaultMinCommandIntervalMs);
            this.tabPage5.Controls.Add(this.pictureBox5);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(576, 312);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "PacLed";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(271, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "ms";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(10, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(560, 58);
            this.label10.TabIndex = 18;
            this.label10.Text = resources.GetString("label10.Text");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 34);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(192, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "PacLed Default Min Command Interval:";
            // 
            // PacLedDefaultMinCommandIntervalMs
            // 
            this.PacLedDefaultMinCommandIntervalMs.Location = new System.Drawing.Point(207, 32);
            this.PacLedDefaultMinCommandIntervalMs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PacLedDefaultMinCommandIntervalMs.Name = "PacLedDefaultMinCommandIntervalMs";
            this.PacLedDefaultMinCommandIntervalMs.Size = new System.Drawing.Size(58, 20);
            this.PacLedDefaultMinCommandIntervalMs.TabIndex = 16;
            this.PacLedDefaultMinCommandIntervalMs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.PacLedDefaultMinCommandIntervalMs.ThousandsSeparator = true;
            this.PacLedDefaultMinCommandIntervalMs.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // pictureBox5
            // 
            this.pictureBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(431, 6);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(139, 45);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 15;
            this.pictureBox5.TabStop = false;
            // 
            // GlobalConfigEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.Menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.Menu;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "GlobalConfigEdit";
            this.Text = "Global Configuration Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GlobalConfigEdit_FormClosing);
            this.Load += new System.EventHandler(this.GlobalConfigEdit_Load);
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LedWizDefaultMinCommandIntervalMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PacLedDefaultMinCommandIntervalMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private new System.Windows.Forms.MenuStrip Menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button SelectIniFilePathButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox IniFilesPath;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button SelectLogFileButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox LogFilename;
        private System.Windows.Forms.CheckBox LoggingEnabled;
        private System.Windows.Forms.CheckBox ClearLogOnSessionStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button SelectCabinetConfigFileButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox CabinetFilename;
        private System.Windows.Forms.FolderBrowserDialog SelectIniFileDirectoryDialog;
        private System.Windows.Forms.OpenFileDialog SelectCabinetConfigFileDialog;
        private System.Windows.Forms.SaveFileDialog SelectLogFileDialog;
        private System.Windows.Forms.SaveFileDialog SaveGlobalConfigDialog;
        private System.Windows.Forms.OpenFileDialog OpenGlobalConfigDialog;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown LedWizDefaultMinCommandIntervalMs;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown PacLedDefaultMinCommandIntervalMs;
        private System.Windows.Forms.PictureBox pictureBox5;
    }
}

