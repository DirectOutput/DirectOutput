namespace DirectOutput.Frontend
{
    partial class CabinetEditor
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Cabinet");
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.CabinetTabPage = new System.Windows.Forms.TabPage();
            this.CabinetParts = new System.Windows.Forms.TreeView();
            this.CabinetContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabControl4 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.tabControl5 = new System.Windows.Forms.TabControl();
            this.ReferencesTab = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.PropertiesTabPage = new System.Windows.Forms.TabPage();
            this.Properties = new System.Windows.Forms.PropertyGrid();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.CabinetTabPage.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.tabControl5.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.PropertiesTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Location = new System.Drawing.Point(0, -4);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer2.Size = new System.Drawing.Size(1803, 882);
            this.splitContainer2.SplitterDistance = 939;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tabControl2);
            this.splitContainer3.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tabControl4);
            this.splitContainer3.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer3.Size = new System.Drawing.Size(939, 882);
            this.splitContainer3.SplitterDistance = 670;
            this.splitContainer3.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.CabinetTabPage);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(929, 660);
            this.tabControl2.TabIndex = 0;
            // 
            // CabinetTabPage
            // 
            this.CabinetTabPage.Controls.Add(this.CabinetParts);
            this.CabinetTabPage.Location = new System.Drawing.Point(4, 22);
            this.CabinetTabPage.Name = "CabinetTabPage";
            this.CabinetTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.CabinetTabPage.Size = new System.Drawing.Size(921, 634);
            this.CabinetTabPage.TabIndex = 0;
            this.CabinetTabPage.Text = "Cabinet";
            this.CabinetTabPage.UseVisualStyleBackColor = true;
            // 
            // CabinetParts
            // 
            this.CabinetParts.ContextMenuStrip = this.CabinetContextMenuStrip;
            this.CabinetParts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CabinetParts.FullRowSelect = true;
            this.CabinetParts.Location = new System.Drawing.Point(3, 3);
            this.CabinetParts.Name = "CabinetParts";
            treeNode2.Name = "Knoten0";
            treeNode2.Text = "Cabinet";
            this.CabinetParts.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.CabinetParts.ShowNodeToolTips = true;
            this.CabinetParts.Size = new System.Drawing.Size(915, 628);
            this.CabinetParts.TabIndex = 0;
            this.CabinetParts.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.CabinetParts_AfterSelect);
            this.CabinetParts.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.CabinetParts_NodeMouseClick);
            this.CabinetParts.Click += new System.EventHandler(this.CabinetParts_Click);
            // 
            // CabinetContextMenuStrip
            // 
            this.CabinetContextMenuStrip.Name = "CabinetContextMenuStrip";
            this.CabinetContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            this.CabinetContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.CabinetContextMenuStrip_Opening);
            // 
            // tabControl4
            // 
            this.tabControl4.Controls.Add(this.tabPage1);
            this.tabControl4.Controls.Add(this.tabPage2);
            this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl4.Location = new System.Drawing.Point(3, 3);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new System.Drawing.Size(929, 198);
            this.tabControl4.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(921, 172);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(915, 166);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(921, 172);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.tabControl5);
            this.splitContainer4.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.tabControl3);
            this.splitContainer4.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer4.Size = new System.Drawing.Size(860, 882);
            this.splitContainer4.SplitterDistance = 369;
            this.splitContainer4.TabIndex = 1;
            // 
            // tabControl5
            // 
            this.tabControl5.Controls.Add(this.ReferencesTab);
            this.tabControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl5.Location = new System.Drawing.Point(3, 3);
            this.tabControl5.Name = "tabControl5";
            this.tabControl5.SelectedIndex = 0;
            this.tabControl5.Size = new System.Drawing.Size(850, 359);
            this.tabControl5.TabIndex = 0;
            // 
            // ReferencesTab
            // 
            this.ReferencesTab.Location = new System.Drawing.Point(4, 22);
            this.ReferencesTab.Name = "ReferencesTab";
            this.ReferencesTab.Padding = new System.Windows.Forms.Padding(3);
            this.ReferencesTab.Size = new System.Drawing.Size(842, 333);
            this.ReferencesTab.TabIndex = 0;
            this.ReferencesTab.Text = "References";
            this.ReferencesTab.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.PropertiesTabPage);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(3, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(850, 499);
            this.tabControl3.TabIndex = 0;
            // 
            // PropertiesTabPage
            // 
            this.PropertiesTabPage.Controls.Add(this.Properties);
            this.PropertiesTabPage.Location = new System.Drawing.Point(4, 22);
            this.PropertiesTabPage.Name = "PropertiesTabPage";
            this.PropertiesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.PropertiesTabPage.Size = new System.Drawing.Size(842, 473);
            this.PropertiesTabPage.TabIndex = 0;
            this.PropertiesTabPage.Text = "Properties";
            this.PropertiesTabPage.UseVisualStyleBackColor = true;
            // 
            // Properties
            // 
            this.Properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Properties.Location = new System.Drawing.Point(3, 3);
            this.Properties.Name = "Properties";
            this.Properties.Size = new System.Drawing.Size(836, 467);
            this.Properties.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 881);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1803, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // CabinetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1803, 903);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer2);
            this.Name = "CabinetEditor";
            this.Text = "CabinetEditor";
            this.Activated += new System.EventHandler(this.CabinetEditor_Activated);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.CabinetTabPage.ResumeLayout(false);
            this.tabControl4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.tabControl5.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.PropertiesTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage CabinetTabPage;
        private System.Windows.Forms.TreeView CabinetParts;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage PropertiesTabPage;
        private System.Windows.Forms.PropertyGrid Properties;
        private System.Windows.Forms.TabControl tabControl4;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TabControl tabControl5;
        private System.Windows.Forms.TabPage ReferencesTab;
        private System.Windows.Forms.ContextMenuStrip CabinetContextMenuStrip;
    }
}