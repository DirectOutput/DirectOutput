using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.General.Generic;
using System.Threading;

namespace DirectOutput.Cab.Out
{
    /// <summary>
    /// This abstract class implement the full base logic for a output controller with a separate thread for the hardware communication.
    /// All you have to do to create your own output controller class is to inherit this class and to implement a few abstract methods (int GetNumberOfConfiguredOutputs(), bool VerifySettings(), void ConnectToController(), void DisconnectFromController(), void UpdateOutputs(byte[] OutputValues).
    /// </summary>
    public abstract class OutputControllerCompleteBase : NamedItemBase, IOutputController
    {

        byte[] OutputValues = new byte[0];

        /// <summary>
        /// Manages to output object of the output controller. Use the GetNumberOfConfiguredOutputs() method to determine the number of outputs to be setup.
        /// </summary>
        protected void SetupOutputs()
        {
            int NumberOfOutputs = GetNumberOfConfiguredOutputs().Limit(0,int.MaxValue);
            if (Outputs == null)
            {
                Outputs = new OutputList();
            }

            Outputs.Where(O => O.Number > NumberOfOutputs).ToList().ForEach(ODEL => Outputs.Remove(ODEL));

            for (int i = 1; i <= NumberOfOutputs; i++)
            {
                if (!Outputs.Any(O => O.Number == i))
                {
                    Outputs.Add(new Output() { Name = "{0}.{1}".Build((Name != null ? Name : ""), i), Number = i, Value = 0 });
                }
            }

            
            OutputValues = Outputs.OrderBy(O=>O.Number).Select(O => O.Value).ToArray();
        }

        private void RenameOutputs()
        {
            foreach (IOutput O in Outputs)
            {
                O.Name = "{0}.{1}".Build((Name != null ? Name : ""), O.Name);
            }
        }

        protected override void AfterNameChange(string OldName, string NewName)
        {
            RenameOutputs();
            base.AfterNameChange(OldName, NewName);
        }


        private OutputList _Outputs = null;
        /// <summary>
        /// Contains the OutputList object for the outputs of the output controller.<br/>
        /// \remark This property is marked with the XMLIgnore attributte so its content does not get serialized, which means that output controller implementations inherting from this class have to create the output objects in the list.<br/>The XMLIgnore attribute is inherted to classes inheriting from this base class. This means that inherited classes do not serialize this property unless a specific serialization implementation is used in that class.
        /// </summary>
        [XmlIgnore]
        public OutputList Outputs
        {
            get { return _Outputs; }
             set
            {
                if (_Outputs != null)
                {
                    _Outputs.OutputValueChanged -= new OutputList.OutputValueChangedEventHandler(Outputs_OutputValueChanged);
                }

                _Outputs = value;

                if (_Outputs != null)
                {
                    _Outputs.OutputValueChanged += new OutputList.OutputValueChangedEventHandler(Outputs_OutputValueChanged);

                }

            }
        }

        private void Outputs_OutputValueChanged(object sender, OutputEventArgs e)
        {
            OnOutputValueChanged(e.Output);
        }

        /// <summary>
        /// This method is called whenever the value of a output in the Outputs property changes its value.<br/>
        /// It doesn't do anything in this base class, but it can be overwritten (use override) in classes inherting the base class.
        /// </summary>
        /// <param name="Output">The output.</param>
        private void OnOutputValueChanged(IOutput Output)
        {
            if (Output.Number < OutputValues.Length)
            {
                lock (ValueChangeLocker)
                {
                    if (OutputValues[Output.Number] != Output.Value)
                    {
                        OutputValues[Output.Number] = Output.Value;
                        UpdateRequired = true;
                    }

                }
            }
        }

        /// <summary>
        /// Initializes the output controller and starts the updater thread.
        /// </summary>
        /// <param name="Cabinet">The cabinet object which is using the output controller instance.</param>
        public virtual void Init(Cabinet Cabinet)
        {
            bool V = false;
            try
            {
                V = VerifySettings();
            }
            catch (Exception E)
            {
                string msg = "A exception occured when verifying the settings for {0} {1}: {2}. Cant initialize.".Build(this.GetType().Name, Name,E.Message);
                Log.Exception(msg, E);
                throw new Exception(msg, E);
            } 
            if (V)
            {
                SetupOutputs();
                InitUpdaterThread();
                Log.Write("{0} {1} intialized and updater thread started.");
            }
            else
            {
                Log.Warning("Settings for {0} {1} are not correct. Cant initialize.".Build(this.GetType().Name, Name));
            }
        }


        /// <summary>
        /// Finishes the output controller and stop the updater thread.
        /// </summary>
        public void Finish() {
            FinishUpdaterThread();
            Log.Write("{0} {1} finished and updater thread stoped.");
        }

        /// <summary>
        /// Triggers the update of the physical outputs
        /// </summary>
        public void Update()
        {
            if (UpdateRequired)
            {
                UpdaterThreadSignal();
            }
        }


        /// <summary>
        /// This method must return the number of configured outputs.
        /// The method is used internaly to determine the number of output objects which have to be setup. 
        /// Return a fixed value for output controllers which have a fixed number of outputs. Return the value of a configurable property for controllers with a defineable number of outputs.
        /// </summary>
        /// <returns>The number of outputs to be configured.</returns>
        protected abstract int GetNumberOfConfiguredOutputs();

        /// <summary>
        /// Verifies the settings of the output controller.
        /// </summary>
        /// <returns><c>true</c> if the verification is OK, otherwise <c>false</c></returns>
        protected abstract bool VerifySettings();

