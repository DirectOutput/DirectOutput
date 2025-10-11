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
// '*' in the Build Number field tells Visual Studio to generate the
// Build Number and Revision fields, which it does as follows:  Build Number
// is set to the current date, expressed as the number of days since 1/1/2000,
// and Revision is set to the current time of day, as the number of seconds
// since midnight divided by 2.  Both are in the local time zone of the
// build machine.  This ensures that the Build Number monotonically increases
// in newer versions and that Build+Revision is unique per build, and also
// embeds the build timestamp in the assembly version data, which allows the
// build date to be determined at runtime.  Several components use this to
// report the build date in their logging mechanisms.
//
// IMPORTANT: The version string MUST be in the format "<major>.<minor>.*",
// with <major> and <minor> manually set to numbers and the literal "*" as
// the third element.  Several components depend upon "*" in the Build Number
// field to retrieve the build timestamp.  Changing the version string layout
// will break the build timestamp reports in the log.
//
// IMPORTANT: VB projects can't share this data!  Whenever the <major>.<minor>
// version numbers are updated, the VB components must be manually updated to
// keep them in sync.  The VB components should always use the identical
// version strings to avoid confusing users when troubleshooting.  The most
// common setup problem users have is mismatched components, so let's try to
// make things easier on users by using consistent component IDs.
//
[assembly: AssemblyVersion("3.3.*")]


// DOF-wide settings
[assembly: AssemblyCompany("DirectOutput")]
[assembly: AssemblyCopyright("Copyright © 2012-2025 Swisslizard")]
[assembly: AssemblyTrademark("")]

