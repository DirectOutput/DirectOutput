Output controllers {#outputcontrollers}
==================

\section outputcontrollers_introduction Introduction 

Output controllers are devices like the Ledwiz, PacLed64 or maybe your own Arduino or Raspberry PI invention controlling one or several physical outputs in a cabinet.  

\image html OutputControllerArchitecture.png

By providing interfaces for the outputcontroller itself (IOutputController) and for the outputs of a outputcontroller (IOutput) it is possible to support different output controllers. Since all outputs of output controllers have to implement _IOutput_ any configured toy can use any configured output of any output controller in the system.


\section outputcontrollers_builtineffects Builtin output controllers 

At the time of writting the framework has built in support for the following output controllers:

\subsection outputcontrollers_ledwiz LedWiz (GroovyGameGear)

The LedWiz is a outputcontroller with 32 outputs which all support 49 <a target="_blank" href="https://en.wikipedia.org/wiki/Pulse-width_modulation">pwm</a> levels. The LedWiz is able to drive Leds directly, but will require some kind of booster of power hunger gadgets like big contactors or motors are connected to it.

\image html LedWizboard.jpg

The DirectOutput framework does fully support the LedWiz and can control up to 16 LedWiz units. The framework can automatically detect connected LedWiz units and configure them for use with the framework.

The LedWiz is made by <a href="http://groovygamegear.com/">GroovyGameGear</a> and can by ordered directly on GroovyGamegears website, but also from some other vendors.

This unit was the first output controller which was widely used in the virtual pinball community and was the unit for which the legacy vbscript solution was developed. The DirectOutput framework replaces the vbscript solution, but can reuse the ini files which were used for the configuration of the tables. Please read \ref ledcontrolfiles for more information.

The framework supports auto detection and configuration of the LedWiz.

\image html LedWizLogo.jpg

\subsection outputcontrollers_ultimarc_pacled64 PacLed64 (Ultimarc)

The PacLed64 is a output controller with 64 outputs all supporting 256 <a target="_blank" href="https://en.wikipedia.org/wiki/Pulse-width_modulation">pwm</a> levels. Since the outputs of the unit are constant current drivers providing 20ma each, smaller leds can be connected directly to the outputs, but booster circuits might be used to driver higher loads (e.g. Cree leds). Up to 4 PacLed64 controllers can be used with the DirectOutput framework.

The framework supports auto detection and configuration of these units.

This unit is made and sold by <a target="_blank" href="http://www.ultimarc.com">Ultimarc</a>.

\image html PacLed64Logo.png

\subsection outputcontrollers_artnet Art-Net / DMX

<a target="_blank" href="https://en.wikipedia.org/wiki/Art-Net">Art-Net</a> is a industry standard protocol used to control <a target="_blank" href="https://en.wikipedia.org/wiki/DMX512">DMX</a> lighting effects over othernet. Using Art-Net it is possible to connect a very wide range of lighting effects like <a target="_blank" href="https://www.google.ch/search?q=dmx+strobe">strobes</a> or <a target="_blank" href="https://www.google.ch/search?q=dmx+dimmer">dimmer packs</a>. There are tons of DMX controlled effects available on the market (from very cheap and small to very expensive and big). It might sounds a bit crazy, but with Art-net and DMX you could at keast in theory control a whole stage lighting system (this would likely make you feel like Tommy in the movie).

\image html DMX.png

To use Art-Net you will need a Art-Net node (unit that converts from ethernet to DMX protocol) and also some DMX controlled lighting effect. There are quite a few different Art-Net nodes available on the market and most of them should be compatible with the DirectOutput framework. For testing the Art-Net node sold on http://www.ulrichradig.de/home/index.php/avr/dmx-avr-artnetnode as a DIY kit was used. 

Each Art-Net node/DMX universe supports 512 DMX channels and several Art-Net nodes controlling different DMX universes can be used in parallel.

If you want to read more about Art-net, visit the website of <a href="http://www.artisticlicence.com">Artistic License</a>. The specs for Art-net can be found in the Resources -> User Guides & Datasheets section of the site.


\section outputcontrollers_customeffects Custom output controllers 

The DirectOutput framework allows for the implementation of custom output controllers. This can be done trough scripting (see chapter on scripting) or the classes required for a custom outputcontroller can be integrated in the DirectOutput project. 

\subsection outputcontrollers_interfaces Interfaces for output controllers

\subsubsection outputcontrollers_ioutputcontroller IOutputController interface

All output controllers need to implement the interface _IOutputController_ to allow the DirectOutput framework to use the output controller. The IOutputController interface inherits the _INamedItem_ interface, so the members of this interface have to be implemented as well.

A good starting point for your own implementation of a output controller is the abstract _OutputControllerBase_ class. _OutputControllerBase_ implements the _INamedItem_ interface by inheriting _NamedItemBase_ and also the _Outputs_ property of the _IOutputController_ interface. The simplest implemention of a output controller is probably the NullOutputController which is part of the framework.

Here is a short explanation of the interface members of _IOutputController_:

- <b>string Name { get; set; }</b><br/>Must set or get the name of the output controller. If the value of the name property is changed the _BeforeNameChange_ and _AfterNameChange_ events have to fire. _OutputControllerBase_ does implement this member (inherited from _NamedItemBase_).
- <b>event EventHandler<NameChangeEventArgs> BeforeNameChange</b><br/>This event has to be fired before the value of the _Name_ property gets changeds. _OutputControlledBase_ is implementing this event (inherited from _NamedItemBase_). 
- <b>event EventHandler<NameChangeEventArgs> AfterNameChanged</b><br/>Event has to be fired after the value of the _Name_ property has changed.  _OutputControlledBase_ is implementing this event (inherited from _NamedItemBase_). 

- <b>OutputList Outputs { get; set; }</b><br/>This property contains the list of outputs (objects implementing IOutput) of a output controllers. Property is implement in the _OutputControllerBase_ class, but can be overwritten.
- <b>void Init()</b><br/>Method to initialize the output controller. This is the first method which is classed on a output controller after it has been instanciated.
- <b>void Update()</b><br/>This method is called by the framework after data received from the outside world (typically PinMame) has been processed or after the UpdateTime has fired, to tell the output controller to update its physical outputs. If updating the physical outputs involves some time lag (very likely it does), consider doing the actual update in a separate thread and only to use this method to signal the updater thread, that data is ready.
- <b>void Finish()</b><br/>Finish is the last method which is called on a output controller, before the object gets discared. Do all necessary cleanup work in this method and make sure all physical outputs are turned off.

Please have a look at the class documentations for more information on the interface and its implementation.


\subsubsection outputcontrollers_ioutput IOutput interface

Output controllers implement the property _Outputs_ which contains a list of the outputs of the output controller. This list can contain objects implement the _IOutput_ interface.

The class _Output_ is a full implementation of the _IOutput_ interface and can be used directly for your output controller implementation or can be inherited to create your own extended _IOutput_ implementation (e.g. having a additional output number property).

\remark Since the _OutputList_ class used in the _Outputs_ property of _IOutputController_ will always return _IOutput_ objects, you might need to cast these objects into your own type, before you can access your own methods or properties of a extended output implementation.

The _IOutput_ interface has the following members:

- <b>string Name { get; set; }</b><br/>Must set or get the name of the output. If the value of the name property is changed the _BeforeNameChange_ and _AfterNameChange_ events have to fire. The _Output_ class does implement this member (inherited from _NamedItemBase_).
- <b>event EventHandler<NameChangeEventArgs> BeforeNameChange</b><br/>This event has to be fired before the value of the _Name_ property gets changeds. The _Output_ class is implementing this event (inherited from _NamedItemBase_). 
- <b>event EventHandler<NameChangeEventArgs> AfterNameChanged</b><br/>Event has to be fired after the value of the _Name_ property has changed. The _Output_ class is implementing this event (inherited from _NamedItemBase_). 
- <b>byte Value { get; set; }</b>Property getting and setting the value of a output. A typical implementation of a output controller will use the value of this property to set the value of the physical output within its _Update_ method (or updater thread). Make sure your implementation supports the whole value range (0-255) of the property (e.g. if you have a digital output supporting only on and off, map 0 to off and all other values to on).
- <b>event Output.ValueChangedEventHandler ValueChanged</b><br/>This event has to fire if the _Value_ property of the output has changed.

\subsection outputcontrollers_implementationguideline  Implementation guidelines for custom output controllers

* All output controllers must implement the _IOutputController_ interface. This interface specifies some methods and properties which will allow the framerwork to use the output controller.
* If a output controllers uses its own output class, this class must implement the _IOutput_ interface.
* Try to inherit the _OutputControllerBase_ class. This abstract class does already implement some functionality required for your own output controller class.
* Define a unique name for your output controller (if the name is not unqiue loading or saving the configuration will probably fail) and put it in a subnamespace of DirectOutput.Cab.Out (e.g. DirectOutput.Cab.Out.MyOutputController).
* Ensure that your output controller class is XMLSerializable. As a rule of thumb, your class must implement a paramaterless constructor and all settings for the effect have to be available through public read and write enabled properties. If your output controller is not XMLserializeable by default implement the _IXMLSerializable_ interface to ensure that the config for the controller can be loaded and saved.
* Be fault tolerant and handle all errors that eventually occur.
* Keep your code as fast as possible. Updates on output controllers can occur often and slow code will slow down the whole framework. 
* Put the update logic for your output controller in a separate thread, so all time consuming calls to functions talking to the outside world are isolated from the rest of the framework (use the Init and the Finish methods to start/stop your own thread).  
* Comment your code! At least the public methods and properties of your output controller class should be properly documented with XML comments (check the source code of DOF to see what is expected).
