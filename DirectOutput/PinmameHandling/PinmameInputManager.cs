using System;
using System.Collections.Generic;
using System.Threading;


/// <summary>
/// The PinmameHandling namespace contains a few classes used to handle the inputs from Pinmame. 
/// </summary>
namespace DirectOutput.PinmameHandling
{

    /// <summary>
    /// Manages the data received from PinMame and controls the worker thread which processes the data.
    /// </summary>
    public class PinmameInputManager
    {
        const int MaxDataProcessingTimeMs = 10;

        private object PinmameDataQueueLocker = new object();
        private Queue<TableElementData> PinmameDataQueue = new Queue<TableElementData>();


        /// <summary>
        /// Enqueues PinmameData for processing by the worker thread.
        /// </summary>
        /// <param name="TableElementTypeChar">Char specifing the TableElementType of the TableElement (see TableElementTypeEnum for valid values)</param>
        /// <param name="Number">The number of the TableElement.</param>
        /// <param name="Value">The value of the TableElement.</param>
        public void EnqueuePinmameData(Char TableElementTypeChar, int Number, int Value)
        {
            EnqueuePinmameData(new TableElementData(TableElementTypeChar, Number, Value));
        }


        /// <summary>
        /// Enqueues PinmameData for processing by the worker thread.
        /// </summary>
        /// <param name="Data">PinmameData object to enqueue.</param>
        public void EnqueuePinmameData(TableElementData Data)
        {

            lock (PinmameDataQueueLocker)
            {
                PinmameDataQueue.Enqueue(Data);
            }
            lock (WorkerThreadLocker)
            {
                Monitor.Pulse(WorkerThreadLocker);
            }


        }



        /// <summary>
        /// Initializes the PinmameInputManager
        /// and starts the worker thread which is processing the received Pinmamedata.
        /// </summary>
        public void Init()
        {
            StartWorkerThread();
        }


        /// <summary>
        /// Terminates the PinmameInputManger
        /// and stops the worker thread.
        /// </summary>
        public void Terminate()
        {
            TerminateWorkerThread();
        }

        private void StartWorkerThread()
        {

            if (!WorkerThreadIsActive)
            {
                try
                {
                    WorkerThread = new Thread(WorkerThreadDoIt);
                    WorkerThread.Name = "PinmameInputManager Input WorkerThread ";
                    WorkerThread.Start();
                }
                catch (Exception E)
                {
                    throw new Exception("PinmameInputManager Workerthread could not start.", E);
                }
            }
        }

        private void TerminateWorkerThread()
        {
            if (WorkerThread != null)
            {
                try
                {
                    KeepWorkerThreadAlive = false;
                    lock (WorkerThreadLocker)
                    {
                        Monitor.Pulse(WorkerThreadLocker);
                    }
                    if (!WorkerThread.Join(1000))
                    {
                        WorkerThread.Abort();
                    }
                    WorkerThread = null;
                }
                catch (Exception E)
                {
                    throw new Exception("A error occurd during termination of PinmameInputManager Workerthread", E);
                }
            }
        }

        private Thread WorkerThread { get; set; }
        private object WorkerThreadLocker = new object();
        private bool KeepWorkerThreadAlive = true;
        private void WorkerThreadDoIt()
        {

            while (KeepWorkerThreadAlive)
            {
                DateTime Start = DateTime.Now;
                while (PinmameDataQueue.Count > 0 && (DateTime.Now - Start).Milliseconds <= MaxDataProcessingTimeMs && KeepWorkerThreadAlive)
                {
                    TableElementData D;
                    lock (PinmameDataQueueLocker)
                    {
                        D = PinmameDataQueue.Dequeue();
                    }
                    OnPinmameDataReceived(D);
                }
                OnPinmameDataProcessed();


                if (KeepWorkerThreadAlive)
                {
                    lock (WorkerThreadLocker)
                    {
                        while (PinmameDataQueue.Count == 0 && KeepWorkerThreadAlive)
                        {
                            Monitor.Wait(WorkerThreadLocker, 50);  // Lock is released while we’re waiting
                        }
                    }
                }
            }



        }


        /// <summary>
        /// Indicates if the workerthread of the PinmameInputManger is active
        /// </summary>
        public bool WorkerThreadIsActive
        {
            get
            {
                if (WorkerThread != null)
                {
                    if (WorkerThread.IsAlive)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public event EventHandler<EventArgs> PinmameDataProcessed;
        /// <summary>
        /// Occurs when the class is AllPinmameDataProcessed
        /// </summary>
        protected void OnPinmameDataProcessed()
        {
            if (PinmameDataProcessed != null)
            {
                PinmameDataProcessed(this, new EventArgs());
            }
        }


        /// <summary>
        /// Event is fired if new pinmamedata has been received
        /// </summary>
        public event EventHandler<PinmameDataReceivedEventArgs> PinmameDataReceived;

        private void OnPinmameDataReceived(TableElementData PinmameData)
        {
            if (PinmameDataReceived != null && PinmameData != null)
            {
                PinmameDataReceived(this, new PinmameDataReceivedEventArgs(PinmameData));
            }
        }


        /// <summary>
        /// Eventargs for the PinmamedataReceived Event
        /// </summary>
        public class PinmameDataReceivedEventArgs : EventArgs
        {
            public TableElementData PinmameData { get; set; }
            public TableElementTypeEnum TableElementType { get { return PinmameData.TableElementType; } }
            public int Number { get { return PinmameData.Number; } }
            public int State { get { return PinmameData.Value; } }

            public PinmameDataReceivedEventArgs() { }
            public PinmameDataReceivedEventArgs(TableElementData PinmameData)
            {
                this.PinmameData = PinmameData;
            }
        }



    }
}
