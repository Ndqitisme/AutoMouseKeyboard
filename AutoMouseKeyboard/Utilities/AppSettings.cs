
using System;
using System.IO;
using System.Linq;
using AutoMouseKeyboard.UI;
using Newtonsoft.Json;

namespace AutoMouseKeyboard.Utilities
{
    public static class AppSettings
    {
    private static readonly string SettingsFile;
    private static AppSettingsData? _cachedSettings;

    static AppSettings()
    {
        var baseFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AutoMouseKeyboard");
        Directory.CreateDirectory(baseFolder);
        SettingsFile = Path.Combine(baseFolder, "app_settings.json");
    }

    public static string SettingsFilePath => SettingsFile;

    private static AppSettingsData LoadSettings()
    {
        if (_cachedSettings != null)
        {
            return _cachedSettings;
        }

        try
        {
            if (File.Exists(SettingsFile))
            {
                var json = File.ReadAllText(SettingsFile);
                var deserialized = JsonConvert.DeserializeObject<AppSettingsData>(json);
                _cachedSettings = deserialized ?? new AppSettingsData();
            }
            else
            {
                _cachedSettings = new AppSettingsData();
            }
        }
        catch
        {
            _cachedSettings = new AppSettingsData();
        }

        return _cachedSettings;
    }

    private static void SaveSettings()
    {
        try
        {
            if (_cachedSettings != null)
            {
                var json = JsonConvert.SerializeObject(_cachedSettings, Formatting.Indented);
                File.WriteAllText(SettingsFile, json);
            }
        }
        catch
        {
        }
    }

    public static ThemeMode Theme
    {
        get
        {
            var settings = LoadSettings();
            return settings.Theme ?? WindowsThemeDetector.Detect();
        }
        set
        {
            var settings = LoadSettings();
            settings.Theme = value;
            _cachedSettings = settings;
            SaveSettings();
        }
    }

    public static Language Language
    {
        get
        {
            var settings = LoadSettings();
            return settings.Language.HasValue ? settings.Language.Value : Language.English;
        }
        set
        {
            var settings = LoadSettings();
            settings.Language = value;
            _cachedSettings = settings;
            SaveSettings();
        }
    }

    public static int LoopCount
    {
        get
        {
            var settings = LoadSettings();
            return settings.LoopCount.HasValue ? settings.LoopCount.Value : 30;
        }
        set
        {
            var settings = LoadSettings();
            settings.LoopCount = value;
            _cachedSettings = settings;
            SaveSettings();
        }
    }

    public static int GlobalDelay
    {
        get
        {
            var settings = LoadSettings();
            return settings.GlobalDelay.HasValue ? settings.GlobalDelay.Value : 0;
        }
        set
        {
            var settings = LoadSettings();
            settings.GlobalDelay = value;
            _cachedSettings = settings;
            SaveSettings();
        }
    }

    public static bool CapturePositionHintShown
    {
        get
        {
            var settings = LoadSettings();
            return settings.CapturePositionHintShown.HasValue ? settings.CapturePositionHintShown.Value : false;
        }
        set
        {
            var settings = LoadSettings();
            settings.CapturePositionHintShown = value;
            _cachedSettings = settings;
            SaveSettings();
        }
    }

    public static System.Collections.Generic.List<string> ConfigOrder
    {
        get
        {
            var settings = LoadSettings();
            return settings.ConfigOrder ?? new System.Collections.Generic.List<string>();
        }
        set
        {
            var settings = LoadSettings();
            settings.ConfigOrder = value ?? new System.Collections.Generic.List<string>();
            _cachedSettings = settings;
            SaveSettings();
        }
    }

    public static bool RunOnStartup
    {
        get
        {
            var settings = LoadSettings();
            return settings.RunOnStartup.HasValue && settings.RunOnStartup.Value;
        }
        set
        {
            var settings = LoadSettings();
            settings.RunOnStartup = value;
            _cachedSettings = settings;
            SaveSettings();
        }
    }

    public static bool HideOnCompletion
    {
        get
        {
            var settings = LoadSettings();
            return settings.HideOnCompletion.HasValue && settings.HideOnCompletion.Value;
        }
        set
        {
            var settings = LoadSettings();
            settings.HideOnCompletion = value;
            _cachedSettings = settings;
            SaveSettings();
        }
    }

    public static void Refresh()
    {
        _cachedSettings = null;
    }
}

internal class AppSettingsData
{
    [JsonProperty("theme")]
    public ThemeMode? Theme { get; set; }

    [JsonProperty("language")]
    public Language? Language { get; set; }

    [JsonProperty("loopCount")]
    public int? LoopCount { get; set; }

    [JsonProperty("globalDelay")]
    public int? GlobalDelay { get; set; }

    [JsonProperty("capturePositionHintShown")]
    public bool? CapturePositionHintShown { get; set; }

    [JsonProperty("configOrder")]
    public System.Collections.Generic.List<string>? ConfigOrder { get; set; } = new System.Collections.Generic.List<string>();

    [JsonProperty("runOnStartup")]
    public bool? RunOnStartup { get; set; }

    [JsonProperty("hideOnCompletion")]
    public bool? HideOnCompletion { get; set; }
    }
}

