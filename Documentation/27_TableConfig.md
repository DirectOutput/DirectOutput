Table configuration file {#tableconfigfile}
========================

\section tableconfigfile_introduction Introduction

The table configuration defines the table elements (e.g. solenoids or lamps), the configuration of the effects and the assigment of the effects to the table elements. The assigned effects are triggered if the value of a table element changes (typicaly when new data from Pinmame is received).

In addition there is also a collection of static effects which are triggered when the framework is starting up. Use the static effects to control toys which dont have to change their state during game play (e.g. set illuminated flipper button to a specific color).

The framework does also allow to mix the configuration in a table config file with configurations loaded from ledcontrol files. This will allow you to use configurations from the <a href="http://vpuniverse.com/ledwiz/login.php">DOF Config Tool</a> for commonly used gadgets and to supplement this with your own config. 

\section tableconfigfile_configfilestructure Table config file 

Table configurations are stored as XML-files. The basic structure of a table configuration file contains the following sections:

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

  <AddLedControlConfig>true/false</AddLedControlConfig>

</Table>
~~~~~~~~~~~~~

\subsection tableconfigfile_configfilestructuretableelements Table elements section

The TableElements section contains the definition of the table elements (solenoids, lamps and so on) of the pinball table. Table elements are configured using TableElement sections. A TableElements secation can contain any number of TableElement sections.

The definition of a table element consists of the type and the number of the table element. A name for the element can be specified in addition. The name has no technical relevance, but can be useful for informational purposes and configuring a table will for sure be easier if the table elements are named.

For every table element one or several effects can be assigned.

On initialization of the framework the names of the assigned effects are resolved into references to the actual effect objects. If a name cant be resolved, the effect assignmenet will be disabled.

A typical table element definition looks as follows:

~~~~~~~~~~~~~{.xml}
    <TableElement>
      <TableElementType>Solenoid</TableElementType>
      <Number>4</Number>
      <Name>Right slingshot</Name>
      <AssignedEffects>
        <AssignedEffect>
          <EffectName>Fire Right Bottom Contactor</EffectName>
        </AssignedEffect>
        <AssignedEffect>
          <EffectName>Flash Right Backboard Led</EffectName>
        </AssignedEffect>
      </AssignedEffects>
    </TableElement>
~~~~~~~~~~~~~

\subsection tableconfigfile_configfilestructurestaticeffects Assigned static effects section

The AssignedStaticEffects contains the assignment of the static effects which are triggered when the system starts up. The static effects are defined by using AssignedEffect sections. 

On initialization of the framework the names of the assigned effects are resolved into references to the actual effect objects. If a name cant be resolved, the effect assignmenet will be disabled.

This section might looks like this:
  <AssignedStaticEffects>
        <AssignedEffect>
          <EffectName>Turn on Start Button</EffectName>
        </AssignedEffect>
        <AssignedEffect>
          <EffectName>Turn on Launch Button</EffectName>
        </AssignedEffect>
      </AssignedEffects>
  </AssignedStaticEffects>


\subsection tableconfigfile_configfilestructureeffects Effects section

The effects section contains the configurations for all table effects. Effects configured in this section can be referenced by their name in the static effects section and in the assigned effect sections of the various table elements.

Please refer to the pages on \ref fx "Effects" and on \ref fx_builtin "Built in Effects" for more information.