using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ManagedBass;
using ManagedBass.Fx; //these have not been removed yet; reviewing mixer
using ManagedBass.Mix;

// <summary>
// This namespace contains a outputcontroller implementaion which isnt doing anything.
// </summary>
namespace DirectOutput.Cab.Out.SSFImpactController
{

    /// <summary>
    /// The SSFImpactor supports for using common audio bass shakers to emulate/simulate contractors/solenoids in a Virtual Pinball Cabinet.<br/>
    /// It presents itself as a full set of contactors, etc for assignment via DOF Config. Support, hardware suggestions, layout diagrams and the
    /// just about friendliest people in the hobby can be found here:  <a href="https://www.facebook.com/groups/SSFeedback/"/>
    /// <remarks>For help specifically with SSFImpactor look fo Kai "MrKai" Cherry.</remarks>
    /// </summary>
    public class SSFImpactController : OutputControllerBase, IOutputController
    {
        internal string _FrontExciters = "Rear";
        internal string _RearExciters = "RearCenter";
        internal string _Shaker1 = "RearCenter";
        internal string _Shaker2 = "Rear";
        private bool _LowImpactMode = false;
        internal int _DeviceNumber = -1;
        internal float _ImpactAmount = 1F;
        internal float _ShakeAmount = 1F;
        internal float _GearLevel = 0.65F;
        internal uint _FrontExciterPair = 0;  // Uint because BassFlags enum causes problem here?
        internal uint _RearExciterPair = 0;
        internal uint _ShakerChannel1 = 0;
        internal uint _ShakerChannel2 = 0;
        internal float _FlipperVolume = 0.25F;
        internal float _BumperVolume = 0.75F;
        internal float _SlingsEtAlVolume = 1.0F;

        //Create a speaker name dictionary to map speaker assignments to names.
        //The BassInfo enum can't be used because multiple BassInfo flag names
        //resolve to the same value.

        public static Dictionary<BassFlags, string> SpeakerNames = new Dictionary<BassFlags, string>() {
            { BassFlags.SpeakerFront, "SpeakerFront" },
            { BassFlags.SpeakerRear, "SpeakerRear" },
            { BassFlags.SpeakerCenterLFE, "SpeakerCenterLFE" },
            { BassFlags.SpeakerRearCenter, "SpeakerRearCenter" },
            //{ BassFlags.SpeakerPair1, "SpeakerPair1" },   // Same as SpeakerFront
            //{ BassFlags.SpeakerPair2, "SpeakerPair2" },   // Same as SpeakerRear
            //{ BassFlags.SpeakerPair3,"SpeakerPair3" },    // Same as SpeakerCenterLFE
            //{ BassFlags.SpeakerPair4,"SpeakerPair4" },    // Same as SpeakerRearCenter
            { BassFlags.SpeakerPair5,"SpeakerPair5" },
            { BassFlags.SpeakerPair6, "SpeakerPair6" },
            { BassFlags.SpeakerPair7, "SpeakerPair7" },
            { BassFlags.SpeakerPair8, "SpeakerPair8" },
            { BassFlags.SpeakerPair9, "SpeakerPair9" },
            { BassFlags.SpeakerPair10, "SpeakerPair10" },
            { BassFlags.SpeakerPair11, "SpeakerPair11" },
            { BassFlags.SpeakerPair12, "SpeakerPair12" },
            { BassFlags.SpeakerPair13, "SpeakerPair13" },
            { BassFlags.SpeakerPair14, "SpeakerPair14" },
            { BassFlags.SpeakerPair15, "SpeakerPair15" },
            { BassFlags.SpeakerLeft, "SpeakerLeft" },
            { BassFlags.SpeakerRight, "SpeakerRight" },
            { BassFlags.SpeakerFront | BassFlags.SpeakerLeft, "SpeakerFrontLeft" },
            { BassFlags.SpeakerRear | BassFlags.SpeakerLeft, "SpeakerRearLeft" },
            { BassFlags.SpeakerCenterLFE | BassFlags.SpeakerLeft, "SpeakerCenter" },
            { BassFlags.SpeakerRearCenter | BassFlags.SpeakerLeft, "SpeakerRearCenterLeft" },
            { BassFlags.SpeakerFront | BassFlags.SpeakerRight, "SpeakerFrontRight" },
            { BassFlags.SpeakerRear | BassFlags.SpeakerRight, "SpeakerRearRight" },
            { BassFlags.SpeakerCenterLFE | BassFlags.SpeakerRight, "SpeakerLFE" },
            { BassFlags.SpeakerRearCenter | BassFlags.SpeakerRight, "SpeakerRearCenterRight" }
        };
    
