# DOF Build Setup

The build is set up for Visual Studio 2022.  To perform a build, start Visual
Studio, and load the DOF solution (.sln) file.  It should take care of the build
dependency ordering.

If you want to be able to build the MSI installer, a free third-party tool
called WiX Toolset is required, and must be installed manually.  The MSI
installer is the last build step, so if you don't want to bother with WiX, you
can still build everything else; just ignore the errors on the last step where
VS tries to build the MSI.

## Migrating to newer Visual Studio versions

Moving a project to a newer version of Visual Studio is usually pretty easy, but
be aware that a newer Visual Studio might prompt you to change project settings
to update build tool versions and/or .NET component versions.  If you accept
those changes, **they will modify the build** and therefore might make it harder
to merge your changes into other people's forks.

Updates to .NET components require careful consideration because of the effect
they can have on deployment compatibility.  Some people use DOF with older
Windows versions, so a .NET update that only works with Windows 11, say, would
lock out a lot of users.  The virtual pin cab community doesn't have any sort of
official support policy that says which versions of Windows are still supported -
we're just an informal coalition of open-source developers, after all - but we
generally try to keep supporting older systems for longer than Microsoft does.
So .NET updates from Microsoft might well exclude older systems that are still
important to pin cab users.

If you do feel that a build tool update or .NET component update is in order,
it's helpful to **create a pull request or commit just for the tooling
changes**.  That's useful because it makes it easy to test **just** the project
file changes, in case a new bug appears afterward.  If the project file
changes are mixed into the same commit with other changes, it's harder to
isolate whether the bug is coming from the tool updates or the actual code changes.


## WiX setup (optional - required only for .msi creation)

The WiX toolset requires installing two separate packages:

1\. Install the WiX command-line tools from the github release page:
[https://github.com/wixtoolset/wix3/releases](https://github.com/wixtoolset/wix3/releases).
Grab the .EXE for version **3.14.0**, which is a self-installer; run it and
you'll be set.  **WARNING: Do not install 3.14.1;** see below.

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

Note that the non-working WiX 3.14.1 contains fixes for two security
vulnerabilities that are considered severe enough that the WiX developers advise
against using any earlier version.  (They also don't expect you to use 3.14.1,
given the show-stopping new bug mentioned above; they seem to consider v3 as a
whole fully bricked at this point.)  After studying the release notes for the
3.14.1 security fixes, I concluded that the vulnerabilities don't affect the DOF
MSI, because they're only triggered when invoking specific commands that the DOF
MSI doesn't use.  So I consider 3.14.0 safe for DOF use.

2\. Install the WiX add-in tools for Visual Studio. This is done within Visual
Studio: on the main menu, select Extensions > Manage Extensions to bring up the
Extension Manager window.  Select the Browse tab in the window.  Type **Wix v3**
into the search box.  Find **Wix v3 - Visual Studio 2022 Extension** in the
result list.  Click the Install button.


## Build procedure

Build from the Visual Studio GUI as follows:

1. In the configuration selector in the main toolbar, select the **Debug** or
**Release** build as desired.  (Most past DOF releases were built in Debug mode.
This isn't a very professional practice, but it's well-intentioned, in the hope
of protecting users from any surprises from differences in behavior between the
Debug build, which is typically well tested in the sense that it's what the
developers have been routinely running as they worked on it, and the Release
build, which they typically haven't run much, if at all.  In an ideal world, the
Release build would work exactly like the Debug build, just being faster and
smaller, but compilers aren't always perfect at achieving identical behavior
across the two configurations.  An alternative ideal world would be one in which
DOF is well staffed with QA engineers who've thoroughly tested the Release
configuration, but that's another ideal that is not always realized in
practice.)

2. In the configuration selector, choose the desired build architecture, **x86** or **x64**.
Each must be built separately.  **Build x86 first, then x64** (see below).

3. On the main menu, Build > Clean Solution

4. On the main menu, Build > Build Solution

This should build all of the DOF binaries plus the .msi installers, depositing the target
files in various subfolders.

**The build order is important:** Build the **x86** version **first**, then
build the x64 version.  Due to limitations of the WiX tools, the WiX setup
builder for 64-bit has to read from some of the x86 target files, so they have
to exist before the x64 build will complete successfully.  It's a stupid and
fragile setup, I know, but I can't find any other workaround for the WiX issue.
And Visual Studio unfortunately doesn't have any way that I know of to carry
out the x86 and x64 builds in sequence, so you just have to know to run the
two builds manually in that order.


## Release process

The release consists of a set of .msi files (Microsoft Setup install bundles)
and .zip files.

The .msi files are stand-alone MS Setup release bundles, containing all of the
necessary files to perform a complete install.  These are ready for distribution
to users without any further packaging.  A user simply downloads the .msi file
and double-clicks it, which will launch MS Setup and carry out the install.

You can also, optionally, build .zip files containing the install sets, for
people who want to do the install by hand rather than using the .msi files.  I'm
not sure it's worth doing this any more, since the automatic Setup-based install
is so much more reliable, but so far I've continued distributing the .zip files
out of habit.

If you're checking changes into git for the release, it's a good idea to **close
Visual Studio** before doing your final git commit.  Visual Studio sometimes
postpones writing changes to the project and solution files (among others) until
you exit, no matter how many times you hit Save All.


### New single-script release builder

The new batch script BuildRelease.bat lets you run the entire build-and-release with
a single command.  This must run from a CMD (DOS box) windows with the following
environment setup:

- PATH and environment variables for the Visual Studio version you're using

- PATH must include a command-line ZIP.EXE tool

On Windows 10/11, newer versions of Visual Studio automatically create a "Developer
Command Prompt for VS 20xx" configuration for the Terminal program ("Terminal" is
the new name for the CMD.EXE/DOS box on Windows 11).  Alternatively, you can use
the VCVARS.BAT script that Visual Studio traditionally provides in its own install
directory.  You must provide your own ZIP.EXE tool in a location on the path.

