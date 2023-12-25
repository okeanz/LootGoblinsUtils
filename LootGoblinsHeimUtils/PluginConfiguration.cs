using System;
using BepInEx;
using BepInEx.Configuration;

namespace LootGoblinsUtils;

public static class PluginConfiguration
{
    public static ConfigEntry<float> FertilizingDuration;
    public static ConfigEntry<float> PlantGrowMinDuration;
    public static ConfigEntry<float> PlantGrowMaxDuration;

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
        
        PlantGrowMinDuration = plugin.Config.Bind(
            section,
            "PlantGrowMinDuration",
            4000f,
            new ConfigDescription(
                "Минимальная длительность роста растений",
                new AcceptableValueRange<float>(0, float.MaxValue),
                isAdminOnly)
        );
        
        PlantGrowMaxDuration = plugin.Config.Bind(
            section,
            "PlantGrowMaxDuration",
            5000f,
            new ConfigDescription(
                "Максимальная длительность роста растений",
                new AcceptableValueRange<float>(0, float.MaxValue),
                isAdminOnly)
        );
    }
}