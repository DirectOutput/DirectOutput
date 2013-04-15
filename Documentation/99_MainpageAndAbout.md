About
=====
The DirectOutput framework was implemented by <a href="http://www.vpforums.org/index.php?showuser=58068">SwissLizard</a>.
\image html swisslizard.png "Swisslizard"



\mainpage notitle

\image html DirectOutput_Big.png "DirectOutput framework for virtual pinball cabinets"

Welcome to the DirectOutput framework for virtual pinball cabinets
===================================================================


This framework tries to solve several issues with the current solution to control toys in cabinets:

* __Stuttering__ is hopefully a thing of the past. By using several threads the framework isolates the main thread doing the work in Visual Pinball from the communication with the Ledwiz.
* __More outputs__ are also possible. Instead of using only one LedWiz it is now possible to connect several LedWiz units 
* __More configuration options__ are available troughthe DirectOutput framework as well.
* __Open for extensions__ means that the framework is able to load uncompiled C#-script files containing new effects, toys or other types of output controllers at runtime. After the scripts have been loaded they are copiled and integrated into the framework.

To make the start with the DirectOutput framework easy and painless it is possible to reuse the LedControl.ini file used for the configuration of the VBScript solution.