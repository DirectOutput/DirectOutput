B2S.Server Plugin
=================

The DirectOutput framework has been designed a a plugin for the B2S.Server. To allow the B2S.Server to discover and communiacte with the plugin the interface _IDirectPlugin_ has been implemented. To allow frontend calls as well the _IDirectPluginFrontend_ has been implemented as well.

\image html plugin.png "Plugin architecture"

The actual plugin interface of DirectOutput has been implemented as a separate DLL to facilitate reuse of the code.
