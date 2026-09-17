
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMouseKeyboard.Models;
using Newtonsoft.Json;

namespace AutoMouseKeyboard.Services
{
    public class ConfigStorage
    {
    private readonly string _configFolder;

    public ConfigStorage()
    {
        var baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AutoMouseKeyboard");
        _configFolder = Path.Combine(baseFolder, "configs");
        Directory.CreateDirectory(_configFolder);
        MigrateLegacyConfigs();
    }

    /// <summary>
    /// One-time bring-forward of configs saved by the previous app folder
    /// (Roaming\AutoClickerByNDQ). Copies only files that don't already exist —
    /// never overwrites anything the user created in the new folder.
    /// </summary>
    private void MigrateLegacyConfigs()
    {
        try
        {
            var legacyFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AutoClickerByNDQ", "configs");
            if (!Directory.Exists(legacyFolder))
            {
                return;
            }

            foreach (var file in Directory.EnumerateFiles(legacyFolder, "*.json"))
            {
                var destination = Path.Combine(_configFolder, Path.GetFileName(file));
                if (!File.Exists(destination))
                {
                    File.Copy(file, destination);
                }
            }
        }
        catch
        {
        }
    }

    public IReadOnlyList<ActionConfig> LoadAll()
    {
        var configs = new List<ActionConfig>();
        foreach (var file in Directory.EnumerateFiles(_configFolder, "*.json"))
        {
            try
            {
                var json = File.ReadAllText(file);
                var config = JsonConvert.DeserializeObject<ActionConfig>(json);
                if (config == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(config.Name))
                {
                    config.Name = Path.GetFileNameWithoutExtension(file);
                }

                configs.Add(config);
            }
            catch
            {
            }
        }

        return configs
            .OrderBy(c => c.Name, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    public void Save(ActionConfig config, string? previousName = null)
    {
        var normalizedName = string.IsNullOrWhiteSpace(config.Name) ? "Config" : config.Name.Trim();

        if (!string.IsNullOrWhiteSpace(previousName) &&
            !string.Equals(previousName, normalizedName, StringComparison.CurrentCulture))
        {
            var oldPath = BuildPath(previousName);
            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }
        }

        config.Name = normalizedName;
        var path = BuildPath(normalizedName);
        var json = JsonConvert.SerializeObject(config, Formatting.Indented);
        File.WriteAllText(path, json);
    }

    public void Delete(string name)
    {
        var path = BuildPath(name);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public string Import(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var config = JsonConvert.DeserializeObject<ActionConfig>(json);
        if (config == null)
        {
            throw new InvalidOperationException("File cấu hình không hợp l�?");
        }

        var resolvedName = EnsureUniqueName(config.Name ?? string.Empty);
        config.Name = resolvedName;
        Save(config);
        return resolvedName;
    }

    public void Export(ActionConfig config, string destinationPath)
    {
        var json = JsonConvert.SerializeObject(config, Formatting.Indented);
        File.WriteAllText(destinationPath, json);
    }

    public string EnsureUniqueName(string requestedName)
    {
        var baseName = string.IsNullOrWhiteSpace(requestedName) ? "Config" : requestedName.Trim();
        var candidate = baseName;
        var index = 1;
        while (File.Exists(BuildPath(candidate)))
        {
            candidate = string.Format("{0} ({1})", baseName, index++);
        }

        return candidate;
    }

    private string BuildPath(string name)
    {
        var safe = SanitizeName(name);
        return Path.Combine(_configFolder, string.Format("{0}.json", safe));
    }

    private static string SanitizeName(string name)
    {
        var fallback = "Config";
        var normalized = (name ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            return fallback;
        }

        var invalid = Path.GetInvalidFileNameChars();
        var cleaned = new string(normalized
            .Select(ch => invalid.Contains(ch) ? '_' : ch)
            .ToArray());

        return string.IsNullOrWhiteSpace(cleaned) ? fallback : cleaned;
    }
    }
}

