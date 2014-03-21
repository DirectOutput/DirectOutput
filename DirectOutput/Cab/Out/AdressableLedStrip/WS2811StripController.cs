using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;
using System.Threading;

namespace DirectOutput.Cab.Out.AdressableLedStrip
{

    /// <summary>
    /// This output controller class is used to control the WS2811 led strip controller by Swisslizard.
    /// 
    /// The hardware of this controller is based on a Atmel microcontroller and a FT245R USB interface chip by FTDI. To ensure max performance all copde on the controller has been written in assembler.
    /// 
    /// WS2811 is a small controller chip which can controll a RGB led (256 PWM level on each channel) and be daisychained, so long cahins of LEDs (led strip are possible. The WS2812 understands the same protocoll as the WS2811, but is a RGB led with integrated controller chip which allows for even more dense populated RGB strips.
    ///
    /// Those controller chips are controlled using a single data line (there is no clock line). The data has to be sent with a frequency of 800khz. 1 bits have a duration of 0.65uS high and 0.6uS low. 0 bits have a duration of 0.25uS high and 1uS low. A interuption in the dataflow trigger the controller chips to push the data in the shift register to the PWM outputs. 
    ///
    /// \image html WS2811Controller.jpg
    /// This is a image of my controller prototype with classical through the hole parts and a small breakoutboard by SparkFun.
    /// </summary>
    public class WS2811StripController : OutputControllerBase, ISupportsSetValues
    {
        #region ISupportsSetValues Member

        /// <summary>
        /// Sets the values for one or several outputs of the controller.
        /// </summary>
        /// <param name="FirstOutput">The first output to be updated with a new value (zero based).</param>
        /// <param name="Values">The values to be used.</param>
        public void SetValues(int FirstOutput, byte[] Values)
        {
            if (FirstOutput >= LedData.Length) return;
            if (FirstOutput < 0) return;
            int CopyLength = (LedData.Length - FirstOutput).Limit(0, Values.Length);
            if (CopyLength < 1) return;

            lock (UpdateLocker)
            {
                Buffer.BlockCopy(Values, 0, LedData, FirstOutput, CopyLength);
                UpdateRequired = true;
            }
        }

        #endregion


        private byte[] LedData = new byte[0];

        private byte[] OutputLedData = new byte[0];


        private int _ControllerNumber = 1;

        /// <summary>
        /// Gets or sets the number of the controller.
        /// </summary>
        /// <value>
        /// The number of the WS2811 strip controller.
        /// </value>
        public int ControllerNumber
        {
            get { return _ControllerNumber; }
            set { _ControllerNumber = value; }
        }


        private int _NumberOfLeds = 1;

        /// <summary>
        /// Gets or sets the number of leds on the WS2811 based led strip.
        /// </summary>
        /// <value>
        /// The number of leds on the WS2811 based led strip.
        /// </value>
        public int NumberOfLeds
        {
            get { return _NumberOfLeds; }
            set
            {
                _NumberOfLeds = value.Limit(0, 4006);
                LedData = new byte[_NumberOfLeds * 3];
                OutputLedData = new byte[_NumberOfLeds * 3];
            }
        }




        private object UpdateLocker = new object();
        private bool UpdateRequired = true;






        private readonly int[] ColNrLookup = { 1, 0, 2 };
        /// <summary>
        /// This method is called whenever the value of a output in the Outputs property changes its value.<br />
        /// </summary>
        /// <param name="Output">The output which has triggered the event.</param>
        protected override void OnOutputValueChanged(IOutput Output)
        {
            if (Output is IOutputNumbered)
            {
                IOutputNumbered ON = (IOutputNumbered)Output;
                int ByteNr = ON.Number - 1;
                ByteNr = (ByteNr / 3) * 3 + ColNrLookup[ByteNr % 3];


                try
                {

                    lock (UpdateLocker)
                    {
                        if (LedData[ByteNr] != ON.Value)
                        {
                            LedData[ByteNr] = ON.Value;
                            UpdateRequired = true;
                        }

                    }
                }
                catch (Exception E)
                {
                    Log.Exception("WS2811StripController {0} with number {1} has received a update with a illegal or to high output number ({2}).".Build(Name, ControllerNumber, ON.Number), E);
                }
            }

        }


        private void AddOutputs()
        {
            for (int i = 1; i <= NumberOfLeds * 3; i++)
            {
                if (!Outputs.Any(x => ((LedStripOutput)x).Number == i))
                {
                    Outputs.Add(new LedStripOutput() { Name = "{0}.{1}".Build(Name, i), Number = i });
                }
            }
        }



        /// <summary>
        /// Initializes the output controller.
        /// </summary>
        /// <param name="Cabinet">The Cabinet object which is using the IOutputController instance.</param>
        public override void Init(Cabinet Cabinet)
        {
            AddOutputs();

            InitUpdaterThread();
            Log.Write("WS2811StripController {0} with number {1} initialized and updaterthread started.".Build(Name, ControllerNumber));

        }

