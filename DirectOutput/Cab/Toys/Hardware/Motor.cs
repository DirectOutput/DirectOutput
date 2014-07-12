using System.Xml.Serialization;
using DirectOutput.PinballSupport;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.Cab.Toys.Hardware
{

    /// <summary>
    /// Motor toy supporting max. and min. power, max. runtime and kickstart settings.<br/>
    /// The settings of this toy allow for a detailed definition of the behaviour of the connected motor.
    ///
    /// * The KickstartPower and the KickstartDurationMs property allow you to define a initial stronger impulse for the start of the motor. Some hardware, e.g. like the Wolfsoft shaker in my cab, do not start to rotate when they are started with low power, giving them a initial pulse and the reducing to the lower power on the other hand is no problem. These properties allow you to define this behaviour.
    /// * The MinPower and MaxPower properties allow you to define the range of values which are allowed for the motor power. As a example, the shaker in my cab doesn't rotate with powers lower than 50 and starts to rattle and shake my cab appart with powers above 100. The normal value range for motor powers (0-255) is scaled to the range defined by the MinPower and MaxPower properties, so a value of 1 will result the shaker to operate on MinPower and 255 will result in MaxPower.
    /// * The FadingCurveName property can be used to specify the name of a predefined or user defined fading curve. This allows for even better fine tuning of the shaker power for different input values. It is recommended to use either the fading curve __Linear__ (default) and the MinPower/MaxPower settings or to use a fading curve and keep the MinPower/MaxPower settings on their defaults. If MinPower/MaxPower and a Fading Curve are combined, the power value for the motor will first be adjusted by the fading curve and the be scaled into the range of the MinPower/MaxPower settings.
    /// * MaxRunTimeMs allows you to define a maximum runtime for the motor before it is automatically turned off. To get reactivated after a runtime timeout the motor toy must first receive a power off (value=0), before it is reactivated.
    /// </summary>
    public class Motor : AnalogAlphaToy
    {
        private int _MaxRuntimeMs = 300000;

        /// <summary>
        /// Gets or sets the max run time for the toy in milliseconds.<br/>
        /// Default value of this property is 30000 (5 minutes).<br/>
        /// Set value to 0 for infinite runtime.
        /// </summary>
        /// <value>
        /// The max run time in milliseconds.
        /// </value>
        public int MaxRunTimeMs
        {
            get { return _MaxRuntimeMs; }
            set { _MaxRuntimeMs = value.Limit(0, int.MaxValue); }
        }




        private int _KickstartPower = 255;

        /// <summary>
        /// Gets or sets the kickstart power for the motor.<br/>
        /// If motor are run with low power they might not start to rotate without some initial kickstart.
        /// KickstartPower will only be applied if the motor is started with a power setting below the defined KickstartPower.<br/>
        /// Default value of this setting is 255.<br/>
        /// Set value to 0 to skip kickstart.
        /// </summary>
        /// <value>
        /// The kickstart power for the motor.
        /// </value>
        public int KickstartPower
        {
            get { return _KickstartPower; }
            set { _KickstartPower = value.Limit(0, 255); }
        }

        private int _KickstartDurationMs = 100;

        /// <summary>
        /// Gets or sets the kickstart duration (time during which the KickstartPower is applied) in milliseconds.<br/>
        /// Property defaults to 100 milliseconds.<br/>
        /// Set value to 0 to skip kickstart.
        /// </summary>
        /// <value>
        /// The kickstart duration in milliseconds.
        /// </value>
        public int KickstartDurationMs
        {
            get { return _KickstartDurationMs; }
            set { _KickstartDurationMs = value.Limit(0, 5000); }
        }

        private int _MinPower=1;

        /// <summary>
        /// Gets or sets the minimal power for the toy when it is active.
        /// </summary>
        /// <value>
        /// The minimal power for the toy. This property can be used to ensure that the motor will allways rotate when it is active and not getting stuck due to low power.
        /// </value>
        public int MinPower
        {
            get { return _MinPower; }
            set { _MinPower = value.Limit(0,255); }
        }


        private int _MaxPower=255;

        /// <summary>
        /// Gets or sets the max power for the toy. 
        /// </summary>
        /// <value>
        /// The max power for the toy. 
        /// </value>
        public int MaxPower
        {
            get { return _MaxPower; }
            set { _MaxPower = value.Limit(0,255); }
        }
        


        /// <summary>
        /// Power of the motor.<br/>
        /// Value is scaled using MinPower and MaxPower settings.
        /// </summary>
        //[XmlIgnoreAttribute]
        //public int Power
        //{
        //    get
        //    {
                
        //        return (int)((Value - MinPower) * ((double)(MaxPower - MinPower) / 255));
        //    }
        //}


        int CurrentMotorPower=0;
        int TargetMotorPower = 0;
        bool KickstartActive = false;
        bool TurnedOffAfterMaxRunTime = false;

        /// <summary>
        /// Updates the output of the toy.
        /// </summary>
        public override void UpdateOutputs()
        {
            if (Output != null)
            {
                int P = FadingCurve.MapValue(GetResultingValue().Limit(0, 255));

                if (P != 0)
                {
                    P=((int)((double)(MaxPower>=MinPower?MaxPower-MinPower:MinPower-MaxPower)/255*P)+MinPower).Limit(MinPower,MaxPower);
                }




                if (P == 0)
                {
                    TurnedOffAfterMaxRunTime = false;
                }

                if (!TurnedOffAfterMaxRunTime)
                {
                    if (CurrentMotorPower == 0)
                    {
                        //Motor is currently off
                        if (P > 0)
                        {
                            //need to turn the motor on

                            if (KickstartDurationMs > 0 && KickstartPower > 0 && P<=KickstartPower)
                            {
                                //Kickstart is defined, start with kickstart

                                TargetMotorPower = P;

                                if (!KickstartActive)
                                {
                                    CurrentMotorPower = KickstartPower;
                                    Output.Value = (byte)CurrentMotorPower;
                                    KickstartActive = true;
                                    AlarmHandler.RegisterAlarm(KickstartDurationMs, KickStartEnd);
                                }

                            }
                            else
                            {
                                //Just turn the motor on
                                CurrentMotorPower = P;
                                TargetMotorPower = P;
                                Output.Value = (byte)P;
                                KickstartActive = false;

                            }

                            if (MaxRunTimeMs > 0)
                            {
                                AlarmHandler.RegisterAlarm(MaxRunTimeMs, MaxRunTimeMotorStop);

                            }

                        }
                    }
                    else if (KickstartActive)
                    {
                        //Motor is in kickstart phase
                        if (P > 0)
                        {
                            //Need to change the motor power
                            TargetMotorPower = P;
                        }
                        else
                        {
                            //Turn off motor
                            AlarmHandler.UnregisterAlarm(KickStartEnd);
                            AlarmHandler.UnregisterAlarm(MaxRunTimeMotorStop);
                            TargetMotorPower = 0;
                            CurrentMotorPower = 0;
                            Output.Value = 0;
                        }
                    }
                    else
                    {
                        //Motor is on
                        if (P == 0)
                        {
                            //Turn of motor
                            AlarmHandler.UnregisterAlarm(KickStartEnd);
                            AlarmHandler.UnregisterAlarm(MaxRunTimeMotorStop);
                            TargetMotorPower = 0;
                            CurrentMotorPower = 0;
                            Output.Value = 0;
                        }
                        else if (P != CurrentMotorPower)
                        {
                            //Power has changed
                            CurrentMotorPower = P;
                            TargetMotorPower = P;
                            Output.Value = (byte)P;
                        }
                    }

                }
            }
        }

        private void MaxRunTimeMotorStop()
        {
            AlarmHandler.UnregisterAlarm(KickStartEnd);
            KickstartActive = false;
            CurrentMotorPower = 0;
            TargetMotorPower = 0;
            Output.Value = 0;
            TurnedOffAfterMaxRunTime = true;
        }


        private void KickStartEnd()
        {
            KickstartActive = false;
            CurrentMotorPower = TargetMotorPower;
            Output.Value = (byte)CurrentMotorPower;
        }





        private AlarmHandler AlarmHandler;

        /// <summary>
        /// Initalizes the Motor toy.
        /// </summary>
        /// <param name="Cabinet"><see cref="Cabinet" /> object to which the <see cref="Motor" /> belongs.</param>
        public override void Init(Cabinet Cabinet)
        {
            base.Init(Cabinet);
            AlarmHandler = Cabinet.Pinball.Alarms;

        }

        /// <summary>
        /// Finishes the Motor toy and releases used references.
        /// </summary>
        public override void Finish()
        {
            AlarmHandler.UnregisterAlarm(KickStartEnd);
            AlarmHandler.UnregisterAlarm(MaxRunTimeMotorStop);
            AlarmHandler = null;
            base.Finish();
        }

    }
}
