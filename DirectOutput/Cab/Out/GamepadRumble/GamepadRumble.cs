using System;
using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;

namespace DirectOutput.Cab.Out.GamepadRumble
{
    /// <summary>
    /// Output controller that turns DOF output activity into XInput gamepad
    /// vibration. It is intended for desktop setups without feedback hardware:
    /// the strongest active output (for example a DOF Shaker) drives both rumble
    /// motors of an Xbox/XInput controller.
    ///
    /// Configure it in the cabinet config (Cabinet.xml) and route the outputs of
    /// the LedWizEquivalent that carries your shaker to this controller.
    /// </summary>
    public class GamepadRumble : OutputControllerFlexCompleteBase
    {
        private int _ControllerIndex = 0;

        /// <summary>
        /// XInput controller slot to send the vibration to (0-3). Defaults to 0.
        /// </summary>
        public int ControllerIndex
        {
            get { return _ControllerIndex; }
            set { _ControllerIndex = (value < 0) ? 0 : ((value > 3) ? 3 : value); }
        }

        private int _Strength = 100;

        /// <summary>
        /// Rumble strength in percent (0-300). 100 maps a full output (255) to the
        /// maximum motor speed. Defaults to 100.
        /// </summary>
        public int Strength
        {
            get { return _Strength; }
            set { _Strength = (value < 0) ? 0 : ((value > 300) ? 300 : value); }
        }

        private string _PortWeights = "3=50";
        private int[] _WeightTable;

        /// <summary>
        /// Per-output weighting in percent, as a comma separated list of
        /// "port=percent" pairs (e.g. "3=50,2=120"). Ports that are not listed use
        /// 100%. This lets whole categories of feedback be toned down, because the
        /// port numbers mean the same thing on every table: with the standard DOF
        /// config tool assignment 1=Shaker, 2=Knocker, 3=Gear, 4/5=Flippers,
        /// 6/7=Slingshots, 8+=Bumpers.
        ///
        /// The default tones down gear motors, which are driven by the raw solenoid
        /// state and therefore stay on for as long as the motor runs.
        /// </summary>
        public string PortWeights
        {
            get { return _PortWeights; }
            set { _PortWeights = value; _WeightTable = null; }
        }

        private int _SustainMs = 500;

        /// <summary>
        /// How long an output may stay continuously on at full weight before the
        /// sustained-output attenuation starts (milliseconds). Short hits (knocker,
        /// bumpers, slingshots) are always below this and stay untouched.
        /// </summary>
        public int SustainMs
        {
            get { return _SustainMs; }
            set { _SustainMs = (value < 0) ? 0 : value; }
        }

        private int _SustainFadeMs = 800;

        /// <summary>
        /// Time it takes to fade a sustained output from full down to SustainLevel.
        /// </summary>
        public int SustainFadeMs
        {
            get { return _SustainFadeMs; }
            set { _SustainFadeMs = (value < 1) ? 1 : value; }
        }

        private int _SustainLevel = 30;

        /// <summary>
        /// Level in percent a continuously running output settles at (0 = silent).
        /// A real cabinet can happily run a motor for minutes; a gamepad buzzing at
        /// full power for the same time just gets annoying, so long running effects
        /// are faded back to a background hum.
        /// </summary>
        public int SustainLevel
        {
            get { return _SustainLevel; }
            set { _SustainLevel = (value < 0) ? 0 : ((value > 100) ? 100 : value); }
        }

        // Per-output "how long has this been on" tracking for the sustain fade.
        private int[] _OnMs;
        private uint _LastUpdateTick;
        private bool _HaveLastTick;

