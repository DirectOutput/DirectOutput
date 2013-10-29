using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectOutput.GlobalConfiguration
{
    /// <summary>
    /// A list of file patterns.
    /// </summary>
    public class FilePatternList : List<FilePattern>
    {
        /// <summary>
        /// Gets the files matching one of the entries in this list
        /// </summary>
        /// <param name="ReplaceValues">Dictionary containing key/value pairs used to replace placeholders in the form {PlaceHolder} in the patterns.</param>
        /// <returns>The list of files matching one of the entries in this list.</returns>
        public List<FileInfo> GetMatchingFiles(Dictionary<string, string> ReplaceValues=null)
        {
            List<FileInfo> L = new List<FileInfo>();
            foreach(FilePattern P in this) {
                List<FileInfo> PL = P.GetMatchingFiles(ReplaceValues);
                foreach (FileInfo FI in PL)
                {
                    if (!L.Any(x=>x.FullName==FI.FullName))
                    {
                        L.Add(FI);
                    }
                }
            }
            return L;
        }


        /// <summary>
        /// Gets the first matching file for the entries in the list.
        /// </summary>
        /// <param name="ReplaceValues">Dictionary containing key/value pairs used to replace placeholders in the form {PlaceHolder} in the patterns.</param>
        /// <returns>FileInfo object for the first file matching a entry in the list or null if no match is found.</returns>
        public FileInfo GetFirstMatchingFile(Dictionary<string, string> ReplaceValues = null)
        {
            foreach (FilePattern P in this)
            {
                FileInfo F = P.GetFirstMatchingFile(ReplaceValues);
                if (F != null)
                {
                    return F;
                }
            }
            return null;
        }


    }
}
