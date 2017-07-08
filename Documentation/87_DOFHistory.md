DOF History  {#dofhistory}
========


_2017.7_

- Recompiled this documentation using Doxygen.


_2017.5-2017.6_

- Support for Philips Hue.
- Support for <TableOverrideSettings> for overriding outputs using Cabinet.xml per-table, per-device, per-output etc.
- Support for <SequentialOutputSettings> in Cabinet.xml for output cycling for contactors. Can be used during retriggers of an output quicker than a contactor can respond to when hooked onto a Sainsmart. For instance, left flipper -> left slingshot if left flipper contactor is triggered within say 300ms.


_2017.2-2017.4_

- Support for I-PAC Ultimate I/O.
- Auto detects SainSmart USB devices (serial, device). Remove entries in Cabinet.xml for this to go into effect.
- Support for <ScheduledSettings> in Cabinet.xml ("day & night modes"). Defines a time region, a setting name, and a list of devices. Each device has a set of ports that will get their output values modified by OutputPercent. Note that using LED-amps will render anything 1-99 in effect 100 due to the LED-amps taking over.


_2015.6-2015.10_

- Pinballx Plugin improved
- More area/matrix effects added
- new INI file paras added
- support for TeensyStripController added
- Flicker effect improved
- Shape effect added
- many other small improvements 


__2014.10__

- Plugin for Pinballx implemented
- New ini file para for matrix effects
- Support for Pincontrol added

__2014.09__

- Fix for random outputs/random knocks on LedWiz units implemented.

__2014.7.12__

After a lot of testing the second version of DOF is released.

What is new in DOF2:
- Support for addressable ledstrips 
- Support for Sainsmart and other FT244R based controllers 
- Pacled64 problems fixed 
- LedWiz uses now the whole value range from 0 to 49 (instead of only 48 before)
- Better toy objects for Shaker and GearMotor. New settings for MinPower, MaxPower, KickstartPower and KickStartDuration.
- LedStrip toy which allows for the support of addressable ledstrips. The toy support simple strips, but also two dimensional arrays of ledstrips. 
- RGBAToyGroup and AnalogAlphaToyGroup toys support the grouping of RGB toys (e.g. RGBLeds) resp. AnalogAlpha toys (e.g. single color lamps) in arrays, so they can be controlled be the same effects as the ledstrips.
- Most toys do now support fading curves (either predefined curves or your own curves can be used) to fine tune the fading behaviour for the toys.
- New general use effects (e.g. ValueInvertEffect) have been added.
- Special effects for LedStrips and toy groups, including a bitmap animation effect, have been added.
- Loads of new config options for the config ini files are available as well.
- Color in ini files can now also be specified as hex colors (#ff0000 for red).
- Conditions for effect triggering in ini files, e.g. (S48<>0 and S49=0) will only trigger the effect of solenoid 48 is active and solenoid 49 is inactive.
- Lists of table elements can also be used to trigger effects, e.g. S48|S49 will trigger the effect if either solenoid 48 or 49 is activated.
- Variables can be used in ini files (Sorry, docu not yet written)..
- A update notification has been added, which will ask you to update DOF if you are using ini files which require a new DOF version to support all settings.
- The global config system has been changed to make it a bit easier to understand. There is even a small tool to edit your global configs: GlobalConfigEditor.exe
- Many small changes behind the scenes and some bugs fixed as well.


__2014.4__

- More matrix effects added.
- Grouping/matrix toys added for rgb and single channel toys.
- More ini file config options.
- Global config changed.
- Global config editor created .
- Some bugs squashed.

__2014.3__

- Ledstrip support added
- PacDrive support added
- FT245RL bitbang based controller support added (e.g. for SainSmart).
- New effects added
- Effects for areas resp. matrices of RGBA toys added.
- Trigger condition effect added.
- Support for userdefined fading curves added. 
- Loads of new config options for ini files added.
- Support for ini files with more than 32 columns added.

__2013.11.1__

- First public release of the DirectOutput framerwork.
- First upload of the source code to GitHub.

__2013.8__

- Beta testing started

__2013.6__

- Alpha testing

__2013.4-2013.5__

- Further development, documentation extended

__2013.3__

- Created GitHub repository for DirectOutput. Uploaded preliminary documentation.

__2013.2__

- First working version of the framework, including plugin infrustructure of the B2S server. Works nicely on my cabinet :)

__2013.1__

- First implementation using the B2S Server for table event feeds.

__2012.12__

- Started development of the framework.


