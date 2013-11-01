About and Copyright
=====
The DirectOutput framework has been implemented by <a href="http://vpuniverse.com/forums/user/668-swisslizard/">SwissLizard</a>. 

\image html swisslizard.png "Swisslizard"



\mainpage notitle

\image html DirectOutput_Big.png 

Welcome to the DirectOutput framework for virtual pinball cabinets
===================================================================

\warning This software has been designed to control hardware which is connected to a computer. This means that there is always a risk that something goes wrong and that your hardware or something else gets damaged. You use this software at your own risk! Dont blame me if your boards go up in smoke, your house burns down or something or someone else gets damaged. You have been warned! 

\note Please take note that this documentation is not yet complete and that some parts of it might change in the future. 

During my first experiments with Visual Pinball on my laptop, I thought that it would be a cool idea to a some kind of force feedback to the system. It didn't take me long to find out that a solution for this idea did already exist. 
When I started to build my own cabinet the current solution supporting only a single LedWiz with 32 outputs became too limited pretty soon. On some table there were also stuttering issues. So I started to think about another solution and began to implement the DirectOutput framework. From a small thing to solve a few problems with my cabinet, this has fast grown into something mucher and more complex, with a lot of new config options, various types of supported hardware and a lot of other features.

Some key features of the DirectOutput framework are:

* Support of up to 16 \ref hardware_ledwiz "LedWiz" and 4 \ref hardware_ultimarc_pacled64 "Pacled64" units (including a mix of them).
* Support of \ref hardware_artnet "Art-net/DMX".
* Multithreaded to ensure optimal performance and avoid stuttering issues.
* \ref b2sserverplugin "B2S.Server integration".
* \ref tableconfig_VPEM "EM table support".
* Loads of new config options.
* Extendable through scripting.
* Extendable object oriented architecture.
* Support of the \ref inifiles "legacy ini file" format for configs.
* Own more powerfull XML config file format.
* Coded in C#.
* Fully documented code.

If you are new to the DirectOutput framework the pages on \rel installation and \rel tableconfig are a good starting point.

Enjoy (virtual) pinball and keep pinball alive!

\image html swisslizard.png Swisslizard
