Built in Effects  {#fx_builtin}
==========
\section use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect AnalogAlphaMatrixBitmapAnimationEffect

\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_summary Summary

Displays parts of a bitmap as a animation on a matrix of AnalogAlpha elements.
Check the docu on the other bitmap effects for more details on these effect types.



\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_samplexml Sample XML

A configuration section for AnalogAlphaMatrixBitmapAnimationEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<AnalogAlphaMatrixBitmapAnimationEffect>
  <Name>Name of AnalogAlphaMatrixBitmapAnimationEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <FadeMode>Fade</FadeMode>
  <AnimationStepDirection>Frame</AnimationStepDirection>
  <AnimationStepSize>1</AnimationStepSize>
  <AnimationFrameCount>1</AnimationFrameCount>
  <AnimationBehaviour>Loop</AnimationBehaviour>
  <AnimationFrameDurationMs>30</AnimationFrameDurationMs>
  <BitmapFrameNumber>0</BitmapFrameNumber>
  <BitmapTop>0</BitmapTop>
  <BitmapLeft>0</BitmapLeft>
  <BitmapWidth>-1</BitmapWidth>
  <BitmapHeight>-1</BitmapHeight>
  <DataExtractMode>BlendPixels</DataExtractMode>
  <BitmapFilePattern>Pattern string</BitmapFilePattern>
</AnalogAlphaMatrixBitmapAnimationEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_properties Properties

AnalogAlphaMatrixBitmapAnimationEffect has the following 20 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_AnimationBehaviour AnimationBehaviour

The animation behaviour defines if a animation should run only once, run in a loop or continue at its last position when triggered.



The property AnimationBehaviour accepts the following values:

* __Continue__: The animation continues with the next frame when triggered and is shown in a loop
* __Loop__: The animation restarts when it is triggered and is shown in a loop
* __Once__: The animation restarts when it is triggered, it is shown once and stops after the last frame


\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_AnimationFrameCount AnimationFrameCount

The number of frames for the whole animation.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_AnimationFrameDurationMs AnimationFrameDurationMs

The animation frame duration in miliseconds. Defaults to 30ms if not set.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_AnimationStepDirection AnimationStepDirection

The direction in which the effect will step formward through the source image to get the next frame of the animation.



The property AnimationStepDirection accepts the following values:

* __Down__: Animation steps from top to bottom through the source image
* __Frame__: Animation steps though frames of the source image (this mainly for animated gifs).
* __Right__: Animation steps from left to right through the source image


\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_AnimationStepSize AnimationStepSize

The size of the step in pixels or frames (depending on the \ref AnimationStepDirection) to the next frame of the animation.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_BitmapFilePattern BitmapFilePattern

The bitmap file pattern which is used to load the bitmap file for the effect.



__Nested Properties__

The following nested propteries exist for BitmapFilePattern:
* __Pattern__<br/>  The pattern used to look for files.




\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_BitmapFrameNumber BitmapFrameNumber

The number of the frame to be displayed.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_BitmapHeight BitmapHeight

The height of the the part of the bitmap which is to be displayed. -1 selects the fully height resp. the remaining height from the BitMapTop position.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_BitmapLeft BitmapLeft

The left boundary in pixels of the the part of the bitmap which is to be displayed.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_BitmapTop BitmapTop

The top of the the part of the bitmap which is to be displayed.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_BitmapWidth BitmapWidth

The width in pixels of the the part of the bitmap which is to be displayed. -1 selects the fully width resp. the remaining width from the BitMapLeft position.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_DataExtractMode DataExtractMode

The data extract mode which defines how the data is extracted from the source bitmap.



The property DataExtractMode accepts the following values:

* __SinglePixelTopLeft__
* __SinglePixelCenter__
* __BlendPixels__


\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect AnalogAlphaMatrixBitmapEffect

\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_summary Summary

Displays a defined part of a bitmap on a area of a AnalogAlpha Matrix.



\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_samplexml Sample XML

A configuration section for AnalogAlphaMatrixBitmapEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<AnalogAlphaMatrixBitmapEffect>
  <Name>Name of AnalogAlphaMatrixBitmapEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <FadeMode>Fade</FadeMode>
  <BitmapFrameNumber>0</BitmapFrameNumber>
  <BitmapTop>0</BitmapTop>
  <BitmapLeft>0</BitmapLeft>
  <BitmapWidth>-1</BitmapWidth>
  <BitmapHeight>-1</BitmapHeight>
  <DataExtractMode>BlendPixels</DataExtractMode>
  <BitmapFilePattern>Pattern string</BitmapFilePattern>
</AnalogAlphaMatrixBitmapEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_properties Properties

AnalogAlphaMatrixBitmapEffect has the following 15 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_BitmapFilePattern BitmapFilePattern

The bitmap file pattern which is used to load the bitmap file for the effect.



__Nested Properties__

The following nested propteries exist for BitmapFilePattern:
* __Pattern__<br/>  The pattern used to look for files.




\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_BitmapFrameNumber BitmapFrameNumber

The number of the frame to be used (for animated gifs).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_BitmapHeight BitmapHeight

The height of the the part of the bitmap which is to be used.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_BitmapLeft BitmapLeft

The left boundary of the the part of the bitmap which is to be used.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_BitmapTop BitmapTop

The top of the the part of the bitmap which is to be used.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_BitmapWidth BitmapWidth

The width of the the part of the bitmap which is to be used.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_DataExtractMode DataExtractMode

The data extract mode which defines how the data is extracted from the source bitmap.



The property DataExtractMode accepts the following values:

* __SinglePixelTopLeft__
* __SinglePixelCenter__
* __BlendPixels__


\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect AnalogAlphaMatrixFlickerEffect

\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_summary Summary

Does create random flickering with a defineable density, durations and value within the spefied area of a matrix.



\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_samplexml Sample XML

A configuration section for AnalogAlphaMatrixFlickerEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<AnalogAlphaMatrixFlickerEffect>
  <Name>Name of AnalogAlphaMatrixFlickerEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <FadeMode>Fade</FadeMode>
  <Density>10</Density>
  <MinFlickerDurationMs>60</MinFlickerDurationMs>
  <MaxFlickerDurationMs>150</MaxFlickerDurationMs>
  <FlickerFadeUpDurationMs>0</FlickerFadeUpDurationMs>
  <FlickerFadeDownDurationMs>0</FlickerFadeDownDurationMs>
  <ActiveValue>
    <Value>0</Value>
    <Alpha>0</Alpha>
  </ActiveValue>
  <InactiveValue>
    <Value>0</Value>
    <Alpha>0</Alpha>
  </InactiveValue>
</AnalogAlphaMatrixFlickerEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_properties Properties

AnalogAlphaMatrixFlickerEffect has the following 15 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_ActiveValue ActiveValue

The active value.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_Density Density

The density if the flickering in percent.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_FlickerFadeDownDurationMs FlickerFadeDownDurationMs

The fade down duration in milliseconds for a single flicker/blink of a element.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_FlickerFadeUpDurationMs FlickerFadeUpDurationMs

The fade up duration in milliseconds for a single flicker/blink of a element.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_InactiveValue InactiveValue

The inactive value.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_MaxFlickerDurationMs MaxFlickerDurationMs

The max duration in milliseconds for a single flicker/blink of a element.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_MinFlickerDurationMs MinFlickerDurationMs

The min duration in milliseconds for a single flicker/blink of a element.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect AnalogAlphaMatrixShiftEffect

\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_summary Summary

Same kind of effect like the RGBAMatrixShift effect, but for AnalogAlpha elements (just about everything which is not a RGBA element).




\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_samplexml Sample XML

A configuration section for AnalogAlphaMatrixShiftEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<AnalogAlphaMatrixShiftEffect>
  <Name>Name of AnalogAlphaMatrixShiftEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <FadeMode>Fade</FadeMode>
  <ShiftDirection>Right</ShiftDirection>
  <ShiftSpeed>200</ShiftSpeed>
  <ShiftAcceleration>0</ShiftAcceleration>
  <ActiveValue>
    <Value>0</Value>
    <Alpha>0</Alpha>
  </ActiveValue>
  <InactiveValue>
    <Value>0</Value>
    <Alpha>0</Alpha>
  </InactiveValue>
</AnalogAlphaMatrixShiftEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_properties Properties

AnalogAlphaMatrixShiftEffect has the following 13 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_ActiveValue ActiveValue

The active Value.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_InactiveValue InactiveValue

The inactive Value.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_ShiftAcceleration ShiftAcceleration

The acceleration for the shift speed in percent of the effect area per second.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_ShiftDirection ShiftDirection

The shift direction (Left, Right, Up, Down).



The property ShiftDirection accepts the following values:

* __Down__: Shift down
* __Left__: Shift left
* __Right__: Shift right
* __Up__: Shift up


\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_ShiftSpeed ShiftSpeed

The shift speed in percentage of the effect area (Left, Top, Width, Height properties) per second .



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect AnalogAlphaMatrixValueEffect

\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_summary Summary

Sets the spefied area of matrix to the specified values depending on the trigger value.



\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_samplexml Sample XML

A configuration section for AnalogAlphaMatrixValueEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<AnalogAlphaMatrixValueEffect>
  <Name>Name of AnalogAlphaMatrixValueEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <FadeMode>Fade</FadeMode>
  <ActiveValue>
    <Value>0</Value>
    <Alpha>0</Alpha>
  </ActiveValue>
  <InactiveValue>
    <Value>0</Value>
    <Alpha>0</Alpha>
  </InactiveValue>
</AnalogAlphaMatrixValueEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_properties Properties

AnalogAlphaMatrixValueEffect has the following 10 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_ActiveValue ActiveValue

The active value.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_InactiveValue InactiveValue

The inactive value.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_Width Width

The width in percent of the target area for the effect (0-100).



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

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_InactiveValue InactiveValue

The inactive value and alpha channel between 0 and 255.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_LayerNr LayerNr

The layer number.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_ToyName ToyName

The name of the AnalogToy.



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

* __Immediate__: Blinking stops immediately
* __CompleteHigh__: Completes the high cycle of the blinking before stopping.


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

* __Restart__: The effect or its behaviour gets restarted in a retrigger situation.
* __Ignore__: Retrigger calls are ignored. The effect or its behaviour is not being restarted.


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

* __CurrentToTarget__: The duration(s) specify whoe long it will take to fade from the current value to the target value.
* __FullValueRange__: The duration(s) specify how long it would take to fade through the whole possible value range (0-255) for the target value. The effective fading duration will depend on the difference between the current and the target value.


\subsubsection DirectOutput_FX_TimmedFX_FadeEffect_FadeUpDuration FadeUpDuration

The duration for fading up.



\subsubsection DirectOutput_FX_TimmedFX_FadeEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_TimmedFX_FadeEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



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



\section use_DirectOutput_FX_TimmedFX_MaxDurationEffect MaxDurationEffect

\subsection use_DirectOutput_FX_TimmedFX_MaxDurationEffect_summary Summary

Limits the max duration of the effect to the specified number of milliseconds.



\subsection use_DirectOutput_FX_TimmedFX_MaxDurationEffect_samplexml Sample XML

A configuration section for MaxDurationEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<MaxDurationEffect>
  <Name>Name of MaxDurationEffect</Name>
  <TargetEffectName>Name of TargetEffect</TargetEffectName>
  <RetriggerBehaviour>Restart</RetriggerBehaviour>
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

* __Restart__: The effect or its behaviour gets restarted in a retrigger situation.
* __Ignore__: Retrigger calls are ignored. The effect or its behaviour is not being restarted.


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
  <RetriggerBehaviour>Restart</RetriggerBehaviour>
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

* __Restart__: The effect or its behaviour gets restarted in a retrigger situation.
* __Ignore__: Retrigger calls are ignored. The effect or its behaviour is not being restarted.


\subsubsection DirectOutput_FX_TimmedFX_MinDurationEffect_TargetEffectName TargetEffectName

Name of the target effect.<br />
Triggers EffectNameChanged if value is changed.



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

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


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



\section use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect RGBAMatrixBitmapAnimationEffect

\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_summary Summary

The RGBAMatrixBitmapAnimationEffect displays a anmation which is based on a image file on the defineable part of a matrix of rgb toys (e.g. adressable ledstrip).

The properties of the effect allow you to specify the position, frame and size of the first image part to be displayed on the matrix. In addition you can define how the effect steps forward through the source picture for the further animation frames.

To get a better idea, have a look at the following video and the picture below it.

\htmlinclude 61_FX_BuiltIn_RGBAMatrixBitmapAnimationVideo.html

\image html RGBAMatrixBitmapAnimationEffectExample.png
The image above shows what DOF does for the following settings:

* AnimationStepDirection: Down
* AnimationStepSize:5
* AnimationFrameCount:116
* AnimationBehaviour:Loop
* AnimationFrameDurationMs:30
* BitmapTop:10
* BitmapLeft:0
* BitmapWidth:100
* BitmapHeight:20
* DataExtractMode:BlendPixels

In this example DOF extracts a area of 20x100pixels for every frame of the animation. For every frame of the animation it steps 5 pixels down, so we slowly progress through the whole image.




\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_samplexml Sample XML

A configuration section for RGBAMatrixBitmapAnimationEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAMatrixBitmapAnimationEffect>
  <Name>Name of RGBAMatrixBitmapAnimationEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <FadeMode>Fade</FadeMode>
  <AnimationStepDirection>Frame</AnimationStepDirection>
  <AnimationStepSize>1</AnimationStepSize>
  <AnimationFrameCount>1</AnimationFrameCount>
  <AnimationBehaviour>Loop</AnimationBehaviour>
  <AnimationFrameDurationMs>30</AnimationFrameDurationMs>
  <BitmapFrameNumber>0</BitmapFrameNumber>
  <BitmapTop>0</BitmapTop>
  <BitmapLeft>0</BitmapLeft>
  <BitmapWidth>-1</BitmapWidth>
  <BitmapHeight>-1</BitmapHeight>
  <DataExtractMode>BlendPixels</DataExtractMode>
  <BitmapFilePattern>Pattern string</BitmapFilePattern>
</RGBAMatrixBitmapAnimationEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_properties Properties

RGBAMatrixBitmapAnimationEffect has the following 20 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_AnimationBehaviour AnimationBehaviour

The animation behaviour defines if a animation should run only once, run in a loop or continue at its last position when triggered.



The property AnimationBehaviour accepts the following values:

* __Continue__: The animation continues with the next frame when triggered and is shown in a loop
* __Loop__: The animation restarts when it is triggered and is shown in a loop
* __Once__: The animation restarts when it is triggered, it is shown once and stops after the last frame


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_AnimationFrameCount AnimationFrameCount

The number of frames for the whole animation.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_AnimationFrameDurationMs AnimationFrameDurationMs

The animation frame duration in miliseconds. Defaults to 30ms if not set.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_AnimationStepDirection AnimationStepDirection

The direction in which the effect will step formward through the source image to get the next frame of the animation.



The property AnimationStepDirection accepts the following values:

* __Down__: Animation steps from top to bottom through the source image
* __Frame__: Animation steps though frames of the source image (this mainly for animated gifs).
* __Right__: Animation steps from left to right through the source image


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_AnimationStepSize AnimationStepSize

The size of the step in pixels or frames (depending on the \ref AnimationStepDirection) to the next frame of the animation.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_BitmapFilePattern BitmapFilePattern

The bitmap file pattern which is used to load the bitmap file for the effect.



__Nested Properties__

The following nested propteries exist for BitmapFilePattern:
* __Pattern__<br/>  The pattern used to look for files.




\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_BitmapFrameNumber BitmapFrameNumber

The number of the frame to be displayed.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_BitmapHeight BitmapHeight

The height of the the part of the bitmap which is to be displayed. -1 selects the fully height resp. the remaining height from the BitMapTop position.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_BitmapLeft BitmapLeft

The left boundary in pixels of the the part of the bitmap which is to be displayed.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_BitmapTop BitmapTop

The top of the the part of the bitmap which is to be displayed.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_BitmapWidth BitmapWidth

The width in pixels of the the part of the bitmap which is to be displayed. -1 selects the fully width resp. the remaining width from the BitMapLeft position.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_DataExtractMode DataExtractMode

The data extract mode which defines how the data is extracted from the source bitmap.



The property DataExtractMode accepts the following values:

* __SinglePixelTopLeft__
* __SinglePixelCenter__
* __BlendPixels__


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect RGBAMatrixBitmapEffect

\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_summary Summary

The RGBAMatrixBitmapEffect displays a defined part of a bitmap on a area of a RGBAtoy Matrix.

The properties of the effect allow you to select the part of the bitmap to display as well as the area of the matrix on which the bitmap is displayed. Dempending on the size of your bitmap you might choose different modes for the image extraction.

The effect supports numerous imahe formats, inluding png, gif (also animated) and jpg.

The image extraction takes place upon initalization of the framework. While the framework is active, it only outputs the previously extracted and scaled data to allow for better performance.



\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_samplexml Sample XML

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
  <FadeMode>Fade</FadeMode>
  <BitmapFrameNumber>0</BitmapFrameNumber>
  <BitmapTop>0</BitmapTop>
  <BitmapLeft>0</BitmapLeft>
  <BitmapWidth>-1</BitmapWidth>
  <BitmapHeight>-1</BitmapHeight>
  <DataExtractMode>BlendPixels</DataExtractMode>
  <BitmapFilePattern>Pattern string</BitmapFilePattern>
</RGBAMatrixBitmapEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_properties Properties

RGBAMatrixBitmapEffect has the following 15 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_BitmapFilePattern BitmapFilePattern

The bitmap file pattern which is used to load the bitmap file for the effect.



__Nested Properties__

The following nested propteries exist for BitmapFilePattern:
* __Pattern__<br/>  The pattern used to look for files.




\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_BitmapFrameNumber BitmapFrameNumber

The number of the frame to be used (for animated gifs).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_BitmapHeight BitmapHeight

The height of the the part of the bitmap which is to be used.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_BitmapLeft BitmapLeft

The left boundary of the the part of the bitmap which is to be used.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_BitmapTop BitmapTop

The top of the the part of the bitmap which is to be used.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_BitmapWidth BitmapWidth

The width of the the part of the bitmap which is to be used.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_DataExtractMode DataExtractMode

The data extract mode which defines how the data is extracted from the source bitmap.



The property DataExtractMode accepts the following values:

* __SinglePixelTopLeft__
* __SinglePixelCenter__
* __BlendPixels__


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect RGBAMatrixColorEffect

\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_summary Summary

Sets the spefied area of matrix to the specified colors depending on the trigger value.



\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_samplexml Sample XML

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
  <FadeMode>Fade</FadeMode>
  <ActiveColor>
    <HexColor>#00000000</HexColor>
  </ActiveColor>
  <InactiveColor>
    <HexColor>#00000000</HexColor>
  </InactiveColor>
</RGBAMatrixColorEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_properties Properties

RGBAMatrixColorEffect has the following 10 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_ActiveColor ActiveColor

The active color.



__Nested Properties__

The following nested propteries exist for ActiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_InactiveColor InactiveColor

The inactive color.



__Nested Properties__

The following nested propteries exist for InactiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect RGBAMatrixColorScaleBitmapAnimationEffect

\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_summary Summary

This displays a defined part of a bitmap as a animation in the given colors on a area of a RGBAtoy Matrix. The effect take the overall brightness of the pixels of the bitmap to control the brightness of the specified colors for each pixel.

The properties of the effect allow you to select the parts of the bitmap to display for the animation as well as the area of the matrix on which the bitmap is displayed. Depending on the size of your bitmap you might choose different modes for the image extraction.

The effect supports numerous image formats, inluding png, gif (also animated) and jpg.

The image extraction takes place upon initalization of the framework. While the framework is active, it only outputs the previously extracted and scaled data to allow for better performance.



\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_samplexml Sample XML

A configuration section for RGBAMatrixColorScaleBitmapAnimationEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAMatrixColorScaleBitmapAnimationEffect>
  <Name>Name of RGBAMatrixColorScaleBitmapAnimationEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <FadeMode>Fade</FadeMode>
  <AnimationStepDirection>Frame</AnimationStepDirection>
  <AnimationStepSize>1</AnimationStepSize>
  <AnimationFrameCount>1</AnimationFrameCount>
  <AnimationBehaviour>Loop</AnimationBehaviour>
  <AnimationFrameDurationMs>30</AnimationFrameDurationMs>
  <BitmapFrameNumber>0</BitmapFrameNumber>
  <BitmapTop>0</BitmapTop>
  <BitmapLeft>0</BitmapLeft>
  <BitmapWidth>-1</BitmapWidth>
  <BitmapHeight>-1</BitmapHeight>
  <DataExtractMode>BlendPixels</DataExtractMode>
  <BitmapFilePattern>Pattern string</BitmapFilePattern>
  <ActiveColor>
    <HexColor>#00000000</HexColor>
  </ActiveColor>
  <InactiveColor>
    <HexColor>#00000000</HexColor>
  </InactiveColor>
</RGBAMatrixColorScaleBitmapAnimationEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_properties Properties

RGBAMatrixColorScaleBitmapAnimationEffect has the following 22 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_ActiveColor ActiveColor

The active color.



__Nested Properties__

The following nested propteries exist for ActiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_AnimationBehaviour AnimationBehaviour

The animation behaviour defines if a animation should run only once, run in a loop or continue at its last position when triggered.



The property AnimationBehaviour accepts the following values:

* __Continue__: The animation continues with the next frame when triggered and is shown in a loop
* __Loop__: The animation restarts when it is triggered and is shown in a loop
* __Once__: The animation restarts when it is triggered, it is shown once and stops after the last frame


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_AnimationFrameCount AnimationFrameCount

The number of frames for the whole animation.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_AnimationFrameDurationMs AnimationFrameDurationMs

The animation frame duration in miliseconds. Defaults to 30ms if not set.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_AnimationStepDirection AnimationStepDirection

The direction in which the effect will step formward through the source image to get the next frame of the animation.



The property AnimationStepDirection accepts the following values:

* __Down__: Animation steps from top to bottom through the source image
* __Frame__: Animation steps though frames of the source image (this mainly for animated gifs).
* __Right__: Animation steps from left to right through the source image


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_AnimationStepSize AnimationStepSize

The size of the step in pixels or frames (depending on the \ref AnimationStepDirection) to the next frame of the animation.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_BitmapFilePattern BitmapFilePattern

The bitmap file pattern which is used to load the bitmap file for the effect.



__Nested Properties__

The following nested propteries exist for BitmapFilePattern:
* __Pattern__<br/>  The pattern used to look for files.




\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_BitmapFrameNumber BitmapFrameNumber

The number of the frame to be displayed.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_BitmapHeight BitmapHeight

The height of the the part of the bitmap which is to be displayed. -1 selects the fully height resp. the remaining height from the BitMapTop position.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_BitmapLeft BitmapLeft

The left boundary in pixels of the the part of the bitmap which is to be displayed.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_BitmapTop BitmapTop

The top of the the part of the bitmap which is to be displayed.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_BitmapWidth BitmapWidth

The width in pixels of the the part of the bitmap which is to be displayed. -1 selects the fully width resp. the remaining width from the BitMapLeft position.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_DataExtractMode DataExtractMode

The data extract mode which defines how the data is extracted from the source bitmap.



The property DataExtractMode accepts the following values:

* __SinglePixelTopLeft__
* __SinglePixelCenter__
* __BlendPixels__


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_InactiveColor InactiveColor

The inactive color.



__Nested Properties__

The following nested propteries exist for InactiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect RGBAMatrixColorScaleBitmapEffect

\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_summary Summary

The RGBAMatrixBitmapEffect displays a defined part of a bitmap in the given colors on a area of a RGBAtoy Matrix. The effect take the overall brightness of the pixels of the bitmap to control the brightness of the specified colors for each pixel.

The properties of the effect allow you to select the part of the bitmap to display as well as the area of the matrix on which the bitmap is displayed. Depending on the size of your bitmap you might choose different modes for the image extraction.

The effect supports numerous image formats, inluding png, gif (also animated) and jpg.

The image extraction takes place upon initalization of the framework. While the framework is active, it only outputs the previously extracted and scaled data to allow for better performance.



\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_samplexml Sample XML

A configuration section for RGBAMatrixColorScaleBitmapEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAMatrixColorScaleBitmapEffect>
  <Name>Name of RGBAMatrixColorScaleBitmapEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <FadeMode>Fade</FadeMode>
  <BitmapFrameNumber>0</BitmapFrameNumber>
  <BitmapTop>0</BitmapTop>
  <BitmapLeft>0</BitmapLeft>
  <BitmapWidth>-1</BitmapWidth>
  <BitmapHeight>-1</BitmapHeight>
  <DataExtractMode>BlendPixels</DataExtractMode>
  <BitmapFilePattern>Pattern string</BitmapFilePattern>
  <ActiveColor>
    <HexColor>#00000000</HexColor>
  </ActiveColor>
  <InactiveColor>
    <HexColor>#00000000</HexColor>
  </InactiveColor>
</RGBAMatrixColorScaleBitmapEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_properties Properties

RGBAMatrixColorScaleBitmapEffect has the following 17 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_ActiveColor ActiveColor

The active color.



__Nested Properties__

The following nested propteries exist for ActiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_BitmapFilePattern BitmapFilePattern

The bitmap file pattern which is used to load the bitmap file for the effect.



__Nested Properties__

The following nested propteries exist for BitmapFilePattern:
* __Pattern__<br/>  The pattern used to look for files.




\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_BitmapFrameNumber BitmapFrameNumber

The number of the frame to be used (for animated gifs).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_BitmapHeight BitmapHeight

The height of the the part of the bitmap which is to be used.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_BitmapLeft BitmapLeft

The left boundary of the the part of the bitmap which is to be used.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_BitmapTop BitmapTop

The top of the the part of the bitmap which is to be used.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_BitmapWidth BitmapWidth

The width of the the part of the bitmap which is to be used.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_DataExtractMode DataExtractMode

The data extract mode which defines how the data is extracted from the source bitmap.



The property DataExtractMode accepts the following values:

* __SinglePixelTopLeft__
* __SinglePixelCenter__
* __BlendPixels__


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_InactiveColor InactiveColor

The inactive color.



__Nested Properties__

The following nested propteries exist for InactiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect RGBAMatrixColorScaleShapeEffect

\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_summary Summary

Displays a shape on a RGBA matrix (typically a ledstrip array). The color of the displayed shape is controlled by the effect.



\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_samplexml Sample XML

A configuration section for RGBAMatrixColorScaleShapeEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAMatrixColorScaleShapeEffect>
  <Name>Name of RGBAMatrixColorScaleShapeEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <FadeMode>Fade</FadeMode>
  <ActiveColor>
    <HexColor>#00000000</HexColor>
  </ActiveColor>
  <InactiveColor>
    <HexColor>#00000000</HexColor>
  </InactiveColor>
  <ShapeName>Name of Shape</ShapeName>
</RGBAMatrixColorScaleShapeEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_properties Properties

RGBAMatrixColorScaleShapeEffect has the following 11 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_ActiveColor ActiveColor

The active color.



__Nested Properties__

The following nested propteries exist for ActiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_InactiveColor InactiveColor

The inactive color.



__Nested Properties__

The following nested propteries exist for InactiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_ShapeName ShapeName

The name of the shape.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect RGBAMatrixFlickerEffect

\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_summary Summary

Does create random flickering with a defineable density, durations and color within the spefied area of a ledstrip.



\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_samplexml Sample XML

A configuration section for RGBAMatrixFlickerEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAMatrixFlickerEffect>
  <Name>Name of RGBAMatrixFlickerEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <FadeMode>Fade</FadeMode>
  <Density>10</Density>
  <MinFlickerDurationMs>60</MinFlickerDurationMs>
  <MaxFlickerDurationMs>150</MaxFlickerDurationMs>
  <FlickerFadeUpDurationMs>0</FlickerFadeUpDurationMs>
  <FlickerFadeDownDurationMs>0</FlickerFadeDownDurationMs>
  <ActiveColor>
    <HexColor>#00000000</HexColor>
  </ActiveColor>
  <InactiveColor>
    <HexColor>#00000000</HexColor>
  </InactiveColor>
</RGBAMatrixFlickerEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_properties Properties

RGBAMatrixFlickerEffect has the following 15 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_ActiveColor ActiveColor

The active color.



__Nested Properties__

The following nested propteries exist for ActiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_Density Density

The density if the flickering in percent.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_FlickerFadeDownDurationMs FlickerFadeDownDurationMs

The fade down duration in milliseconds for a single flicker/blink of a element.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_FlickerFadeUpDurationMs FlickerFadeUpDurationMs

The fade up duration in milliseconds for a single flicker/blink of a element.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_InactiveColor InactiveColor

The inactive color.



__Nested Properties__

The following nested propteries exist for InactiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_MaxFlickerDurationMs MaxFlickerDurationMs

The max duration in milliseconds for a single flicker/blink of a element.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_MinFlickerDurationMs MinFlickerDurationMs

The min duration in milliseconds for a single flicker/blink of a element.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect RGBAMatrixPlasmaEffect

\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_summary Summary

Displayes a classical plasma effect on a RGBA matrix/ledstrip array.
For more details on the math of the plasma effect, read the following page: http://www.bidouille.org/prog/plasma



\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_samplexml Sample XML

A configuration section for RGBAMatrixPlasmaEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAMatrixPlasmaEffect>
  <Name>Name of RGBAMatrixPlasmaEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <FadeMode>Fade</FadeMode>
  <PlasmaSpeed>100</PlasmaSpeed>
  <PlasmaScale>100</PlasmaScale>
  <PlasmaDensity>100</PlasmaDensity>
  <ActiveColor1>
    <HexColor>#00000000</HexColor>
  </ActiveColor1>
  <ActiveColor2>
    <HexColor>#00000000</HexColor>
  </ActiveColor2>
  <InactiveColor>
    <HexColor>#00000000</HexColor>
  </InactiveColor>
</RGBAMatrixPlasmaEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_properties Properties

RGBAMatrixPlasmaEffect has the following 14 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_ActiveColor1 ActiveColor1

The active color.



__Nested Properties__

The following nested propteries exist for ActiveColor1:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_ActiveColor2 ActiveColor2

The active color 2.



__Nested Properties__

The following nested propteries exist for ActiveColor2:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_InactiveColor InactiveColor

The inactive color.



__Nested Properties__

The following nested propteries exist for InactiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_PlasmaDensity PlasmaDensity

The plasma density.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_PlasmaScale PlasmaScale

The plasma scale.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_PlasmaSpeed PlasmaSpeed

The plasma speed.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect RGBAMatrixShapeEffect

\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_summary Summary

Displays a shape on a RGBA matrix (typically a ledstrip matrix). The color of the displayed shape is the org color of the shape (multicolor shapes work as well).



\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_samplexml Sample XML

A configuration section for RGBAMatrixShapeEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAMatrixShapeEffect>
  <Name>Name of RGBAMatrixShapeEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <FadeMode>Fade</FadeMode>
  <ShapeName>Name of Shape</ShapeName>
</RGBAMatrixShapeEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_properties Properties

RGBAMatrixShapeEffect has the following 9 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_ShapeName ShapeName

The name of the shape.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect RGBAMatrixShiftEffect

\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_samplexml Sample XML

A configuration section for RGBAMatrixShiftEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<RGBAMatrixShiftEffect>
  <Name>Name of RGBAMatrixShiftEffect</Name>
  <ToyName>Name of Toy</ToyName>
  <Width>100</Width>
  <Height>100</Height>
  <Left>0</Left>
  <Top>0</Top>
  <LayerNr>0</LayerNr>
  <FadeMode>Fade</FadeMode>
  <ShiftDirection>Right</ShiftDirection>
  <ShiftSpeed>200</ShiftSpeed>
  <ShiftAcceleration>0</ShiftAcceleration>
  <ActiveColor>
    <HexColor>#00000000</HexColor>
  </ActiveColor>
  <InactiveColor>
    <HexColor>#00000000</HexColor>
  </InactiveColor>
</RGBAMatrixShiftEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_properties Properties

RGBAMatrixShiftEffect has the following 13 configurable properties:

\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_ActiveColor ActiveColor

The active color.



__Nested Properties__

The following nested propteries exist for ActiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_FadeMode FadeMode

Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values &gt;0, otherwise inactive value/color will be used).