        /// <summary>
        /// Gets or sets speakers that SSF will send impactor samples to
        /// </summary>
        /// <value>
        /// One of the ManagedBass.Speaker* enums, i.e. 
        /// </value>
        public string FrontExciters
        {
            get { return _FrontExciters; }
            set { _FrontExciters = value; }
        }
        public string RearExciters
        {
            get { return _RearExciters; }
            set { _RearExciters = value; }
        }

        /// <summary>
        /// Gets or sets the channel where a BassShaker is located, or None to disable
        /// NB. If a bass shaker is on a .1 channel of a 2.1 exciter amp, choose the 
        /// exciter channel (eg, RearCenter for a Bass Shaker on the surround/side channel amp
        /// </summary>
        /// <value>
        /// One of the ManagedBass.Speaker* enums, or None
        /// </value>
        public string BassShaker1
        {
            get { return _Shaker1; }
            set { _Shaker1 = value; }
        }
        /// <summary>
        /// Gets or sets the optional/secondary BassShaker, or None to disable
        /// NB. If a bass shaker is on a .1 channel of a 2.1 exciter amp, choose the 
        /// exciter channel (eg, RearCenter for a Bass Shaker on the surround/side channel amp
        /// </summary>
        /// <value>
        /// One of the ManagedBass.Speaker* enums, or None
        /// </value>
        public string BassShaker2
        {
            get { return _Shaker2; }
            set { _Shaker2 = value; }
        }
        /// <summary>
        /// Switch to set if all exciters are used for Solenoids
        /// </summary>
        /// <value>
        /// true or false 
        /// </value>
        public string LowImpactMode
        {
            get { return _LowImpactMode.ToString(); }
            set { bool.TryParse(value, out _LowImpactMode); }
        }

        /// <summary>
        /// Gets or sets Audio output device used. Defaults to Default/Primary
        /// </summary>
        /// <value>
        ///Number representing the Device 
        /// </value>
        public int DeviceNumber
        {
            get { return _DeviceNumber; }
            set { _DeviceNumber = value; }
        }

        /// <summary>
        /// Sets the percent of strength of the signal from 0 (silent) to 100%
        /// </summary>
        /// <value>
        /// Number, 0 - 100
        /// </value>
        public float ImpactFactor
        {
            get { return _ImpactAmount; }
            set { _ImpactAmount = value/100; }
        }
        public float ShakerImpactFactor
        {
            get { return _ShakeAmount; }
            set { _ShakeAmount = value/100; }
        }
        public float FlipperLevel
        {
            get { return _FlipperVolume; }
            set { _FlipperVolume = value/100; }
        }
        public float BumperLevel
        {
            get { return _BumperVolume; }
            set { _BumperVolume = value/100; }
        }
        public float SlingsLevel
        {
            get { return _SlingsEtAlVolume; }
            set { _SlingsEtAlVolume = value/100; }
        }
        public float GearLevel
        {
            get { return _GearLevel; }
            set { _GearLevel = value / 100; }
        }

        internal SoundBank bank = new SoundBank();
        internal List<String> myNames = new List<String>();
        internal List<SSFnoid> Contactors = new List<SSFnoid>();

        internal Assembly assembly = Assembly.GetExecutingAssembly();
        internal Stream SSF = Assembly.GetExecutingAssembly().GetManifestResourceStream("DirectOutput.Cab.Out.SSF.SSF1C");
        internal MemoryStream ssfStream = new MemoryStream();
        internal bool haveBass, useFaker = false;
        internal Faker fakeShaker;
        internal SSFGear fakeGear;
        internal int stream = 0, stream1 = 0;

