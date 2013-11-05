Toys {#toys}
==========
\section toys_introduction Introduction 

Toys are the part of the DirectOutput framework representing the gadgets that you have installed in you cabinet. Typically toys are controlling one or several outputs (referenced by the name of the output) and they are themselfs controlled by effects.

Toys are configured in the cabinet config file or are, depending on your config, added automatically to the cabinet config (in particular when ledcontrol.ini/directoutputconfig.ini files are used and no cabinet config file exists).



\section toys_bultin Builtin toys

A list of builtin toys, including configuration samples, can be found on the following page: \ref toy_builtin "Built in toys"


\section toys_custom Custom toys 


\section toys_implementationguideline  Implementation guidelines for custom toys

* Toys must implement the _IToy_ interface to allow the framework to identify and use the class as a toy.
* If possible, inherit one of the abstact toy base classes or a existing toy as the base of your own toy implementation.
* Each toy class must have a globaly unique name (not only unique within the namespace). This name is used when toy configs are loaded and saved. If the name is not unique, loading and saving will fail.
* Ensure that your toy class is XMLSerializable. As a rule of thumb, your class must implement a paramaterless constructor and all settings for the effect have to be available through public properties.
* Put your toys into a meaningfull subnamespace of the Cab.Toys namespace. If no suitable subnamespace exists, create a new subnamespace.
* Be fault tolerant! Handel all exceptions that might occur in your implementations. In particular, make sure that no exceptions are thrown if the outputs used by the toy do not exist and the reference to the output can not be resolved.
* If your toy needs timed events, register for the alarm function of the _Alarms_ object in the _Pinball_ object if possible. This will ensure that the necessary update functions are called and at the same time make sure that no unnessecary update calls to output controllers are triggered. 
* Make your code fast! In particular, resolve all references in the Init method.
* Implement a Finish method which turn off all outputs of the toy.
* Use methods to change the state of the toy (e.g. to turn on a contactor). Use public read and writable properties for the configuration of your toy. If you cant stick to that rule, be sure to add the necessary attributes to control the XML-serialization of the toy. 
* Last, but not least, please document your code! At least the public methods and properties should be properly documented with XML comments, since the docu on DOF is generated from these comments.



