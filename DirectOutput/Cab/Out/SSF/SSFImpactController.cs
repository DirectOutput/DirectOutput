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
        internal Assembly assembly = Assembly.GetExecutingAssembly();
        internal Stream SSF = Assembly.GetExecutingAssembly().GetManifestResourceStream("DirectOutput.Cab.Out.SSF.SSF");
        internal Stream SSFLI = Assembly.GetExecutingAssembly().GetManifestResourceStream("DirectOutput.Cab.Out.SSF.SSFLI"); //low intensity
        internal MemoryStream ssfStream = new MemoryStream();
        internal bool haveBass = false;

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
            //Log.Write("Updates");
            
     
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

            if (Output.Number <= 1 || Output.Value == 42)
            {
                Log.Write(String.Format("BYPASS:: Ouput.Number ->{0} Output.Value ->{1}", Output.Number, Output.Value));
                return;
            }

                foreach (IOutput outp in Outputs)
                {
                    if (outp.Value == 255)
                    {

                        int stream = Bass.CreateStream(ssfStream.ToArray(), 0, ssfStream.Length, BassFlags.Default);
                        if (stream != 0)
                        {

                            if (outp.Number < 5 || outp.Number > 13)
                            {
                                Bass.ChannelSetAttribute(stream, ChannelAttribute.Volume, 1); //Per Rusty, do "front 4" and extras 'harder'
                            }
                            else
                            {
                                Bass.ChannelSetAttribute(stream, ChannelAttribute.Volume, 0.5); //pop bumpers, etc are further away, generally :)
                            }

                        Log.Write("Playing " + outp.Name);
                            Bass.ChannelPlay(stream);
                            Outputs.GetEnumerator().Current.Value = 42;

                            while (Bass.ChannelIsActive(stream) == PlaybackState.Playing)
                            {
                                //need to let it play, Kai
                            }
                            //done this way as opposed to "one-liner" as I suspect that method has a leak...
                            Bass.StreamFree(stream);
                        }
                    }
                }
            
            
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

                    myNames.Add(SoundBank.Names[i]);
                    Log.Write(String.Format("Added: {0} tointernal list...", Outputs.Last().Name));

                }
            }
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

            ports = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8,9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            names = new List<String> { "StartButton", "FlipperLeft", "FlipperRight", "SlingshotLeft", "SlingshotRight",
                "8-BumperCenter", "8-BumperRight", "8-BumperLeft", "10-BumperBackLeft", "10-BumperBackCenter","10-BumperBackLeft",
                "10-BumperMiddleLeft", "10-BumperMiddleCenter", "10-BumperMiddleRight", "Knocker","Shaker","Gear","LaunchButton",
                "AuthLaunchBall","FireButton","ExtraBall","Bell","HellBallMotor" }; ;

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
