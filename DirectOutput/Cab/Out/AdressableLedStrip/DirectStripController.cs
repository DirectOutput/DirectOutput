using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;
using System.Threading;

namespace DirectOutput.Cab.Out.AdressableLedStrip
{

    /// <summary>
    /// This output controller class is used to control the direct strip controller by Swisslizard.
    /// 
    /// The hardware of this controller is based on a Atmel microcontroller and a FT245R USB interface chip by FTDI. To ensure max performance all copde on the controller has been written in assembler.
    /// 
    /// WS2811 is a small controller chip which can controll a RGB led (256 PWM level on each channel) and be daisychained, so long cahins of LEDs (led strip are possible. The WS2812 understands the same protocoll as the WS2811, but is a RGB led with integrated controller chip which allows for even more dense populated RGB strips.
    ///
    /// Those controller chips are controlled using a single data line (there is no clock line). The data has to be sent with a frequency of 800khz. 1 bits have a duration of 0.65uS high and 0.6uS low. 0 bits have a duration of 0.25uS high and 1uS low. A interuption in the dataflow triggers the controller chips to push the data in the shift register to the PWM outputs. Since the timing requirements are very strict it is not easily possible to output that signal directly from a computer with normal operating system. Thats why controllers like the one displayed below are needed.
    ///
    /// \image html WS2811Controller.jpg
    /// This is a image of my controller prototype with classical through the hole parts and a small breakoutboard by SparkFun.
    /// At the time of the release of DOF R2, the first prototypes of SMD version of the controller are in production. Check back in the forums for more information.
    /// </summary>
    public class DirectStripController : OutputControllerBase, ISupportsSetValues
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



