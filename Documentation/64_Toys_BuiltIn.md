Built in Toys  {#toy_builtin}
==========
\section use_DirectOutput_Cab_Toys_Basic_RGBLed RGBLed

\subsection use_DirectOutput_Cab_Toys_Basic_RGBLed_summary Summary

RGB led toy controlls a multicolor led.
Implement IToy, inherits Toy.



\subsection use_DirectOutput_Cab_Toys_Basic_RGBLed_samplexml Sample XML

A configuration section for RGBLed might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBLed>
  <Name>Name of RGBLed</Name>
  <OutputNameRed>OutputNameRed string</OutputNameRed>
  <OutputNameGreen>OutputNameGreen string</OutputNameGreen>
  <OutputNameBlue>OutputNameBlue string</OutputNameBlue>
</RGBLed>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Basic_RGBLed_properties Properties

RGBLed has the following 4 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Basic_RGBLed_OutputNameRed OutputNameRed

Name of the IOutput for red.



\subsubsection DirectOutput_Cab_Toys_Basic_RGBLed_OutputNameGreen OutputNameGreen

Name of the IOutput for green.



\subsubsection DirectOutput_Cab_Toys_Basic_RGBLed_OutputNameBlue OutputNameBlue

Name of the IOutput for blue.



\subsubsection DirectOutput_Cab_Toys_Basic_RGBLed_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



\section use_DirectOutput_Cab_Toys_Basic_DigitalToy DigitalToy

\subsection use_DirectOutput_Cab_Toys_Basic_DigitalToy_summary Summary

Implementation of a generic digital toy.
Implements IToy.



\subsection use_DirectOutput_Cab_Toys_Basic_DigitalToy_samplexml Sample XML

A configuration section for DigitalToy might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<DigitalToy>
  <Name>Name of DigitalToy</Name>
  <OutputName>Name of Output</OutputName>
</DigitalToy>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Basic_DigitalToy_properties Properties

DigitalToy has the following 2 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Basic_DigitalToy_OutputName OutputName

Name of the Output for the GenericDigitalToy



\subsubsection DirectOutput_Cab_Toys_Basic_DigitalToy_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



\section use_DirectOutput_Cab_Toys_Basic_ReplayKnocker ReplayKnocker

\subsection use_DirectOutput_Cab_Toys_Basic_ReplayKnocker_summary Summary

Replayknocker toy which can fire the replay knocker one or several times at given intervalls.



\subsection use_DirectOutput_Cab_Toys_Basic_ReplayKnocker_samplexml Sample XML

A configuration section for ReplayKnocker might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<ReplayKnocker>
  <Name>Name of ReplayKnocker</Name>
  <OutputName>Name of Output</OutputName>
  <DefaultIntervallMs>300</DefaultIntervallMs>
</ReplayKnocker>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Basic_ReplayKnocker_properties Properties

ReplayKnocker has the following 3 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Basic_ReplayKnocker_DefaultIntervallMs DefaultIntervallMs

Gets or sets the default intervall between knocks in milliseconds.<br />
Default value of this property is 300 milliseconds.



__Value__

The default intervall in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_ReplayKnocker_OutputName OutputName

Name of the Output for the GenericDigitalToy



\subsubsection DirectOutput_Cab_Toys_Basic_ReplayKnocker_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



\section use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy AnalogAlphaToy

\subsection use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_summary Summary

This toy handles analog values (0-255) in a layer structure including alpha value (0=completely transparent, 255=fully opaque) and outputs the belended result of the layers on a single output.



\subsection use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_properties Properties

AnalogAlphaToy has the following 2 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_OutputName OutputName

Gets or sets the name of the IOutput object of the toy.



__Value__

The name of the output.



\subsubsection DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



\section use_DirectOutput_Cab_Toys_Basic_AnalogToy AnalogToy

\subsection use_DirectOutput_Cab_Toys_Basic_AnalogToy_summary Summary

Implementation of a generic analog toy.
Implements IToy.



\subsection use_DirectOutput_Cab_Toys_Basic_AnalogToy_samplexml Sample XML

A configuration section for AnalogToy might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<AnalogToy>
  <Name>Name of AnalogToy</Name>
  <OutputName>Name of Output</OutputName>
</AnalogToy>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Basic_AnalogToy_properties Properties

AnalogToy has the following 2 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Basic_AnalogToy_OutputName OutputName

Name of the Output for the GenericAnalogToy.



\subsubsection DirectOutput_Cab_Toys_Basic_AnalogToy_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



\section use_DirectOutput_Cab_Toys_Basic_Motor Motor

\subsection use_DirectOutput_Cab_Toys_Basic_Motor_summary Summary

Motor toy supporting max. and min. power, max. runtime and kickstart settings.<br />
Inherits from GenericAnalogToy, implements IToy.



\subsection use_DirectOutput_Cab_Toys_Basic_Motor_samplexml Sample XML

A configuration section for Motor might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<Motor>
  <Name>Name of Motor</Name>
  <OutputName>Name of Output</OutputName>
  <MaxRunTimeMs>300000</MaxRunTimeMs>
  <MinPower>10</MinPower>
  <MaxPower>255</MaxPower>
  <KickstartPower>128</KickstartPower>
  <KickstartDurationMs>100</KickstartDurationMs>
</Motor>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Basic_Motor_properties Properties

Motor has the following 7 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Basic_Motor_MaxRunTimeMs MaxRunTimeMs

Gets or sets the max run time for the toy in milliseconds.<br />
Default value of this property is 30000 (5 minutes).<br />
Set value to 0 for infinite runtime.



__Value__

The max run time in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_Motor_MinPower MinPower

Gets or sets the minimal power for the toy.<br />
Motors beeing run with very low power might tend to stutter or block. Setting this property to a meaningfull value will ensure that motors are always properly running.<br />
Default value of this property is 10.



__Value__

The minimal power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_Motor_MaxPower MaxPower

Gets or sets the maximum power (e.g. to ensure that you cabinet is shaken into pieces by a powerfull shaker motor) for the motor controlled by the toy.<br />
Default value of the property is 255.



__Value__

The maximum power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_Motor_KickstartPower KickstartPower

Gets or sets the kickstart power for the motor.<br />
If motor are run with low power they might not start to rotate without some initial kickstart.
KickstartPower will only be applied if the motor is started with a power setting below the defined KickstartPower.<br />
Default value of this setting is 128.<br />
Set value to 0 to skip kickstart.



__Value__

The kickstart power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_Motor_KickstartDurationMs KickstartDurationMs

Gets or sets the kickstart duration (time during which the KickstartPower is applied) in milliseconds.<br />
Property defaults to 100 milliseconds.<br />
Set value to 0 to skip kickstart.



__Value__

The kickstart duration in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_Motor_OutputName OutputName

Name of the Output for the GenericAnalogToy.



\subsubsection DirectOutput_Cab_Toys_Basic_Motor_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



\section use_DirectOutput_Cab_Toys_Basic_Shaker Shaker

\subsection use_DirectOutput_Cab_Toys_Basic_Shaker_summary Summary

Shaker toy.<br />
This is just a simple wrapper around the motor toy.<br />
Inherits from GenericAnalogToy, implements IToy.



\subsection use_DirectOutput_Cab_Toys_Basic_Shaker_samplexml Sample XML

A configuration section for Shaker might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<Shaker>
  <Name>Name of Shaker</Name>
  <OutputName>Name of Output</OutputName>
  <MaxRunTimeMs>300000</MaxRunTimeMs>
  <MinPower>10</MinPower>
  <MaxPower>255</MaxPower>
  <KickstartPower>128</KickstartPower>
  <KickstartDurationMs>100</KickstartDurationMs>
</Shaker>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Basic_Shaker_properties Properties

Shaker has the following 7 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Basic_Shaker_MaxRunTimeMs MaxRunTimeMs

Gets or sets the max run time for the toy in milliseconds.<br />
Default value of this property is 30000 (5 minutes).<br />
Set value to 0 for infinite runtime.



__Value__

The max run time in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_Shaker_MinPower MinPower

Gets or sets the minimal power for the toy.<br />
Motors beeing run with very low power might tend to stutter or block. Setting this property to a meaningfull value will ensure that motors are always properly running.<br />
Default value of this property is 10.



__Value__

The minimal power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_Shaker_MaxPower MaxPower

Gets or sets the maximum power (e.g. to ensure that you cabinet is shaken into pieces by a powerfull shaker motor) for the motor controlled by the toy.<br />
Default value of the property is 255.



__Value__

The maximum power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_Shaker_KickstartPower KickstartPower

Gets or sets the kickstart power for the motor.<br />
If motor are run with low power they might not start to rotate without some initial kickstart.
KickstartPower will only be applied if the motor is started with a power setting below the defined KickstartPower.<br />
Default value of this setting is 128.<br />
Set value to 0 to skip kickstart.



__Value__

The kickstart power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_Shaker_KickstartDurationMs KickstartDurationMs

Gets or sets the kickstart duration (time during which the KickstartPower is applied) in milliseconds.<br />
Property defaults to 100 milliseconds.<br />
Set value to 0 to skip kickstart.



__Value__

The kickstart duration in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_Shaker_OutputName OutputName

Name of the Output for the GenericAnalogToy.



\subsubsection DirectOutput_Cab_Toys_Basic_Shaker_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



\section use_DirectOutput_Cab_Toys_Basic_Flasher Flasher

\subsection use_DirectOutput_Cab_Toys_Basic_Flasher_summary Summary

The Flasher toy fires one or several short pluses/flashes on the configured IOutput at given intervalls.



\subsection use_DirectOutput_Cab_Toys_Basic_Flasher_samplexml Sample XML

A configuration section for Flasher might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<Flasher>
  <Name>Name of Flasher</Name>
  <OutputName>Name of Output</OutputName>
  <DefaultIntervallMs>150</DefaultIntervallMs>
  <FlashDurationMs>20</FlashDurationMs>
</Flasher>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Basic_Flasher_properties Properties

Flasher has the following 4 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Basic_Flasher_DefaultIntervallMs DefaultIntervallMs

Gets or sets the default intervall between flashes in milliseconds.<br />
Default value of this property is 150 milliseconds.



__Value__

The default intervall in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_Flasher_FlashDurationMs FlashDurationMs

Gets or sets the flash duration in milliseconds.<br />
Default value of this property is set to 20ms or the value of Pinball.UpdateTimer.IntervalMs if this value is above 20 milliseconds.



__Value__

The flash duration in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_Flasher_OutputName OutputName

Name of the Output for the GenericDigitalToy



\subsubsection DirectOutput_Cab_Toys_Basic_Flasher_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



\section use_DirectOutput_Cab_Toys_Basic_GearMotor GearMotor

\subsection use_DirectOutput_Cab_Toys_Basic_GearMotor_summary Summary

GearMotor toy is just a simple wrapper around the Motor toy.



\subsection use_DirectOutput_Cab_Toys_Basic_GearMotor_samplexml Sample XML

A configuration section for GearMotor might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<GearMotor>
  <Name>Name of GearMotor</Name>
  <OutputName>Name of Output</OutputName>
  <MaxRunTimeMs>300000</MaxRunTimeMs>
  <MinPower>10</MinPower>
  <MaxPower>255</MaxPower>
  <KickstartPower>128</KickstartPower>
  <KickstartDurationMs>100</KickstartDurationMs>
</GearMotor>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Basic_GearMotor_properties Properties

GearMotor has the following 7 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Basic_GearMotor_MaxRunTimeMs MaxRunTimeMs

Gets or sets the max run time for the toy in milliseconds.<br />
Default value of this property is 30000 (5 minutes).<br />
Set value to 0 for infinite runtime.



__Value__

The max run time in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_GearMotor_MinPower MinPower

Gets or sets the minimal power for the toy.<br />
Motors beeing run with very low power might tend to stutter or block. Setting this property to a meaningfull value will ensure that motors are always properly running.<br />
Default value of this property is 10.



__Value__

The minimal power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_GearMotor_MaxPower MaxPower

Gets or sets the maximum power (e.g. to ensure that you cabinet is shaken into pieces by a powerfull shaker motor) for the motor controlled by the toy.<br />
Default value of the property is 255.



__Value__

The maximum power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_GearMotor_KickstartPower KickstartPower

Gets or sets the kickstart power for the motor.<br />
If motor are run with low power they might not start to rotate without some initial kickstart.
KickstartPower will only be applied if the motor is started with a power setting below the defined KickstartPower.<br />
Default value of this setting is 128.<br />
Set value to 0 to skip kickstart.



__Value__

The kickstart power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_GearMotor_KickstartDurationMs KickstartDurationMs

Gets or sets the kickstart duration (time during which the KickstartPower is applied) in milliseconds.<br />
Property defaults to 100 milliseconds.<br />
Set value to 0 to skip kickstart.



__Value__

The kickstart duration in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_GearMotor_OutputName OutputName

Name of the Output for the GenericAnalogToy.



\subsubsection DirectOutput_Cab_Toys_Basic_GearMotor_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



\section use_DirectOutput_Cab_Toys_Layer_RGBAToy RGBAToy

\subsection use_DirectOutput_Cab_Toys_Layer_RGBAToy_properties Properties

RGBAToy has the following 4 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Layer_RGBAToy_OutputNameRed OutputNameRed

Name of the IOutput for red.



\subsubsection DirectOutput_Cab_Toys_Layer_RGBAToy_OutputNameGreen OutputNameGreen

Name of the IOutput for green.



\subsubsection DirectOutput_Cab_Toys_Layer_RGBAToy_OutputNameBlue OutputNameBlue

Name of the IOutput for blue.



\subsubsection DirectOutput_Cab_Toys_Layer_RGBAToy_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



\section use_DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent LedWizEquivalent

\subsection use_DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_summary Summary

The LEDWizEquivalent toy provides a Ledwiz like interface to 32 outputs.<br />
The outputs listes in the Outputs property can point to any IOutput in the Cabinet.<br />
This toy is also used when legacy LedCOntrol.ini files are used to configure the framework.



\subsection use_DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_samplexml Sample XML

A configuration section for LedWizEquivalent might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<LedWizEquivalent>
  <Name>Name of LedWizEquivalent</Name>
  <Outputs>
    <LedWizEquivalentOutput>
      <OutputName>Name of Output</OutputName>
      <LedWizEquivalentOutputNumber>0</LedWizEquivalentOutputNumber>
    </LedWizEquivalentOutput>
    <LedWizEquivalentOutput>
      <OutputName>Name of Output</OutputName>
      <LedWizEquivalentOutputNumber>0</LedWizEquivalentOutputNumber>
    </LedWizEquivalentOutput>
    <LedWizEquivalentOutput>
      <OutputName>Name of Output</OutputName>
      <LedWizEquivalentOutputNumber>0</LedWizEquivalentOutputNumber>
    </LedWizEquivalentOutput>
  </Outputs>
  <LedWizNumber>-1</LedWizNumber>
</LedWizEquivalent>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_properties Properties

LedWizEquivalent has the following 3 configurable properties:

\subsubsection DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_Outputs Outputs

Gets or sets the outputs of the LedWizEquivalent toy.



__Value__

The outputs of the LedWizEquivalent toy.



__Nested Properties__

The following nested propteries exist for Outputs:
* __OutputName__
* __LedWizEquivalentOutputNumber__

\subsubsection DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_LedWizNumber LedWizNumber

Gets or sets the number of the virtual LedWiz emulated by the LedWizEquivalentToy.



__Value__

The number of the virtual LedWiz emulated by the LedWizEquivalentToy.



\subsubsection DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



\section use_DirectOutput_Cab_Toys_Basic_Contactor Contactor

\subsection use_DirectOutput_Cab_Toys_Basic_Contactor_summary Summary

Contactor toy.
Basicaly just a more descriptive name for a GenericDigitalToy.
Implements IToy, inherits GenericDigitalToy



\subsection use_DirectOutput_Cab_Toys_Basic_Contactor_samplexml Sample XML

A configuration section for Contactor might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<Contactor>
  <Name>Name of Contactor</Name>
  <OutputName>Name of Output</OutputName>
</Contactor>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Basic_Contactor_properties Properties

Contactor has the following 2 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Basic_Contactor_OutputName OutputName

Name of the Output for the GenericDigitalToy



\subsubsection DirectOutput_Cab_Toys_Basic_Contactor_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



\section use_DirectOutput_Cab_Toys_Basic_Lamp Lamp

\subsection use_DirectOutput_Cab_Toys_Basic_Lamp_summary Summary

Lamp toy.<br />
Inherits from <see cref="T:DirectOutput.Cab.Toys.Basic.AnalogToy" />, implements <see cref="T:DirectOutput.Cab.Toys.IToy" />.



\subsection use_DirectOutput_Cab_Toys_Basic_Lamp_samplexml Sample XML

A configuration section for Lamp might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<Lamp>
  <Name>Name of Lamp</Name>
  <OutputName>Name of Output</OutputName>
</Lamp>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Basic_Lamp_properties Properties

Lamp has the following 2 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Basic_Lamp_OutputName OutputName

Name of the Output for the GenericAnalogToy.



\subsubsection DirectOutput_Cab_Toys_Basic_Lamp_Name Name

Name of the Named item.<br />
Triggers BeforeNameChange before a new Name is set.<br />
Triggers AfterNameChanged after a new name has been set.