        private int GetWeight(int outputIndex)
        {
            if (_WeightTable == null)
            {
                var table = new int[64];
                for (int i = 0; i < table.Length; i++)
                    table[i] = 100;
                if (!string.IsNullOrEmpty(_PortWeights))
                {
                    foreach (var part in _PortWeights.Split(',', ';'))
                    {
                        var kv = part.Split('=');
                        int port, pct;
                        if (kv.Length == 2
                            && int.TryParse(kv[0].Trim(), out port)
                            && int.TryParse(kv[1].Trim(), out pct)
                            && port >= 1 && port <= table.Length)
                        {
                            table[port - 1] = (pct < 0) ? 0 : ((pct > 300) ? 300 : pct);
                        }
                    }
                }
                _WeightTable = table;
            }
            return (outputIndex >= 0 && outputIndex < _WeightTable.Length) ? _WeightTable[outputIndex] : 100;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct XInputVibration
        {
            public ushort LeftMotor;
            public ushort RightMotor;
        }

        [DllImport("xinput1_4.dll", EntryPoint = "XInputSetState")]
        private static extern int XInputSetState_1_4(int dwUserIndex, ref XInputVibration pVibration);

        [DllImport("xinput9_1_0.dll", EntryPoint = "XInputSetState")]
        private static extern int XInputSetState_9(int dwUserIndex, ref XInputVibration pVibration);

        private bool _Use14 = true;

        // Shared rumble channel so VPX (ball-contact flipper etc.) and this DOF
        // controller combine their vibration instead of overwriting each other.
        // Layout (16 bytes): [0]u32 vpxTick [4]u16 vpxL [6]u16 vpxR
        //                    [8]u32 dofTick [12]u16 dofL [14]u16 dofR
        // Both sides write their own slot and send max(own, other) to XInput.
        // [16]u16 dofScalePercent: per-table DOF rumble scale set by VPX's LiveUI
        // (1..300, 0 = unset -> 100%).
        private const string ShareName = "Local\\VPXDOFRumbleShare";
        private const uint StaleMs = 5000;
        private MemoryMappedFile _shareMmf;
        private MemoryMappedViewAccessor _share;

        private void InitShare()
        {
            if (_share != null) return;
            try
            {
                _shareMmf = MemoryMappedFile.CreateOrOpen(ShareName, 64);
                _share = _shareMmf.CreateViewAccessor(0, 64);
            }
            catch { _share = null; }
        }

        /// <summary>
        /// No hardware to verify - always valid.
        /// </summary>
        protected override bool VerifySettings()
        {
            return true;
        }

        /// <summary>
        /// Nothing to open for XInput; just make sure the motors start silent.
        /// </summary>
        protected override void ConnectToController()
        {
            InitShare();
            SetVibration(0);
            StartTicker();
        }

        /// <summary>
        /// Stop the motors when the controller is finished.
        /// </summary>
        protected override void DisconnectFromController()
        {
            StopTicker();
            lock (_ComputeLock)
            {
                _LastValues = null;
                _OnMs = null;
                _HaveLastTick = false;
            }
            SetVibration(0);
        }

        /// <summary>
        /// Called by the base class worker thread with the current value of every
        /// output. Since only tactile toys (e.g. the shaker) are routed here, the
        /// strongest output value drives the rumble.
        /// </summary>
        // The base class only calls UpdateOutputs when a value actually changes, so
        // a steady output (e.g. a gear motor that runs for seconds) would never be
        // re-evaluated. This ticker re-runs the computation so the sustain fade can
        // actually take effect while nothing changes.
        private byte[] _LastValues;
        private readonly object _ComputeLock = new object();
        private System.Threading.Thread _Ticker;
        private volatile bool _TickerRun;

        private void StartTicker()
        {
            if (_Ticker != null) return;
            _TickerRun = true;
            _Ticker = new System.Threading.Thread(TickerLoop);
            _Ticker.IsBackground = true;
            _Ticker.Name = "GamepadRumble sustain ticker";
            _Ticker.Start();
        }

        private void StopTicker()
        {
            _TickerRun = false;
            var t = _Ticker;
            _Ticker = null;
            if (t != null)
            {
                try { t.Join(200); }
                catch { }
            }
        }

        private void TickerLoop()
        {
            while (_TickerRun)
            {
                System.Threading.Thread.Sleep(50);
                if (!_TickerRun) return;
                try { Compute(); }
                catch { }
            }
        }

        protected override void UpdateOutputs(byte[] OutputValues)
        {
            lock (_ComputeLock)
            {
                _LastValues = (OutputValues == null) ? null : (byte[])OutputValues.Clone();
            }
            Compute();
        }

        private void Compute()
        {
            lock (_ComputeLock)
            {
                ComputeLocked(_LastValues);
            }
        }

        private void ComputeLocked(byte[] OutputValues)
        {
            uint now = (uint)Environment.TickCount;
            int elapsed = 0;
            if (_HaveLastTick)
            {
                long d = (long)(now - _LastUpdateTick);
                elapsed = (d < 0) ? 0 : ((d > 500) ? 500 : (int)d);
            }
            _LastUpdateTick = now;
            _HaveLastTick = true;

            // Live overrides pushed by VPX (Rumble/Haptics menu). 0 = not set, so
            // the values configured here in the cabinet config stay in effect.
            int sustainMs = _SustainMs, fadeMs = _SustainFadeMs, level = _SustainLevel;
            InitShare();
            if (_share != null)
            {
                try
                {
                    ushort s1 = _share.ReadUInt16(18);
                    ushort s2 = _share.ReadUInt16(20);
                    ushort s3 = _share.ReadUInt16(22);
                    if (s1 > 0) sustainMs = s1;
                    if (s2 > 0) fadeMs = s2;
                    if (s3 > 0) level = (s3 - 1 > 100) ? 100 : (s3 - 1); // stored as level+1
                }
                catch { }
            }

            int best = 0;
            if (OutputValues != null)
            {
                if (_OnMs == null || _OnMs.Length != OutputValues.Length)
                    _OnMs = new int[OutputValues.Length];

                for (int i = 0; i < OutputValues.Length; i++)
                {
                    int v = OutputValues[i];
                    if (v <= 0)
                    {
                        _OnMs[i] = 0;
                        continue;
                    }

                    // A) category weighting by port number
                    int val = v * GetWeight(i) / 100;

                    // B) sustained-output attenuation: fade anything that stays on
                    // for longer than SustainMs down towards SustainLevel.
                    int on = _OnMs[i] + elapsed;
                    if (on < 0) on = int.MaxValue; // overflow guard
                    _OnMs[i] = on;
                    if (on > sustainMs)
                    {
                        int over = on - sustainMs;
                        int factor = (over >= fadeMs)
                            ? level
                            : 100 - (100 - level) * over / fadeMs;
                        val = val * factor / 100;
                    }

                    if (val > best) best = val;
                }
            }

            // 0..255 -> 0..65535, scaled by Strength percent.
            long scaled = (long)best * 257 * _Strength / 100;
            if (scaled < 0) scaled = 0;
            if (scaled > 65535) scaled = 65535;
            SetVibration((int)scaled);
        }

        private void SetVibration(int motor)
        {
            // DOF drives the LOW-frequency (left) motor only - the heavy rumble
            // for game events. The HIGH-frequency (right) motor is left to VPX
            // (ball-on-flipper contact) so the two are felt as distinct effects
            // even when both fire at the same time.
            // Publish our value and combine with VPX's (max per motor), so neither
            // side stomps the other on the single XInput vibration channel.
            InitShare();

            // Per-table DOF rumble scale, set live from VPX's Rumble/Haptics menu.
            if (_share != null)
            {
                try
                {
                    ushort pct = _share.ReadUInt16(16);
                    if (pct >= 1 && pct <= 300)
                        motor = (int)((long)motor * pct / 100);
                    if (motor > 65535) motor = 65535;
                }
                catch { }
            }

            ushort cl = (ushort)motor; // low / left
            ushort cr = 0;             // high / right (owned by VPX)

            if (_share != null)
            {
                try
                {
                    uint now = (uint)Environment.TickCount;
                    _share.Write(8, now);
                    _share.Write(12, (ushort)motor); // dofLow
                    _share.Write(14, (ushort)0);     // dofHigh (unused)
                    uint vpxTick = _share.ReadUInt32(0);
                    if (now - vpxTick <= StaleMs)
                    {
                        ushort vl = _share.ReadUInt16(4);
                        ushort vr = _share.ReadUInt16(6);
                        if (vl > cl) cl = vl;
                        if (vr > cr) cr = vr;
                    }
                }
                catch { }
            }

            XInputVibration v;
            v.LeftMotor = cl;
            v.RightMotor = cr;
            try
            {
                if (_Use14)
                    XInputSetState_1_4(_ControllerIndex, ref v);
                else
                    XInputSetState_9(_ControllerIndex, ref v);
            }
            catch (DllNotFoundException)
            {
                // xinput1_4.dll only exists on Windows 8 and later; fall back.
                _Use14 = false;
                try { XInputSetState_9(_ControllerIndex, ref v); }
                catch { }
            }
            catch
            {
                // No controller connected or transient error - ignore.
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamepadRumble"/> class.
        /// </summary>
        public GamepadRumble()
        {
            Name = "GamepadRumble";
            NumberOfOutputs = 32;
        }

        /// <summary>
        /// Initializes a new instance with explicit settings.
        /// </summary>
        public GamepadRumble(string Name, int ControllerIndex, int Strength)
        {
            this.Name = Name;
            this.ControllerIndex = ControllerIndex;
            this.Strength = Strength;
            NumberOfOutputs = 32;
        }
    }
}
