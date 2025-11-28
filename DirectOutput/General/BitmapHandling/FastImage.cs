using DirectOutput.General.Generic;
using Q42.HueApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

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

        private bool _UseCache = false;

        public bool UseCache
        {
            get { return _UseCache; }
            set { _UseCache = value; }
        }

        public static string RawImageExtension => ".rawimage";

        private static string GetCachedImagePath(string ImageFilePath)
        {
            return Path.Combine(
                Path.GetDirectoryName(ImageFilePath),
                "cache") + Path.DirectorySeparatorChar + Path.GetFileName(ImageFilePath) + RawImageExtension;
        }

        private bool LoadCachedImage(string ImageFilePath)
        {
            string CachedImageFilePath = GetCachedImagePath(ImageFilePath);
            try {
                if (File.Exists(CachedImageFilePath)) {
                    if (File.GetLastWriteTimeUtc(CachedImageFilePath) > File.GetLastWriteTimeUtc(ImageFilePath)) {
                        using (var stream = File.Open(CachedImageFilePath, FileMode.Open, FileAccess.Read)) {
                            using (var reader = new BinaryReader(stream, Encoding.UTF8, false)) {
                                Log.Instrumentation("Image", $"Loading cached image file {CachedImageFilePath}.");
                                int FrameCount = reader.ReadInt32();

                                for (int FrameNumber = 0; FrameNumber < FrameCount; FrameNumber++) {
                                    FastBitmap F = new FastBitmap();
                                    F.Load(reader);
                                    Frames.Add(FrameNumber, F);
                                }

                                Log.Instrumentation("Image", $"Loaded cached image file {FrameCount} frames.");
                                return true;
                            }
                        }
                    }
                }
            } catch (Exception E) {
                throw new Exception($"Could not load cached image file {CachedImageFilePath} to the FastBitmapList.\nYou can delete it so it can ge generated again.\n{E}");
            }

            return false;
        }

        private void SaveCachedImage(string ImageFilePath)
        {
            string CachedImageFilePath = GetCachedImagePath(ImageFilePath);
            try {
                if (!Directory.Exists(Path.GetDirectoryName(CachedImageFilePath))){
                    Directory.CreateDirectory(Path.GetDirectoryName(CachedImageFilePath));
                }
                using (var stream = File.Open(CachedImageFilePath, FileMode.Create)) {
                    using (var writer = new BinaryWriter(stream, Encoding.UTF8, false)) {
                        Log.Instrumentation("Image", $"Saving cached image file {CachedImageFilePath}, {Frames.Count} frames.");
                        writer.Write(Frames.Count);
                        for (int FrameNumber = 0; FrameNumber < Frames.Count; FrameNumber++) {
                            FastBitmap bitMap = Frames[FrameNumber];
                            writer.Write(bitMap.Width);
                            writer.Write(bitMap.Height);
                            for (int y = 0; y < bitMap.Height; y++) {
                                for (int x = 0; x < bitMap.Width; x++) {
                                    PixelData pixelData = bitMap.GetPixel(x, y);
                                    writer.Write(pixelData.Alpha);
                                    writer.Write(pixelData.Red);
                                    writer.Write(pixelData.Green);
                                    writer.Write(pixelData.Blue);
                                }
                            }
                        }
                        Log.Instrumentation("Image", $"{CachedImageFilePath} saved (size {stream.Length} bytes).");
                    }
                }
            } catch (Exception E) {
                throw new Exception($"Could not save cached image file {CachedImageFilePath}.\n{E}");
            }
        }

        public void LoadImageFile(string ImageFilePath)
        {
            Frames = new Dictionary<int, FastBitmap>();

            if (!UseCache || !LoadCachedImage(ImageFilePath)) 
            {
                Log.Instrumentation("Image", $"Loading image file {ImageFilePath}.");
                Image Img = Image.FromFile(ImageFilePath);

                FrameDimension dimension = new FrameDimension(Img.FrameDimensionsList[0]);
                // Number of frames
                int FrameCount = Img.GetFrameCount(dimension);
                // Return an Image at a certain index

                Log.Instrumentation("Image", $"Extracting {FrameCount} frames from image.");
                for (int FrameNumber = 0; FrameNumber < FrameCount; FrameNumber++) {
                    Img.SelectActiveFrame(dimension, FrameNumber);

                    FastBitmap F = new FastBitmap(Img);

                    Frames.Add(FrameNumber, F);
                }

                Img.Dispose();
                Log.Instrumentation("Image", $"Image loaded.");

                if (UseCache) {
                    SaveCachedImage(ImageFilePath);
                }
            }
        }


        void FastBitmap_AfterNameChanged(object sender, NameChangeEventArgs e)
        {
            LoadImageFile(e.NewName);
        }


        public FastImage(string Name, bool UseCache = false) :this()
        {
            this.UseCache = UseCache;
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
