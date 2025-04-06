using System;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using Jotunn;
using LootGoblinsUtils.Utils;

namespace LootGoblinsUtils.Submods.Armor;

public static class ArmorFeatureConfiguration
{
    private static string FileName = "armorconfig.json";

    public static ConfigEntry<bool> ArmorFeatureToggle;
    public static ArmorFeatureConfig Config;

    public static void Init(BaseUnityPlugin plugin, ConfigurationManagerAttributes isAdminOnly)
    {
        SetupFileConfig();
        
        const string section = "Armor Feature";

        ArmorFeatureToggle = plugin.Config.Bind(
            section,
            "ArmorFeatureToggle",
            true,
            new ConfigDescription(
                "Включить генерацию классов брони",
                null,
                isAdminOnly)
        );
    }

    private static void SetupFileConfig()
    {
        var assemblyPath = PathUtil.PluginFolder;
        
        FileSystemWatcher watcher = new FileSystemWatcher(assemblyPath);
        watcher.Changed += WatcherOnChanged;
        watcher.Filter = FileName;
        watcher.EnableRaisingEvents = true;


        ReadConfig(Path.Combine(assemblyPath, FileName));
    }

    private static void WatcherOnChanged(object sender, FileSystemEventArgs e)
    {
        ReadConfig(e.FullPath);
    }

    private static void ReadConfig(string path)
    {
        Logger.LogWarning($"Trying read config at {path}");
        try
        {
            var configText = File.ReadAllText(path);
            Logger.LogWarning("Updated config");
            Logger.LogWarning(configText);
            Config = SimpleJson.SimpleJson.DeserializeObject<ArmorFeatureConfig>(configText);
            ArmorFeature.UpdateArmor();
        }
        catch (Exception exc)
        {
            Logger.LogError(exc);
        }
    }
}