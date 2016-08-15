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

- `0` - left return lane
- `1` - right return lane
- `2` - time maching
- `3` - lock alpha
- `4` - lock beta
- `5` - lock gamma
- `6` - lock delta
- `7` - crystal

#### Solenoids
	
- `0` - plunger
- `1` - through eject
- `2` - knocker
- `3` - left slingshot
- `4` - right slingshot
- `5` - left jet
- `6` - right jet,
- `7` - bottom jet
- `8` - left drops up
- `9` - right drops up
- `10` - lock release 1
- `11` - lock release 2
- `12` - lock release 3
- `13` - lock release A
- `14` - lock release B
- `15` - lock release C
- `16` - lock release D
- `17` - middle eject
- `18` - top eject strong,
- `19` - top eject weak
- `20` - middle ramp down
- `21` - high divertor
- `22` - low divertor
- `23` - scoop retract
- `24` - magno save
- `25` - magno lock

#### Flippers

- `0` - low left
- `1` - low right
- `2` - high right

#### Button Lights        

- `0` - start
- `1` - fire
- `2` - magnosave
