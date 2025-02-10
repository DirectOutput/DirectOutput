using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace DirectOutput
{
    /// <summary>
    /// A simple logger used to record important events and exceptions.<br/>
    /// </summary>
    public class Log
    {
        static StreamWriter Logger;
        static bool IsInitialized = false;
        static bool IsOk = false;

        private static readonly object Locker = new object();

        private static string _Filename = ".\\DirectOutput.log";

        private static HashSet<string> _ActiveInstrumentations = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // collection of records from before the log file was set up, to allow
        // logging and debugging of initial config file setup
        static List<String> PreLogFileLog = new List<string>();

        /// <summary>
        /// Gets or sets the filename for the log.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public static string Filename
        {
            get { return _Filename; }
            set { _Filename = value; }
        }

        public static string Instrumentations
        {
            set {
                if (!value.IsNullOrEmpty())
                {
                    _ActiveInstrumentations.Clear();
                    foreach (var s in value.Split(','))
                        _ActiveInstrumentations.Add(s.Trim());
                }
            }
        }

        /// <summary>
        /// Initializes the log using the file defined in the Filename property.
        /// </summary>
        public static void Init()
        {
            lock (Locker)
            {
                if (!IsInitialized)
                {
                    try
                    {
                        Logger = File.AppendText(Filename);

                        var BuildConfiguration =
                            #if DEBUG
                                "Debug"
                            #else
                                "Release";
                            #endif
                        ;

                        string instrumentationsEnabledNote = (_ActiveInstrumentations.Count != 0) ?
                            $"; Instrumentations enabled: {string.Join(", ", _ActiveInstrumentations)}" : "";
						
                        Logger.WriteLine("---------------------------------------------------------------------------------");
                        Version V = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                        DateTime BuildDate = new DateTime(2000, 1, 1).AddDays(V.Build).AddSeconds(V.Revision * 2);
                        Logger.WriteLine("DirectOutput Framework - Version {0}, {1}-{2}, built {3}".Build(
                            V.ToString(), Environment.Is64BitProcess ? "x64" : "x86", BuildConfiguration,
                            BuildDate.ToString("yyyy.MM.dd HH:mm")));
						Logger.WriteLine("DOF created by SwissLizard / MIT License");
						Logger.WriteLine("MJR Grander Unified DOF R3++ edition feat. Djrobx, Rambo3, Vroonsh, CSD, and Freezy");
                        Logger.WriteLine("https://github.com/mjrgh/DirectOutput");
                        Logger.WriteLine("{0}\t{1}", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff"), 
                            $"DirectOutput logger initialized{instrumentationsEnabledNote}");

						IsOk = true;

                        // copy the pre-log-file items to the log
                        if (PreLogFileLog != null)
                        {
                            foreach (String s in PreLogFileLog)
                                Logger.WriteLine(s);

                            // clear the pre-log items
                            PreLogFileLog = null;
                        }
                    }
                    catch
                    {
                        IsOk = false;
                    }

                    IsInitialized = true;
                }
            }
        }

        /// <summary>
        /// Finalize initialization.  Call during startup.  If logging will
        /// be used, call after Init().  Otherwise call at any convenient
        /// point during startup.
        /// </summary>
        public static void AfterInit()
        {
            // Discard any pre-log messages, and forget the list so that we
            // don't collect any more messages.  We collect pre-log messages
            // so that we can log events before the log file is open.  When
            // we reach this point, the log file will have already been opened
            // if there's going to be one at all, and we will have copied the
            // accumulated pre-log messages into the file.  Once we get past
            // initialization, there's no point in saving more messages, since 
            // log file won't be opened after this point.
            PreLogFileLog = null;
        }

        /// <summary>
        /// Finishes the logger.<br/>
        /// Closes the log file.
        /// </summary>
        public static void Finish()
        {
            lock (Locker)
            {
                if (Logger != null)
                {
                    Write("Logging stopped");
                    Logger.Flush();
                    Logger.Close();
                    IsOk = false;
                    IsInitialized = false;
                    Logger = null;
                }
            }
        }

        // Raw message writer.  This writes a string that's already been
        // formatted by Write() to the current backing store, either the
        // log file or the pre-log-file internal list.
        private static void WriteRaw(string s)
        {
            lock (Locker)
            {
                if (IsOk)
                {
                    try
                    {
                        Logger.WriteLine("{0}", s);
                        Logger.Flush();
                    }
                    catch
                    {
                    }
                }
                else if (PreLogFileLog != null)
                {
                    // we're in the pre-log-file stage - log earlier messages
                    PreLogFileLog.Add(s);
                }
            }
        }

        /// <summary>
        /// Writes the specified message to the logfile.
        /// </summary>
        /// <param name="Message">The message.</param>
        public static void Write(string Message)
        {
            if (Message.IsNullOrWhiteSpace()) {
                WriteRaw("{0}\t{1}".Build(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff"), ""));
            } else {
                foreach (string M in Message.Split(new[] { '\r', '\n' }))
                    WriteRaw("{0}\t{1}".Build(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff"), M));
            }
        }

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        /// <param name="Message">The message.</param>
        public static void Error(string Message)
        {
            Write("Error: {0}".Build(Message));
        }

		/// <summary>
		/// One-time message logging.  Logs a message, identified by a string
		/// key, <b>only</b> the first time the key is used.  This is useful for
		/// logging information that's always stable throughout a session, and 
        /// thus would just repeat the same information over and over if it were
        /// repeated every time the code path was taken.
        /// 
		/// After a message is logged under a given key, subsequent calls with 
        /// the same key will do nothing, suppressing the additional redundant
        /// copies of the message.  Note that the message itself isn't considered
        /// when deciding whether or not the repeat should be suppressed - the
        /// suppression is based purely on the key.  Use this only when you're
        /// reasonably certain that the logged information really is unchanging
        /// throughout any given session.
		/// </summary>
		/// <param name="key">An arbitrary caller-defined string uniquely identifying the message</param>
		/// <param name="message">The message text to log</param>
		static public void Once(string key, string message)
		{
			if (!OneTimeLogMessages.Contains(key))
			{
				OneTimeLogMessages.Add(key);
				Log.Write(message);
			}
		}

        // Hash set containing the Once() keys used so far.  Keys are added
        // to this set as Once() messages are logged, so that repeated
        // messages with the same keys can be suppressed.
        protected static HashSet<string> OneTimeLogMessages = new HashSet<string>();


		/// <summary>
		/// Writes a warning message to the log.
		/// </summary>
		/// <param name="Message">The message.</param>
		public static void Warning(string Message)
        {
            Write("Warning: {0}".Build(Message));
        }

        /// <summary>
        /// Writes a exception message to the log.
        /// </summary>
        /// <param name="Message">The message.</param>
        /// <param name="E">The Exception to be logged.</param>
        public static void Exception(string Message, Exception E = null)
        {
            lock (Locker)
            {
                if (!Message.IsNullOrWhiteSpace())
                    Write("EXCEPTION: {0}".Build(Message));

                Write("EXCEPTION: Thread: {0}".Build(Thread.CurrentThread.Name));
                if (E != null)
                {
                    Write("EXCEPTION: Message: {0} --> {1}".Build(E.GetType().Name, E.Message));

                    foreach (string S in E.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                        Write("EXCEPTION: Stack trace: {0}".Build(S));

                    if (E.TargetSite != null)
                        Write("EXCEPTION: Target site: {0}".Build(E.TargetSite.ToString()));

                    try
                    {
                        // Get stack trace for the exception with source file information
                        StackTrace ST = new StackTrace(E, true);
                        // Get the top stack frame
                        StackFrame Frame = ST.GetFrame(0);

                        int Line = Frame.GetFileLineNumber();
                        string ExceptionFilename = Frame.GetFileName();

                        Write("EXCEPTION: Location: LineNr {0} in {1]".Build(Line, ExceptionFilename));
                    }
                    catch { }

                    for (int Level = 1; E.InnerException != null && Level <= 20; ++Level)
                    {
                        E = E.InnerException;
                        Write("EXCEPTION: InnerException {0}: {1} --> {2}".Build(Level, E.GetType().Name, E.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Writes a exception to the log.
        /// </summary>
        /// <param name="E">The Exception to be logged.</param>
        public static void Exception(Exception E)
        {
            Exception("", E);
        }

        /// <summary>
        /// Will return if all provided instrumentation keys are part of the instrumentations provided
        /// in the GlobalConfig.xml file.
        ///
        /// The &lt;Instrumentation&gt; element in the config file can also be set to a
        /// single asterisk (*) to enable ALL of the instrumentation keys.  That means
        /// that you shouldn't use "*" as an actual message key, of course.
        /// </summary>
        /// <param name="keys">The instrumentation keys.  This is an arbitrary string identifying the instrumentation source, to determine whether or not to include the message in the log.</param>
        public static bool HasInstrumentations(string keys)
        {
            if (_ActiveInstrumentations.Contains("*"))
                return true;
            var keysSplit= keys.Split(',');
            return keysSplit.Intersect(_ActiveInstrumentations, StringComparer.OrdinalIgnoreCase).Count() == keysSplit.Count();
        }

        /// <summary>
        /// Writes the specified instrumentation debug message to the log file, ONLY IF the
        /// provided keys are all present within the keys specified in the &lt;Instrumentation&gt; element
        /// in the GlobalConfig.xml file.  If the keys aren't listed there, the message is
        /// suppressed.  This can be used for debugging messages that you don't want to
        /// include in the log EXCEPT during development work, or when helping a user
        /// troubleshoot a problem that requires visibility into low-level details.  This
        /// is especially useful for messages that are generated many times during a
        /// session, such as messages generated every time an output port is triggered.
        ///
        /// </summary>
        /// <param name="keys">The instrumentation keys.  This is an arbitrary string identifying the instrumentation source, to determine whether or not to include the message in the log.</param>
        /// <param name="Message">The message to be written to the log file.</param>
        public static void Instrumentation(string keys, string Message)
        {
            if (HasInstrumentations(keys))
                Write($"Debug [{keys}]: {Message}");
        }

        /// <summary>
        /// Writes the specified debug message to the log file.
        /// </summary>
        /// <param name="Message">The message to be written to the log file.</param>
        public static void Debug(string Message)
        {
            Write($"Debug: {Message}");
        }

    }
}
