using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace DirectOutput.GlobalConfig
{
    /// <summary>
    /// A file pattern class used to lookup files matching a specified pattern.
    /// </summary>
    public class FilePattern : INotifyPropertyChanged, IXmlSerializable
    {


        #region IXmlSerializable Member

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            Pattern = reader.ReadString();
            if (!reader.IsEmptyElement)
            {
                reader.ReadEndElement();
            }
            else
            {
                reader.ReadStartElement();
            }
        }


        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteString(Pattern);
        }

        #endregion

        #region INotifyPropertyChanged Member

        /// <summary>
        /// Is fired if the value of a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        #endregion

        private string _Pattern;

        /// <summary>
        /// Gets or sets the pattern used to look for files.
        /// </summary>
        /// <value>
        /// The pattern used to look for files.
        /// </value>
        public string Pattern
        {
            get { return _Pattern; }
            set
            {
                if (_Pattern != value)
                {
                    _Pattern = value;
                    NotifyPropertyChanged("Pattern");
                }
            }
        }

        /// <summary>
        /// Gets the files matching the value of the property Pattern.
        /// </summary>
        /// <param name="ReplaceValues">Dictionary containing key/value pairs used to replace placeholders in the form {PlaceHolder} in the pattern.</param>
        /// <returns>The list of files matching the value of the property Pattern or a empty list if no file matches the pattern.</returns>
        public List<FileInfo> GetMatchingFiles(Dictionary<string, string> ReplaceValues = null)
        {
            if (Pattern.IsNullOrWhiteSpace()) return new List<FileInfo>();

            string P = Pattern;
            if (ReplaceValues != null)
            {
                foreach (KeyValuePair<string, string> KV in ReplaceValues)
                {
                    P = P.Replace("{"+(KV.Key)+"}", KV.Value);
                }
            }
            try
            {
                
                DirectoryInfo FilesDirectory = new DirectoryInfo((Path.GetDirectoryName(P).IsNullOrWhiteSpace()?".":Path.GetDirectoryName(P)));
                return FilesDirectory.GetFiles(Path.GetFileName(P)).ToList<FileInfo>();
            }
            catch
            {
                return new List<FileInfo>();
            }
        }

        /// <summary>
        /// Gets the first file matching the value of the Pattern property.
        /// </summary>
        /// <param name="ReplaceValues">Dictionary containing key/value pairs used to replace placeholders in the form {PlaceHolder} in the pattern.</param>
        /// <returns>The first file matching the value of the Pattern property or null if no file matches the pattern.</returns>
        public FileInfo GetFirstMatchingFile(Dictionary<string, string> ReplaceValues = null)
        {
            List<FileInfo> L = GetMatchingFiles(ReplaceValues);
            if (L.Count > 0)
            {
                return L[0];
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Gets a value indicating whether the Pattern is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the Pattern is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid
        {
            get
            {
                if (Pattern.IsNullOrWhiteSpace()) return true;

                try
                {
                    FileInfo DummyFileInfo = new FileInfo(Pattern.Replace("*", "test").Replace("?", "X"));
                } catch {
                    return false;
                }

                bool BracketOpen = false;
                foreach (char C in Pattern)
                {
                    switch (C)
                    {
                        case '{':
                            if (BracketOpen) return false;
                            BracketOpen = true;
                            break;
                        case '}':
                            if (!BracketOpen) return false;
                            BracketOpen = false;
                            break;
                        case '\\':
                        case '*':
                        case '?':
                            if (BracketOpen) return false;
                            break;
                    }
                }
                if (BracketOpen) return false;
                return true;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="FilePattern"/> class.
        /// </summary>
        /// <param name="Pattern">The file pattern.</param>
        public FilePattern(string Pattern)
        {
            if (Pattern != null)
            {
                this.Pattern = Pattern;
            }

        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Pattern;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="FilePattern"/> class.
        /// </summary>
        public FilePattern() { 
        
        }


    }
}
