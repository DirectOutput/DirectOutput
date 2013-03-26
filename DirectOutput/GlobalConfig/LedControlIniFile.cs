using System.ComponentModel;
using System.IO;


namespace DirectOutput.GlobalConfig
{
    public class LedControlIniFile : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }



        #endregion


        private string _Filename = "";
        private FileInfo _File;

        public string Filename
        {
            get { return _Filename; }
            set
            {
                _Filename = value;
                try
                {
                    _File = new FileInfo(value);
                }
                catch
                {
                    _File = null;
                }
                NotifyPropertyChanged("FileName");
            }
        }

        public FileInfo File
        {
            get
            {
                return _File;
            }
        }

        public bool FileExists
        {
            get
            {
                if (File != null)
                {
                    return _File.Exists;
                }
                return false;
            }
        }

        public string FileStatus
        {
            get
            {
                if (Filename.IsNullOrWhiteSpace())
                {
                    return "No file set.";
                }
                if (FileExists)
                {
                    return "OK";
                }
                else
                {
                    return "File does not exist";
                }
            }
        }


        private int _LedWizNumber = 99999;


        public int LedWizNumber
        {
            get { return _LedWizNumber; }
            set
            {
                _LedWizNumber = value;
                NotifyPropertyChanged("LedWizNumber");
            }
        }


        public override string ToString()
        {
            return "{0} (LedWiz:{1})".Build(Filename,LedWizNumber);
        }

        public LedControlIniFile(string Filename)
        {
            this.Filename = Filename;
            this.LedWizNumber = int.MaxValue;
        }

        public LedControlIniFile() { }


    }
}