        /// <summary>
        /// Init initializes the ouput controller.<br />
        /// This method is called after the
        /// objects haven been instanciated.
        /// 
        /// Specifically, Init is prepping the "Soundbank" for the currently supported 'hardware'
        /// and setting the user preffered output style/profiles via the following options: 
        /// - Speakers: a valid named Speaker target regonized by Bass, eg 'Rear' or RearCenter'
        /// - BassShaker1: Location of "primary" Bass Shaker if not controlled be amp crossover, as recognized by Bass
        /// - BassShaker2: See Previous
        /// - LowImpactMode: true or false value determines if alternate routing/less overall intesity is used
        /// - DeviceNumber: Bass recognized soundcard output if not using Default system device. Devices are listed in DirectOutput.log
        /// Levels/Fine Tuning options: These all have values from 0 (minimum) to 100 (maximum)
        /// - ImpactFactor: Global Modifier for relative "effect" overall
        /// - ShakerImapactFactor: Fine tuning of Shaker intensity.
        /// - FlipperLevel: Fine tuning of the effect applied to the flippers
        /// - BumperLevel: same as above, for Pop Bumpers
        /// - SlingsLevel: the Slingshots and everything else
        /// </summary>
        /// <param name="Cabinet">The cabinet object which is using the output controller instance.</param>
        public override void Init(Cabinet Cabinet)
        {
            if (bank == null)
            {
                Log.Exception("Could not Initialize SSFImpactor");
                return;
            }

            try
            {
                _FrontExciterPair = (uint)(BassFlags)Enum.Parse(typeof(BassFlags), "Speaker" + _FrontExciters);
                _RearExciterPair = (uint)(BassFlags)Enum.Parse(typeof(BassFlags), "Speaker" + _RearExciters);
                if (_Shaker1 != "None")
                {
                    _ShakerChannel1 = (uint)(BassFlags)Enum.Parse(typeof(BassFlags), "Speaker" + _Shaker1);
                }
                if (_Shaker2 != "None")
                {
                    _ShakerChannel2 = (uint)(BassFlags)Enum.Parse(typeof(BassFlags), "Speaker" + _Shaker2);
                }
                

            }
            catch
            {
               Log.Write("Invalid value for a Speaker in Cabinet.xml, using defaults ");
                _FrontExciterPair = (uint)BassFlags.SpeakerRear;
                _RearExciterPair = (uint)BassFlags.SpeakerRearCenter;
                _ShakerChannel1 = (uint)BassFlags.SpeakerRearCenter;
                _ShakerChannel2 = (uint)BassFlags.SpeakerRear;
            }


            if (SoundBank.Names.Count == 0 || true)  // This should always be run
            {
                DeviceInfo mydevice;

                try
                {
                    bank.PrepBox(_DeviceNumber);
                    var info = Bass.Info;
                    try
                    {
                        for (int dev = 1; ; dev++)
                        {
                            var bd = Bass.GetDeviceInfo(dev);
                            Log.Write("BASS device " + dev.ToString() + " is " + bd.Name);
                        }
                    }
                    catch (Exception)
                    {
                        // You have to wait until GetDeviceInfo fails.  Yuck.
                    }

                    Log.Write("SSFImpactor DeviceNumber = " + _DeviceNumber);

                    if (_DeviceNumber == -1)
                    {
                        Log.Write("BASS using default Windows sound device");
                    }

                    Log.Write("BASS current device number = " + Bass.CurrentDevice);
                    mydevice = Bass.GetDeviceInfo(Bass.CurrentDevice);
                    Log.Write("BASS currnet device name: " + mydevice.Name);
                    Log.Write("BASS detects " + info.SpeakerCount.ToString() + " speakers.");

                    SSF.CopyTo(ssfStream);
                      
                    if(_LowImpactMode)
                    {
                        stream = Bass.CreateStream(ssfStream.ToArray(), 0, ssfStream.Length, (BassFlags)_FrontExciterPair);

                        if ((stream == 0) && (Bass.LastError == Errors.Speaker))
                        {
                            Log.Warning("Unable to assign FrontExciters stream to speakers: " + _FrontExciters);
                        }



                    }
                    else
                    { 
                        stream = Bass.CreateStream(ssfStream.ToArray(), 0, ssfStream.Length, (BassFlags)_FrontExciterPair);
                        if ((stream == 0) && (Bass.LastError == Errors.Speaker))
                        {
                             Log.Warning("Unable to assign FrontExciters stream to speakers: " + _FrontExciters);
                        }

                        stream1 = Bass.CreateStream(ssfStream.ToArray(), 0, ssfStream.Length, (BassFlags)_RearExciterPair);
                        if ((stream1 == 0) && (Bass.LastError == Errors.Speaker))
                        {
                            Log.Warning("Unable to assign RearExciters stream to speakers: " + _RearExciters);
                        }


                    }


                    useFaker = true;
                    fakeShaker = new Faker
                    {
                        Shaker1 = _Shaker1,
                        Shaker2 = _Shaker2,
                        ImpactEffect = _ShakeAmount
                    };

                    fakeGear = new SSFGear()
                    {
                        Shaker1 = _Shaker1,
                        GearLevel = _GearLevel
                    };

                    Log.Write("SSFShaker activated");
                    Log.Write("SSFImpactor \"Hardware\" Initialized\n");
                    haveBass = true;
                    AddOutputs();
                }
                catch (Exception e)
                {
                    Log.Error("Could Not Initialze Bass - " + e.Message);
                }
            }



        }

