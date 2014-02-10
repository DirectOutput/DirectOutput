Built in Output controllers  {#outputcontrollers_builtin}
==========
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
</LedWiz>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_LW_LedWiz_properties Properties

LedWiz has the following 2 configurable properties:

\subsubsection DirectOutput_Cab_Out_LW_LedWiz_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_LW_LedWiz_Number Number

The unique number of the LedWiz (Range 1-16).



\section use_DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController WS2811StripController

\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_summary Summary

This output controller class is used to control the WS2811 led strip controller by Swisslizard.

The hardware of this controller is based on a Atmel microcontroller and a FT245R USB interface chip by FTDI. To ensure max performance all copde on the controller has been written in assembler.

WS2811 is a small controller chip which can controll a RGB led (256 PWM level on each channel) and be daisychained, so long cahins of LEDs (led strip are possible. The WS2812 understands the same protocoll as the WS2811, but is a RGB led with integrated controller chip which allows for even more dense populated RGB strips.

Those controller chips are controlled using a single data line (there is no clock line). The data has to be sent with a frequency of 800khz. 1 bits have a duration of 0.65uS high and 0.6uS low. 0 bits have a duration of 0.25uS high and 1uS low. A interuption in the dataflow trigger the controller chips to push the data in the shift register to the PWM outputs.



\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_samplexml Sample XML

A configuration section for WS2811StripController might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<WS2811StripController>
  <Name>Name of WS2811StripController</Name>
  <ControllerNumber>1</ControllerNumber>
  <NumberOfLeds>1</NumberOfLeds>
</WS2811StripController>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_properties Properties

WS2811StripController has the following 3 configurable properties:

\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_ControllerNumber ControllerNumber

The number of the WS2811 strip controller.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_NumberOfLeds NumberOfLeds

The number of leds on the WS2811 based led strip.



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
  <Id>-1</Id>
</PacLed64>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_Pac_PacLed64_properties Properties

PacLed64 has the following 2 configurable properties:

\subsubsection DirectOutput_Cab_Out_Pac_PacLed64_Id Id

The unique Id of the PacLed64 (Range 1-4).



\subsubsection DirectOutput_Cab_Out_Pac_PacLed64_Name Name

The name of the item.



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



\section use_DirectOutput_Cab_Out_SainSmart_SainSmartIO SainSmartIO

\subsection use_DirectOutput_Cab_Out_SainSmart_SainSmartIO_summary Summary

The SainSmart is a output controller with 4 or more relay outputs

The framework supports auto detection and configuration of these units.

This unit is made and sold by <a target="_blank" href="http://www.sainsmart.com">Ultimarc</a>.

The implemention of the SainSmartIO driver uses a separate thread per connected unit to ensure max. performance.

\image html SainSmartIOLogo.png



\subsection use_DirectOutput_Cab_Out_SainSmart_SainSmartIO_samplexml Sample XML

A configuration section for SainSmartIO might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<SainSmartIO>
  <Name>Name of SainSmartIO</Name>
  <Id>-1</Id>
</SainSmartIO>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Out_SainSmart_SainSmartIO_properties Properties

SainSmartIO has the following 2 configurable properties:

\subsubsection DirectOutput_Cab_Out_SainSmart_SainSmartIO_Id Id

The unique Id of the SainSmartIO (Range 1-4).



\subsubsection DirectOutput_Cab_Out_SainSmart_SainSmartIO_Name Name

The name of the item.



\section use_DirectOutput_Cab_Out_Pac_PacDrive PacDrive

\subsection use_DirectOutput_Cab_Out_Pac_PacDrive_summary Summary

The PacDrive is a simple output controller with 16 digital/on off outputs.

DOF supports a the use of 1 PacDrive unit. This unit can be detected and configured automatically. If auto configuration is used, the generated LedWizEquivalent toy for the PacDrive will have number 19. This means that ini files numbered with 19 are automatically used to configure a PicDrive unit.

The outputs are by default turned on when the PacDrive unit is powered up. This controller class will turn off the PacDrive outputs upon initialisation and when it is finished.

This unit is made and sold by <a target="_blank" href="http://www.ultimarc.com">Ultimarc</a>.

\image html PacDriveLogo.png



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



