DirectOutput Framework R3++, Grand Unified Edition

DirectOutput is an add-in for Visual Pinball and other programs that
provides software control over external feedback devices in a virtual
pinball cabinet.  

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

This is the mjr "Grand Unified" edition of DOF, which merges all of
the known forks as of January 2018.  Several forks with different
add-on features have emerged since the last official DOF release from
SwissLizard in December 2015.  This edition is an attempt to re-unify
all of this work under a single version so that users don't have to
pick subsets of available features - just pick this version and you'll
have them all.

