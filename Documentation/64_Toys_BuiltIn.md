Built in Toys  {#toy_builtin}
==========
\section use_DirectOutput_Cab_Toys_Basic_AnalogToy AnalogToy

\subsection use_DirectOutput_Cab_Toys_Basic_AnalogToy_summary Summary

\deprecated The use of this toy is depreceated. Use the new AnalogAlphaToy instead.
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

The name of the item.



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

The name of the item.



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

The name of the item.



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

The max run time in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_GearMotor_MinPower MinPower

The minimal power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_GearMotor_MaxPower MaxPower

The maximum power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_GearMotor_KickstartPower KickstartPower

The kickstart power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_GearMotor_KickstartDurationMs KickstartDurationMs

The kickstart duration in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_GearMotor_OutputName OutputName

Name of the Output for the GenericAnalogToy.



\subsubsection DirectOutput_Cab_Toys_Basic_GearMotor_Name Name

The name of the item.



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

The name of the item.



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

The max run time in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_Motor_MinPower MinPower

The minimal power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_Motor_MaxPower MaxPower

The maximum power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_Motor_KickstartPower KickstartPower

The kickstart power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_Motor_KickstartDurationMs KickstartDurationMs

The kickstart duration in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_Motor_OutputName OutputName

Name of the Output for the GenericAnalogToy.



\subsubsection DirectOutput_Cab_Toys_Basic_Motor_Name Name

The name of the item.



\section use_DirectOutput_Cab_Toys_Basic_RGBLed RGBLed

\subsection use_DirectOutput_Cab_Toys_Basic_RGBLed_summary Summary

\deprecated The use of this toy is depreceated. Use the new RGBAToy instead.
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

The name of the item.



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

The max run time in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_Shaker_MinPower MinPower

The minimal power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_Shaker_MaxPower MaxPower

The maximum power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_Shaker_KickstartPower KickstartPower

The kickstart power for the motor.



\subsubsection DirectOutput_Cab_Toys_Basic_Shaker_KickstartDurationMs KickstartDurationMs

The kickstart duration in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Basic_Shaker_OutputName OutputName

Name of the Output for the GenericAnalogToy.



\subsubsection DirectOutput_Cab_Toys_Basic_Shaker_Name Name

The name of the item.



\section use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy AnalogAlphaToy

\subsection use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_summary Summary

This toy handles analog values (0-255) in a layer structure including alpha value (0=completely transparent, 255=fully opaque) and outputs the belended result of the layers on a single output.



\subsection use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_samplexml Sample XML

A configuration section for AnalogAlphaToy might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<AnalogAlphaToy>
  <Name>Name of AnalogAlphaToy</Name>
  <OutputName>Name of Output</OutputName>
</AnalogAlphaToy>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_properties Properties

AnalogAlphaToy has the following 2 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_OutputName OutputName

The name of the output.



\subsubsection DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_Name Name

The name of the item.



\section use_DirectOutput_Cab_Toys_Layer_RGBAToy RGBAToy

\subsection use_DirectOutput_Cab_Toys_Layer_RGBAToy_summary Summary

Thie RGBAToy controls RGB leds and other gadgets displaying RGB colors.<br /><br />
The RGBAToy has multilayer support with alpha channels. This allows the effects targeting RGBAToys to send their data to different layers.
Values in a layer do also have a alpha/transparency channel which will allow us to blend the colors/values in the various layers (e.g. if  a bottom layer is blue and top is a semi transparent red, you will get some mix of both or if one of the two blinks you get changing colors).<br />
The following picture might give you a clearer idea how the layers with their alpha channels work:

\image html LayersRGBA.png "RGBA Layers"



\subsection use_DirectOutput_Cab_Toys_Layer_RGBAToy_samplexml Sample XML

A configuration section for RGBAToy might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAToy>
  <Name>Name of RGBAToy</Name>
  <OutputNameRed>OutputNameRed string</OutputNameRed>
  <OutputNameGreen>OutputNameGreen string</OutputNameGreen>
  <OutputNameBlue>OutputNameBlue string</OutputNameBlue>
</RGBAToy>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Layer_RGBAToy_properties Properties

RGBAToy has the following 4 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Layer_RGBAToy_OutputNameRed OutputNameRed

Name of the IOutput for red.



\subsubsection DirectOutput_Cab_Toys_Layer_RGBAToy_OutputNameGreen OutputNameGreen

Name of the IOutput for green.



\subsubsection DirectOutput_Cab_Toys_Layer_RGBAToy_OutputNameBlue OutputNameBlue

Name of the IOutput for blue.



\subsubsection DirectOutput_Cab_Toys_Layer_RGBAToy_Name Name

The name of the item.



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

The outputs of the LedWizEquivalent toy.



__Nested Properties__

The following nested propteries exist for Outputs:
* __OutputName__<br/>  The name of the IOutput object beeing controlled by the LedWizEquivalenOutput.



* __LedWizEquivalentOutputNumber__<br/>  The number of the LedWizEquivalentOutput.




\subsubsection DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_LedWizNumber LedWizNumber

The number of the virtual LedWiz emulated by the LedWizEquivalentToy.



\subsubsection DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_Name Name

The name of the item.



