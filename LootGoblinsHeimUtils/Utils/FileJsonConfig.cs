using System;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using Jotunn;
using Jotunn.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LootGoblinsUtils.Utils;

public sealed class FileJsonConfig<TConfig> : IDisposable where TConfig : class
{
    private readonly Action<TConfig> _onUpdated;
    private readonly FileSystemWatcher _watcher;

    private readonly EventHandler _entryChangedHandler;

    private bool _suppressEntryHandler;

    public ConfigEntry<string> SyncedJson { get; }
    public TConfig Config { get; private set; }

    public FileJsonConfig(BaseUnityPlugin plugin, string fileName, Action<TConfig> onUpdated = null)
    {
        if (fileName == null) throw new ArgumentNullException(nameof(fileName));
        _onUpdated = onUpdated;

        // Синкаемый "контейнер" JSON (серверная истина)
        SyncedJson = plugin.Config.Bind(
            "Synced",
            $"{typeof(TConfig).Name}Json",
            "",
            new ConfigDescription(
                "Server-synced JSON (minified).",
                null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }
            )
        );

        _entryChangedHandler = (_, __) =>
        {
            if (!_suppressEntryHandler) ApplyJson(SyncedJson.Value);
        };
        SyncedJson.SettingChanged += _entryChangedHandler;

        var folder = PathUtil.PluginFolder;
        var fullPath = Path.Combine(folder, fileName);

        _watcher = new FileSystemWatcher(folder)
        {
            Filter = fileName,
            EnableRaisingEvents = true
        };

        _watcher.Changed += WatcherOnChanged;

        // Первичная загрузка:
        //  - если файл есть: читаем и пушим в SyncedJson (на сервере это разойдётся)
        //  - если файла нет: пытаемся применить текущее значение SyncedJson (клиенты получат от сервера)
        if (File.Exists(fullPath))
        {
            var text = File.ReadAllText(fullPath);
            PushFileJsonToEntry(text);
        }
        else
        {
            ApplyJson(SyncedJson.Value);
        }
    }

    private void WatcherOnChanged(object sender, FileSystemEventArgs e)
    {
        if (!ZNet.instance || !ZNet.instance.IsServer())
            return;
        // Идея: файл меняется только на сервере (источник истины),
        // поэтому просто перечитываем и пушим в ConfigEntry.
        try
        {
            var text = File.ReadAllText(e.FullPath);
            PushFileJsonToEntry(text);
        }
        catch
        {
            // минимально: игнор
        }
    }

    private void PushFileJsonToEntry(string json)
    {
        try
        {
            // Обновляем entry так, чтобы не делать двойную работу в хэндлере SettingChanged
            _suppressEntryHandler = true;
            SyncedJson.Value = json; // Jotunn синкнет клиентам
        }
        finally
        {
            _suppressEntryHandler = false;
        }

        // Применяем локально сразу
        ApplyJson(json);
    }

    private void ApplyJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return;

        try
        {
            var parsed = Newtonsoft.Json.JsonConvert.DeserializeObject<TConfig>(json, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
            });
            if (parsed == null) return;

            Config = parsed;
            _onUpdated?.Invoke(Config);
        }
        catch (Exception e)
        {
            Logger.LogError($"Failed to parse json: {json}");
            Logger.LogError(e);
        }
    }

    public void Dispose()
    {
        SyncedJson.SettingChanged -= _entryChangedHandler;
        _watcher.Changed -= WatcherOnChanged;
        _watcher.Dispose();
    }
}