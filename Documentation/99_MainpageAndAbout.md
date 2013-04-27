About
=====
The DirectOutput framework was implemented by <a href="http://www.vpforums.org/index.php?showuser=58068">SwissLizard</a>.
\image html swisslizard.png "Swisslizard"



\mainpage notitle

\image html DirectOutput_Big.png "DirectOutput framework for virtual pinball cabinets"

Welcome to the DirectOutput framework for virtual pinball cabinets
===================================================================

During my first experiments with Visual Pinbal on my laptop, I thought that it would be a cool idea to a some kind of force feedback to the system. It didn't take me long to find out that a solution for this idea, did already exist. 
When I started to build my own cabinet the current vbscript implementation supporting only one LedWiz with 32 outputs became too limited pretty soon. On somes table there were also stuttering issues. So I started to think about another solution and began to implement the DirectOutput framework.

The DirectOutput framework tries to overcome the limitations of the current solution to control toys in cabinets:

* __Stuttering__ is hopefully a thing of the past. By using several threads the framework isolates the main thread doing the work in Visual Pinball from the communication with the Ledwiz.
* __More outputs__ are also possible. Instead of using only one LedWiz it is now possible to connect several LedWiz units or other custom output controllers as well.
* __More configuration options__ are available troughthe DirectOutput framework as well.
* __Open for extensions__ means that the framework is able to load uncompiled C#-script files containing new effects, toys or other types of output controllers at runtime. After the scripts have been loaded they are copiled and integrated into the framework.

To make the start with the DirectOutput framework easy and painless it is possible to reuse the LedControl.ini file used for the configuration of the VBScript solution.
