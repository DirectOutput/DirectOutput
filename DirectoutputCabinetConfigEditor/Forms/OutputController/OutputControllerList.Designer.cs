namespace DirectoutputCabinetConfigEditor.Forms.OutputController
{
    partial class OutputControllerList
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.OutputControllerListView = new ComponentOwl.BetterListView.BetterListView();
            this.OutputControllerTypeColumn = new ComponentOwl.BetterListView.BetterListViewColumnHeader();
            this.OutputControllerNameColumn = new ComponentOwl.BetterListView.BetterListViewColumnHeader();
            ((System.ComponentModel.ISupportInitialize)(this.OutputControllerListView)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(579, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // OutputControllerListView
            // 
            this.OutputControllerListView.Columns.Add(this.OutputControllerTypeColumn);
            this.OutputControllerListView.Columns.Add(this.OutputControllerNameColumn);
            this.OutputControllerListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputControllerListView.Location = new System.Drawing.Point(0, 25);
            this.OutputControllerListView.Name = "OutputControllerListView";
            this.OutputControllerListView.Size = new System.Drawing.Size(579, 719);
            this.OutputControllerListView.TabIndex = 1;
            // 
            // OutputControllerTypeColumn
            // 
            this.OutputControllerTypeColumn.Name = "OutputControllerTypeColumn";
            this.OutputControllerTypeColumn.Style = ComponentOwl.BetterListView.BetterListViewColumnHeaderStyle.Sortable;
            this.OutputControllerTypeColumn.Text = "Type";
            this.OutputControllerTypeColumn.Width = 167;
            // 
            // OutputControllerNameColumn
            // 
            this.OutputControllerNameColumn.Name = "OutputControllerNameColumn";
            this.OutputControllerNameColumn.Text = "Name";
            this.OutputControllerNameColumn.Width = 197;
            // 
            // OutputControllerList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 744);
            this.Controls.Add(this.OutputControllerListView);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OutputControllerList";
            this.Text = "OutputControllerList";
            ((System.ComponentModel.ISupportInitialize)(this.OutputControllerListView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private ComponentOwl.BetterListView.BetterListView OutputControllerListView;
        private ComponentOwl.BetterListView.BetterListViewColumnHeader OutputControllerTypeColumn;
        private ComponentOwl.BetterListView.BetterListViewColumnHeader OutputControllerNameColumn;
    }
}