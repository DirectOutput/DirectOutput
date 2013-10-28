using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


/// <summary>
/// Extends the DirectoryInfo calls with additional functionality.
/// </summary>
public static class DirectoryInfoExtensions
{

    /// <summary>
    /// Creates the full path for a directory.
    /// </summary>
    public static void CreateDirectoryPath(this DirectoryInfo DI)
    {
        if (DI.Parent != null)
        {
            CreateDirectoryPath(DI.Parent);
        }
        if (!DI.Exists)
        {
            DI.Create();
        }
    }

}

