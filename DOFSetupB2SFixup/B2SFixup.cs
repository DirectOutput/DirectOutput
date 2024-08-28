using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Xml;
using System.IO;
using Microsoft.Win32;
using IWshRuntimeLibrary;

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
            String bitness = session.CustomActionData["BITNESS"];
            session.Log("Installation folder is " + dofPath + ", bitness=" + bitness);

            // Look up B2S.Server and try to get the local file system location
            // where it's installed.  Do this by loading the type for "B2S.Server"
            // and traversing into Type.Module.Assembly.CodeBase.  This contains
            // the path to the DLL.  We're looking for a file:/// path.  If it's
            // not a local file system path, we can't create a shortcut there.
            session.Log("Finding B2S.Server DLL");
            var bt = Type.GetTypeFromProgID("B2S.Server");
            Module module = null;
            Assembly assembly = null;
            String codeBase = null;
            Match m = null;
            String b2sErr = "The B2S Backglass Server program doesn't appear to be installed "
                    + "on this computer, or it's not set up correctly for DOF (error details: "
                    + "{0}). You might want to reinstall Visual Pinball using the VPX "
                    + "installer and then re-run the DOF installation.";
            if (bt == null)
                errors.Add(String.Format(b2sErr, "the registry entry for the server is missing"));
            else if ((module = bt.Module) == null)
                errors.Add(String.Format(b2sErr, "the program module can't be loaded"));
            else if ((assembly = module.Assembly) == null)
                errors.Add(String.Format(b2sErr, "the program module can't be accessed"));
            else if ((codeBase = assembly.CodeBase) == null)
                errors.Add(String.Format(b2sErr, "the installation location is unknown"));
            else if (!(m = Regex.Match(codeBase, @"^file:///(.*)$")).Success)
                errors.Add(String.Format(b2sErr, "the installation location isn't on the local disk"));
            else
            {
                // success - extract the file system path
                String dllFile = m.Groups[1].Value.Replace('/', '\\');
                String path = Path.GetDirectoryName(dllFile);
                session.Log("B2S.Server DLL file is " + dllFile);

                // Create a shortcut to the DirectOutput install directory in the
                // B2S Plugins folder.  The shortcut must be named DirectOutput.lnk.
                session.Log("Creating B2S Plugins\\DirectOutput.lnk");
                String pluginsDir = Path.Combine(path, "Plugins");
                String shortcutFile = Path.Combine(pluginsDir, "DirectOutput.lnk");
                try
                {
                    // Create the Plugins folder if it doesn't already exist
                    Directory.CreateDirectory(pluginsDir);

                    // delete any old shortcut
                    System.IO.File.Delete(shortcutFile);

                    // Create the shortcut
                    var shell = new WshShell();
                    IWshShortcut shortcut = shell.CreateShortcut(shortcutFile);
                    shortcut.Description = "DirectOutput";
                    shortcut.TargetPath = dofPath;
                    shortcut.Save();
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
                        + "but recommended, since it remove unneeded error messages if you "
                        + "run a DirectOutput-enabled table that doesn't have a backglass "
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
