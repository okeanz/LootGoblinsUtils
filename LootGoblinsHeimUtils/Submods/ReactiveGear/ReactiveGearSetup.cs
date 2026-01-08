using BepInEx.Configuration;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;
using LootGoblinsUtils.Submods.ReactiveGear.GearSets;
using LootGoblinsUtils.Utils;
using Newtonsoft.Json;

namespace LootGoblinsUtils.Submods.ReactiveGear;

public static class ReactiveGearSetup
{
    public static FileJsonConfig<GearSetList> GearConfig;

    public static void Init()
    {
        GearConfig = new FileJsonConfig<GearSetList>(
            LootGoblinsHeimUtilsPlugin.Instance,
            "gearSets.json",
            list =>
            {
                GearSetRegistry.BuildIndex(list);
            }
        );

        EffectManager.Init();
    }
    
}