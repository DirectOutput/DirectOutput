using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;

namespace DirectOutput.General.Bitmap
{
    public class FastImage:NamedItemBase
    {


        


        private Dictionary<int,FastBitmap> _Frames=new Dictionary<int,FastBitmap>();

        public Dictionary<int,FastBitmap> Frames
        {
            get { return _Frames; }
            private set { _Frames = value; }
        }

        public void LoadImageFile(string p)
        {
            throw new NotImplementedException();
        }


        void FastBitmap_AfterNameChanged(object sender, NameChangeEventArgs e)
        {
            LoadImageFile(e.NewName);
        }


        public FastImage(string Name) :this()
        {
            this.Name = Name;
        }

        public FastImage():base()
        {
            this.AfterNameChanged += new EventHandler<NameChangeEventArgs>(FastBitmap_AfterNameChanged);

        }

        ~FastImage()
        {
            this.AfterNameChanged -= new EventHandler<NameChangeEventArgs>(FastBitmap_AfterNameChanged);
        }

    }
}
