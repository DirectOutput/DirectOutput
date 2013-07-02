namespace DirectOutput.Frontend
{
    partial class SystemMonitor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemMonitor));
            this.SystemMonitorTabs = new System.Windows.Forms.TabControl();
            this.ThreadsTab = new System.Windows.Forms.TabPage();
            this.StatisticsTab = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ThreadDisplay = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.ThreadName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThreadHostObject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThreadProcessorNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThreadIsAlive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ThreadLastHeartBeat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThreadExceptions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemMonitorTabs.SuspendLayout();
            this.ThreadsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SystemMonitorTabs
            // 
            this.SystemMonitorTabs.Controls.Add(this.ThreadsTab);
            this.SystemMonitorTabs.Controls.Add(this.StatisticsTab);
            this.SystemMonitorTabs.Controls.Add(this.tabPage3);
            this.SystemMonitorTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SystemMonitorTabs.Location = new System.Drawing.Point(0, 0);
            this.SystemMonitorTabs.Name = "SystemMonitorTabs";
            this.SystemMonitorTabs.SelectedIndex = 0;
            this.SystemMonitorTabs.Size = new System.Drawing.Size(766, 485);
            this.SystemMonitorTabs.TabIndex = 1;
            // 
            // ThreadsTab
            // 
            this.ThreadsTab.Controls.Add(this.ThreadDisplay);
            this.ThreadsTab.Location = new System.Drawing.Point(4, 22);
            this.ThreadsTab.Name = "ThreadsTab";
            this.ThreadsTab.Padding = new System.Windows.Forms.Padding(3);
            this.ThreadsTab.Size = new System.Drawing.Size(758, 459);
            this.ThreadsTab.TabIndex = 0;
            this.ThreadsTab.Text = "Threads";
            this.ThreadsTab.UseVisualStyleBackColor = true;
            // 
            // StatisticsTab
            // 
            this.StatisticsTab.Location = new System.Drawing.Point(4, 22);
            this.StatisticsTab.Name = "StatisticsTab";
            this.StatisticsTab.Padding = new System.Windows.Forms.Padding(3);
            this.StatisticsTab.Size = new System.Drawing.Size(758, 469);
            this.StatisticsTab.TabIndex = 1;
            this.StatisticsTab.Text = "Statistics";
            this.StatisticsTab.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(758, 469);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ThreadDisplay
            // 
            this.ThreadDisplay.AllowUserToAddRows = false;
            this.ThreadDisplay.AllowUserToDeleteRows = false;
            this.ThreadDisplay.AllowUserToOrderColumns = true;
            this.ThreadDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ThreadDisplay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ThreadName,
            this.ThreadHostObject,
            this.ThreadProcessorNumber,
            this.ThreadIsAlive,
            this.ThreadLastHeartBeat,
            this.ThreadExceptions});
            this.ThreadDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThreadDisplay.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ThreadDisplay.Location = new System.Drawing.Point(3, 3);
            this.ThreadDisplay.MultiSelect = false;
            this.ThreadDisplay.Name = "ThreadDisplay";
            this.ThreadDisplay.ReadOnly = true;
            this.ThreadDisplay.RowHeadersVisible = false;
            this.ThreadDisplay.RowHeadersWidth = 4;
            this.ThreadDisplay.Size = new System.Drawing.Size(752, 453);
            this.ThreadDisplay.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.SystemMonitorTabs);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.RefreshButton);
            this.splitContainer1.Size = new System.Drawing.Size(766, 517);
            this.splitContainer1.SplitterDistance = 485;
            this.splitContainer1.TabIndex = 2;
            // 
            // RefreshButton
            // 
            this.RefreshButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RefreshButton.Location = new System.Drawing.Point(0, 0);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(766, 28);
            this.RefreshButton.TabIndex = 0;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            // 
            // ThreadName
            // 
            this.ThreadName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThreadName.HeaderText = "Thread Name";
            this.ThreadName.Name = "ThreadName";
            this.ThreadName.ReadOnly = true;
            // 
            // ThreadHostObject
            // 
            this.ThreadHostObject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThreadHostObject.HeaderText = "Host Object";
            this.ThreadHostObject.Name = "ThreadHostObject";
            this.ThreadHostObject.ReadOnly = true;
            // 
            // ThreadProcessorNumber
            // 
            this.ThreadProcessorNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ThreadProcessorNumber.DefaultCellStyle = dataGridViewCellStyle1;
            this.ThreadProcessorNumber.FillWeight = 60F;
            this.ThreadProcessorNumber.HeaderText = "Processor Number";
            this.ThreadProcessorNumber.Name = "ThreadProcessorNumber";
            this.ThreadProcessorNumber.ReadOnly = true;
            // 
            // ThreadIsAlive
            // 
            this.ThreadIsAlive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ThreadIsAlive.HeaderText = "Alive";
            this.ThreadIsAlive.MinimumWidth = 30;
            this.ThreadIsAlive.Name = "ThreadIsAlive";
            this.ThreadIsAlive.ReadOnly = true;
            this.ThreadIsAlive.Width = 36;
            // 
            // ThreadLastHeartBeat
            // 
            this.ThreadLastHeartBeat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThreadLastHeartBeat.FillWeight = 50F;
            this.ThreadLastHeartBeat.HeaderText = "Last Heartbeat";
            this.ThreadLastHeartBeat.Name = "ThreadLastHeartBeat";
            this.ThreadLastHeartBeat.ReadOnly = true;
            // 
            // ThreadExceptions
            // 
            this.ThreadExceptions.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ThreadExceptions.DefaultCellStyle = dataGridViewCellStyle2;
            this.ThreadExceptions.FillWeight = 30F;
            this.ThreadExceptions.HeaderText = "Exceptions";
            this.ThreadExceptions.Name = "ThreadExceptions";
            this.ThreadExceptions.ReadOnly = true;
            // 
            // SystemMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 517);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SystemMonitor";
            this.Text = "System Monitor";
            this.SystemMonitorTabs.ResumeLayout(false);
            this.ThreadsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ThreadDisplay)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl SystemMonitorTabs;
        private System.Windows.Forms.TabPage ThreadsTab;
        private System.Windows.Forms.TabPage StatisticsTab;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView ThreadDisplay;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThreadName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThreadHostObject;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThreadProcessorNumber;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ThreadIsAlive;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThreadLastHeartBeat;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThreadExceptions;
    }
}