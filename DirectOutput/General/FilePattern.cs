using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace DirectOutput.General
{
    /// <summary>
    /// A file pattern class used to lookup files matching a specified pattern.
    /// </summary>
    public class FilePattern : INotifyPropertyChanged, IXmlSerializable
    {


        #region IXmlSerializable Member
        /// <summary>
        /// Method is required by the IXmlSerializable interface
        /// </summary>
        /// <returns>Returns always null</returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Deserializes the FilePattern in the XmlReader.<br/>
        /// ReadXml is part of the IXmlSerializable interface.
        /// </summary>
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

        /// <summary>
        /// Serializes the FilePattern to Xml.<br/>
        /// WriteXml is part of the IXmlSerializable interface.
        /// </summary>
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

        private string _Pattern="";

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
        /// Returns the pattern with replaced placeholders.
        /// </summary>
        /// <param name="ReplaceValues">A dictionary containing the replace values.</param>
        /// <returns>Pattern with replaced placeholders.</returns>
        public string ReplacePlaceholders(Dictionary<string, string> ReplaceValues = null)
        {
            if (Regex.Match(Pattern, @"\{(DllDir|DllDirectory|AssemblyDir|AssemblyDirectory)\}").Success)
            {
                Log.Once("ReplacePlaceholders:" + this.Pattern,
                    "Warning: filename substitution variables {DllDir} and {AssemblyDir} are deprecated; "
                    + "use {InstallDir} for the install folder, or {BinDir} for the executing assembly's binary folder");
            }

            string P = Pattern;
            if (ReplaceValues != null)
            {
                foreach (KeyValuePair<string, string> KV in ReplaceValues)
                    P = P.Replace("{" + (KV.Key) + "}", KV.Value);
            }
            return P;
        }


        /// <summary>
        /// Gets the files matching the value of the property Pattern.
        /// </summary>
        /// <param name="ReplaceValues">Dictionary containing key/value pairs used to replace placeholders in the form {PlaceHolder} in the pattern.</param>
        /// <param name="ExcludeExtensions">List of file extensions which should be ignored (in the for ".ext")</param>
        /// <returns>The list of files matching the value of the property Pattern or a empty list if no file matches the pattern.</returns>
        public List<FileInfo> GetMatchingFiles(Dictionary<string, string> ReplaceValues = null, List<string> ExcludeExtensions = null)
        {
            if (Pattern.IsNullOrWhiteSpace()) return new List<FileInfo>();

            string P = ReplacePlaceholders(ReplaceValues);


            try
            {

                DirectoryInfo FilesDirectory = new DirectoryInfo((Path.GetDirectoryName(P).IsNullOrWhiteSpace() ? "." : Path.GetDirectoryName(P)));

                if (FilesDirectory.Exists) {
                    List<FileInfo> Files = FilesDirectory.GetFiles(Path.GetFileName(P)).ToList<FileInfo>();
                    Files.RemoveAll(FI => ExcludeExtensions != null &&
                                    ExcludeExtensions.Contains(FI.Extension, StringComparer.InvariantCultureIgnoreCase)
                                    );
                    return Files;
                } else
                {
                    return new List<FileInfo>();
                }
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
        /// <param name="ExcludeExtensions">List of file extensions which should be ignored (in the for ".ext")</param>
        /// <returns>The first file matching the value of the Pattern property or null if no file matches the pattern.</returns>
        public FileInfo GetFirstMatchingFile(Dictionary<string, string> ReplaceValues = null, List<string> ExcludeExtensions = null)
        {
            List<FileInfo> L = GetMatchingFiles(ReplaceValues, ExcludeExtensions);
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
                }
                catch
                {
                    return false;
                }

                bool BracketOpen = false;
                bool JustOpened = false;
                foreach (char C in Pattern)
                {
                    switch (C)
                    {
                        case '{':
                            if (BracketOpen) return false;
                            BracketOpen = true;
                            JustOpened = true;
                            break;
                        case '}':
                            if (!BracketOpen) return false;
                            if (JustOpened) return false;
                            BracketOpen = false;
                            JustOpened = false;
                            break;
                        case '\\':
                        case '*':
                        case '?':
                            if (BracketOpen) return false;
                            JustOpened = false;
                            break;
                        default:
                            JustOpened = false;
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
        public FilePattern()
        {

        }


    }
}
