using System;
using System.Linq;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using LootGoblinsUtils.Pieces;
using LootGoblinsUtils.Pieces.Configurators;
using LootGoblinsUtils.Pieces.Stations;
using UnityEngine;
using UnityEngine.Rendering;

namespace LootGoblinsUtils
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class LootGoblinsHeimUtilsPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.lootgoblinsheim.utils";
        public const string PluginName = "LootGoblinsHeimUtils";
        public const string PluginVersion = "1.0.0";
        
        private readonly Harmony _harmony = new(PluginGuid);
        
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();
        
        public static bool IsServer => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;

        private void Awake()
        {
            Jotunn.Logger.LogInfo("LootGoblinsHeimUtilsPlugin has landed");
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            
            PluginConfiguration.InitConfigs(this);
            
            PrefabManager.OnVanillaPrefabsAvailable += Setup;

        }

        private void Setup()
        {
            Jotunn.Logger.LogInfo("------------ LootGoblinsHeimUtilsPlugin start ------------");
            BushUtils.CacheDependencies();
            
            FarmersTable.Configure();
            FarmersTableExtensionT1.Configure();
            
            RaspberryBush.Setup();
            BlueberryBush.Setup();
            CloudberryBush.Setup();
            ThistleBush.Setup();
            
            Mushroom.Configure();
            MushroomYellow.Configure();
            Dandelion.Configure();

            Saplings.ReplaceRecipes();

            Jotunn.Logger.LogInfo("------------ LootGoblinsHeimUtilsPlugin end ------------");

            PrefabManager.OnVanillaPrefabsAvailable -= Setup;
        }
    }
}