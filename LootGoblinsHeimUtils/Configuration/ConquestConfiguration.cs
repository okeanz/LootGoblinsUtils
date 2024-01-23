using BepInEx;
using BepInEx.Configuration;

namespace LootGoblinsUtils.Configuration;

public class ConquestConfiguration
{
    public ConfigEntry<bool> ConquestFeature;
    public ConfigEntry<float> PlayerAlertRadius;
    public ConfigEntry<float> DefendRadius;
    public ConfigEntry<float> SpawnRadius;
    public ConfigEntry<float> SpawnRate;
    public ConfigEntry<int> GuardMaxCount;
    public ConfigEntry<int> OfficerMaxCount;
    public ConfigEntry<float> DefenderRaidTime;

    private ConfigurationManagerAttributes isAdminOnly = new() {IsAdminOnly = true};
    private const string SectionConquest = "Conquest";
    private BaseUnityPlugin _plugin;

    private ConfigEntry<T> _config<T>(string name, string desc, T defaultValue) => _plugin.Config.Bind(
        SectionConquest,
        name,
        defaultValue,
        new ConfigDescription(desc, null, isAdminOnly)
    );

    public ConquestConfiguration(BaseUnityPlugin plugin)
    {
        _plugin = plugin;
    }

    public void InitConfigs()
    {
        ConquestFeature = _config(
            "ConquestFeature",
            "Режим завоевания",
            true
        );

        PlayerAlertRadius = _config(
            "PlayerAlertRadius",
            "Радиус в котором должен находиться игрок, чтобы ядро перешло в режим внимания",
            100f
        );
        
        DefendRadius = _config(
            "DefendRadius",
            "Радиус в котором должен находиться игрок, чтобы ядро перешло в режим защиты",
            80f
        );
        
        SpawnRadius = _config(
            "SpawnRadius",
            "Радиус спауна мобов аванпостами",
            30f
        );
        
        SpawnRate = _config(
            "SpawnRate",
            "Таймер дореспа мобов",
            10f
        );

        GuardMaxCount = _config(
            "GuardMaxCount",
            "Количество защитников на ядро",
            3
        );

        OfficerMaxCount = _config(
            "OfficerMaxCount",
            "Количество элиток на ядро",
            4
        );
        
        DefenderRaidTime = _config(
            "DefenderRaidTime",
            "Время в течении которого надо защищать тотем",
            60f
        );
    }
}