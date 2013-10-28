Built in Output controllers  {#outputcontrollers_builtin}
==========
\section use_DirectOutput_Cab_Out_LW_LedWiz LedWiz

\subsection use_DirectOutput_Cab_Out_LW_LedWiz_summary Summary

The LedWiz is a easy to use outputcontroller with 32 outputs which all support 49 <a target="_blank" href="https://en.wikipedia.org/wiki/Pulse-width_modulation">pwm</a> levels. The LedWiz is able to drive leds and smaller loads directly, but will require some kind of booster for power hungery gadgets like big contactors or motors.

\image html LedWizboard.jpg

The DirectOutput framework does fully support the LedWiz and can control up to 16 LedWiz units. The framework can automatically detect connected LedWiz units and configure them for use with the framework.

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



\section use_DirectOutput_Cab_Out_Pac_PacLed64 PacLed64

\subsection use_DirectOutput_Cab_Out_Pac_PacLed64_summary Summary

The PacLed64 is a output controller with 64 outputs all supporting 256 <a target="_blank" href="https://en.wikipedia.org/wiki/Pulse-width_modulation">pwm</a> levels. Since the outputs of the unit are constant current drivers providing 20ma each, smaller leds can be connected directly to the outputs, but booster circuits might be used to driver higher loads (e.g. Cree leds). Up to 4 PacLed64 controllers can be used with the DirectOutput framework.

The framework supports auto detection and configuration of these units.

This unit is made and sold by <a target="_blank" href="http://www.ultimarc.com">Ultimarc</a>.

The implemention of the PacLed64 driver uses a separate thred per connected unit to ensure max. performance.

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

Artnet is a industry standard protocol used to control <a target="_blank" href="https://en.wikipedia.org/wiki/DMX512">DMX</a> lighting effects over othernet. Using <a target="_blank" href="https://en.wikipedia.org/wiki/Art-Net">Art-Net</a> it is possible to connect a very wide range of lighting effects like <a target="_blank" href="https://www.google.ch/search?q=dmx+strobe">strobes</a> or <a target="_blank" href="https://www.google.ch/search?q=dmx+dimmer">dimmer packs</a>. There are tons of DMX controlled effects available on the market (from very cheap and small to very expensive and big). It might sounds a bit crazy, but with Art-net and DMX you could at least in theory control a whole stage lighting system (this would likely make you feel like Tommy in the movie).

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

String containing broadcast address.



\subsubsection DirectOutput_Cab_Out_DMX_ArtNet_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Out_DMX_ArtNet_Universe Universe

The number of the Dmx universe.



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



