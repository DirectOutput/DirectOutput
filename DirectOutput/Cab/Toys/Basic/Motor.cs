using System.Xml.Serialization;
using DirectOutput.Cab.Toys.Generic;
using DirectOutput.PinballSupport;

namespace DirectOutput.Cab.Toys.Basic
{

    /// <summary>
    /// Motor toy supporting max. and min. power, max. runtime and kickstart settings.<br/>
    /// Inherits from GenericAnalogToy, implements IToy.
    /// </summary>
    public class Motor : GenericAnalogToy, IToy
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


        private int _MinPower = 10;

        /// <summary>
        /// Gets or sets the minimal power for the toy.<br/>
        /// Motors beeing run with very low power might tend to stutter or block. Setting this property to a meaningfull value will ensure that motors are always properly running.<br/>
        /// Default value of this property is 10.
        /// </summary>
        /// <value>
        /// The minimal power for the motor.
        /// </value>
        public int MinPower
        {
            get { return _MinPower; }
            set { _MinPower = value.Limit(0, 255); }
        }

        private int _MaxPower = 255;

        /// <summary>
        /// Gets or sets the maximum power (e.g. to ensure that you cabinet is shaken into pieces by a powerfull shaker motor) for the motor controlled by the toy.<br/>
        /// Default value of the property is 255.
        /// </summary>
        /// <value>
        /// The maximum power for the motor.
        /// </value>
        public int MaxPower
        {
            get { return _MaxPower; }
            set { _MaxPower = value.Limit(0, 255); }
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
        /// Power of the motor.<br>
        /// Value is scaled using MinPower and MaxPower settings.
        /// </summary>
        [XmlIgnoreAttribute]
        public int Power
        {
            get
            {
                
                return (int)((Value - MinPower) * ((double)(MaxPower - MinPower) / 255));
            }
        }


        int MotorPower;
        bool KickstartActive = false;
        /// <summary>
        /// Sets the power of the motor.<br/>
        /// Value range for Power is 0-255. Power will be scaled using the MinPower and MaxPower settings.
        /// </summary>
        /// <param name="Power">Power of the gear motor.</param>
        public void SetPower(int Power)
        {

            if (this.Power != Power)
            {

                MotorPower = Power;

                if (!KickstartActive)
                {

                    if (KickstartPower > 0 && Power<KickstartPower && KickstartDurationMs > 0)
                    {
                        KickstartActive = true;
                        SetValue(KickstartPower);
                        AlarmHandler.RegisterAlarm(KickstartDurationMs, StartMotor);
                    }
                    else
                    {

                        StartMotor();
                    }
                }

            }
        }

        private void StartMotor()
        {
            KickstartActive = false;
            if (MotorPower == 0)
            {
                StopMotor();
            }
            else
            {
                SetMotorPower(MotorPower);
                if (MaxRunTimeMs > 0)
                {
                    AlarmHandler.UnregisterAlarm(StartMotor);
                    AlarmHandler.RegisterAlarm(MaxRunTimeMs, StopMotor);
                }
            }
        }

        private void SetMotorPower(int Power)
        {
            int ScaledPower = (int)(MinPower + (MaxPower - MinPower) / 255 * (double)MotorPower).Limit(MinPower, MaxPower);
            SetValue(ScaledPower);
        }

        private void StopMotor()
        {
            KickstartActive = false;
            SetValue(0);
            AlarmHandler.UnregisterAlarm(StartMotor);
            AlarmHandler.UnregisterAlarm(StopMotor);
        }



        private AlarmHandler AlarmHandler;

        /// <summary>
        /// Initalizes the Motor toy.
        /// </summary>
        /// <param name="Cabinet"><see cref="Cabinet" /> object to which the <see cref="Motor" /> belongs.</param>
        public override void Init(Cabinet Cabinet)
        {
            AlarmHandler = Cabinet.Pinball.Alarms;

            base.Init(Cabinet);
        }

        /// <summary>
        /// Finishes the Motor toy and releases used references.
        /// </summary>
        public override void Finish()
        {
            AlarmHandler.UnregisterAlarm(StartMotor);
            AlarmHandler.UnregisterAlarm(StopMotor);
            AlarmHandler = null;
            base.Finish();
        }

    }
}
