Global Configuration {#globalconfig}
====================

The global configuration of the DirectOutput framework defines which files the system loads for the \ref cabinetconfig "Cabinet" and Table configuration. In addition the global conguration specifies which C#-script files containing extension for the framework to load and compile. The global configuration information is valid for the whole framework no matter what cabinet or table config is loaded.

\section globalconfig_settings Global configuration file sections

\subsection globalconfig_cabinetconfigfilepattern CabinetConfigFilePattern

CabinetConfigFilePattern defines the pattern for the \ref cabinetconfig "cabinet configuration file". This setting supports wildcards (* represents any number of characters, ? represents one character). In addition the following placeholders are supported:

* {DllDir} represents the directory of the DirectOutput.dll.
* {GlobalConfigDir} represents the default global config directory (usually same as {DllDir}\config).

A typical CabinetConfigFilePattern sections might looks as follows:

~~~~~~~~~~~~~{.xml}
 <CabinetConfigFilePattern>{GlobalConfigDir}\cabinet.xml</CabinetConfigFilePattern>
~~~~~~~~~~~~~

\subsection globalconfig_cabinetscriptsfilepattern CabinetScriptsFilePattern

CabinetScriptsFilePattern defines the search patterns for cabinet script files. The script files are loaded an compiled before the cabinet config is loaded. This settings supports the same wildcards and placeholders as the _CabinetConfigFilePattern_ setting.
This section can contain any number of file patterns.

A CabinetScriptsFilePattern section could contain the following content:

~~~~~~~~~~~~~{.xml}
<CabinetScriptFilePatterns>
  <Pattern>{GlobalConfigDir}\cabinet*.cs</Pattern>
</CabinetScriptFilePatterns>
~~~~~~~~~~~~~



\subsection globalconfig_tableconfigfilepattern TableConfigFilePattern

TableConfigFilePattern defines the search pattern which is used to lookup the table configuration file. This setting supports wildcards (* represents any number of characters, ? represents one character). In addition the following placeholders are supported:

* {TableDir} represents the full path to the directory of the table.
* {TableDirName} represents the name of the table directory (last part of {TableDir}).
* {TableName} represents the name of the table without extensions.
* {DllDir} represents the directory of the DirectOutput.dll.
* {GlobalConfigDir} represents the default global config directory (usually same as {DllDir}\config).

A typical TableConfigFilePattern section looks as follows:

~~~~~~~~~~~~~{.xml}
  <TableConfigFilePattern>{GlobalConfigDir}\ {TableName}.xml</TableConfigFilePattern>
~~~~~~~~~~~~~


\subsection globalconfig_tablescriptsfilepattern TableScriptFilePatterns

TableScriptsFilePatterns defines the search patterns for table specific script files. The script files are loaded and compiled before the table config is loaded. This settings supports the same wildcards and placeholders as the _TableConfigFilePattern_ setting.

A typical TableScriptsFilePattern section might looks as follows:

~~~~~~~~~~~~~{.xml}
<TableScriptFilePatterns>
  <Pattern>{GlobalConfigDir}\ {TableName}*.cs</Pattern>
</TableScripsFilePatterns>
~~~~~~~~~~~~~

\subsection globalconfig_ledcontrolinifiles LedControlIniFiles

LedControlIniFiles holds the paths to LedControl.ini files as well as the LedWiz number assiociated with the ledcontrol.ini files. LedControl.ini files can be used as a fallback solution if no cabinet and/or cabinet config is loaded. The LedControlIniFiles section can contain any number of LedControlIniFile sections.

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
The value of this setting default to 20 milliseconds.

A typical expample for this setting is:

~~~~~~~~~~~~~{.xml}
  <UpdateTimerIntervall>20</UpdateTimerIntervall>
~~~~~~~~~~~~~

\section globalconfig_example Example


~~~~~~~~~~~~~{.xml}
<?xml version="1.0" encoding="utf-8"?>
<!--Global configuration for the DirectOutput framework-->
<Config>

  <CabinetConfigFilePattern>{GlobalConfigDir}\cabinet.xml</CabinetConfigFilePattern>

  <CabinetScriptFilePatterns>
    <Pattern>{GlobalConfigDir}\cabinet*.cs</Pattern>
  </CabinetScriptFilePatterns>

  <TableConfigFilePattern>{GlobalConfigDir}\ {TableName}.xml</TableConfigFilePattern>

  <TableScriptFilePatterns>
    <Pattern>{GlobalConfigDir}\ {TableName}*.cs</Pattern>
  </TableScriptFilePatterns>

  <LedControlIniFiles>

    <LedControlIniFile>
      <Filename>c:\Ledcontrol\LedControl.ini</Filename>
      <LedWizNumber>1</LedWizNumber>
    </LedControlIniFile>

    <LedControlIniFile>
      <Filename>c:\Ledcontrol\LedControl.ini</Filename>
      <LedWizNumber>2</LedWizNumber>
    </LedControlIniFile>

  </LedControlIniFiles>

  <UpdateTimerIntervall>20</UpdateTimerIntervall>

</Config>
~~~~~~~~~~~~~

