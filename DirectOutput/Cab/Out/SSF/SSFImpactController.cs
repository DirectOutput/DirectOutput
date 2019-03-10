using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ManagedBass;

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

        
        internal SoundBank bank = new SoundBank();
        internal List<String> myNames = new List<String>();
        internal List<SSFnoid> Contactors = new List<SSFnoid>();

        internal Assembly assembly = Assembly.GetExecutingAssembly();
        internal Stream SSF = Assembly.GetExecutingAssembly().GetManifestResourceStream("DirectOutput.Cab.Out.SSF.SSF");
        internal Stream SSFLI = Assembly.GetExecutingAssembly().GetManifestResourceStream("DirectOutput.Cab.Out.SSF.SSFLI"); //low intensity
        internal MemoryStream ssfStream = new MemoryStream();
        internal bool haveBass, useFaker = false;
        internal Faker fakeShaker;

        /// <summary>
        /// Init initializes the ouput controller.<br />
        /// This method is called after the
        /// objects haven been instanciated.
        /// 
        /// Specifically, Init is prepping the "Soundbank" for the currently supported 'hardware'
        /// and setting the user preffered output style/profile via presence, or lack of, 
        /// a file named "SSFLI" (Surround Sound Feedback - Low Intensity)
        /// </summary>
        /// <param name="Cabinet">The cabinet object which is using the output controller instance.</param>
        public override void Init(Cabinet Cabinet)
        {

            if (bank == null)
            {
                Log.Exception("Could not Initialize SSFImpactor");
                return;
            }

            if (SoundBank.Names.Count == 0)
            {
                try
                {
                    bank.PrepBox();
                    var info = Bass.Info;

                    if (File.Exists(@"C:\DirectOutput\SSFLI")) //Low Intensity Single channel
                    {
                        SSFLI.CopyTo(ssfStream);
                        SSF = null;
                    }
                    else 
                    {
                        SSF.CopyTo(ssfStream);
                        SSFLI = null;
                    }

                    
                   
                     useFaker = true;
                     fakeShaker = new Faker();
                     Log.Write("SSFShaker activated");
                    Log.Write("SSFImpactor \"Hardware\" Initialized\n");
                    haveBass = true;
                    AddOutputs();
                }
                catch (Exception e)
                {
                    Log.Write("Could Not Initialze Bass - " + e.Message);
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

            Outputs = null;
            try
            {
                Bass.Free();
            }
            catch (Exception)
            {
                Log.Write("Bass subsystem was not initialized");
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
                    if (outp.Number == 11 && useFaker)
                    {
                        fakeShaker.SetSpeed(outp.Value);
                        //Contactors[11].Value = outp.Value;
                       // Contactors[11].fired = true;
                        Log.Write("Playing " + outp.Name);
                        return;
                    }



                    if (Contactors[outp.Number].fired && (Contactors[outp.Number].Value == outp.Value))
                    {
                        Log.Write(String.Format("BYPASS:: Ouput.Number ->{0} Output.Value ->{1}", outp.Number, outp.Value));
                        return;
                    }

                    if (outp.Value != 0)
                    {

                        int stream = Bass.CreateStream(ssfStream.ToArray(), 0, ssfStream.Length, BassFlags.Default);
                        if (stream != 0)
                        {

                            if (outp.Number < 4 || outp.Number > 9)
                            {
                                Bass.ChannelSetAttribute(stream, ChannelAttribute.Volume, 1); //Per Rusty, do "front 4" and extras 'harder'
                            }
                            else
                            {
                                Bass.ChannelSetAttribute(stream, ChannelAttribute.Volume, 0.75); //pop bumpers, etc are further away, generally :)
                            }

                            Log.Write("Playing " + outp.Name);
                            Bass.ChannelPlay(stream);



                            //shaker experiement
                            Contactors[outp.Number].fired = true;
                            Contactors[outp.Number].Value= outp.Value;
                            while (Bass.ChannelIsActive(stream) == PlaybackState.Playing)
                            {
                                //need to let it play, Kai
                            }
                            //done this way as opposed to "one-liner" as I suspect that method has a leak...
                            Bass.StreamFree(stream);
                        }
                    }
                    else
                    {
                        Contactors[outp.Number].fired = false;
                        Contactors[outp.Number].Value = outp.Value;
                    }
                }


            }
            catch (Exception e)
            {

                Log.Write("UPDATE EXCEPTION:: " + e.Message);
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
                Log.Write(String.Format("BYPASS:: Ouput.Number ->{0} Outputs[{0}].Value ->{1}, current val not changed, non zero", Output.Number, Output.Value));
                return;
            }

            
                Outputs[Output.Number].Value = Output.Value;
               // Contactors[Output.Number].Value = Output.Value;
            

            


        }/// <summary>
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
                    Contactors.Add(new SSFnoid() { Number = i, Value = 0 });
                    myNames.Add(SoundBank.Names[i]);
                    Log.Write($"Added: {Outputs.Last().Name} to internal list...");

                }
            }
        }

    }

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


    class Faker
    {
        internal bool isShaking = false;
        public byte currentValue = 0;
        internal int startup, running, shutdown;
        internal Stream S1W = Assembly.GetExecutingAssembly().GetManifestResourceStream("DirectOutput.Cab.Out.SSF.S1W");
        internal Stream PE = Assembly.GetExecutingAssembly().GetManifestResourceStream("DirectOutput.Cab.Out.SSF.PE");
        internal MemoryStream startstream,runstream = new MemoryStream();

        static Faker()
        {
            Log.Write("Using SSFImpactor Shaking");
            

        }

        public void TurnOn()
        {
            if(isShaking)
            {
                return;
            }

            S1W.CopyTo(startstream);
            PE.CopyTo(runstream);
           
            startup = Bass.CreateStream(startstream.ToArray(), 0, startstream.Length, BassFlags.Default); //wobly ramp up
            Bass.ChannelSetAttribute(startup, ChannelAttribute.Volume, 1);
            Bass.ChannelPlay(startup);
            Log.Write("ShakerON");
            isShaking = true;
            while (Bass.ChannelIsActive(startup) == PlaybackState.Playing)
            {
                //need to let it play, Kai
            }
            //done this way as opposed to "one-liner" as I suspect that method has a leak...
            Bass.StreamFree(startup);
        }

        public void TurnOff()
        {
            //for now:
            if(isShaking && currentValue == 0)
            {
                Bass.ChannelPause(running);
                isShaking = false;
                Log.Write("ShakerOFF");

            }
        }


        public void SetSpeed(byte speed)
        {
            if (speed == 0)
            {
                TurnOff();
                currentValue = 0;
                
                return;
            }

            if (speed == currentValue) // reality: speed == currentValue
            {
                return;
            }
            Log.Write("ShakerSpped => " + speed.ToString());


            if(!isShaking)
            {
                TurnOn();
                currentValue = speed;
            }

            if(running == 0)
            {
                running = Bass.CreateStream(runstream.ToArray(), 0, runstream.Length, BassFlags.Default); //perfect loop sample
            }
            else
            {
                //already on, speed not implemented
                //"speed" to pitch or modifier here
                return;
            }
            
            Bass.ChannelSetAttribute(startup, ChannelAttribute.Volume, 1);
            Bass.ChannelAddFlag(running, BassFlags.Loop);
            Bass.ChannelPlay(running);

          


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

        public void PrepBox()
        {
            try
            {
                Bass.Init();
            }
            catch (Exception)
            {
                Log.Write("Could Not Initialze Bass. SSFImpactor disabled for events.");
                return;
            }

            ports = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8,9, 10, 11, 12, 13, 14 };
            names = new List<String> {  "FlipperLeft", "FlipperRight", "SlingshotLeft", "SlingshotRight",
                "10-BumperBackLeft", "10-BumperBackCenter","10-BumperBackRight",
                "10-BumperMiddleLeft", "10-BumperMiddleCenter", "10-BumperMiddleRight", "Knocker","Shaker","Gear",
                "HellBallMotor","Bell" }; ;

            int max = ports.Count - 1;

            Log.Write("Initializing SSFImpactor 'Hardware'...\n");

            for (int i = 0; i < max; i++)
            {
                Log.Write(String.Format("PORT {0}: {1}", i.ToString(),names[i]));
            }
           
            Log.Write(String.Format("SSFImpactor: {0} ready.", ports.Count.ToString()));
        }

    }

}
