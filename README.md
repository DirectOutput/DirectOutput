# DirectOutput Framework R3++, Grand Unified Edition

DirectOutput is an add-in for Visual Pinball and other programs that
provides software control over external feedback devices in a virtual
pinball cabinet.

## Now with 32/64-bit support

As of August 2024, this DOF release supports 32- and 64-bit installs,
and supports installing both of them together, *in the same folder*,
sharing a common set of configuration files.  I know people have been
managing to get DOF working in 64-bit mode for a while now, but the
new "official" support will hopefully make things a lot easier and
a lot more seamless.  In particular, the 32-bit and 64-bit editions
now both have fully automated .msi installers, and you can install
them together in a single folder so that they share a common set
of configuration files.

## Installation

DOF releases now offer **two** Windows .msi installer files, one for
32-bit, one for 64-bit.  You can take your pick of 32-bit only, 64-bit
only, or both.  The 32-bit version is required if you use any 32-bit
DOF-enabled applications, and the 64-bit version is required if you
use any 64-bit DOF-enabled applications.  Most people still use a mix
of 32-bit and 64-bit programs, so most people will want to install
both DOF versions.

If you only want to install **one of** the 32-bit **or** 64-bit
editions, just run the .msi installer for the one you want, and ignore
the other one.

To install both editions together, run **both** .msi installs, one at a
time.  The order doesn't matter.  Assuming that you want to use a
common set of configuration files for both editions, **install both
editions in the same folder**.

It's best to keep the two editions in sync by installing the **same
release version** for both editions.  If you already have a previous
32-bit version installed, and you want to add the 64-bit build, you
should **also** update your existing 32-bit DOF to the same new
release, so that everything is on the same build version.

The reason that it's now possible to install both editions in a single
folder, with shared config files, is that the new installer separates
the program files into 32-bit and 64-bit subfolders off of the main
install directory.  The new folders are called **x86** for the 32-bit
program files and **x64** for the 64-bit program files.  The DOF
programs all now understand that they're installed in these
subfolders, so they know to look for the configuration files in the
shared parent folder, ensuring that both editions use the same config
files.  All of the registry keys and other references are also aware
of the subfolder structure.  There's nothing you have to move around
or fix up by hand - the two editions should just happily coexist now.

If, for some reason, you'd actually prefer to keep the configuration
files for the 32-bit and 64-bit versions separated, that's easy,
too: just install each edition in a separate root folder.

### Install folder naming

The recommended install folder name is **C:\DirectOutput**.  This
isn't required, but it's one of those things where you might save
yourself some hassle by sticking with what everyone else uses.

**DO NOT** install DOF **anywhere** within the Windows **C:\Program Files**
tree or any other Windows system folders.  Windows has some special
security rules for system folders that can make DOF malfunction if
installed there.  It's best to use a custom folder like C:\DirectOutput.

In the distant past, some people ran into mysterious problems (that
were never explained or understood) that *seemed* to be related to
using path names that contained spaces, and/or were on drives other
than C:\ (or, more specifically, the drive containing Windows).  If
DOF is acting weird or just not working, and your install path sounds
like what I just described, you might try reinstalling in that
C:\DirectOutput folder as a first troubleshooting step just to rule
out the mysterious space/E: problem.


## About DOF

Feedback devices are things like lights, beacons, solenoids, shaker
motors, and gear motors that augment the "video game" action with
audio, visual, and tactile effects.  These feedback devices are
physically connected to the PC through an "output controller",
typically a USB device.  A variety of output controllers are in common
use, including LedWiz, PacLed, SainSmart USB relay boards, and
open-source systems such as Pinscape.  

DOF acts as a hardware virtualization layer: it provides a common
interface to the different hardware devices so that the pinball
simulator software doesn't have to speak 10 different USB protocols.
DOF also handles all details of effects timing and device state
management, so that the pinball simulator doesn't have to know
anything about the physical devices; it merely sends DOF data on the
abstract game events, and DOF takes care of mapping the game events to
device effects, mapping the device effects to hardware states that
evolve over time, and mapping the hardware states to the output
controller protocol commands necessary to effect same.

DOF documentation can be found at http://directoutput.github.io/DirectOutput/


## History of this edition

DOF was originally created by a fellow who went by SwissLizard.  He
retired from DOF development in 2015, but he kindly made the source
available on github under a permissive open-source license, allowing
DOF to live on and continue to improve, thanks to the work of many
subsequent contributors.  In the years after SwissLizard's departure,
several people created forked version of DOF with their own individual
feature additions.

Around 2018, I merged together all of the active forks that I
could find, creating what I called the MJR Grand Unified edition.
The "R3++" name is a riff on SwissLizard's final release, which he called R3,
with the pluses signifying two major rounds of reunification of
divergent forks that followed.  That's what you're looking at in this repository, assuming that you're
looking at this page on [https://github.com/mjrgh/DirectOutput/](https://github.com/mjrgh/DirectOutput/).

Since creating that first unified edition, I've tried to maintain this
repository as the quasi "official", reference edition of DOF, by
trying to keep it up to date with new work that other developers are
doing on DOF.  I encourage anyone working on adding their own new
features to please contribute their changes back to this version via a
pull request, so that the pin cab community can continue to have a
single reference edition with all of the latest features under one
roof.  The big problem that developed before the first unification was
that new features became mutually exclusive when they only existed on
different forks, so users had to choose *which* new features they
wanted.  We'd all like to avoid lapsing back into that sort of
incoherent mess again.
