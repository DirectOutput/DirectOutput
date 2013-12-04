using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.General.Bitmap
{
    public class FastBitmap
    {
        


        private Dictionary<int,FastBitmapFrame> _Frames=new Dictionary<int,FastBitmapFrame>();

        public Dictionary<int,FastBitmapFrame> Frames
        {
            get { return _Frames; }
            private set { _Frames = value; }
        }
        




    }
}
