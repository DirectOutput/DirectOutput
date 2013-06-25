Installation and Configuration
====================

\section installation_requirements Requirements 

The Directoutput framework relies on the B2S-Server which has been developed by Herweh/Stefan. The B2S-Server implements a plugin interface which can load and execute plugins. This framework has been developed as such a plugin.

Therefore you must first install and configure Herwehs B2S-Server before you can install the DirectOutput framework. You can download the B2S-Server from <a href="www.vpuniverse.com">vpuniverse.com</a> or <a href="www.vpforums.org">VPForums</a>.

\section installation_installation Download 

The DirectOutput framework can be downloaded from <a target="_blank" href="www.vpuniverse.com">vpuniverse.com</a>, but there are other sites might host the binaries as well. 

You are also very welcome to fork/download and enhance the source code from <a target="_blank" href="https://github.com/DirectOutput/DirectOutput">GitHub</a>.


\section installation_installation Installation

Unzip the contents of the zip-file containing the framework to the following subpath of the B2S-Server: {B2S-Server directory}\\plugins\\DirectOutput

The B2S-Server will automatically detect the framework on startup and integrate it. Please check <a target="_blank" href="http://www.vpforums.org/index.php?showforum=86">VPForums</a> for more information on the B2S-Server.

Typically the directory and file structure of your installtion should look like this (depending on the framework version, more files might exist in the installation directory):

\image html Installation_DirectoryStructure.png DirectOutput Installation

Alternatively the DirectOutput framework can also be put into any other directory on your system and a windows shortcut pointing to this directory can be added to the {B2S-Server directory}\\plugins directory. The B2S.Server will follow this shortcut to your plugin directory.

\section installation_b2sserverconfig B2S.Server Configuration

To enable the plugin support in the B2S.Server (DirectOutput is a B2S.Server plugin) you need to add the _ArePluginsOn_ option to your B2STableSettings.xml file (located in the same directory as the B2S.Server):

~~~~~~~~~~~~~~~{.xml}
<B2STableSettings>
  ....
  <ArePluginsOn>1</ArePluginsOn>
  ...
</B2STableSettings>
~~~~~~~~~~~~~~~

\section installation_visualpinballcorevbs Visual Pinball core.vbs Adjustment

If you have used the <a target="_blank" href="http://www.hyperspin-fe.com/forum/showthread.php?10980-Tutorial-How-to-config-Ledwiz-PacDrive">VBScript solution</a> to control your LedWiz before you will have to _REMOVE_ the following line from your core.vbs:

~~~~~~~~~~~~~~~{.vbs}
ExecuteGlobal GetTextFile("ledcontrol.vbs")
~~~~~~~~~~~~~~~

If you dont remove the mentioned statement you'll run into trouble since the framework and the ledcontrol.vbs solutions will run simultaneously!

\section installation_visualpinballtableconfig Visual Pinball Table Configuration

Tables using the DirectOutput framework resp. the B2S-Server have to instanciate the B2S.Server instead of the Pinmame.Controller.

Replace the following line in the table scripts of the tables you want to use the B2S.Server and the DirectOutput framework

~~~~~~~~~~~~~~~{.vbs}
Set Controller = CreateObject("VPinMAME.Controller")     
~~~~~~~~~~~~~~~

with

~~~~~~~~~~~~~~~{.vbs}
Set Controller = CreateObject("B2S.Server") 
~~~~~~~~~~~~~~~

Please read the page on \ref tableconfig for more information on this topic.



\section installation_configuration Configuration 

\subsection installation_autoconfiguration Using Auto configuration

If correctly installed the DirectOutput framework can configure itself automatically. This means that you dont need to create any of the config files mentioned in the sections below to get started.

Auto configuration will automatically detect the LedWiz units connected to your system and try to lookup the ledcontrol.ini file with table configurations in the following locations:

- Table directory
- Config directory of DirectOutput
- Directory containing the DirectOutput.dll

Up to 16 LedWiz units are supported, which can be controlled by numbered ledcontrol.ini files. For LedWiz nr. 1 the file ledcontrol.ini will be loaded, for LedWiz numbers 2-16 files ledcontrol{LedWizNumber}.ini (e.g. ledcontrol2.ini) will be loaded.

The create your own lecontrol.ini files it is best to use the <a target="_blank" href="http://vpuniverse.com/ledwiz/login.php">LedWiz Config Tool</a>.

At the moment only LedWiz units are automatically detected. If you are using other output controllers you will have to create a cabinet configuration file, specifying the output controllers and some matching LedWizEquivalent toys, to allow the use of ledcontrol files.

\subsection installation_configfiles Using configuration files

The auto configuration function of the framework does only support some basic functionality of the framework. If you want to unleash the full power of DirectOutput, you can venture into the more detailed configuration options.

\subsection installation_ledcontrolini Using LedControl.ini files

The DirectOutput framework can use one or several classical LedControl.ini files to configure tables. If no other config files are found, the framework will try to find the ledcontrol.ini files automatically.

Please check the page on \ref ledcontrolfiles for details.

\subsubsection installation_globalconfig Global Configuration

The global configuration specifies some global settings for the framework, like the places where cabinet and table configurations are looked up. 

If the framework is called through the B2S.Server Plugin it will search for a file named _GlobalConfiguration_b2SServer.xml_ in the following places:
- A directory named _config_ withing the directory of the DirectOutput.dll.
- A shortcut named _config_ to a directory. 
- Table directory.
- Directory of the DirectOutput.dll.

If no global configuration file can be found, the framework tries to configure itself by detecting LedWiz units and searching ledcontrol.ini file in the table directory, the config directory of the DirectOutput framework or the dll directory of the framework (see section on auto configuration above). In this mode, the framework behaves like the vbscript solution.

To create your own global configuration, you can either use the built in global configuration editor or write a XML-file with the necessary information yourself.

Please read the page on \ref global config for a detailed explanation of the settings.

\subsubsection installation_cabinetconfig Cabinet Configuration

The cabinet configuration specifies the output controllers (e.g. Ledwiz) and toys (e.g. contactors and RGB leds) in your cabinet. 

Typically the cabinet configuration file will be named cabinet.xml and reside in the config directory of the DirectOutput framework. Be sure to configure a file pattern in the global configuration pointing on your own cabinet configuration file.

Please read the page on \ref cabinetconfig for more information.

\subsubsection installation_tableconfig Table Configuration

The table configuration specifies the elements on a pinball table (e.g. solenoids or lamps) and the effects assigned to the table elements. 
  
Please read the page on \ref tableconfig for details.

