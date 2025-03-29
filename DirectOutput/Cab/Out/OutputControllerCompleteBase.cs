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
    public abstract class OutputControllerCompleteBase : NamedItemBase, IOutputController, ISupportsSetValues
    {
		// Current brightness value for each output, from 0 (off) to 255 (100% on).
        byte[] OutputValues = new byte[0];

		/// <summary>
		/// Output controller in-use state.  This tells us if we've ever received a value update from the
		/// client, and if we've sent any commands yet to the connected device.  Initially, this is set
		/// to Startup.  On a value change event, if this is still startup, we set it to ValueChanged.
		/// The updater thread can use this to determine if it needs to send initialization commands 
		/// and/or value updates to the device.  The updater thread can simply take no action when the
		/// value is Startup, as this means that the unit hasn't yet been addressed by the client and
		/// thus might not be in use at all during this session.  When the state is ValueChanged, the
		/// updater thread should send any necessary initialization commands to the device and change
		/// the state to Running.  When the state is Running, the thread can simply send value updates
		/// as normal.
		/// 
		/// The point of this state tracking is to avoid sending initialization commands to interfaces
		/// that aren't in use.  It's possible for a single physical device to appear to DOF as multiple
		/// software devices, because a single physical device can expose multiple USB interfaces or
		/// might be accessible via multiple protocols.  The main practical case right now where this
		/// can occur is the Pinscape controller, which has both an LedWiz emulation mode and its own
		/// native mode, but the same principle could easily apply to other devices in the future, so 
		/// it's good to deal with it as a general problem rather than hard-coding something special
		/// into the LedWiz and/or Pinscape drivers.  At any rate, the problem that can occur when DOF
		/// sees multiple interfaces for one physical device is that DOF will (without this state
		/// management) want to send initialization commands to all of the different interfaces it
		/// sees for that single device, to set all of its ports to a known initial state (usually
		/// this means just turning off all of the ports).  DOF handles each device interface on a
		/// separate thread, so the sequencing of those initialization commands across the multiple
		/// spoofed interfaces to the one device is unpredictable.  So the result can be that thread 
		/// T1 for interface I1 sends its initialization commands, and then gets some value updates
		/// from the host to turn on some outputs - turn on the START button light, say - and *then*
		/// thread T2 for interface I2 sends *its* initialization commands, turning the outputs back
		/// to their initial OFF state.  So the START button light flashes on for an instant and
		/// goes right back off.  But if I2 isn't actually used in the configuration, there really
		/// was never a need for T2 to send the initialization commands.  So we can easily clear up
		/// this kind of one-device/multiple-interfaces conflict by just waiting to send initialization
		/// commands until we know that the interface is actually in use in the configuration, by
		/// waiting until the configuration has sent us at least one explicit value change.
		/// </summary>
		protected enum InUseStates { Startup, ValueChanged, Running };

		/// <summary>
		/// Current in-use state for this controller.
		/// </summary>
		protected InUseStates InUseState = InUseStates.Startup;

        /// <summary>
        /// Manages to output object of the output controller. Use the GetNumberOfConfiguredOutputs() method to determine the number of outputs to be setup.
        /// </summary>
        protected void SetupOutputs()
        {
            int NumberOfOutputs = GetNumberOfConfiguredOutputs().Limit(0, int.MaxValue);
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


            OutputValues = Outputs.OrderBy(O => O.Number).Select(O => O.Value).ToArray();
        }

        private void RenameOutputs()
        {
            if (Outputs != null)
            {
                foreach (IOutput O in Outputs)
                {
                    O.Name = "{0}.{1}".Build((Name != null ? Name : ""), O.Name);
                }
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
            if (Output.Number > 0 && Output.Number <= OutputValues.Length)
            {
                lock (ValueChangeLocker)
                {
                    if (OutputValues[Output.Number - 1] != Output.Value)
                    {
						// if we're in Startup state, transition to ValueChanged state
						if (InUseState == InUseStates.Startup)
							InUseState = InUseStates.ValueChanged;

						// set the new value
                        OutputValues[Output.Number - 1] = Output.Value;

						// tell the updater thread we have to update the physical device
                        UpdateRequired = true;
                    }

                }
            }
        }



        #region ISupportsSetValues Member

        /// <summary>
        /// Sets the values for one or several outputs of the controller.
        /// </summary>
        /// <param name="FirstOutput">The first output to be updated with a new value (zero based).</param>
        /// <param name="Values">The values to be used.</param>
        public void SetValues(int FirstOutput, byte[] Values)
        {
            if (FirstOutput >= OutputValues.Length) return;
            if (FirstOutput < 0) return;
            int CopyLength = (OutputValues.Length - FirstOutput).Limit(0, Values.Length);
            if (CopyLength < 1) return;

            lock (ValueChangeLocker)
            {
				// copy the new values
				Buffer.BlockCopy(Values, 0, OutputValues, FirstOutput, CopyLength);

				// if we're in Startup state, transition to ValueChanged state
				if (InUseState == InUseStates.Startup)
					InUseState = InUseStates.ValueChanged;

				// an update is now required
                UpdateRequired = true;
            }
        }

        #endregion



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
                string msg = "A exception occurred when verifying the settings for {0} \"{1}\": {2}. Can't initialize.".Build(this.GetType().Name, Name, E.Message);
                Log.Exception(msg, E);
                throw new Exception(msg, E);
            }
            if (V)
            {
                SetupOutputs();
                InitUpdaterThread();
                Log.Write("{0} \"{1}\" initialized and updater thread started.".Build(this.GetType().Name, Name));
            }
            else
            {
                Log.Warning("Settings for {0} \"{1}\" are not correct. Can't initialize.".Build(this.GetType().Name, Name));
            }
        }


        /// <summary>
        /// Finishes the output controller and stop the updater thread.
        /// </summary>
        public virtual void Finish()
        {
            FinishUpdaterThread();
            Log.Write("{0} \"{1}\" finished and updater thread stopped.".Build(this.GetType().Name, Name));
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
        /// The method is used internally to determine the number of output objects which have to be setup. 
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
                    UpdaterThread.Name = "{0} \"{1}\" updater thread".Build(this.GetType().Name, Name);
                    UpdaterThread.Start();
                }
                catch (Exception E)
                {
                    Log.Exception("{0} \"{1}\" updater thread could not start.".Build(this.GetType().Name, Name), E);
                    throw new Exception("{0} \"{1}\" updater thread could not start.".Build(this.GetType().Name, Name), E);
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
                    Log.Exception("A error occurred during termination of the {0}: {1}.".Build(UpdaterThread.Name), E);
                    throw new Exception("A error occurred during termination of the {0}: {1}.".Build(UpdaterThread.Name), E);
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
                    Log.Exception("{0} could not connect to the controller. Thread will quit.".Build(Thread.CurrentThread.Name), E);

                    try
                    {
                        DisconnectFromController();
                        return;
                    }
                    catch { }
                }

                Log.Write("{0} has connected to {1} \"{2}\".".Build(Thread.CurrentThread.Name, this.GetType().Name, Name));

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
							// If we're in ValueChanged state, we've received at least one value update
							// from the client, but we haven't yet initialized the physical device.  Do
							// so now.
							if (InUseState == InUseStates.ValueChanged)
							{
								// send a physical update to turn off all outputs, to ensure that the
								// output controller's initial physical state matches our internal state
								UpdateOutputs(Enumerable.Repeat((byte)0, OutputValues.Length).ToArray());

								// switch to Running state - we want to send updates from now on
								InUseState = InUseStates.Running;
							}

							// Send the update if we're in Running state.  We don't send updates until
							// we reach this state, since otherwise we haven't received any value changes
							// from the client and hence might not be in use during this session.
							if (InUseState == InUseStates.Running)
								UpdateOutputs(ValuesToSend);
                        }
                        catch (Exception E)
                        {
                            Log.Exception("{0} could not send update for {1} \"{2}\": {3}. Will try again.".Build(new object[] { Thread.CurrentThread.Name, this.GetType().Name, Name, E.Message }), E);
                            UpdateOK = false;
                        }
                        if (UpdateOK) break;
                    }

                    if (!UpdateOK)
                    {
                        Log.Warning("{0} tries to reconnect to {1} \"{2}\".".Build(Thread.CurrentThread.Name, this.GetType().Name, Name));
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
                            Log.Exception("{0} could not reconnect to the controller. Thread will quit.".Build(Thread.CurrentThread.Name), E);

                            try
                            {
                                DisconnectFromController();
                                return;
                            }
                            catch { }
                        }
                        Log.Write("{0} has reconnected to {1} \"{2}\".".Build(Thread.CurrentThread.Name, this.GetType().Name, Name));
                        Thread.Sleep(100);
                        try
                        {
                            UpdateOutputs(ValuesToSend);
                        }
                        catch (Exception E)
                        {
                            Log.Exception("{0} could still not send update for {1} \"{2}\": {3}. Thread will quit.".Build(new object[] { Thread.CurrentThread.Name, this.GetType().Name, Name, E.Message }), E);
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

                try
				{
					if (InUseState != InUseStates.Startup)
						UpdateOutputs(new byte[OutputValues.Length]);
                }
                catch (Exception E)
                {
                    Log.Exception("A exception occurred in {0} while trying to turn of all outputs for {1} \"{2}\"".Build(Thread.CurrentThread.Name, this.GetType().Name, Name), E);
                }
                try
                {
                    DisconnectFromController();
                }
                catch {}

                Log.Write("{0} has disconnected from {1} \"{2}\" and will terminate.".Build(Thread.CurrentThread.Name, this.GetType().Name, Name));
            }
            catch (Exception EU)
            {
                Log.Exception("A exception has occurred in {0}. Thread will quit. Message: {1}".Build(Thread.CurrentThread.Name, EU.Message), EU);
            }

        }

        #endregion


    }
}
