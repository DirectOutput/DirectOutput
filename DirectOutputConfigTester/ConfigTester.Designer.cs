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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.ActivateAllButton = new System.Windows.Forms.Button();
            this.DeactivateAllButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.PulseDurationInput = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.LoadConfigButton = new System.Windows.Forms.Button();
            this.ShowFrontEndButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TableElements)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PulseDurationInput)).BeginInit();
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
            this.TableElements.AllowUserToAddRows = false;
            this.TableElements.AllowUserToDeleteRows = false;
            this.TableElements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TableElements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TableElements.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TEType,
            this.TEName,
            this.TENumber,
            this.TEValue,
            this.TEActivate,
            this.TEPulse});
            this.TableElements.Location = new System.Drawing.Point(3, 1);
            this.TableElements.Name = "TableElements";
            this.TableElements.RowHeadersVisible = false;
            this.TableElements.Size = new System.Drawing.Size(719, 396);
            this.TableElements.TabIndex = 0;
            this.TableElements.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TableElements_CellClick);
            this.TableElements.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.TableElements_CellValueChanged);
            // 
            // TEType
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TEType.DefaultCellStyle = dataGridViewCellStyle1;
            this.TEType.HeaderText = "Type";
            this.TEType.Name = "TEType";
            this.TEType.ReadOnly = true;
            // 
            // TEName
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TEName.DefaultCellStyle = dataGridViewCellStyle2;
            this.TEName.HeaderText = "Name";
            this.TEName.Name = "TEName";
            this.TEName.ReadOnly = true;
            // 
            // TENumber
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TENumber.DefaultCellStyle = dataGridViewCellStyle3;
            this.TENumber.HeaderText = "Number";
            this.TENumber.Name = "TENumber";
            this.TENumber.ReadOnly = true;
            // 
            // TEValue
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.TEValue.DefaultCellStyle = dataGridViewCellStyle4;
            this.TEValue.HeaderText = "Value";
            this.TEValue.Name = "TEValue";
            // 
            // TEActivate
            // 
            this.TEActivate.HeaderText = "Activate";
            this.TEActivate.Name = "TEActivate";
            this.TEActivate.ReadOnly = true;
            // 
            // TEPulse
            // 
            this.TEPulse.HeaderText = "Pulse";
            this.TEPulse.Name = "TEPulse";
            this.TEPulse.ReadOnly = true;
            // 
            // ActivateAllButton
            // 
            this.ActivateAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ActivateAllButton.Location = new System.Drawing.Point(356, 403);
            this.ActivateAllButton.Name = "ActivateAllButton";
            this.ActivateAllButton.Size = new System.Drawing.Size(90, 23);
            this.ActivateAllButton.TabIndex = 1;
            this.ActivateAllButton.Text = "Activate all";
            this.ActivateAllButton.UseVisualStyleBackColor = true;
            this.ActivateAllButton.Click += new System.EventHandler(this.ActivateAllButton_Click);
            // 
            // DeactivateAllButton
            // 
            this.DeactivateAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeactivateAllButton.Location = new System.Drawing.Point(452, 403);
            this.DeactivateAllButton.Name = "DeactivateAllButton";
            this.DeactivateAllButton.Size = new System.Drawing.Size(93, 23);
            this.DeactivateAllButton.TabIndex = 2;
            this.DeactivateAllButton.Text = "Deactivate all";
            this.DeactivateAllButton.UseVisualStyleBackColor = true;
            this.DeactivateAllButton.Click += new System.EventHandler(this.DeactivateAllButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(551, 408);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Pulse duration:";
            // 
            // PulseDurationInput
            // 
            this.PulseDurationInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PulseDurationInput.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.PulseDurationInput.Location = new System.Drawing.Point(634, 406);
            this.PulseDurationInput.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.PulseDurationInput.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.PulseDurationInput.Name = "PulseDurationInput";
            this.PulseDurationInput.Size = new System.Drawing.Size(66, 20);
            this.PulseDurationInput.TabIndex = 5;
            this.PulseDurationInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.PulseDurationInput.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.PulseDurationInput.ValueChanged += new System.EventHandler(this.PulseDurationInput_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(701, 408);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "ms";
            // 
            // LoadConfigButton
            // 
            this.LoadConfigButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LoadConfigButton.Location = new System.Drawing.Point(12, 403);
            this.LoadConfigButton.Name = "LoadConfigButton";
            this.LoadConfigButton.Size = new System.Drawing.Size(97, 23);
            this.LoadConfigButton.TabIndex = 7;
            this.LoadConfigButton.Text = "Load config";
            this.LoadConfigButton.UseVisualStyleBackColor = true;
            this.LoadConfigButton.Click += new System.EventHandler(this.LoadConfigButton_Click);
            // 
            // ShowFrontEndButton
            // 
            this.ShowFrontEndButton.Location = new System.Drawing.Point(116, 403);
            this.ShowFrontEndButton.Name = "ShowFrontEndButton";
            this.ShowFrontEndButton.Size = new System.Drawing.Size(152, 23);
            this.ShowFrontEndButton.TabIndex = 8;
            this.ShowFrontEndButton.Text = "Show DirectOutput FrontEnd";
            this.ShowFrontEndButton.UseVisualStyleBackColor = true;
            this.ShowFrontEndButton.Click += new System.EventHandler(this.ShowFrontEndButton_Click);
            // 
            // ConfigTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 438);
            this.Controls.Add(this.ShowFrontEndButton);
            this.Controls.Add(this.LoadConfigButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PulseDurationInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DeactivateAllButton);
            this.Controls.Add(this.ActivateAllButton);
            this.Controls.Add(this.TableElements);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfigTester";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "DirectOutput configuration tester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigTester_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigTester_FormClosed);
            this.Load += new System.EventHandler(this.ConfigTester_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TableElements)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PulseDurationInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OpenGlobalConfigDialog;
        private System.Windows.Forms.OpenFileDialog OpenTableDialog;
        private System.Windows.Forms.DataGridView TableElements;
        private System.Windows.Forms.Button ActivateAllButton;
        private System.Windows.Forms.Button DeactivateAllButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown PulseDurationInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button LoadConfigButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn TEType;
        private System.Windows.Forms.DataGridViewTextBoxColumn TEName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TENumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn TEValue;
        private System.Windows.Forms.DataGridViewButtonColumn TEActivate;
        private System.Windows.Forms.DataGridViewButtonColumn TEPulse;
        private System.Windows.Forms.Button ShowFrontEndButton;
    }
}

