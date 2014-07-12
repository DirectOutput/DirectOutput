About and Copyright
=====
The DirectOutput framework R2 has been implemented by <a target="_blank" href="http://vpuniverse.com/forums/user/668-swisslizard/">SwissLizard</a>. 

\image html swisslizard.png "Swisslizard"



\mainpage notitle

\image html DirectOutputR2_Big.png 

Welcome to the DirectOutput framework R2 for virtual pinball cabinets
=====================================================================

\warning This software has been designed to control hardware which is connected to a computer. This means that there is always a risk that something goes wrong and that your hardware or something else gets damaged. You use this software at your own risk! Dont blame me if your boards go up in smoke, your house burns down or something or someone else gets damaged. You have been warned! 

\note Please take note that this documentation is not yet complete (will probably never be) and that some parts of it might change in the future. 

This is the documentation for the second release (R2) of the Direct Output framework which has been released in july 2014. Check the \ref DOFHistory "History" for the list of changes in R2. This software will help you to connect all kinds of fun gadgets which you might have connected to your virtual pinball cabinet. 

For a start, here is a small video showing some of the possibilities of the Direct Output framework.

\htmlinclude 99_Mainpage_DOFVideo.html

The key features of the DirectOutput framework are:

* Support of up to 16 \ref hardware_ledwiz "LedWiz" units.
* Support of 4 \ref hardware_ultimarc_pacled64 "Pacled64" units and one \ref hardware_ultimarc_pacdrive "PacDrive" unit (including a mix of them).
* Support of \ref hardware_artnet "Art-net/DMX".
* Support of 1 \ref hardware_pacdrive "PacDrive" unit.
* Support of up to 16 \ref hardware_WS2811 "WS2811 led strip controllers".
* Support of any number of \ref hardware_FT245bitbang "FT245RL bitbang controllers" (e.g. SainSmart).
* Multithreaded to ensure optimal performance and avoid stuttering issues.
* \ref b2sserverplugin "B2S.Server integration".
* \ref tableconfig_VPEM "EM table support".
* Loads of new \ref inifiles_settingspara "config options in ini files".
* Extendable object oriented architecture.
* Own more powerfull XML config file format.
* Coded in C#.
* Fully documented code (that is what you're looking at).

If you are new to the DirectOutput framework the pages on \ref installation "installation" and \ref tableconfig "tableconfig" are a good starting point. If you want to know what hardware you can connect to your system have a look at the \ref outputcontrollers_builtin "Builtin Outputcontrollers" and the \ref "Builtin Toys" pages.

Enjoy (virtual) pinball and keep pinball alive!

\image html swisslizard.png Swisslizard
