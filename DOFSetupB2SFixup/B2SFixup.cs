using System;
using System.Collections.Generic;
using WixToolset.Dtf.WindowsInstaller;
using System.Reflection;
using System.Xml;
using System.IO;
using Microsoft.Win32;

namespace DOFSetupB2SFixup
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult B2SFixup(Session session)
        {
            session.Log("Begin B2S -> DOF connection setup");

            // No errors yet
            List<String> errors = new List<String>();

            // Get the DirectOutput install location.  
            String dofPath = session.CustomActionData["INSTALLEDPATH"];
            String binDir = session.CustomActionData["BINDIR"];
            String bitness = session.CustomActionData["BITNESS"];
            session.Log("Installation path: " + dofPath + ", binary dir: " + binDir + ", bitness=" + bitness);

			// Try load the registered B2S.Server COM object, so that we can
			// ask it for its install location.
			session.Log("Finding B2S.Server DLL");
			var registeredType = Type.GetTypeFromProgID("B2S.Server");
            string dllFile = null;
            if (registeredType == null)
            {
                errors.Add("It appears that B2S Backglass Server is not installed "
                    + "on this system.  DOF requires B2S to be installed before DOF "
                    + "can run.  After this Setup process finishes, please separately "
                    + "download and install the " + bitness + "-bit version of B2S "
                    + "Backglass Server, then re-run this DOF installer.");

                session.Log("GetTypeFromProgID(\"B2S.Server\") failed - B2S is not installed");
            }
            else
            {
                // found the type; try creating an instance
                Object instance = null;
                try
                {
                    // create the instance
                    instance = System.Activator.CreateInstance(registeredType);
                }
                catch (Exception exc)
                {
					// If the error is "class not registered", when we perfectly well
					// found a registered class, it means that B2S isn't installed for
					// the bitness of the current installer.  This is a weird quirk of
					// COM that seems like a bug, but it's at least consistent; 
					// GetTypeFromProgID() returns a valid Type object, but it seems
					// to be for a generic System.__ComObject class that you can't
					// actually instantiate.  The only way I can find to tell if we
					// have such an object is to try to instantiate it and check for
					// this error.
					session.Log("Error loading B2S COM object: " + exc.Message);
					if (exc.Message.Contains("REGDB_E_CLASSNOTREG"))
                    {
                        // B2S is registered, but not for the current bitness
                        errors.Add("It looks like B2S Backglass Server is installed on this "
                            + "system, but NOT the " + bitness + "-bit version.  " + bitness
                            + "-bit DOF requires " + bitness + "-bit B2S.  After this Setup exits, "
                            + "please separately download and install " + bitness + "-bit B2S, "
                            + "then re-run this DOF install.");

                        session.Log("Note: this error usually indicates that B2S is installed, "
                            + "but NOT for the " + bitness + "-bit version required by this DOF "
                            + "install");
                    }
                    else
                    {
                        // some other error occurred
                        errors.Add("An error occurred loading the " + bitness
                            + "-bit B2S Backglass Server.  DOF won't work properly if B2S "
                            + "isn't installed.  After this Setup exits, you please download "
                            + "and install the " + bitness + "-bit version of B2S Backglass Server, "
                            + "then re-run this DOF install.");

                    }
                }

                // ask it for its DLL path
                if (instance != null)
                {
                    try
                    {
                        var dirObj = instance.GetType().InvokeMember(
                                "B2SServerDirectory", System.Reflection.BindingFlags.GetProperty, null, instance, null, null);

                        if (dirObj != null && dirObj is string s)
                        {
                            // got it - get the directory path (minus DLL filename)
                            dllFile = s;
                            session.Log("B2S DLL path successfully identified as \"" + dllFile + "\"");
                        }
                        else
                        {
                            errors.Add("B2S Backglass Server appears to be installed, but the "
                                + "setup program was unable to determine its install location.  "
                                + "The installed version of B2S might not be compatible with this "
                                + "setup program.  You might try re-installing B2S and re-running "
                                + "this DOF setup.");

                            session.Log("B2S object.B2SServerDirectory returned null or non-string result; can't identify B2S DLL path");
                        }
                    }
                    catch (Exception exc)
                    {
                        errors.Add("B2S Backglass Server appears to be installed, but the "
                            + "setup program was unable to determine its install location.  "
                            + "The installed version of B2S might not be compatible with this "
                            + "setup program.  You might try re-installing B2S and re-running "
                            + "this DOF setup.");

                        session.Log("Error getting property value for B2S object.B2SServerDirectory: " + exc.Message);
                    }
                }
            }

            // check if we found the path
            if (dllFile != null && dllFile.Length > 0)
            {
                // success - extract the file system path
                String path = Path.GetDirectoryName(dllFile);
                session.Log("B2S.Server DLL file is " + dllFile);

                // Create a shortcut to the DirectOutput install directory in the
                // B2S Plugins folder.  The shortcut must be named DirectOutput.lnk.
                session.Log("Creating B2S Plugins\\DirectOutput.lnk");
                String pluginsDir = Path.Combine(path, bitness == "64" ? "Plugins64" : "Plugins");
                String shortcutFile = Path.Combine(pluginsDir, "DirectOutput.lnk");
                try
                {
                    // Create the Plugins folder if it doesn't already exist
                    Directory.CreateDirectory(pluginsDir);

                    // delete any old shortcut
                    System.IO.File.Delete(shortcutFile);

                    // Create the shortcut using dynamic COM
                    var shell = Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Shell"));
                    dynamic shortcut = shell.GetType().InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, shell, new object[] { shortcutFile });
                    shortcut.GetType().InvokeMember("Description", BindingFlags.SetProperty, null, shortcut, new object[] { "DirectOutput" });
                    shortcut.GetType().InvokeMember("TargetPath", BindingFlags.SetProperty, null, shortcut, new object[] { Path.Combine(dofPath, binDir) });
                    shortcut.GetType().InvokeMember("Save", BindingFlags.InvokeMethod, null, shortcut, null);
                }
                catch (Exception exc)
                {
                    errors.Add("Setup wasn't able to create the B2S Plugins shortcut to the "
                        + "DirectOutput installer folder.  This shortcut is needed so that "
                        + "B2S can find the DirectOutput files when you start a game.  The "
                        + "shortcut file that Setup tried to create is \"" + shortcutFile
                        + "\".  You can create this shortcut manually if you wish; see the "
                        + "DirectOutput documentation for help.  (System error details: "
                        + exc.Message + ".)");
                }

                // Enable B2S plug-ins by setting the registry key DWORD value "Plugins"
                // under key HKEY_CURRENT_USER\Software\B2S
                session.Log("Enabling B2S plugins (setting HKCU\\Software\\B2S\\Plugins to 1)");
                try
                {
                    RegistryKey b2sKey = Registry.CurrentUser.CreateSubKey("Software\\B2S");
                    if (b2sKey != null)
                        b2sKey.SetValue("Plugins", 1, RegistryValueKind.DWord);
                }
                catch (Exception exc)
                {
                    errors.Add("Setup wasn't able to enable B2S plugins.  This is required "
                        + "before DirectOutput will work.  You can enable plugins manually "
                        + "if you wish; see the DirectOutput documentation for help. "
                        + "(Error details: error updating registry key "
                       + "HKEY_CURRENT_USER\\Software\\B2S\\Plugins; system error details: "
                       + exc.Message + ".)");
                }

                // Fix up B2STableSettings.xml to turn off "backglass not found" error
                // messages.  A table might want to load B2S.Server as the controller 
                // even if it doesn't have a backglass to display, purely for DOF access.
                session.Log("Turning off B2S \"missing backglass\" load error (in B2STableSettings.xml)");
                String settingsFile = Path.Combine(path, "B2STableSettings.xml");
                try
                {
                    // load the file
                    XmlDocument doc = new XmlDocument();
                    doc.Load(settingsFile);

                    // make sure we have a valid root node
                    XmlNode rootNode = doc.DocumentElement.SelectSingleNode("/B2STableSettings");
                    if (rootNode != null)
                    {
                        // find ShowStartUpError
                        XmlNode errNode = rootNode.SelectSingleNode("/B2STableSettings/ShowStartupError");

                        // if it doesn't exist, add it
                        if (errNode == null)
                            errNode = rootNode.AppendChild(doc.CreateElement("ShowStartupError"));

                        // fix it up
                        errNode.InnerText = "0";

                        // save the file
                        doc.Save(settingsFile);
                    }
                }
                catch (Exception exc)
                {
                    errors.Add("Setup was unable to update the B2S settings file to disable "
                        + "errors on startup when backglasses are missing.  This is optional, "
                        + "but recommended, since it suppresses superfluous error messages when "
                        + "you run a DirectOutput-enabled table that doesn't have a backglass "
                        + "installed.  You can update the setting manually if you wish; see "
                        + "the DirectOutput documentation for help.  (This happened while "
                        + "attempting to update \"" + settingsFile + "\"; system error "
                        + "details: " + exc.Message + ".)");
                }
            }

            // if there are any errors, show them
            if (errors.Count != 0)
            {
                String msg = "";
                foreach (var err in errors)
                {
                    if (msg != "")
                        msg += "\r\n\r\n";
                    msg += err;
                }
                session.Message(InstallMessage.Error, new Record { FormatString = msg });
            }

            return ActionResult.Success;
        }
    }
}
