using System.Runtime.InteropServices;

namespace PinballX
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GameInfo
    {

        public string System;
        public string SystemName;
        public string INI;
        public string GameName;
        public string GameDescription;
        public string GameShortDescription;
        public string TableFile;
        public string TablePath;
        public string Parameters;
        public int DisplayWidth;
        public int DisplayHeight;

        public int DisplayRotation;
        public bool DisplayWindowed;
        public int PlayFieldDisplayNbr;
        public int BackGlassDisplayNbr;
        public int DMDDisplayNbr;

        public string Filter;
        public string VideoImage_Playfield;
        public string VideoImage_BackGlass;
        public string VideoImage_DMD;

        public string VideoImage_Wheel;
        public string LastPlayed;
        public long TimesPlayed;
        public long SecondsPlayed;
        public string HighScore_1;
        public string HighScore_2;

        public string HighScore_3;

        public int Rating;
    }
}
