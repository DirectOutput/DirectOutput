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
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LedControlFileTestWizard());
        }
    }
}
