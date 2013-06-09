using System;
using System.Windows.Forms;

/// <summary>
/// Namespace for the LedControlFileTester application.
/// </summary>
namespace LedControlFileTester
{
    static class Program
    {
        /// <summary>
        /// Entry point of the LedControlFileTester application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length < 1)
            {
                Application.Run(new LedControlFileTestWizard());
            }
            else
            {
                Application.Run(new LedControlFileTestWizard(args[0]));

            }
        }
    }
}
