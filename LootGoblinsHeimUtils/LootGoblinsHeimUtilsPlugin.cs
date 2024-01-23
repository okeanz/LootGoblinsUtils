using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using LootGoblinsUtils.Configuration;
using LootGoblinsUtils.Conquest;
using LootGoblinsUtils.Creatures;
using LootGoblinsUtils.Hooks;
using LootGoblinsUtils.Submods.Farming;
using LootGoblinsUtils.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace LootGoblinsUtils
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.NotEnforced, VersionStrictness.None)]
    internal class LootGoblinsHeimUtilsPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.lootgoblinsheim.utils";
        public const string PluginName = "LootGoblinsHeimUtils";
        public const string PluginVersion = "2.0.0";

        private readonly Harmony _harmony = new(PluginGuid);

        public static readonly CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        public static bool IsServer => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;

        private static KeyboardShortcut ToggleShortcut;
        private static KeyboardShortcut ToggleLoggingShortcut;

        public static bool OptimizationsEnabled;
        public static bool LoggingEnabled;

        public static ConfigFile CFG;

        private void Awake()
        {
            Jotunn.Logger.LogInfo("LootGoblinsHeimUtilsPlugin has landed ");
            CFG = Config;
            Blueprints.Init();
            
            ToggleShortcut = new KeyboardShortcut(KeyCode.LeftShift, KeyCode.T);
            ToggleLoggingShortcut = new KeyboardShortcut(KeyCode.LeftShift, KeyCode.Y);

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);

            ProfilePatcher.Patch(_harmony);

            PluginConfiguration.InitConfigs(this);
            FarmingSetup.Init();
            CreatureDB.Init();

            if(PluginConfiguration.Conquest.ConquestFeature.Value)
                ConquestFeature.Load();
        }


        private void Update()
        {
            if (ToggleShortcut.IsDown())
            {
                OptimizationsEnabled = !OptimizationsEnabled;
                Logger.LogMessage($"OptimizationsEnabled set to {OptimizationsEnabled}");
            }
            
            if (ToggleLoggingShortcut.IsDown())
            {
                LoggingEnabled = !LoggingEnabled;
                Logger.LogMessage($"LoggingEnabled set to {LoggingEnabled}");
            }
        }

        private void LateUpdate()
        {
            if (!OptimizationsEnabled) return;
            ProfilePatcher.LateUpdate();
        }
    }
}