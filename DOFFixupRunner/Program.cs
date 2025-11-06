using System;
using System.Windows.Forms;
using B2SFixup = DOFSetupB2SFixup.CustomActions;
using PBXFixup = DOFSetupPBXFixup.CustomActions;

namespace DOFFixupRunner
{
    internal static class Program
    {
        [STAThread]
        private static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length < 3)
            {
                MessageBox.Show(
                    "FixupRunner requires installPath, binDir, and bitness arguments.",
                    "DirectOutput Fixup Runner",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return 2;
            }

            var installPath = args[0];
            var binDir = args[1];
            var bitness = args[2];

            try
            {
                RunFixup("DirectOutput B2S Integration", B2SFixup.Run, installPath, binDir, bitness);
                RunFixup("DirectOutput PinballX Integration", PBXFixup.Run, installPath, binDir, bitness);
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unexpected error while running DirectOutput fixups:\n\n" + ex.Message,
                    "DirectOutput Fixup Runner",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return 1;
            }
        }

        private static void RunFixup(string title, Func<string, string, string, string> fixup, string installPath, string binDir, string bitness)
        {
            var result = fixup(installPath, binDir, bitness);
            if (!string.IsNullOrEmpty(result))
            {
                MessageBox.Show(result, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
