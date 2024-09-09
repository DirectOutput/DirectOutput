Global Configuration {#globalconfig2}
====================

\section globalconfig_introduction Introduction

\note Global config settings have changed for release 2 of the DirectOutput framework. If you are upgrading to the new version, be sure to create a new global config file using the global config editor.

The global configuration of the DirectOutput framework defines which files the system loads for the Cabinet and Table configuration as well as the options for logging.

Depending on the interface used to communicate with the framework, different global configuration files are loaded.

For the B2S.Server Plugin the configuration file "GlobalConfig_B2SServer.xml" is loaded. The framework is searching for this file in the following locations:

- A directory named _config_ withing the directory of the DirectOutput.dll.
- A shortcut to a directory withing the directory of the DirectOutput.dll named _config_.
- Table directory.
- Directory of the DirectOutput.dll.


\section globalconfig_editor Global config editor

In the directory where you have installed the DirectOutput framework, you can also find the global configuration editor (GlobalConfigEditor.exe).

This tool allows you to define the settings for the global configuration.

In the file menu you find the usual commands for loading and saving.

The editor has several tabs for the different aspects of the gobal config settings.


\subsection globalconfig_inifiles Ini Files tab

\image html GlobalConfigEditor_IniFiles.png "Ini Files tab"

The ini files table contains the settings for ini files.

Usually DOF will try to find the ini files with the table configurations in one of the following directories:

- Directory of the table file
- Global config directory
- Directory where the DirectOutput.dll is located

If you prefer to store your ini files in another directory, you'll have to enter the path to this directory in Ini Files Path textbox.

\subsection globalconfig_cabinetconfig Cabinet Config tab

\image html GlobalConfigEditor_Cabinet.png "Cabinet Config tab"

The cabinet config tab allows you to specify the cabinet config file to be used.

If you use no cabinet config file (not needed if you just use one or several LedWiz, PacDrive or Pacled64 devices) keep this setting empty.
If you have defined a cabinet config file, specify the name and path of your cabinet config file.

This setting supports wildcards (* represents any number of characters, ? represents one character). In addition the following placeholders are supported:

* {DllDir} represents the directory of the DirectOutput.dll.
* {GlobalConfigDir} represents the default global config directory.

\subsection globalconfig_logging Logging tab

\image html GlobalConfigEditor_Logging.png "Logging tab"

DOF can write a log file in which information about the initialization and operation of the framework is recorded.

This tab allows you to turn on and off the logging, specify the name and path for your log file and whether you want to discard the logfile for every session (good to keep the log file small).

The following placeholders can be used in the path and filename for the logfile:

* {DllDir} represents the main DirectOutput install folder.  **Deprecated:** use {InstallDir} instead.  This dates back to the original "flat" install configuration, where the DLL files were installed directly in the main DirectOutput folder.  Its name suggests that it's the DLL folder location, but it was actually used in most cases as the starting point for config file paths, so in the new 32/64-bit setup it points to the main folder rather than the actual DLL folder.  This makes its name misleading, so it should no longer be used; the new variables {InstallDir} and {BinDir} should be used instead, since they let you distinguish between the two possibly different folders in the newer 32/64-bit install configuration.
* {InstallDir} is the main DirectOutput install folder.  In the newer 32/64-bit install setup, the DLLs are located in x86 and x64 subfolders of this main folder.
* {BinDir} is the folder containing the currently executing DOF DLL (the program "**bin**ary" file).  In the 32/64-bit install setup, this is the x86 or x64 subfolder of the main install folder.
* {GlobalConfigDir} represents the default global config directory.
* {TableDir} represents the full path to the directory of the table.
* {TableDirName} represents the name of the table directory (last part of {TableDir}).
* {TableName} represents the name of the table without extensions.
* {RomName} is the name of the game rom.
* {DateTime} is a timestamp containing the current date and type in the following form: yyyymmdd_hhmmss.
* {Date} is the current date in the following form: yyyymmdd.
* {Time} is the current time in the following form: hhmmss


\subsection globalconfig_misc Misc tab

\image html GlobalConfigEditor_misc.png "Misc tab"

In the Misc tab you can change the default interval between commands which are sent to LedWiz units. For most installations 1ms should be just fine, but depending on the mainboard, usb hardware on the board, usb drivers and other factors the LedWiz does sometime tend to loose or misunderstand commands received if the are sent in to short intervals.
This settings allows to increase the default minimum interval between commands from 1ms to a higher value. Higher values will make problems less likely, but decreases the number of possible updates of the ledwiz outputs in a given time frame.
It is recommended to use the default interval of 1 ms and only to increase this interval if problems occur (Toys which are sometimes not reacting, random knocks of replay knocker or solenoids).


\section Log file instrumentation

The `<Instrumentation>` element lets you specify a list of "instrumentation keys", separated by
commas.  These are short strings that the DOF program code uses internally to identify certain
low-level debugging messages that aren't needed during normal use, so they're normally excluded
from the log, to avoid overly bloating the log file with unnecessary technical details that
are normally only of interest to the developers while working on the code.  However, it's
sometimes useful to see some of those details when troubleshooting a problem, so the
`<Instrumentation>` element lets you selectively enable specific ones.  You should only
need to add this to your config file if a developer advises you to do so, in which case
they should also tell you exactly what strings to include here.

You can enable **all** of the low-level debug messages by setting this value
to a single asterisk, `*`.

Since this setting is mostly for the developers' use, we won't provide
a list of the available keys here (and such a list would quickly go
out of date if we did provide one, since new keys are likely to be
added fairly often).  If you want the full list, search the source
code for the string `Log.Instrumentation`.


\section globalconfig_fileformat File format

Global config files are save in xml format. A typical global config file might looks like this:

~~~~~~~~~~~~~{.xml}
<?xml version="1.0" encoding="utf-8"?>
<!--Global configuration for the DirectOutput framework.-->
<!--Saved by DirectOutput Version 0.6.5218.23775: 2014-04-15 14-32-46-->
<GlobalConfig>
  <LedControlMinimumEffectDurationMs>60</LedControlMinimumEffectDurationMs>
  <LedControlMinimumRGBEffectDurationMs>120</LedControlMinimumRGBEffectDurationMs>
  <IniFilesPath>{DllDir}\config</IniFilesPath>
  <CabinetConfigFilePattern>{DllDir}\config\cabinet.xml</CabinetConfigFilePattern>
  <EnableLogging>true</EnableLogging>
  <ClearLogOnSessionStart>false</ClearLogOnSessionStart>
  <LogFilePattern>{DllDir}\DirectOutput.log</LogFilePattern>
  <Instrumentation>DudesCab, LedWizDiscovery</Instrumentation>
</GlobalConfig>
~~~~~~~~~~~~~