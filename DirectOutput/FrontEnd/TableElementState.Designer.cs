namespace DirectOutput.FrontEnd
{
    partial class TableElementState
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
            this.States = new System.Windows.Forms.DataGridView();
            this.ColumnTableElementType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTableElementNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TableElementName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTableElementValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Filter = new System.Windows.Forms.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.States)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // States
            // 
            this.States.AllowUserToAddRows = false;
            this.States.AllowUserToDeleteRows = false;
            this.States.AllowUserToOrderColumns = true;
            this.States.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.States.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTableElementType,
            this.ColumnTableElementNumber,
            this.TableElementName,
            this.ColumnTableElementValue});
            this.States.Dock = System.Windows.Forms.DockStyle.Fill;
            this.States.Location = new System.Drawing.Point(0, 0);
            this.States.Name = "States";
            this.States.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.States.Size = new System.Drawing.Size(977, 578);
            this.States.TabIndex = 0;
            // 
            // ColumnTableElementType
            // 
            this.ColumnTableElementType.HeaderText = "Type";
            this.ColumnTableElementType.Name = "ColumnTableElementType";
            this.ColumnTableElementType.ReadOnly = true;
            // 
            // ColumnTableElementNumber
            // 
            this.ColumnTableElementNumber.HeaderText = "Number";
            this.ColumnTableElementNumber.Name = "ColumnTableElementNumber";
            this.ColumnTableElementNumber.ReadOnly = true;
            // 
            // TableElementName
            // 
            this.TableElementName.HeaderText = "Name";
            this.TableElementName.Name = "TableElementName";
            this.TableElementName.ReadOnly = true;
            // 
            // ColumnTableElementValue
            // 
            this.ColumnTableElementValue.HeaderText = "Value";
            this.ColumnTableElementValue.Name = "ColumnTableElementValue";
            this.ColumnTableElementValue.ReadOnly = true;
            // 
            // Filter
            // 
            this.Filter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Filter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Filter.FormattingEnabled = true;
            this.Filter.Location = new System.Drawing.Point(0, 0);
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(919, 21);
            this.Filter.TabIndex = 1;
            this.Filter.SelectedIndexChanged += new System.EventHandler(this.Filter_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.States);
            this.splitContainer1.Size = new System.Drawing.Size(977, 607);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.Filter);
            this.splitContainer2.Size = new System.Drawing.Size(977, 25);
            this.splitContainer2.SplitterDistance = 54;
            this.splitContainer2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Show:";
            // 
            // TableElementState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 607);
            this.Controls.Add(this.splitContainer1);
            this.Name = "TableElementState";
            this.Text = "TableElementState x";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TableElementState_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.States)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView States;
        private System.Windows.Forms.ComboBox Filter;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTableElementType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTableElementNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn TableElementName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTableElementValue;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
    }
}