Toys
==========
\section toys_introduction Introduction 

Toys are the part of the DirectOutput framework representing the gadgets that you have installed in you cabinet. Typically toys are controlling one or several outputs (referenced by the name of the output) and their are themselfs controlled by effects. The framework contains a bunch of built in toys (e.g. contactor or RGB led) and additional toys can be added through scripting.

\section toys_builtin Builtin toys 

At the time of writting the framework contains the following toys:

\subsection toys_contactor Contactor 

The contactor toy represents a contactor. It implements the following important members

* __Property State__ is a readonly boolean (true or false) value representing the current state of the contactor.
* __Property OutputName__ is the name of the output assigned to the toy.
* __Method SetState(bool State)__ sets the state of the contactor. 

The XML to configure a contactor toy looks as follows:
~~~~~~~~~~~~~{.xml}
<Contactor>
  <Name>Toy name</Name>
  <OutputName>Name of a output</OutputName>
</Contactor>
~~~~~~~~~~~~~
\subsection toys_lamp Lamp
~~~~~~~~~~~~~{.xml}
<Lamp>
  <Name>Toy name</Name>
  <OutputName>Name of a output</OutputName>
</Lamp>
~~~~~~~~~~~~~
\subsection toys_rgbled RGBLed

~~~~~~~~~~~~~{.xml}
<RGBLed>
  <Name>Toy name</Name>
  <OutputNameRed>Name of a output</OutputNameRed>
  <OutputNameGreen>Name of a output</OutputNameGreen>
  <OutputNameBlue>Name of a output</OutputNameBlue>
</RGBLed>
~~~~~~~~~~~~~
\subsection toys_replayknocker ReplayKnocker

~~~~~~~~~~~~~{.xml}
<ReplayKnocker>
  <Name>Toy name</Name>
  <OutputName>Name of a output</OutputName>
  <DefaultIntervallMs>300</DefaultIntervallMs>
</ReplayKnocker>
~~~~~~~~~~~~~
\subsection toys_motor Motor/Shaker/GearMotor

~~~~~~~~~~~~~{.xml}
<Motor>
  <Name>Toy name</Name>
  <OutputName>Name of a output</OutputName>
  <MaxRunTimeMs>300000</MaxRunTimeMs>
  <MinPower>10</MinPower>
  <MaxPower>255</MaxPower>
  <KickstartPower>128</KickstartPower>
  <KickstartDurationMs>100</KickstartDurationMs>
</Motor>
~~~~~~~~~~~~~
\subsection toys_flasher Flasher
~~~~~~~~~~~~~{.xml}
<Flasher>
  <Name>Toy name</Name>
  <OutputName>Name of a output</OutputName>
  <DefaultIntervallMs>150</DefaultIntervallMs>
  <FlashDurationMs>20</FlashDurationMs>
</Flasher>
~~~~~~~~~~~~~
\subsection toys_ledwizequivalent LedWizEquivalent
~~~~~~~~~~~~~{.xml}
<LedWizEquivalent>
  <Name>Toy name</Name>
  <Outputs />
  <LedWizNumber>-1</LedWizNumber>
</LedWizEquivalent>
~~~~~~~~~~~~~
\subsection toys_genericdigitaltoy GenericDigitalToy
~~~~~~~~~~~~~{.xml}
<GenericDigitalToy>
  <Name>Toy name</Name>
  <OutputName>Name of a output</OutputName>
</GenericDigitalToy>
~~~~~~~~~~~~~
\subsection toys_genericdigitaltoy GenericAnalogToy
~~~~~~~~~~~~~{.xml}
<GenericAnalogToy>
  <Name>Toy name</Name>
  <OutputName>Name of a output</OutputName>
</GenericAnalogToy>
~~~~~~~~~~~~~

\section toys_custom Custom toys 


\subsection toys_implementationguideline  Implementation guidelines for custom toys

