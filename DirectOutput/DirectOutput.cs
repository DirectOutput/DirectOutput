using System.IO;
using System.Reflection;
using System;
using System.Windows.Forms.VisualStyles;
using System.Collections.Generic;

// <summary>
// DirectOutput is the root namespace for the DirectOutput framework. 
// </summary>
namespace DirectOutput
{
    /// <summary>
    /// Static class providing simple access to the DirectOutput framework functionality.
    /// </summary>
    public static class DirectOutputHandler
    {

        /// <summary>
        /// Gets the version of the DirectOutput framework.
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Receives data to be processed by the DirectOutput framework.<br/>
        /// This function has to be called whenever the state of a table element changes in the hosting application.
        /// </summary>
        /// <param name="TableElementTypeChar">The table element type char.</param>
        /// <param name="Number">The number of the table element.</param>
        /// <param name="Value">The value of the table element.</param>
        /// <exception cref="System.Exception">
        /// Could not extract the first char of the TableElementTypeChar parameter
        /// or
        /// You must call Init before passing data to the DirectOutput framework
        /// or
        /// A exception occurred when passing in data (TableElementTypeChar: {0}, Number: {1}, Value: {2})
        /// </exception>
        public static void UpdateTableElement(string TableElementTypeChar, int Number, int Value)
        {
            char C;
            try
            {
                C = TableElementTypeChar[0];
            }
            catch (Exception E)
            {

                throw new Exception("Could not extract the first char of the TableElementTypeChar parameter", E);
            }
            try
            {
                Pinball.ReceiveData(C, Number, Value);
            }
            catch (Exception E)
            {
                if (Pinball == null)
                {
                    throw new Exception("You must call Init before passing data to the DirectOutput framework");
                }
                else
                {
                    throw new Exception("A exception occurred when passing in data (TableElementTypeChar: {0}, Number: {1}, Value: {2})".Build(C, Number, Value), E);
                }
            }
        }


        /// <summary>
        /// Receives  data for named table elements.
        /// The received data is put in a queue and the internal thread of the framework is notified about the availability of new data.
        /// </summary>
        /// <param name="TableElementName">Name of the table element.</param>
        /// <param name="Value">The value of the table element.</param>
        /// <exception cref="System.Exception">
        /// You must call Init before passing data to the DirectOutput framework
        /// or
        /// A exception occurred when passing in data (TableElementName: {0}, Value: {1}).Build(TableElementName, Value)
        /// </exception>
        public static void UpdateNamedTableElement(string TableElementName, int Value)
        {
            try
            {
                Pinball.ReceiveData(TableElementName, Value);
            }
            catch (Exception E)
            {
                if (Pinball == null)
                {
                    throw new Exception("You must call Init before passing data to the DirectOutput framework");
                }
                else
                {
                    throw new Exception("A exception occurred when passing in data (TableElementName: {0}, Value: {1})".Build(TableElementName, Value), E);
                }
            }

        }




        /// <summary>
        /// Finishes the DirectOutput framework.<br/>
        /// The is the last methods to be called on the framework.
        /// </summary>
        public static void Finish()
        {
            if (Pinball != null)
            {
                Pinball.Finish();
                Pinball = null;
            } else {
                Log.Error("DirectOutput.Finish: Pinball is null, unable to shut down cab!");
            }

        }

