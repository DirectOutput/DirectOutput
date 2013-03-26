using System.IO;

namespace DirectOutput.General
{

    /// <summary>
    /// Static class to read files.
    /// </summary>
    public static class FileReader
    {

         /// <summary>
        /// Reads the content of a file into a string. 
        /// </summary>
        /// <param name="File">FileInfo object for the file to be read.</param>
        /// <returns>string containing the contents of the file.</returns>
        public static string ReadFileToString(FileInfo File)
        {
            return ReadFileToString(File.FullName);
        }


        /// <summary>
        /// Reads the content of a file into a string. 
        /// </summary>
        /// <param name="Filename">Name of the file</param>
        /// <returns>string containing the contents of the file.</returns>
        public static string ReadFileToString(string Filename)
        {
            StreamReader streamReader = new StreamReader(Filename);
            string Data = streamReader.ReadToEnd();
            streamReader.Close();
            streamReader.Dispose();
            return Data;
        }


    }
}