        #region ControllerNumber property of type int with events
        #region ControllerNumber property core parts
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
            set
            {
                if (_ControllerNumber != value)
                {
                    OnControllerNumberChanging();
                    _ControllerNumber = value;
                    OnControllerNumberChanged();
                }
            }
        }

        /// <summary>
        /// Fires when the ControllerNumber property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> ControllerNumberChanging;

        /// <summary>
        /// Fires when the ControllerNumber property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> ControllerNumberChanged;
        #endregion

        /// <summary>
        /// Is called when the ControllerNumber property is about to change its value and fires the ControllerNumberChanging event
        /// </summary>
        protected void OnControllerNumberChanging()
        {
            if (ControllerNumberChanging != null) ControllerNumberChanging(this, new EventArgs());

            //Insert more logic to execute before the ControllerNumber property changes here
        }

        /// <summary>
        /// Is called when the ControllerNumber property has changed its value and fires the ControllerNumberChanged event
        /// </summary>
        protected void OnControllerNumberChanged()
        {
            //Insert more logic to execute after the ControllerNumber property has changed here
            OnPropertyChanged("ControllerNumber");
            if (ControllerNumberChanged != null) ControllerNumberChanged(this, new EventArgs());
        }

        #endregion


        #region NumberOfLeds property of type int with events
        #region NumberOfLeds property core parts
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
                if (_NumberOfLeds != value)
                {
                    OnNumberOfLedsChanging();
                    _NumberOfLeds = value.Limit(0, 4006);
                    LedData = new byte[_NumberOfLeds * 3];
                    OutputLedData = new byte[_NumberOfLeds * 3];
                    OnNumberOfLedsChanged();
                }
            }
        }

        /// <summary>
        /// Fires when the NumberOfLeds property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsChanging;

        /// <summary>
        /// Fires when the NumberOfLeds property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsChanged;
        #endregion

        /// <summary>
        /// Is called when the NumberOfLeds property is about to change its value and fires the NumberOfLedsChanging event
        /// </summary>
        protected void OnNumberOfLedsChanging()
        {
            if (NumberOfLedsChanging != null) NumberOfLedsChanging(this, new EventArgs());

            //Insert more logic to execute before the NumberOfLeds property changes here
        }

        /// <summary>
        /// Is called when the NumberOfLeds property has changed its value and fires the NumberOfLedsChanged event
        /// </summary>
        protected void OnNumberOfLedsChanged()
        {
            //Insert more logic to execute after the NumberOfLeds property has changed here
            OnPropertyChanged("NumberOfLeds");
            if (NumberOfLedsChanged != null) NumberOfLedsChanged(this, new EventArgs());
        }

        #endregion

        #region PackData property of type bool with events
        #region PackData property core parts
        private bool _PackData = false;

        /// <summary>
        /// Gets or sets a value indicating whether the data which is sent to the controller should be packed.
        /// Data packing uses a simple IFF (Interchangable File Format) like system.
        /// </summary>
        /// <value>
        ///   <c>true</c> if data should be packed bore it is sent to the controller; otherwise <c>false</c> (default). 
        /// </value>
        public bool PackData
        {
            get { return _PackData; }
            set
            {
                if (_PackData != value)
                {
                    OnPackDataChanging();
                    _PackData = value;
                    OnPackDataChanged();
                }
            }
        }

        /// <summary>
        /// Fires when the PackData property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> PackDataChanging;

        /// <summary>
        /// Fires when the PackData property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> PackDataChanged;
        #endregion

        /// <summary>
        /// Is called when the PackData property is about to change its value and fires the PackDataChanging event
        /// </summary>
        protected void OnPackDataChanging()
        {
            if (PackDataChanging != null) PackDataChanging(this, new EventArgs());

            //Insert more logic to execute before the PackData property changes here
        }

        /// <summary>
        /// Is called when the PackData property has changed its value and fires the PackDataChanged event
        /// </summary>
        protected void OnPackDataChanged()
        {
            //Insert more logic to execute after the PackData property has changed here
            OnPropertyChanged("PackData");
            if (PackDataChanged != null) PackDataChanged(this, new EventArgs());
        }

        #endregion


        private object UpdateLocker = new object();
        private bool UpdateRequired = true;






        private readonly int[] ColNrLookup = { 1, 0, 2 };
        /// <summary>
        /// This method is called whenever the value of a output in the Outputs property changes its value.<br />
        /// </summary>
        /// <param name="ChangedOutput">The output which has triggered the event.</param>
        protected override void OnOutputValueChanged(IOutput ChangedOutput)
        {
            if (ChangedOutput is IOutput)
            {
                IOutput ON = ChangedOutput;
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
                    Log.Exception("DirectStripController {0} with number {1} has received a update with a illegal or to high output number ({2}).".Build(Name, ControllerNumber, ON.Number), E);
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
        /// <param name="Cabinet">The cabinet object which is using the IOutputController instance.</param>
        public override void Init(Cabinet Cabinet)
        {
            AddOutputs();

            InitUpdaterThread();
            Log.Write("DirectStripController {0} with number {1} initialized and updaterthread started.".Build(Name, ControllerNumber));

        }

        /// <summary>
        /// Finishes the output controller.
        /// </summary>
        public override void Finish()
        {
            FinishUpdaterThread();
            Log.Write("DirectStripController {0} with number {1} finished and updaterthread stopped.".Build(Name, ControllerNumber));
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
        /// <exception cref="System.Exception">DirectStripController {0} named {1} updater thread could not start.</exception>
        private void InitUpdaterThread()
        {

            if (!UpdaterThreadIsActive)
            {
                KeepUpdaterThreadAlive = true;
                try
                {
                    UpdaterThread = new Thread(UpdaterThreadDoIt);
                    UpdaterThread.Name = "DirectStripController {0} named {1} updater thread ".Build(ControllerNumber, Name);
                    UpdaterThread.Start();
                }
                catch (Exception E)
                {
                    Log.Exception("DirectStripController {0} named {1} updater thread could not start.".Build(ControllerNumber, Name), E);
                    throw new Exception("DirectStripController {0} named {1} updater thread could not start.".Build(ControllerNumber, Name), E);
                }
            }
        }

        /// <summary>
        /// Finishes the updater thread.
        /// </summary>
        /// <exception cref="System.Exception">A error occured during termination of DirectStripController updater thread.</exception>
        private void FinishUpdaterThread()
        {
            if (UpdaterThread != null)
            {
                try
                {
                    KeepUpdaterThreadAlive = false;
                    UpdaterThreadSignal();
                    if (!UpdaterThread.Join(1000))
                    {
                        UpdaterThread.Abort();
                    }
                    UpdaterThread = null;
                }
                catch (Exception E)
                {
                    Log.Exception("A error occured during termination of DirectStripController updater thread.", E);
                    throw new Exception("A error occured during termination of DirectStripController updater thread.", E);
                }
            }
        }


        /// <summary>
        /// Indicates whether the UpdaterThread of the DirectStripController instance is active or not.
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




        private DirectStripControllerApi Controller = null;

        /// <summary>
        /// This is the main method of the DirectStripController object updater thread.
        /// </summary>
        private void UpdaterThreadDoIt()
        {
            if (Controller == null)
            {
                Controller = new DirectStripControllerApi(ControllerNumber);
                if (!Controller.DeviceIsPresent)
                {
                    Log.Warning("WS2811 Strip Controller Nr. {0} is not present. Will not send updates.".Build(ControllerNumber));
                    Controller = null;
                }
            }


            OutputLedData.Fill((byte)0);
            if (Controller != null)
            {
                if (PackData)
                {
                    Controller.SetAndDisplayPackedData(OutputLedData);
                }
                else
                {
                    Controller.SetAndDisplayData(OutputLedData);
                }
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
                        if (PackData)
                        {
                            Controller.SetAndDisplayPackedData(OutputLedData);
                        }
                        else
                        {
                            Controller.SetAndDisplayData(OutputLedData);
                        }
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
                if (PackData)
                {
                    Controller.SetAndDisplayPackedData(OutputLedData);
                }
                else
                {
                    Controller.SetAndDisplayData(OutputLedData);
                }
                Controller.Close();
            }
            Controller = null;

        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectStripController"/> class.
        /// </summary>
        public DirectStripController()
        {
            Outputs = new OutputList();
        }


    }
}
