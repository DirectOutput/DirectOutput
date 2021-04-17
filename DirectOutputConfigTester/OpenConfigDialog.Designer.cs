namespace DirectOutputConfigTester
{
    partial class OpenConfigDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenConfigDialog));
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OpenGlobalConfigFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.GlobalConfigFileSelectButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.TableFileSelectButton = new System.Windows.Forms.Button();
            this.RomNameComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.OpenTableFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.TableFileClearButton = new System.Windows.Forms.Button();
            this.GlobalConfigFilenameComboBox = new System.Windows.Forms.ComboBox();
            this.TableFilenameComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(110, 105);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(159, 23);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(484, 105);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(150, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // OpenGlobalConfigFileDialog
            // 
            this.OpenGlobalConfigFileDialog.DefaultExt = "xml";
            this.OpenGlobalConfigFileDialog.Filter = "XML files (*.xml)|*.xml|All files|*.*";
            this.OpenGlobalConfigFileDialog.Title = "Select a global config file";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Global config file:";
            // 
            // GlobalConfigFileSelectButton
            // 
            this.GlobalConfigFileSelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GlobalConfigFileSelectButton.Location = new System.Drawing.Point(640, 5);
            this.GlobalConfigFileSelectButton.Name = "GlobalConfigFileSelectButton";
            this.GlobalConfigFileSelectButton.Size = new System.Drawing.Size(45, 23);
            this.GlobalConfigFileSelectButton.TabIndex = 4;
            this.GlobalConfigFileSelectButton.Text = "Select";
            this.GlobalConfigFileSelectButton.UseVisualStyleBackColor = true;
            this.GlobalConfigFileSelectButton.Click += new System.EventHandler(this.GlobalConfigFileSelectButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Table file:";
            // 
            // TableFileSelectButton
            // 
            this.TableFileSelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TableFileSelectButton.Location = new System.Drawing.Point(640, 36);
            this.TableFileSelectButton.Name = "TableFileSelectButton";
            this.TableFileSelectButton.Size = new System.Drawing.Size(45, 23);
            this.TableFileSelectButton.TabIndex = 8;
            this.TableFileSelectButton.Text = "Select";
            this.TableFileSelectButton.UseVisualStyleBackColor = true;
            this.TableFileSelectButton.Click += new System.EventHandler(this.TableFileSelectButton_Click);
            // 
            // RomNameComboBox
            // 
            this.RomNameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RomNameComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.RomNameComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.RomNameComboBox.FormattingEnabled = true;
            this.RomNameComboBox.Items.AddRange(new object[] {
            "",
            "abv106",
            "afm",
            "ali",
            "alienstr",
            "alpok",
            "amazonh",
            "apollo13",
            "arena",
            "atlantis",
            "austin",
            "babypac",
            "badgirls",
            "barbwire",
            "barra",
            "batmanf",
            "baywatch",
            "bbb109",
            "bcats",
            "beatclck",
            "bguns",
            "biggame",
            "bighouse",
            "bighurt",
            "bk",
            "bk2k",
            "blackblt",
            "blackjck",
            "blakpyra",
            "blckhole",
            "blkou",
            "bnzai",
            "bonebstr",
            "bop",
            "bowarrow",
            "br",
            "brvteam",
            "bsv103",
            "btmn",
            "bttf",
            "bullseye",
            "canasta",
            "cc",
            "centaur",
            "cftbl",
            "charlies",
            "ckpt",
            "clas1812",
            "closeenc",
            "comet",
            "congo",
            "corv",
            "csmic",
            "cueball",
            "cv",
            "cybrnaut",
            "cycln",
            "cyclopes",
            "dd",
            "deadweap",
            "dh",
            "diamond",
            "diner",
            "disco",
            "dm",
            "dollyptb",
            "drac",
            "dracula",
            "dragon",
            "dvlrider",
            "dw",
            "eatpm",
            "eballchp",
            "eballdlx",
            "eightbll",
            "eldorado",
            "elvis",
            "embryon",
            "esha",
            "f14",
            "faeton",
            "fathom",
            "fbclass",
            "ffv104",
            "fh",
            "flash",
            "flashgdn",
            "flight2k",
            "frankst",
            "freddy",
            "freefall",
            "frontier",
            "frpwr",
            "fs",
            "ft",
            "futurspa",
            "galaxy",
            "genesis",
            "genie",
            "gi",
            "gldneye",
            "gnr",
            "godzilla",
            "goldcue",
            "gprix",
            "grand",
            "grgar",
            "gs",
            "gw",
            "hd",
            "hglbtrtb",
            "hh",
            "hirolcas",
            "hook",
            "hothand",
            "hs",
            "hurr",
            "i500",
            "icefever",
            "id4",
            "ij",
            "ind250cc",
            "ironmaid",
            "jamesb2",
            "jb",
            "jd",
            "jm",
            "jokrz",
            "jolypark",
            "jplstw22",
            "jupk",
            "jy",
            "kissb",
            "kosteel",
            "kpv",
            "lah",
            "lectrono",
            "lightnin",
            "lostspc",
            "lostwrld",
            "lotr",
            "lsrcu",
            "lw3",
            "m",
            "matahari",
            "mav",
            "mb",
            "medusa",
            "mephisto",
            "metalman",
            "mm",
            "monopoly",
            "mousn",
            "mysticb",
            "nascar",
            "nbaf",
            "nf",
            "ngg",
            "ngndshkr",
            "nineball",
            "panthera",
            "paragon",
            "pb",
            "pharo",
            "pinchamp",
            "pinpool",
            "play",
            "playboyb",
            "playboys",
            "pmv112",
            "pnkpnthr",
            "polic",
            "pop",
            "poto",
            "princess",
            "prtyanim",
            "pwerplay",
            "pz",
            "qbquest",
            "rab",
            "radcl",
            "raven",
            "rctycn",
            "rdkng",
            "rescu911",
            "rflshdlx",
            "ripleys",
            "roadrunr",
            "robo",
            "robot",
            "robowars",
            "rocky",
            "rollr",
            "rollstob",
            "rs",
            "rvrbt",
            "seawitch",
            "sfight2",
            "simp",
            "simpprty",
            "slbmania",
            "smb3",
            "solaride",
            "sopranos",
            "sorcr",
            "spaceinv",
            "spcrider",
            "spectru4",
            "spidermn7",
            "spirit",
            "sprk",
            "SS",
            "sshooter",
            "sshtl",
            "sst",
            "ssvc",
            "stargat4",
            "stargoda",
            "stars",
            "startrek",
            "startrp",
            "stk",
            "strngsci",
            "sttng",
            "stwr",
            "swrds",
            "swtril43",
            "t2",
            "taf",
            "taxi",
            "teedoff3",
            "term3",
            "tftc",
            "tmac",
            "tmnt",
            "tom",
            "tomy",
            "totan",
            "trailer",
            "trek",
            "trident",
            "trucksp3",
            "ts",
            "tstrk",
            "twst",
            "tz",
            "vegas",
            "viprsega",
            "vortex",
            "wcs",
            "wd",
            "whirl",
            "wipeout",
            "wrldtou2",
            "ww",
            "wwfr",
            "Xenon",
            "xfiles"});
            this.RomNameComboBox.Location = new System.Drawing.Point(110, 67);
            this.RomNameComboBox.Name = "RomNameComboBox";
            this.RomNameComboBox.Size = new System.Drawing.Size(524, 21);
            this.RomNameComboBox.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "RomName:";
            // 
            // OpenTableFileDialog
            // 
            this.OpenTableFileDialog.DefaultExt = "vpt";
            this.OpenTableFileDialog.Filter = "VP Table file (*.vpt;*.vpx)|*.vpt;*.vpx|All files|*.*";
            this.OpenTableFileDialog.Title = "Select a pinball table file";
            // 
            // TableFileClearButton
            // 
            this.TableFileClearButton.Location = new System.Drawing.Point(687, 36);
            this.TableFileClearButton.Name = "TableFileClearButton";
            this.TableFileClearButton.Size = new System.Drawing.Size(50, 23);
            this.TableFileClearButton.TabIndex = 11;
            this.TableFileClearButton.Text = "Clear";
            this.TableFileClearButton.UseVisualStyleBackColor = true;
            this.TableFileClearButton.Click += new System.EventHandler(this.TableFileClearButton_Click);
            // 
            // GlobalConfigFilenameComboBox
            // 
            this.GlobalConfigFilenameComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.GlobalConfigFilenameComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.GlobalConfigFilenameComboBox.FormattingEnabled = true;
            this.GlobalConfigFilenameComboBox.Location = new System.Drawing.Point(110, 9);
            this.GlobalConfigFilenameComboBox.MaxDropDownItems = 16;
            this.GlobalConfigFilenameComboBox.Name = "GlobalConfigFilenameComboBox";
            this.GlobalConfigFilenameComboBox.Size = new System.Drawing.Size(520, 21);
            this.GlobalConfigFilenameComboBox.TabIndex = 12;
            // 
            // TableFilenameComboBox
            // 
            this.TableFilenameComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.TableFilenameComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.TableFilenameComboBox.FormattingEnabled = true;
            this.TableFilenameComboBox.Location = new System.Drawing.Point(110, 37);
            this.TableFilenameComboBox.Name = "TableFilenameComboBox";
            this.TableFilenameComboBox.Size = new System.Drawing.Size(520, 21);
            this.TableFilenameComboBox.TabIndex = 13;
            // 
            // OpenConfigDialog
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 141);
            this.Controls.Add(this.TableFilenameComboBox);
            this.Controls.Add(this.GlobalConfigFilenameComboBox);
            this.Controls.Add(this.TableFileClearButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RomNameComboBox);
            this.Controls.Add(this.TableFileSelectButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.GlobalConfigFileSelectButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OpenConfigDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Open Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private new System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.OpenFileDialog OpenGlobalConfigFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button GlobalConfigFileSelectButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button TableFileSelectButton;
        private System.Windows.Forms.ComboBox RomNameComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog OpenTableFileDialog;
        private System.Windows.Forms.Button TableFileClearButton;
        private System.Windows.Forms.ComboBox GlobalConfigFilenameComboBox;
        private System.Windows.Forms.ComboBox TableFilenameComboBox;
    }
}