        /// <summary>
        /// Finishes the ouput controller.<br/>
        /// All necessary cleanup tasks have to be implemented here und all physical outputs have to be turned off.
        /// </summary>
        public override void Finish()
        {
            if (!haveBass)
            {
                Log.Write("!haveBass test kickout, Finish()");
                return;
            }

            if (stream != 0)
            {
                Bass.StreamFree(stream);
            }

            Outputs = null;
            try
            {
                Bass.Free();
            }
            catch (Exception)
            {
                Log.Error("Bass subsystem was not initialized");
            }

        }

        /// <summary>
        /// Update must update the physical outputs to the values defined in the Outputs list. 
        /// </summary>
        public override void Update()
        {

            try
            {
                foreach (IOutput outp in Outputs)
                {

                    if (outp.Number == 11)
                    {
                        fakeShaker.SetSpeed(outp.Value);
                        continue;
                    }

                    if (outp.Number == 12)
                    {
                        fakeGear.GearSet(outp.Value);
                        continue;
                    }

                    if (Contactors[outp.Number].fired && (Contactors[outp.Number].Value == outp.Value))
                    {

                        continue;
                    }

                    if (outp.Value != 0)
                    {
                       
                        if (stream != 0)
                        {
                            if (outp.Number < 4 || outp.Number > 9)
                            {
                                if (stream != 0)
                                {
                                    Bass.ChannelSetAttribute(stream, ChannelAttribute.Volume, _SlingsEtAlVolume * _ImpactAmount);
                                }
                                if (stream1 != 0)
                                {
                                    Bass.ChannelSetAttribute(stream1, ChannelAttribute.Volume, _SlingsEtAlVolume * _ImpactAmount);
                                }

                            }
                            else
                            {

                                if (stream != 0)
                                {
                                    Bass.ChannelSetAttribute(stream, ChannelAttribute.Volume, _BumperVolume * _ImpactAmount);
                                }
                                if (stream1 != 0)
                                {
                                    Bass.ChannelSetAttribute(stream1, ChannelAttribute.Volume, _BumperVolume * _ImpactAmount);
                                }

                            }


                            if (outp.Number < 2) //the flippers
                            {
                                //HOWEVER...flips don't need 'Full Hollywood' maybe :)
                                if (stream != 0)
                                {
                                    Bass.ChannelSetAttribute(stream, ChannelAttribute.Volume, _FlipperVolume * _ImpactAmount);
                                }
                                if (stream1 !=0)
                                {
                                    Bass.ChannelSetAttribute(stream1, ChannelAttribute.Volume, _FlipperVolume * _ImpactAmount);
                                }
                                
                            }

                            //"mixing" - .Mix seems to just not work "inside" vpx, so...
                          
                            //Log.Write("Firing " + outp.Name);
                            
                            if (_LowImpactMode == false) 
                            {
                                Bass.ChannelPlay(stream, true);
                                Bass.ChannelPlay(stream1, true);
                            }
                            else
                            {
                                Bass.ChannelPlay(stream, true); //lay off in LI mode
                            }

                            Contactors[outp.Number].fired = true;
                            Contactors[outp.Number].Value = outp.Value;
                            
                        }
                    }
                    else
                    {
                        Contactors[outp.Number].fired = false;
                        Contactors[outp.Number].Value = 0;
                    }
                }


            }
            catch (Exception e)
            {

                Log.Error("UPDATE EXCEPTION:: " + e.Message);
            }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SSFImpactController"/> class.
        /// </summary>
        public SSFImpactController()
        {
            Outputs = new OutputList();


        }


        /// <summary>
        /// This method is called whenever the value of a output in the Outputs property changes its value.<br />
        /// Due to some clever "orthogonal thinking", the whole of "hardware suite" can be controled here
        /// Hattip to djrobx for the idea!
        /// </summary>
        /// <param name="Output">The output.</param>
        protected override void OnOutputValueChanged(IOutput Output)
        {
            if (Output.Number > Contactors.Count - 1)
            {
                // Log.Write(String.Format("BYPASS:: Ouput.Number ->{0} Outputs[{0}].Value ->{1}, current val not changed, non zero", Output.Number, Output.Value));
                return;
            }


            Outputs[Output.Number].Value = Output.Value;
        }
        /// <summary>
        /// Adds the outputs from the SoundBank.<br/>
        /// This method adds OutputNumbered objects for all outputs to the list of outputs.
        /// </summary>
        internal void AddOutputs()
        {

            for (int i = 0; i <= SoundBank.Names.Count - 1; i++)
            {
                if (!Outputs.Any(x => x.Number == i))
                {
                    Outputs.Add(new Output() { Name = "{0}.{1:00}".Build(SoundBank.Names[i], i), Number = i, Value = 0 });
                    Contactors.Add(new SSFnoid() { Number = i, Value = 0 }); //yes, .11 is Shaker...
                    myNames.Add(SoundBank.Names[i]);
                    Log.Write($"Added: {Outputs.Last().Name} to internal list...");

                }
            }
        }

    }

