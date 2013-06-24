Table configuration
===================

\section tableconfig_intro Introduction

To make the DirectOutput framework work, it is necessary that the framework gets information on the status changes of the elements (e.g. bumpers or lamps) on the pinball table. Depending on the type of table there a different ways how to achieve this.

If you modify a table to use the B2S.Server, the table will require the B2S.Server to be installed otherwise it will not run. Apart from the B2S.Server there are no requirements. Even if you add special commands for the DirectOutput framework to the table script, the table will run without problemes if the framework is not installed.

\section tableconfig_VPSS VP solid state (SS) tables

\subsection tableconfig_VPSSconfig Configure SS tables

Visual Pinball solid state table are using PinMame for rom and game logic emulation. Therefore all data on changed table elements is anyway sent forth and back between PinMame and Visual Pinball. This makes the necessary changes to make DirectOutput and also the B2S.Server work with the table very easy.

The only change required is to replace the statement instanciating the PinMame object:

Replace the following line

~~~~~~~~~~~~~~~{.vbs}
Set Controller = CreateObject("VPinMAME.Controller")     
~~~~~~~~~~~~~~~

with this modifed version

~~~~~~~~~~~~~~~{.vbs}
Set Controller = CreateObject("B2S.Server") 
~~~~~~~~~~~~~~~

\subsection tableconfig_VPSSextend Extend SS tables

The config explained in the previous paragraph will usually be all you you to do to make a SS table work, but you have some more options.

If you want to tell the B2S.Server and the framework about events on the table which are not reported to PinMame, you can easily add some of the EM table commands of the B2S.Server to the table to send additionaly information to the B2S.Server.

This mix will work without any problem as long as the table is run in a environement which has the B2S.Server installed.

Please read the section \ref tableconfig_VPEMscore for information on the EM table commands. The helpfile of the B2S Backglass Designer does also contain information on EM table commands.


\section tableconfig_VPEM VP electro mechanical (EM) tables and original tables

To make VP EM and original tables work with B2S.Server and DirectOutput, some more work is required. 

Normally these table types dont send any information to the outside world and B2S.Server as well as DirectOutput would never know about changed table element statuses. Therefore it is necessary to add some statements which will inform the B2S.Sever about the changes of table elements to the table script.

\note I recommend, that you also read the section on EM tables in the help file of the B2S Backglass Designer.

\subsection tableconfig_VPEMinit Initialization

At the beginning of the table script, usually after the Dim statements, the B2S.Server has to be instanciated and initialized. This is done with the following statements:

~~~~~~~~~~~~~~~{.vbs}
Dim Controller
Set Controller = CreateObject("B2S.Server")
Controller.B2SName = "MyTable"
Controller.Run
~~~~~~~~~~~~~~~

The B2SName which is set in the third line of code is some kind of fake romname, which will be used by DirectOutput to identify the table config in a LedControl file. Be sure to use a simple and unique name for the B2SName.

\subsection tableconfig_VPEMexit Termination

It is important that the B2S.Server and the DirectOutput framework are informed, when the user exits the table. The following code will do this:

~~~~~~~~~~~~~~~{.vbs}
Sub Table1_Exit
    Controller.Stop
End Sub
~~~~~~~~~~~~~~~

Before copying the code fragement to the table script, please check if the Table Exit section does already exist in the table. If it does, insert only the _Controller.Stop_ statement.

\subsection tableconfig_VPEMscore Score commands

The B2S.Server has a bunch of special commands to forward information on the scores to the B2S.Server and the backglass. The DirectOutput framework does also receive the values from these commands.

For a list and explanation of commands to set the scores, please read the section on EM tables in the helpfile of the B2S Backglass Deginer.

\subsection tableconfig_VPEMscore Table element status updates

Apart from initializing the most important thing for the DirectOutput framework is to receive updates on table element changes.

To send this information to the B2S.Server and the framework, you have to use the _B2SSetData_ command of the B2S.Server. This commands accepts 2 parameters:

* _ID_ is the number of the table element changing its state or value. Just invent a numbering scheme of your choice for your table elements.
* _Value_ is value or status of the table element.

A complete _B2SSetData_ statement might looks as follows:
~~~~~~~~~~~~~~~{.vbs}
Controller.B2SSetData 123,1
~~~~~~~~~~~~~~~

Typically, the _B2SSetData_ satement will be put into the event handlers of the table elements. For a switch you'll maybe end up with something like this:

~~~~~~~~~~~~~~~{.vbs}
Sub sw5_Hit()
    ... some other event handling code ....
    
	Controller.B2SSetData 205,1
End Sub

Sub sw5_UnHit()
    ... some other event handling code ....
  Controller.B2SSetData 205,0
End Sub
~~~~~~~~~~~~~~~

It is important, that you also send data to the B2S.Server when the table element reverts back to it original state (e.g. in the UnHit event of a switch). If you only use a single _B2SSetData_ statement which always sends the same value for a element, the framework will not show any reaction, since the value does not change (DirectOutput reacts on value changes and NOT on the fact that data is sent).



