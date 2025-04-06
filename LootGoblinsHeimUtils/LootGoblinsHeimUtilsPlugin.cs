using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using LootGoblinsUtils.Configuration;
using LootGoblinsUtils.Submods.Armor;
using LootGoblinsUtils.Submods.Farming;
using LootGoblinsUtils.Submods.FatalProtection;
using LootGoblinsUtils.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace LootGoblinsUtils
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Main.ModGuid)] 
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Patch)]
    internal class LootGoblinsHeimUtilsPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.lootgoblinsheim.utils";
        public const string PluginName = "LootGoblinsHeimUtils";
        public const string PluginVersion = "2.2.4";

        private readonly Harmony _harmony = new(PluginGuid);

        public static readonly CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        public static ConfigFile CFG;

        private void Awake()
        {
            Jotunn.Logger.LogInfo("------------ LootGoblinsHeimUtilsPlugin start ------------");
            CFG = Config;
            

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);

            PluginConfiguration.InitConfigs(this);
            FarmingSetup.Init();
            FatalProtectionFeature.Init();
            ArmorFeature.Init();
            
            // Blueprints.Init();
            /*if(PluginConfiguration.Conquest.ConquestFeature.Value)
                ConquestFeature.Load();
            CreatureDB.Init();
                */
            
            Jotunn.Logger.LogInfo("------------ LootGoblinsHeimUtilsPlugin end ------------");
        }
    }
}