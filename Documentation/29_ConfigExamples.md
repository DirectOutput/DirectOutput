Cabinet Configuration examples {#configexamples}
========================

\section configexamples_introduction Introduction

Below you find a number of example cabinet configurations. Those configs are mostly rather simlpe and do only configure one output controller. If your hardware is more comlex you might need to mix content of several examples.

If you need more information on the different types of output controllers and toys, please read the following pages \ref outputcontrollers_builtin and \ref toy_builtin 


\section configexamples_examples Config Examples/Templates

This section contains configuration examples and templates.

\subsection configexamples_teensystripcontroller TeensyStripController example

This configures 1 TeensyStripController plus ledstrips for playfield back, playfield left and playfield right. Read the comments in the config to get a idea which values you have to replace, so the config work for your hardware.

~~~~~~~~~~~~~{.xml}
<?xml version="1.0"?>
<Cabinet xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">

  <!-- The name of your cabinet. The settings has no technical relevance. -->
  <Name>Lizard Pin</Name>

  <!-- The following section contains the definitions of the output controllers you have installed in your cab.
  Depending on the hardware in your cab the section can also contain additional definitions for other output controllers (e.g. Artnet).
  Some types of output controllers (e.g. Ledwiz) are automatically detected and dont need to be defined (it is still possible to defined them if you like).
  -->
  <OutputControllers>

    <!-- This defines all settings for the TeensyStripController- -->
    <TeensyStripController>
      <!-- Name for the TeensyStripController. DOF uses this name for all references to the controller. 
      Make sure you reference the correct controller name in the toy definitins below. -->
      <Name>TeensyStripController</Name>

      <!-- The number of leds connected to each of the 8 channels of the TeensyStripController. 
    DOF treats all 8 channels like 1 long ledstrip. -->
      <NumberOfLedsStrip1>Number of Leds on 1. output of the Teensy</NumberOfLedsStrip1>
      <NumberOfLedsStrip2>Number of Leds on 2. output of the Teensy</NumberOfLedsStrip2>
      <NumberOfLedsStrip3>Number of Leds on 3. output of the Teensy</NumberOfLedsStrip3>
      <NumberOfLedsStrip4>Number of Leds on 4. output of the Teensy</NumberOfLedsStrip4>
      <NumberOfLedsStrip5>Number of Leds on 5. output of the Teensy</NumberOfLedsStrip5>
      <NumberOfLedsStrip6>Number of Leds on 6. output of the Teensy</NumberOfLedsStrip6>
      <NumberOfLedsStrip7>Number of Leds on 7. output of the Teensy</NumberOfLedsStrip7>
      <NumberOfLedsStrip8>Number of Leds on 8. output of the Teensy</NumberOfLedsStrip8>
      <!-- The TeensyStripController appears to the system as a virtual com port. Specify the name of the virtual com port (e.g. COM15) in the next setting. -->
      <ComPortName>Name of the virtual com port</ComPortName>
    </TeensyStripController>
  </OutputControllers>

  <!-- The Toys section caontains the definitions of the toys in your cab. Depending on the hardware you have you might have additional toy definitions in this section.
  Ledstrip and area toys have to be explicitly defined. Many other toy types (e.g. RGB leds) are automatically configured based on the infomation found in the ini files containg the table configs (but you can define those toys explicitly as well for more detailed control of the setup). 
  -->
  <Toys>

    <!-- Each ledstrip matrix (DOF treats all leds strips as matrices) needs its own LedStrip section.-->
    <LedStrip>
      <!-- The name for the LedStrip toy. This name can be anything you like. Just make sure you reference the correct toy name in the LedWizEquivalent toy definition below. -->
      <Name>PF Back</Name>
      <Width>Number of leds in horizontal direction for the PF back</Width>
      <Height>Number of leds in vertical direction for the PF back</Height>
      <LedStripArrangement>LeftRightTopDown (check the DOF docu for other allowed values)</LedStripArrangement>
      <!-- WS2812 leds use green, red, blue instead of the classical red, green, blue color order. If you use other types of leds all combination R,G, B are valid for the next setting. -->
      <ColorOrder>GRB ()</ColorOrder>
      <!-- Defines the number of the first leds for the strip (DOF treats all 8 channels of the Teensy controller as 1 strip). -->
      <FirstLedNumber>1</FirstLedNumber>
      <!-- DOF supports quite a few other fading curves as well (e.g. Linear). Check the DOF docu for details.-->
      <FadingCurveName>SwissLizardsLedCurve</FadingCurveName>
      <!-- Name of the TeensyStripController as defined above. -->
      <OutputControllerName>TeensyStripController</OutputControllerName>
    </LedStrip>

    <!-- Check comments above for details on the settings -->
    <LedStrip>
      <Name>PF Right</Name>
      <Width>1</Width>
      <Height>65</Height>
      <LedStripArrangement>TopDownLeftRight</LedStripArrangement>
      <ColorOrder>GRB</ColorOrder>
      <FirstLedNumber>97</FirstLedNumber>
      <FadingCurveName>SwissLizardsLedCurve</FadingCurveName>
      <OutputControllerName>TeensyStripController</OutputControllerName>
    </LedStrip>

    <!-- Check comments above for details on the settings -->
    <LedStrip>
      <Name>PF Left</Name>
      <Width>1</Width>
      <Height>65</Height>
      <LedStripArrangement>TopDownLeftRight</LedStripArrangement>
      <ColorOrder>GRB</ColorOrder>
      <FirstLedNumber>167</FirstLedNumber>
      <FadingCurveName>SwissLizardsLedCurve</FadingCurveName>
      <OutputControllerName>TeensyStripController</OutputControllerName>
    </LedStrip>


    <!-- The LedWizEquivalent toy maps the toys defined above to the ini file columns containing the table configs -->
    <LedWizEquivalent>

      <!-- The name can be anything you like, but including the number of the ini file might be a good practice.-->
      <Name>LedWizEquivalent 30</Name>

      <Outputs>

    <LedWizEquivalentOutput>
      <!--For LedStrip and area toys the OutputName defines the name of the toy (as defined above) which is to be controlled by the ini file. -->
      <OutputName>PF Back</OutputName>
      <!-- Column of the ini file containing the config data for the toy defined in the OutputName. -->
      <LedWizEquivalentOutputNumber>1</LedWizEquivalentOutputNumber>
    </LedWizEquivalentOutput>

    <!-- Check comments above for details on the settings -->
    <LedWizEquivalentOutput>
      <OutputName>PF Right</OutputName>
      <LedWizEquivalentOutputNumber>4</LedWizEquivalentOutputNumber>
    </LedWizEquivalentOutput>

    <!-- Check comments above for details on the settings -->
    <LedWizEquivalentOutput>
      <OutputName>PF Left</OutputName>
      <LedWizEquivalentOutputNumber>10</LedWizEquivalentOutputNumber>
    </LedWizEquivalentOutput>

      </Outputs>
      <!-- The number of the ini file, which contains the configs for the toys.-->
      <LedWizNumber>30</LedWizNumber>
    </LedWizEquivalent>

  </Toys>

</Cabinet>
~~~~~~~~~~~~~


\section configexamples_users User Cabinet configs

This contains cabinet configs provided by DOF users. 

\section configexamples_arngrim Arngrims config

Arngrims config contains quite a few settings for Artnet devices. HIs other controllers are detected automatically and are therefore not listed in the config file.

~~~~~~~~~~~~~{.xml}
<?xml version="1.0"?>
<Cabinet xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Name>Arngrim</Name>
  <OutputControllers>
    <ArtNet>
      <Name>Artnet Node 1</Name>
      <Universe>0</Universe>
      <BroadcastAddress>255.255.255.255</BroadcastAddress>
    </ArtNet>
  </OutputControllers>
  <Toys>
    <RGBAToyGroup>
      <Name>Ledbar Left</Name>
      <ToyNames>
        <Row>
          <Column>32</Column>
        </Row>
        <Row>
          <Column>31</Column>
        </Row>
        <Row>
          <Column>30</Column>
        </Row>
        <Row>
          <Column>29</Column>
        </Row>
        <Row>
          <Column>28</Column>
        </Row>
        <Row>
          <Column>27</Column>
        </Row>
        <Row>
          <Column>26</Column>
        </Row>
        <Row>
          <Column>25</Column>
        </Row>
        <Row>
          <Column>24</Column>
        </Row>
        <Row>
          <Column>23</Column>
        </Row>
        <Row>
          <Column>22</Column>
        </Row>
        <Row>
          <Column>21</Column>
        </Row>
        <Row>
          <Column>20</Column>
        </Row>
        <Row>
          <Column>19</Column>
        </Row>
        <Row>
          <Column>18</Column>
        </Row>
        <Row>
          <Column>17</Column>
        </Row>
      </ToyNames>
      <LayerOffset>0</LayerOffset>
    </RGBAToyGroup>
    <RGBAToyGroup>
      <Name>Ledbar Right</Name>
      <ToyNames>
        <Row>
          <Column>16</Column>
        </Row>
        <Row>
          <Column>15</Column>
        </Row>
        <Row>
          <Column>14</Column>
        </Row>
        <Row>
          <Column>13</Column>
        </Row>
        <Row>
          <Column>12</Column>
        </Row>
        <Row>
          <Column>11</Column>
        </Row>
        <Row>
          <Column>10</Column>
        </Row>
        <Row>
          <Column>9</Column>
        </Row>
        <Row>
          <Column>8</Column>
        </Row>
        <Row>
          <Column>7</Column>
        </Row>
        <Row>
          <Column>6</Column>
        </Row>
        <Row>
          <Column>5</Column>
        </Row>
        <Row>
          <Column>4</Column>
        </Row>
        <Row>
          <Column>3</Column>
        </Row>
        <Row>
          <Column>2</Column>
        </Row>
        <Row>
          <Column>1</Column>
        </Row>
      </ToyNames>
      <LayerOffset>0</LayerOffset>
    </RGBAToyGroup>
    <RGBAToy>
      <Name>1</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.001</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.002</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.003</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>2</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.004</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.005</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.006</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>
    <RGBAToy>
      <Name>3</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.007</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.008</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.009</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>4</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.010</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.011</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.012</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>    
    <RGBAToy>
      <Name>5</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.013</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.014</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.015</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>6</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.016</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.017</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.018</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>    
    <RGBAToy>
      <Name>7</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.019</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.020</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.021</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>8</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.022</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.023</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.024</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>    
    <RGBAToy>
      <Name>9</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.025</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.026</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.027</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>10</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.028</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.029</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.030</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>    
    <RGBAToy>
      <Name>11</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.031</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.032</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.033</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>12</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.034</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.035</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.036</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>    
    <RGBAToy>
      <Name>13</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.037</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.038</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.039</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>14</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.040</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.041</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.042</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>    
    <RGBAToy>
      <Name>15</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.043</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.044</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.045</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>16</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.046</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.047</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.048</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>17</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.128</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.129</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.130</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>18</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.131</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.132</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.133</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>    
    <RGBAToy>
      <Name>19</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.134</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.135</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.136</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>20</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.137</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.138</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.139</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>21</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.140</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.141</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.142</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>22</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.143</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.144</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.145</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>23</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.146</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.147</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.148</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>24</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.149</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.150</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.151</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>25</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.152</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.153</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.154</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>26</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.155</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.156</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.157</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>27</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.158</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.159</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.160</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>28</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.161</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.162</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.163</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>29</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.164</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.165</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.166</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>30</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.167</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.168</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.169</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>31</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.170</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.171</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.172</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>  
    <RGBAToy>
      <Name>32</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.173</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.174</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.175</OutputNameBlue>    
      <FadingCurveName>Linear</FadingCurveName>
    </RGBAToy>
    <AnalogAlphaToyGroup>
      <Name>Strobe</Name>
      <ToyNames>
        <Row>
        <Column>49</Column>
        <Column>50</Column>      
        <Column>51</Column>
        <Column>52</Column>      
        <Column>53</Column>
        <Column>54</Column>
      </Row>
      </ToyNames>
      <LayerOffset>0</LayerOffset>
    </AnalogAlphaToyGroup>
    <Lamp>
      <Name>49</Name>
      <OutputName>Artnet Node 1\Artnet Node 1.049</OutputName>
      <FadingCurveName>Linear</FadingCurveName>
    </Lamp>   
    <Lamp>
      <Name>50</Name>
      <OutputName>Artnet Node 1\Artnet Node 1.050</OutputName>
      <FadingCurveName>Linear</FadingCurveName>
    </Lamp>   
    <Lamp>
      <Name>51</Name>
      <OutputName>Artnet Node 1\Artnet Node 1.051</OutputName>
      <FadingCurveName>Linear</FadingCurveName>
    </Lamp>   
    <Lamp>
      <Name>52</Name>
      <OutputName>Artnet Node 1\Artnet Node 1.052</OutputName>
      <FadingCurveName>Linear</FadingCurveName>
    </Lamp>   
    <Lamp>
      <Name>53</Name>
      <OutputName>Artnet Node 1\Artnet Node 1.053</OutputName>
      <FadingCurveName>Linear</FadingCurveName>
    </Lamp>   
    <Lamp>
      <Name>54</Name>
      <OutputName>Artnet Node 1\Artnet Node 1.054</OutputName>
      <FadingCurveName>Linear</FadingCurveName>
    </Lamp>   
    <LedWizEquivalent>
      <Name>LedWizEquivalent 100</Name>      
      <LedWizNumber>100</LedWizNumber>  
      <Outputs>
      <LedWizEquivalentOutput>
          <OutputName>Ledbar Left</OutputName>
          <LedWizEquivalentOutputNumber>1</LedWizEquivalentOutputNumber>      
      </LedWizEquivalentOutput>
      <LedWizEquivalentOutput>
          <OutputName>Ledbar Right</OutputName>
          <LedWizEquivalentOutputNumber>4</LedWizEquivalentOutputNumber>      
      </LedWizEquivalentOutput>
      <LedWizEquivalentOutput>
      <OutputName>Artnet Node 1\Artnet Node 1.049</OutputName>
          <LedWizEquivalentOutputNumber>7</LedWizEquivalentOutputNumber>
      </LedWizEquivalentOutput>
      <LedWizEquivalentOutput>
      <OutputName>Artnet Node 1\Artnet Node 1.050</OutputName>
          <LedWizEquivalentOutputNumber>8</LedWizEquivalentOutputNumber>
      </LedWizEquivalentOutput>
      <LedWizEquivalentOutput>
      <OutputName>Artnet Node 1\Artnet Node 1.051</OutputName>
          <LedWizEquivalentOutputNumber>9</LedWizEquivalentOutputNumber>
      </LedWizEquivalentOutput>
      <LedWizEquivalentOutput>
      <OutputName>Artnet Node 1\Artnet Node 1.052</OutputName>
          <LedWizEquivalentOutputNumber>10</LedWizEquivalentOutputNumber>
      </LedWizEquivalentOutput>
      <LedWizEquivalentOutput>
      <OutputName>Artnet Node 1\Artnet Node 1.053</OutputName>
          <LedWizEquivalentOutputNumber>11</LedWizEquivalentOutputNumber>
      </LedWizEquivalentOutput>
      <LedWizEquivalentOutput>
      <OutputName>Artnet Node 1\Artnet Node 1.054</OutputName>
          <LedWizEquivalentOutputNumber>12</LedWizEquivalentOutputNumber>
      </LedWizEquivalentOutput>
      </Outputs>
    </LedWizEquivalent>    
  </Toys>
</Cabinet>
~~~~~~~~~~~~~~

\subsection configexamples_swisslizard Swisslizards config

This is the config of Swisslizards cabinet. Since there are quite a few output controllers and toys in that cab the config is quite long and messy. Dont use this as a example how thing can be done in the most readable and understandable way.

~~~~~~~~~~~~~{.xml}
<?xml version="1.0"?>
<Cabinet xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Name>Lizard Pin</Name>
  <OutputControllers>
    
    <ArtNet>
      <Name>Artnet Node 1</Name>
      <Universe>0</Universe>
      <BroadcastAddress>255.255.255.255</BroadcastAddress>
    </ArtNet>

    <TeensyStripController>
      <Name>LedStripController</Name>
      <NumberOfLedsStrip1>235</NumberOfLedsStrip1>
      <NumberOfLedsStrip2>0</NumberOfLedsStrip2>
      <NumberOfLedsStrip3>0</NumberOfLedsStrip3>
      <NumberOfLedsStrip4>0</NumberOfLedsStrip4>
      <NumberOfLedsStrip5>0</NumberOfLedsStrip5>
      <NumberOfLedsStrip6>0</NumberOfLedsStrip6>
      <NumberOfLedsStrip7>0</NumberOfLedsStrip7>
      <NumberOfLedsStrip8>0</NumberOfLedsStrip8>
      <ComPortName>COM18</ComPortName>
    </TeensyStripController>

    <!-- Ledwiz, Pacdrive and Pacled controllers are detected automatically and dont need to be configured here  -->

    <!-- Here is the config for the second ledwiz anyway -->
    <LedWiz>
      <Name>LedWiz 02</Name>
      <Number>2</Number>
    </LedWiz>
  </OutputControllers>


  <Toys>

    <!-- This toygroup exists only for reference and testing. There are no configs for it.
    This a a example how a matrix can be built which has unassigned locations. 
    -->
    <AnalogAlphaToyGroup>
      <Name>Cabinet Front Lamps</Name>
      <ToyNames>
        <Row>
          <Column>Start Button</Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
        </Row>
        <Row>
          <Column>Extra Ball Button</Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
        </Row>
        <Row>
          <Column>Exit Button</Column>
          <Column></Column>
          <Column></Column>
          <Column>Coin Left</Column>
          <Column>Coin Right</Column>
          <Column></Column>
          <Column>Launch Ball Button</Column>
        </Row>
      </ToyNames>
      <LayerOffset>50</LayerOffset>
    </AnalogAlphaToyGroup>



    <!-- The lamps at my cabinet front -->
    <Lamp>
      <Name>Launch Ball Button</Name>
      <OutputName>LedWiz 02.08</OutputName>
      <FadingCurveName>Linear</FadingCurveName>
    </Lamp>

    <Lamp>
      <Name>Coin Right</Name>
      <OutputName>LedWiz 02.13</OutputName>
      <FadingCurveName>Linear</FadingCurveName>
    </Lamp>

    <Lamp>
      <Name>Coin Left</Name>
      <OutputName>LedWiz 02.12</OutputName>
      <FadingCurveName>Linear</FadingCurveName>
    </Lamp>

    <Lamp>
      <Name>Start Button</Name>
      <OutputName>LedWiz 02.14</OutputName>
      <FadingCurveName>Linear</FadingCurveName>
    </Lamp>

    <Lamp>
      <Name>Extra Ball Button</Name>
      <OutputName>LedWiz 02.15</OutputName>
      <FadingCurveName>Linear</FadingCurveName>
    </Lamp>

    <Lamp>
      <Name>Exit Button</Name>
      <OutputName>LedWiz 02.16</OutputName>
      <FadingCurveName>Linear</FadingCurveName>
    </Lamp>


    <!-- Config for the shaker toy. The shaker toy has a few extra config options which allow for finetuning of the shaker behaviour. 
    If the shaker toy is not explicitly configured, DOF will automatically confure the shaker output based on the ini file data (but the options of the shaker toy cant be used). 
    Since my shaker is rather strong, I'm limiting its power to 96 (max value would be 255). The minpower is 50, because my shaker stops rotating with lower values.
    -->
    <Shaker>
      <Name>Shaker</Name>
      <OutputName>LedWiz 02.32</OutputName>
      <FadingCurveName>Linear</FadingCurveName>
      <MaxRunTimeMs>300000</MaxRunTimeMs>
      <KickstartPower>255</KickstartPower>
      <KickstartDurationMs>100</KickstartDurationMs>

      <MinPower>50</MinPower>
      <MaxPower>96</MaxPower>
    </Shaker>


    <!-- The so called Hellball is a DMX lightning fixture made for small discos. I have extended it with 48 addressable leds to get a more sophisticated effect.
    The extra leds are not arranged in a proper matrix (there are "holes" in the matrix). Therefore the matrix cant be configured through the LedStrip toy (which expects a matrix without holes)
    and the RGBAToyGroup (which accepts the same effects as the ledstrip toy) is used instead.
    -->
    <RGBAToyGroup>
      <Name>HellballLeds</Name>
      <ToyNames>
        <Row>
          <Column>HellballLed 0 0</Column>
          <Column>HellballLed 1 0</Column>
          <Column>HellballLed 2 0</Column>
          <Column>HellballLed 3 0</Column>
          <Column>HellballLed 4 0</Column>
          <Column>HellballLed 5 0</Column>
          <Column>HellballLed 6 0</Column>
          <Column>HellballLed 7 0</Column>
        </Row>
        <Row>
          <Column>HellballLed 0 1</Column>
          <Column>HellballLed 1 1</Column>
          <Column>HellballLed 2 1</Column>
          <Column>HellballLed 3 1</Column>
          <Column>HellballLed 4 1</Column>
          <Column>HellballLed 5 1</Column>
          <Column>HellballLed 6 1</Column>
          <Column>HellballLed 7 1</Column>
        </Row>
        <Row>
          <Column>HellballLed 0 2</Column>
          <Column>HellballLed 1 2</Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column>HellballLed 6 2</Column>
          <Column>HellballLed 7 2</Column>
        </Row>
        <Row>
          <Column>HellballLed 0 3</Column>
          <Column>HellballLed 1 3</Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column>HellballLed 6 3</Column>
          <Column>HellballLed 7 3</Column>
        </Row>
        <Row>
          <Column>HellballLed 0 4</Column>
          <Column>HellballLed 1 4</Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column>HellballLed 6 4</Column>
          <Column>HellballLed 7 4</Column>
        </Row>
        <Row>
          <Column>HellballLed 0 5</Column>
          <Column>HellballLed 1 5</Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column></Column>
          <Column>HellballLed 6 5</Column>
          <Column>HellballLed 7 5</Column>
        </Row>
        <Row>
          <Column>HellballLed 0 6</Column>
          <Column>HellballLed 1 6</Column>
          <Column>HellballLed 2 6</Column>
          <Column>HellballLed 3 6</Column>
          <Column>HellballLed 4 6</Column>
          <Column>HellballLed 5 6</Column>
          <Column>HellballLed 6 6</Column>
          <Column>HellballLed 7 6</Column>
        </Row>
        <Row>
          <Column>HellballLed 0 7</Column>
          <Column>HellballLed 1 7</Column>
          <Column>HellballLed 2 7</Column>
          <Column>HellballLed 3 7</Column>
          <Column>HellballLed 4 7</Column>
          <Column>HellballLed 5 7</Column>
          <Column>HellballLed 6 7</Column>
          <Column>HellballLed 7 7</Column>
        </Row>
      </ToyNames>
      <LayerOffset>0</LayerOffset>
    </RGBAToyGroup>

    <!-- These are the single leds which are used in the above RGBAToyGroup.
    All these leds could also be targeted directly instead of using the group.
    -->
    <RGBAToy>
      <Name>HellballLed 0 0</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.034</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.033</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.035</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 1 0</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.037</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.036</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.038</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 2 0</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.040</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.039</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.041</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 3 0</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.043</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.042</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.044</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 4 0</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.046</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.045</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.047</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 5 0</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.049</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.048</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.050</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 6 0</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.052</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.051</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.053</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 7 0</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.055</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.054</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.056</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 0 1</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.058</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.057</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.059</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 1 1</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.061</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.060</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.062</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 2 1</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.064</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.063</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.065</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 3 1</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.067</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.066</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.068</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 4 1</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.070</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.069</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.071</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 5 1</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.073</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.072</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.074</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 6 1</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.076</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.075</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.077</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 7 1</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.079</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.078</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.080</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 0 2</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.082</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.081</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.083</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 1 2</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.085</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.084</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.086</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 6 2</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.088</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.087</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.089</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 7 2</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.091</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.090</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.092</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 0 3</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.094</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.093</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.095</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 1 3</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.097</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.096</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.098</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 6 3</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.100</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.099</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.101</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 7 3</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.103</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.102</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.104</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 0 4</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.106</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.105</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.107</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 1 4</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.109</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.108</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.110</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 6 4</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.112</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.111</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.113</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 7 4</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.115</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.114</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.116</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 0 5</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.118</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.117</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.119</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 1 5</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.121</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.120</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.122</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 6 5</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.124</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.123</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.125</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 7 5</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.127</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.126</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.128</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 0 6</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.130</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.129</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.131</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 1 6</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.133</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.132</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.134</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 2 6</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.136</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.135</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.137</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 3 6</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.139</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.138</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.140</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 4 6</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.142</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.141</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.143</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 5 6</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.145</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.144</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.146</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 6 6</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.148</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.147</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.149</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 7 6</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.151</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.150</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.152</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 0 7</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.154</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.153</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.155</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 1 7</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.157</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.156</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.158</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 2 7</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.160</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.159</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.161</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 3 7</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.163</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.162</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.164</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 4 7</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.166</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.165</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.167</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 5 7</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.169</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.168</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.170</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 6 7</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.172</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.171</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.173</OutputNameBlue>
    </RGBAToy>
    <RGBAToy>
      <Name>HellballLed 7 7</Name>
      <OutputNameRed>Artnet Node 1\Artnet Node 1.175</OutputNameRed>
      <OutputNameGreen>Artnet Node 1\Artnet Node 1.174</OutputNameGreen>
      <OutputNameBlue>Artnet Node 1\Artnet Node 1.176</OutputNameBlue>
    </RGBAToy>

    <!--Ledstrip configs for PF back, PF left, PF right and right flipper buttons (left is still missing) -->
    <!-- I have 3 rows with 32 leds each as a PF back -->
    <LedStrip>
      <Name>BackBoard</Name>
      <Width>32</Width>
      <Height>3</Height>
      <LedStripArrangement>LeftRightTopDown</LedStripArrangement>
      <ColorOrder>GRB</ColorOrder>
      <FirstLedNumber>1</FirstLedNumber>
      <FadingCurveName>SwissLizardsLedCurve</FadingCurveName>
      <OutputControllerName>LedStripController</OutputControllerName>
    </LedStrip>
    <!-- PF sides have 65 leds each.-->
    <LedStrip>
      <Name>Sideboard Right</Name>
      <Width>1</Width>
      <Height>65</Height>
      <LedStripArrangement>TopDownLeftRight</LedStripArrangement>
      <ColorOrder>GRB</ColorOrder>
      <FirstLedNumber>97</FirstLedNumber>
      <FadingCurveName>SwissLizardsLedCurve</FadingCurveName>
      <OutputControllerName>LedStripController</OutputControllerName>
    </LedStrip>
    <!-- 5 leds behind the flipper buttons -->
    <LedStrip>
      <Name>Buttons Right</Name>
      <Width>1</Width>
      <Height>5</Height>
      <LedStripArrangement>LeftRightTopDown</LedStripArrangement>
      <ColorOrder>GRB</ColorOrder>
      <FirstLedNumber>162</FirstLedNumber>
      <FadingCurveName>SwissLizardsLedCurve</FadingCurveName>
      <OutputControllerName>LedStripController</OutputControllerName>
    </LedStrip>
    <!-- PF sides have 65 leds each.-->
    <LedStrip>
      <Name>Sideboard Left</Name>
      <Width>1</Width>
      <Height>65</Height>
      <LedStripArrangement>TopDownLeftRight</LedStripArrangement>
      <ColorOrder>GRB</ColorOrder>
      <FirstLedNumber>167</FirstLedNumber>
      <FadingCurveName>SwissLizardsLedCurve</FadingCurveName>
      <OutputControllerName>LedStripController</OutputControllerName>
    </LedStrip>

    <!-- This maps the ini file for the second ledwiz to the correct outputs. 
    It is technically not necessary to configure this by hand. DOF detects the necessary config of the output from the ini file.
    -->
    <LedWizEquivalent>
      <Name>LedWizEquivalent 2</Name>
      <Outputs>
        <LedWizEquivalentOutput>
          <OutputName>LedWiz 02.08</OutputName>
          <LedWizEquivalentOutputNumber>8</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>LedWiz 02.12</OutputName>
          <LedWizEquivalentOutputNumber>12</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>LedWiz 02.13</OutputName>
          <LedWizEquivalentOutputNumber>13</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>LedWiz 02.14</OutputName>
          <LedWizEquivalentOutputNumber>14</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>LedWiz 02.15</OutputName>
          <LedWizEquivalentOutputNumber>15</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>LedWiz 02.16</OutputName>
          <LedWizEquivalentOutputNumber>16</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>LedWiz 02.32</OutputName>
          <LedWizEquivalentOutputNumber>32</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
      </Outputs>
      <LedWizNumber>2</LedWizNumber>
    </LedWizEquivalent>


    <!-- This maps the ini file comumns of the ledstrip configs to the correct toys. 
    ^DOF cant do this automatically, so this is mandatory.
    Note: LedwizEquivalents for Ledstrips have to point to toys, not to outputs like other mappings!
    -->
    <LedWizEquivalent>
      <Name>LedWizEquivalent 50</Name>
      <Outputs>
        <LedWizEquivalentOutput>
          <OutputName>BackBoard</OutputName>
          <LedWizEquivalentOutputNumber>1</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Sideboard Right</OutputName>
          <LedWizEquivalentOutputNumber>4</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Buttons Right</OutputName>
          <LedWizEquivalentOutputNumber>7</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Sideboard Left</OutputName>
          <LedWizEquivalentOutputNumber>10</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Cabinet Front Lamps</OutputName>
          <LedWizEquivalentOutputNumber>13</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
      </Outputs>
      <LedWizNumber>50</LedWizNumber>
    </LedWizEquivalent>

    <!-- Configs for my Artnet devices -->
    <LedWizEquivalent>
      <Name>LedWizEquivalent 100</Name>
      <Outputs>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.001</OutputName>
          <LedWizEquivalentOutputNumber>1</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>

        <!--Bumper lamp configs -->
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.004</OutputName>
          <LedWizEquivalentOutputNumber>4</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.005</OutputName>
          <LedWizEquivalentOutputNumber>5</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.006</OutputName>
          <LedWizEquivalentOutputNumber>6</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>

        <!-- Configs for the Hellball outputs-->
        <LedWizEquivalentOutput>
          <OutputName>HellballLeds</OutputName>
          <LedWizEquivalentOutputNumber>7</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.010</OutputName>
          <LedWizEquivalentOutputNumber>10</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.011</OutputName>
          <LedWizEquivalentOutputNumber>11</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.012</OutputName>
          <LedWizEquivalentOutputNumber>12</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.013</OutputName>
          <LedWizEquivalentOutputNumber>13</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.014</OutputName>
          <LedWizEquivalentOutputNumber>14</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.015</OutputName>
          <LedWizEquivalentOutputNumber>15</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.016</OutputName>
          <LedWizEquivalentOutputNumber>16</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.017</OutputName>
          <LedWizEquivalentOutputNumber>17</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.018</OutputName>
          <LedWizEquivalentOutputNumber>18</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.019</OutputName>
          <LedWizEquivalentOutputNumber>19</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.020</OutputName>
          <LedWizEquivalentOutputNumber>20</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.021</OutputName>
          <LedWizEquivalentOutputNumber>21</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.022</OutputName>
          <LedWizEquivalentOutputNumber>22</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.023</OutputName>
          <LedWizEquivalentOutputNumber>23</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.024</OutputName>
          <LedWizEquivalentOutputNumber>24</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.025</OutputName>
          <LedWizEquivalentOutputNumber>25</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.026</OutputName>
          <LedWizEquivalentOutputNumber>26</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.027</OutputName>
          <LedWizEquivalentOutputNumber>27</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.028</OutputName>
          <LedWizEquivalentOutputNumber>28</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.029</OutputName>
          <LedWizEquivalentOutputNumber>29</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.030</OutputName>
          <LedWizEquivalentOutputNumber>30</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.031</OutputName>
          <LedWizEquivalentOutputNumber>31</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
        <LedWizEquivalentOutput>
          <OutputName>Artnet Node 1\Artnet Node 1.032</OutputName>
          <LedWizEquivalentOutputNumber>32</LedWizEquivalentOutputNumber>
        </LedWizEquivalentOutput>
      </Outputs>
      <LedWizNumber>100</LedWizNumber>
    </LedWizEquivalent>
  </Toys>

</Cabinet>
~~~~~~~~~~~~~