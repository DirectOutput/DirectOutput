History, ToDo and Roadmap {#HistoryTodoRoadmap}
====================

\section todo ToDo

This section contains a list of additions and ideas for the DirectOutput framework:


- Modify LedControl and TableConfig loading to allow a mix of both. Add new property to table config to enable/disable this behaviour.
- Add AssignedToy and AssignedToysList to be used for effects. This would probably facilitaty the implementation of a configuration editor.
- Add conditions (expressions that evluate to true or false) for assigned effects. CSScript makes this easy, but unlopading of changed effects will not be possible without restarting the framework.
- Invent some common, proper interface for the AutoConfig of Output controllers.
- EM support. Will need some extensions of the external interface.
- Review LedControl code regarding exception handling/capturing.

\section roadmap Roadmap

The following features are planned for future releases:

- <b>DMX support</b><br/>
DMX is the industry standard protocol used to control stage lighting and effects. There is a huge number of light effects supporting DMX available on the market. DMX support will allow for the use of any DMX device, no matter if it is small or large.<br/>
Very likely the first DMX controller supported will be one of the smaller units from http://www.dmx4all.de/. Suggestions for other, lowcost controllers are welcome.


\section history History


2013.3
- Created GitHub repository for DirectOutput. Uploaded preliminary documentation.

2013.2
- First working version of the framework, including plugin infrustructure of the B2S server. Works nicely on my cabinet :)

2013.1
- First implementation using the B2S Server for table event feeds.

2012.12
- Started development of the framework.


