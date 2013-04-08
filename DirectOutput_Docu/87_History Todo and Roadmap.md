History, ToDo and Roadmap {#HistoryTodoRoadmap}
====================

\section todo ToDo

This section contains a list of additions and ideas for the DirectOutput framework:

- Add DoxConfig to project/git.
- Modify LedControl and TableConfig loading to allow a mix of both. Add new property to table config to enable/disable this behaviour.
- Add AssignedToy and AssignedToysList to be used for effects. This would probably facilitaty the implementation of a configuration editor.
- Add conditions (expressions that evluate to true or false) for assigned effects. CSScript makes this easy, but unlopading of changed effects will not be possible without restarting the framework.
- Create a delay effect (effect which fires another effect after a specified delay). Maybe this could also be added the the EffectListEffect. When implementing, special caution is required since the value of the table element firing the efect has maybe changed when the delayed effect is fired.
- Invent some common, proper interface for the AutoConfig of Output controllers.
- EM support. Will need some extensions of the external interface.
- Log captured exceptions and other important events.
- Review LedControl code regarding exception handlinging/capturing.

\section roadmap Roadmap



\section history History


2013.3
-
Created GitHub repository for DirectOutput. Uploaded preliminary documentation.

2013.2
-
First working version of the framework, including plugin infrustructure of the B2S server. Works nicely on my cabinet :)

2013.1
-
First implementation using the B2S Server for table event feeds.

2012.12
-
Started development of the framework


