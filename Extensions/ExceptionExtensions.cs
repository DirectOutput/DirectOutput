using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


public static class ExceptionExtensions
{
    public static List<string> GetNestedMessages(this Exception E)
    {
        List<string> L = new List<string>();
        L.Add(E.Message);
        if (E.InnerException != null)
        {
            L.AddRange(GetNestedMessages(E.InnerException));
        }
        return L;
    }

    public static string GetFullExceptionDetails(this Exception E)
    {
        StringBuilder SB = new StringBuilder();


        SB.AppendLine("Message: {0} --> {1}".Build(E.GetType().Name, E.Message));

        SB.AppendLine("Thread: {0}".Build(System.Threading.Thread.CurrentThread.Name));
        SB.AppendLine("Source: {0}".Build(E.Source));


        foreach (string S in E.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
        {
            SB.AppendLine("Stacktrace: {0}".Build(S));
        }

        if (E.TargetSite != null)
        {
            SB.AppendLine("Targetsite: {0}".Build(E.TargetSite.ToString()));
        }

        try
        {
            // Get stack trace for the exception with source file information
            StackTrace ST = new StackTrace(E, true);
            // Get the top stack frame
            StackFrame Frame = ST.GetFrame(0);

            int Line = Frame.GetFileLineNumber();
            string ExceptionFilename = Frame.GetFileName();

            SB.AppendLine("Location: LineNr {0} in {1]".Build(Line, ExceptionFilename));
        }
        catch { }


        //Output inner exceptions
        Exception EInner = E;
        int Level = 1;
        while (EInner.InnerException != null)
        {
            EInner = EInner.InnerException;
            SB.AppendLine("InnerException {0}: {1} --> {2}".Build(Level, EInner.GetType().Name, EInner.Message));
            Level++;

            if (Level > 30)
            {
                break;
            }
        }


        return SB.ToString();
    }



}

