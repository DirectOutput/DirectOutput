using System;
using System.Collections.Generic;
using System.IO;

namespace DirectOutput.Scripting
{
    /// <summary>
    /// List of <see cref="Script" /> objects.
    /// </summary>
    public class ScriptList : IEnumerable<Script>
    {
        private List<Script> InternalList = new List<Script>();


        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Script> GetEnumerator()
        {
            return InternalList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// Gets the <see cref="Script"/> with the specified filename.
        /// </summary>
        /// <value>
        /// The <see cref="Script"/> object for the spiecifed file.
        /// </value>
        /// <param name="Filename">The filename of the script.</param>
        /// <returns></returns>
        public Script this[string Filename]
        {
            get
            {
                FileInfo FI = new FileInfo(Filename);
                foreach (Script S in InternalList)
                {
                    if (S.File.FullName == FI.FullName) return S;
                }
                return null;
            }
        }




        /// <summary>
        /// Gets the <see cref="Script"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="Script"/> at the specified index.
        /// </value>
        /// <param name="Index">The index of the object.</param>
        /// <returns></returns>
        public Script this[int Index]
        {
            get
            {
                return InternalList[Index];
            }
        }

        /// <summary>
        /// Gets the count of <see cref="Script"/> objects in the list.
        /// </summary>
        /// <value>
        /// The count of <see cref="Script"/> objects in the list.
        /// </value>
        public int Count
        {
            get { return InternalList.Count; }
        }

        /// <summary>
        /// Loads and adds scripts from the specified directory.
        /// </summary>
        /// <param name="ScriptDirectory">The path to the script directory.</param>
        /// <param name="FilePattern">The file search pattern.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> exceptions are thrown if errors occur during script loading and compiling or if a script has already been added to the list.</param>
        public void LoadAndAddScripts(string ScriptDirectory, string FilePattern = "*.cs", bool ThrowExceptions = false)
        {
            LoadAndAddScripts(new DirectoryInfo(ScriptDirectory), FilePattern, ThrowExceptions);
        }

        /// <summary>
        /// Loads and adds scripts from the specified directory.
        /// </summary>
        /// <param name="ScriptDirectory">The DirectoryInfo object for the script directory.</param>
        /// <param name="FilePattern">The file search pattern.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> exceptions are thrown if errors occur during script loading and compiling or if a script has already been added to the list.</param>
        public void LoadAndAddScripts(DirectoryInfo ScriptDirectory, string FilePattern = "*.cs", bool ThrowExceptions = false)
        {
            LoadAndAddScripts(ScriptDirectory.GetFiles(FilePattern), ThrowExceptions);
        }

        /// <summary>
        /// Loads and adds the scripts in the specified list of FileInfo objects.
        /// </summary>
        /// <param name="ScriptFiles">IEnumerable list of FIleInfo objects.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> exceptions are thrown if errors occur during script loading and compiling or if a script has already been added to the list.</param>
        public void LoadAndAddScripts(IEnumerable<FileInfo> ScriptFiles, bool ThrowExceptions = false)
        {
            foreach (FileInfo F in ScriptFiles)
            {
                LoadAndAddScript(F, ThrowExceptions);
            }
        }


        /// <summary>
        /// Load and adds the script specified in the FileInfo object.
        /// </summary>
        /// <param name="ScriptFile">The script file.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> exceptions are thrown if errors occur during script loading and compiling or if a script has already been added to the list.</param>
        public void LoadAndAddScript(FileInfo ScriptFile, bool ThrowExceptions = false)
        {

            foreach (Script S in InternalList)
            {
                if (S.File.FullName == ScriptFile.FullName)
                {
                    if (ThrowExceptions)
                    {
                        throw new Exception("Cold not add file {0} to the list. It does already exist in this list.".Build(ScriptFile.FullName));
                    }

                    return;
                }
            }


            InternalList.Add(new Script(ScriptFile, ThrowExceptions));
        }


    }
}
