# DOF Build Setup

The build is set up for Visual Studio 2022.  Load the solution (.sln) file, and
it should take care of the build dependency ordering.

If you want to be able to build the MSI installer, a free third-party tool
called WiX Toolset is required, and must be installed manually.  The MSI
installer is the last build step, so if you don't want to bother with WiX, you
can still build everything else; just ignore the errors on the last step where
VS tries to build the MSI.

The WiX toolset requires two separate install steps:

1\. Install the WiX command-line tools from the github release page:
[https://github.com/wixtoolset/wix3/releases](https://github.com/wixtoolset/wix3/releases).
Grab the .EXE for version **3.14.0**, which is a self-installer; run it and
you'll be set.

**Important:** As of this writing, you **MUST** use WiX version **3.14.0**.  I
can't tell you how much I hate it when open-source projects require installing
a specific out-of-date version of some tool; it's a sure sign that it's going to
be a nightmare to get the build working, and that probably even the original
developers aren't sure how the build really works.  So let me explain why I'm
asking the same thing, and why it's necessary in this case.  The DirectOutput
MSI builder is written in WiX v3, and Wix 3.14.0 is the last release on the WiX
v3 branch that works.  It's the last version of v3 that will **ever** work,
because the WiX developers have stated publicly that they've discontinued v3
work and will never release any more patches on that branch, *even though* the
final v3 release, 3.14.1, contains a known bug that makes it produce non-working
installs.  (Installs produced with 3.14.1 fail in a particularly spectacular
way, leaving your registry hosed and requiring manual cleanup.  Avoid at all
costs.)  WiX has moved on to v4 and v5, but those are thoroughly incompatible
with v3 scripts, requiring a substantial rewrite.  I've investigated doing that,
but decided that I'm not willing at this time because it's basically starting
from scratch, and the new WiX language is essentially undocumented.  It would be
a lot of work even if WiX v4/5 were documented, but the lack of documentation
makes it simply out of the question for me right now.  If anyone wants to
volunteer, though...

Note that the non-working WiX 3.14.1 contains a fix for two security
vulnerabilities that are considered severe enough that the WiX developers advise
against using any earlier version.  (They also don't expect you to use 3.14.1
given the show-stopping new bug mentioned above, so I guess they consider v3 to
be fully bricked at this point.)  After studying the release notes for the
3.14.1 security fix, I concluded that the vulnerabilities don't affect the DOF
MSI, because they're only triggered when invoking specific commands that the DOF
MSI doesn't use.  So I consider 3.14.0 safe for DOF use.

2\. Install the WiX add-in tools for Visual Studio. This is done within Visual
Studio: on the main menu, select Extensions > Manage Extensions to bring up the
Extension Manager window.  Select the Browse tab in the window.  Type **Wix v3**
into the search box.  Find **Wix v3 - Visual Studio 2022 Extension** in the
result list.  Click the Install button.

