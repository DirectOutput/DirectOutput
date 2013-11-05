Cabinet Configuration {#cabinetconfig}
=====================

\warning This page is not fully up to date! 

\section cabinetconfig_introduction Introduction

The Cabinet configuration defines the parts of a virtual pinball cabinet. The parts of the cabinet are divided into the following main areas:

* __Output controllers__ are the units which connects the computer in the cabinet to the installed toys. A typical example of a output controller is the LedWiz. 
* __Toys__ are all the fun gadgets like contactors, lamps, RGB-leds and shakers which are installed in a cabinet to make the gaming experience more realistic.
* __Colors__ are color definitions which allow toys that can display colors (e.g. RGB-Leds) to use named colors insted of numeric color representations.


\section cabinetconfig_configfile Cabinet configuration file

The configuration file for a cabinet defines the installed output controllers, their outputs and the installed toys. 

Cabinet configuration files are XML files with the following basic structure:

~~~~~~~~~~~~~{.xml}
<?xml version="1.0"?>
<Cabinet xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Name>Name of the cabinet</Name>
  <OutputControllers>
     ... configuration of any number of output controllers ...
  </OutputControllers}
  <Toys>
     ... configuration of any number toys ...
  </Toys>
 <Colors>
     ... any number of color definitions ...
  </Colors>
</Cabinet>

~~~~~~~~~~~~~

\subsection cabinetconfig_outputcontrollers OutputControllers section

The OutputControllers section contains the configurations for the output controllers installed in the cabinet. The required tags and values depend on the type of output controller used. The minimal structure for every type of output controller is as follows:

~~~~~~~~~~~~~{.xml}
<TypeOfOutputController>
  <Name>Unique name of the output controller</Name>
  ... more output controller specific configuration tags ...
  <Outputs>
     ... configuration of output of the output controller ....
  </Outputs>
</TypeOfOutputController>

~~~~~~~~~~~~~

Output controllers might require additional tags to configure their behaviour. For a LedWiz the basic structure for the output controller configuration looks as follows:

~~~~~~~~~~~~~{.xml}
<LedWiz>
  <Name>LedWiz 01</Name>
  <Number>1</Number>
  <Outputs>
     ... configuration of outputs of the output controller ....
  </Outputs>
</LedWiz>
~~~~~~~~~~~~~

The __Outputs__ section of the output controller configuration defines the outputs for the output controller. The required tags and structure depend on the type of output used by the output controller. The minimal structure for a output configuration looks as follows:

~~~~~~~~~~~~~{.xml}
<TypeOfOutput>
	<Name>Unique name of the output</Name>
	... more output specific configuration tags ...
</TypeOfOutput>
~~~~~~~~~~~~~
This structure is repeated for every output of the output controller.

Typically outputs will require additional tags for a proper configuration.	For the outputs of a LedWiz this structure would contain the following:

~~~~~~~~~~~~~{.xml}
<LedWizOutput>
   <Name>LedWizOutput 01.01</Name>
   <LedWizOutputNumber>1</LedWizOutputNumber>
</LedWizOutput>
~~~~~~~~~~~~~

\subsection cabinetconfig_toys Toys section

The toys section configures the gadgets installed in a cabinet. The structure and tags required depend on the type of toy beeing configured. A toy configuration will at least have the follwing mininmal structure:

~~~~~~~~~~~~~{.xml}
<TypeOfTheToy>
   <Name>Unique name of the toy</Name>
   ... more toy specific configuration tags ....
</TypeOfTheToy>
~~~~~~~~~~~~~

In addition to the minimal tags in the example above, a typical toy will have at least one tag defining the name of the output to which the toy is connected. For a toy of type Contactor this looks as follows:

~~~~~~~~~~~~~{.xml}
<Contactor>
   <Name>Unique name of the Contactor</Name>
   <OutputName>Name of the output </OutputName>
</Contactor>
~~~~~~~~~~~~~


\section cabinetconfig_example Example cabinet configuration

The following xml is the config for a cabinet with 1 LedWiz unis, 10 contactors, 1 replay knocker, 5 lamps, 5 RGB-leds and one shaker installed.
In addition 1 LedWizEquivalent toy is configured to allow the system, to use legacy ledcontrol.ini files.

~~~~~~~~~~~~~{.xml}
  
