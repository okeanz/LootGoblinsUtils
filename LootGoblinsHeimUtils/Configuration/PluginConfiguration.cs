using BepInEx;
using BepInEx.Configuration;
using LootGoblinsUtils.Submods.Armor;

namespace LootGoblinsUtils.Configuration;

public static class PluginConfiguration
{
    public static ConfigEntry<float> FertilizingDuration;
    public static ConfigEntry<float> PlantGrowMinDuration;
    public static ConfigEntry<float> PlantGrowMaxDuration;
    public static ConfigEntry<bool> ReliableBlockToggle;
    public static ConfigEntry<bool> DisableBedRespawnToggle;
    public static ConfigEntry<bool> EnableCombatOwnerToggle;
    public static ConfigEntry<bool> EnableEventCreatureDropsToggle;
    public static ConfigEntry<bool> EnableEventReward;
    public static ConfigEntry<int> EventRewardMultiplier;

    public static ConquestConfiguration Conquest;
    
    public static void InitConfigs(BaseUnityPlugin plugin)
    {
        var isAdminOnly = new ConfigurationManagerAttributes {IsAdminOnly = true};
        FatalProtectionConfiguration.Init(plugin, isAdminOnly);
        ArmorFeatureConfiguration.Init(plugin, isAdminOnly);
        
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
        
        DisableBedRespawnToggle = plugin.Config.Bind(
            section,
            "DisableBedRespawnToggle",
            true,
            new ConfigDescription(
                "Во включенном состоянии игрок воскрешается только у алтаря",
                null,
                isAdminOnly)
        );
        
        EnableCombatOwnerToggle = plugin.Config.Bind(
            "Feature Combat Owner",
            "EnableCombatOwnerToggle",
            false,
            new ConfigDescription(
                "Меняет владельца атакующего моба на его цель. Облегчает парирование и уклонение в мультиплеере",
                null,
                isAdminOnly)
        );
        
        EnableEventCreatureDropsToggle = plugin.Config.Bind(
            "Feature Event Creature",
            "EnableEventCreatureDropsToggle",
            true,
            new ConfigDescription(
                "Отключает дроп с ивентовых мобов",
                null,
                isAdminOnly)
        );
        
        EnableEventReward = plugin.Config.Bind(
            "Feature Event Creature",
            "EnableEventReward",
            true,
            new ConfigDescription(
                "Спаун сундуков с наградой",
                null,
                isAdminOnly)
        );
        
        EventRewardMultiplier = plugin.Config.Bind(
            "Feature Event Creature",
            "EventRewardMultiplier",
            200,
            new ConfigDescription(
                "Множитель награды за событие",
                new AcceptableValueRange<int>(0, 10000),
                isAdminOnly)
        );
    }
}