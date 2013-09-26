using DirectOutput.PinballSupport;

namespace DirectOutput.Cab.Toys.Basic
{

    /// <summary>
    /// Replayknocker toy which can fire the replay knocker one or several times at given intervalls.
    /// </summary>
    public class ReplayKnocker : DigitalToy, IToy
    {

        private int _DefaultIntervallMs = 300;

        /// <summary>
        /// Gets or sets the default intervall between knocks in milliseconds.<br/>
        /// Default value of this property is 300 milliseconds.
        /// </summary>
        /// <value>
        /// The default intervall in milliseconds.
        /// </value>
        public int DefaultIntervallMs
        {
            get { return _DefaultIntervallMs; }
            set { _DefaultIntervallMs = value; }
        }

        private int _KnockDurationMs = 30;

        /// <summary>
        /// Gets or sets the knock duration (power on time) in milliseconds.<br/>
        /// Default value of this property is set to 30ms or the value of Pinball.UpdateTimer.IntervalMs if this value is above 20 milliseconds.
        /// </summary>
        /// <value>
        /// The knock duration in milliseconds.
        /// </value>
        public int KnockDurationMs
        {
            get { return _KnockDurationMs; }


        }
        /// <summary>
        /// Fires the replay knocker once.
        /// </summary>
        public void Knock()
        {
            Knock(1);
        }


        /// <summary>
        /// Fires the replay knocker several times.<br/>
        /// The value of DefaultIntervallMs will be used for the intervall between knocks.
        /// </summary>
        /// <param name="NumberOfKnocks">Number of knocks</param>
        public void Knock(int NumberOfKnocks)
        {
            Knock(NumberOfKnocks, DefaultIntervallMs);
        }

        private int RemainingKnocks = 0;
        private int KnockIntervallMs = 0;

        /// <summary>
        /// Fires the replay knocker several times.
        /// </summary>
        /// <param name="NumberOfKnocks">Number of knocks.</param>
        /// <param name="IntervallMs">Intervall in milliseconds between knocks.</param>
        public void Knock(int NumberOfKnocks, int IntervallMs)
        {
            if (NumberOfKnocks < 1) return;

            RemainingKnocks = NumberOfKnocks;
            KnockIntervallMs = IntervallMs;

            SetState(true);
            RemainingKnocks--;

            AlarmHandler.RegisterAlarm(KnockDurationMs, KnockerOn);
        }

        private void KnockerOn()
        {
            SetState(false);
            if (RemainingKnocks > 0)
            {
                AlarmHandler.RegisterAlarm(KnockIntervallMs, KnockerOff);
            }
        }

        private void KnockerOff()
        {
            if (RemainingKnocks > 0)
            {
                SetState(true);
                RemainingKnocks--;
                AlarmHandler.RegisterAlarm(KnockDurationMs, KnockerOn);
            }
        }



        private AlarmHandler AlarmHandler;

        /// <summary>
        /// Initalizes the ReplayKnocker toy.
        /// </summary>
        /// <param name="Cabinet"><see cref="Cabinet" /> object to which the <see cref="ReplayKnocker" /> belongs.</param>
        public override void Init(Cabinet Cabinet)
        {
            AlarmHandler = Cabinet.Pinball.Alarms;
            if (DefaultIntervallMs < 2) { DefaultIntervallMs = 2; }
            base.Init(Cabinet);
        }

        /// <summary>
        /// Finishes the ReplayKnocker toy and releases used references.
        /// </summary>
        public override void Finish()
        {
            RemainingKnocks = 0;
            AlarmHandler = null;
            base.Finish();
        }
    }
}
