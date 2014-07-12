
namespace DirectOutput.Cab.Toys.Hardware
{
    /// <summary>
    /// GearMotor toy supporting max. and min. power, max. runtime and kickstart settings.<br/>
    /// The settings of this toy allow for a detailed definition of the behaviour of the connected motor.
    ///
    /// * The KickstartPower and the KickstartDurationMs property allow you to define a initial stronger impulse for the start of the motor. Some hardware, e.g. like the Wolfsoft shaker in my cab, do not start to rotate when they are started with low power, giving them a initial pulse and the reducing to the lower power on the other hand is no problem. These properties allow you to define this behaviour.
    /// * The MinPower and MaxPower properties allow you to define the range of values which are allowed for the motor power. As a example, the shaker in my cab doesn't rotate with powers lower than 50 and starts to rattle and shake my cab appart with powers above 100. The normal value range for motor powers (0-255) is scaled to the range defined by the MinPower and MaxPower properties, so a value of 1 will result the shaker to operate on MinPower and 255 will result in MaxPower.
    /// * The FadingCurveName property can be used to specify the name of a predefined or user defined fading curve. This allows for even better fine tuning of the shaker power for different input values. It is recommended to use either the fading curve __Linear__ (default) and the MinPower/MaxPower settings or to use a fading curve and keep the MinPower/MaxPower settings on their defaults. If MinPower/MaxPower and a Fading Curve are combined, the power value for the motor will first be adjusted by the fading curve and the be scaled into the range of the MinPower/MaxPower settings.
    /// * MaxRunTimeMs allows you to define a maximum runtime for the motor before it is automatically turned off. To get reactivated after a runtime timeout the motor toy must first receive a power off (value=0), before it is reactivated.
    /// </summary>
    public class GearMotor: Motor
    {
    }
}
