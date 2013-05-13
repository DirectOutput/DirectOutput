Global Configuration {#globalconfig}
====================

\section globalconfig_introduction Introduction

The global configuration of the DirectOutput framework defines which files the system loads for the \ref cabinetconfig "Cabinet" and \ref tableconfig "Table" configuration. In addition the global conguration specifies which C#-script files containing extensions for the framework to load and compile. The global configuration information is valid for the whole framework no matter what cabinet or table config is loaded.

Depending on the interface used to communicate with the framework, different global configuration files are loaded. 

For the B2S.Server Plugin the configuration file "GlobalConfiguration_b2SServer.xml" is loaded. The framework is searching for this file in the following locations:

- A directory named -config- withing the directory of the DirectOutput.dll.
- A shortcut named -config- to a directory. 
- Table directory.
- Directory of the DirectOutput.dll.

\section globalconfig_settings Global configuration file sections

\subsection globalconfig_tablescriptsfilepattern GlobalScriptFilePatterns

GlobalScriptsFilePatterns defines the search patterns for global script files. The script files are loaded and compiled before the cabinet and table configs or any other scripts are loaded. 
This setting supports wildcards (* represents any number of characters, ? represents one character). In addition the following placeholders are supported:

* {DllDir} represents the directory of the DirectOutput.dll.
* {GlobalConfigDir} represents the global config directory.

A GlobalScriptsFilePatterns section could contain the following content:

~~~~~~~~~~~~~{.xml}
<GlobalScriptFilePatterns>
  <FilePattern>{GlobalConfigDir}\cabinet*.cs</FilePattern>
</GlobalScriptFilePatterns>
~~~~~~~~~~~~~


\subsection globalconfig_cabinetconfigfilepattern CabinetConfigFilePatterns

CabinetConfigFilePatterns defines the patterns for the \ref cabinetconfig "cabinet configuration file". If the patterns are matching more than one file, only the first matching file is loaded for the cabinet configuration.
This setting supports wildcards (* represents any number of characters, ? represents one character). In addition the following placeholders are supported:

* {DllDir} represents the directory of the DirectOutput.dll.
* {GlobalConfigDir} represents the default global config directory.

A typical CabinetConfigFilePattern sections might looks as follows:

~~~~~~~~~~~~~{.xml}
<CabinetConfigFilePatterns>
  <FilePattern>{GlobalConfigDir}\cabinet.xml</FilePattern>
</CabinetConfigFilePatterns>
~~~~~~~~~~~~~

\subsection globalconfig_cabinetscriptsfilepattern CabinetScriptsFilePattern

CabinetScriptsFilePattern defines the search patterns for cabinet script files. The script files are loaded an compiled before the cabinet config is loaded. This settings supports the same wildcards and placeholders as the _CabinetConfigFilePatterns_ setting.

A CabinetScriptsFilePattern section could contain the following content:

~~~~~~~~~~~~~{.xml}
<CabinetScriptFilePatterns>
  <FilePattern>{GlobalConfigDir}\cabinet*.cs</FilePattern>
</CabinetScriptFilePatterns>
~~~~~~~~~~~~~


\subsection globalconfig_tableconfigfilepatterns TableConfigFilePatterns

TableConfigFilePatterns define the search patterns which are used to lookup the table configuration file. If more than one file matches the search patterns, only the first matching file is loaded for the table configuration.
This setting supports wildcards (* represents any number of characters, ? represents one character). In addition the following placeholders are supported:

* {TableDir} represents the full path to the directory of the table.
* {TableDirName} represents the name of the table directory (last part of {TableDir}).
* {TableName} represents the name of the table without extensions.
* {DllDir} represents the directory of the DirectOutput.dll.
* {GlobalConfigDir} represents the default global config directory.

A typical TableConfigFilePattern section looks as follows:

~~~~~~~~~~~~~{.xml}
<TableScriptFilePatterns>
  <FilePattern>{GlobalConfigDir}\\{TableName}.xml</FilePattern>
</TableScripsFilePatterns>
~~~~~~~~~~~~~


\subsection globalconfig_tablescriptsfilepattern TableScriptFilePatterns

TableScriptsFilePatterns define the search patterns for table specific script files. The script files are loaded and compiled before the table config is loaded. This settings supports the same wildcards and placeholders as the _TableConfigFilePatterns_ setting.

A typical TableScriptsFilePattern section might looks as follows:

~~~~~~~~~~~~~{.xml}
<TableScriptFilePatterns>
  <FilePattern>{GlobalConfigDir}\\{TableName}*.cs</FilePattern>
</TableScriptFilePatterns>
~~~~~~~~~~~~~

\subsection globalconfig_ledcontrolinifiles LedControlIniFiles

LedControlIniFiles holds the paths to LedControl.ini files as well as the LedWiz number assiociated with the ledcontrol.ini files. LedControl.ini files can be used as a fallback solution if no cabinet and/or cabinet config is loaded. The LedControlIniFiles section can contain up to 16 LedControlIniFile sections.

A LedControlIniFile section defines a single LedControl.ini file and the associated LedWiz number.

* _FileName_ contains the full filename of the LedControl.ini file.
* _LedWizNumber_ contains the LedWiz number associated with the file defined in _FileName_.

Please read the chapter \ref ledcontrolfiles for more information on this topic.

A typical LedControlIniFiles section looks as follows:

~~~~~~~~~~~~~{.xml}
  <LedControlIniFiles>
    <LedControlIniFile>
      <Filename>c:\Ledcontrol\LedControl.ini</Filename>
      <LedWizNumber>1</LedWizNumber>
    </LedControlIniFile>
  </LedControlIniFiles>
~~~~~~~~~~~~~

\subsection globalconfig_updatetimerintervall UpdateTimerIntervall

UpdateTimerIntervall contains the minimal interval in milliseconds at which the update timer fires. Be carefull when changing this setting, since very low values might impact the performance of the framework.
The value of this setting defaults to 20 milliseconds.

A typical expample for this setting is:

~~~~~~~~~~~~~{.xml}
  <UpdateTimerIntervall>20</UpdateTimerIntervall>
~~~~~~~~~~~~~


\subsection globalconfig_logging Logging

EnableLogging does whats its name says. It enables log output to the file specified in the LogFilePattern.

The LogFilePattern can defines a filename for the log file and can include the following placeholders:

* {DllDir} represents the directory of the DirectOutput.dll.
* {GlobalConfigDir} represents the default global config directory.
* {TableDir} represents the full path to the directory of the table.
* {TableDirName} represents the name of the table directory (last part of {TableDir}).
* {TableName} represents the name of the table without extensions.
* {RomName} is the name of the game rom.
* {DateTime} is a timestamp containing the current date and type in the following form: yyyymmdd_hhmmss.
* {Date} is the current date in the following form: yyyymmdd.
* {Time} is the current time in the following form: hhmmss

Typical entries for the loggin configuration might look as follows:

~~~~~~~~~~~~~{.xml}
  <EnableLogging>true</EnableLogging>
  <LogFilePattern>.\{TableName}_{DateTime}.log</LogFilePattern>
~~~~~~~~~~~~~

\section globalconfig_example Example


~~~~~~~~~~~~~{.xml}
<?xml version="1.0" encoding="utf-8"?>
<!--Global configuration for the DirectOutput framework-->
<GlobalConfig>
  
  <LedControlIniFiles>
    <LedControlIniFile>
      <Filename>c:\Ledcontrol\LedControl.ini</Filename>
      <LedWizNumber>1</LedWizNumber>
    <LedControlIniFile>
      <Filename>c:\Ledcontrol\LedControl2.ini</Filename>
      <LedWizNumber>2</LedWizNumber>
    </LedControlIniFile>
  </LedControlIniFiles>
  
  <CabinetConfigFilePatterns>
    <FilePattern>{GlobalConfigDir}\cabinet.xml</FilePattern>
  </CabinetConfigFilePatterns>
  
  <CabinetScriptFilePatterns>
    <FilePattern>{GlobalConfigDir}\cabinet*.cs</FilePattern>
  </CabinetScriptFilePatterns>
  
  <TableScriptFilePatterns>
    <FilePattern>{GlobalConfigDir}\\{TableName}*.cs</FilePattern>
  </TableScriptFilePatterns>
  
  <TableConfigFilePatterns>
    <FilePattern>{GlobalConfigDir}\\{TableName}.xml</FilePattern>
  </TableConfigFilePatterns>
  
  <GlobalScriptFilePatterns>
    <FilePattern>{GlobalConfigDir}\cabinet*.cs</FilePattern>
  </GlobalScriptFilePatterns>
  
  <UpdateTimerIntervall>20</UpdateTimerIntervall>
  
  <EnableLogging>false</EnableLogging>
  <LogFilePattern>.\DirectOutput.log</LogFilePattern>

</GlobalConfig>

~~~~~~~~~~~~~

