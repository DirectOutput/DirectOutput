namespace DirectOutputConfigTester
{
    partial class ConfigTester
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigTester));
            this.OpenGlobalConfigDialog = new System.Windows.Forms.OpenFileDialog();
            this.OpenTableDialog = new System.Windows.Forms.OpenFileDialog();
            this.TableElements = new System.Windows.Forms.DataGridView();
            this.TEType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TEName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TENumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TEValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TEActivate = new System.Windows.Forms.DataGridViewButtonColumn();
            this.TEPulse = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.TableElements)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenGlobalConfigDialog
            // 
            this.OpenGlobalConfigDialog.FileName = "openFileDialog1";
            // 
            // OpenTableDialog
            // 
            this.OpenTableDialog.FileName = "openFileDialog1";
            // 
            // TableElements
            // 
            this.TableElements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TableElements.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TEType,
            this.TEName,
            this.TENumber,
            this.TEValue,
            this.TEActivate,
            this.TEPulse});
            this.TableElements.Location = new System.Drawing.Point(25, 32);
            this.TableElements.Name = "TableElements";
            this.TableElements.RowHeadersVisible = false;
            this.TableElements.Size = new System.Drawing.Size(638, 356);
            this.TableElements.TabIndex = 0;
            // 
            // TEType
            // 
            this.TEType.HeaderText = "Type";
            this.TEType.Name = "TEType";
            this.TEType.ReadOnly = true;
            // 
            // TEName
            // 
            this.TEName.HeaderText = "Name";
            this.TEName.Name = "TEName";
            this.TEName.ReadOnly = true;
            // 
            // TENumber
            // 
            this.TENumber.HeaderText = "Number";
            this.TENumber.Name = "TENumber";
            this.TENumber.ReadOnly = true;
            // 
            // TEValue
            // 
            this.TEValue.HeaderText = "Value";
            this.TEValue.Name = "TEValue";
            // 
            // TEActivate
            // 
            this.TEActivate.HeaderText = "Activate";
            this.TEActivate.Name = "TEActivate";
            this.TEActivate.ReadOnly = true;
            this.TEActivate.UseColumnTextForButtonValue = true;
            // 
            // TEPulse
            // 
            this.TEPulse.HeaderText = "Pulse";
            this.TEPulse.Name = "TEPulse";
            this.TEPulse.ReadOnly = true;
            // 
            // ConfigTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 438);
            this.Controls.Add(this.TableElements);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfigTester";
            this.Text = "DirectOutput configuration tester";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigTester_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.TableElements)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OpenGlobalConfigDialog;
        private System.Windows.Forms.OpenFileDialog OpenTableDialog;
        private System.Windows.Forms.DataGridView TableElements;
        private System.Windows.Forms.DataGridViewTextBoxColumn TEType;
        private System.Windows.Forms.DataGridViewTextBoxColumn TEName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TENumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn TEValue;
        private System.Windows.Forms.DataGridViewButtonColumn TEActivate;
        private System.Windows.Forms.DataGridViewButtonColumn TEPulse;
    }
}

