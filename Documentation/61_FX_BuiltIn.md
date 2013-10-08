Built in Effects  {#fx_builtin}
==========
\section use_DirectOutput_FX_AnalogToyFX_AnalogToyFadeOnOffEffect AnalogToyFadeOnOffEffect

\subsection use_DirectOutput_FX_AnalogToyFX_AnalogToyFadeOnOffEffect_summary Summary

A effect fading the output value of a AnalogToy object to a active or inactive value. The fading is controlled by the value property (0, not 0 or null) of the TableElementData parameter of the Trigger method.
\image html FX_FadeOnOff.png "FadeOnOff effect"



\subsection use_DirectOutput_FX_AnalogToyFX_AnalogToyFadeOnOffEffect_samplexml Sample XML

A configuration section for AnalogToyFadeOnOffEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<AnalogToyFadeOnOffEffect>
  <Name>Name of AnalogToyFadeOnOffEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Layer>0</Layer>
  <RetriggerBehaviour>RestartEffect</RetriggerBehaviour>
  <FadeMode>DefinedToDefined</FadeMode>
  <FadeInactiveDurationMs>500</FadeInactiveDurationMs>
  <FadeActiveDurationMs>500</FadeActiveDurationMs>
  <ActiveValue>
    <Value>0</Value>
    <Alpha>0</Alpha>
  </ActiveValue>
  <InactiveValue>
    <Value>0</Value>
    <Alpha>0</Alpha>
  </InactiveValue>
</AnalogToyFadeOnOffEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_AnalogToyFX_AnalogToyFadeOnOffEffect_properties Properties

AnalogToyFadeOnOffEffect has the following 9 configurable properties:

\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyFadeOnOffEffect_RetriggerBehaviour RetriggerBehaviour

Valid values are RestartEffect or IgnoreRetrigger.



The property RetriggerBehaviour accepts the following values:

RestartEffect
* IgnoreRetrigger

__Valid values__

The property RetriggerBehaviour accepts the following values:

RestartEffect
* IgnoreRetrigger
\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyFadeOnOffEffect_FadeMode FadeMode

CurrentToDefinedColor or DefinedColor



The property FadeMode accepts the following values:

CurrentToDefined
* DefinedToDefined

__Valid values__

The property FadeMode accepts the following values:

CurrentToDefined
* DefinedToDefined
\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyFadeOnOffEffect_FadeInactiveDurationMs FadeInactiveDurationMs

The fading duration in milliseconds.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyFadeOnOffEffect_FadeActiveDurationMs FadeActiveDurationMs

The fading duration in milliseconds.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyFadeOnOffEffect_ActiveValue ActiveValue

The active value between 0 and 255.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyFadeOnOffEffect_InactiveValue InactiveValue

The inactive value between 0 and 255.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyFadeOnOffEffect_ToyName ToyName

The name of the AnalogToy.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyFadeOnOffEffect_Layer Layer

The layer number.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyFadeOnOffEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_AnalogToyFX_AnalogToyOnOffEffect AnalogToyOnOffEffect

\subsection use_DirectOutput_FX_AnalogToyFX_AnalogToyOnOffEffect_summary Summary

A basic effect setting the output of a AnalogToy object to a active or inactive value, based on value property (0, not 0 or null) of the TableElementData parameter of the Trigger method.



\subsection use_DirectOutput_FX_AnalogToyFX_AnalogToyOnOffEffect_samplexml Sample XML

A configuration section for AnalogToyOnOffEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<AnalogToyOnOffEffect>
  <Name>Name of AnalogToyOnOffEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Layer>0</Layer>
  <ActiveValue>
    <Value>0</Value>
    <Alpha>0</Alpha>
  </ActiveValue>
  <InactiveValue>
    <Value>0</Value>
    <Alpha>0</Alpha>
  </InactiveValue>
</AnalogToyOnOffEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_AnalogToyFX_AnalogToyOnOffEffect_properties Properties

AnalogToyOnOffEffect has the following 5 configurable properties:

\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyOnOffEffect_ActiveValue ActiveValue

The active value between 0 and 255.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyOnOffEffect_InactiveValue InactiveValue

The inactive value between 0 and 255.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyOnOffEffect_ToyName ToyName

The name of the AnalogToy.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyOnOffEffect_Layer Layer

The layer number.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyOnOffEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_BasicFX_BasicAnalogEffect BasicAnalogEffect

\subsection use_DirectOutput_FX_BasicFX_BasicAnalogEffect_samplexml Sample XML

A configuration section for BasicAnalogEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<BasicAnalogEffect>
  <Name>Name of BasicAnalogEffect</Name>
  <AnalogToyName>Name of AnalogToy</AnalogToyName>
  <ValueOn>255</ValueOn>
  <ValueOff>0</ValueOff>
</BasicAnalogEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_BasicFX_BasicAnalogEffect_properties Properties

BasicAnalogEffect has the following 4 configurable properties:

\subsubsection DirectOutput_FX_BasicFX_BasicAnalogEffect_AnalogToyName AnalogToyName

Name of the <see cref="T:DirectOutput.Cab.Toys.Basic.IAnalogToy" />.

\subsubsection DirectOutput_FX_BasicFX_BasicAnalogEffect_ValueOn ValueOn

The value which will be set for the IAnalogToy if the value of the TableElement supplied to the trigger method is &gt;0.



\subsubsection DirectOutput_FX_BasicFX_BasicAnalogEffect_ValueOff ValueOff

The value which will be set for the IAnalogToy if the value of the TableElement supplied to the trigger method is 0.



\subsubsection DirectOutput_FX_BasicFX_BasicAnalogEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_BasicFX_BasicDigitalEffect BasicDigitalEffect

\subsection use_DirectOutput_FX_BasicFX_BasicDigitalEffect_samplexml Sample XML

A configuration section for BasicDigitalEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<BasicDigitalEffect>
  <Name>Name of BasicDigitalEffect</Name>
  <DigitalToyName>Name of DigitalToy</DigitalToyName>
</BasicDigitalEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_BasicFX_BasicDigitalEffect_properties Properties

BasicDigitalEffect has the following 2 configurable properties:

\subsubsection DirectOutput_FX_BasicFX_BasicDigitalEffect_DigitalToyName DigitalToyName

Name of the <see cref="T:DirectOutput.Cab.Toys.Basic.IDigitalToy" />.

\subsubsection DirectOutput_FX_BasicFX_BasicDigitalEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_BasicFX_BasicRGBEffect BasicRGBEffect

\subsection use_DirectOutput_FX_BasicFX_BasicRGBEffect_summary Summary

The BasicRGBToyEffect is used to turn on (/set a color) and off RGB toys based on the value of a TableElement.<br />
If the value of the table element is &gt;0, the assigned IRGBToy will be set to the value specified in the Color property, for 0 the IRGBToy is set to black. <br />
If this effect is used as a static effect, the value of Color will be set on table start.



\subsection use_DirectOutput_FX_BasicFX_BasicRGBEffect_samplexml Sample XML

A configuration section for BasicRGBEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<BasicRGBEffect>
  <Name>Name of BasicRGBEffect</Name>
  <RGBToyName>Name of RGBToy</RGBToyName>
  <Color>Color string</Color>
</BasicRGBEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_BasicFX_BasicRGBEffect_properties Properties

BasicRGBEffect has the following 3 configurable properties:

\subsubsection DirectOutput_FX_BasicFX_BasicRGBEffect_RGBToyName RGBToyName

The name of the RGB toy.



\subsubsection DirectOutput_FX_BasicFX_BasicRGBEffect_Color Color

The color for the RGB toy.



\subsubsection DirectOutput_FX_BasicFX_BasicRGBEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_LedControlFX_LedControlEffect LedControlEffect

\subsection use_DirectOutput_FX_LedControlFX_LedControlEffect_summary Summary

The LedControlEffect is used when LedControl.ini files are parsed for this framework.<br />
It is recommended not to use this effect, for other purposes. Use specific effects instead.



\subsection use_DirectOutput_FX_LedControlFX_LedControlEffect_samplexml Sample XML

A configuration section for LedControlEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<LedControlEffect>
  <Name>Name of LedControlEffect</Name>
  <LedWizEquivalentName>Name of LedWizEquivalent</LedWizEquivalentName>
  <FirstOutputNumber>0</FirstOutputNumber>
  <Intensity>48</Intensity>
  <RGBColor>
    <int>-1</int>
    <int>-1</int>
    <int>-1</int>
  </RGBColor>
  <Blink>0</Blink>
  <BlinkInterval>500</BlinkInterval>
  <Duration>-1</Duration>
  <FadeUpDurationMs>0</FadeUpDurationMs>
  <FadeDownDurationMs>0</FadeDownDurationMs>
</LedControlEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_LedControlFX_LedControlEffect_properties Properties

LedControlEffect has the following 10 configurable properties:

\subsubsection DirectOutput_FX_LedControlFX_LedControlEffect_LedWizEquivalentName LedWizEquivalentName

The name of the LedWizEquivalent used for the effect output.



\subsubsection DirectOutput_FX_LedControlFX_LedControlEffect_FirstOutputNumber FirstOutputNumber

The number of the first output for this effect (1-32).



\subsubsection DirectOutput_FX_LedControlFX_LedControlEffect_Intensity Intensity

The intensity (0-48).



\subsubsection DirectOutput_FX_LedControlFX_LedControlEffect_RGBColor RGBColor

The array of color parts (Red, Green, Blue).



\subsubsection DirectOutput_FX_LedControlFX_LedControlEffect_Blink Blink

The blink configuration for the effect.



\subsubsection DirectOutput_FX_LedControlFX_LedControlEffect_BlinkInterval BlinkInterval

The blink interval in milliseconds.



\subsubsection DirectOutput_FX_LedControlFX_LedControlEffect_Duration Duration

The duration of the effect.



\subsubsection DirectOutput_FX_LedControlFX_LedControlEffect_FadeUpDurationMs FadeUpDurationMs

The fading up duration in miliseconds.



\subsubsection DirectOutput_FX_LedControlFX_LedControlEffect_FadeDownDurationMs FadeDownDurationMs

The fading down duration in miliseconds.



\subsubsection DirectOutput_FX_LedControlFX_LedControlEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_ListFX_ListEffect ListEffect

\subsection use_DirectOutput_FX_ListFX_ListEffect_summary Summary

IEffect class which handles a list of other IEffect objects.<br />
Attention! Be careful not to add ListEffect objects which finnaly contain a reference to the instance you're working with. This will create a recursive loop which never exit!.



\subsection use_DirectOutput_FX_ListFX_ListEffect_samplexml Sample XML

A configuration section for ListEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<ListEffect>
  <Name>Name of ListEffect</Name>
</ListEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_ListFX_ListEffect_properties Properties

ListEffect has the following 2 configurable properties:

\subsubsection DirectOutput_FX_ListFX_ListEffect_AssignedEffects AssignedEffects

The list of effects assigned to the ListEffect.



__Nested Properties__

The following nested propteries exist for AssignedEffects:
* __EffectName__<br/>  Name of the AssignedEffect.<br />
Triggers EffectNameChanged if value is changed.




\subsubsection DirectOutput_FX_ListFX_ListEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_NullFX_NullEffect NullEffect

\subsection use_DirectOutput_FX_NullFX_NullEffect_summary Summary

The NullEffect is a empty effect no doing anything.



\subsection use_DirectOutput_FX_NullFX_NullEffect_samplexml Sample XML

A configuration section for NullEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<NullEffect>
  <Name>Name of NullEffect</Name>
</NullEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_NullFX_NullEffect_properties Properties

NullEffect has the following 1 configurable properties:

\subsubsection DirectOutput_FX_NullFX_NullEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_RGBAFX_RGBAFadeOnOffEffect RGBAFadeOnOffEffect

\subsection use_DirectOutput_FX_RGBAFX_RGBAFadeOnOffEffect_summary Summary

This RGBA effect fades the color of a RGBA toys towards a defined target color based on the state (not 0, 0 or null) of the triggering table element (see Trigger method for details).
\image html FX_FadeOnOff.png "FadeOnOff effect"



\subsection use_DirectOutput_FX_RGBAFX_RGBAFadeOnOffEffect_samplexml Sample XML

A configuration section for RGBAFadeOnOffEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAFadeOnOffEffect>
  <Name>Name of RGBAFadeOnOffEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Layer>0</Layer>
  <RetriggerBehaviour>RestartEffect</RetriggerBehaviour>
  <FadeMode>DefinedToDefined</FadeMode>
  <FadeInactiveDurationMs>500</FadeInactiveDurationMs>
  <FadeActiveDurationMs>500</FadeActiveDurationMs>
  <ActiveColor>
    <HexColor>#00000000</HexColor>
  </ActiveColor>
  <InactiveColor>
    <HexColor>#00000000</HexColor>
  </InactiveColor>
</RGBAFadeOnOffEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_RGBAFX_RGBAFadeOnOffEffect_properties Properties

RGBAFadeOnOffEffect has the following 9 configurable properties:

\subsubsection DirectOutput_FX_RGBAFX_RGBAFadeOnOffEffect_RetriggerBehaviour RetriggerBehaviour

Valid values are RestartEffect or IgnoreRetrigger.



The property RetriggerBehaviour accepts the following values:

RestartEffect
* IgnoreRetrigger

__Valid values__

The property RetriggerBehaviour accepts the following values:

RestartEffect
* IgnoreRetrigger
\subsubsection DirectOutput_FX_RGBAFX_RGBAFadeOnOffEffect_FadeMode FadeMode

CurrentToDefinedColor or DefinedColor



The property FadeMode accepts the following values:

CurrentToDefined
* DefinedToDefined

__Valid values__

The property FadeMode accepts the following values:

CurrentToDefined
* DefinedToDefined
\subsubsection DirectOutput_FX_RGBAFX_RGBAFadeOnOffEffect_FadeInactiveDurationMs FadeInactiveDurationMs

The fading duration in milliseconds.



\subsubsection DirectOutput_FX_RGBAFX_RGBAFadeOnOffEffect_FadeActiveDurationMs FadeActiveDurationMs

The fading duration in milliseconds.



\subsubsection DirectOutput_FX_RGBAFX_RGBAFadeOnOffEffect_ActiveColor ActiveColor

The RGBA color to be used when the effect is active.



__Nested Properties__

The following nested propteries exist for ActiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_RGBAFX_RGBAFadeOnOffEffect_InactiveColor InactiveColor

The RGBA color to be used when the effect is inactive.



__Nested Properties__

The following nested propteries exist for InactiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_RGBAFX_RGBAFadeOnOffEffect_ToyName ToyName

The name of the RGBAToy.



\subsubsection DirectOutput_FX_RGBAFX_RGBAFadeOnOffEffect_Layer Layer

The layer number.



\subsubsection DirectOutput_FX_RGBAFX_RGBAFadeOnOffEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_RGBAFX_RGBAOnOffEffect RGBAOnOffEffect

\subsection use_DirectOutput_FX_RGBAFX_RGBAOnOffEffect_summary Summary

A basic RBA effect which sets the color of a layer of a RGBA toy to a specified color based on the state (not 0, 0 or null) of the triggering table element (see Trigger method for details).



\subsection use_DirectOutput_FX_RGBAFX_RGBAOnOffEffect_samplexml Sample XML

A configuration section for RGBAOnOffEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAOnOffEffect>
  <Name>Name of RGBAOnOffEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Layer>0</Layer>
  <ActiveColor>
    <HexColor>#00000000</HexColor>
  </ActiveColor>
  <InactiveColor>
    <HexColor>#00000000</HexColor>
  </InactiveColor>
</RGBAOnOffEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_RGBAFX_RGBAOnOffEffect_properties Properties

RGBAOnOffEffect has the following 5 configurable properties:

\subsubsection DirectOutput_FX_RGBAFX_RGBAOnOffEffect_ActiveColor ActiveColor

The RGBA color to be used when the effect is active.



__Nested Properties__

The following nested propteries exist for ActiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_RGBAFX_RGBAOnOffEffect_InactiveColor InactiveColor

The RGBA color to be used when the effect is inactive.



__Nested Properties__

The following nested propteries exist for InactiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_RGBAFX_RGBAOnOffEffect_ToyName ToyName

The name of the RGBAToy.



\subsubsection DirectOutput_FX_RGBAFX_RGBAOnOffEffect_Layer Layer

The layer number.



\subsubsection DirectOutput_FX_RGBAFX_RGBAOnOffEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_TimmedFX_BlinkEffect BlinkEffect

\subsection use_DirectOutput_FX_TimmedFX_BlinkEffect_summary Summary

Blink effect which triggers a TargetEffect at specified intervalls with active (org value of TableElementData used in Trigger method is used to trigger the TargetEffect) and inactive (uses 0 as the Value of the TableElementData to trigger the TargetEffect) values.<br />
\image html FX_Blink.png "Blink effect"



\subsection use_DirectOutput_FX_TimmedFX_BlinkEffect_samplexml Sample XML

A configuration section for BlinkEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<BlinkEffect>
  <Name>Name of BlinkEffect</Name>
  <TargetEffectName>Name of TargetEffect</TargetEffectName>
  <DurationActiveMs>500</DurationActiveMs>
  <DurationInactiveMs>500</DurationInactiveMs>
</BlinkEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_TimmedFX_BlinkEffect_properties Properties

BlinkEffect has the following 4 configurable properties:

\subsubsection DirectOutput_FX_TimmedFX_BlinkEffect_DurationActiveMs DurationActiveMs

The active duration of the blinking in milliseconds.



\subsubsection DirectOutput_FX_TimmedFX_BlinkEffect_DurationInactiveMs DurationInactiveMs

The inactive duration of the blinking in milliseconds.



\subsubsection DirectOutput_FX_TimmedFX_BlinkEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



\subsubsection DirectOutput_FX_TimmedFX_BlinkEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_TimmedFX_DelayEffect DelayEffect

\subsection use_DirectOutput_FX_TimmedFX_DelayEffect_summary Summary

The effect fires a assigned target effect after a specified delay.<br />
The original values supplied when the effect is triggered are forwarded to the target effect.<br />
\image html FX_Delay.png "Delay effect"



\subsection use_DirectOutput_FX_TimmedFX_DelayEffect_samplexml Sample XML

A configuration section for DelayEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<DelayEffect>
  <Name>Name of DelayEffect</Name>
  <TargetEffectName>Name of TargetEffect</TargetEffectName>
  <DelayMs>0</DelayMs>
</DelayEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_TimmedFX_DelayEffect_properties Properties

DelayEffect has the following 3 configurable properties:

\subsubsection DirectOutput_FX_TimmedFX_DelayEffect_DelayMs DelayMs

The delay in milliseconds.



\subsubsection DirectOutput_FX_TimmedFX_DelayEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



\subsubsection DirectOutput_FX_TimmedFX_DelayEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_TimmedFX_DurationEffect DurationEffect

\subsection use_DirectOutput_FX_TimmedFX_DurationEffect_summary Summary

Duration effect which triggers a specified target effect for a specified duration.<br />
When this effect is triggered it triggers the target effect immediately with the same data it has received. After the specified duration it calls trigger on the target effect again with data for the same table elmenet, but with the value changed to 0.<br />
\image html FX_Duration.png "Duration effect"



\subsection use_DirectOutput_FX_TimmedFX_DurationEffect_samplexml Sample XML

A configuration section for DurationEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<DurationEffect>
  <Name>Name of DurationEffect</Name>
  <TargetEffectName>Name of TargetEffect</TargetEffectName>
  <RetriggerBehaviour>RestartEffect</RetriggerBehaviour>
  <DurationMs>500</DurationMs>
</DurationEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_TimmedFX_DurationEffect_properties Properties

DurationEffect has the following 4 configurable properties:

\subsubsection DirectOutput_FX_TimmedFX_DurationEffect_RetriggerBehaviour RetriggerBehaviour

Valid values are RestartEffect (Restarts the duration) or IgnoreRetrigger (keeps the org duration).



The property RetriggerBehaviour accepts the following values:

RestartEffect
* IgnoreRetrigger

__Valid values__

The property RetriggerBehaviour accepts the following values:

RestartEffect
* IgnoreRetrigger
\subsubsection DirectOutput_FX_TimmedFX_DurationEffect_DurationMs DurationMs

The effect duration in milliseconds.



\subsubsection DirectOutput_FX_TimmedFX_DurationEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



\subsubsection DirectOutput_FX_TimmedFX_DurationEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_TimmedFX_ExtendDurationEffect ExtendDurationEffect

\subsection use_DirectOutput_FX_TimmedFX_ExtendDurationEffect_summary Summary

The extend duration effect triggers another effect for a duration which is extebnded by the number of milliseconds specified in DurationMs.<br />
This is done by forwarding triggers calls which are seting the effect to active directly to the target effect and delaying the forwarding of calls which set the effect to inactive by the number of milliseconds specified in DurationMs.<br />
\image html FX_ExtendDuration.png "ExtendDuration effect"



\subsection use_DirectOutput_FX_TimmedFX_ExtendDurationEffect_samplexml Sample XML

A configuration section for ExtendDurationEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<ExtendDurationEffect>
  <Name>Name of ExtendDurationEffect</Name>
  <TargetEffectName>Name of TargetEffect</TargetEffectName>
  <DurationMs>500</DurationMs>
</ExtendDurationEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_TimmedFX_ExtendDurationEffect_properties Properties

ExtendDurationEffect has the following 3 configurable properties:

\subsubsection DirectOutput_FX_TimmedFX_ExtendDurationEffect_DurationMs DurationMs

The extended duration in milliseconds.



\subsubsection DirectOutput_FX_TimmedFX_ExtendDurationEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



\subsubsection DirectOutput_FX_TimmedFX_ExtendDurationEffect_Name Name

The name of the item.



\section use_DirectOutput_FX_TimmedFX_MinDurationEffect MinDurationEffect

\subsection use_DirectOutput_FX_TimmedFX_MinDurationEffect_summary Summary

This effect enforces a minimum duration on the effect calls.<br />
Calls which are setting a effect to active (having a trigger value which is not equal 0 or null) are forwarded directly to the TargetEffect.<br />
Calls setting the effect to inactive (having a trigger value of 0) are only forwarded to the TargetEffect after the specified minimum duration has expired.<br />
\image html FX_MinDuration.png "MinDuration effect"



\subsection use_DirectOutput_FX_TimmedFX_MinDurationEffect_samplexml Sample XML

A configuration section for MinDurationEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<MinDurationEffect>
  <Name>Name of MinDurationEffect</Name>
  <TargetEffectName>Name of TargetEffect</TargetEffectName>
  <RetriggerBehaviour>RestartEffect</RetriggerBehaviour>
  <MinDurationMs>500</MinDurationMs>
</MinDurationEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_TimmedFX_MinDurationEffect_properties Properties

MinDurationEffect has the following 4 configurable properties:

\subsubsection DirectOutput_FX_TimmedFX_MinDurationEffect_RetriggerBehaviour RetriggerBehaviour

Valid values are RestartEffect (Restarts the minimal duration) or IgnoreRetrigger (keeps the org duration).



The property RetriggerBehaviour accepts the following values:

RestartEffect
* IgnoreRetrigger

__Valid values__

The property RetriggerBehaviour accepts the following values:

RestartEffect
* IgnoreRetrigger
\subsubsection DirectOutput_FX_TimmedFX_MinDurationEffect_MinDurationMs MinDurationMs

The minimal effect duration in milliseconds.



\subsubsection DirectOutput_FX_TimmedFX_MinDurationEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



\subsubsection DirectOutput_FX_TimmedFX_MinDurationEffect_Name Name

The name of the item.



