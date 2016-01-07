Troubleshooting {#troubleshooting}
===============

\section troubleshooting_b2sserver Check B2S.Server Path

One of the most common problems with DOF installations is caused by multiple B2S.Server installation on a computer. There have been several cases where users have been very certain that they only had one B2S.Server installation, but futher research showed that there was more than one anyway.

When VP loads the B2S.Server it doesnt load the B2S.Server from your table path or Visuual Pinball path. Instead it loads the B2S.Server by its COM-object name. For this COM-object name windows is trying to find the path for the DLL it has to load from the registry. Typically the B2S.Server which is loaded is the last one which has been registered.

To determine which B2S.Server instance is beeing loaded, you have to check the registry of windows. To find the path of the B2SBackglassServer.dll which is loaded, when the B2S.Server is instanciated, please do the following (all screenshots from Win7):

__Step 1: Start Registry Editor__

The first thing to do is to start the Registry Editor. Click the Windows Start Button and enter regedit in the search textbox of your start menu. Once you see the Registry Editor in the search results, click it to start.

\image html B2SServerCheck_Step1_Regedit.png "Start the Registry Editor"

__Step 2: Registry Editor started__

After the Registry Editor has been started you should see the following window.

\image html B2SServerCheck_Step2_RegeditOpen.png "Registry Editor started"

__Step 3: Search for B2S.Server__

Press CTRL+F or select search from the menu to open the search window and enter _B2S.Server_. Click the button to start searching.

\image html B2SServerCheck_Step3_SearchB2SServer.png "Search window"

__Step 4: B2S.Server search result__

If the B2S.Server is registred in your machine, you should get a search result which looks like the following screenshot. 
Your search result should have a subkey (subfolder) called CLSID (short for class id). Navigate to this subkey.

\image html B2SServerCheck_Step4_SearchResultB2SServer.png "B2S.Server search result"

__Step 5: Copy the class ID__

In the right part of the Registry Editor window, you'll find a single entry which contains a cryptic class ID (The class id is a so called global unqiue identifier and those look always very cryptic).
Doubleclick this entry to open the editor window for this value. Mark the whole class id and copy the class id to your clipboard (CTRL+C or copy in the context menu).

\image html B2SServerCheck_Step5_SearchOpenCLassID.png "Copy the class id"

__Step 6: Search for the class ID__

Press CTRL+F or select search from the menu to open the search window and paste that class id which you have copied in the previous step into the search window (CTRL+V or past from contect menu).

\image html B2SServerCheck_Step5_SearchOpenCLassID.png "Search for the class id"

__Step 7: Get the B2S.Server DLL path__

If your search is successfull you should find a node in the Registry Editors node tree on the left side which is named like the class id you have been looking for. This node should have a subnode called _InProcServer32_. Click this subnode to see its data.
The entry _CodeBase_ will contain the path to the B2S.Server dll which is loaded when the B2S.Server object is called by its name.

Please check if this is the path you are expecting for the B2S.Server and also check if the B2S.Server installation in this path is also the one for which you want to load DOF.

\image html B2SServerCheck_Step7_SearchClassIDResult.png "B2S.Server path in Codebase"


