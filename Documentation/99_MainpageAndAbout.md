About and Copyright
=====
The DirectOutput framework has been implemented by <a href="http://vpuniverse.com/forums/user/668-swisslizard/">SwissLizard</a>. 

\image html swisslizard.png "Swisslizard"



\mainpage notitle

\image html DirectOutput_Big.png "DirectOutput framework for virtual pinball cabinets"

Welcome to the DirectOutput framework for virtual pinball cabinets
===================================================================

\warning This software has been designed to control hardware which is connected to a computer. This means that there is always a risk that something goes wrong and that your hardware or something else gets damaged. You use this software at your own risk. Dont blame me if your boards go up in smoke, your house burns down or something or someone else gets damaged. You have been warned! 

\note Please take note that this documentation is not yet complete and that some parts of it might change in the future. 

During my first experiments with Visual Pinball on my laptop, I thought that it would be a cool idea to a some kind of force feedback to the system. It didn't take me long to find out that a solution for this idea did already exist. 
When I started to build my own cabinet the current solution supporting only a single LedWiz with 32 outputs became too limited pretty soon. On some table there were also stuttering issues. So I started to think about another solution and began to implement the DirectOutput framework.

Some key features of the DirectOutput framework are:

* Support of up to __16 LedWiz__ and 4 __Pacled64__ units (including a mix of them).
* Support of __Art-net/DMX__.
* __Multithreaded__ to ensure optimal performance and avoid stuttering issues.
* __B2S.Server integration__.
* __EM table support__.
* Loads of __new config options__.
* Extendable through scripting.
* Extendable object oriented architecture.
* Support of the legacy in file format for configs.
* Own more powerfull XML config file format.
* Coded in C#.
* Fully documented code.

If you are new to the DirectOutput framework the page on \rel installation is a good starting point.

Enjoy (virtual) pinball and keep pinball alive!

\image html swisslizard.png Swisslizard
