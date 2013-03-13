using System.IO;

public static class FileInfoExtensions
{
    public static string GetNameWithoutExtension(this FileInfo f)
    {
        if (!f.Extension.IsNullOrWhiteSpace())
        {
            return f.Name.Left(f.Name.Length - f.Extension.Length);
        };
        return f.Name;
    }

}

