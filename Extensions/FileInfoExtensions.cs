using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System;


/// <summary>
/// Extends the FileInfo object with additional functionality.
/// </summary>
public static class FileInfoExtensions
{
    /// <summary>
    /// Gets the name of the file specified in the object without extension.
    /// </summary>
    /// <param name="f">The f.</param>
    /// <returns>The file name without extension.</returns>
    public static string GetNameWithoutExtension(this FileInfo f)
    {
        if (f == null) return "";
        if (!f.Extension.IsNullOrWhiteSpace())
        {
            return f.Name.Left(f.Name.Length - f.Extension.Length);
        };
        return f.Name;
    }



    /// <summary>
    /// Reads the content of the file into a string. 
    /// </summary>
    /// <returns>string containing the contents of the file.</returns>
    public static string ReadFileToString(this FileInfo f)
    {
        StreamReader streamReader = f.OpenText();
        string Data = streamReader.ReadToEnd();
        streamReader.Close();
        streamReader.Dispose();
        return Data;
    }

    /// <summary>
    /// Checks if the file really exists.
    /// </summary>
    /// <returns><c>true</c> if the file exists, otherwise <c>false</c>.</returns>
    public static bool CheckExists(this FileInfo FI)
    {
        return new FileInfo(FI.FullName).Exists;
    }

    /// <summary>
    /// Gets the MD5 hash of the file.
    /// </summary>
    /// <param name="FI">The FI.</param>
    /// <returns>MD5 hash of the file.</returns>
    public static string GetMD5Hash(this FileInfo FI)
    {
        byte[] hashBytes = null;
        try
        {
            using (var inputFileStream = FI.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                hashBytes = md5.ComputeHash(inputFileStream);
            }
            return string.Join("", hashBytes.Select(B => "{0:x2}".Build(B)).ToArray());

        }
        catch (Exception E)
        {

            throw new Exception("Cant calculate MD5 has of file {0}.".Build(FI.FullName), E);
        }
    }


    /// <summary>
    /// Deserializes the xml content of the file as a object of the given type.
    /// </summary>
    /// <typeparam name="T">Type of the object</typeparam>
    /// <returns></returns>
    public static T XMLDeserialize<T>(this FileInfo FI)
    {
        using (FileStream ms = FI.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            try
            {
                return (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(ms);
            }
            catch (Exception E)
            {

                Exception Ex = new Exception("Could not deserialize {0} from XML data in file: {1}".Build(typeof(T).Name, FI.FullName), E);
                throw Ex;
            }
        }

    }

    /// <summary>
    /// Determines whether the file is locked.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <returns>
    ///   <c>true</c> if [is file locked] [the specified file]; otherwise, <c>false</c>.
    /// </returns>
    public static bool CheckFileLocked(this FileInfo file)
    {
        FileStream stream = null;

        try
        {
            stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        catch (IOException)
        {
            if (!file.CheckExists())
            {
                throw new FileNotFoundException("File {0} not found".Build(file.FullName), file.FullName);
            }
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }
        finally
        {
            if (stream != null)
                stream.Close();
        }

        //file is not locked
        return false;
    }


    public static bool CheckCanRead(this FileInfo file)
    {
        FileStream stream;
        try
        {
            // try to open the file to check if we can access it for read
            stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read);
            stream.Dispose();
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Moves the file to the specified destination.
    /// </summary>
    /// <param name="Source">The source.</param>
    /// <param name="Dest">The dest.</param>
    public static void MoveTo(this FileInfo Source, FileInfo Dest)
    {
        Source.MoveTo(Dest.FullName);
    }

    /// <summary>
    /// Copies the file to the specified destination.
    /// </summary>
    /// <param name="Source">The source.</param>
    /// <param name="Dest">The dest.</param>
    public static void CopyTo(this FileInfo Source, FileInfo Dest)
    {
        Source.CopyTo(Dest.FullName);
    }
}


