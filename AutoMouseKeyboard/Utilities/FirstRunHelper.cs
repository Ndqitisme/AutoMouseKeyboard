using System;
using System.IO;
using System.Linq;

namespace AutoMouseKeyboard.Utilities
{
    public static class FirstRunHelper
{
    private static readonly string SettingsFile;

    static FirstRunHelper()
    {
        var baseFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AutoMouseKeyboard");
        Directory.CreateDirectory(baseFolder);
        SettingsFile = Path.Combine(baseFolder, "settings.txt");
    }

    public static bool ShouldShowCapturePositionHint()
    {
        try
        {
            return !AppSettings.CapturePositionHintShown;
        }
        catch
        {
            return true;
        }
    }

    public static void MarkCapturePositionHintShown()
    {
        try
        {
            AppSettings.CapturePositionHintShown = true;
        }
        catch
        {
        }
    }

    public static int LoadLoopCount(int defaultValue = 30)
    {
        try
        {
            return AppSettings.LoopCount;
        }
        catch
        {
            return defaultValue;
        }
    }

    public static void SaveLoopCount(int value)
    {
        try
        {
            AppSettings.LoopCount = value;
        }
        catch
        {
        }
    }

    public static int LoadGlobalDelay(int defaultValue = 0)
    {
        try
        {
            return AppSettings.GlobalDelay;
        }
        catch
        {
            return defaultValue;
        }
    }

    public static void SaveGlobalDelay(int value)
    {
        try
        {
            AppSettings.GlobalDelay = value;
        }
        catch
        {
        }
    }
    }
}

