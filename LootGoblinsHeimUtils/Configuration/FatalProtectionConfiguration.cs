using BepInEx;
using BepInEx.Configuration;

namespace LootGoblinsUtils.Configuration;

public static class FatalProtectionConfiguration
{
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
    public static ConfigEntry<float> FPMeleeElementalResist;
    public static ConfigEntry<float> FPMeleePhysicalResist;
    
    public static void Init(BaseUnityPlugin plugin, ConfigurationManagerAttributes isAdminOnly)
    {
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
            3f,
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
        
        FPMeleeElementalResist = plugin.Config.Bind(
            fatalSection,
            "FPMeleeElementalResist",
            0.3f,
            new ConfigDescription(
                "Базовый элементальный резист для бойцов ближнего боя",
                new AcceptableValueRange<float>(0f, 1f),
                isAdminOnly)
        );
        
        FPMeleeElementalResist = plugin.Config.Bind(
            fatalSection,
            "FPMeleeElementalResist",
            0.2f,
            new ConfigDescription(
                "Базовый физический резист для бойцов ближнего боя",
                new AcceptableValueRange<float>(0f, 1f),
                isAdminOnly)
        );
    }
}