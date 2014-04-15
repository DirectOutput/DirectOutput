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

* {DllDir} represents the directory of the DirectOutput.dll.
* {GlobalConfigDir} represents the default global config directory.
* {TableDir} represents the full path to the directory of the table.
* {TableDirName} represents the name of the table directory (last part of {TableDir}).
* {TableName} represents the name of the table without extensions.
* {RomName} is the name of the game rom.
* {DateTime} is a timestamp containing the current date and type in the following form: yyyymmdd_hhmmss.
* {Date} is the current date in the following form: yyyymmdd.
* {Time} is the current time in the following form: hhmmss

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
</GlobalConfig>
~~~~~~~~~~~~~