# Pro Pinball

[Pro Pinball](http://www.pro-pinball.com/) is one of the best commercial 
pinball simulations out there. It has recently added support for real DMDs
as well as external force feedback.

This is an executable that sends all feedback signals to DOF.

### Installation

1. Make sure [DOF is installed](https://directoutput.github.io/DirectOutput/installation.html) 
2. Copy `DOFSlave.exe` to Pro Pinball's installation folder, usually at `%PROGRAMFILES(X86)%\Steam\SteamLibrary\steamapps\common\Pro Pinball Ultra`.
3. In Steam, right-click on Pro Pinball, set launch options and add `fDOFSlave`. 
   You'll also need the `m2` or `m3` parameter depending on your setup, otherwise
   the executable won't be called.
4. Add an environment variable called `DOF_HOME` pointing to your DOF installation 
   (the folder where `config` sits in).
5. Create `GlobalConfig_Dmdext.xml` with your config (TBD)

### Configuration

Pro Pinball supports four different type of events. They are mapped like this:

- Flashers -> Lamp (`L`)
- Solenoids -> Solenoids (`S`)
- Flippers -> Solenoids (`S`), starting at 128
- Button Lights -> Leds (`D`)

The following is a list of the IDs assigned to each event:

#### Flashers

- `L0` - left return lane
- `L1` - right return lane
- `L2` - time maching
- `L3` - lock alpha
- `L4` - lock beta
- `L5` - lock gamma
- `L6` - lock delta
- `L7` - crystal

#### Solenoids
	
- `S0` - plunger
- `S1` - through eject
- `S2` - knocker
- `S3` - left slingshot
- `S4` - right slingshot
- `S5` - left jet
- `S6` - right jet,
- `S7` - bottom jet
- `S8` - left drops up
- `S9` - right drops up
- `S10` - lock release 1
- `S11` - lock release 2
- `S12` - lock release 3
- `S13` - lock release A
- `S14` - lock release B
- `S15` - lock release C
- `S16` - lock release D
- `S17` - middle eject
- `S18` - top eject strong,
- `S19` - top eject weak
- `S20` - middle ramp down
- `S21` - high divertor
- `S22` - low divertor
- `S23` - scoop retract
- `S24` - magno save
- `S25` - magno lock

#### Flippers

- `S128` - low left
- `S129` - low right
- `S130` - high right

#### Button Lights        

- `D0` - start
- `D1` - fire
- `D2` - magnosave
