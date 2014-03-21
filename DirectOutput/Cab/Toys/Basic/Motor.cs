using System.Xml.Serialization;
using DirectOutput.PinballSupport;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.Cab.Toys.Basic
{

    /// <summary>
    /// \deprecated The use of this toy is depreceated. A new version of this toy will be available later.
    /// 
    /// Motor toy supporting max. and min. power, max. runtime and kickstart settings.<br/>
    /// Inherits from GenericAnalogToy, implements IToy.
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




        private int _KickstartPower = 128;

        /// <summary>
        /// Gets or sets the kickstart power for the motor.<br/>
        /// If motor are run with low power they might not start to rotate without some initial kickstart.
        /// KickstartPower will only be applied if the motor is started with a power setting below the defined KickstartPower.<br/>
        /// Default value of this setting is 128.<br/>
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
                int P = FadingCurve.MapValue(Layers.GetResultingValue()).Limit(0, 255);

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

                            if (KickstartDurationMs > 0 && KickstartPower > 0)
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
                        TargetMotorPower = P;

                    }
                    else
                    {
                        //Motor is on
                        if (P != CurrentMotorPower)
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
