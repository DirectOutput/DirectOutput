Table configuration {#tableconfig}
===================

\section tableconfig_introduction Introduction

The table configuration defines the table elements (e.g. solenoids or lamps), the configuration of the effects and the assigment of the effects to the table elements. The assigned effects are triggered if the value of a table element changes (typicaly when new data from Pinmame is received).

In addition there is also a collection of static effects which are triggered when the framework is starting up. Use the static effects to control toys which dont have to change their state during game play (e.g. set illuminated flipper button to a specific color).

\section tableconfig_configfilestructure Table config file structure

The basic structure of a table configuration file contains the following sections:

* __Effects__ configures all effects for a table. These effects can be assigned to table elements or to the static effects. 
* __TableElements__ defines the table elements with their type (e.g. solenoid or lamp) and number and also the assignment of effects to the table elements. 
* __AssignedStaticEffects__ defines the static effects for the table.
* __TableName__ contains the name of the pinball table.

The basic structure of a table configuration looks as follows:

~~~~~~~~~~~~~{.xml}
<?xml version="1.0"?>
<Table xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <TableName>Name of the table</TableName>
  <TableElements>
      ... Table element definitions ...
  </TableElements>
  <Effects>
       ... Effects configuration ...
  </Effects>
  <AssignedStaticEffects>
       ... Static effects assignments ...
  </AssignedStaticEffects>
</Table>
~~~~~~~~~~~~~

\subsection tableconfig_configfilestructuretableelements Table elements section

The TableElements section contains the definition of the table elements (solenoids, lamps and so on) of the pinball table. Table elements are configured using TableElement sections. A TableElements secation can contain any number of TableElement sections.

The definition of a table element consists of the type and the number of the table element. A name for the element can be specified in addition. The name has no technical relevance, but can be useful for informational purposes and configuring a table will for sure be easier if the table elements are named.

For every table element one or several effects can be assigned. A effect assignment consists of the name of the effect and a number which defines the order in which the effects are triggered. If the order of the effects doesnt matter, you can also use the same order number for every assigned effect.

On initialization of the framework the names of the assigned effects are resolved into references to the actual effect objects. If a name cant be resolved, the effect assignmenet will be disabled.

A typical table element definition looks as follows:

~~~~~~~~~~~~~{.xml}
    <TableElement>
      <TableElementType>Solenoid</TableElementType>
      <Number>4</Number>
      <Name>Right slingshot</Name>
      <AssignedEffects>
        <AssignedEffectOrder>
          <EffectName>Fire Right Bottom Contactor</EffectName>
          <Order>1</Order>
        </AssignedEffectOrder>
        <AssignedEffectOrder>
          <EffectName>Flash Right Backboard Led</EffectName>
          <Order>2</Order>
        </AssignedEffectOrder>
      </AssignedEffects>
    </TableElement>
~~~~~~~~~~~~~

\subsection tableconfig_configfilestructurestaticeffects Assigned static effects section

The AssignedStaticEffects contains the assignment of the static effects which are triggered when the system starts up. The static effects are defined by using AssignedEffectOrder sections. 

The AssignedEffectOrder sections consist of the name of the assigned effect and a number specifying the order in which the static effects are triggered.

On initialization of the framework the names of the assigned effects are resolved into references to the actual effect objects. If a name cant be resolved, the effect assignmenet will be disabled.

A typical AssignedEffectOrder might looks like this:

~~~~~~~~~~~~~{.xml}
    <AssignedEffectOrder>
      <EffectName>Set Flipperbutton to Red</EffectName>
      <Order>1</Order>
    </AssignedEffectOrder>
~~~~~~~~~~~~~


\subsection tableconfig_configfilestructureeffects Effects section

The effects section contains the configurations for all table effects. Effects configured in this section can be referenced by their name in the static effects section and in the assigned effect sections of the various table elements.

The Directoutput framework contains some built in effects and more effects can be added through scripting (see scripting page for details). 

At the time of writting the following built in effects exist in the framewwork:

\subsubsection tableconfig_configfilestructureeffectsbasicanalogeffect BasicAnalogEffect

The BasicAnalogEffect can be used to control toys which are using analog outputs (e.g. lamps). These toys have to implement the IAnalogToy interface.

This effect will set the value of the assigned toy to the values defined in ValueOn and ValueOff depending on the value of the table element triggering the effect. Using the effect as a static effect is also possible. The effect will set the toys value to ValueOn if it as called as a static effect.

A typical config for this effect type might looks as follows:

<BasicAnalogEffect>
  <Name>Effect name</Name>
  <AnalogToyName>Name of a analog toy</AnalogToyName>
  <ValueOn>255</ValueOn>
  <ValueOff>0</ValueOff>
</BasicAnalogEffect>

* __Name__ is the name of the effect. What else?
* __AnalogToyName__ is the name of toy a implementing the IAnalogToy interface. The toy has to be configured in the cabinet.
* __ValueOn__ is the value which will be set for the toy, if the effect is triggered by a table element having a value which is not equal 0. The value will also be used the set the value of the referenced toy, if the effect is assigned as a static effect. 
* __ValueOff__ is the value which will be set for the toy, if the effect is triggered by a table element having a value of 0.

\subsubsection tableconfig_configfilestructureeffectsbasicdigitaleffect BasicDigitalEffect

The BasicDigitalEffect is used to control toy which are using digital outputs (e.g. contactors). The toys have to implement the IDigitalToy interface.

This effect sets the state of a toy implementing the IDigitalToy interface depending on the value of the table element triggering the effect. A table element value of 0, will set the toy state to false/off, any other value will set the toys state to true/on. If assigned as a static effect, the state of the toy will always be set to true/on.

Here is a typical BasicDigitalEffect configuration:

<BasicDigitalEffect>
  <Name>Effect name</Name>
  <DigitalToyName>Name of a toy</DigitalToyName>
</BasicDigitalEffect>

* __Name__ is as usual the name of the effect.
* __DigitalToyName__ is the name of the assigned toy. The assigned toy has be configured in the cabinet and must to implement the IDigitalToy interface.

\subsubsection tableconfig_configfilestructureeffectsbasicrgbeffect BasicRGBEffect

The BasicRGBEffect is used to control RGBLeds or any other toy implementing the IRGBToy interface. Depending on the vaölue of the table element triggering the effect, the IRGBToy will be set to the configured color or to off. A table element value of 0 will turn of the IRGBToy, a value not equal 0 will set the toy to the specified color.

If the effect is assigned as a static effect it will set the assigned toy to to specified color.

<BasicRGBEffect>
  <Name>Effect name</Name>
  <RGBToyName>RGBLedName value</RGBToyName>
  <Color>Color value (e.g. #00ff00 for blue)</Color>
</BasicRGBEffect>

* __Name__ of the effect. 
* __RGBToyName__ is the name of the assigned RGB toy. This toy must implement the IRGBToy interface.
* __Color__  defines color which is set for the toy, if table element triggering the effect has a value which not equal 0. The color can be specified as a hexadecimal color definition (e.g. \#ff0000 for red), comma separated color (e.g. 0,255,0 for green) or color name as defined in the cabinet colors section.

\subsubsection tableconfig_configfilestructureeffectslisteffect ListEffect

The ListEffect is a effect which triggers a list of other effects assigned to the list effect. The data of the table element which has triggered the effect is passed on to the effects assigned to the list effect.
Used as a static effect the ListEffect will trigger all assigned effect as well.


<ListEffect>
  <Name>Effect name</Name>
  <AssignedEffects>
    <AssignedEffectOrder>
      <EffectName>Name of the first effect</EffectName>
      <Order>1</Order>
    </AssignedEffectOrder>
    <AssignedEffectOrder>
      <EffectName>Name of a second effect</EffectName>
      <Order>2</Order>
    </AssignedEffectOrder>
  </AssignedEffects>
</ListEffect>

* __Name__ is the name of the effect. Hard to guess?
* __AssignedEffects__ section contains any number of ordered effect assignments.
* __EffectName__ contains the name of the assigned effect.
* __Order__ is a number specifying the order in which the assigned effects are executed.

