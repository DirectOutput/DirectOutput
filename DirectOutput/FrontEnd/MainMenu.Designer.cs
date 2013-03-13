namespace DirectOutput.FrontEnd
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.ShowCabinetConfiguration = new System.Windows.Forms.Button();
            this.ShowTableElementStates = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(245, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(411, 140);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(246, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(410, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "DirectOutput framework for virtual pinball cabinets";
            // 
            // Version
            // 
            this.Version.Location = new System.Drawing.Point(247, 175);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(409, 23);
            this.Version.TabIndex = 2;
            this.Version.Text = "<unknown version>";
            this.Version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ShowCabinetConfiguration
            // 
            this.ShowCabinetConfiguration.Location = new System.Drawing.Point(245, 247);
            this.ShowCabinetConfiguration.Name = "ShowCabinetConfiguration";
            this.ShowCabinetConfiguration.Size = new System.Drawing.Size(269, 23);
            this.ShowCabinetConfiguration.TabIndex = 3;
            this.ShowCabinetConfiguration.Text = "Show cabinet configuration";
            this.ShowCabinetConfiguration.UseVisualStyleBackColor = true;
            this.ShowCabinetConfiguration.Click += new System.EventHandler(this.ShowCabinetConfiguration_Click);
            // 
            // ShowTableElementStates
            // 
            this.ShowTableElementStates.Location = new System.Drawing.Point(637, 269);
            this.ShowTableElementStates.Name = "ShowTableElementStates";
            this.ShowTableElementStates.Size = new System.Drawing.Size(190, 23);
            this.ShowTableElementStates.TabIndex = 4;
            this.ShowTableElementStates.Text = "Show table element states";
            this.ShowTableElementStates.UseVisualStyleBackColor = true;
            this.ShowTableElementStates.Click += new System.EventHandler(this.ShowTableElementStates_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 394);
            this.Controls.Add(this.ShowTableElementStates);
            this.Controls.Add(this.ShowCabinetConfiguration);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MainMenu";
            this.Text = "DirectOutput framework";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Version;
        private System.Windows.Forms.Button ShowCabinetConfiguration;
        private System.Windows.Forms.Button ShowTableElementStates;
    }
}