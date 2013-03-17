namespace DirectOutput.Frontend
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
            this.ShowTableConfiguration = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.EditGlobalConfiguration = new System.Windows.Forms.Button();
            this.TableName = new System.Windows.Forms.TextBox();
            this.TableFilename = new System.Windows.Forms.TextBox();
            this.TableRomname = new System.Windows.Forms.TextBox();
            this.GlobalConfigFilename = new System.Windows.Forms.TextBox();
            this.TableConfigFilename = new System.Windows.Forms.TextBox();
            this.CabinetConfigFilename = new System.Windows.Forms.TextBox();
            this.ShowLoadedScripts = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(214, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(542, 140);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(215, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(410, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "DirectOutput framework for virtual pinball cabinets";
            // 
            // Version
            // 
            this.Version.Location = new System.Drawing.Point(216, 175);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(409, 23);
            this.Version.TabIndex = 2;
            this.Version.Text = "<unknown version>";
            this.Version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ShowCabinetConfiguration
            // 
            this.ShowCabinetConfiguration.Location = new System.Drawing.Point(341, 387);
            this.ShowCabinetConfiguration.Name = "ShowCabinetConfiguration";
            this.ShowCabinetConfiguration.Size = new System.Drawing.Size(157, 23);
            this.ShowCabinetConfiguration.TabIndex = 3;
            this.ShowCabinetConfiguration.Text = "Show cabinet configuration";
            this.ShowCabinetConfiguration.UseVisualStyleBackColor = true;
            this.ShowCabinetConfiguration.Click += new System.EventHandler(this.ShowCabinetConfiguration_Click);
            // 
            // ShowTableConfiguration
            // 
            this.ShowTableConfiguration.Location = new System.Drawing.Point(178, 387);
            this.ShowTableConfiguration.Name = "ShowTableConfiguration";
            this.ShowTableConfiguration.Size = new System.Drawing.Size(157, 23);
            this.ShowTableConfiguration.TabIndex = 4;
            this.ShowTableConfiguration.Text = "Show table Configuration";
            this.ShowTableConfiguration.UseVisualStyleBackColor = true;
            this.ShowTableConfiguration.Click += new System.EventHandler(this.ShowTableConfiguration_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 214);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Table:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 237);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Table file:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 259);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Table rom:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 326);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Table configuration file:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 361);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Cabinet configuration file:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 294);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Global configuration file:";
            // 
            // EditGlobalConfiguration
            // 
            this.EditGlobalConfiguration.Location = new System.Drawing.Point(15, 387);
            this.EditGlobalConfiguration.Name = "EditGlobalConfiguration";
            this.EditGlobalConfiguration.Size = new System.Drawing.Size(157, 23);
            this.EditGlobalConfiguration.TabIndex = 11;
            this.EditGlobalConfiguration.Text = "Edit global configuration";
            this.EditGlobalConfiguration.UseVisualStyleBackColor = true;
            this.EditGlobalConfiguration.Click += new System.EventHandler(this.EditGlobalConfiguration_Click);
            // 
            // TableName
            // 
            this.TableName.Location = new System.Drawing.Point(148, 211);
            this.TableName.Name = "TableName";
            this.TableName.ReadOnly = true;
            this.TableName.Size = new System.Drawing.Size(565, 20);
            this.TableName.TabIndex = 12;
            // 
            // TableFilename
            // 
            this.TableFilename.Location = new System.Drawing.Point(148, 234);
            this.TableFilename.Name = "TableFilename";
            this.TableFilename.ReadOnly = true;
            this.TableFilename.Size = new System.Drawing.Size(565, 20);
            this.TableFilename.TabIndex = 13;
            // 
            // TableRomname
            // 
            this.TableRomname.Location = new System.Drawing.Point(148, 256);
            this.TableRomname.Name = "TableRomname";
            this.TableRomname.ReadOnly = true;
            this.TableRomname.Size = new System.Drawing.Size(565, 20);
            this.TableRomname.TabIndex = 14;
            // 
            // GlobalConfigFilename
            // 
            this.GlobalConfigFilename.Location = new System.Drawing.Point(148, 291);
            this.GlobalConfigFilename.Name = "GlobalConfigFilename";
            this.GlobalConfigFilename.ReadOnly = true;
            this.GlobalConfigFilename.Size = new System.Drawing.Size(565, 20);
            this.GlobalConfigFilename.TabIndex = 15;
            // 
            // TableConfigFilename
            // 
            this.TableConfigFilename.Location = new System.Drawing.Point(148, 326);
            this.TableConfigFilename.Name = "TableConfigFilename";
            this.TableConfigFilename.ReadOnly = true;
            this.TableConfigFilename.Size = new System.Drawing.Size(565, 20);
            this.TableConfigFilename.TabIndex = 16;
            // 
            // CabinetConfigFilename
            // 
            this.CabinetConfigFilename.Location = new System.Drawing.Point(148, 361);
            this.CabinetConfigFilename.Name = "CabinetConfigFilename";
            this.CabinetConfigFilename.ReadOnly = true;
            this.CabinetConfigFilename.Size = new System.Drawing.Size(565, 20);
            this.CabinetConfigFilename.TabIndex = 17;
            // 
            // ShowLoadedScripts
            // 
            this.ShowLoadedScripts.Location = new System.Drawing.Point(504, 387);
            this.ShowLoadedScripts.Name = "ShowLoadedScripts";
            this.ShowLoadedScripts.Size = new System.Drawing.Size(157, 23);
            this.ShowLoadedScripts.TabIndex = 18;
            this.ShowLoadedScripts.Text = "Show loaded scripts";
            this.ShowLoadedScripts.UseVisualStyleBackColor = true;
            this.ShowLoadedScripts.Click += new System.EventHandler(this.ShowLoadedScripts_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(178, 416);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(157, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Show available toys";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(341, 416);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(157, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "Show available effects";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 558);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ShowLoadedScripts);
            this.Controls.Add(this.CabinetConfigFilename);
            this.Controls.Add(this.TableConfigFilename);
            this.Controls.Add(this.GlobalConfigFilename);
            this.Controls.Add(this.TableRomname);
            this.Controls.Add(this.TableFilename);
            this.Controls.Add(this.TableName);
            this.Controls.Add(this.EditGlobalConfiguration);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ShowTableConfiguration);
            this.Controls.Add(this.ShowCabinetConfiguration);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainMenu";
            this.Text = "DirectOutput";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Version;
        private System.Windows.Forms.Button ShowCabinetConfiguration;
        private System.Windows.Forms.Button ShowTableConfiguration;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button EditGlobalConfiguration;
        private System.Windows.Forms.TextBox TableName;
        private System.Windows.Forms.TextBox TableFilename;
        private System.Windows.Forms.TextBox TableRomname;
        private System.Windows.Forms.TextBox GlobalConfigFilename;
        private System.Windows.Forms.TextBox TableConfigFilename;
        private System.Windows.Forms.TextBox CabinetConfigFilename;
        private System.Windows.Forms.Button ShowLoadedScripts;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}