        /// <summary>
        /// Get the DirectOutput install folder location.
        /// 
        /// Traditionally, all of the DOF binaries (the "assemblies", in .NET speak)
        /// were installed in the main install folder, so identifying the install
        /// folder was simply a matter of getting the path to the active assembly.
        ///
        /// More recently, the DOF install convention has been revised to accommodate
        /// side-by-side 32-bit and 64-bit installs by placing the binaries in
        /// x86\ and x64\ subfolders under the main install folder.
        /// 
        /// This code is designed to work with both configurations, by checking
        /// for the presence of a Config folder.  We start in the folder containing
        /// the assembly.  If there's no Config folder there, we look to the parent
        /// folder, and if we find a Config folder there, we take it that we're in
        /// the new configuration that segregates the binaries by architecture.
        /// Otherwise, default to the original flat configuration.
        /// </summary>
        /// <returns>
        /// The path to use for all configuration files, or null if no path can be
        /// identified.
        /// </returns>
        public static String GetInstallFolder()
        {
            // Get the full path to the running assembly (i.e., the DOF DLL that
            // this code is part of).  This is the full name of the DLL file,
            // with absolute path.  If this is null, return null.
            var AssemblyLocation = Assembly.GetExecutingAssembly().Location;
            if (AssemblyLocation.IsNullOrEmpty())
                return null;

            // get the path to the assembly
            var AssemblyPath = new FileInfo(AssemblyLocation).Directory.FullName;

			// Check for the existence of a Config folder in this directory.  If
			// there's no such folder, AND there's a Config folder in the parent
			// directory, assume that we're running in the new install configuration
			// with x86 and x64 subfolders for the binaries.
			var AssemblyConfigPath = new DirectoryInfo(Path.Combine(AssemblyPath, "Config"));
            var AssemblyParentConfigPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(AssemblyPath, "..\\Config")));
			if (!AssemblyConfigPath.Exists && AssemblyParentConfigPath.Exists)
            {
                // new configuration with binary subfolders - the assembly is in
                // a subfolder within the install folder, so the install folder
                // is the parent of the assembly folder
                var parent = Path.GetDirectoryName(AssemblyPath);
				Log.Once("InstallFolderLoc", "Install folder lookup: assembly: {0}, install folder: {1} (PARENT of the assembly folder -> new shared x86/x64 install)".Build(AssemblyLocation, parent));
                return parent;
            }
            else
            {
                // old flat configuration - the assembly is in the install folder
                Log.Once("InstallFolderLoc", "Install folder lookup: assembly: {0}, install folder: {1} (ASSEMBLY folder -> original flat install configuration)".Build(AssemblyLocation, AssemblyPath));
                return AssemblyPath;
            }
        }

        /// <summary>
        /// Find the global config file for a given host application. 
        /// 
        /// Each application has a separate config file, "GlobalConfig_(X).xml",
        /// where (X) is the name of the host application as passed in the
        /// HostingApplicationName parameter.  The parameter is meant to be a
        /// human-readable name, so the filename is formed by deleting any
        /// periods and any characters not valid in Windows filenames, including
        /// any path separator characters.
        /// 
        /// The config file is normally stored in (DirectOutput)\Config, where
        /// (DirectOutput) is the main install folder.  This routine attempts to
        /// locate the main install folder via GetInstallFolder(), and if that's
        /// successful, it looks in the Config subfolder.
        /// 
        /// If the config file isn't present in the Config folder, we look for
        /// a .lnk file with the same root name as the config file (i.e., with
        /// the .xml suffix replaced by .lnk).  If found, this is taken to be
        /// a shortcut to the folder actually containing the config file.  Note
        /// that an .xml file in this folder takes priority - any .lnk file is
        /// ignored if there's also an .xml file.
        /// 
        /// Failing any of the above, we look for the .xml file in the current
        /// working directory, which is usually the directory containing the
        /// host application (but not always; the working directory can be
        /// explicitly manipulated by the host application, or can be set by
        /// the user when launching the application, if using a shortcut).
        /// </summary>
        /// <param name="HostingApplicationName">The display name of the host application that's invoking DOF</param>
        /// <returns>
        /// The name (with full path) of the config file, suitable for use in
        /// Pinball.Setup().  This always returns a valid filename, but the
        /// referenced file isn't guaranteed to exist.
        /// </returns>
        public static string GetGlobalConfigFileName(string HostingApplicationName)
        {
            // Convert the host application name to a filename suffix, by
            // deleting periods and invalid file and path characters.
            string HostAppFilename = HostingApplicationName.Replace(".", "");
            foreach (char C in Path.GetInvalidFileNameChars())
                HostAppFilename = HostAppFilename.Replace(C.ToString(), "");
            foreach (char C in Path.GetInvalidPathChars())
                HostAppFilename = HostAppFilename.Replace(C.ToString(), "");

            // form the name of the application-specific config file by appending
            // the host application name suffix
            var HostAppConfigRootName = "GlobalConfig_{0}".Build(HostAppFilename);
            var HostAppConfigFileName = HostAppConfigRootName + ".xml";

            // Get the config file location.  Start with the install folder.
            var installFolder = GetInstallFolder();
			FileInfo F;
            if (installFolder == null)
            {
                // we can't identify the install folder, so default to the working
                // directory by returning the root name without a path prefix
                return HostAppConfigFileName;
            }

            // look for an .xml file in the Config folder
            F = new FileInfo(Path.Combine(installFolder, "Config", HostAppConfigFileName));
            if (F.Exists)
            {
                Log.Once("GlobalConfigLoc", "Global config file lookup: found in Config folder: " + F.FullName);
                return F.FullName;
            }

            // no luck with the .xml file; look for a shortcut (.lnk) to the file
            FileInfo LnkFile = new FileInfo(Path.Combine(installFolder, "Config", HostAppConfigRootName + ".lnk"));
            if (LnkFile.Exists)
            {
                // there's a link; resolve it
                string ResolvedLinkPath = ResolveShortcut(LnkFile);
				Log.Once("GlobalConfigLoc", "Global config file lookup: found shortcut ({0}) -> {1}".Build(LnkFile.FullName, ResolvedLinkPath));
				if (Directory.Exists(ResolvedLinkPath))
                {
                    F = new FileInfo(Path.Combine(ResolvedLinkPath, HostAppConfigFileName));
                    if (F.Exists)
                    {
						Log.Once("GlobalConfigLoc", "Global config file lookup: found at shortcut location ({0})".Build(F.FullName));
                        return F.FullName;
                    }
                }
            }

            // try looking directly in the install folder
            F = new FileInfo(Path.Combine(installFolder, HostAppConfigFileName));
            if (F.Exists)
            {
                Log.Once("GlobalConfigLoc", "Global config file search: found in main install folder ({0})".Build(F.FullName));
                return F.FullName;
            }

            // If we still haven't found the file, give up; we still need a filename,
            // so set it to the default Config folder location if one exists, otherwise
            // the install folder.
            F = new FileInfo(Path.Combine(installFolder, "Config", HostAppConfigFileName));
            if (F.Directory.Exists)
            {
				Log.Once("GlobalConfigLoc", "Global config file search: file not found, but Config folder will be used for other file searches ({0})".Build(F.Directory.FullName));
			}
			else
            {
				Log.Once("GlobalConfigLoc", "Global config file search: Config folder ({0}) not found, using main install folder for other file searches ({1})".Build(F.Directory.FullName, installFolder));
                F = new FileInfo(Path.Combine(installFolder, HostAppConfigFileName));
            }

            // return what we found
            return F.FullName;
        }

		/// <summary>
		/// Initializes the DirectOutput framework.<br/>
		/// The method has to be called before any data is sent to DOF.<br/>
		/// It loads all necessary configuration data and starts all internal processes.
		/// </summary>
		/// <param name="HostingApplicationName">Name of the hosting application.</param>
		/// <param name="TableFilename">The table filename (specify a dummy filename of no table file is available).</param>
		/// <param name="RomName">Name of the rom (If there is no rom name of a table, specify your own unique name for the game).</param>
		/// <exception cref="System.Exception">Object has already been initialized. You must call Finish() before initializing again.</exception>
		public static void Init(string HostingApplicationName, string TableFilename, string RomName)
        {
            if (Pinball == null)
            {
                // Get the config file name
                var F = new FileInfo(GetGlobalConfigFileName(HostingApplicationName));

                // Create and initialize the main Pinball object
                Pinball = new DirectOutput.Pinball();
				Pinball.Setup((F.Exists ? F.FullName : ""), TableFilename, RomName);
                Pinball.Init();
            }
            else
            {
                throw new Exception("Object has already been initialized. You must call Finish() before initializing again.");
            }
        }


        /// <summary>
        /// Shows the frontend of the DirectOutput framework.
        /// </summary>
        /// <exception cref="System.Exception">Init has to be called before the frontend is opened.</exception>
        public static void ShowFrontend()
        {
            if (Pinball != null)
            {
                try
                {
                    DirectOutput.Frontend.MainMenu.Open(Pinball);
                }
                catch (Exception E)
                {
                    System.Windows.Forms.MessageBox.Show("Could not show DirectOutput frontend.\n The following exception occurred:\n{0}".Build(E.Message), "DirectOutput");
                }

            }
            else
            {
                throw new Exception("Init has to be called before the frontend is opened.");
            }
        }




        private static string ResolveShortcut(FileInfo ShortcutFile)
        {
            string TargetPath = "";
            try
            {
                Type WScriptShell = Type.GetTypeFromProgID("WScript.Shell");
                object Shell = Activator.CreateInstance(WScriptShell);
                object Shortcut = WScriptShell.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, Shell, new object[] { ShortcutFile.FullName });
                TargetPath = (string)Shortcut.GetType().InvokeMember("TargetPath", BindingFlags.GetProperty, null, Shortcut, null);
                Shortcut = null;
                Shell = null;
            }
            catch
            {

            }

            try
            {
                if (Directory.Exists(TargetPath))
                {
                    return TargetPath;
                }
                else if (File.Exists(TargetPath))
                {
                    return TargetPath;
                }
                else
                {
                    return "";
                }

            }
            catch
            {
                return "";
            }
        }

        #region Properties


        /// <summary>
        /// Gets a value indicating whether DOF is initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if DOF is initialized; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInitialized
        {
            get
            {
                return Pinball != null;
            }
        }


        private static Pinball _Pinball;

        /// <summary>
        /// Gets the \ref DirectOutput.Pinball object.
        /// </summary>
        /// <value>
        /// The \ref DirectOutput.Pinball object.
        /// </value>
        public static Pinball Pinball
        {
            get { return _Pinball; }
            private set { _Pinball = value; }
        }

        #endregion

    }
}
