# Bridge Library for Pro Pinball

Pro Pinball uses Boost's [message queue](http://www.boost.org/doc/libs/1_60_0/doc/html/boost/interprocess/message_queue.html)
for sending feedback data to a separate process. Pro Pinball can launch a custom process for supporting
third party modules such as DOF by using the `f<executable>` option when launching the game.

From [Adrian's post](https://www.pro-pinball.com/forum/viewtopic.php?f=22&t=778):

> 1.2.1 also added support for a 'feedback' slave. This is sent a message approximately 60 times per second containing the current intensity of each flasher (0.0 off, 1.0 maximum brightness), the on/off state of each solenoid, and the on/off state of the start, fire and magnosave button lights (not displayed in the game itself, though used to show/hide buttons in the mobile version). These should be suitable for driving real hardware via e.g. an Ledwiz. The game does not provide a feedback slave, but the source package contains code and an executable for a dummy slave that just prints out state changes to the console. The feedback slave is started by adding the command line option f followed by the executable name, e.g. 'fFeedbackSlave'.

The goal of this bridge is to provide a DLL which can be easily used by any language able to import
COM libraries.