namespace DirectOutput.Frontend
{
    partial class TimeSpanStatisticsDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeSpanStatisticsDetails));
            this.CloseButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.DetailsTab = new System.Windows.Forms.TabPage();
            this.MinValuesTab = new System.Windows.Forms.TabPage();
            this.MaxValuesTab = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.DetailGroup = new System.Windows.Forms.Label();
            this.DetailName = new System.Windows.Forms.Label();
            this.DetailMaxDuration = new System.Windows.Forms.Label();
            this.DetailMinDuration = new System.Windows.Forms.Label();
            this.DetailAvgDuration = new System.Windows.Forms.Label();
            this.DetailTotalDuration = new System.Windows.Forms.Label();
            this.DetailValuesCount = new System.Windows.Forms.Label();
            this.MinDurationsGrid = new System.Windows.Forms.DataGridView();
            this.MinDurations = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxDurationsGrid = new System.Windows.Forms.DataGridView();
            this.MaxDurations = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.DetailsTab.SuspendLayout();
            this.MinValuesTab.SuspendLayout();
            this.MaxValuesTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinDurationsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDurationsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(431, 352);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.DetailsTab);
            this.tabControl1.Controls.Add(this.MinValuesTab);
            this.tabControl1.Controls.Add(this.MaxValuesTab);
            this.tabControl1.Location = new System.Drawing.Point(2, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(504, 343);
            this.tabControl1.TabIndex = 1;
            // 
            // DetailsTab
            // 
            this.DetailsTab.Controls.Add(this.DetailValuesCount);
            this.DetailsTab.Controls.Add(this.DetailTotalDuration);
            this.DetailsTab.Controls.Add(this.DetailAvgDuration);
            this.DetailsTab.Controls.Add(this.DetailMinDuration);
            this.DetailsTab.Controls.Add(this.DetailMaxDuration);
            this.DetailsTab.Controls.Add(this.DetailName);
            this.DetailsTab.Controls.Add(this.DetailGroup);
            this.DetailsTab.Controls.Add(this.label7);
            this.DetailsTab.Controls.Add(this.label6);
            this.DetailsTab.Controls.Add(this.label5);
            this.DetailsTab.Controls.Add(this.label4);
            this.DetailsTab.Controls.Add(this.label3);
            this.DetailsTab.Controls.Add(this.label2);
            this.DetailsTab.Controls.Add(this.label1);
            this.DetailsTab.Location = new System.Drawing.Point(4, 22);
            this.DetailsTab.Name = "DetailsTab";
            this.DetailsTab.Padding = new System.Windows.Forms.Padding(3);
            this.DetailsTab.Size = new System.Drawing.Size(496, 317);
            this.DetailsTab.TabIndex = 0;
            this.DetailsTab.Text = "Details";
            this.DetailsTab.UseVisualStyleBackColor = true;
            // 
            // MinValuesTab
            // 
            this.MinValuesTab.Controls.Add(this.MinDurationsGrid);
            this.MinValuesTab.Location = new System.Drawing.Point(4, 22);
            this.MinValuesTab.Name = "MinValuesTab";
            this.MinValuesTab.Padding = new System.Windows.Forms.Padding(3);
            this.MinValuesTab.Size = new System.Drawing.Size(496, 317);
            this.MinValuesTab.TabIndex = 1;
            this.MinValuesTab.Text = "Min. Values";
            this.MinValuesTab.UseVisualStyleBackColor = true;
            // 
            // MaxValuesTab
            // 
            this.MaxValuesTab.Controls.Add(this.MaxDurationsGrid);
            this.MaxValuesTab.Location = new System.Drawing.Point(4, 22);
            this.MaxValuesTab.Name = "MaxValuesTab";
            this.MaxValuesTab.Padding = new System.Windows.Forms.Padding(3);
            this.MaxValuesTab.Size = new System.Drawing.Size(496, 317);
            this.MaxValuesTab.TabIndex = 2;
            this.MaxValuesTab.Text = "Max. Values";
            this.MaxValuesTab.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Group:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Total duration:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Values count:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Maxiumum duration:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Minimum duration:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Average duration:";
            // 
            // DetailGroup
            // 
            this.DetailGroup.AutoSize = true;
            this.DetailGroup.Location = new System.Drawing.Point(119, 13);
            this.DetailGroup.Name = "DetailGroup";
            this.DetailGroup.Size = new System.Drawing.Size(36, 13);
            this.DetailGroup.TabIndex = 7;
            this.DetailGroup.Text = "Group";
            // 
            // DetailName
            // 
            this.DetailName.AutoSize = true;
            this.DetailName.Location = new System.Drawing.Point(119, 37);
            this.DetailName.Name = "DetailName";
            this.DetailName.Size = new System.Drawing.Size(35, 13);
            this.DetailName.TabIndex = 8;
            this.DetailName.Text = "Name";
            // 
            // DetailMaxDuration
            // 
            this.DetailMaxDuration.AutoSize = true;
            this.DetailMaxDuration.Location = new System.Drawing.Point(119, 157);
            this.DetailMaxDuration.Name = "DetailMaxDuration";
            this.DetailMaxDuration.Size = new System.Drawing.Size(67, 13);
            this.DetailMaxDuration.TabIndex = 10;
            this.DetailMaxDuration.Text = "MaxDuration";
            // 
            // DetailMinDuration
            // 
            this.DetailMinDuration.AutoSize = true;
            this.DetailMinDuration.Location = new System.Drawing.Point(119, 133);
            this.DetailMinDuration.Name = "DetailMinDuration";
            this.DetailMinDuration.Size = new System.Drawing.Size(64, 13);
            this.DetailMinDuration.TabIndex = 11;
            this.DetailMinDuration.Text = "MinDuration";
            // 
            // DetailAvgDuration
            // 
            this.DetailAvgDuration.AutoSize = true;
            this.DetailAvgDuration.Location = new System.Drawing.Point(119, 109);
            this.DetailAvgDuration.Name = "DetailAvgDuration";
            this.DetailAvgDuration.Size = new System.Drawing.Size(66, 13);
            this.DetailAvgDuration.TabIndex = 12;
            this.DetailAvgDuration.Text = "AvgDuration";
            // 
            // DetailTotalDuration
            // 
            this.DetailTotalDuration.AutoSize = true;
            this.DetailTotalDuration.Location = new System.Drawing.Point(119, 85);
            this.DetailTotalDuration.Name = "DetailTotalDuration";
            this.DetailTotalDuration.Size = new System.Drawing.Size(71, 13);
            this.DetailTotalDuration.TabIndex = 13;
            this.DetailTotalDuration.Text = "TotalDuration";
            // 
            // DetailValuesCount
            // 
            this.DetailValuesCount.AutoSize = true;
            this.DetailValuesCount.Location = new System.Drawing.Point(119, 61);
            this.DetailValuesCount.Name = "DetailValuesCount";
            this.DetailValuesCount.Size = new System.Drawing.Size(67, 13);
            this.DetailValuesCount.TabIndex = 14;
            this.DetailValuesCount.Text = "ValuesCount";
            // 
            // MinDurationsGrid
            // 
            this.MinDurationsGrid.AllowUserToAddRows = false;
            this.MinDurationsGrid.AllowUserToDeleteRows = false;
            this.MinDurationsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MinDurationsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MinDurations});
            this.MinDurationsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MinDurationsGrid.Location = new System.Drawing.Point(3, 3);
            this.MinDurationsGrid.MultiSelect = false;
            this.MinDurationsGrid.Name = "MinDurationsGrid";
            this.MinDurationsGrid.ReadOnly = true;
            this.MinDurationsGrid.RowHeadersVisible = false;
            this.MinDurationsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MinDurationsGrid.Size = new System.Drawing.Size(490, 311);
            this.MinDurationsGrid.TabIndex = 0;
            // 
            // MinDurations
            // 
            this.MinDurations.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MinDurations.HeaderText = "Durations";
            this.MinDurations.Name = "MinDurations";
            this.MinDurations.ReadOnly = true;
            // 
            // MaxDurationsGrid
            // 
            this.MaxDurationsGrid.AllowUserToAddRows = false;
            this.MaxDurationsGrid.AllowUserToDeleteRows = false;
            this.MaxDurationsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MaxDurationsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MaxDurations});
            this.MaxDurationsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaxDurationsGrid.Location = new System.Drawing.Point(3, 3);
            this.MaxDurationsGrid.MultiSelect = false;
            this.MaxDurationsGrid.Name = "MaxDurationsGrid";
            this.MaxDurationsGrid.ReadOnly = true;
            this.MaxDurationsGrid.RowHeadersVisible = false;
            this.MaxDurationsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MaxDurationsGrid.Size = new System.Drawing.Size(490, 311);
            this.MaxDurationsGrid.TabIndex = 1;
            // 
            // MaxDurations
            // 
            this.MaxDurations.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MaxDurations.HeaderText = "Durations";
            this.MaxDurations.Name = "MaxDurations";
            this.MaxDurations.ReadOnly = true;
            // 
            // TimeSpanStatisticsDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(509, 377);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.CloseButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TimeSpanStatisticsDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Statistic Details";
            this.tabControl1.ResumeLayout(false);
            this.DetailsTab.ResumeLayout(false);
            this.DetailsTab.PerformLayout();
            this.MinValuesTab.ResumeLayout(false);
            this.MaxValuesTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MinDurationsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDurationsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage DetailsTab;
        private System.Windows.Forms.TabPage MinValuesTab;
        private System.Windows.Forms.TabPage MaxValuesTab;
        private System.Windows.Forms.Label DetailValuesCount;
        private System.Windows.Forms.Label DetailTotalDuration;
        private System.Windows.Forms.Label DetailAvgDuration;
        private System.Windows.Forms.Label DetailMinDuration;
        private System.Windows.Forms.Label DetailMaxDuration;
        private System.Windows.Forms.Label DetailName;
        private System.Windows.Forms.Label DetailGroup;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView MinDurationsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn MinDurations;
        private System.Windows.Forms.DataGridView MaxDurationsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxDurations;
    }
}