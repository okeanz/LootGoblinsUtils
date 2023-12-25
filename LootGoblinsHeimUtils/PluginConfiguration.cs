using System;
using BepInEx;
using BepInEx.Configuration;

namespace LootGoblinsUtils;

public static class PluginConfiguration
{
    public static ConfigEntry<float> FertilizingDuration;

    public static void InitConfigs(BaseUnityPlugin plugin)
    {
        var isAdminOnly = new ConfigurationManagerAttributes {IsAdminOnly = true};
        const string section = "Defaults";
        FertilizingDuration = plugin.Config.Bind(
            section,
            "FertilizingDuration",
            1500f,
            new ConfigDescription(
                "Длительность роста продуктов",
                new AcceptableValueRange<float>(0, float.MaxValue),
                isAdminOnly)
        );
    }
}