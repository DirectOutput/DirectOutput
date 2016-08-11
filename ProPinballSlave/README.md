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

### Configuration

