About and Copyright
=====
The DirectOutput framework has been implemented by <a href="http://vpuniverse.com/forums/user/668-swisslizard/">SwissLizard</a>. 

\image html swisslizard.png "Swisslizard"



\mainpage notitle

\image html DirectOutput_Big.png "DirectOutput framework for virtual pinball cabinets"

Welcome to the DirectOutput framework for virtual pinball cabinets
===================================================================

\note Please take note that this documentation is not yet complete and that some parts of it might change in the future. 
\note This project is still in a beta stutus. This means that not all functionality has been fully tested and that problems have to be expected. 

During my first experiments with Visual Pinball on my laptop, I thought that it would be a cool idea to a some kind of force feedback to the system. It didn't take me long to find out that a solution for this idea did already exist. 
When I started to build my own cabinet the current vbscript implementation supporting only a single LedWiz with 32 outputs became too limited pretty soon. On some table there were also stuttering issues. So I started to think about another solution and began to implement the DirectOutput framework.

The DirectOutput framework tries to overcome some limitations of the current solution to control toys in cabinets:

* __Stuttering__ is hopefully a thing of the past. By using several threads the framework isolates the main thread doing the work in Visual Pinball from the communication with the Ledwiz and other output controllers.
* __More outputs__ are also possible. Instead of using only one LedWiz it is now possible to connect several LedWiz units or other output controllers.
* __More configuration options__ are available troughthe DirectOutput framework as well.
* __Legacy support__ by supporting LedControl.ini file(s).
* __Open for extensions__ means that the framework is able to load uncompiled C#-script files containing new effects, toys or other types of output controllers at runtime. After the scripts have been loaded they are copiled and integrated into the framework.

If you are new to the DirectOutput framework the page on \rel installation is a good starting point.

Enjoy (virtual) pinball and keep pinball alive!

\image html swisslizard.png Swisslizard
