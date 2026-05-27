using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Lumina.Models;

namespace Lumina.Services;

public class SettingsService
{
    private static readonly string DataFile = Path.Combine(
    AppContext.BaseDirectory, "data", "settings.json");

    public Settings CurrentSettings { get; private set; } = new Settings { Hours = 24 };

    public SettingsService()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(DataFile)!);
        LoadSettings();
    }

    public void LoadSettings()
    {
        if (!File.Exists(DataFile)) return;
        try
        {
            var json = File.ReadAllText(DataFile);
            var s = JsonSerializer.Deserialize<Settings>(json);
            if (s != null) CurrentSettings = s;
        }
        catch { }
    }

    public Task ChangeHour(int hour)
    {
        CurrentSettings.Hours = hour;
        var json = JsonSerializer.Serialize(CurrentSettings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(DataFile, json);
        return Task.CompletedTask;
    }
}
