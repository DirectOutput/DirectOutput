using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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

        /// <summary>
        /// Initializes the log with the spcified filename.
        /// </summary>
        /// <param name="Filename">The name of the logfile.</param>
        public static void Init(string Filename)
        {
            lock (Locker)
            {
                if (!IsInitialized)
                {
                    try
                    {
                        Logger = File.AppendText(Filename);

                        Logger.WriteLine("{0}\t{1}", DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss.fff"), "DirectOutput Logger initialized");


                        IsOk = true;

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
        /// Writes the specified message to the logfile.
        /// </summary>
        /// <param name="Message">The message.</param>
        public static void Write(string Message)
        {
            lock (Locker)
            {
                if (IsOk)
                {
                    if (Message.IsNullOrWhiteSpace())
                    {
                        Logger.WriteLine("{0}\t{1}", DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss.fff"), "");
                    }
                    else
                    {
                        foreach (string M in Message.Split(new[] { '\r', '\n' }))
                        {
                            Logger.WriteLine("{0}\t{1}", DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss.fff"), M);
                        }
                    }
                }
            }
        }

        public static void Warning(string Message)
        {
            Write("Warning: {0}".Build(Message));
        }

        public static void Exception(string Message = "", Exception E = null)
        {
            if (!Message.IsNullOrWhiteSpace())
            {
                Write("EXCEPTION: {0}".Build(Message));
            }
            if (E != null)
            {
                Write("EXCEPTION: {0}".Build(E.Message));

                int Level = 1;
                while (E.InnerException!=null) {
                    E = E.InnerException;
                    Write("EXCEPTION: InnerException {0}: {1}".Build(Level,E.Message));
                    Level++;
                }
            }
        }

        public static void Exception(Exception E = null)
        {
            Exception("",E);
        }

    }
}
