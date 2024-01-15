using System;
using BepInEx;
using BepInEx.Configuration;

namespace LootGoblinsUtils;

public static class PluginConfiguration
{
    public static ConfigEntry<float> FertilizingDuration;
    public static ConfigEntry<float> PlantGrowMinDuration;
    public static ConfigEntry<float> PlantGrowMaxDuration;
    public static ConfigEntry<bool> ReliableBlockToggle;

    public static void InitConfigs(BaseUnityPlugin plugin)
    {
        var isAdminOnly = new ConfigurationManagerAttributes {IsAdminOnly = true};
        const string section = "Defaults";
        FertilizingDuration = plugin.Config.Bind(
            section,
            "FertilizingDuration",
            4500f,
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
        
        ReliableBlockToggle = plugin.Config.Bind(
            section,
            "ReliableBlockToggle",
            true,
            new ConfigDescription(
                "Во включенном состоянии снижение урона блокированием работает даже если получено оглушение, что делает блокирование более предсказуемым",
                null,
                isAdminOnly)
        );
    }
}