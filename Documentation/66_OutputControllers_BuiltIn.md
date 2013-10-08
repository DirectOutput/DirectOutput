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

\subsubsection DirectOutput_Cab_Out_LW_LedWiz_Number Number

Gets or sets the number of the LedWiz.<br />
The number of the LedWiz must be unique.<br />
Setting changes the Name property, if it is blank or if the Name coresponds to LedWiz {Number}.



__Value__

The unique number of the LedWiz (Range 1-16).



\subsubsection DirectOutput_Cab_Out_LW_LedWiz_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



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

Gets or sets the Id of the PacLed64.<br />
The Id of the PacLed64 must be unique and in the range of 1 to 4.<br />
Setting changes the Name property, if it is blank or if the Name coresponds to PacLed64 {Id}.



__Value__

The unique Id of the PacLed64 (Range 1-4).



\subsubsection DirectOutput_Cab_Out_Pac_PacLed64_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



\section use_DirectOutput_Cab_Out_DMX_ArtNet ArtNet

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

\subsubsection DirectOutput_Cab_Out_DMX_ArtNet_Universe Universe

Gets or sets the number of the Dmx universe for the ArtNet node.



__Value__

The number of the Dmx universe.



\subsubsection DirectOutput_Cab_Out_DMX_ArtNet_BroadcastAddress BroadcastAddress

Gets or sets the broadcast address for the ArtNet object.<br />
If no BroadcastAddress is set, the default address (255.255.255.255) will be used.



__Value__

String containing broadcast address.



\subsubsection DirectOutput_Cab_Out_DMX_ArtNet_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



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

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



