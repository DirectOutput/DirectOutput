Built in Effects  {#fx_builtin}
==========
\section use_DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect AnalogToyValueEffect

\subsection use_DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_summary Summary

This effect controlls sets the value and alpha channel of a analog alpha toy based on the trigger value.

Dependinging on the FadeMode property the effect sets the value of the target layer either to the active inactive value in OnOff mode or a mix of the value in Fade mode.



\subsection use_DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_samplexml Sample XML

A configuration section for AnalogToyValueEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<AnalogToyValueEffect>
  <Name>Name of AnalogToyValueEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <LayerNr>0</LayerNr>
  <ActiveValue>
    <Value>0</Value>
    <Alpha>0</Alpha>
  </ActiveValue>
  <InactiveValue>
    <Value>0</Value>
    <Alpha>0</Alpha>
  </InactiveValue>
  <FadeMode>Fade</FadeMode>
</AnalogToyValueEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_properties Properties

AnalogToyValueEffect has the following 6 configurable properties:

\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_ActiveValue ActiveValue

The active value and alpha channel between 0 and 255.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_FadeMode FadeMode

Fade (active and inactive color will fade depending on trigger value) or OnOff (actvice color is used for triger values &gt;0, otherwise inactive color will be used).



The property FadeMode accepts the following values:

* Fade
* OnOff

__Valid values__

The property FadeMode accepts the following values:

* Fade
* OnOff
\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_InactiveValue InactiveValue

The inactive value and alpha channel between 0 and 255.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_LayerNr LayerNr

The layer number.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_ToyName ToyName

The name of the AnalogToy.



\section use_DirectOutput_FX_ListFX_ListEffect ListEffect

\subsection use_DirectOutput_FX_ListFX_ListEffect_summary Summary

This effect triggers a list of other effect when it is triggered.<br />

\warning Be careful not to add ListEffect objects which finnaly contain a reference to the instance you're working with. This will create a recursive loop which never exit!.



\subsection use_DirectOutput_FX_ListFX_ListEffect_samplexml Sample XML

A configuration section for ListEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<ListEffect>
  <Name>Name of ListEffect</Name>
  <AssignedEffects>
    <AssignedEffect>
      <EffectName>Name of Effect</EffectName>
    </AssignedEffect>
    <AssignedEffect>
      <EffectName>Name of Effect</EffectName>
    </AssignedEffect>
    <AssignedEffect>
      <EffectName>Name of Effect</EffectName>
    </AssignedEffect>
  </AssignedEffects>
</ListEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_ListFX_ListEffect_properties Properties

ListEffect has the following 2 configurable properties:

\subsubsection DirectOutput_FX_ListFX_ListEffect_AssignedEffects AssignedEffects

The list of effects assigned to the ListEffect.



__Nested Properties__

The following nested propteries exist for AssignedEffects:
* __EffectName__<br/>  The name of the assigned effect.




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



\section use_DirectOutput_FX_RGBAFX_RGBAColorEffect RGBAColorEffect

\subsection use_DirectOutput_FX_RGBAFX_RGBAColorEffect_summary Summary

The effects sets the color of a RGBAToy based on the trigger value.

Depending on the setting of the FadeMode property, the effect uses the active or inactive color or a mix of those colors.



\subsection use_DirectOutput_FX_RGBAFX_RGBAColorEffect_samplexml Sample XML

A configuration section for RGBAColorEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAColorEffect>
  <Name>Name of RGBAColorEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <LayerNr>0</LayerNr>
  <ActiveColor>
    <HexColor>#00000000</HexColor>
  </ActiveColor>
  <InactiveColor>
    <HexColor>#00000000</HexColor>
  </InactiveColor>
  <FadeMode>Fade</FadeMode>
</RGBAColorEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_RGBAFX_RGBAColorEffect_properties Properties

RGBAColorEffect has the following 6 configurable properties:

\subsubsection DirectOutput_FX_RGBAFX_RGBAColorEffect_ActiveColor ActiveColor

The active color.



__Nested Properties__

The following nested propteries exist for ActiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_RGBAFX_RGBAColorEffect_FadeMode FadeMode

Fade (active and inactive color will fade depending on trigger value) or OnOff (actvice color is used for triger values &gt;0, otherwise inactive color will be used).



The property FadeMode accepts the following values:

* Fade
* OnOff

__Valid values__

The property FadeMode accepts the following values:

* Fade
* OnOff
\subsubsection DirectOutput_FX_RGBAFX_RGBAColorEffect_InactiveColor InactiveColor

The inactive color.



__Nested Properties__

The following nested propteries exist for InactiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_RGBAFX_RGBAColorEffect_LayerNr LayerNr

The layer number.



\subsubsection DirectOutput_FX_RGBAFX_RGBAColorEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_RGBAFX_RGBAColorEffect_ToyName ToyName

The name of the RGBAToy.



\section use_DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect RGBAMatrixBitmapEffect

\subsection use_DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_summary Summary

Displays a defined part of a bitmap on a area of a ledstrip.



\subsection use_DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_samplexml Sample XML

A configuration section for RGBAMatrixBitmapEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAMatrixBitmapEffect>
  <Name>Name of RGBAMatrixBitmapEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <BitmapFrameNumber>0</BitmapFrameNumber>
  <BitmapTop>0</BitmapTop>
  <BitmapLeft>0</BitmapLeft>
  <BitmapWidth>-1</BitmapWidth>
  <BitmapHeight>-1</BitmapHeight>
  <DataExtractMode>SinglePixelCenter</DataExtractMode>
  <FadeMode>Fade</FadeMode>
  <BitmapFilePattern>Pattern string</BitmapFilePattern>
</RGBAMatrixBitmapEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_properties Properties

RGBAMatrixBitmapEffect has the following 15 configurable properties:

\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_BitmapFilePattern BitmapFilePattern

The bitmap file pattern which is used to load the bitmap file for the effect.



__Nested Properties__

The following nested propteries exist for BitmapFilePattern:
* __Pattern__<br/>  The pattern used to look for files.




\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_BitmapFrameNumber BitmapFrameNumber

The number of the frame to be displayed.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_BitmapHeight BitmapHeight

The height of the the part of the bitmap which is to be displayed.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_BitmapLeft BitmapLeft

The left boundary of the the part of the bitmap which is to be displayed.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_BitmapTop BitmapTop

The top of the the part of the bitmap which is to be displayed.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_BitmapWidth BitmapWidth

The width of the the part of the bitmap which is to be displayed.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_DataExtractMode DataExtractMode

The data extract mode which defines how the data is extracted from the source bitmap.



The property DataExtractMode accepts the following values:

* SinglePixelTopLeft
* SinglePixelCenter
* BlendPixels

__Valid values__

The property DataExtractMode accepts the following values:

* SinglePixelTopLeft
* SinglePixelCenter
* BlendPixels
\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_FadeMode FadeMode

Fade (active and inactive color will fade depending on trigger value) or OnOff (actvice color is used for triger values &gt;0, otherwise inactive color will be used).



The property FadeMode accepts the following values:

* Fade
* OnOff

__Valid values__

The property FadeMode accepts the following values:

* Fade
* OnOff
\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_Height Height

The height of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_Left Left

The left resp. X position of the upper left corner of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_Top Top

The top resp. Y position of the upper left corner of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixBitmapEffect_Width Width

The width of the target area for the effect (0-100).



\section use_DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect RGBAMatrixColorEffect

\subsection use_DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect_summary Summary

Sets the spefied area of a ledstrip to a color depending on configured colors and the trigger value.



\subsection use_DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect_samplexml Sample XML

A configuration section for RGBAMatrixColorEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAMatrixColorEffect>
  <Name>Name of RGBAMatrixColorEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <ActiveColor>
    <HexColor>#00000000</HexColor>
  </ActiveColor>
  <InactiveColor>
    <HexColor>#00000000</HexColor>
  </InactiveColor>
  <FadeMode>Fade</FadeMode>
</RGBAMatrixColorEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect_properties Properties

RGBAMatrixColorEffect has the following 10 configurable properties:

\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect_ActiveColor ActiveColor

The active color.



__Nested Properties__

The following nested propteries exist for ActiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect_FadeMode FadeMode

Fade (active and inactive color will fade depending on trigger value) or OnOff (actvice color is used for triger values &gt;0, otherwise inactive color will be used).



The property FadeMode accepts the following values:

* Fade
* OnOff

__Valid values__

The property FadeMode accepts the following values:

* Fade
* OnOff
\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect_Height Height

The height of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect_InactiveColor InactiveColor

The inactive color.



__Nested Properties__

The following nested propteries exist for InactiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect_Left Left

The left resp. X position of the upper left corner of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect_Top Top

The top resp. Y position of the upper left corner of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorEffect_Width Width

The width of the target area for the effect (0-100).



\section use_DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect RGBAMatrixColorShiftEffect

\subsection use_DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_samplexml Sample XML

A configuration section for RGBAMatrixColorShiftEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAMatrixColorShiftEffect>
  <Name>Name of RGBAMatrixColorShiftEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <ActiveColor>
    <HexColor>#00000000</HexColor>
  </ActiveColor>
  <InactiveColor>
    <HexColor>#00000000</HexColor>
  </InactiveColor>
  <FadeMode>Fade</FadeMode>
  <ShiftDirection>Right</ShiftDirection>
  <ShiftSpeed>200</ShiftSpeed>
</RGBAMatrixColorShiftEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_properties Properties

RGBAMatrixColorShiftEffect has the following 12 configurable properties:

\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_ActiveColor ActiveColor

The active color.



__Nested Properties__

The following nested propteries exist for ActiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_FadeMode FadeMode

Fade (active and inactive color will fade depending on trigger value) or OnOff (actvice color is used for triger values &gt;0, otherwise inactive color will be used).



The property FadeMode accepts the following values:

* Fade
* OnOff

__Valid values__

The property FadeMode accepts the following values:

* Fade
* OnOff
\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_Height Height

The height of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_InactiveColor InactiveColor

The inactive color.



__Nested Properties__

The following nested propteries exist for InactiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_Left Left

The left resp. X position of the upper left corner of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_ShiftDirection ShiftDirection

The shift direction (Left, Right, Up, Down).



The property ShiftDirection accepts the following values:

* Down
* Left
* Right
* Up

__Valid values__

The property ShiftDirection accepts the following values:

* Down
* Left
* Right
* Up
\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_ShiftSpeed ShiftSpeed

The shift speed in percentage of the effect area (Left, Top, Width, Height properties) per second .



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_Top Top

The top resp. Y position of the upper left corner of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_RGBAMatrixFX_RGBAMatrixColorShiftEffect_Width Width

The width of the target area for the effect (0-100).



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
  <HighValue>-1</HighValue>
  <LowValue>0</LowValue>
  <DurationActiveMs>500</DurationActiveMs>
  <DurationInactiveMs>500</DurationInactiveMs>
  <UntriggerBehaviour>Immediate</UntriggerBehaviour>
</BlinkEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_TimmedFX_BlinkEffect_properties Properties

BlinkEffect has the following 7 configurable properties:

\subsubsection DirectOutput_FX_TimmedFX_BlinkEffect_DurationActiveMs DurationActiveMs

The active duration of the blinking in milliseconds.



\subsubsection DirectOutput_FX_TimmedFX_BlinkEffect_DurationInactiveMs DurationInactiveMs

The inactive duration of the blinking in milliseconds.



\subsubsection DirectOutput_FX_TimmedFX_BlinkEffect_HighValue HighValue

The high value for the blinking. Values between 0 and 255 define the actual values which have to be output during the on phase of the blinking. A value of -1 defines that the value which has been received by the trigger event is used.



\subsubsection DirectOutput_FX_TimmedFX_BlinkEffect_LowValue LowValue

The low value for the blinking (0-255).



\subsubsection DirectOutput_FX_TimmedFX_BlinkEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_TimmedFX_BlinkEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



\subsubsection DirectOutput_FX_TimmedFX_BlinkEffect_UntriggerBehaviour UntriggerBehaviour

The untrigger behaviour defines how the blinking stops.



The property UntriggerBehaviour accepts the following values:

* Immediate
* CompleteHigh

__Valid values__

The property UntriggerBehaviour accepts the following values:

* Immediate
* CompleteHigh
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



\subsubsection DirectOutput_FX_TimmedFX_DelayEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_TimmedFX_DelayEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



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
  <RetriggerBehaviour>Restart</RetriggerBehaviour>
  <DurationMs>500</DurationMs>
</DurationEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_TimmedFX_DurationEffect_properties Properties

DurationEffect has the following 4 configurable properties:

\subsubsection DirectOutput_FX_TimmedFX_DurationEffect_DurationMs DurationMs

The effect duration in milliseconds.



\subsubsection DirectOutput_FX_TimmedFX_DurationEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_TimmedFX_DurationEffect_RetriggerBehaviour RetriggerBehaviour

Valid values are Restart (Restarts the duration) or Ignore (keeps the org duration).



The property RetriggerBehaviour accepts the following values:

* Restart
* Ignore

__Valid values__

The property RetriggerBehaviour accepts the following values:

* Restart
* Ignore
\subsubsection DirectOutput_FX_TimmedFX_DurationEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



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



\subsubsection DirectOutput_FX_TimmedFX_ExtendDurationEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_TimmedFX_ExtendDurationEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



\section use_DirectOutput_FX_TimmedFX_FadeEffect FadeEffect

\subsection use_DirectOutput_FX_TimmedFX_FadeEffect_summary Summary

This effect fades towards the value passed to the effect in the TableElementData of the trigger methods.
It is calling the target effect repeatedly with the changing values.



\subsection use_DirectOutput_FX_TimmedFX_FadeEffect_samplexml Sample XML

A configuration section for FadeEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<FadeEffect>
  <Name>Name of FadeEffect</Name>
  <TargetEffectName>Name of TargetEffect</TargetEffectName>
  <FadeUpDuration>300</FadeUpDuration>
  <FadeDownDuration>300</FadeDownDuration>
  <FadeDurationMode>CurrentToTarget</FadeDurationMode>
</FadeEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_TimmedFX_FadeEffect_properties Properties

FadeEffect has the following 5 configurable properties:

\subsubsection DirectOutput_FX_TimmedFX_FadeEffect_FadeDownDuration FadeDownDuration

The duration for the fading down.



\subsubsection DirectOutput_FX_TimmedFX_FadeEffect_FadeDurationMode FadeDurationMode

The fade duration mode.<br />
Depending on the FadeDurationMode the transition from the current to the target value will use one of the duration values directly or use the duration values to determine how long it would take to fade through the whole possible value range and the effective fading duration will depend on the defference between the current and the target value.



The property FadeDurationMode accepts the following values:

* CurrentToTarget
* FullValueRange

__Valid values__

The property FadeDurationMode accepts the following values:

* CurrentToTarget
* FullValueRange
\subsubsection DirectOutput_FX_TimmedFX_FadeEffect_FadeUpDuration FadeUpDuration

The duration for fading up.



\subsubsection DirectOutput_FX_TimmedFX_FadeEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_TimmedFX_FadeEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



\section use_DirectOutput_FX_TimmedFX_MaxDurationEffect MaxDurationEffect

\subsection use_DirectOutput_FX_TimmedFX_MaxDurationEffect_summary Summary

Limits the max duration of the effect to the specified number of milliseconds.



\subsection use_DirectOutput_FX_TimmedFX_MaxDurationEffect_samplexml Sample XML

A configuration section for MaxDurationEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<MaxDurationEffect>
  <Name>Name of MaxDurationEffect</Name>
  <TargetEffectName>Name of TargetEffect</TargetEffectName>
  <RetriggerBehaviour>Ignore</RetriggerBehaviour>
  <MaxDurationMs>500</MaxDurationMs>
</MaxDurationEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_TimmedFX_MaxDurationEffect_properties Properties

MaxDurationEffect has the following 4 configurable properties:

\subsubsection DirectOutput_FX_TimmedFX_MaxDurationEffect_MaxDurationMs MaxDurationMs

The max effect duration in milliseconds.



\subsubsection DirectOutput_FX_TimmedFX_MaxDurationEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_TimmedFX_MaxDurationEffect_RetriggerBehaviour RetriggerBehaviour

Valid values are Restart (restarts the minimal duration) or Ignore (keeps the org duration).



The property RetriggerBehaviour accepts the following values:

* Restart
* Ignore

__Valid values__

The property RetriggerBehaviour accepts the following values:

* Restart
* Ignore
\subsubsection DirectOutput_FX_TimmedFX_MaxDurationEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



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
  <RetriggerBehaviour>Ignore</RetriggerBehaviour>
  <MinDurationMs>500</MinDurationMs>
</MinDurationEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_TimmedFX_MinDurationEffect_properties Properties

MinDurationEffect has the following 4 configurable properties:

\subsubsection DirectOutput_FX_TimmedFX_MinDurationEffect_MinDurationMs MinDurationMs

The minimal effect duration in milliseconds.



\subsubsection DirectOutput_FX_TimmedFX_MinDurationEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_TimmedFX_MinDurationEffect_RetriggerBehaviour RetriggerBehaviour

Valid values are Restart (restarts the minimal duration) or Ignore (keeps the org duration).



The property RetriggerBehaviour accepts the following values:

* Restart
* Ignore

__Valid values__

The property RetriggerBehaviour accepts the following values:

* Restart
* Ignore
\subsubsection DirectOutput_FX_TimmedFX_MinDurationEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



\section use_DirectOutput_FX_ValueFX_ValueInvertEffect ValueInvertEffect

\subsection use_DirectOutput_FX_ValueFX_ValueInvertEffect_summary Summary

Inverts the trigger value of the effect before the target effect is called (e.g. 0 becomes 255, 255 becomes 0, 10 becomes 245).



\subsection use_DirectOutput_FX_ValueFX_ValueInvertEffect_samplexml Sample XML

A configuration section for ValueInvertEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<ValueInvertEffect>
  <Name>Name of ValueInvertEffect</Name>
  <TargetEffectName>Name of TargetEffect</TargetEffectName>
</ValueInvertEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_ValueFX_ValueInvertEffect_properties Properties

ValueInvertEffect has the following 2 configurable properties:

\subsubsection DirectOutput_FX_ValueFX_ValueInvertEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_ValueFX_ValueInvertEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



\section use_DirectOutput_FX_ValueFX_ValueMapFullRangeEffect ValueMapFullRangeEffect

\subsection use_DirectOutput_FX_ValueFX_ValueMapFullRangeEffect_summary Summary

This effects maps the trigger value to the full range of 0 - 255.
If the trigger value is 0, the mapped trigger value for the target effect is also 0.
If the trigger value is &gt;0, the mapped trigger value for the target effect is 255.



\subsection use_DirectOutput_FX_ValueFX_ValueMapFullRangeEffect_samplexml Sample XML

A configuration section for ValueMapFullRangeEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<ValueMapFullRangeEffect>
  <Name>Name of ValueMapFullRangeEffect</Name>
  <TargetEffectName>Name of TargetEffect</TargetEffectName>
</ValueMapFullRangeEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_ValueFX_ValueMapFullRangeEffect_properties Properties

ValueMapFullRangeEffect has the following 2 configurable properties:

\subsubsection DirectOutput_FX_ValueFX_ValueMapFullRangeEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_ValueFX_ValueMapFullRangeEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