Once you have the CMD window set up, just CD to the main DirectOutput source folder
and run:

  BuildRelease AUTHOR

where AUTHOR is a short name identifying you as the author of this build.  I use
my initials "mjr" here.  This gets embedded in the final .zip and .msi filenames
to help people identify their source once released.  The point of this is just
to make it easier for people to keep track of where a particular download came
from, so that if they contact you about a problem, you can sanity-check that
they're in fact using a file that you built.  Some problems turn out to be as
simple as "you're using the wrong download".

BuildRelease performs the following steps:

- Clean the Release|x86 and Release|x64 configurations

- Build the Release|x86 and Release|x6r configurations

- Create the release MSI and ZIP files in the .\Builds folder

The MSI and ZIP files are named DirectOutput-AUTHOR-CONFIG-release-YYYYMMDD.msi
and .zip, where AUTHOR is the AUTHOR tag you included in the BuildRelease
command, CONFIG is x86 or x64, and YYYYMMDD is today's date. 


### Original manual release process

Here's the procedure I used before the single .bat script.

1. In Visual Studio, go through each configuration (Debug/x86, Debug/x64,
Release/x86, Release/x64) and run a **Build > Clean Solution**, to make sure that all old
build files are removed.

2. Now go through the configurations **in this order**: Debug/x86, Debug/x64,
Release/x86, Release/x64.  For each, run a **Build > Build Solution**.

   - On each build step, it's a good idea to check that the build actually
     succeeded, by looking at the end of the Output log window, and confirming that
     it says "=== Build: N succeeded, 0 failed, 0 skipped ===".  The important
     thing is the "0 failed" part.

3. Run the MakeZip steps (see below)

4. The Builds\ folder should now have a complete set of .msi and .zip files
to upload, with the filenames set to show today's date and thee respective
configurations.  The file naming makes these suitable for uploading to a
simple flat directory structure on a Web server or other repository.


How to build the ZIP files: The main folder contains a script, **MakebinZip.bat**,
that builds the .ZIP.  Run it from a CMD window after completing the Visual Studio 
build.  You'll need a command-line ZIP tool somewhere on your path, named ZIP.EXE.  
The script takes three arguments, selecting the binaries by the architecture and 
build configuration, with a third argument giving a short identifier tag (I use
my initials) to embed in the filenames, to help identify their source if you
distribute them online.

```
MakeZip x86 debug mjr
MakeZip x86 release mjr
MakeZip x64 debug mjr
MakeZip x64 release mjr

```