        /// <summary>
        /// Finishes the output controller.
        /// </summary>
        public override void Finish()
        {
            FinishUpdaterThread();
            Log.Write("WS2811StripController {0} with number {1} finished and updaterthread stopped.".Build(Name, ControllerNumber));
        }

        /// <summary>
        /// Notifies the updater thread to sdend data to the controller hardware.
        /// </summary>
        public override void Update()
        {
            if (UpdateRequired)
            {
                UpdaterThreadSignal();
            }
        }



        #region UpdaterThread
        /// <summary>
        /// Initializes the updater thread.
        /// </summary>
        /// <exception cref="System.Exception">WS2811StripController {0} named {1} updater thread could not start.</exception>
        private void InitUpdaterThread()
        {

            if (!UpdaterThreadIsActive)
            {
                KeepUpdaterThreadAlive = true;
                try
                {
                    UpdaterThread = new Thread(UpdaterThreadDoIt);
                    UpdaterThread.Name = "WS2811StripController {0} named {1} updater thread ".Build(ControllerNumber, Name);
                    UpdaterThread.Start();
                }
                catch (Exception E)
                {
                    Log.Exception("WS2811StripController {0} named {1} updater thread could not start.".Build(ControllerNumber, Name), E);
                    throw new Exception("WS2811StripController {0} named {1} updater thread could not start.".Build(ControllerNumber, Name), E);
                }
            }
        }

        /// <summary>
        /// Finishes the updater thread.
        /// </summary>
        /// <exception cref="System.Exception">A error occured during termination of WS2811StripController updater thread.</exception>
        private void FinishUpdaterThread()
        {
            if (UpdaterThread != null)
            {
                try
                {
                    KeepUpdaterThreadAlive = false;
                    lock (UpdaterThreadLocker)
                    {
                        Monitor.Pulse(UpdaterThreadLocker);
                    }
                    if (!UpdaterThread.Join(1000))
                    {
                        UpdaterThread.Abort();
                    }
                    UpdaterThread = null;
                }
                catch (Exception E)
                {
                    Log.Exception("A error occured during termination of WS2811StripController updater thread.", E);
                    throw new Exception("A error occured during termination of WS2811StripController updater thread.", E);
                }
            }
        }


        /// <summary>
        /// Indicates whether the UpdaterThread of the WS2811StripController instance is active or not.
        /// </summary>
        public bool UpdaterThreadIsActive
        {
            get
            {
                if (UpdaterThread != null)
                {
                    if (UpdaterThread.IsAlive)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Signals the updater thread to continue its work (if currently sleeping).
        /// </summary>
        private void UpdaterThreadSignal()
        {

            lock (UpdaterThreadLocker)
            {
                Monitor.Pulse(UpdaterThreadLocker);
            }
        }


        private Thread UpdaterThread { get; set; }
        private object UpdaterThreadLocker = new object();
        private bool KeepUpdaterThreadAlive = true;




        private WS2811StripControllerApi Controller = null;

        /// <summary>
        /// This is the main method of the WS2811StripController object updater thread.
        /// </summary>
        private void UpdaterThreadDoIt()
        {
            if (Controller == null)
            {
                Controller = new WS2811StripControllerApi(ControllerNumber);
                if (!Controller.DeviceIsPresent)
                {
                    Log.Warning("WS2811 Strip Controller Nr. {0} is not present. Will not send updates.".Build(ControllerNumber));
                    Controller = null;
                }
            }


            OutputLedData.Fill((byte)0);
            if (Controller != null)
            {
                Controller.SetAndDisplayData(OutputLedData);
            }
            while (KeepUpdaterThreadAlive)
            {

                if (UpdateRequired)
                {
                    lock (UpdateLocker)
                    {
                        UpdateRequired = false;

                        Buffer.BlockCopy(LedData, 0, OutputLedData, 0, LedData.Length);
                    }

                    if (Controller != null)
                    {

                        Controller.SetAndDisplayData(OutputLedData);
                    }
                }

                if (KeepUpdaterThreadAlive)
                {
                    lock (UpdaterThreadLocker)
                    {
                        while (UpdateRequired == false && KeepUpdaterThreadAlive)
                        {
                            Monitor.Wait(UpdaterThreadLocker, 50);  // Lock is released while we’re waiting
                        }
                    }
                }

            }

            OutputLedData.Fill((byte)0);
            if (Controller != null)
            {
                Controller.SetAndDisplayData(OutputLedData);

                Controller.Close();
            }
            Controller = null;

        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="WS2811StripController"/> class.
        /// </summary>
        public WS2811StripController()
        {
            Outputs = new OutputList();
        }


    }
}