        /// <summary>
        /// This method is called whenever new data has to be sent to the output controller.
        /// Implement the communication with your hereware in this method.
        /// </summary>
        /// <param name="OutputValues">Array of output values for each numbered output.</param>
        protected abstract void UpdateOutputs(byte[] OutputValues);



        /// <summary>
        /// This method is called when DOF wants to connect to the controller.
        /// Implement your own logic to connect to the controller here.
        /// </summary>
        protected abstract void ConnectToController();

        /// <summary>
        /// This method is called when DOF wants to disconnect from the controller.
        /// Implement your own logic to disconnect from the controller here.

        /// </summary>
        protected abstract void DisconnectFromController();


        #region UpdaterThread
        /// <summary>
        /// Initializes the updater thread.
        /// </summary>
        /// <exception cref="System.Exception">{0} {1}  updater thread could not start..Build(this.GetType().Name, Name)</exception>
        private void InitUpdaterThread()
        {

            if (!UpdaterThreadIsActive)
            {
                KeepUpdaterThreadAlive = true;
                try
                {
                    UpdaterThread = new Thread(UpdaterThreadDoIt);
                    UpdaterThread.Name = "{0} {1} updater thread ".Build(this.GetType().Name, Name);
                    UpdaterThread.Start();
                }
                catch (Exception E)
                {
                    Log.Exception("{0} {1}  updater thread could not start.".Build(this.GetType().Name, Name), E);
                    throw new Exception("{0} {1}  updater thread could not start.".Build(this.GetType().Name, Name), E);
                }
            }
        }

        /// <summary>
        /// Finishes the updater thread.
        /// </summary>

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
                        Log.Warning("{0} did not quit. Forcing abort.".Build(UpdaterThread.Name));
                    }
                    UpdaterThread = null;
                }
                catch (Exception E)
                {
                    Log.Exception("A error occured during termination of the {0}: {1}.".Build(UpdaterThread.Name), E);
                    throw new Exception("A error occured during termination of the {0}: {1}.".Build(UpdaterThread.Name), E);
                }
            }
        }


        /// <summary>
        /// Indicates whether the UpdaterThread is active or not.
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

        private Thread UpdaterThread { get; set; }
        private object UpdaterThreadLocker = new object();
        private bool KeepUpdaterThreadAlive = true;

        private bool UpdateRequired = false;

        private void UpdaterThreadDoIt()
        {
            Log.Write("{0} started.".Build(Thread.CurrentThread.Name));
            try
            {
                try
                {
                    ConnectToController();

                }
                catch (Exception E)
                {
                    Log.Warning("{0} could not connect to the controller. Thread will quit.".Build(Thread.CurrentThread.Name));

                    try
                    {
                        DisconnectFromController();
                        return;
                    }
                    catch { }
                }

                Log.Write("{0} has connected to {1} {2}.".Build(Thread.CurrentThread.Name, this.GetType().Name, Name));

                while (KeepUpdaterThreadAlive)
                {
                    byte[] ValuesToSend = new byte[0];
                    lock (ValueChangeLocker)
                    {
                        ValuesToSend = (byte[])OutputValues.Clone();
                    }


                    bool UpdateOK = true;
                    for (int i = 0; i < 1; i++)
                    {

                        try
                        {
                            UpdateOutputs(ValuesToSend);
                        }
                        catch (Exception E)
                        {
                            Log.Warning("{0} could not send update for {1} {2}: {3}. Will try again.".Build(new object[] { Thread.CurrentThread.Name, this.GetType().Name, Name, E.Message }));
                            UpdateOK = false;
                        }
                        if (UpdateOK) break;
                    }

                    if (!UpdateOK)
                    {
                        Log.Warning("{0} tries to reconnect to {1} {2}.".Build(Thread.CurrentThread.Name, this.GetType().Name, Name));
                        try
                        {
                            DisconnectFromController();
                        }
                        catch { }
                        Thread.Sleep(100);
                        try
                        {
                            ConnectToController();

                        }
                        catch (Exception E)
                        {
                            Log.Warning("{0} could not reconnect to the controller. Thread will quit.".Build(Thread.CurrentThread.Name));

                            try
                            {
                                DisconnectFromController();
                                return;
                            }
                            catch { }
                        }
                        Log.Write("{0} has reconnected to {1} {2}.".Build(Thread.CurrentThread.Name, this.GetType().Name, Name));
                        Thread.Sleep(100);
                        try
                        {
                            UpdateOutputs(ValuesToSend);
                        }
                        catch (Exception E)
                        {
                            Log.Warning("{0} could still not send update for {1} {2}: {3}. Thread will quit.".Build(new object[] { Thread.CurrentThread.Name, this.GetType().Name, Name, E.Message }));
                            try
                            {
                                DisconnectFromController();
                                return;
                            }
                            catch { }
                        }

                    }



                    if (KeepUpdaterThreadAlive)
                    {
                        lock (UpdaterThreadLocker)
                        {
                            while (!UpdateRequired && KeepUpdaterThreadAlive)
                            {
                                Monitor.Wait(UpdaterThreadLocker, 50);  // Lock is released while we’re waiting
                            }
                        }
                      
                    }
                    UpdateRequired = false;
                }

                DisconnectFromController();

                Log.Warning("{0} has disconnected to {1} {2} and will terminate.".Build(Thread.CurrentThread.Name, this.GetType().Name, Name));
            }
            catch (Exception EU)
            {
                Log.Exception("A exception has occured in {0}. Thread will quit. Message: {1}".Build(Thread.CurrentThread.Name, EU.Message), EU);
            }

            
        }

        #endregion


    }
}
