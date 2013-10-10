Architecture
============

\warning This page is not up to date. It was last update in spring 2013.

\section architecture_objectmodel Object model 

\image html GlobalArchitecture.png "Please check the docu on the classes for details on all class members and class descriptions."

The DirectOutput framework consist of 3 main areas:

* __Table__ where the events for changed table elements (e.g. a solenoid) from Pinmame are processed and if available the _effects_ which are assigned to that table element are triggered. _Table_ does also have a list of static _Effects_ which are tiggered when the _Table_ is initialized.
* __Cabinet__ which represents the physical parts of a virtual pinball cabinet. _OutputControllers_ represent units like the LedWiz. _Toys_ represent the gadgets which are installed in a cabinet (e.g. contactors, RGB-leds or a shaker). _Toys_ are controlled by _Effects_ and will, if necessary, change the value of one or severel _Outputs_ of _OutputControllers_.
* __Effects__ are defining the actions for _Toys_ in a _Cabinet_. _Effects_ are triggered for events in _Table_.

\subsection architecture_table Table 
\image html TableArchitecture.png "Please check the docu on the classes for details on all class members and class descriptions."

\subsection architecture_effects Effects 
\image html EffectsArchitecture.png "Please check the docu on the classes for details on all class members and class descriptions."

All _Effects_ must implement the interface IEffect. _EffectBase_ is a abstract base class for effect implementation. It is recommended to inherit from this class when implementing new effects.

More effects can be added to the framework by using scripts.

\subsection architecture_cabinet Cabinet 
\image html CabinetArchitecture.png "Please check the docu on the classes for details on all class members and class descriptions."

\subsubsection architecture_toys Toys 
\image html ToyArchitecture.png "Please check the docu on the classes for details on all class members and class descriptions."

All toys must implement the iToy interface to be usable in the framework. More toys can be added through scripting.

\subsubsection architecture_outputcontroller Outputcontroller 
\image html OutputControllerArchitecture.png "Please check the docu on the classes for details on all class members and class descriptions."

\section architecture_multithreading Multithreading

This framework distributes its work over several threads to ensure that there are no (or at least very little) side effects and performance problems with the application calling the framework functions.

If the framework is called with new data from the outside (e.g. through B2S.Server), all it will do is to put the received data in a queue and notifiy the main loop thread. After that the independent mainloop thread will take care of the received data and take all necessary actions.

The LedWiz object does also use a separate thread for each connected LedWiz, to overcome the response time problems of the LedWiz. 

Depending on the config you are using other threads might be active as well.
