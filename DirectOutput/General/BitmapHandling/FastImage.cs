using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace DirectOutput.General.BitmapHandling
{
    public class FastImage:NamedItemBase
    {


        


        private Dictionary<int,FastBitmap> _Frames=new Dictionary<int,FastBitmap>();

        public Dictionary<int,FastBitmap> Frames
        {
            get { return _Frames; }
            private set { _Frames = value; }
        }

        public void LoadImageFile(string ImageFilePath)
        {
            Frames = new Dictionary<int, FastBitmap>();

            Image Img = Image.FromFile(ImageFilePath);

            FrameDimension dimension = new FrameDimension(Img.FrameDimensionsList[0]);
            // Number of frames
            int FrameCount = Img.GetFrameCount(dimension);
            // Return an Image at a certain index

            for (int FrameNumber = 0; FrameNumber < FrameCount; FrameNumber++)
            {
                Img.SelectActiveFrame(dimension, FrameNumber);

                FastBitmap F = new FastBitmap(Img);

                Frames.Add(FrameNumber, F);
            }

            Img.Dispose();
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
