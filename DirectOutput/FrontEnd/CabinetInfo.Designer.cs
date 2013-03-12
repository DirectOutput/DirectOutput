namespace DirectOutput.FrontEnd
{
    partial class CabinetInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CabinetInfo));
            this.label2 = new System.Windows.Forms.Label();
            this.CabinetOutputControllers = new System.Windows.Forms.DataGridView();
            this.CabinetOutputControllerOutputs = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.CabinetToys = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.CabinetOutputControllerProperties = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.CabinetToyProperties = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.SaveCabinetConfiguration = new System.Windows.Forms.SaveFileDialog();
            this.ExportCabinetConfiguration = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CabinetOutputControllers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CabinetOutputControllerOutputs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CabinetToys)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CabinetOutputControllerProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CabinetToyProperties)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output Controllers:";
            // 
            // CabinetOutputControllers
            // 
            this.CabinetOutputControllers.AllowUserToAddRows = false;
            this.CabinetOutputControllers.AllowUserToDeleteRows = false;
            this.CabinetOutputControllers.AllowUserToOrderColumns = true;
            this.CabinetOutputControllers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CabinetOutputControllers.Location = new System.Drawing.Point(14, 24);
            this.CabinetOutputControllers.MultiSelect = false;
            this.CabinetOutputControllers.Name = "CabinetOutputControllers";
            this.CabinetOutputControllers.ReadOnly = true;
            this.CabinetOutputControllers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CabinetOutputControllers.Size = new System.Drawing.Size(401, 150);
            this.CabinetOutputControllers.TabIndex = 4;
            this.CabinetOutputControllers.SelectionChanged += new System.EventHandler(this.CabinetOutputControllers_SelectionChanged);
            // 
            // CabinetOutputControllerOutputs
            // 
            this.CabinetOutputControllerOutputs.AllowUserToAddRows = false;
            this.CabinetOutputControllerOutputs.AllowUserToDeleteRows = false;
            this.CabinetOutputControllerOutputs.AllowUserToOrderColumns = true;
            this.CabinetOutputControllerOutputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CabinetOutputControllerOutputs.Location = new System.Drawing.Point(441, 24);
            this.CabinetOutputControllerOutputs.MultiSelect = false;
            this.CabinetOutputControllerOutputs.Name = "CabinetOutputControllerOutputs";
            this.CabinetOutputControllerOutputs.ReadOnly = true;
            this.CabinetOutputControllerOutputs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.CabinetOutputControllerOutputs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CabinetOutputControllerOutputs.Size = new System.Drawing.Size(691, 150);
            this.CabinetOutputControllerOutputs.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(438, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Outputs:";
            // 
            // CabinetToys
            // 
            this.CabinetToys.AllowUserToAddRows = false;
            this.CabinetToys.AllowUserToDeleteRows = false;
            this.CabinetToys.AllowUserToOrderColumns = true;
            this.CabinetToys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CabinetToys.Location = new System.Drawing.Point(14, 402);
            this.CabinetToys.Name = "CabinetToys";
            this.CabinetToys.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CabinetToys.Size = new System.Drawing.Size(401, 150);
            this.CabinetToys.TabIndex = 7;
            this.CabinetToys.SelectionChanged += new System.EventHandler(this.CabinetToys_SelectionChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 386);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Toys:";
            // 
            // CabinetOutputControllerProperties
            // 
            this.CabinetOutputControllerProperties.AllowUserToAddRows = false;
            this.CabinetOutputControllerProperties.AllowUserToDeleteRows = false;
            this.CabinetOutputControllerProperties.AllowUserToOrderColumns = true;
            this.CabinetOutputControllerProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CabinetOutputControllerProperties.Location = new System.Drawing.Point(441, 203);
            this.CabinetOutputControllerProperties.Name = "CabinetOutputControllerProperties";
            this.CabinetOutputControllerProperties.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.CabinetOutputControllerProperties.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CabinetOutputControllerProperties.Size = new System.Drawing.Size(691, 150);
            this.CabinetOutputControllerProperties.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(438, 187);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Output Controller Properties:";
            // 
            // CabinetToyProperties
            // 
            this.CabinetToyProperties.AllowUserToAddRows = false;
            this.CabinetToyProperties.AllowUserToDeleteRows = false;
            this.CabinetToyProperties.AllowUserToOrderColumns = true;
            this.CabinetToyProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CabinetToyProperties.Location = new System.Drawing.Point(441, 402);
            this.CabinetToyProperties.Name = "CabinetToyProperties";
            this.CabinetToyProperties.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CabinetToyProperties.Size = new System.Drawing.Size(691, 150);
            this.CabinetToyProperties.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(438, 386);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Toy properties:";
            // 
            // SaveCabinetConfiguration
            // 
            this.SaveCabinetConfiguration.DefaultExt = "xml";
            this.SaveCabinetConfiguration.Filter = "Config files (*.xml)|*.xml|All files (*.*)|*.*";
            this.SaveCabinetConfiguration.Title = "Export cabinet configuration";
            // 
            // ExportCabinetConfiguration
            // 
            this.ExportCabinetConfiguration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ExportCabinetConfiguration.Image = ((System.Drawing.Image)(resources.GetObject("ExportCabinetConfiguration.Image")));
            this.ExportCabinetConfiguration.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ExportCabinetConfiguration.Location = new System.Drawing.Point(940, 579);
            this.ExportCabinetConfiguration.Name = "ExportCabinetConfiguration";
            this.ExportCabinetConfiguration.Size = new System.Drawing.Size(192, 39);
            this.ExportCabinetConfiguration.TabIndex = 13;
            this.ExportCabinetConfiguration.Text = "Export cabinet configuration";
            this.ExportCabinetConfiguration.UseVisualStyleBackColor = true;
            this.ExportCabinetConfiguration.Click += new System.EventHandler(this.ExportCabinetConfiguration_Click);
            // 
            // CabinetInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 648);
            this.Controls.Add(this.ExportCabinetConfiguration);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.CabinetToyProperties);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CabinetOutputControllerProperties);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CabinetToys);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CabinetOutputControllerOutputs);
            this.Controls.Add(this.CabinetOutputControllers);
            this.Controls.Add(this.label2);
            this.Name = "CabinetInfo";
            this.Text = "CabinetInfo";
            ((System.ComponentModel.ISupportInitialize)(this.CabinetOutputControllers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CabinetOutputControllerOutputs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CabinetToys)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CabinetOutputControllerProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CabinetToyProperties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView CabinetOutputControllers;
        private System.Windows.Forms.DataGridView CabinetOutputControllerOutputs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView CabinetToys;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView CabinetOutputControllerProperties;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView CabinetToyProperties;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.SaveFileDialog SaveCabinetConfiguration;
        private System.Windows.Forms.Button ExportCabinetConfiguration;
    }
}