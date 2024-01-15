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
using LootGoblinsUtils.Creatures;
using LootGoblinsUtils.Hooks;
using LootGoblinsUtils.Pieces;
using LootGoblinsUtils.Pieces.Configurators;
using LootGoblinsUtils.Pieces.Stations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace LootGoblinsUtils
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.NotEnforced, VersionStrictness.None)]
    internal class LootGoblinsHeimUtilsPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.lootgoblinsheim.utils";
        public const string PluginName = "LootGoblinsHeimUtils";
        public const string PluginVersion = "1.0.1";

        private readonly Harmony _harmony = new(PluginGuid);

        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        public static bool IsServer => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;

        private static KeyboardShortcut ToggleShortcut;
        private static KeyboardShortcut ToggleLoggingShortcut;

        public static bool OptimizationsEnabled;
        public static bool LoggingEnabled;

        private void Awake()
        {
            Jotunn.Logger.LogInfo("LootGoblinsHeimUtilsPlugin has landed ");

            ToggleShortcut = new KeyboardShortcut(KeyCode.LeftShift, KeyCode.T);
            ToggleLoggingShortcut = new KeyboardShortcut(KeyCode.LeftShift, KeyCode.Y);

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);

            ProfilePatcher.Patch(_harmony);

            PluginConfiguration.InitConfigs(this);

            PrefabManager.OnVanillaPrefabsAvailable += Setup;
            // CreatureManager.OnVanillaCreaturesAvailable += CreatureManagerOnOnVanillaCreaturesAvailable;
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

        private void CreatureManagerOnOnVanillaCreaturesAvailable()
        {
            CreatureDB.LoadCreatures();
            CreatureManager.OnVanillaCreaturesAvailable -= CreatureManagerOnOnVanillaCreaturesAvailable;
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