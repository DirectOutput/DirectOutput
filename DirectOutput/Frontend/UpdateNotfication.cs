using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectOutput.Frontend
{
    public partial class UpdateNotfication : Form
    {
        public UpdateNotfication()
        {
            InitializeComponent();
        }



        static public void ShowNotification()
        {
            UpdateNotfication U = new UpdateNotfication();
            U.CenterToScreen();
            U.Show();




        }

        private void UpdateNotfication_Shown(object sender, EventArgs e)
        {
            Screen Screen = Screen.FromControl(this);

            if ((Screen.AllScreens.Length > 1 && Screen.FromControl(this).Primary))
            {
                //Show rotated version
                NotificationNormal.Visible = false;
                NoticficationRotated.Visible = true;
            }
            else
            {
                //Show normal version
                NotificationNormal.Visible = true;
                NoticficationRotated.Visible = false;
            }

            this.BringToFront();
            CloseTimer.Start();
            TopTimer.Start();
            
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TopTimer_Tick(object sender, EventArgs e)
        {
            this.BringToFront();
            
        }
    }
}
