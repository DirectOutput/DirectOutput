using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Xml;
using System.IO;
using Microsoft.Win32;

namespace DOFSetupPBXFixup
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult PBXFixup(Session session)
        {
            session.Log("Begin PinballX -> DOF connection setup");

            // No errors yet
            List<String> errors = new List<string>();

            // Get the DirectOutput install location.  
            String dofPath = session.CustomActionData["INSTALLEDPATH"];
            session.Log("Installation folder is " + dofPath);

            // Find PinballX's install location by searching for its uninstall key
            String pbxPath = null;
            try
            {
                // Search the uninstall keys.  The subkeys are the Setup package
                // GUIDs for the installed programs.  We don't want to count on
                // PinballX's package GUID being permanent across versions, as
                // that's a separate closed-source project that we can't make
                // that sort of assumption about, so we'll do this heuristically,
                // by searching for some values it's known to set.
                session.Log("Searching for PinballX install entry");
                String keyname = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                RegistryKey mainkey = Registry.LocalMachine.OpenSubKey(keyname);
                foreach (String subkeyname in mainkey.GetSubKeyNames())
                {
                    // open this key
                    RegistryKey subkey = mainkey.OpenSubKey(subkeyname);

                    // check DisplayIcon
                    object icon = subkey.GetValue("DisplayIcon");
                    if (icon != null && icon is string && Regex.IsMatch((string)icon, @".*\\PinballX\.exe$"))
                    {
                        // looks good - check the program name
                        session.Log("Found matching DisplayIcon under " + keyname + "\\" + subkeyname + " -> " + (string)icon);
                        object name = subkey.GetValue("DisplayName");
                        if (name != null && name is string && Regex.IsMatch((string)name, @"^PinballX(\s*$|\s+\d+\.\d+.*)"))
                        {
                            // got it - get the install location
                            session.Log(".. DisplayName is " + (string)name);
                            object dir = subkey.GetValue("InstallLocation");
                            if (dir != null && dir is string)
                            {
                                session.Log(".. InstallLocation " + (string)dir);
                                pbxPath = (string)dir;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                errors.Add("Setup was unable to search for PinballX's install "
                    + "location in the system registry.  (System error details: "
                    + exc.Message + ")");
            }

            // if we found it, do the rest of the setup
            if (pbxPath != null)
            {
                session.Log("Install entry found, path is " + pbxPath);
                String pluginsPath = Path.Combine(pbxPath, "Plugins");
                try
                {
                    // make sure the Plugins folder exists
                    Directory.CreateDirectory(pluginsPath);

                    String dllName = "DirectOutput PinballX Plugin.dll";
                    String src = Path.Combine(dofPath, dllName);
                    String dst = Path.Combine(pluginsPath, dllName);
                    try
                    {
                        // copy the plugin DLL
                        session.Log("Copying DOF PBX plugin: " + src + " -> " + dst);
                        File.Copy(src, dst, true);

                        String iniFile = Path.Combine(pbxPath, @"Config\PinballX.ini");
                        session.Log("Checking for PBX INI file (" + iniFile + ")");
                        try
                        {
                            // load the PinballX INI file
                            String[] ini = { };
                            if (File.Exists(iniFile))
                            {
                                session.Log("INI file exists; reading it");
                                ini = File.ReadAllLines(iniFile, Encoding.Unicode);
                            }

                            // scan for an existing mention of our plugin
                            session.Log("Scanning INI file for DOF plugin section");
                            int pluginSectNo = -1;
                            int maxPluginSectNo = 0;
                            int pluginSectLineNo = -1;
                            int ourPluginSectLineNo = -1;
                            for (int i = 0; i < ini.Length; ++i)
                            {
                                // get this line
                                String l = ini[i];

                                // Look for key strings:
                                //
                                // [Plugin_N]    - start of a plugin section
                                // Name=XXX      - plugin DLL name
                                //
                                Match m;
                                if ((m = Regex.Match(l, @"(?i)\s*\[Plugin_(\d+)\]\s*")).Success)
                                {
                                    // note the plugin section we're currently in
                                    pluginSectLineNo = i;
                                    pluginSectNo = int.Parse(m.Groups[1].Value);

                                    // in case we have to create a new entry, note the
                                    // high water mark for plugin sections so far
                                    if (pluginSectNo > maxPluginSectNo)
                                        maxPluginSectNo = pluginSectNo;
                                }
                                else if ((m = Regex.Match(l, @"(?i)\s*Name\s*=\s*DirectOutput PinballX Plugin\.dll\s*")).Success)
                                {
                                    // this is our section!
                                    ourPluginSectLineNo = pluginSectLineNo;
                                    session.Log("DOF plugin section found (line " + (ourPluginSectLineNo + 1) + ")");

                                    // no need to scan any further
                                    break;
                                }
                            }

                            // Rebuild the array, inserting our text as needed
                            session.Log("Updating INI file contents");
                            List<String> newIni = new List<string>();
                            bool inOurSect = false;
                            bool foundEnabled = false;
                            for (int i = 0; i < ini.Length; ++i)
                            {
                                // check for entry and exit of our section
                                String l = ini[i];
                                if (i == ourPluginSectLineNo)
                                {
                                    // entering our section
                                    session.Log("Entering our section on rewrite");
                                    inOurSect = true;
                                }
                                else if (inOurSect && Regex.IsMatch(l, @"(?i)\s*\[Plugin_(\d+)\]\s*"))
                                {
                                    // entering a new section, so we're leaving our section
                                    inOurSect = false;

                                    // if we didn't find an Enabled line in our section, add one
                                    session.Log("Exiting our section; foundEnabled=" + foundEnabled);
                                    if (!foundEnabled)
                                        newIni.Add("Enabled=True");
                                }

                                // if we're in our section, note if this is the Enabled line
                                if (inOurSect && Regex.IsMatch(l, @"(?i)\s*Enabled\s*=.*"))
                                {
                                    // it's our enabled line - change it to Enabled=True
                                    session.Log(". found Enabled line (" + (i+1) + ")");
                                    l = "Enabled=True";
                                    foundEnabled = true;
                                }

                                // copy the (possibly modified) line to the output
                                newIni.Add(l);
                            }

                            // If there wasn't a pre-existing section for our plugin at
                            // all, add one
                            if (ourPluginSectLineNo == -1)
                            {
                                int n = maxPluginSectNo + 1;
                                session.Log("DOF plugin section not found in INI file; adding it as #" + n);
                                newIni.Add("[Plugin_" + n + "]");
                                newIni.Add("Name=DirectOutput PinballX Plugin.dll");
                                newIni.Add("Enabled=True");
                            }

                            // rewrite the file
                            try
                            {
                                session.Log("Rewriting PBX INI file -> " + iniFile);
                                File.WriteAllLines(iniFile, newIni, Encoding.Unicode);
                            }
                            catch (Exception exc)
                            {
                                errors.Add("Setup was unable to update the PinballX settings file ("
                                    + iniFile + "; system error details: " + exc.Message + ")");
                            }
                        }
                        catch (Exception exc)
                        {
                            errors.Add("Setup was unable to read the PinballX settings file ("
                                + iniFile + "; system error details: " + exc.Message + ")");
                        }
                    }
                    catch (Exception exc)
                    {
                        errors.Add("Setup was unable to copy the DirectOutput plugin "
                            + "for PinballX to PinballX's install folder.  (File copy "
                            + "attempted: " + src + " -> " + dst + "; system error details: "
                            + exc.Message + ")");
                    }
                }
                catch (Exception exc)
                {
                    errors.Add("Setup was unable to create the PinballX Plugins folder ("
                        + pluginsPath + ", system error " + exc.Message + ").");
                }
            }
            else
            {
                // note that it wasn't found
                session.Log("No PinballX installer entry found in registry; skipping PinballX setup");
            }

            // if there are any errors, show them
            if (errors.Count != 0)
            {
				String msg = "";
				String padding = "";
				foreach (var err in errors)
				{
					msg += padding + err;
					padding = "\r\n\r\n";
				}

                session.Message(InstallMessage.Error, new Record { FormatString = msg });
            }

            // done
            return ActionResult.Success;
        }
    }
}
