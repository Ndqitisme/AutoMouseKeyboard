
using System;
using System.Windows.Forms;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            if (!AppInstanceManager.TryLockInstance())
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new MainForm());
            }
            finally
            {
                AppInstanceManager.Release();
            }
        }
    }
}
