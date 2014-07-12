Source Code {#sourcecode}
==========

\section sourcecode_introduction Introduction 

The source code of the DirectOutput framework is freely available on <a target="_blank" href="https://github.com/DirectOutput/DirectOutput/">GitHub</a>.

You are more than welcome to download, analyze, modify, improve or extend the code. The best way to do this is to fork the source code (using Git), so you can easily upload your changes back to GitHub.

The framework has been implmented in C# using Visual Studio 2010.

\section sourcecode_fork Download source code/Fork source code

If you want to download a copy of the source code, it is best if you directly fork it from GitHub. More information on forking repos/projects can be found on the following GitHub Help page: <a target="_blank" href="https://help.github.com/articles/fork-a-repo">Fork a repo</a>. Appart from the commandline git version explained on the help page you can also use one of the GUIs for Git available on the internet. 

\section sourcecode_pull Contribute your code/Pull requests

If you think, that you have made some change or extensions to the source which you want to contribute to the project, please file a pull request on GitHub. Details can be found here: <a target="_blank" href="https://help.github.com/articles/using-pull-requests">Using Pull Requests</a>.


\section sourcecode_tools Tools

The following tools and addons have been used to develop DirectOutput:

- Visual Studio 2010. You can get the free Express version of Visual Studio from Microsofts website.
- Git Source Control Provider. This addon for Visual Studio is available from http://visualstudiogallery.msdn.microsoft.com/63a7e40d-4d71-4fbb-a23b-d262124b8f4c. It integrates directly with Visual Studio and allows you to exceute most of the relevant Git operations directly from Visual Studio.
- GhostDoc was used to help documenting the code. This addon for Visual Studio is available at <a target="_blank" href="http://submain.com/products/ghostdoc.aspx">http://submain.com/products/ghostdoc.aspx</a>.
- DoxyGen is used to generate the documention on the framework. Doxygen is open source and can be found at <a target="_blank" href="http://www.doxygen.org/">http://www.doxygen.org/</a>. The config file for doxygen is included in the project files on GitHub.
- 7Zip is used to create the deployment packages of the framework. It is available at <a target="_blank" href="http://www.7-zip.org/">http://www.7-zip.org/</a>.


\section sourcecode_use Using the source code

After you have forked the source code, you can open the DirectOutput.sln file in Visual Studio and start plaing with the code.

However, if you want to sucessfully build the framework, you might need to adjust the postbuild events of the included projects. Most likely you will need to adjust the paths to the external tools used to create the docu (Doxygen) and the deployment packages (7-Zip).

\section sourcecode_where What is where

The source code of the DirectOutput framework consists of several projects. This chapter will give you some hints on where to find which part of the code. 
Detailed information on all classes and methods can be found in the chapters on Packages, Classes and Files.

\subsection sourcecode_wheredirectoutput DirectOutput project

The DirectOutput project contains the actual Directoutput framework. The main areas are:

- <b>Cab</b> containing all cabinet related classes (e.g. toys and output controllers).
- <b>Cab/Out</b> contains everthing related to output controllers and controlling physical outputs in your cabinet.
- <b>Cab/Out/Lw</b> contains the output controller classes for the LedWiz.
- <b>Cab/Toys</b> contains all toys.
- <b>Config</b> is only a directory and not a namespace. It is the home to the example config files of the framework.
- <b>Docu</b> just stores some class diagrams which have been used to genrate the images for the documentation.
- <b>Extension</b> some extension to the built in objects of C# (e.g. the between method for int can be found here).
- <b>FrontEnd</b> is the place where the forms of the front are stored.
- <b>FX</b> contains everything which is related to effects.
- <b>FX/BasicFX</b> contains a bunch of basic effects.
- <b>FX/LedControlFX</b> holds the effects which is used to make directoutputconfig.ini files work with the framework.
- <b>FX/ListFX</b> is home to the ListEffect, providing functiality to trigger several effects in a list.
- <b>General</b> contains classes which are of general use (e.g. a filereader).
- <b>GlobalConfig</b> contains all classes required for the global configuration of the system.
- <b>LedControl</b> is home to everthing required to load the classical directoutputconfig.ini files. I think the code is a bit messy, since it had to be created on some kind of trial and error basis.
- <b>InputHandling</b> is the place where the data received from the outside world is processed and queued. 
- <b>Scripting</b> contaings the logic to load and compile external script files using CSScript.
- <b>Table</b> contains classes related to the table which is currently played (e.g. status of the table elements).

\subsection sourcecode_wheredocu Documentation project

The documentation on the DirectOutput framework is generated from the comments in the source code using Doxygen. These comments contain a lot of information about the classes and their methods, but no other information (e.g. installtion instructions).

In the Documentation project you can find all the other pages of the documentation in the form of somewhat messy markdown files.

This project was only added, to allow documentation editing within Visual Studio.


\subsection sourcecode_whereplugin B2SServerPlugin project

The B2SServerPlugin project contains the implementaion of the plugin for the B2SServer.

Nothing else should be put into this project to avoid depedency issues.

For more information about the plugin please read \ref b2sserverplugin

\subsection sourcecode_wheredplugin LedControlFileTester project

This project contains the source code for the LedControlFileTester.exe which as part of the DirectOutput package.

Read \ref ledcontrol_testingapp for more information on this tool.


\section sourcecode_extend Extending and changing the source code (guidelines)

Every contribution to the source code is very welcome! It is however recommend to observe a few guidelines:

- Please document you code using XML comments. Doxygen is used to create the documentation of the framework and will automatically create docu pages for all classes using the XML comments you enter. At least the classes themselfs and the public methods sould be documented. GhostDoc can be very helpfull the create the necessary comments.
- Put things in the right namespace and directory. There a separate namespaces for the major areas of the framework. Please put your code into a meaningfull namespace. If you implement something completly new (e.g. a new output controller), consider putting your addition into a sub namespace (e.g. DirectOutput.Cab.Out.MyOutputController), but think twice before you add a new main namespace (e.g. DirectOutput.MyAddition).
- Use descriptive names for classes, methods and namespaces.
- Think about exception handling. To ensure reliable operation of the framework, please implement proper exception handling for your classes.
- Be very carefull and conservative when changing one of the outside interfaces (e.g. B2S Server Plugin) of the framework. Changes in these areas might lead to compatibility problems with external programms and components.
- The framework is using <a target="_blank" href="http://support.microsoft.com/kb/815813/en-us">XMLSerialization</a> to save and load the configuration of the framework. This means that everything which has to be saved in the configuration must be XML serializable. If your class doesn't work with the built in XML serializer, you might need to implement the <a href="http://msdn.microsoft.com/en-us/library/system.xml.serialization.ixmlserializable.aspx">IXmlSerializable interface</a>.
- If you are working on effects, toys or output controllers, please read the implementation guidelines on the documention pages on these topics.
- If you are planning for bigger extensions or changes it might be wise, to check with the <a target="_blank" href="https://github.com/DirectOutput?tab=members">members of the DirectOutput project</a> first.
 










