Supported Hardware {#hardware}
===================

\section hardware_intro Introduction

A key element when it comes to gadgets in a pinball cabinet are the installed output controllers which are controlling the physical outputs. 

The DirectOutput framework supports the use of several output controllers in parralel to get more outputs and it is even possible to use a mix of different output controllers. 

For the end user the output controllers and their drivers are hidden behind a abstraction layer which ensures, that all hardware can be accessed in a common way. For more information regarding the software side of of the output controller part of the framework, please read the page on \ref outputcontrollers

The following devices are supported by the framework:

\section hardware_ledwiz LedWiz (GroovyGameGear)

The LedWiz is a outputcontroller with 32 outputs which all support 49 <a target="_blank" href="https://en.wikipedia.org/wiki/Pulse-width_modulation">pwm</a> levels with a PWM frequency of approx. 50hz. The LedWiz is able to drive Leds and other small loads directly, but will require some kind of booster for power hunger gadgets like big contactors or motors are connected to it.

\image html LedWizboard.jpg

The DirectOutput framework does fully support the LedWiz and can control up to 16 LedWiz units. The framework can automatically detect connected LedWiz units and configure them for use with the framework.

The LedWiz is made by <a target="_blank" href="http://groovygamegear.com/">GroovyGameGear</a> and can by ordered directly on GroovyGamegears website, but also from some other vendors.

This unit was the first output controller which was widely used in the virtual pinball community and was the unit for which the legacy vbscript solution was developed. The DirectOutput framework replaces the vbscript solution, but can reuse the ini files which were used for the configuration of the tables. Please read \ref ledcontrolfiles for more information.

The framework supports auto detection and configuration of the LedWiz.

\image html LedWizLogo.jpg

\section hardware_ultimarc_pacled64 PacLed64 (Ultimarc)

The PacLed64 is a output controller with 64 outputs all supporting 256 <a target="_blank" href="https://en.wikipedia.org/wiki/Pulse-width_modulation">pwm</a> levels with a PWM frequency of 100khz. The unit is mainly designed to connect leds (cosuming 20ma each) directly to the outputs, but boosters must be used to driver higher loads (e.g. Cree leds). Up to 4 PacLed64 controllers can be used with the DirectOutput framework.

The framework supports auto detection and configuration of these units.

This unit is made and sold by <a target="_blank" href="http://www.ultimarc.com">Ultimarc</a>.

\image html PacLed64Logo.png

\section hardware_ultimarc_pacdrive PacDrive (Ultimarc)

The PacDrive is smaller output controller with 16 digital outputs. It is capable of driving Leds and other smaller gadgets directly, but will require a booster to driver power hungry loads (e.g. motors). 

Only 1 PacDrive unit can be connected to the system. The framework supports auto detection and configuration of that unit.

\image html pacdrivelogo.jpg

This unit is made and sold by <a target="_blank" href="http://www.ultimarc.com">Ultimarc</a>.

\section hardware_artnet Art-Net / DMX

<a target="_blank" href="https://en.wikipedia.org/wiki/Art-Net">Art-Net</a> is a industry standard protocol used to control <a target="_blank" href="https://en.wikipedia.org/wiki/DMX512">DMX</a> lighting effects over othernet. Using Art-Net it is possible to connect a very wide range of lighting effects like <a target="_blank" href="https://www.google.ch/search?q=dmx+strobe">strobes</a> or <a target="_blank" href="https://www.google.ch/search?q=dmx+dimmer">dimmer packs</a>. There are tons of DMX controlled effects available on the market (from very cheap and small to very expensive and big). It might sounds a bit crazy, but with Art-net and DMX you could controll a whole stage lighting system (this would likely make you feel like Tommy in the movie).

\image html DMX.png

To use Art-Net you will need a Art-Net node (unit that converts from ethernet to DMX protocol) and also some DMX controlled lighting effect. There are quite a few different Art-Net nodes available on the market and most of them should be compatible with the DirectOutput framework. For testing the Art-Net node sold on http://www.ulrichradig.de/home/index.php/avr/dmx-avr-artnetnode as a DIY kit was used. 

Each Art-Net node/DMX universe supports 512 DMX channels and several Art-Net nodes controlling different DMX universes can be used in parallel.

Here is a small demo video showing DMX/Artnet support in action:

\htmlonly
<iframe width="560" height="315" src="http://www.youtube.com/embed/F4FI1NQ5nrc" frameborder="0" allowfullscreen></iframe>
\endhtmlonly


\section hardware_TeensyStripController Teensy Strip Controller

The Teensy Strip Controller can control 8 channels with up to 1100 WS2811/WS2812 leds per channel. 

\image html TeensyOctoWS2811.jpg

The controller hardware uses a <a target="_blank" href="http://pjrc.com/store/teensy32_pins.html">Teensy 3.1/3.2</a> and the <a target="_blank" href="http://pjrc.com/store/octo28_adaptor.html">OctoWS2811 adaptor</a> for the Teensy. 

Compiled firmware and the source code of the firmware is available on <a target="_blank" href="https://github.com/DirectOutput/TeensyStripController">Github</a>. Be sure to read the wiki pages (also on Github) for details on setup and use.

\section hardware_WS2811 WS2811 addressable LedStrip controller

\not Work on this board has been stopped. DOF will continue to support the board anyway. If you are after a controller for WS2811/WS2812 based ledstrips check out the Teensybased controller mentioned above.

This is a small controller board by Swisslizard, which is able to control addressable ledstrips which are base on the WS2811 or the WS2812 led chip.

The hardware of this controller is based on a Atmel microcontroller and a FT245R USB interface chip by FTDI. To ensure max performance all copde on the controller has been written in assembler.

WS2811 is a small controller chip which can controll a RGB led (256 PWM level on each channel) and be daisychained, so long cahins of LEDs (led strip are possible. The WS2812 understands the same protocoll as the WS2811, but is a RGB led with integrated controller chip which allows for even more dense populated RGB strips.

Those leds chips are controlled using a single data line (there is no clock line). The data has to be sent with a frequency of 800khz. 1 bits have a duration of 0.65uS high and 0.6uS low. 0 bits have a duration of 0.25uS high and 1uS low. A interuption in the dataflow trigger the controller chips to push the data in the shift register to the PWM outputs.

\image html WS2811Controller.jpg

This is a image of my controller prototype with classical through the hole parts and a small breakoutboard by SparkFun. If feel like building your own controller board, get in touch with Swisslizard.


\section hardware_FT245bitbang FT245RL based controllers (e.g. SainSmart)

The FT245RL (http://www.ftdichip.com/Products/ICs/FT245R.htm) is a USB interface chip by FTDI with 8 output lines. If this chip is run in the so called bitbang mode each output can be controlled directly.

The SainSmart USB relay boards (http://www.sainsmart.com/arduino-compatibles-1/relay/usb-relay.html) are compatible with DOF, but other hardware which is based on the same controller chip might be compatible as well. Generally controller units which is exclusively using the FT245R (no extra cpu on board) and having max. 8 output ports are likely to be compatible. Please let me know, if you have tested other hardware successfully, so I can ammend the docu.

\image html SainSmart8PortUsbRelay.jpg SainSmart 8port USB relay board
