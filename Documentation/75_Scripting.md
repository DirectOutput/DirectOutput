Scripting {#scripting}
==========

\section scripting_introduction Introduction 

The DirectOutput framework can be extended using the built in CSScript engine. Scripting allows for easy implemention of new effects, toys and output controllers.

\section scripting_guidelines Architecture

The scripting functionality of the DirectOutput framework has been implemnted using the <a target="_blank" href="http://www.csscript.net/index.html">CSScript engine</a>. More details on CSScript can found in the <a target="_blank" href="http://www.csscript.net/help/Online/index.html">online help on CSScript</a>.


\section scripting_guidelines Scripting guidelines

- Document your code. We all know the saying "It was hard to write, so it should be hard to understand as well", but this is definitvely not the goal of the DirectOutput project. You dont need to document every line of code you write, but at least the public function and the classes shoud be documented. The prefered way of code documentation are XML comments (have a look at the source code of the framework or one of the scripting examples to get a idea).
- Use descriptive names for classes, methods and namespaces.
- Think about exception handling. To ensure reliable operation of the framework, please implement proper exception handling for your classes.
- The framework is using <a target="_blank" href="http://support.microsoft.com/kb/815813/en-us">XMLSerialization</a> to save and load the configuration of the framework. This means that everything which has to be saved in the configuration must be XML serializable. If your class doesn't work with the built in XML serializer, you might need to implement the <a href="http://msdn.microsoft.com/en-us/library/system.xml.serialization.ixmlserializable.aspx">IXmlSerializable interface</a>.
- If you are working on effects, toys or output controllers, please read the implementation guidelines on the documention pages on these topics.

 






