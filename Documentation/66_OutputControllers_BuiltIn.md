Built in Output controllers  {#outputcontrollers_builtin}
==========
\section use_DirectOutput_Cab_Out_DMX_ArtNet ArtNet

\subsection use_DirectOutput_Cab_Out_DMX_ArtNet_summary Summary

Artnet is a industry standard protocol used to control <a target="_blank" href="https://en.wikipedia.org/wiki/DMX512">DMX</a> lighting effects over ethernet. Using <a target="_blank" href="https://en.wikipedia.org/wiki/Art-Net">Art-Net</a> it is possible to connect a very wide range of lighting effects like <a target="_blank" href="https://www.google.ch/search?q=dmx+strobe">strobes</a> or <a target="_blank" href="https://www.google.ch/search?q=dmx+dimmer">dimmer packs</a>. There are tons of DMX controlled effects available on the market (from very cheap and small to very expensive and big). It might sounds a bit crazy, but with Art-net and DMX you could at least in theory control a whole stage lighting system (this would likely make you feel like Tommy in the movie).

To use Art-Net you will need a Art-Net node (unit that converts from ethernet to DMX protocol) and also some DMX controlled lighting effect. There are quite a few different Art-Net nodes available on the market and most of them should be compatible with the DirectOutput framework. For testing the Art-Net node <a target="_blank" href="http://www.ulrichradig.de/home/index.php/avr/dmx-avr-artnetnode">sold by Ulrich Radig</a> as a DIY kit was used.

Each Art-Net node/DMX universe supports 512 DMX channels and several Art-Net nodes controlling different DMX universes can be used in parallel.

If you want to read more about Art-net, visit the website of <a href="http://www.artisticlicence.com">Artistic License</a>. The specs for Art-net can be found in the Resources - User Guides + Datasheets section of the site.

\image html DMX.png DMX



\subsection use_DirectOutput_Cab_Out_DMX_ArtNet_samplexml Sample XML

A configuration section for ArtNet might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<ArtNet>
  <Name>Name of ArtNet</Name>
  <Universe>0</Universe>
  <BroadcastAddress>BroadcastAddress string</BroadcastAddress>
</ArtNet>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_DMX_ArtNet_properties Properties

ArtNet has the following 3 configurable properties:

\subsubsection DirectOutput_Cab_Out_DMX_ArtNet_BroadcastAddress BroadcastAddress

String containing broadcast address. If this parameter is not set the default broadcast address (255.255.255.255) will be used.
Valid values are any IP adresses (e.g. 192.168.1.53).



\subsubsection DirectOutput_Cab_Out_DMX_ArtNet_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_DMX_ArtNet_Universe Universe

The number of the Dmx universe.



\section use_DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController DirectStripController

\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_summary Summary

This output controller class is used to control the direct strip controller by Swisslizard.

The hardware of this controller is based on a Atmel microcontroller and a FT245R USB interface chip by FTDI. To ensure max performance all copde on the controller has been written in assembler.

WS2811 is a small controller chip which can controll a RGB led (256 PWM level on each channel) and be daisychained, so long cahins of LEDs (led strip are possible. The WS2812 understands the same protocoll as the WS2811, but is a RGB led with integrated controller chip which allows for even more dense populated RGB strips.

Those controller chips are controlled using a single data line (there is no clock line). The data has to be sent with a frequency of 800khz. 1 bits have a duration of 0.65uS high and 0.6uS low. 0 bits have a duration of 0.25uS high and 1uS low. A interuption in the dataflow triggers the controller chips to push the data in the shift register to the PWM outputs. Since the timing requirements are very strict it is not easily possible to output that signal directly from a computer with normal operating system. Thats why controllers like the one displayed below are needed.

\image html WS2811Controller.jpg
This is a image of my controller prototype with classical through the hole parts and a small breakoutboard by SparkFun.
At the time of the release of DOF R2, the first prototypes of SMD version of the controller are in production. Check back in the forums for more information.



\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_samplexml Sample XML

A configuration section for DirectStripController might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<DirectStripController>
  <Name>Name of DirectStripController</Name>
  <ControllerNumber>1</ControllerNumber>
  <NumberOfLeds>1</NumberOfLeds>
  <PackData>false</PackData>
</DirectStripController>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_properties Properties

DirectStripController has the following 4 configurable properties:

\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_ControllerNumber ControllerNumber

The number of the WS2811 strip controller.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_NumberOfLeds NumberOfLeds

The number of leds on the WS2811 based led strip.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_PackData PackData

<c>true</c> if data should be packed bore it is sent to the controller; otherwise <c>false</c> (default). 
            

\section use_DirectOutput_Cab_Out_DudesCab_DudesCab DudesCab

\subsection use_DirectOutput_Cab_Out_DudesCab_DudesCab_summary Summary

The <a href="https://shop.arnoz.com/en/">Dude's Cab Controller</a> is a all-in-one virtual pinball controller

It's based on the RP2040 processor, provide 32 inputs, accelerometer, plunger &amp; Dof support.
The Dof support provides 128 PWM outputs through up to 8 extension boards.
Maximum intensity, Flipper logic, Chime logic &amp; Gamma Correction are supported for all outputs
Each outputs can also have its intensity lowered independently from the Dof values
As for the Pinscape controller, the communication is fully supported through Hid In/Out protocol with multipart messages support
The Dude's Cab controllers are registered in the Dof config tool from unit #90 to 94




\subsection use_DirectOutput_Cab_Out_DudesCab_DudesCab_samplexml Sample XML

A configuration section for DudesCab might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<DudesCab>
  <Name>Name of DudesCab</Name>
  <NumberOfOutputs>1</NumberOfOutputs>
  <Number>-1</Number>
  <MinCommandIntervalMs>1</MinCommandIntervalMs>
</DudesCab>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_DudesCab_DudesCab_properties Properties

DudesCab has the following 4 configurable properties:

\subsubsection DirectOutput_Cab_Out_DudesCab_DudesCab_MinCommandIntervalMs MinCommandIntervalMs

The mininimal interval between command in miliseconds.  The default is 1ms, which is also the minimum, since it's
the fastest that USB allows at the hardware protocol level.



\subsubsection DirectOutput_Cab_Out_DudesCab_DudesCab_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_DudesCab_DudesCab_Number Number

The unique unit number of the controller (Range 1-5).



\subsubsection DirectOutput_Cab_Out_DudesCab_DudesCab_NumberOfOutputs NumberOfOutputs

\section use_DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController FT245RBitbangController

\subsection use_DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_summary Summary

This is a generic output controller class which are based on the FT245R chip (http://www.ftdichip.com/Products/ICs/FT245R.htm). Only units using the chip in bitbang mode are supported by this output controller class.
The SainSmart USB relay boards (http://www.sainsmart.com/arduino-compatibles-1/relay/usb-relay.html) are compatible with this output controller, but other hardware which is based on the same controller chip might be compatible as well. Generally controller units which is exclusively using the FT245R (no extra cpu on board) and having max. 8 output ports are likely to be compatible. Please let me know, if you have tested other hardware successfully, so I can ammend the docu.
\image html SainSmart8PortUsbRelay.jpg SainSmart 8port USB relay board
Thanks go to <a href="http://vpuniverse.com/forums/user/3117-djrobx/">DJRobX</a> for his early implementation of a SainSmart output controller which was the starting point for the implementation of this class.



\subsection use_DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_samplexml Sample XML

A configuration section for FT245RBitbangController might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<FT245RBitbangController>
  <Name>Name of FT245RBitbangController</Name>
  <SerialNumber>SerialNumber string</SerialNumber>
  <Description>Description string</Description>
  <Id>0</Id>
</FT245RBitbangController>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_properties Properties

FT245RBitbangController has the following 4 configurable properties:

\subsubsection DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_Description Description

The description string.



\subsubsection DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_Id Id

The unique Id of the device (Range 0-9).



\subsubsection DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_SerialNumber SerialNumber

The serial number of the FT245R chip which is to be controlled.



\section use_DirectOutput_Cab_Out_LW_LedWiz LedWiz

\subsection use_DirectOutput_Cab_Out_LW_LedWiz_summary Summary

The LedWiz is a easy to use outputcontroller with 32 outputs which all support 49 <a target="_blank" href="https://en.wikipedia.org/wiki/Pulse-width_modulation">pwm</a> levels with a PWM frequency of approx. 50hz. The LedWiz is able to drive leds and smaller loads directly, but will require some kind of booster for power hungery gadgets like big contactors or motors.

\image html LedWizboard.jpg

The DirectOutput framework does fully support the LedWiz and can control up to 16 LedWiz units.

The framework can automatically detect connected LedWiz units and configure them for use with the framework.

The LedWiz is made by <a href="http://groovygamegear.com/">GroovyGameGear</a> and can by ordered directly on GroovyGamegears website, but also from some other vendors.

This unit was the first output controller which was widely used in the virtual pinball community and was the unit for which the legacy vbscript solution was developed. The DirectOutput framework replaces the vbscript solution, but can reuse the ini files which were used for the configuration of the tables. Please read \ref ledcontrolfiles for more information.

The current implementation of the LedWiz driver uses a separate thread for every ledwiz connected to the system to ensure max. performance.

\image html LedWizLogo.jpg




\subsection use_DirectOutput_Cab_Out_LW_LedWiz_samplexml Sample XML

A configuration section for LedWiz might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<LedWiz>
  <Name>Name of LedWiz</Name>
  <Number>-1</Number>
  <MinCommandIntervalMs>10</MinCommandIntervalMs>
</LedWiz>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_LW_LedWiz_properties Properties

LedWiz has the following 3 configurable properties:

\subsubsection DirectOutput_Cab_Out_LW_LedWiz_MinCommandIntervalMs MinCommandIntervalMs

The minimum interval between command in milliseconds



\subsubsection DirectOutput_Cab_Out_LW_LedWiz_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_LW_LedWiz_Number Number

The unique number of the LedWiz (Range 1-16).



\section use_DirectOutput_Cab_Out_NullOutputController_NullOutputController NullOutputController

\subsection use_DirectOutput_Cab_Out_NullOutputController_NullOutputController_summary Summary

This is a dummy output controller not doing anthing with the data it receives.<br />
It is mainly thought as a sample how to implement a simple output controller.<br /><remarks>Be sure to check the abstract OutputControllerBase class and the IOutputController interface for a better understanding.</remarks>


\subsection use_DirectOutput_Cab_Out_NullOutputController_NullOutputController_samplexml Sample XML

A configuration section for NullOutputController might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<NullOutputController>
  <Name>Name of NullOutputController</Name>
</NullOutputController>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_NullOutputController_NullOutputController_properties Properties

NullOutputController has the following 1 configurable properties:

\subsubsection DirectOutput_Cab_Out_NullOutputController_NullOutputController_Name Name

The name of the item.



\section use_DirectOutput_Cab_Out_Pac_PacDrive PacDrive

\subsection use_DirectOutput_Cab_Out_Pac_PacDrive_summary Summary

The PacDrive is a simple output controller with 16 digital/on off outputs.

DOF supports a the use of 1 PacDrive unit. This unit can be detected and configured automatically. If auto configuration is used, the generated LedWizEquivalent toy for the PacDrive will have number 19. This means that ini files numbered with 19 are automatically used to configure a PicDrive unit.

The outputs are by default turned on when the PacDrive unit is powered up. This controller class will turn off the PacDrive outputs upon initialisation and when it is finished.

This unit is made and sold by <a target="_blank" href="http://www.ultimarc.com">Ultimarc</a>.

\image html pacdrivelogo.jpg



\subsection use_DirectOutput_Cab_Out_Pac_PacDrive_samplexml Sample XML

A configuration section for PacDrive might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<PacDrive>
  <Name>PacDrive</Name>
</PacDrive>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_Pac_PacDrive_properties Properties

PacDrive has the following 1 configurable properties:

\subsubsection DirectOutput_Cab_Out_Pac_PacDrive_Name Name

The name of the item.



\section use_DirectOutput_Cab_Out_Pac_PacLed64 PacLed64

\subsection use_DirectOutput_Cab_Out_Pac_PacLed64_summary Summary

The PacLed64 is a output controller with 64 outputs all supporting 256 <a target="_blank" href="https://en.wikipedia.org/wiki/Pulse-width_modulation">pwm</a> levels with a PWM frequency of 100khz. Since the outputs of the unit are constant current drivers providing 20ma each, leds can be connected directly to the outputs (no resistor needed), but booster circuits must be used to driver higher loads (e.g. Cree leds). Up to 4 PacLed64 controllers can be used with the DirectOutput framework.

The framework supports auto detection and configuration of these units. If auto config is used, two LedWizEquivalent toys are added for each connected PacLed64. The numbers of the LedWizEquivalents are based on the Id of the PacLed64. Id1=LedwizEquivalent 20+21, Id2=LedwizEquivalent 22+23, Id3=LedwizEquivalent 24+25, Id4=LedwizEquivalent 26+27. If the numbers of ini files used for the configuration match these numbers, they will be used to set up the effects for the table.

This unit is made and sold by <a target="_blank" href="http://www.ultimarc.com">Ultimarc</a>.

The implemention of the PacLed64 driver uses a separate thread per connected unit to ensure max. performance.

\image html PacLed64Logo.png



\subsection use_DirectOutput_Cab_Out_Pac_PacLed64_samplexml Sample XML

A configuration section for PacLed64 might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<PacLed64>
  <Name>Name of PacLed64</Name>
  <MinUpdateIntervalMs>10</MinUpdateIntervalMs>
  <FullUpdateThreshold>30</FullUpdateThreshold>
  <Id>-1</Id>
</PacLed64>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_Pac_PacLed64_properties Properties

PacLed64 has the following 4 configurable properties:

\subsubsection DirectOutput_Cab_Out_Pac_PacLed64_FullUpdateThreshold FullUpdateThreshold

\subsubsection DirectOutput_Cab_Out_Pac_PacLed64_Id Id

The unique Id of the PacLed64 (Range 1-4).



\subsubsection DirectOutput_Cab_Out_Pac_PacLed64_MinUpdateIntervalMs MinUpdateIntervalMs

\subsubsection DirectOutput_Cab_Out_Pac_PacLed64_Name Name

The name of the item.



\section use_DirectOutput_Cab_Out_Pac_PacUIO PacUIO

\subsection use_DirectOutput_Cab_Out_Pac_PacUIO_summary Summary

The Ultimate I/O is a output controller with 96 outputs all supporting 256 <a target="_blank" href="https://en.wikipedia.org/wiki/Pulse-width_modulation">pwm</a> levels with a PWM frequency of 100khz. Since the outputs of the unit are constant current drivers providing 20ma each, leds can be connected directly to the outputs (no resistor needed), but booster circuits must be used to driver higher loads (e.g. Cree leds). Up to 2 Ultimate I/O (currently) can technically be used with the DirectOutput framework as supported by the PacDrive SDK (ask Ultimarc for firmware to program second ID), but this has not been tested nor confirmed.

The framework supports auto detection and configuration of these units. If auto config is used, two LedWizEquivalent toys are added for each connected Ultimate I/O. The numbers of the LedWizEquivalents are based on the Id of the PacUIO.

This unit is made and sold by <a target="_blank" href="http://www.ultimarc.com">Ultimarc</a>.

The class was based on PacLed64.cs

\image html PacUIOLogo.png



\subsection use_DirectOutput_Cab_Out_Pac_PacUIO_samplexml Sample XML

A configuration section for PacUIO might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<PacUIO>
  <Name>Name of PacUIO</Name>
  <Id>-1</Id>
</PacUIO>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_Pac_PacUIO_properties Properties

PacUIO has the following 2 configurable properties:

\subsubsection DirectOutput_Cab_Out_Pac_PacUIO_Id Id

The unique Id of the PacUIO (Range 0-1).



\subsubsection DirectOutput_Cab_Out_Pac_PacUIO_Name Name

The name of the item.



\section use_DirectOutput_Cab_Out_Pac_PhilipsHueController PhilipsHueController

\subsection use_DirectOutput_Cab_Out_Pac_PhilipsHueController_summary Summary

The Philips Hue family consists of Zigbee controlled lights and sensors using a bridge (the Philips Hue "hub").

The framework supports auto detection and configuration of these units.

Philips Hue is made and sold by <a target="_blank" href="http://www2.meethue.com">Philips</a>.

The class was based off PacUIO.cs, and implementation makes use of <a target="_blank" href="https://github.com/Q42/Q42.HueApi">Q42.HueApi</a>.
As such it retains the 3-channel RGB style inputs, but converts that over to a single-channel #rrggbb hex string.
Technically a single bridge can control about 50 lights x3 = 150 input channels (sensors are additionally ~60).

Before DOF can start communicating with the bridge it will need a valid key.
To get a key off the bridge the Link-button on the bridge needs to be pressed, and an dof#pincab "user" needs to be registered in the bridge (whitelist) within 30 seconds.
This will return a unique key (for instance "2P4R5UT6KAQcpOjFaqwLDrbikEEBsMIHY6z6Gjwg") which can then be used from that point on.
The same registration on another bridge, or even the same, will create a new key.

To avoid duplicates / spamming the whitelist, and to avoid bugs crashing the bridge, currently this is the suggested approach for getting a key:

Step 1: set static IP to bridge
Get IP of the bridge. Check your phone Hue App -&gt; Settings -&gt; My Bridge -&gt; Network settings. It should default to DHCP. Change this to a static IP, and make note of it.

Step 2: whitelist DOF using a browser
Open up the bridge API in a browser using your IP, example (replace IP with your static IP):
http://10.0.1.174/debug/clip.html
In the "CLIP API Debugger" it should say something like URL: "/api/1234/" with GET, PUT, POST, and DELETE-buttons.
Copy &amp; paste the following line into URL-field (not in your browser, but in the CLIP API Debugger):
/api
Copy &amp; paste the following into the Message Body textfield:
{"devicetype":"dof_app#pincab"}
Next, run over to your bridge and press the physical Link button. You now have 30 seconds to run back to your browser, and press "POST"-button.
You should now get a username in the Command Response textbox, for example: "ywCNFGOagGoJYtm16Kq4PS1tkGBAd3bj1ajg7uCk". Make note of this.

Step 3: add IP and key to Cabinet.xml
Open up your Cabinet.xml, and add the following lines in the OutputControllers section, replacing the IP and key with your own:
<PhilipsHueController><Name>PhilipsHueController</Name><BridgeIP>10.0.1.174</BridgeIP><BridgeKey>ywCNFGOagGoJYtm16Kq4PS1tkGBAd3bj1ajg7uCk</BridgeKey></PhilipsHueController>

Step 4: add lights using http://configtool.vpuniverse.com/login.php
A bridge can handle about 50 lights. Each light will multiplex RGB (3 channels) on each send, similar to using RGB-buttons on a PacLed64 or UIO.
To match your output channels to a specific light, use your Android / iOS Philips Hue app and decide which light you need to control.
Each light should have a number in it, for instance "10. Hue lightstrip 1". That 10-number is the light ID.
Mapped to the individual RGB-outputs in DOF Config Tool port assignments this means: ((light ID -1) * 3) + 1 = ((10 - 1) *3) + 1 = 27 +1 = 28, resulting in port 28, 29, 30 (R, G, B).
If your light ID was 3, you'd map it to port 7, 8, 9.
If your light ID was 1, you'd map it to port 1, 2, 3.


If you want to delete the key from your bridge, open up CLIP again, enter the API-URL (replace both keys with your own, they're the same used twice), then press DELETE:
http://10.0.1.174/debug/clip.html
/api/ywCNFGOagGoJYtm16Kq4PS1tkGBAd3bj1ajg7uCk/config/whitelist/ywCNFGOagGoJYtm16Kq4PS1tkGBAd3bj1ajg7uCk


///



\subsection use_DirectOutput_Cab_Out_Pac_PhilipsHueController_samplexml Sample XML

A configuration section for PhilipsHueController might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<PhilipsHueController>
  <Name>Name of PhilipsHueController</Name>
  <Id>0</Id>
  <BridgeIP>BridgeIP string</BridgeIP>
  <BridgeKey>BridgeKey string</BridgeKey>
  <BridgeDeviceType>dof_app#pincab</BridgeDeviceType>
</PhilipsHueController>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_Pac_PhilipsHueController_properties Properties

PhilipsHueController has the following 5 configurable properties:

\subsubsection DirectOutput_Cab_Out_Pac_PhilipsHueController_BridgeDeviceType BridgeDeviceType

Gets or sets active bridge device type / "app id".
For an API to communicate with a Philips Hue bridge, a first-time pairing mode using BridgeDeviceType as ID is required.
Once this pairing is complete, a BridgeKey will be generated.
To start communicating with a bridge, BridgeIP and BridgeKey is required as handshake before controlling lights.
Ideally this should not be changed / set, only read during initial pairing mode to get the actual key.



\subsubsection DirectOutput_Cab_Out_Pac_PhilipsHueController_BridgeIP BridgeIP

Gets or sets active bridge IP.



\subsubsection DirectOutput_Cab_Out_Pac_PhilipsHueController_BridgeKey BridgeKey

Gets or sets active bridge user key.



\subsubsection DirectOutput_Cab_Out_Pac_PhilipsHueController_Id Id

The unique Id of the PhilipsHueController (Range 0-1).



\subsubsection DirectOutput_Cab_Out_Pac_PhilipsHueController_Name Name

The name of the item.



\section use_DirectOutput_Cab_Out_ComPort_PinControl PinControl

\subsection use_DirectOutput_Cab_Out_ComPort_PinControl_summary Summary

PinControl is a Arduniobased output controller by http://www.vpforums.org/index.php?showuser=79113
Is has 4 pwm output, 6 digital outputs. DOF supports any number of these controllers.
Outputs 1,8,9,10 are pwm outputs.
Outputs 2,3,4,5,6,7 are digital outputs.



\subsection use_DirectOutput_Cab_Out_ComPort_PinControl_samplexml Sample XML

A configuration section for PinControl might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<PinControl>
  <Name>Name of PinControl</Name>
  <ComPort>ComPort string</ComPort>
</PinControl>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_ComPort_PinControl_properties Properties

PinControl has the following 2 configurable properties:

\subsubsection DirectOutput_Cab_Out_ComPort_PinControl_ComPort ComPort

The COM port for the controller.



\subsubsection DirectOutput_Cab_Out_ComPort_PinControl_Name Name

The name of the item.



\section use_DirectOutput_Cab_Out_PinOne_PinOne PinOne

\subsection use_DirectOutput_Cab_Out_PinOne_PinOne_summary Summary

The <a href="https://clevelandsoftwaredesign.com">PinOne Controller</a> is a free for non commercial use
software/hardware project based on the inexpensive, easy to use Arduino microcontroller development platform. Almost any
Arduino based board will work with the software, but the Cleveland Software Design PinOne board is
specifically designed to work with the software and has all the hardware components already added into it.
The ClevelandSoftwareDesign PinOne controller has 63 outputs built into the board that can be utilized by DOF. 31 outputs are
available in the board and an additional 32 outputs can be added via expansion boards.
DOF can automatically detect connected PinOne controllers and configure them for use with the framework.




\subsection use_DirectOutput_Cab_Out_PinOne_PinOne_samplexml Sample XML

A configuration section for PinOne might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<PinOne>
  <Name>PinOne Controller 01</Name>
  <NumberOfOutputs>63</NumberOfOutputs>
  <Number>1</Number>
  <MinCommandIntervalMs>1</MinCommandIntervalMs>
  <ComPort>ComPort string</ComPort>
</PinOne>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_PinOne_PinOne_properties Properties

PinOne has the following 5 configurable properties:

\subsubsection DirectOutput_Cab_Out_PinOne_PinOne_ComPort ComPort

The minimal interval between command in milliseconds.  The default is 1ms, which is also the minimum, since it's
the fastest that USB allows at the hardware protocol level.



\subsubsection DirectOutput_Cab_Out_PinOne_PinOne_MinCommandIntervalMs MinCommandIntervalMs

The minimal interval between command in milliseconds.  The default is 1ms, which is also the minimum, since it's
the fastest that USB allows at the hardware protocol level.



\subsubsection DirectOutput_Cab_Out_PinOne_PinOne_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_PinOne_PinOne_Number Number

The unique unit number of the controller (Range 1-16).



\subsubsection DirectOutput_Cab_Out_PinOne_PinOne_NumberOfOutputs NumberOfOutputs

\section use_DirectOutput_Cab_Out_PS_Pinscape Pinscape

\subsection use_DirectOutput_Cab_Out_PS_Pinscape_summary Summary

The <a href="https://developer.mbed.org/users/mjr/code/Pinscape_Controller/">Pinscape Controller</a> is an open-source
software/hardware project based on the inexpensive and powerful Freescale FRDM-KL25Z microcontroller development platform.
It provides a full set of virtual pinball cabinet I/O features, including analog plunger, accelerometer nudging, key/button
input, and a flexible array of PWM outputs.

For DOF purposes, we're only interested in the output controller features; all of the input features are handled through
the standard Windows USB joystick drivers.  The output controller emulates an LedWiz, so legacy LedWiz-aware software can
access its basic functionality.  However, the Pinscape controller has expanded functionality that the LedWiz protocol
can't access due to its inherent design limits.  To allow access to the expanded functionality, the Pinscape controller
uses custom extensions to the LedWiz protocol.  This DirectOutput framework module lets DOF use the extended protocol to
take full advantage of the extended features.  First and most importantly, the Pinscape controller can support many more
output channels than a real LedWiz.  In fact, there's no hard limit to the number of channels that could be attached
to one controller, although the practical limit is probably about 200, and the reference hardware design provides
up to about 60.  The extended protocol allows for about 130 channels, which is hopefully well beyond what anyone will
be motivated to actually build in hardware.  Second, the extended protocol provides 8-bit PWM resolution, whereas the
LedWiz protocol is limited to 49 levels (about 5-1/2 bit resolution).  DOF uses 8-bit resolution internally, so this
lets devices show the full range of brightness levels that DOF can represent internally, for smoother fades and more
precise color control in RGB devices (or more precise speed control in motors, intensity control in solenoids, etc).

DOF uses the extended protocol, so it can fully access all of the expanded features.
Legacy software that uses only the original LedWiz protocol (e.g., Future Pinball) can still recognize the device and
access the first 32 output ports, using 49-level PWM resolution.

DOF can automatically detect connected Pinscape controllers and configure them for use with the framework.

The Pinscape Controller project can be found on <a href="https://developer.mbed.org/users/mjr/code/Pinscape_Controller/">mbed.org</a>.



\subsection use_DirectOutput_Cab_Out_PS_Pinscape_samplexml Sample XML

A configuration section for Pinscape might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<Pinscape>
  <Name>Name of Pinscape</Name>
  <NumberOfOutputs>1</NumberOfOutputs>
  <Number>-1</Number>
  <MinCommandIntervalMs>1</MinCommandIntervalMs>
</Pinscape>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_PS_Pinscape_properties Properties

Pinscape has the following 4 configurable properties:

\subsubsection DirectOutput_Cab_Out_PS_Pinscape_MinCommandIntervalMs MinCommandIntervalMs

The minimal interval between command in milliseconds.  The default is 1ms, which is also the minimum, since it's
the fastest that USB allows at the hardware protocol level.



\subsubsection DirectOutput_Cab_Out_PS_Pinscape_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_PS_Pinscape_Number Number

The unique unit number of the controller (Range 1-16).



\subsubsection DirectOutput_Cab_Out_PS_Pinscape_NumberOfOutputs NumberOfOutputs

\section use_DirectOutput_Cab_Out_PSPico_PinscapePico PinscapePico

\subsection use_DirectOutput_Cab_Out_PSPico_PinscapePico_summary Summary

The <a href="https://gihub.com/mjrgh/PinscapePico/">Pinscape Pico</a> controller is an open-source
software/hardware project based on the Raspberry Pi Pico.  Pinscape Pico is a sequel to the original
Pinscape Controller for KL25Z, providing a full set of pinball cabinet I/O features, including analog
plunger, accelerometer nudging, key/button input, and a flexible array of PWM outputs.

DOF is only concerned with the Pinscape Pico's output controller features.  Pinscape Pico provides a
custom HID interface for the output controller functions, which DOF uses to send commands to the
device.  Refer to USBProtocol/FeedbackControllerInterface.h in the Pinscape Pico source repository
for documentation on the protocol.

DOF can automatically detect connected Pinscape Pico units (by scanning the set of live HID instances)
and configure them for use with the framework.  No manual user configuration is required to use these
devices.



\subsection use_DirectOutput_Cab_Out_PSPico_PinscapePico_samplexml Sample XML

A configuration section for PinscapePico might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<PinscapePico>
  <Name>Name of PinscapePico</Name>
  <NumberOfOutputs>1</NumberOfOutputs>
  <Number>-1</Number>
  <MinCommandIntervalMs>1</MinCommandIntervalMs>
</PinscapePico>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_PSPico_PinscapePico_properties Properties

PinscapePico has the following 4 configurable properties:

\subsubsection DirectOutput_Cab_Out_PSPico_PinscapePico_MinCommandIntervalMs MinCommandIntervalMs

The minimal interval between command in milliseconds.  The default is 1ms, which is also the minimum, since it's
the fastest that USB allows at the hardware protocol level.



\subsubsection DirectOutput_Cab_Out_PSPico_PinscapePico_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_PSPico_PinscapePico_Number Number

The unique unit number of the controller (Range 1-16).



\subsubsection DirectOutput_Cab_Out_PSPico_PinscapePico_NumberOfOutputs NumberOfOutputs

\section use_DirectOutput_Cab_Out_SSFImpactController_SSFImpactController SSFImpactController

\subsection use_DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_summary Summary

The SSFImpactor supports for using common audio bass shakers to emulate/simulate contractors/solenoids in a Virtual Pinball Cabinet.<br />
It presents itself as a full set of contactors, etc for assignment via DOF Config. Support, hardware suggestions, layout diagrams and the
just about friendliest people in the hobby can be found here:  <a href="https://www.facebook.com/groups/SSFeedback/" /><remarks>For help specifically with SSFImpactor look fo Kai "MrKai" Cherry.</remarks>


\subsection use_DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_samplexml Sample XML

A configuration section for SSFImpactController might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<SSFImpactController>
  <Name>Name of SSFImpactController</Name>
  <FrontExciters>Rear</FrontExciters>
  <RearExciters>RearCenter</RearExciters>
  <BassShaker1>RearCenter</BassShaker1>
  <BassShaker2>Rear</BassShaker2>
  <LowImpactMode>False</LowImpactMode>
  <DeviceNumber>-1</DeviceNumber>
  <ImpactFactor>1</ImpactFactor>
  <ShakerImpactFactor>1</ShakerImpactFactor>
  <FlipperLevel>0.25</FlipperLevel>
  <BumperLevel>0.75</BumperLevel>
  <SlingsLevel>1</SlingsLevel>
  <GearLevel>0.65</GearLevel>
</SSFImpactController>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_properties Properties

SSFImpactController has the following 13 configurable properties:

\subsubsection DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_BassShaker1 BassShaker1

One of the ManagedBass.Speaker* enums, or None



\subsubsection DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_BassShaker2 BassShaker2

One of the ManagedBass.Speaker* enums, or None



\subsubsection DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_BumperLevel BumperLevel

\subsubsection DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_DeviceNumber DeviceNumber

Number representing the Device



\subsubsection DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_FlipperLevel FlipperLevel

\subsubsection DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_FrontExciters FrontExciters

One of the ManagedBass.Speaker* enums, i.e.



\subsubsection DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_GearLevel GearLevel

\subsubsection DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_ImpactFactor ImpactFactor

Number, 0 - 100



\subsubsection DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_LowImpactMode LowImpactMode

true or false



\subsubsection DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_RearExciters RearExciters

\subsubsection DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_ShakerImpactFactor ShakerImpactFactor

\subsubsection DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_SlingsLevel SlingsLevel

\section use_DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController TeensyStripController

\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_summary Summary

The TeensyStripController is used to control up to 8 WS2811/WS2812 based ledstrips with up to 1100 leds per strip which are connected to a Teensy 3.1, 3.2 or later.

\image html TeensyOctoWS2811.jpg

The best place to get the hardware is probably the <a target="_blank" href="http://pjrc.com/teensy/">website</a> of the Teennsy inventor, where you can buy the <a target="_blank" href="http://pjrc.com/store/teensy32_pins.html">Teensy boards</a> (check if newer versions are available) and also a <a target="_blank" href="http://pjrc.com/store/octo28_adaptor.html">adapter board</a> which allows for easy connection of up to 8 led strips. There are also numerous other vendors of Teensy hardware (just ask Google).

The firmware for the Teensy based ledstrip controller is based on a slightly hacked version of Paul Stoffregens excellent OctoWS2811 LED Library which can easily drive up to 1100leds per channel on a Teensy 3.1 or later. More information on the OctoWS2811 lib can be found on the Teensy website, on Github and many other places on the internet.

Ready to use, compiled firmware files can be downloaded from the <a target="_blank" href="http://github.com/DirectOutput/TeensyStripController/releases/">Github page</a> for the TeensyStripController, the source code for the firmware is available on Github as well.




\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_samplexml Sample XML

A configuration section for TeensyStripController might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<TeensyStripController>
  <Name>Name of TeensyStripController</Name>
  <NumberOfLedsStrip1>0</NumberOfLedsStrip1>
  <NumberOfLedsStrip2>0</NumberOfLedsStrip2>
  <NumberOfLedsStrip3>0</NumberOfLedsStrip3>
  <NumberOfLedsStrip4>0</NumberOfLedsStrip4>
  <NumberOfLedsStrip5>0</NumberOfLedsStrip5>
  <NumberOfLedsStrip6>0</NumberOfLedsStrip6>
  <NumberOfLedsStrip7>0</NumberOfLedsStrip7>
  <NumberOfLedsStrip8>0</NumberOfLedsStrip8>
  <NumberOfLedsStrip9>0</NumberOfLedsStrip9>
  <NumberOfLedsStrip10>0</NumberOfLedsStrip10>
  <ComPortName>Name of ComPort</ComPortName>
  <ComPortBaudRate>9600</ComPortBaudRate>
  <ComPortParity>None</ComPortParity>
  <ComPortDataBits>8</ComPortDataBits>
  <ComPortStopBits>One</ComPortStopBits>
  <ComPortTimeOutMs>200</ComPortTimeOutMs>
  <ComPortOpenWaitMs>50</ComPortOpenWaitMs>
  <ComPortHandshakeStartWaitMs>20</ComPortHandshakeStartWaitMs>
  <ComPortHandshakeEndWaitMs>50</ComPortHandshakeEndWaitMs>
  <ComPortDtrEnable>false</ComPortDtrEnable>
</TeensyStripController>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_properties Properties

TeensyStripController has the following 21 configurable properties:

\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortBaudRate ComPortBaudRate

The baud rate of the Com port (by default 9600) the Teensy board is using.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortDataBits ComPortDataBits

The DataBits of the Com port (by default 8) the Teensy board is using.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortDtrEnable ComPortDtrEnable

\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortHandshakeEndWaitMs ComPortHandshakeEndWaitMs

The COM port wait after write in milliseconds (Valid range 50-500ms, default: 50ms).



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortHandshakeStartWaitMs ComPortHandshakeStartWaitMs

The COM port wait before read in milliseconds (Valid range 20-500ms, default: 20ms).



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortName ComPortName

The name of the Com port (typicaly COM{Number}) the Teensy board is using.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortOpenWaitMs ComPortOpenWaitMs

The COM port wait on opening in milliseconds (Valid range 50-5000ms, default: 50ms).



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortParity ComPortParity

The Parity of the Com port (by default Parity.None) the Teensy board is using.



The property ComPortParity accepts the following values:

* __None__
* __Odd__
* __Even__
* __Mark__
* __Space__


\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortStopBits ComPortStopBits

The StopBits of the Com port (by default StopBits.One) the Teensy board is using.



The property ComPortStopBits accepts the following values:

* __None__
* __One__
* __Two__
* __OnePointFive__


\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortTimeOutMs ComPortTimeOutMs

The COM port timeout in milliseconds (Valid range 1-5000ms, default: 200ms).



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip1 NumberOfLedsStrip1

The number of leds on the ledstrip connected to channel 1 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip10 NumberOfLedsStrip10

The number of leds on the ledstrip connected to channel 10 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip2 NumberOfLedsStrip2

The number of leds on the ledstrip connected to channel 2 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip3 NumberOfLedsStrip3

The number of leds on the ledstrip connected to channel 3 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip4 NumberOfLedsStrip4

The number of leds on the ledstrip connected to channel 4 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip5 NumberOfLedsStrip5

The number of leds on the ledstrip connected to channel 5 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip6 NumberOfLedsStrip6

The number of leds on the ledstrip connected to channel 6 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip7 NumberOfLedsStrip7

The number of leds on the ledstrip connected to channel 7 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip8 NumberOfLedsStrip8

The number of leds on the ledstrip connected to channel 8 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip9 NumberOfLedsStrip9

The number of leds on the ledstrip connected to channel 9 of the Teensy.



\section use_DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController WemosD1MPStripController

\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_summary Summary

The WemosD1MPStripController is a Teensy equivalent board called Wemos D1 Mini Pro (also known as ESP8266), it's cheaper than the Teensy and can support the same amount of Ledstrip and leds per strip.

\image html wemos-d1-mini-pro.jpg

The Wemos D1 Mini Pro firmware made by aetios50, peskopat and yoyofr can be found there <a target="_blank" href="https://github.com/aetios50/PincabLedStrip">aetios50 Github page</a>

You can also find a tutorial (in french for now) explaining the flashing process for the Wemos D1 Mini Pro using these firmware <a target="_blank" href="https://shop.arnoz.com/laboratoire/2019/10/29/flasher-unewemos-d1-mini-pro-pour-lutiliser-dans-son-pincab/">Arnoz' lab Wemos D1 flashing tutorial</a>

There is a great online tool to setup easily both Teensy and Wemos D1 based ledstrips (an english version is also available) <a target="_blank" href="https://shop.arnoz.com/laboratoire/2020/09/17/cacabinet-generator/">Arnoz' cacabinet generator</a>


\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_samplexml Sample XML

A configuration section for WemosD1MPStripController might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<WemosD1MPStripController>
  <Name>Name of WemosD1MPStripController</Name>
  <NumberOfLedsStrip1>0</NumberOfLedsStrip1>
  <NumberOfLedsStrip2>0</NumberOfLedsStrip2>
  <NumberOfLedsStrip3>0</NumberOfLedsStrip3>
  <NumberOfLedsStrip4>0</NumberOfLedsStrip4>
  <NumberOfLedsStrip5>0</NumberOfLedsStrip5>
  <NumberOfLedsStrip6>0</NumberOfLedsStrip6>
  <NumberOfLedsStrip7>0</NumberOfLedsStrip7>
  <NumberOfLedsStrip8>0</NumberOfLedsStrip8>
  <NumberOfLedsStrip9>0</NumberOfLedsStrip9>
  <NumberOfLedsStrip10>0</NumberOfLedsStrip10>
  <ComPortName>Name of ComPort</ComPortName>
  <ComPortBaudRate>9600</ComPortBaudRate>
  <ComPortParity>None</ComPortParity>
  <ComPortDataBits>8</ComPortDataBits>
  <ComPortStopBits>One</ComPortStopBits>
  <ComPortTimeOutMs>200</ComPortTimeOutMs>
  <ComPortOpenWaitMs>50</ComPortOpenWaitMs>
  <ComPortHandshakeStartWaitMs>20</ComPortHandshakeStartWaitMs>
  <ComPortHandshakeEndWaitMs>50</ComPortHandshakeEndWaitMs>
  <ComPortDtrEnable>false</ComPortDtrEnable>
  <SendPerLedstripLength>false</SendPerLedstripLength>
  <UseCompression>false</UseCompression>
  <TestOnConnect>false</TestOnConnect>
</WemosD1MPStripController>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_properties Properties

WemosD1MPStripController has the following 24 configurable properties:

\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortBaudRate ComPortBaudRate

The baud rate of the Com port (by default 9600) the Teensy board is using.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortDataBits ComPortDataBits

The DataBits of the Com port (by default 8) the Teensy board is using.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortDtrEnable ComPortDtrEnable

\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortHandshakeEndWaitMs ComPortHandshakeEndWaitMs

The COM port wait after write in milliseconds (Valid range 50-500ms, default: 50ms).



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortHandshakeStartWaitMs ComPortHandshakeStartWaitMs

The COM port wait before read in milliseconds (Valid range 20-500ms, default: 20ms).



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortName ComPortName

The name of the Com port (typicaly COM{Number}) the Teensy board is using.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortOpenWaitMs ComPortOpenWaitMs

The COM port wait on opening in milliseconds (Valid range 50-5000ms, default: 50ms).



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortParity ComPortParity

The Parity of the Com port (by default Parity.None) the Teensy board is using.



The property ComPortParity accepts the following values:

* __None__
* __Odd__
* __Even__
* __Mark__
* __Space__


\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortStopBits ComPortStopBits

The StopBits of the Com port (by default StopBits.One) the Teensy board is using.



The property ComPortStopBits accepts the following values:

* __None__
* __One__
* __Two__
* __OnePointFive__


\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortTimeOutMs ComPortTimeOutMs

The COM port timeout in milliseconds (Valid range 1-5000ms, default: 200ms).



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip1 NumberOfLedsStrip1

The number of leds on the ledstrip connected to channel 1 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip10 NumberOfLedsStrip10

The number of leds on the ledstrip connected to channel 10 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip2 NumberOfLedsStrip2

The number of leds on the ledstrip connected to channel 2 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip3 NumberOfLedsStrip3

The number of leds on the ledstrip connected to channel 3 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip4 NumberOfLedsStrip4

The number of leds on the ledstrip connected to channel 4 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip5 NumberOfLedsStrip5

The number of leds on the ledstrip connected to channel 5 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip6 NumberOfLedsStrip6

The number of leds on the ledstrip connected to channel 6 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip7 NumberOfLedsStrip7

The number of leds on the ledstrip connected to channel 7 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip8 NumberOfLedsStrip8

The number of leds on the ledstrip connected to channel 8 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip9 NumberOfLedsStrip9

The number of leds on the ledstrip connected to channel 9 of the Teensy.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_SendPerLedstripLength SendPerLedstripLength

true if the commands are sent



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_TestOnConnect TestOnConnect

true, will ask for the test at connection stage



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_UseCompression UseCompression

true, use the compression when it worth it



\section use_DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController WS2811StripController

\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_summary Summary

The WS2811StripController class is just a simple wrapper around the DirectStripController class. It is only here to allow the use of old configs.
Use the DirectStripController class for your configs.
\deprecated The use of this class is deprecated. Please use the DirectStripController class instead.



\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_samplexml Sample XML

A configuration section for WS2811StripController might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<WS2811StripController>
  <Name>Name of WS2811StripController</Name>
  <ControllerNumber>1</ControllerNumber>
  <NumberOfLeds>1</NumberOfLeds>
  <PackData>false</PackData>
</WS2811StripController>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_properties Properties

WS2811StripController has the following 4 configurable properties:

\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_ControllerNumber ControllerNumber

The number of the WS2811 strip controller.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_NumberOfLeds NumberOfLeds

The number of leds on the WS2811 based led strip.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_PackData PackData

<c>true</c> if data should be packed bore it is sent to the controller; otherwise <c>false</c> (default). 
            