    /// <summary>
    /// The SSFNoid is a simple class for storing state information on the virtual contactors.<br/>
    /// </summary>
    class SSFnoid
    {
        internal bool fired = false;
        public byte Value = 0;
        public int Number;

        static SSFnoid()
        {

        }

        public void Activate()
        {

        }
    }

    class SSFGear : SSFnoid
    {
        internal Stream OG = Assembly.GetExecutingAssembly().GetManifestResourceStream("DirectOutput.Cab.Out.SSF.OG");
        internal MemoryStream runstream = new MemoryStream();
        internal int running = 0;
        internal float _GearVolume = 1F;
        internal uint _ShakerChannel1 = 0;
        private string _BassShaker1 = "RearCenter";

        public string Shaker1
        {
            set
            {
                Log.Write("Shaker1 set to:" + value);
                if (value != "None")
                {
                    _ShakerChannel1 = (uint)(BassFlags)Enum.Parse(typeof(BassFlags), "Speaker" + value);
                }
                _BassShaker1 = value;
            }
        }

        public float GearLevel{

            set { _GearVolume = value; }
        }
       

        public void GearSet(byte state)
        {
            if (state > 0 && fired)
            {
                return;
            }
            else if (state > 0 && fired == false)
            {
                if (running == 0)
                {
                    if (runstream.Length == 0)
                    {
                        OG.CopyTo(runstream);
                    }

                    if (_BassShaker1 != "None")
                    {
                        running = Bass.CreateStream(runstream.ToArray(), 0, runstream.Length, (BassFlags)_ShakerChannel1); //perfect loop sample
                        Bass.ChannelAddFlag(running, BassFlags.Loop);
                        Bass.ChannelSetAttribute(running, ChannelAttribute.Volume, 1.0 * _GearVolume);

                        if ((running == 0) && (Bass.LastError == Errors.Speaker))
                        {
                            Log.Write("Unable to assign Shaker1 stream to speakers: " + _BassShaker1);
                            return;
                        }

                    }
                  
                }

                Bass.ChannelPlay(running);
                fired = true;
            }
            else
            {
                Bass.ChannelStop(running);
                fired = false;
            }

        }
    }

    /// <summary>
    /// The Faker is a software implementation of a variable shaker motor.<br/>
    /// </summary>
    class Faker
    {
        internal bool isShaking = false;
        public byte currentValue = 0;
        internal int running, running2;
        internal uint _ShakerChannel1 = 0;
        internal uint _ShakerChannel2 = 0;
        internal float _impactMod = 1.0F;
        private string _BassShaker1 = "RearCenter";
        private string _BassShaker2 = "Rear";
        internal Stream PE = Assembly.GetExecutingAssembly().GetManifestResourceStream("DirectOutput.Cab.Out.SSF.7hzOD"); //PE40Hz1s
        internal Stream PE1 = Assembly.GetExecutingAssembly().GetManifestResourceStream("DirectOutput.Cab.Out.SSF.7hzOD");
        internal MemoryStream runstream = new MemoryStream();
        internal MemoryStream runstream2 = new MemoryStream();
        

        public string Shaker1
        {
            set {
                Log.Write("Shaker1 set to:" + value);
                if (value != "None")
                {
                    _ShakerChannel1 = (uint)(BassFlags)Enum.Parse(typeof(BassFlags), "Speaker" + value);
                }
                _BassShaker1 = value;
            }
        }
        public string Shaker2
        {
            set
            {
                Log.Write("Shaker2 set to:" + value);
                if (value != "None")
                {
                    _ShakerChannel2 = (uint)(BassFlags)Enum.Parse(typeof(BassFlags), "Speaker" + value);
                }
                _BassShaker2 = value;

            }
        }
        public float ImpactEffect
        {
            set { _impactMod = value; }
        }

