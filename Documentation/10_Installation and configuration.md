Installation and Configuration {#installation}
==============================

\section installation_requirements Requirements 

The Directoutput framework relies on the B2S-Server which has been developed by Herweh/Stefan. The B2S-Server implements a plugin interface which can load and execute plugins. This framework has been developed as such a plugin.

Therefore you must first install and configure Herwehs B2S-Server before you can install the DirectOutput framework. You can download the B2S-Server from <a href="http://vpuniverse.com/forums/files/file/2042-b2s-backglass-server/">vpuniverse.com</a> or <a href="http://www.vpforums.org/index.php?app=downloads&showfile=7426">VPForums</a>.

\section installation_download Download 

\note Sorry, public downloads are not yet available. As soon as the framework is released to the public, everyone will be allowed to download the framework.

The DirectOutput framework can be downloaded from <a target="_blank" href="http://www.vpuniverse.com">vpuniverse.com</a>, but other sites might host the binaries as well. 

You are also very welcome to fork/download and enhance the source code from <a target="_blank" href="https://github.com/DirectOutput/DirectOutput">GitHub</a>.

\section installation_installation Installation

Hosting applications which can use the DirectOutput framework, implement plugin interfaces which will automatically detect, load and use the library if it is correctly installed. 

The DirectOutput framework can be installed in two ways:

* Directly within the directory structure of the hosting application (e.g. B2S.Server). This will require only minimal effort when installing, but it might lead to duplicate installations if DOF is used in more than one hosting application.
* In any other directory of your system. This will require a little more work during installation, because shortcuts from the hosting applications plugin directory to the directoy containg DOF have to be added. This installation pattern will all all applications which support DOF to use the same installation, so no duplicate installtions are required. This is the recommanded installtion pattern.

\subsection installation_installation_hostappdir Installation within hosting application directory structure 

To install DOF withing the directory structure of the application using the framework (e.g. B2S.Server), please do the following:

* In the installation directory of the hosting application, create a directoy called _plugins_.
* Inside the newly created _plugins_ directory, create a subdirectory called _DirectOutput_ (any other name will be ok as well).
* Unzip the contents of the zip-file containg the framework to the newly created _DirectOutput_ directory.
* If using platforms like Vista or Win7, the installed files might be blocked by UAC. To unblock the files please read: \ref installation_unblockauc 

If this has been done correctly, the hosting application will automatically detect, load and use the framework.

__Example:__
A typical installation of the framework for the B2S.Server (provided that B2S.Server is installed in the VP tables directory) will have to following directory and file structure:

\image html Installation_DirectoryStructure.png Installation in hosting application directory structure
Note: Depending on the DirectOutput framework version, more files might exist in the installation.

\subsection installation_installation_owndir Installation in own directory (Recommended)

To avoid duplicate installations of the DirectOutput framework, the applications using the framework as a plugin can follow windows shortcuts to the directory containging DOF.

To install DirectOutput like this, please do the following:
* Create a directory for you DirectOutput installtion (e.g. C:\\DirectOutput). The name and the path to this directory can be anything you like, but it might be a clever idea to have DirectOutput in the directory name.
* Unzip the contents of the zip-file containg the framework to the newly created directory.
* If using platforms like Vista or Win7, the installed files might be blocked by UAC. To unblock the files please read: \ref installation_unblockauc 
 
To allow the hosting application (e.g. B2S.Server) to detect the DirectOutput framework, do the following:
* Go to the directory containing the files of the hosting application (For B2S.Server this is typically the VP tables folder).
* In this directory create a subdirectory named _plugins_.
* In the newly created _plugins_ directory, create a windows shortcut pointing to the directory containg the DirectOutput framework. This can be done by right clicking in the _plugins_ directory and selecting New -> Shortcut in the context menu.

__Example:__
A typical installation of the framework in its own directory might resemble the following file and directory structure:
\image html Installation_OwnDirectory1.png DirectOutput installtion in own directory
Note: Depending on the DirectOutput framework version, more files might exist in the installation.

If the framework is used with a B2S.Server installtion (which normaly reside in the VP table folder), you should have the following directory structure and shortcut (pointing to the directory containg the DirectOutput framework):
\image html Installation_OwnDirectory2.png Shortcut pinting to the DirectOutput installation directory


\section installation_unblockauc Unblock the DLLs

On platforms like Win7 or Vista you might need to unblock the files of the DirectOutput framework, before it can be recognized by hosting applications (e.g. B2S.Server).

Please exceute the following procedure for all dll and exe files of the DirectOutput installtion:
* Right click the DLL file and select _Properties_ in the context menu.
* Select the _General_ tab in the properties window.
* If you find the text _"This file came from another computer and might be blocked to help protect this computer" or somthing similar on this tab, click the _Unblock_ button. This should fix the issue.

If the text about the file coming from another computer does not exist, everthing should be fine.

\section installation_b2sserverconfig B2S.Server Configuration

To enable the plugin support in the B2S.Server (DirectOutput is a B2S.Server plugin) you need to add the ArePluginsOn option to your B2STableSettings.xml file (In the Table directory). The result should resemble to following example:
~~~~~~~~~~~~~~~{.xml}
<B2STableSettings>
  ....
  <ArePluginsOn>1</ArePluginsOn>
  ...
</B2STableSettings>
~~~~~~~~~~~~~~~

\warning This block does not go to the end of the B2STableSettings.xml file. You really only have to put the line containing _ArePluginsOn_ into the file.



\section installation_visualpinballcorevbs Visual Pinball core.vbs Adjustment

If you have used the <a target="_blank" href="http://www.hyperspin-fe.com/forum/showthread.php?10980-Tutorial-How-to-config-Ledwiz-PacDrive">VBScript solution</a> to control your LedWiz before you will have to _REMOVE_ the line loading ledcontrol.vbs from your core.vbs.

Depending on your version of core.vbs this line might looks like one of the following two:
~~~~~~~~~~~~~~~{.vbs}
ExecuteGlobal GetTextFile("ledcontrol.vbs")
~~~~~~~~~~~~~~~

~~~~~~~~~~~~~~~{.vbs}
LoadScript("ledcontrol.vbs"):Err.Clear    
~~~~~~~~~~~~~~~

If you dont remove the mentioned statement you'll run into trouble since the framework and the ledcontrol.vbs solutions will run simultaneously!

\section installation_tableconfig Table Configuration

Tables using the DirectOutput framework resp. the B2S-Server have to instanciate the B2S.Server instead of the Pinmame.Controller.
Replace the following line in the table scripts of the tables you want to use the B2S.Server and the DirectOutput framework

~~~~~~~~~~~~~~~{.vbs}
Set Controller = CreateObject("VPinMAME.Controller")     
~~~~~~~~~~~~~~~

with

~~~~~~~~~~~~~~~{.vbs}
Set Controller = CreateObject("B2S.Server") 
~~~~~~~~~~~~~~~

 For EM tables it is also necessary to a some statements to send data about the table element statuses to DirectOutput and B2S.Server.


Please read the page on \ref tableconfig for more detailed information on this topic.

\section installation_configuration Configuration 

The DirectOutput framework supports several configuration options:

* __Auto configuration__ which tries to detect the connected hardware and the files containing the table definitions automatically. Auto configuration is using the legacy ledcontrol.ini file(s) for the table definitions if available.
* __XML config files__ are the best way to configure the framework. They allow to configure every detail of the framework in detail.

\subsection installation_autoconfiguration Using Auto configuration

If correctly installed the DirectOutput framework can configure itself automatically. This means that you dont need to create any of the config files mentioned in the sections below to get started.

Auto configuration will automatically detect the LedWiz units connected to your system and try to lookup the ledcontrol.ini file(s) with table configurations in the following locations:

- Table directory
- Config directory of DirectOutput
- Directory containing the DirectOutput.dll

Up to 16 LedWiz units are supported, which can be controlled by numbered ledcontrol.ini files. For LedWiz nr. 1 the file ledcontrol.ini will be loaded, for LedWiz numbers 2-16 files ledcontrol{LedWizNumber}.ini (e.g. ledcontrol2.ini) will be loaded.

To create your own lecontrol.ini files it is best to use the <a target="_blank" href="http://vpuniverse.com/ledwiz/login.php">LedWiz Config Tool</a>.

At the moment only LedWiz units are automatically detected. If you are using other output controllers you will have to create a cabinet configuration file, specifying the output controllers and some matching LedWizEquivalent toys, to allow the use of ledcontrol files.

\subsection installation_ledcontrolini Using LedControl.ini files

The DirectOutput framework can use one or several classical LedControl.ini files to configure tables. If no other table config files are found, the framework will try to load the ledcontrol.ini file(s). By deafult the framework tries to find the ledcontrol files specified in the Global Config file. If global config does not specify ledcontrol file(s) the framework will search for ledcontrol file(s) in the following directories:

- Table directory
- Config directory of DirectOutput
- Directory containing the DirectOutput.dll

Please check the page on \ref ledcontrolfiles for details.

\subsection installation_configfiles Using configuration files

The auto configuration function of the framework does only support some basic functionality of the framework. If you want to unleash the full power of DirectOutput, you can venture into the more detailed configuration options.

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

