B2S Server Plugin {#b2sserverplugin}
=================


The DirectOutput framework implements the plugin interface for the great B2S.Server. This allows the B2S.Server to recognize and load the framework.

To allow the B2S.Server to discover and communicate with DirectOutput the interface _IDirectPlugin_ has been implemented. To allow frontend calls as well the _IDirectPluginFrontend_ has been implemented as well.

\image html plugin.png "Plugin architecture"

The actual plugin interface of DirectOutput has been implemented as a separate DLL to facilitate reuse of the code.