The property FadeMode accepts the following values:

* __Fade__: Fading is enabled.
* __OnOff__: No fading. There will be a simple on/off behaviour depending on the triggering value.


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_Height Height

The height in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_InactiveColor InactiveColor

The inactive color.



__Nested Properties__

The following nested propteries exist for InactiveColor:
* __HexColor__<br/>  6 digit hexadecimal color code with leading  #(e.g. #ff0000 for red).


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_LayerNr LayerNr

The number of the target layer for the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_Left Left

The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_ShiftAcceleration ShiftAcceleration

The acceleration for the shift speed in percent of the effect area per second.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_ShiftDirection ShiftDirection

The shift direction (Left, Right, Up, Down).



The property ShiftDirection accepts the following values:

* __Down__: Shift down
* __Left__: Shift left
* __Right__: Shift right
* __Up__: Shift up


\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_ShiftSpeed ShiftSpeed

The shift speed in percentage of the effect area (Left, Top, Width, Height properties) per second .



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_Top Top

The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_ToyName ToyName

The name of the toy which is controlled by the effect.



\subsubsection DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_Width Width

The width in percent of the target area for the effect (0-100).



\section use_DirectOutput_FX_ConditionFX_TableElementConditionEffect TableElementConditionEffect

\subsection use_DirectOutput_FX_ConditionFX_TableElementConditionEffect_summary Summary

This effect evaluates the condition specified in the Condition property.



\subsection use_DirectOutput_FX_ConditionFX_TableElementConditionEffect_samplexml Sample XML

A configuration section for TableElementConditionEffect might resemble the following structure:

~~~~~~~~~~~~~{.xml}
<TableElementConditionEffect>
  <Name>Name of TableElementConditionEffect</Name>
  <TargetEffectName>Name of TargetEffect</TargetEffectName>
  <Condition>Condition string</Condition>
</TableElementConditionEffect>
~~~~~~~~~~~~~
\subsection use_DirectOutput_FX_ConditionFX_TableElementConditionEffect_properties Properties

TableElementConditionEffect has the following 3 configurable properties:

\subsubsection DirectOutput_FX_ConditionFX_TableElementConditionEffect_Condition Condition

The condition.



\subsubsection DirectOutput_FX_ConditionFX_TableElementConditionEffect_Name Name

The name of the item.



\subsubsection DirectOutput_FX_ConditionFX_TableElementConditionEffect_TargetEffectName TargetEffectName

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



