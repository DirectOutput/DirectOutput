using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Win32;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace DOFSetupPBXFixup
{
    [ComVisible(true)]
    [Guid("F4F2DA2E-9F7E-487A-8F29-0C7D6AA18D6A")]
    public class CustomActions
    {
        public static string Run(string dofPath, string binDir, string bitness)
        {
            var errors = new List<string>();
            var log = new StringBuilder();

            void Log(string message) => log.AppendLine($"[PBXFixup] {message}");

            Log("Begin PinballX -> DOF connection setup");

            if (string.IsNullOrWhiteSpace(dofPath))
                errors.Add("Installation path not supplied.");
            if (string.IsNullOrWhiteSpace(binDir))
                errors.Add("Binary directory not supplied.");
            if (string.IsNullOrWhiteSpace(bitness))
                errors.Add("Bitness not supplied.");

            if (errors.Count > 0)
                return Finish(log, dofPath, errors);

            Log("Installation path: " + dofPath + ", binary dir: " + binDir + ", bitness=" + bitness);

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
                Log("Searching for PinballX install entry");
                String keyname = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                RegistryKey mainkey = Registry.LocalMachine.OpenSubKey(keyname);
                if (mainkey != null)
                {
                    foreach (String subkeyname in mainkey.GetSubKeyNames())
                    {
                        // open this key
                        RegistryKey subkey = mainkey.OpenSubKey(subkeyname);

                        // check DisplayIcon
                        object icon = subkey?.GetValue("DisplayIcon");
                        if (icon is string iconPath && Regex.IsMatch(iconPath, @".*\\PinballX\.exe$"))
                        {
                            Log("Found matching DisplayIcon under " + keyname + "\\" + subkeyname + " -> " + iconPath);
                            object name = subkey.GetValue("DisplayName");
                            if (name is string displayName && Regex.IsMatch(displayName, @"^PinballX(\s*$|\s+\d+\.\d+.*)"))
                            {
                                Log(".. DisplayName is " + displayName);
                                object dir = subkey.GetValue("InstallLocation");
                                if (dir is string installLocation)
                                {
                                    try
                                    {
                                        using (var fs = File.OpenRead(Path.Combine(installLocation, "pinballx.exe")))
                                        {
                                            using (var peReader = new PEReader(fs))
                                            {
                                                var headers = peReader.PEHeaders;
                                                if (headers.IsExe)
                                                {
                                                    bool is64 = headers.PEHeader.Magic == PEMagic.PE32Plus;
                                                    if (is64 == (bitness == "64"))
                                                    {
                                                        // success - log it
                                                        Log(".. InstallLocation " + installLocation);
                                                        pbxPath = installLocation;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    // ignore the error and keep looking
                                        continue;
                                    }
                                }
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
                Log("Install entry found, path is " + pbxPath);
                String pluginsPath = Path.Combine(pbxPath, "Plugins");
                try
                {
                    // make sure the Plugins folder exists
                    Directory.CreateDirectory(pluginsPath);

                    String dllName = "DirectOutput PinballX Plugin.dll";
                    String src = Path.Combine(dofPath, binDir, dllName);
                    String dst = Path.Combine(pluginsPath, dllName);
                    try
                    {
                        // copy the plugin DLL
                        Log("Copying DOF PBX plugin: " + src + " -> " + dst);
                        File.Copy(src, dst, true);

                        String iniFile = Path.Combine(pbxPath, @"Config\PinballX.ini");
                        Log("Checking for PBX INI file (" + iniFile + ")");
                        try
                        {
                            // load the PinballX INI file
                            String[] ini = { };
                            if (File.Exists(iniFile))
                            {
                                Log("INI file exists; reading it");
                                ini = File.ReadAllLines(iniFile, Encoding.Unicode);
                            }

                            // scan for an existing mention of our plugin
                            Log("Scanning INI file for DOF plugin section");
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
                                    Log("DOF plugin section found (line " + (ourPluginSectLineNo + 1) + ")");

                                    // no need to scan any further
                                    break;
                                }
                            }

                            // Rebuild the array, inserting our text as needed
                            Log("Updating INI file contents");
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
                                    Log("Entering our section on rewrite");
                                    inOurSect = true;
                                }
                                else if (inOurSect && Regex.IsMatch(l, @"(?i)\s*\[Plugin_(\d+)\]\s*"))
                                {
                                    // entering a new section, so we're leaving our section
                                    inOurSect = false;

                                    // if we didn't find an Enabled line in our section, add one
                                    Log("Exiting our section; foundEnabled=" + foundEnabled);
                                    if (!foundEnabled)
                                        newIni.Add("Enabled=True");
                                }

                                // if we're in our section, note if this is the Enabled line
                                if (inOurSect && Regex.IsMatch(l, @"(?i)\s*Enabled\s*=.*"))
                                {
                                    // it's our enabled line - change it to Enabled=True
                                    Log(". found Enabled line (" + (i+1) + ")");
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
                                Log("DOF plugin section not found in INI file; adding it as #" + n);
                                newIni.Add("[Plugin_" + n + "]");
                                newIni.Add("Name=DirectOutput PinballX Plugin.dll");
                                newIni.Add("Enabled=True");
                            }

                            // rewrite the file
                            try
                            {
                                Log("Rewriting PBX INI file -> " + iniFile);
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
                Log("No PinballX installer entry found in registry; skipping PinballX setup");
            }

            return Finish(log, dofPath, errors);
        }

        private static string Finish(StringBuilder log, string dofPath, List<string> errors)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(dofPath))
                {
                    var logPath = Path.Combine(dofPath, "PinballXFixup.log");
                    File.WriteAllText(logPath, log.ToString(), Encoding.UTF8);
                }
            }
            catch
            {
                // ignore logging failures
            }

            return errors.Count == 0
                ? null
                : string.Join("\r\n\r\n", errors);
        }
    }
}
