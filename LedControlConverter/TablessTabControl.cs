using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace System.Windows.Forms
{
    class TablessTabControl : TabControl
    {
        protected override void WndProc(ref Message m)
        {
            // Hide tabs by trapping the TCM_ADJUSTRECT message
            if (!TabsVisible && m.Msg == 0x1328 && !DesignMode) m.Result = (IntPtr)1;
            else base.WndProc(ref m);
          
        }

        bool _TabsVisible = false;
        public bool TabsVisible
        {
            get { return _TabsVisible; }
            set
            {
                if (_TabsVisible != value)
                {
                    _TabsVisible = value;
                    this.Refresh();
                }
            }
        }



    }
}