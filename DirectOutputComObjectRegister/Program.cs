using System;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Namespace for the DirectOutputComObjectRegister application.
/// </summary>
namespace DirectOutputComObjectRegister
{
    /// <summary>
    /// Main class of the DirectOutputComObjectRegister application.
    /// </summary>
    class Program
    {
        /// <summary>
        /// This method does the actual registration work for the DirectOutputComObject.
        /// </summary>
        static void Main(string[] args)
        {
            string Result = "";

            string RegAsm = Path.Combine(Environment.ExpandEnvironmentVariables("%systemroot%"), "Microsoft.NET", "Framework", System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion(), "regasm.exe");
            if (File.Exists(RegAsm))
            {
                string ComObject = Path.Combine(new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).Directory.FullName, "DirectOutputComObject.dll");
                if (File.Exists(ComObject))
                {

                    System.Diagnostics.ProcessStartInfo ProcStartInfo = new System.Diagnostics.ProcessStartInfo();
                    ProcStartInfo.FileName = RegAsm;
                    ProcStartInfo.Arguments = "\"" + ComObject + "\" /silent /nologo /codebase";

                    ProcStartInfo.RedirectStandardOutput = true;
                    ProcStartInfo.UseShellExecute = false;
                    // Do not create the black window.
                    ProcStartInfo.CreateNoWindow = true;

                    // Now we create a process, assign its ProcessStartInfo and start it
                    System.Diagnostics.Process Proc = new System.Diagnostics.Process();
                    Proc.StartInfo = ProcStartInfo;
                    Proc.Start();

                    Result = Proc.StandardOutput.ReadToEnd();


                    if (Proc.ExitCode == 0 && Result.Length == 0)
                    {
                        MessageBox.Show("DirectOutput COM object successfully registered.", "DirectOutput", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Registering the DirectOutput COM object returned the following information:\nExit Code: {0}\nMessage: {1}", Proc.ExitCode, Result), "DirectOutput", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("Could not register the DirectOutput COM object, since the file could not be found.\nMissing file: {0}", ComObject), "DirectOutput", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(string.Format("Could not register the DirectOutput COM object, since the regasm.exe could not be found.\nMissing file: {0}", RegAsm), "DirectOutput", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }
    }
}