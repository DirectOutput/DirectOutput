using System;
using System.IO;
using System.Reflection;
using CSScriptLibrary;

namespace DirectOutput.Scripting
{

    /// <summary>
    /// Contains information on a loaded and compiled C# script file. 
    /// </summary>
    public class Script
    {
        /// <summary>
        /// FileInfo object for the Scriptfile.
        /// </summary>
        public FileInfo File { get; private set; }


        /// <summary>
        /// Indicates whether the script file has been successfully loaded and compiled.
        /// </summary>
        public bool Compiled
        {
            get
            {
                return (CompilationException == null);
            }
        }

        /// <summary>
        /// If a error occurs during compilation, this property contains the Exception which was thrown. 
        /// </summary>
        public Exception CompilationException { get; private set; }


        /// <summary>
        /// Holds a reference to the Assembly for the loaded script file.
        /// </summary>
        public Assembly Assembly { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Script" /> class.<br />
        /// Loads and compiles the specified script file.
        /// </summary>
        /// <param name="ScriptFilename">The script filename.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> a exception will be thrown, if a error occurs when loading and compiling the script file.</param>
        /// <exception cref="System.Exception">A error occured while loading or loading the script file {0}.</exception>
        public Script(string ScriptFilename, bool ThrowExceptions = false) : this(new FileInfo(ScriptFilename), ThrowExceptions) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Script" /> class.<br />
        /// Loads and compiles the specified script file.
        /// </summary>
        /// <param name="ScriptFile">The FileInfo object for the script file.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> a exception will be thrown, if a error occurs when loading and compiling the script file.</param>
        /// <exception cref="System.Exception">A error occured while loading or loading the script file {0}.</exception>
        public Script(FileInfo ScriptFile, bool ThrowExceptions = false)
        {
            this.File = ScriptFile;


            Assembly A = null;
            try
            {
                A = CSScript.Load(ScriptFile.FullName, null, true);
                Assembly = A;
            }
            catch (Exception e)
            {
                CompilationException = e;
                if (ThrowExceptions)
                {
                    throw new Exception("A error occured while loading or loading the script file {0}.".Build(ScriptFile.FullName),e);
                }
            }


        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Script"/> class.
        /// </summary>
        public Script() { }


    }
}
