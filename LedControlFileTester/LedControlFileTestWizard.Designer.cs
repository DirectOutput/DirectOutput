namespace LedControlFileTester
{
    partial class LedControlFileTestWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LedControlFileTestWizard));
            this.SelectLedControlFile = new System.Windows.Forms.Button();
            this.LedControlFileName = new System.Windows.Forms.Label();
            this.OpenLedControlFile = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.ParsingResults = new System.Windows.Forms.DataGridView();
            this.ParsingResultsTimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParsingResultsMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ParsingResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // SelectLedControlFile
            // 
            this.SelectLedControlFile.Location = new System.Drawing.Point(12, 12);
            this.SelectLedControlFile.Name = "SelectLedControlFile";
            this.SelectLedControlFile.Size = new System.Drawing.Size(123, 23);
            this.SelectLedControlFile.TabIndex = 0;
            this.SelectLedControlFile.Text = "Select LedControl file";
            this.SelectLedControlFile.UseVisualStyleBackColor = true;
            this.SelectLedControlFile.Click += new System.EventHandler(this.SelectLedControlFile_Click);
            // 
            // LedControlFileName
            // 
            this.LedControlFileName.AutoSize = true;
            this.LedControlFileName.Location = new System.Drawing.Point(141, 17);
            this.LedControlFileName.Name = "LedControlFileName";
            this.LedControlFileName.Size = new System.Drawing.Size(149, 13);
            this.LedControlFileName.TabIndex = 1;
            this.LedControlFileName.Text = "Please select a LedControl file";
            // 
            // OpenLedControlFile
            // 
            this.OpenLedControlFile.DefaultExt = "ini";
            this.OpenLedControlFile.FileName = "ledcontrol.ini";
            this.OpenLedControlFile.Filter = "Ini-Files (*.ini)|*.ini|All files (*.*)|*.*";
            this.OpenLedControlFile.Title = "Select a LedControlFile";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Parsing results:";
            // 
            // ParsingResults
            // 
            this.ParsingResults.AllowUserToAddRows = false;
            this.ParsingResults.AllowUserToDeleteRows = false;
            this.ParsingResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ParsingResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ParsingResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParsingResultsTimeStamp,
            this.ParsingResultsMessage});
            this.ParsingResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ParsingResults.Location = new System.Drawing.Point(12, 54);
            this.ParsingResults.MultiSelect = false;
            this.ParsingResults.Name = "ParsingResults";
            this.ParsingResults.ReadOnly = true;
            this.ParsingResults.RowHeadersVisible = false;
            this.ParsingResults.Size = new System.Drawing.Size(1120, 585);
            this.ParsingResults.TabIndex = 4;
            // 
            // ParsingResultsTimeStamp
            // 
            this.ParsingResultsTimeStamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ParsingResultsTimeStamp.FillWeight = 20F;
            this.ParsingResultsTimeStamp.HeaderText = "Time";
            this.ParsingResultsTimeStamp.Name = "ParsingResultsTimeStamp";
            this.ParsingResultsTimeStamp.ReadOnly = true;
            // 
            // ParsingResultsMessage
            // 
            this.ParsingResultsMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ParsingResultsMessage.FillWeight = 80F;
            this.ParsingResultsMessage.HeaderText = "Message";
            this.ParsingResultsMessage.Name = "ParsingResultsMessage";
            this.ParsingResultsMessage.ReadOnly = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(993, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(139, 45);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // LedControlFileTestWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 651);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ParsingResults);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LedControlFileName);
            this.Controls.Add(this.SelectLedControlFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LedControlFileTestWizard";
            this.Text = "LedControl Tester";
            ((System.ComponentModel.ISupportInitialize)(this.ParsingResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SelectLedControlFile;
        private System.Windows.Forms.Label LedControlFileName;
        private System.Windows.Forms.OpenFileDialog OpenLedControlFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView ParsingResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParsingResultsTimeStamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParsingResultsMessage;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

