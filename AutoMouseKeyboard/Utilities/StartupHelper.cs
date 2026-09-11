using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AutoMouseKeyboard.Utilities
{
    public static class StartupHelper
    {
        private const string RunRegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "AutoMouseKeyboard";

        public static bool IsEnabled()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RunRegistryPath, false))
                {
                    var value = key?.GetValue(AppName) as string;
                    return !string.IsNullOrEmpty(value);
                }
            }
            catch
            {
                return false;
            }
        }

        public static void SetStartup(bool enable)
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RunRegistryPath, true) ??
                                 Registry.CurrentUser.CreateSubKey(RunRegistryPath, true))
                {
                    if (key == null)
                    {
                        return;
                    }

                    if (enable)
                    {
                        var exePath = $"\"{Application.ExecutablePath}\"";
                        key.SetValue(AppName, exePath);
                    }
                    else
                    {
                        key.DeleteValue(AppName, false);
                    }
                }
            }
            catch
            {
            }
        }
    }
}

