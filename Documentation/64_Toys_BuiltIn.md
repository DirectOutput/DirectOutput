Built in Toys  {#toy_builtin}
==========
\section use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy AnalogAlphaToy

\subsection use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_summary Summary

This toy handles analog values (0-255) in a layer structure including alpha value (0=completely transparent, 255=fully opaque) and outputs the belended result of the layers on a single output.



\subsection use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_samplexml Sample XML

A configuration section for AnalogAlphaToy might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<AnalogAlphaToy>
  <Name>Name of AnalogAlphaToy</Name>
  <OutputName>Name of Output</OutputName>
  <FadingCurveName>Linear</FadingCurveName>
</AnalogAlphaToy>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_properties Properties

AnalogAlphaToy has the following 3 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_FadingCurveName FadingCurveName

The name of the fading curve.



\subsubsection DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_OutputName OutputName

The name of the output.



\section use_DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup AnalogAlphaToyGroup

\subsection use_DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup_summary Summary

This toys allows the grouping of several AnalogAlpha toys (e.g. <see cref="T:DirectOutput.Cab.Toys.Layer.AnalogAlphaToy" /> or <see cref="T:DirectOutput.Cab.Toys.Hardware.Lamp" />) into a matrix, which can be controlled by the matrix effects.

\note Be sure to define this toy in the config file before the toys, which are listed in the ToyNames array.



\subsection use_DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup_samplexml Sample XML

A configuration section for AnalogAlphaToyGroup might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<AnalogAlphaToyGroup>
  <Name>Name of AnalogAlphaToyGroup</Name>
  <ToyNames>
    <Row>
      <Column>Item</Column>
      <Column>Item</Column>
    </Row>
    <Row>
      <Column>Item</Column>
      <Column>Item</Column>
    </Row>
    <Row>
      <Column>Item</Column>
      <Column>Item</Column>
    </Row>
  </ToyNames>
  <LayerOffset>0</LayerOffset>
</AnalogAlphaToyGroup>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup_properties Properties

AnalogAlphaToyGroup has the following 3 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup_LayerOffset LayerOffset

The layer offset which defines a fixed positive or negative offset to the layers which are controlled on the target toy..



\subsubsection DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup_ToyNames ToyNames

The 2 dimensional array of RGBA toy names.



__Nested Properties__

The following nested propteries exist for ToyNames:
* __Capacity__<br/>* __Item__<br/>
\section use_DirectOutput_Cab_Toys_Hardware_GearMotor GearMotor

\subsection use_DirectOutput_Cab_Toys_Hardware_GearMotor_summary Summary

GearMotor toy supporting max. and min. power, max. runtime and kickstart settings.<br />
The settings of this toy allow for a detailed definition of the behaviour of the connected motor.

* The KickstartPower and the KickstartDurationMs property allow you to define a initial stronger impulse for the start of the motor. Some hardware, e.g. like the Wolfsoft shaker in my cab, do not start to rotate when they are started with low power, giving them a initial pulse and the reducing to the lower power on the other hand is no problem. These properties allow you to define this behaviour.
* The MinPower and MaxPower properties allow you to define the range of values which are allowed for the motor power. As a example, the shaker in my cab doesn't rotate with powers lower than 50 and starts to rattle and shake my cab appart with powers above 100. The normal value range for motor powers (0-255) is scaled to the range defined by the MinPower and MaxPower properties, so a value of 1 will result the shaker to operate on MinPower and 255 will result in MaxPower.
* The FadingCurveName property can be used to specify the name of a predefined or user defined fading curve. This allows for even better fine tuning of the shaker power for different input values. It is recommended to use either the fading curve __Linear__ (default) and the MinPower/MaxPower settings or to use a fading curve and keep the MinPower/MaxPower settings on their defaults. If MinPower/MaxPower and a Fading Curve are combined, the power value for the motor will first be adjusted by the fading curve and the be scaled into the range of the MinPower/MaxPower settings.
* MaxRunTimeMs allows you to define a maximum runtime for the motor before it is automatically turned off. To get reactivated after a runtime timeout the motor toy must first receive a power off (value=0), before it is reactivated.



\subsection use_DirectOutput_Cab_Toys_Hardware_GearMotor_samplexml Sample XML

A configuration section for GearMotor might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<GearMotor>
  <Name>Name of GearMotor</Name>
  <OutputName>Name of Output</OutputName>
  <FadingCurveName>Linear</FadingCurveName>
  <MaxRunTimeMs>300000</MaxRunTimeMs>
  <KickstartPower>255</KickstartPower>
  <KickstartDurationMs>100</KickstartDurationMs>
  <MinPower>1</MinPower>
  <MaxPower>255</MaxPower>
</GearMotor>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Hardware_GearMotor_properties Properties

GearMotor has the following 8 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Hardware_GearMotor_FadingCurveName FadingCurveName

The name of the fading curve.



\subsubsection DirectOutput_Cab_Toys_Hardware_GearMotor_KickstartDurationMs KickstartDurationMs

The kickstart duration in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Hardware_GearMotor_KickstartPower KickstartPower

The kickstart power for the motor.



\subsubsection DirectOutput_Cab_Toys_Hardware_GearMotor_MaxPower MaxPower

The max power for the toy.



\subsubsection DirectOutput_Cab_Toys_Hardware_GearMotor_MaxRunTimeMs MaxRunTimeMs

The max run time in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Hardware_GearMotor_MinPower MinPower

The minimal power for the toy. This property can be used to ensure that the motor will allways rotate when it is active and not getting stuck due to low power.



\subsubsection DirectOutput_Cab_Toys_Hardware_GearMotor_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Toys_Hardware_GearMotor_OutputName OutputName

The name of the output.



\section use_DirectOutput_Cab_Toys_Hardware_Lamp Lamp

\subsection use_DirectOutput_Cab_Toys_Hardware_Lamp_summary Summary

Lamp toy.<br />
Just a simple wrapper for the <see cref="T:DirectOutput.Cab.Toys.Layer.AnalogAlphaToy" />.



\subsection use_DirectOutput_Cab_Toys_Hardware_Lamp_samplexml Sample XML

A configuration section for Lamp might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<Lamp>
  <Name>Name of Lamp</Name>
  <OutputName>Name of Output</OutputName>
  <FadingCurveName>Linear</FadingCurveName>
</Lamp>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Hardware_Lamp_properties Properties

Lamp has the following 3 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Hardware_Lamp_FadingCurveName FadingCurveName

The name of the fading curve.



\subsubsection DirectOutput_Cab_Toys_Hardware_Lamp_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Toys_Hardware_Lamp_OutputName OutputName

The name of the output.



\section use_DirectOutput_Cab_Toys_Hardware_LedStrip LedStrip

\subsection use_DirectOutput_Cab_Toys_Hardware_LedStrip_summary Summary

Represents a adressable led strip.

The toy supports several layers and supports transparency/alpha channels for every single led.



\subsection use_DirectOutput_Cab_Toys_Hardware_LedStrip_samplexml Sample XML

A configuration section for LedStrip might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<LedStrip>
  <Name>Name of LedStrip</Name>
  <Width>1</Width>
  <Height>1</Height>
  <LedStripArrangement>LeftRightTopDown</LedStripArrangement>
  <ColorOrder>RBG</ColorOrder>
  <FirstLedNumber>1</FirstLedNumber>
  <FadingCurveName>Linear</FadingCurveName>
  <BrightnessGammaCorrection>2.2</BrightnessGammaCorrection>
  <Brightness>100</Brightness>
  <OutputControllerName>Name of OutputController</OutputControllerName>
</LedStrip>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Hardware_LedStrip_properties Properties

LedStrip has the following 10 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Hardware_LedStrip_Brightness Brightness

\subsubsection DirectOutput_Cab_Toys_Hardware_LedStrip_BrightnessGammaCorrection BrightnessGammaCorrection

\subsubsection DirectOutput_Cab_Toys_Hardware_LedStrip_ColorOrder ColorOrder

The color order of the leds on the strip.



The property ColorOrder accepts the following values:

* __RGB__: Red-Green-Blue (usual color order)
* __RBG__: Red - Blue - Green
* __GRB__: Green - Red - Blue (WS2812 led chips are using the scheme)
* __WS2812__: WS2812 led chip (uses green - red - blue color order)
* __GBR__: Green - Blue - Red
* __BRG__: Green - Blue - Red
* __BGR__: Blue - Green - Red


\subsubsection DirectOutput_Cab_Toys_Hardware_LedStrip_FadingCurveName FadingCurveName

The name of the fading curve.



\subsubsection DirectOutput_Cab_Toys_Hardware_LedStrip_FirstLedNumber FirstLedNumber

The number of the first led of the strip.



\subsubsection DirectOutput_Cab_Toys_Hardware_LedStrip_Height Height

The height of the led stripe.



\subsubsection DirectOutput_Cab_Toys_Hardware_LedStrip_LedStripArrangement LedStripArrangement

The strip arrangement value as defined in the LedStripArrangementEnum.
 \image html LedStripArrangementEnum.jpg Supported led string arrangements



The property LedStripArrangement accepts the following values:

* __LeftRightTopDown__
* __LeftRightBottomUp__
* __RightLeftTopDown__
* __RightLeftBottomUp__
* __TopDownLeftRight__
* __TopDownRightLeft__
* __BottomUpLeftRight__
* __BottomUpRightLeft__
* __LeftRightAlternateTopDown__
* __LeftRightAlternateBottomUp__
* __RightLeftAlternateTopDown__
* __RightLeftAlternateBottomUp__
* __TopDownAlternateLeftRight__
* __TopDownAlternateRightLeft__
* __BottomUpAlternateLeftRight__
* __BottomUpAlternateRightLeft__


\subsubsection DirectOutput_Cab_Toys_Hardware_LedStrip_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Toys_Hardware_LedStrip_OutputControllerName OutputControllerName

The name of the output controller.



\subsubsection DirectOutput_Cab_Toys_Hardware_LedStrip_Width Width

The width of the led stripe.



\section use_DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent LedWizEquivalent

\subsection use_DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_summary Summary

The LEDWizEquivalent toy is only used by the framework when ini files are used for the configuration to determine which outputs should be controled by the columns in the ini files.<br />
Do not target this type of toy with any effects.



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

\subsubsection DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_LedWizNumber LedWizNumber

The number of the virtual LedWiz resp. ini file to be matched with the LedWizEquivalentToy.



\subsubsection DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_Outputs Outputs

The outputs of the LedWizEquivalent toy.



__Nested Properties__

The following nested propteries exist for Outputs:
* __LedWizEquivalentOutputNumber__<br/>  The number of the LedWizEquivalentOutput.



* __OutputName__<br/>  The name of the IOutput object beeing controlled by the LedWizEquivalenOutput.




\section use_DirectOutput_Cab_Toys_Hardware_Motor Motor

\subsection use_DirectOutput_Cab_Toys_Hardware_Motor_summary Summary

Motor toy supporting max. and min. power, max. runtime and kickstart settings.<br />
The settings of this toy allow for a detailed definition of the behaviour of the connected motor.

* The KickstartPower and the KickstartDurationMs property allow you to define a initial stronger impulse for the start of the motor. Some hardware, e.g. like the Wolfsoft shaker in my cab, do not start to rotate when they are started with low power, giving them a initial pulse and the reducing to the lower power on the other hand is no problem. These properties allow you to define this behaviour.
* The MinPower and MaxPower properties allow you to define the range of values which are allowed for the motor power. As a example, the shaker in my cab doesn't rotate with powers lower than 50 and starts to rattle and shake my cab appart with powers above 100. The normal value range for motor powers (0-255) is scaled to the range defined by the MinPower and MaxPower properties, so a value of 1 will result the shaker to operate on MinPower and 255 will result in MaxPower.
* The FadingCurveName property can be used to specify the name of a predefined or user defined fading curve. This allows for even better fine tuning of the shaker power for different input values. It is recommended to use either the fading curve __Linear__ (default) and the MinPower/MaxPower settings or to use a fading curve and keep the MinPower/MaxPower settings on their defaults. If MinPower/MaxPower and a Fading Curve are combined, the power value for the motor will first be adjusted by the fading curve and the be scaled into the range of the MinPower/MaxPower settings.
* MaxRunTimeMs allows you to define a maximum runtime for the motor before it is automatically turned off. To get reactivated after a runtime timeout the motor toy must first receive a power off (value=0), before it is reactivated.



\subsection use_DirectOutput_Cab_Toys_Hardware_Motor_samplexml Sample XML

A configuration section for Motor might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<Motor>
  <Name>Name of Motor</Name>
  <OutputName>Name of Output</OutputName>
  <FadingCurveName>Linear</FadingCurveName>
  <MaxRunTimeMs>300000</MaxRunTimeMs>
  <KickstartPower>255</KickstartPower>
  <KickstartDurationMs>100</KickstartDurationMs>
  <MinPower>1</MinPower>
  <MaxPower>255</MaxPower>
</Motor>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Hardware_Motor_properties Properties

Motor has the following 8 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Hardware_Motor_FadingCurveName FadingCurveName

The name of the fading curve.



\subsubsection DirectOutput_Cab_Toys_Hardware_Motor_KickstartDurationMs KickstartDurationMs

The kickstart duration in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Hardware_Motor_KickstartPower KickstartPower

The kickstart power for the motor.



\subsubsection DirectOutput_Cab_Toys_Hardware_Motor_MaxPower MaxPower

The max power for the toy.



\subsubsection DirectOutput_Cab_Toys_Hardware_Motor_MaxRunTimeMs MaxRunTimeMs

The max run time in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Hardware_Motor_MinPower MinPower

The minimal power for the toy. This property can be used to ensure that the motor will allways rotate when it is active and not getting stuck due to low power.



\subsubsection DirectOutput_Cab_Toys_Hardware_Motor_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Toys_Hardware_Motor_OutputName OutputName

The name of the output.



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
  <FadingCurveName>Linear</FadingCurveName>
</RGBAToy>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Layer_RGBAToy_properties Properties

RGBAToy has the following 5 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Layer_RGBAToy_FadingCurveName FadingCurveName

The name of the fading curve.



\subsubsection DirectOutput_Cab_Toys_Layer_RGBAToy_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Toys_Layer_RGBAToy_OutputNameBlue OutputNameBlue

Name of the IOutput for blue.



\subsubsection DirectOutput_Cab_Toys_Layer_RGBAToy_OutputNameGreen OutputNameGreen

Name of the IOutput for green.



\subsubsection DirectOutput_Cab_Toys_Layer_RGBAToy_OutputNameRed OutputNameRed

Name of the IOutput for red.



\section use_DirectOutput_Cab_Toys_Virtual_RGBAToyGroup RGBAToyGroup

\subsection use_DirectOutput_Cab_Toys_Virtual_RGBAToyGroup_summary Summary

This toys allows the grouping of several RGBA toys (e.g. <see cref="T:DirectOutput.Cab.Toys.Layer.RGBAToy" /> or <see cref="T:DirectOutput.Cab.Toys.Hardware.RGBLed" />) into a matrix, which can be controlled by the matrix effects.

\note Be sure to define this toy in the config file before the toys, which are listed in the ToyNames array.



\subsection use_DirectOutput_Cab_Toys_Virtual_RGBAToyGroup_samplexml Sample XML

A configuration section for RGBAToyGroup might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAToyGroup>
  <Name>Name of RGBAToyGroup</Name>
  <ToyNames>
    <Row>
      <Column>Item</Column>
      <Column>Item</Column>
    </Row>
    <Row>
      <Column>Item</Column>
      <Column>Item</Column>
    </Row>
    <Row>
      <Column>Item</Column>
      <Column>Item</Column>
    </Row>
  </ToyNames>
  <LayerOffset>0</LayerOffset>
</RGBAToyGroup>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Virtual_RGBAToyGroup_properties Properties

RGBAToyGroup has the following 3 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Virtual_RGBAToyGroup_LayerOffset LayerOffset

The layer offset which defines a fixed positive or negative offset to the layers which are controlled on the target toy..



\subsubsection DirectOutput_Cab_Toys_Virtual_RGBAToyGroup_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Toys_Virtual_RGBAToyGroup_ToyNames ToyNames

The 2 dimensional array of RGBA toy names.



__Nested Properties__

The following nested propteries exist for ToyNames:
* __Capacity__<br/>* __Item__<br/>
\section use_DirectOutput_Cab_Toys_Hardware_RGBLed RGBLed

\subsection use_DirectOutput_Cab_Toys_Hardware_RGBLed_summary Summary

Toy controlling a RGB led.<br />
This is just a simple wrapper around the <see cref="T:DirectOutput.Cab.Toys.Layer.RGBAToy" />.<br />


\subsection use_DirectOutput_Cab_Toys_Hardware_RGBLed_samplexml Sample XML

A configuration section for RGBLed might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBLed>
  <Name>Name of RGBLed</Name>
  <OutputNameRed>OutputNameRed string</OutputNameRed>
  <OutputNameGreen>OutputNameGreen string</OutputNameGreen>
  <OutputNameBlue>OutputNameBlue string</OutputNameBlue>
  <FadingCurveName>Linear</FadingCurveName>
</RGBLed>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Hardware_RGBLed_properties Properties

RGBLed has the following 5 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Hardware_RGBLed_FadingCurveName FadingCurveName

The name of the fading curve.



\subsubsection DirectOutput_Cab_Toys_Hardware_RGBLed_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Toys_Hardware_RGBLed_OutputNameBlue OutputNameBlue

Name of the IOutput for blue.



\subsubsection DirectOutput_Cab_Toys_Hardware_RGBLed_OutputNameGreen OutputNameGreen

Name of the IOutput for green.



\subsubsection DirectOutput_Cab_Toys_Hardware_RGBLed_OutputNameRed OutputNameRed

Name of the IOutput for red.



\section use_DirectOutput_Cab_Toys_Hardware_Shaker Shaker

\subsection use_DirectOutput_Cab_Toys_Hardware_Shaker_summary Summary

Shaker toy with settings for minimum and maximum power, fading curves, max runtime and kickstart.

\image html Shaker.jpg

The shaker toy is a simple wrapper around the Motor toy, so you can define your toy with the precise type.

The settings of this toy allow for a detailed definition of the behaviour of the connected motor.

* The KickstartPower and the KickstartDurationMs property allow you to define a initial stronger impulse for the start of the motor. Some hardware, e.g. like the Wolfsoft shaker in my cab, do not start to rotate when they are started with low power, giving them a initial pulse and the reducing to the lower power on the other hand is no problem. These properties allow you to define this behaviour.
* The MinPower and MaxPower properties allow you to define the range of values which are allowed for the motor power. As a example, the shaker in my cab doesn't rotate with powers lower than 50 and starts to rattle and shake my cab appart with powers above 100. The normal value range for motor powers (0-255) is scaled to the range defined by the MinPower and MaxPower properties, so a value of 1 will result the shaker to operate on MinPower and 255 will result in MaxPower.
* The FadingCurveName property can be used to specify the name of a predefined or user defined fading curve. This allows for even better fine tuning of the shaker power for different input values. It is recommended to use either the fading curve __Linear__ (default) and the MinPower/MaxPower settings or to use a fading curve and keep the MinPower/MaxPower settings on their defaults. If MinPower/MaxPower and a Fading Curve are combined, the power value for the motor will first be adjusted by the fading curve and the be scaled into the range of the MinPower/MaxPower settings.
* MaxRunTimeMs allows you to define a maximum runtime for the motor before it is automatically turned off. To get reactivated after a runtime timeout the motor toy must first receive a power off (value=0), before it is reactivated.



\subsection use_DirectOutput_Cab_Toys_Hardware_Shaker_samplexml Sample XML

A configuration section for Shaker might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<Shaker>
  <Name>Name of Shaker</Name>
  <OutputName>Name of Output</OutputName>
  <FadingCurveName>Linear</FadingCurveName>
  <MaxRunTimeMs>300000</MaxRunTimeMs>
  <KickstartPower>255</KickstartPower>
  <KickstartDurationMs>100</KickstartDurationMs>
  <MinPower>1</MinPower>
  <MaxPower>255</MaxPower>
</Shaker>
~~~~~~~~~~~~~
\subsection use_DirectOutput_Cab_Toys_Hardware_Shaker_properties Properties

Shaker has the following 8 configurable properties:

\subsubsection DirectOutput_Cab_Toys_Hardware_Shaker_FadingCurveName FadingCurveName

The name of the fading curve.



\subsubsection DirectOutput_Cab_Toys_Hardware_Shaker_KickstartDurationMs KickstartDurationMs

The kickstart duration in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Hardware_Shaker_KickstartPower KickstartPower

The kickstart power for the motor.



\subsubsection DirectOutput_Cab_Toys_Hardware_Shaker_MaxPower MaxPower

The max power for the toy.



\subsubsection DirectOutput_Cab_Toys_Hardware_Shaker_MaxRunTimeMs MaxRunTimeMs

The max run time in milliseconds.



\subsubsection DirectOutput_Cab_Toys_Hardware_Shaker_MinPower MinPower

The minimal power for the toy. This property can be used to ensure that the motor will allways rotate when it is active and not getting stuck due to low power.



\subsubsection DirectOutput_Cab_Toys_Hardware_Shaker_Name Name

The name of the item.



\subsubsection DirectOutput_Cab_Toys_Hardware_Shaker_OutputName OutputName

The name of the output.



