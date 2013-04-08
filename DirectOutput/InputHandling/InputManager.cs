using System;
using System.Collections.Generic;
using System.Threading;


/// <summary>
/// The PinmameHandling namespace contains a few classes used to handle the inputs from Pinmame. 
/// </summary>
namespace DirectOutput.InputHandling
{

    /// <summary>
    /// Manages the data received from PinMame and controls the worker thread which processes the data.
    /// </summary>
    public class InputManager
    {
        const int MaxDataProcessingTimeMs = 10;

        private object InputDataQueueLocker = new object();
        private Queue<TableElementData> InputDataQueue = new Queue<TableElementData>();


        /// <summary>
        /// Enqueues input data for processing by the worker thread.
        /// </summary>
        /// <param name="TableElementTypeChar">Char specifing the TableElementType of the TableElement (see TableElementTypeEnum for valid values)</param>
        /// <param name="Number">The number of the TableElement.</param>
        /// <param name="Value">The value of the TableElement.</param>
        public void EnqueueInputData(Char TableElementTypeChar, int Number, int Value)
        {
            EnqueueInputData(new TableElementData(TableElementTypeChar, Number, Value));
        }


        /// <summary>
        /// Enqueues input data for processing by the worker thread.
        /// </summary>
        /// <param name="Data">TableElementData object to enqueue.</param>
        public void EnqueueInputData(TableElementData Data)
        {

            lock (InputDataQueueLocker)
            {
                InputDataQueue.Enqueue(Data);
            }
            lock (WorkerThreadLocker)
            {
                Monitor.Pulse(WorkerThreadLocker);
            }


        }



        /// <summary>
        /// Initializes the InputManager
        /// and starts the worker thread which is processing the received input data.
        /// </summary>
        public void Init()
        {
            StartWorkerThread();
        }


        /// <summary>
        /// Terminates the InputManger
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
                    WorkerThread.Name = "InputManager WorkerThread ";
                    WorkerThread.Start();
                }
                catch (Exception E)
                {
                    Log.Exception("InputManager Workerthread could not start.", E);
                    throw new Exception("InputManager Workerthread could not start.", E);
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
                    Log.Exception("A error occured during termination of InputManager Workerthread", E);
                    throw new Exception("A error occured during termination of InputManager Workerthread", E);
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
                while (InputDataQueue.Count > 0 && (DateTime.Now - Start).Milliseconds <= MaxDataProcessingTimeMs && KeepWorkerThreadAlive)
                {
                    TableElementData D;
                    lock (InputDataQueueLocker)
                    {
                        D = InputDataQueue.Dequeue();
                    }

                    try
                    {
                        OnInputDataReceived(D);
                    }
                    catch (Exception E)
                    {
                        Log.Exception("A exception occured while processing input data for table element {0} {1} with value {2}".Build(D.TableElementType, D.Number, D.Value), E);
                    }
                }

                OnInputDataProcessed();



                if (KeepWorkerThreadAlive)
                {
                    lock (WorkerThreadLocker)
                    {
                        while (InputDataQueue.Count == 0 && KeepWorkerThreadAlive)
                        {
                            Monitor.Wait(WorkerThreadLocker, 50);  // Lock is released while we’re waiting
                        }
                    }
                }
            }



        }


        /// <summary>
        /// Indicates if the workerthread of the InputManger is active
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

        public event EventHandler<EventArgs> InputDataProcessed;
        /// <summary>
        /// Occurs when all input data has been processed
        /// </summary>
        private void OnInputDataProcessed()
        {
            if (InputDataProcessed != null)
            {
                InputDataProcessed(this, new EventArgs());
            }
        }


        /// <summary>
        /// Event is fired if new input data has been received
        /// </summary>
        public event EventHandler<InputDataReceivedEventArgs> InputDataReceived;

        private void OnInputDataReceived(TableElementData PinmameData)
        {
            if (InputDataReceived != null && PinmameData != null)
            {
                InputDataReceived(this, new InputDataReceivedEventArgs(PinmameData));
            }
        }


        /// <summary>
        /// Eventargs for the InputDataReceived Event
        /// </summary>
        public class InputDataReceivedEventArgs : EventArgs
        {
            public TableElementData TableElementData { get; set; }
            public TableElementTypeEnum TableElementType { get { return TableElementData.TableElementType; } }
            public int Number { get { return TableElementData.Number; } }
            public int Value { get { return TableElementData.Value; } }

            public InputDataReceivedEventArgs() { }
            public InputDataReceivedEventArgs(TableElementData TableElementData)
            {
                this.TableElementData = TableElementData;
            }
        }



    }
}
