// Use #define DebugLog or #undef DebugLog to turn debug log messages on or off.
#define DEBUGLOG

using System;
using System.IO;
using System.Diagnostics;

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

        private static object Locker = new object();

        private static string _Filename=".\\DirectOutput.log";

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
        

        /// <summary>
        /// Initializes the log using the file defnied in the Filename property.
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

                        Logger.WriteLine("---------------------------------------------------------------------------------");
                        Logger.WriteLine("{0}\t{1}", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff"), "DirectOutput Logger initialized");

                        Version V = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                        DateTime BuildDate = new DateTime(2000, 1, 1).AddDays(V.Build).AddSeconds(V.Revision * 2);
                        Logger.WriteLine("{0}\t{1}", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff"), "DirectOutput Version {0} as of {1}".Build(V.ToString(), BuildDate.ToString("yyyy.MM.dd HH:mm")));
                        
                        IsOk = true;
                        
                        Debug("Writting of debug log messages is enabled");

                       

                    }
                    catch
                    {
                        IsOk = false;
                    }

                    IsInitialized = true;
                }
            }
        }

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


        /// <summary>
        /// Writes the specified message to the logfile.
        /// </summary>
        /// <param name="Message">The message.</param>
        public static void Write(string Message)
        {
            
            lock (Locker)
            {
                if (IsOk)
                {
                    try
                    {

                        if (Message.IsNullOrWhiteSpace())
                        {
                            Logger.WriteLine("{0}\t{1}", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff"), "");
                        }
                        else
                        {
                            foreach (string M in Message.Split(new[] { '\r', '\n' }))
                            {
                                Logger.WriteLine("{0}\t{1}", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff"), M);
                            }
                        }
                        Logger.Flush();
                    }
                    catch 
                    {
                        
                    }
                }
            }
        }

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
        public static void Exception(string Message = "", Exception E = null)
        {
            lock (Locker)
            {
                if (!Message.IsNullOrWhiteSpace())
                {
                    Write("EXCEPTION: {0}".Build(Message));
                }
                if (E != null)
                {
                    Write("EXCEPTION: {0}".Build(E.Message));

                    int Level = 1;
                    while (E.InnerException != null)
                    {
                        E = E.InnerException;
                        Write("EXCEPTION: InnerException {0}: {1}".Build(Level, E.Message));
                        Level++;

                        if (Level > 20)
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Writes a exception to the log.
        /// </summary>
        /// <param name="E">The Exception to be logged.</param>
        public static void Exception(Exception E = null)
        {
            Exception("",E);
        }


        //TODO: Make conditional compilation work
        /// <summary>
        /// Writes the specified debug message to the log file.
        /// \note The calls to this method are only executed, if the DebugLog symbol is defined. Generally this will only be active in special debug releases. The statement to define or undefine the DebugLog symbol can be found on the top of the code of this class.
        /// </summary>
        /// <param name="Message">The message to be written to the log file.</param>
        [Conditional("DEBUGLOG")]
        public static void Debug(string Message = "")
        {
            Write("Debug: {0}".Build(Message));

        }


    }
}
