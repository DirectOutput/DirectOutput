namespace LedControlConverter
{
    partial class ConverterInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConverterInput));
            this.FinishButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SelectInputFiles = new System.Windows.Forms.OpenFileDialog();
            this.SelectCabinetConfigFile = new System.Windows.Forms.OpenFileDialog();
            this.Wizard = new System.Windows.Forms.TablessTabControl();
            this.InputFileSelect = new System.Windows.Forms.TabPage();
            this.AddLedControlFilesButton = new System.Windows.Forms.Button();
            this.InputFiles = new System.Windows.Forms.DataGridView();
            this.InputFilesFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InputFilesLedwizNr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RomSelect = new System.Windows.Forms.TabPage();
            this.UnselectAllConfigsButton = new System.Windows.Forms.Button();
            this.SelectAllConfigsButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ConfigsToConvert = new System.Windows.Forms.CheckedListBox();
            this.CabinetConfigFile = new System.Windows.Forms.TabPage();
            this.CabinetConfigFilenameLabel = new System.Windows.Forms.Label();
            this.CabinetConfigFilename = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CabinetConfigMode = new System.Windows.Forms.ComboBox();
            this.DefineOutput = new System.Windows.Forms.TabPage();
            this.Finish = new System.Windows.Forms.TabPage();
            this.SaveCabinetConfigFile = new System.Windows.Forms.SaveFileDialog();
            this.Wizard.SuspendLayout();
            this.InputFileSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InputFiles)).BeginInit();
            this.RomSelect.SuspendLayout();
            this.CabinetConfigFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // FinishButton
            // 
            this.FinishButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FinishButton.Location = new System.Drawing.Point(644, 522);
            this.FinishButton.Name = "FinishButton";
            this.FinishButton.Size = new System.Drawing.Size(135, 28);
            this.FinishButton.TabIndex = 1;
            this.FinishButton.Text = "Finish";
            this.FinishButton.UseVisualStyleBackColor = true;
            this.FinishButton.Click += new System.EventHandler(this.FinishButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NextButton.Location = new System.Drawing.Point(505, 522);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(122, 28);
            this.NextButton.TabIndex = 2;
            this.NextButton.Text = "Next >";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // BackButton
            // 
            this.BackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BackButton.Location = new System.Drawing.Point(377, 522);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(122, 28);
            this.BackButton.TabIndex = 3;
            this.BackButton.Text = "< Back";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CancelButton.Location = new System.Drawing.Point(5, 521);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(122, 29);
            this.CancelButton.TabIndex = 4;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SelectInputFiles
            // 
            this.SelectInputFiles.DefaultExt = "ini";
            this.SelectInputFiles.Filter = "*.ini (inifiles)|*.ini";
            this.SelectInputFiles.Multiselect = true;
            this.SelectInputFiles.Title = "Select ledcontrol.ini files";
            // 
            // SelectCabinetConfigFile
            // 
            this.SelectCabinetConfigFile.DefaultExt = "xml";
            this.SelectCabinetConfigFile.Filter = "*.xml (XML configs)|*.xml|*.* (All files)|*.*";
            this.SelectCabinetConfigFile.Title = "Select cabinet config file";
            // 
            // Wizard
            // 
            this.Wizard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Wizard.Controls.Add(this.InputFileSelect);
            this.Wizard.Controls.Add(this.RomSelect);
            this.Wizard.Controls.Add(this.CabinetConfigFile);
            this.Wizard.Controls.Add(this.DefineOutput);
            this.Wizard.Controls.Add(this.Finish);
            this.Wizard.Location = new System.Drawing.Point(1, 2);
            this.Wizard.Name = "Wizard";
            this.Wizard.SelectedIndex = 0;
            this.Wizard.Size = new System.Drawing.Size(782, 517);
            this.Wizard.TabIndex = 0;
            this.Wizard.TabStop = false;
            this.Wizard.TabsVisible = false;
            // 
            // InputFileSelect
            // 
            this.InputFileSelect.BackColor = System.Drawing.Color.Transparent;
            this.InputFileSelect.Controls.Add(this.AddLedControlFilesButton);
            this.InputFileSelect.Controls.Add(this.InputFiles);
            this.InputFileSelect.Location = new System.Drawing.Point(4, 22);
            this.InputFileSelect.Name = "InputFileSelect";
            this.InputFileSelect.Padding = new System.Windows.Forms.Padding(3);
            this.InputFileSelect.Size = new System.Drawing.Size(774, 491);
            this.InputFileSelect.TabIndex = 0;
            this.InputFileSelect.Text = "Select Input Files";
            // 
            // AddLedControlFilesButton
            // 
            this.AddLedControlFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddLedControlFilesButton.Location = new System.Drawing.Point(7, 446);
            this.AddLedControlFilesButton.Name = "AddLedControlFilesButton";
            this.AddLedControlFilesButton.Size = new System.Drawing.Size(764, 39);
            this.AddLedControlFilesButton.TabIndex = 1;
            this.AddLedControlFilesButton.Text = "Add Ledcontrol files";
            this.AddLedControlFilesButton.UseVisualStyleBackColor = true;
            this.AddLedControlFilesButton.Click += new System.EventHandler(this.AddLedControlFilesButton_Click);
            // 
            // InputFiles
            // 
            this.InputFiles.AllowDrop = true;
            this.InputFiles.AllowUserToAddRows = false;
            this.InputFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InputFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.InputFilesFilename,
            this.InputFilesLedwizNr});
            this.InputFiles.Location = new System.Drawing.Point(7, 6);
            this.InputFiles.Name = "InputFiles";
            this.InputFiles.Size = new System.Drawing.Size(764, 434);
            this.InputFiles.TabIndex = 0;
            this.InputFiles.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.InputFiles_UserDeletedRow);
            this.InputFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.InputFiles_DragDrop);
            this.InputFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.InputFiles_DragEnter);
            // 
            // InputFilesFilename
            // 
            this.InputFilesFilename.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InputFilesFilename.FillWeight = 90F;
            this.InputFilesFilename.HeaderText = "File";
            this.InputFilesFilename.Name = "InputFilesFilename";
            // 
            // InputFilesLedwizNr
            // 
            this.InputFilesLedwizNr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InputFilesLedwizNr.FillWeight = 10F;
            this.InputFilesLedwizNr.HeaderText = "LedWiz Number";
            this.InputFilesLedwizNr.Name = "InputFilesLedwizNr";
            // 
            // RomSelect
            // 
            this.RomSelect.BackColor = System.Drawing.Color.Transparent;
            this.RomSelect.Controls.Add(this.UnselectAllConfigsButton);
            this.RomSelect.Controls.Add(this.SelectAllConfigsButton);
            this.RomSelect.Controls.Add(this.label1);
            this.RomSelect.Controls.Add(this.ConfigsToConvert);
            this.RomSelect.Location = new System.Drawing.Point(4, 22);
            this.RomSelect.Name = "RomSelect";
            this.RomSelect.Padding = new System.Windows.Forms.Padding(3);
            this.RomSelect.Size = new System.Drawing.Size(774, 491);
            this.RomSelect.TabIndex = 1;
            this.RomSelect.Text = "Select Roms";
            // 
            // UnselectAllConfigsButton
            // 
            this.UnselectAllConfigsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.UnselectAllConfigsButton.Location = new System.Drawing.Point(88, 462);
            this.UnselectAllConfigsButton.Name = "UnselectAllConfigsButton";
            this.UnselectAllConfigsButton.Size = new System.Drawing.Size(75, 23);
            this.UnselectAllConfigsButton.TabIndex = 3;
            this.UnselectAllConfigsButton.Text = "Unselect all";
            this.UnselectAllConfigsButton.UseVisualStyleBackColor = true;
            this.UnselectAllConfigsButton.Click += new System.EventHandler(this.UnselectAllConfigsButton_Click);
            // 
            // SelectAllConfigsButton
            // 
            this.SelectAllConfigsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectAllConfigsButton.Location = new System.Drawing.Point(7, 462);
            this.SelectAllConfigsButton.Name = "SelectAllConfigsButton";
            this.SelectAllConfigsButton.Size = new System.Drawing.Size(75, 23);
            this.SelectAllConfigsButton.TabIndex = 2;
            this.SelectAllConfigsButton.Text = "Select all";
            this.SelectAllConfigsButton.UseVisualStyleBackColor = true;
            this.SelectAllConfigsButton.Click += new System.EventHandler(this.SelectAllConfigsButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Configs to convert:";
            // 
            // ConfigsToConvert
            // 
            this.ConfigsToConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConfigsToConvert.CheckOnClick = true;
            this.ConfigsToConvert.FormattingEnabled = true;
            this.ConfigsToConvert.Location = new System.Drawing.Point(7, 26);
            this.ConfigsToConvert.MultiColumn = true;
            this.ConfigsToConvert.Name = "ConfigsToConvert";
            this.ConfigsToConvert.Size = new System.Drawing.Size(760, 424);
            this.ConfigsToConvert.Sorted = true;
            this.ConfigsToConvert.TabIndex = 0;
            this.ConfigsToConvert.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ConfigsToConvert_ItemCheck);
            // 
            // CabinetConfigFile
            // 
            this.CabinetConfigFile.BackColor = System.Drawing.Color.Transparent;
            this.CabinetConfigFile.Controls.Add(this.CabinetConfigFilenameLabel);
            this.CabinetConfigFile.Controls.Add(this.CabinetConfigFilename);
            this.CabinetConfigFile.Controls.Add(this.label2);
            this.CabinetConfigFile.Controls.Add(this.CabinetConfigMode);
            this.CabinetConfigFile.Location = new System.Drawing.Point(4, 22);
            this.CabinetConfigFile.Name = "CabinetConfigFile";
            this.CabinetConfigFile.Padding = new System.Windows.Forms.Padding(3);
            this.CabinetConfigFile.Size = new System.Drawing.Size(774, 491);
            this.CabinetConfigFile.TabIndex = 4;
            this.CabinetConfigFile.Text = "Cabinet Configuraion File";
            // 
            // CabinetConfigFilenameLabel
            // 
            this.CabinetConfigFilenameLabel.AutoSize = true;
            this.CabinetConfigFilenameLabel.Location = new System.Drawing.Point(7, 46);
            this.CabinetConfigFilenameLabel.Name = "CabinetConfigFilenameLabel";
            this.CabinetConfigFilenameLabel.Size = new System.Drawing.Size(94, 13);
            this.CabinetConfigFilenameLabel.TabIndex = 3;
            this.CabinetConfigFilenameLabel.Text = "Cabinet config file:";
            // 
            // CabinetConfigFilename
            // 
            this.CabinetConfigFilename.AllowDrop = true;
            this.CabinetConfigFilename.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CabinetConfigFilename.Location = new System.Drawing.Point(162, 43);
            this.CabinetConfigFilename.Name = "CabinetConfigFilename";
            this.CabinetConfigFilename.ReadOnly = true;
            this.CabinetConfigFilename.Size = new System.Drawing.Size(603, 20);
            this.CabinetConfigFilename.TabIndex = 2;
            this.CabinetConfigFilename.Click += new System.EventHandler(this.CabinetConfigFilename_Click);
            this.CabinetConfigFilename.DragDrop += new System.Windows.Forms.DragEventHandler(this.CabinetConfigFilename_DragDrop);
            this.CabinetConfigFilename.DragEnter += new System.Windows.Forms.DragEventHandler(this.CabinetConfigFilename_DragEnter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Cabinet configuration mode:";
            // 
            // CabinetConfigMode
            // 
            this.CabinetConfigMode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CabinetConfigMode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CabinetConfigMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CabinetConfigMode.FormattingEnabled = true;
            this.CabinetConfigMode.Items.AddRange(new object[] {
            "Create new cabinet config file",
            "Use existing cabinet config file"});
            this.CabinetConfigMode.Location = new System.Drawing.Point(162, 16);
            this.CabinetConfigMode.Name = "CabinetConfigMode";
            this.CabinetConfigMode.Size = new System.Drawing.Size(241, 21);
            this.CabinetConfigMode.TabIndex = 0;
            this.CabinetConfigMode.SelectedIndexChanged += new System.EventHandler(this.CabinetConfigMode_SelectedIndexChanged);
            // 
            // DefineOutput
            // 
            this.DefineOutput.BackColor = System.Drawing.Color.Transparent;
            this.DefineOutput.Location = new System.Drawing.Point(4, 22);
            this.DefineOutput.Name = "DefineOutput";
            this.DefineOutput.Padding = new System.Windows.Forms.Padding(3);
            this.DefineOutput.Size = new System.Drawing.Size(774, 491);
            this.DefineOutput.TabIndex = 2;
            this.DefineOutput.Text = "Define Output";
            // 
            // Finish
            // 
            this.Finish.BackColor = System.Drawing.Color.Transparent;
            this.Finish.Location = new System.Drawing.Point(4, 22);
            this.Finish.Name = "Finish";
            this.Finish.Padding = new System.Windows.Forms.Padding(3);
            this.Finish.Size = new System.Drawing.Size(774, 491);
            this.Finish.TabIndex = 3;
            this.Finish.Text = "Finish";
            // 
            // SaveCabinetConfigFile
            // 
            this.SaveCabinetConfigFile.DefaultExt = "xml";
            this.SaveCabinetConfigFile.Filter = "*.xml (XML configs)|*.xml|*.* (All files)|*.*";
            this.SaveCabinetConfigFile.Title = "Create new cabinet config file";
            // 
            // ConverterInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.FinishButton);
            this.Controls.Add(this.Wizard);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "ConverterInput";
            this.Text = "LedControl converter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConverterImput_FormClosing);
            this.Wizard.ResumeLayout(false);
            this.InputFileSelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InputFiles)).EndInit();
            this.RomSelect.ResumeLayout(false);
            this.RomSelect.PerformLayout();
            this.CabinetConfigFile.ResumeLayout(false);
            this.CabinetConfigFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TablessTabControl Wizard;
        private System.Windows.Forms.TabPage InputFileSelect;
        private System.Windows.Forms.TabPage RomSelect;
        private System.Windows.Forms.Button AddLedControlFilesButton;
        private System.Windows.Forms.DataGridView InputFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputFilesFilename;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputFilesLedwizNr;
        private System.Windows.Forms.TabPage DefineOutput;
        private System.Windows.Forms.TabPage Finish;
        private System.Windows.Forms.Button FinishButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.OpenFileDialog SelectInputFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox ConfigsToConvert;
        private System.Windows.Forms.Button UnselectAllConfigsButton;
        private System.Windows.Forms.Button SelectAllConfigsButton;
        private System.Windows.Forms.TabPage CabinetConfigFile;
        private System.Windows.Forms.ComboBox CabinetConfigMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label CabinetConfigFilenameLabel;
        private System.Windows.Forms.TextBox CabinetConfigFilename;
        private System.Windows.Forms.OpenFileDialog SelectCabinetConfigFile;
        private System.Windows.Forms.SaveFileDialog SaveCabinetConfigFile;
    }
}

