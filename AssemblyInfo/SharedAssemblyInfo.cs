// Shared AssemblyInfo data for all DOF components
//
// This is a central, shared definition for all of the product-wide
// assembly info fields for most DirectOutput components.  We collect 
// the shared fields here so that only one copy needs to be maintained
// and updated.
//
// To use the shared settings in a C# project, do the following:
//
// 1. Add this file as a LINKED ITEM to the target project: right-click
//    the target project in Solution Explorer, navigate to this project
//    folder, click on (DON'T double-click) SharedAssemblyInfo.cs, then 
//    click the DROP ARROW next to the "Add" button and select Add As 
//    Link.
//
// 2. Make certain you actually added a LINK, not a copy, since making
//    a copy defeats the whole purpose.  In the target project, click
//    on SharedAssembly.cs.  Make sure that its FullPath shown in the
//    Properties pane is in the AssemblyInfo folder, not the target
//    project folder.
//
// 3. Edit the target project's AssemblyInfo.cs.  Remove the [assembly]
//    declarations from that file that also appear in this file.  Leave
//    all of the other items intact; anything that doesn't appear here
//    is intended to be project-specific and NOT shared.
//
// 4. Add the AssemblyInfo project as a dependency of the target project:
//    right-click the target project in Solution Explorer, select Build
//    Dependencies > Project Dependencies, then make sure the checkbox
//    for AssemblyInfo in the list is checked.
//
// It's not possible for VB projects to incorporate this C# source
// data, so VB projects must be updated separately to reflect the same
// changes you make here.  The VB equivalent file is AssemblyInfo.vb,
// listed in Solution Explorer under My Project, but note that it's
// hidden by default.  Click the "Show All Files" button in the 
// Solution Explorer panel toolbar to make it visible.
//

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// '*' in the Build Number or Revision fields cause Visual Studio to generate
// automatically updates values on each build.
//
// IMPORTANT: VB projects can't share this data, so any version updates must
// be propagated to VB projects manually.
//
[assembly: AssemblyVersion("3.1.*")]


// DOF-wide settings
[assembly: AssemblyCompany("DirectOutput")]
[assembly: AssemblyCopyright("Copyright ©  2012-2018 Swisslizard")]
[assembly: AssemblyTrademark("")]

