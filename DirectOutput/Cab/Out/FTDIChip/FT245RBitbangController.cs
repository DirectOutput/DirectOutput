using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DirectOutput.Cab.Out.FTDIChip
{

    /// <summary>
    /// This is a generic output controller class which are based on the FT245R chip (http://www.ftdichip.com/Products/ICs/FT245R.htm). Only units using the chip in bitbang mode are supported by this output controller class.
    /// The SainSmart USB relay boards (http://www.sainsmart.com/arduino-compatibles-1/relay/usb-relay.html) are compatible with this output controller, but other hardware which is based on the same controller chip might be compatible as well. Generally controller units which is exclusively using the FT245R (no extra cpu on board) and having max. 8 output ports are likely to be compatible. Please let me know, if you have tested other hardware successfully, so I can ammend the docu.
    /// \image html SainSmart8PortUsbRelay.jpg SainSmart 8port USB relay board
    /// Thanks go to <a href="http://vpuniverse.com/forums/user/3117-djrobx/">DJRobX</a> for his early implementation of a SainSmart output controller which was the starting point for the implementation of this class.
    /// </summary>
    public class FT245RBitbangController : OutputControllerBase, IOutputController
    {
        private string _SerialNumber = "";

        /// <summary>
        /// Gets or sets the serial number of the FT245R chip which is to be controlled.
        /// </summary>
        /// <value>
        /// The serial number of the FT245R chip which is to be controlled.
        /// </value>
        public string SerialNumber
        {
            get { return _SerialNumber; }
            set { _SerialNumber = value; }
        }

        /// <summary>
        /// Initializes the FT245RBitbangController and starts the updater thread.
        /// </summary>
        /// <param name="Cabinet">The cabinet object which is using the output controller instance.</param>
        public override void Init(Cabinet Cabinet)
        {
            if (SerialNumber.IsNullOrWhiteSpace())
            {
                Log.Exception("Could not initialize FT245RBitbangController {0}. SerialNumber has not been set.".Build(Name));
                return;
            }
            AddOutputs();
            InitUpdaterThread();
            Log.Write("FT245RBitbangController {0} with serial number {1} has been initialized and the updater thread has been started.".Build(Name, SerialNumber));
        }

        /// <summary>
        /// Finishes the FT245RBitbangController object.<br/>
        /// Finish does also terminate the workerthread.
        /// </summary>
        public override void Finish()
        {
            FinishUpdaterThread();
            Log.Write("FT245RBitbangController {0} with serial number {1} has been finished and the updater thread has been terminated.".Build(Name, SerialNumber));
        }

        /// <summary>
        /// Signals the workerthread that all pending updates for the FT245RBitbangController should be sent to the hardware.
        /// </summary>
        public override void Update()
        {
            if (NewValue != CurrentValue)
            {
                UpdaterThreadSignal();
            }
        }

        /// <summary>
        /// Adds the outputs for a FT245R chip.<br/>
        /// A FT245R chip has 8 outputs numbered from 1 to 8. This method adds OutputNumbered objects for all outputs to the list of outputs.
        /// </summary>
        private void AddOutputs()
        {
            for (int i = 1; i <= 8; i++)
            {
                if (!Outputs.Any(x => x.Number == i))
                {
                    Outputs.Add(new Output() { Name = "{0}.{1:00}".Build(Name, i), Number = i });
                }
            }
        }



        /// <summary>
        /// This method is called whenever the value of a output in the Outputs property changes its value.<br />
        /// It updates the internal array holding the states of the outputs.
        /// </summary>
        /// <param name="Output">The output which has changed.</param>
        /// <exception cref="System.Exception">
        /// The OutputValueChanged event handler for the FT245RBitbangController with serial {0} has been called by a sender which is not a OutputNumbered.
        /// or
        /// FT245RBitbangController output numbers must be in the range of 1-8. The supplied output number {0} for FT245RBitbangController with serial number {1} is out of range.
        /// </exception>
        protected override void OnOutputValueChanged(IOutput Output)
        {

            IOutput ON = Output;

            if (!ON.Number.IsBetween(1, 64))
            {
                throw new Exception("FT245RBitbangController output numbers must be in the range of 1-8. The supplied output number {0} for FT245RBitbangController with serial number {1} is out of range.".Build(ON.Number, SerialNumber));
            }

            if (ON.Value > 0)
            {
                lock (ValueChangeLocker)
                {
                    NewValue |= (byte)(1 << (ON.Number - 1));
                }
            }
            else
            {
                lock (ValueChangeLocker)
                {
                    NewValue &= (byte)~(1 << (ON.Number - 1));
                }
            }
        }




        #region UpdaterThread
        /// <summary>
        /// Initializes the updater thread.
        /// </summary>
        /// <exception cref="System.Exception">FT245RBitbangController {0} named {1} updater thread could not start.</exception>
        private void InitUpdaterThread()
        {

            if (!UpdaterThreadIsActive)
            {
                KeepUpdaterThreadAlive = true;
                try
                {
                    UpdaterThread = new Thread(UpdaterThreadDoIt);
                    UpdaterThread.Name = "FT245RBitbangController {0} named {1} updater thread ".Build(SerialNumber, Name);
                    UpdaterThread.Start();
                }
                catch (Exception E)
                {
                    Log.Exception("FT245RBitbangController {0} named {1} updater thread could not start.".Build(SerialNumber, Name), E);
                    throw new Exception("FT245RBitbangController {0} named {1} updater thread could not start.".Build(SerialNumber, Name), E);
                }
            }
        }

        /// <summary>
        /// Finishes the updater thread.
        /// </summary>
        /// <exception cref="System.Exception">A error occured during termination of FT245RBitbangController ({0}) updater thread.</exception>
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
                    Log.Exception("A error occured during termination of FT245RBitbangController ({0}) updater thread.".Build(SerialNumber), E);
                    throw new Exception("A error occured during termination of FT245RBitbangController ({0}) updater thread.".Build(SerialNumber, E));
                }
            }
        }


        /// <summary>
        /// Indicates whether the UpdaterThread of the FT245RBitbangController instance is active or not.
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

        private object ValueChangeLocker = new object();
        private byte NewValue = 0;
        private byte CurrentValue = 255;


        private Thread UpdaterThread { get; set; }
        private object UpdaterThreadLocker = new object();
        private bool KeepUpdaterThreadAlive = true;
        private int FirstTryFailCnt = 0;
        private void UpdaterThreadDoIt()
        {

            Connect();
            if (FTDI == null)
            {
                Log.Warning("No connection to FTDI chip {0}. Updater thread will terminate.".Build(SerialNumber));
                return;
            }

            try
            {
                SendUpdate(0);
            }
            catch (Exception)
            {
                Log.Exception("Could not send initial update to FTDI chip {0}. Updater thread will terminate.".Build(SerialNumber));
                Disconnect();
                return;
            }


            while (KeepUpdaterThreadAlive)
            {
                lock (ValueChangeLocker)
                {
                    CurrentValue = NewValue;
                }

                try
                {
                    //Try to send the update
                    SendUpdate(CurrentValue);
                }
                catch (Exception E)
                {
                    //Log error for first 5 exceptions
                    if (FirstTryFailCnt < 5)
                    {
                        Log.Exception("Could not send update to FTDI chip {0} on first try. Will reconnect and send update again.".Build(SerialNumber), E);
                        FirstTryFailCnt++;
                        if (FirstTryFailCnt == 5)
                        {
                            Log.Warning("Will not log futher warnings on first try failures when sending data to FTDI chip {0}.".Build(SerialNumber));
                        }
                    }


                    try
                    {
                        //Try to reconnect if update fails
                        Disconnect();
                        Connect();
                    }
                    catch (Exception EC)
                    {
                        //Reconnect failed
                        Log.Exception("Could not send update to FTDI chip {0}. Tried to reconnect to device but failed. Updater thread will terminate.".Build(SerialNumber), EC);
                        break;
                    }

                    if (FTDI != null)
                    {
                        //Reconnected OK. Try again to send value.
                        try
                        {
                            SendUpdate(CurrentValue);
                        }
                        catch (Exception EE)
                        {
                            //Sending value failed again
                            Log.Exception("Could not send update to FTDI chip {0}. Reconnect to device worked, but sending the update did fail again. Updater thread will terminate.".Build(SerialNumber), EE);
                            break;
                        }

                    }
                    else
                    {
                        //Reconnect failed
                        Log.Exception("Could not send update to FTDI chip {0}. Tried to reconnect to device but failed. Updater thread will terminate.".Build(SerialNumber));
                        break;
                    }
                }

                if (KeepUpdaterThreadAlive)
                {
                    lock (UpdaterThreadLocker)
                    {
                        while (NewValue == CurrentValue && KeepUpdaterThreadAlive)
                        {
                            Monitor.Wait(UpdaterThreadLocker, 50);  // Lock is released while we’re waiting
                        }
                    }
                }
            }

            try
            {
                SendUpdate(0);
            }
            catch (Exception E)
            {
                Log.Exception("Final update to turn off all output fo FTDI chip {0} failed.".Build(SerialNumber), E);
            }
            Disconnect();
        }

        #endregion

        #region FT245R Communication

        object FTDILocker = new object();
        FTDI FTDI = null;

        private void Connect()
        {
            lock (FTDILocker)
            {

                if (FTDI != null)
                {

                    Disconnect();
                }

                if (SerialNumber.IsNullOrWhiteSpace())
                {
                    Log.Exception("The SerialNumber has not been set for the FT245RBitbangController named {0}. Cant connect to device.".Build(Name));
                    return;
                }

                FTDI = new FTDI();

                //Try to open the device
                try
                {
                    FTDI.ErrorHandler(FTDI.OpenBySerialNumber(SerialNumber));
                }
                catch (Exception E)
                {
                    Log.Exception("Could not open the connection to FTDI chip with serial number {0}.".Build(SerialNumber), E);
                    FTDI = null;
                    return;
                }

                // Set FT245RL to synchronous bit-bang mode, used on sainsmart relay board
                try
                {
                    FTDI.ErrorHandler(FTDI.SetBitMode(0xFF, FTDI.FT_BIT_MODES.FT_BIT_MODE_SYNC_BITBANG));
                }
                catch (Exception E)
                {
                    Log.Exception("Could set the bitmode to bitbang for FTDI chip with serial number {0}.".Build(SerialNumber), E);
                    Disconnect();
                    FTDI = null;
                    return;
                }

                Log.Write("Connection to FTDI chip {0} established.".Build(SerialNumber));
            }




        }


        private void SendUpdate(byte OutputData)
        {
            lock (FTDILocker)
            {
                if (FTDI != null)
                {
                    uint numBytes = 0;
                    byte[] Out = { OutputData };


                    FTDI.FT_STATUS Status = FTDI.Write(Out, 1, ref numBytes);
                    if (Status != FTDI.FT_STATUS.FT_OK)
                    {

                        FTDI.ErrorHandler(Status);
                    }
                    if (numBytes != 1)
                    {
                        throw new Exception("Wrong number of written bytes (expected 1, received {0} was returned when sending data to the FTDI chip {1}".Build(numBytes, SerialNumber));
                    }
                }
            }
        }

        private void Disconnect()
        {
            lock (FTDILocker)
            {
                if (FTDI != null)
                {

                    try
                    {
                        SendUpdate(0);
                    }
                    catch { }

                    if (FTDI.IsOpen)
                    {
                        try
                        {
                            FTDI.ErrorHandler(FTDI.Close());
                        }
                        catch (Exception)
                        {
                            Log.Exception("A exception occured when closing the FTDI chip {0}.".Build(SerialNumber));
                        }
                    }
                    FTDI = null;
                    Log.Write("Connection to FTDI chip {0} closed.".Build(SerialNumber));

                }

            }

        }



        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="FT245RBitbangController"/> class.
        /// </summary>
        public FT245RBitbangController()
        {
            Outputs = new OutputList();
        }

    }
}
