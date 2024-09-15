using BepInEx;
using BepInEx.Configuration;

namespace LootGoblinsUtils.Configuration;

public static class PluginConfiguration
{
    public static ConfigEntry<float> FertilizingDuration;
    public static ConfigEntry<float> PlantGrowMinDuration;
    public static ConfigEntry<float> PlantGrowMaxDuration;
    public static ConfigEntry<bool> ReliableBlockToggle;
    public static ConfigEntry<bool> DisableBedRespawnToggle;
    public static ConfigEntry<bool> FatalProtectionToggle;
    public static ConfigEntry<float> FPNormalDefencePercent;
    public static ConfigEntry<float> FPNormalDefenceBlockingPercent;
    public static ConfigEntry<float> FPNormalDefenceShieldPercent;
    public static ConfigEntry<float> FPNormalDefenceShieldBlockingPercent;
    public static ConfigEntry<float> FPCriticalRatio;
    public static ConfigEntry<float> FPArmorToDurabilityRatio;
    public static ConfigEntry<float> FPBlockPowerToDurabilityRatio;
    public static ConfigEntry<float> FPBlockPowerMultiplier;
    public static ConfigEntry<float> FPEquipmentDamageMultiplier;

    public static ConquestConfiguration Conquest;
    
    public static void InitConfigs(BaseUnityPlugin plugin)
    {
        // Conquest = new ConquestConfiguration(plugin);
        // Conquest.InitConfigs();
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
        
        DisableBedRespawnToggle = plugin.Config.Bind(
            section,
            "DisableBedRespawnToggle",
            true,
            new ConfigDescription(
                "Во включенном состоянии игрок воскрешается только у алтаря",
                null,
                isAdminOnly)
        );
        
        const string fatalSection = "Fatal Protection";
        
        FatalProtectionToggle = plugin.Config.Bind(
            fatalSection,
            "FatalProtectionToggle",
            true,
            new ConfigDescription(
                "Включить систему защиты от смертельных ударов",
                null,
                isAdminOnly)
        );
        
        FPNormalDefencePercent = plugin.Config.Bind(
            fatalSection,
            "FPNormalDefencePercent",
            0.6f,
            new ConfigDescription(
                "Предельный урон от максимального хп без условий",
                new AcceptableValueRange<float>(0, 1f),
                isAdminOnly)
        );
        
        FPNormalDefenceBlockingPercent = plugin.Config.Bind(
            fatalSection,
            "FPNormalDefenceBlockingPercent",
            0.45f,
            new ConfigDescription(
                "Предельный урон от максимального хп при блокировании",
                new AcceptableValueRange<float>(0, 1f),
                isAdminOnly)
        );
        
        FPNormalDefenceShieldPercent = plugin.Config.Bind(
            fatalSection,
            "FPNormalDefenceShieldPercent",
            0.5f,
            new ConfigDescription(
                "Предельный урон от максимального хп при ношении щита",
                new AcceptableValueRange<float>(0, 1f),
                isAdminOnly)
        );
        
        FPNormalDefenceShieldBlockingPercent = plugin.Config.Bind(
            fatalSection,
            "FPNormalDefenceShieldBlockingPercent",
            0.35f,
            new ConfigDescription(
                "Предельный урон от максимального хп при ношении щита и блокировании",
                new AcceptableValueRange<float>(0, 1f),
                isAdminOnly)
        );
        
        FPCriticalRatio = plugin.Config.Bind(
            fatalSection,
            "FPCriticalRatio",
            2f,
            new ConfigDescription(
                "Предел от максимального здоровья после которого защита не сработает",
                new AcceptableValueRange<float>(1, 10f),
                isAdminOnly)
        );

        FPArmorToDurabilityRatio = plugin.Config.Bind(
            fatalSection,
            "FPArmorToDurabilityRatio",
            30f,
            new ConfigDescription(
                "Прочность брони за единицу брони",
                new AcceptableValueRange<float>(0.1f, 100f),
                isAdminOnly)
        );
        
        FPBlockPowerToDurabilityRatio = plugin.Config.Bind(
            fatalSection,
            "FPBlockPowerToDurabilityRatio",
            15f,
            new ConfigDescription(
                "Прочность оружия и щитов за единицу силы блокирования",
                new AcceptableValueRange<float>(0.1f, 100f),
                isAdminOnly)
        );
        
        FPBlockPowerMultiplier = plugin.Config.Bind(
            fatalSection,
            "FPBlockPowerMultiplier",
            2f,
            new ConfigDescription(
                "Множитель силы блокирования",
                new AcceptableValueRange<float>(0.1f, 100f),
                isAdminOnly)
        );
        
        FPEquipmentDamageMultiplier = plugin.Config.Bind(
            fatalSection,
            "FPEquipmentDamageMultiplier",
            2f,
            new ConfigDescription(
                "Множитель урона по предметам при срабатывании защиты",
                new AcceptableValueRange<float>(0.1f, 100f),
                isAdminOnly)
        );
    }
}