        static Faker()
        {
            Log.Write("Using SSFImpactor Shaking");


        }

        public void TurnOn()
        {
            if (isShaking)
            {
                return;
            }

            //S1W.CopyTo(startstream);
            PE.CopyTo(runstream);
            PE1.CopyTo(runstream2);

            Log.Write("Shaker::ON");
            isShaking = true;
        }

        public void TurnOff()
        {

            if (isShaking && currentValue == 0)
            {
                Bass.ChannelStop(running);
                Bass.ChannelStop(running2);
                isShaking = false;

                Log.Write("Shaker::OFF");

            }
        }

        /// <summary>
        /// Motor Variability is ultimately a volume control on the overdriven signal<br/>
        /// </summary>
        public void SetSpeed(byte speed)
        {
            if (speed == 0)
            {
                currentValue = 0;
                TurnOff();

                return;
            }

            if (speed == currentValue) // reality: speed == currentValue
            {
                return;
            }

            if (!isShaking)
            {
                TurnOn();
                currentValue = speed;
            }

            if (running == 0)
            {
                if (_BassShaker1 != "None")
                {
                    running = Bass.CreateStream(runstream.ToArray(), 0, runstream.Length, (BassFlags)_ShakerChannel1); //perfect loop sample
                    Log.Write("BassShaker set to speaker:" + SSFImpactController.SpeakerNames[(BassFlags)_ShakerChannel1]);
                    if ((running == 0) && (Bass.LastError == Errors.Speaker))
                    {
                        Log.Warning("Unable to assign Shaker1 stream to speakers: " + _BassShaker1);
                        return;
                    }
                }

                if (_BassShaker2 != "None")
                {
                    running2 = Bass.CreateStream(runstream2.ToArray(), 0, runstream.Length, (BassFlags)_ShakerChannel2);
                    Log.Write("Additional BassShaker set to speaker:" + SSFImpactController.SpeakerNames[(BassFlags)_ShakerChannel2]);
                    if ((running2 == 0) && (Bass.LastError == Errors.Speaker))
                    {
                        Log.Warning("Unable to assign Shaker2 stream to speakers: " + _BassShaker2);
                        return;
                    }
                }
                
                Bass.ChannelAddFlag(running, BassFlags.Loop);
                Bass.ChannelAddFlag(running2, BassFlags.Loop);
            }

            float myFloat = speed;
            Log.Write("FloatedSpeed::" + myFloat.ToString());
            Bass.ChannelSetAttribute(running, ChannelAttribute.Volume, (myFloat / 255) * _impactMod); //for variability:  (speed/255)
            Bass.ChannelSetAttribute(running2, ChannelAttribute.Volume, (myFloat / 255) * _impactMod); // * _impactMod
            Bass.ChannelPlay(running);
            Bass.ChannelPlay(running2);

        }

    }

    
    class SoundBank
    {
        private static List<int> ports = new List<int>();
        private static List<String> names = new List<String>();
        public static List<String> Names { get => names; set => names = value; }
        public static List<int> Ports { get => ports; set => ports = value; }

        //This is a separate class to allow for future expansion; eg parameters to change playback characteristics
        //It is a remnant of the 'soundbank'-based experiments, but future forward can serve a similar purpose via modifiers, etc
        //
        static SoundBank()
        {

        }

        public List<String> BankNames()
        {
            return names;
        }

        public void PrepBox(int DeviceNumber)
        {
            try
            {
                Bass.Init(DeviceNumber);
            }
            catch (Exception)
            {
                Log.Error("Could Not Initialze Bass. SSFImpactor disabled for events.");
                return;
            }

            ports = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            names = new List<String> {  "FlipperLeft", "FlipperRight", "SlingshotLeft", "SlingshotRight",
                "10-BumperBackLeft", "10-BumperBackCenter","10-BumperBackRight",
                "10-BumperMiddleLeft", "10-BumperMiddleCenter", "10-BumperMiddleRight", "Knocker","Shaker","Gear",
                "HellBallMotor","Bell" }; ;

            int max = ports.Count - 1;

            Log.Write("Initializing SSFImpactor 'Hardware'...\n");

            for (int i = 0; i < max; i++)
            {
                Log.Write(String.Format("PORT {0}: {1}", i.ToString(), names[i]));
            }

            Log.Write(String.Format("SSFImpactor: {0} ready.", ports.Count.ToString()));
        }

    }

}