Supported Hardware {#hardware}
===================

\section hardware_intro Introduction

A key element when it comes to gadgets in a pinball cabinet are the installed output controllers which are controlling the physical outputs. 

The DirectOutput framework supports the use of several output controllers in parralel to get more outputs and it is even possible to use a mix of different output controllers. 

For the end user the output controllers and their drivers are hidden behind a abstraction layer which ensures, that all hardware can be accessed in a common way. For more information regarding the software side of of the output controller part of the framework, please read the page on \ref outputcontrollers

Currently the framework supports only a small number of output controllers, but depending on requirements and hardware availability more controllers will be supported in the future.

\section hardware_ready Fully supported hardware

The following devices are fully supported by the framework.  

\subsection hardware_ledwiz LedWiz (GroovyGameGear)

The LedWiz is a outputcontroller with 32 outputs which all support 49 <a target="_blank" href="https://en.wikipedia.org/wiki/Pulse-width_modulation">pwm</a> levels with a PWM frequency of approx. 50hz. The LedWiz is able to drive Leds and other small loads directly, but will require some kind of booster for power hunger gadgets like big contactors or motors are connected to it.

\image html LedWizboard.jpg

The DirectOutput framework does fully support the LedWiz and can control up to 16 LedWiz units. The framework can automatically detect connected LedWiz units and configure them for use with the framework.

The LedWiz is made by <a target="_blank" href="http://groovygamegear.com/">GroovyGameGear</a> and can by ordered directly on GroovyGamegears website, but also from some other vendors.

This unit was the first output controller which was widely used in the virtual pinball community and was the unit for which the legacy vbscript solution was developed. The DirectOutput framework replaces the vbscript solution, but can reuse the ini files which were used for the configuration of the tables. Please read \ref ledcontrolfiles for more information.

The framework supports auto detection and configuration of the LedWiz.

\image html LedWizLogo.jpg

\subsection hardware_ultimarc_pacled64 PacLed64 (Ultimarc)

The PacLed64 is a output controller with 64 outputs all supporting 256 <a target="_blank" href="https://en.wikipedia.org/wiki/Pulse-width_modulation">pwm</a> levels with a PWM frequency of 100khz. The unit is mainly designed to connect leds (cosuming 20ma each) directly to the outputs, but boosters must be used to driver higher loads (e.g. Cree leds). Up to 4 PacLed64 controllers can be used with the DirectOutput framework.

The framework supports auto detection and configuration of these units.

This unit is made and sold by <a target="_blank" href="http://www.ultimarc.com">Ultimarc</a>.

\image html PacLed64Logo.png

\subsection hardware_ultimarc_pacdrive PacDrive (Ultimarc)

The PacDrive is smaller output controller with 16 digital outputs. It is capable of driving Leds and other smaller gadgets directly, but will require a booster to driver power hungry loads (e.g. motors). 

Only 1 PacDrive unit can be connected to the system. The framework supports auto detection and configuration of that unit.

\image html pacdrivelogo.jpg

This unit is made and sold by <a target="_blank" href="http://www.ultimarc.com">Ultimarc</a>.

\subsection hardware_artnet Art-Net / DMX

<a target="_blank" href="https://en.wikipedia.org/wiki/Art-Net">Art-Net</a> is a industry standard protocol used to control <a target="_blank" href="https://en.wikipedia.org/wiki/DMX512">DMX</a> lighting effects over othernet. Using Art-Net it is possible to connect a very wide range of lighting effects like <a target="_blank" href="https://www.google.ch/search?q=dmx+strobe">strobes</a> or <a target="_blank" href="https://www.google.ch/search?q=dmx+dimmer">dimmer packs</a>. There are tons of DMX controlled effects available on the market (from very cheap and small to very expensive and big). It might sounds a bit crazy, but with Art-net and DMX you could controll a whole stage lighting system (this would likely make you feel like Tommy in the movie).

\image html DMX.png

To use Art-Net you will need a Art-Net node (unit that converts from ethernet to DMX protocol) and also some DMX controlled lighting effect. There are quite a few different Art-Net nodes available on the market and most of them should be compatible with the DirectOutput framework. For testing the Art-Net node sold on http://www.ulrichradig.de/home/index.php/avr/dmx-avr-artnetnode as a DIY kit was used. 

Each Art-Net node/DMX universe supports 512 DMX channels and several Art-Net nodes controlling different DMX universes can be used in parallel.

Here is a small demo video showing DMX/Artnet support in action:

\htmlonly
<iframe width="560" height="315" src="http://www.youtube.com/embed/F4FI1NQ5nrc" frameborder="0" allowfullscreen></iframe>
\endhtmlonly

\section hardware_development Hardware support in development

Support for the following devices is currently in development.


\section hardware_WS2811 WS2811 based led stripes

I have already installed some of those stripes in my cabinet and I want to be able to control them through DirectOutput. So this is a must!<br/>
Some working controller hardware has already been designed and tested.<br/>

Check the following video to get a idea what those stripes can do:</br>

\htmlonly
<iframe width="560" height="315" src="http://www.youtube.com/embed/Y_qV8rwzaxs" frameborder="0" allowfullscreen></iframe>
\endhtmlonly

\section hardware_other Other Hardware

Other output controllers might be supported in the future.

If you want to implement support for some outputput controller (e.g. some home grown Raspberry Pi solution), please read the page on \ref outputcontrollers for more information. Implementing drivers for new hardware should be rather simple.

