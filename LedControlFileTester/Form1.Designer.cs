namespace LedControlFileTester
{
    partial class Form1
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
            this.SelectLedControlFile = new System.Windows.Forms.Button();
            this.LedControlFileName = new System.Windows.Forms.Label();
            this.OpenLedControlFile = new System.Windows.Forms.OpenFileDialog();
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 486);
            this.Controls.Add(this.LedControlFileName);
            this.Controls.Add(this.SelectLedControlFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SelectLedControlFile;
        private System.Windows.Forms.Label LedControlFileName;
        private System.Windows.Forms.OpenFileDialog OpenLedControlFile;
    }
}

