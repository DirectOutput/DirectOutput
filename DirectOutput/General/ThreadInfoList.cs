using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DirectOutput.General.Generic;

namespace DirectOutput.General
{
    /// <summary>
    /// List of ThreadInfo objects.<br/>
    /// This class is used by DOF for thread monitoring
    /// </summary>
    public class ThreadInfoList:List<ThreadInfo>
    {

        private object ListUpdateLocker=new object();

        /// <summary>
        /// Calls the HeartBeat method of the ThreadInfo object for the specified thread.<br />
        /// If the specified thread does not have a ThreadInfo object in the list, a new ThreadInfo object will be instanciated and added to the list.
        /// \note This method has to be called by the thread to be monitored.
        /// \warning If you are using this method in you code, please dont forget to call ThreadTerminates before the thread exits. This will allow the system to free up the resources used by the thread.
        /// </summary>
        /// <param name="HostObject">The object hosting the thread to be monitored.</param>       
        public void HeartBeat(object HostObject=null)
        {
            try
            {
                this[Thread.CurrentThread].HeartBeat();
            }
            catch (ArgumentException AE)
            {
                if (AE.ParamName == "Thread")
                {
                    ThreadInfo TI = new ThreadInfo(Thread.CurrentThread);
                    if (HostObject != null)
                    {
                        if (HostObject is INamedItem)
                        {
                            TI.HostName = ((INamedItem)HostObject).Name;
                        }
                        else
                        {
                            TI.HostName = HostObject.GetType().Name;
                        }
                    }
                    lock (ListUpdateLocker)
                    {
                        this.Add(TI);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the HeartBeat method of the ThreadInfo object for the specified thread.<br />
        /// If the specified thread does not have a ThreadInfo object in the list, a new ThreadInfo object will be instanciated and added to the list.
        /// \note This method has to be called by the thread to be monitored.
        /// \warning If you are using this method in you code, please dont forget to call ThreadTerminates before the thread exits. This will allow the system to free up the resources used by the thread.
        /// </summary>
        /// <param name="HostObjectName">The name of the object hosting the thread.</param>       
        public void HeartBeat(string HostObjectName)
        {
            try
            {
                this[Thread.CurrentThread].HeartBeat();
            }
            catch (ArgumentException AE)
            {
                if (AE.ParamName == "Thread")
                {
                    ThreadInfo TI = new ThreadInfo(Thread.CurrentThread);
                    if(!HostObjectName.IsNullOrWhiteSpace()) {
                        TI.HostName = HostObjectName;
                        
                    }
                    lock (ListUpdateLocker)
                    {
                        this.Add(TI);
                    }
                }
            }
        }

        /// <summary>
        /// This method records a exception which has been captured.
        /// \note This method has to be called by the thread to be monitored.
        /// </summary>
        /// <param name="Exception">The exception to record.</param>
        /// <param name="HostObject">The host object</param>
        public void RecordException(Exception Exception, object HostObject=null) {
            HeartBeat(HostObject);
            this[Thread.CurrentThread].RecordException(Exception);
        }

        /// <summary>
        /// Has to be called by the thread before it terminates.<br/>
        /// This command removes the thread from the List, so the system can clean up the resources used by the thread.
        /// \note This method has to be called by the thread to be monitored.
        /// \warning Dont forget to call this method if you are using this class to monitor a threads activity.
        /// </summary>
        public void ThreadTerminates()
        {
            if (this.Contains(Thread.CurrentThread))
            {
                lock (ListUpdateLocker)
                {
                    this.Remove(this[Thread.CurrentThread]);
                }
            }

        }


        /// <summary>
        /// Gets the ThreadInfo object for the specified thread.
        /// </summary>
        /// <param name="Thread">The thread for which to lookup the ThreadInfoObject.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">The ThreadInfoList does not contain a ThreadInfo object for thread {0}..Build(Thread.Name);Thread</exception>
        public ThreadInfo this[Thread Thread]
        {
            get
            {
                if (Contains(Thread))
                {
                    return this.First(TI => TI.Thread == Thread);
                }
                throw new ArgumentException("The ThreadInfoList does not contain a ThreadInfo object for thread {0}.".Build(Thread.Name), "Thread");
            }
        }


        /// <summary>
        /// Determines whether the list contains a ThreadInfo object for the given thread.
        /// </summary>
        /// <param name="Thread">The thread to check.</param>
        /// <returns>
        ///   <c>true</c> if the list contains the specified thread; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(Thread Thread)
        {
            return this.Any(TI=>TI.Thread==Thread);
        }

    }
}
