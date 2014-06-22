namespace UpdateNotification
{
    partial class Notification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Notification));
            this.NoticficationRotated = new System.Windows.Forms.PictureBox();
            this.NotificationNormal = new System.Windows.Forms.PictureBox();
            this.CloseTimer = new System.Windows.Forms.Timer(this.components);
            this.TopTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.NoticficationRotated)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotificationNormal)).BeginInit();
            this.SuspendLayout();
            // 
            // NoticficationRotated
            // 
            this.NoticficationRotated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NoticficationRotated.Image = ((System.Drawing.Image)(resources.GetObject("NoticficationRotated.Image")));
            this.NoticficationRotated.Location = new System.Drawing.Point(0, 0);
            this.NoticficationRotated.Name = "NoticficationRotated";
            this.NoticficationRotated.Size = new System.Drawing.Size(804, 792);
            this.NoticficationRotated.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.NoticficationRotated.TabIndex = 0;
            this.NoticficationRotated.TabStop = false;
            // 
            // NotificationNormal
            // 
            this.NotificationNormal.BackColor = System.Drawing.Color.Transparent;
            this.NotificationNormal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NotificationNormal.Image = ((System.Drawing.Image)(resources.GetObject("NotificationNormal.Image")));
            this.NotificationNormal.Location = new System.Drawing.Point(0, 0);
            this.NotificationNormal.Name = "NotificationNormal";
            this.NotificationNormal.Size = new System.Drawing.Size(804, 792);
            this.NotificationNormal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.NotificationNormal.TabIndex = 1;
            this.NotificationNormal.TabStop = false;
            // 
            // CloseTimer
            // 
            this.CloseTimer.Interval = 6000;
            this.CloseTimer.Tick += new System.EventHandler(this.CloseTimer_Tick);
            // 
            // TopTimer
            // 
            this.TopTimer.Interval = 2000;
            this.TopTimer.Tick += new System.EventHandler(this.TopTimer_Tick);
            // 
            // Notification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(804, 792);
            this.ControlBox = false;
            this.Controls.Add(this.NotificationNormal);
            this.Controls.Add(this.NoticficationRotated);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Notification";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UpdateNotfication";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Black;
            this.Shown += new System.EventHandler(this.UpdateNotfication_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.NoticficationRotated)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotificationNormal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox NoticficationRotated;
        private System.Windows.Forms.PictureBox NotificationNormal;
        private System.Windows.Forms.Timer CloseTimer;
        private System.Windows.Forms.Timer TopTimer;

    }
}