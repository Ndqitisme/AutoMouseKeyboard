using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard.UI
{
    /// <summary>
    /// Detects the Windows app theme (light/dark) from the per-user
    /// Personalize registry key.
    /// </summary>
    public static class WindowsThemeDetector
    {
        public static ThemeMode Detect()
        {
            try
            {
                var v = Microsoft.Win32.Registry.GetValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                    "AppsUseLightTheme", 1);
                return v is int i && i == 0 ? ThemeMode.Dark : ThemeMode.Light;
            }
            catch { return ThemeMode.Light; }
        }
    }
}
