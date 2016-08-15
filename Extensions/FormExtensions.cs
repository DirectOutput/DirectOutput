using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace System.Windows.Forms
{
    public static class FormExtensions
    {
        public static void CenterFormOnMouse(this Form Form)
        {

            Point MousePos = Cursor.Position;
            Rectangle R = Screen.FromPoint(MousePos).WorkingArea;

            int X = (Cursor.Position.X  - Form.Width / 2);
            int Y = (Cursor.Position.Y  - Form.Height / 2);


            X = X.Limit(R.Left+10, (R.Right - Form.Width-10).Limit(0,R.Right-10));
            Y = Y.Limit(R.Top+10, (R.Bottom - Form.Height-10).Limit(0,R.Bottom-10));


            Form.SetDesktopLocation(X, Y);
        }


    }
}