<?xml version="1.0"?>
<Cabinet xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Name>Lizard Pin</Name>

  <!-- The following section contains the config for output controllers -->
  <OutputControllers>
    <LedWiz>
      <Name>LedWiz 01</Name>
      <Number>1</Number>
    </LedWiz>
  </OutputControllers>

  <!-- The following section contains the config for toys -->
  <Toys>
    <Contactor>
      <Name>Contactor Top Left</Name>
      <OutputName>LedWiz 01.05</OutputName>
    </Contactor>
    <Contactor>
      <Name>Contactor Top Center</Name>
      <OutputName>LedWiz 01.10</OutputName>
    </Contactor>
    <Contactor>
      <Name>Contactor Top Right</Name>
      <OutputName>LedWiz 01.09</OutputName>
    </Contactor>
    <Contactor>
      <Name>Contactor Middle Left</Name>
      <OutputName>LedWiz 01.03</OutputName>
    </Contactor>
    <Contactor>
      <Name>Contactor Middle Center</Name>
      <OutputName>LedWiz 01.04</OutputName>
    </Contactor>
    <Contactor>
      <Name>Contactor Middle Right</Name>
      <OutputName>LedWiz 01.08</OutputName>
    </Contactor>
    <Contactor>
      <Name>Contactor Slingshot Left</Name>
      <OutputName>LedWiz 01.02</OutputName>
    </Contactor>
    <Contactor>
      <Name>Contactor Slingshot Right</Name>
      <OutputName>LedWiz 01.07</OutputName>
    </Contactor>
    <Contactor>
      <Name>Contactor Flipper Left</Name>
      <OutputName>LedWiz 01.01</OutputName>
    </Contactor>
    <Contactor>
      <Name>Contactor Flipper Right</Name>
      <OutputName>LedWiz 01.07</OutputName>
    </Contactor>
    <ReplayKnocker>
      <Name>ReplayKnocker</Name>
      <OutputName>LedWiz 01.11</OutputName>
    </ReplayKnocker>
    <Lamp>
      <Name>Start Button</Name>
      <OutputName>LedWiz 01.12</OutputName>
    </Lamp>
    <Lamp>
      <Name>Extra Ball Button</Name>
      <OutputName>LedWiz 01.13</OutputName>
    </Lamp>
    <Lamp>
      <Name>Exit Button</Name>
      <OutputName>LedWiz 01.14</OutputName>
    </Lamp>
    <Lamp>
      <Name>Launch Ball Button</Name>
      <OutputName>LedWiz 01.15</OutputName>
    </Lamp>
    <Lamp>
      <Name>Coin Door</Name>
      <OutputName>LedWiz 01.16</OutputName>
    </Lamp>
    <RGBLed>
      <Name>Backboard Left</Name>
      <OutputNameRed>LedWiz 01.17</OutputNameRed>
      <OutputNameGreen>LedWiz 01.19</OutputNameGreen>
      <OutputNameBlue>LedWiz 01.18</OutputNameBlue>
    </RGBLed>
    <RGBLed>
      <Name>Backboard Center Left</Name>
      <OutputNameRed>LedWiz 01.20</OutputNameRed>
      <OutputNameGreen>LedWiz 01.22</OutputNameGreen>
      <OutputNameBlue>LedWiz 01.21</OutputNameBlue>
    </RGBLed>
    <RGBLed>
      <Name>Backboard Center</Name>
      <OutputNameRed>LedWiz 01.23</OutputNameRed>
      <OutputNameGreen>LedWiz 01.25</OutputNameGreen>
      <OutputNameBlue>LedWiz 01.24</OutputNameBlue>
    </RGBLed>
    <RGBLed>
      <Name>Backboard Center Right</Name>
      <OutputNameRed>LedWiz 01.26</OutputNameRed>
      <OutputNameGreen>LedWiz 01.28</OutputNameGreen>
      <OutputNameBlue>LedWiz 01.27</OutputNameBlue>
    </RGBLed>
    <RGBLed>
      <Name>Backboard Right</Name>
      <OutputNameRed>LedWiz 01.29</OutputNameRed>
      <OutputNameGreen>LedWiz 01.31</OutputNameGreen>
      <OutputNameBlue>LedWiz 01.30</OutputNameBlue>
    </RGBLed>
    <Shaker>
      <Name>Shaker</Name>
      <OutputName>LedWiz 01.31</OutputName>
    </Shaker>

	  <!-- The LedWizEquivalent toy is used when a legacy LedControl.ini file is loaded.   -->
    <LEDWizEquivalent>
      <Name>LedWizEquivalent 1</Name>
      <Outputs>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.01</OutputName>
          <LedWizEquivalentOutputNumber>1</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.02</OutputName>
          <LedWizEquivalentOutputNumber>2</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.03</OutputName>
          <LedWizEquivalentOutputNumber>3</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.04</OutputName>
          <LedWizEquivalentOutputNumber>4</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.05</OutputName>
          <LedWizEquivalentOutputNumber>5</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.06</OutputName>
          <LedWizEquivalentOutputNumber>6</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.07</OutputName>
          <LedWizEquivalentOutputNumber>7</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.08</OutputName>
          <LedWizEquivalentOutputNumber>8</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.09</OutputName>
          <LedWizEquivalentOutputNumber>9</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.10</OutputName>
          <LedWizEquivalentOutputNumber>10</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.11</OutputName>
          <LedWizEquivalentOutputNumber>11</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.12</OutputName>
          <LedWizEquivalentOutputNumber>12</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.13</OutputName>
          <LedWizEquivalentOutputNumber>13</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.14</OutputName>
          <LedWizEquivalentOutputNumber>14</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.15</OutputName>
          <LedWizEquivalentOutputNumber>15</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.16</OutputName>
          <LedWizEquivalentOutputNumber>16</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.17</OutputName>
          <LedWizEquivalentOutputNumber>17</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.18</OutputName>
          <LedWizEquivalentOutputNumber>18</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.19</OutputName>
          <LedWizEquivalentOutputNumber>19</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.20</OutputName>
          <LedWizEquivalentOutputNumber>20</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.21</OutputName>
          <LedWizEquivalentOutputNumber>21</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.22</OutputName>
          <LedWizEquivalentOutputNumber>22</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.23</OutputName>
          <LedWizEquivalentOutputNumber>23</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.24</OutputName>
          <LedWizEquivalentOutputNumber>24</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.25</OutputName>
          <LedWizEquivalentOutputNumber>25</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.26</OutputName>
          <LedWizEquivalentOutputNumber>26</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.27</OutputName>
          <LedWizEquivalentOutputNumber>27</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.28</OutputName>
          <LedWizEquivalentOutputNumber>28</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.29</OutputName>
          <LedWizEquivalentOutputNumber>29</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.30</OutputName>
          <LedWizEquivalentOutputNumber>30</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.31</OutputName>
          <LedWizEquivalentOutputNumber>31</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
        <LEDWizEquivalentOutput>
          <OutputName>LedWiz 01\LedWiz 01.32</OutputName>
          <LedWizEquivalentOutputNumber>32</LedWizEquivalentOutputNumber>
        </LEDWizEquivalentOutput>
      </Outputs>
      <LedWizNumber>1</LedWizNumber>
    </LEDWizEquivalent>
  </Toys>
  <Effects />

  <!-- The following section contains the config for colors -->
  <Colors>
    <Color>
      <Name>Black</Name>
      <HexColor>#000000</HexColor>
    </Color>
    <Color>
      <Name>Red</Name>
      <HexColor>#FF0000</HexColor>
    </Color>
    <Color>
      <Name>Green</Name>
      <HexColor>#0000FF</HexColor>
    </Color>
    <Color>
      <Name>Blue</Name>
      <HexColor>#00FF00</HexColor>
    </Color>
    <Color>
      <Name>White</Name>
      <HexColor>#FFFFFF</HexColor>
    </Color>
    <Color>
      <Name>Yellow</Name>
      <HexColor>#FF00FF</HexColor>
    </Color>
    <Color>
      <Name>Orange</Name>
      <HexColor>#FF0080</HexColor>
    </Color>
    <Color>
      <Name>Cyan</Name>
      <HexColor>#00FFFF</HexColor>
    </Color>
    <Color>
      <Name>Purple</Name>
      <HexColor>#40FF00</HexColor>
    </Color>
    <Color>
      <Name>Violet</Name>
      <HexColor>#80FF00</HexColor>
    </Color>
    <Color>
      <Name>Magenta</Name>
      <HexColor>#FFFF00</HexColor>
    </Color>
  </Colors>
</Cabinet>
~~~~~~~~~~~~~