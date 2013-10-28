using System.IO;

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

}

