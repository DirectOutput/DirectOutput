using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace System.IO
{
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
            if (DI.Exists) return;
            if (DI.Parent != null)
            {
                CreateDirectoryPath(DI.Parent);
            }
            if (!DI.Exists)
            {
                DI.Create();
            }




        }


        public static DirectoryInfo AppendPath(this DirectoryInfo DI, string PathToAppend)
        {
            return new DirectoryInfo(Path.Combine(DI.FullName, PathToAppend));
        }



        public static bool CheckExists(this DirectoryInfo DI)
        {
            return new DirectoryInfo(DI.FullName).Exists;
        }

        public static void DeleteAllDirectories(this DirectoryInfo DI)
        {
            foreach (DirectoryInfo D in DI.GetDirectories())
            {
                try
                {
                    DI.Delete(true);
                }
                catch
                {

                }
            }
        }


        public static void DeleteAllFiles(this DirectoryInfo DI)
        {
            foreach (FileInfo FI in DI.GetFiles())
            {
                try
                {
                    FI.Delete();
                }
                catch
                {

                }
            }
        }


    }
}
