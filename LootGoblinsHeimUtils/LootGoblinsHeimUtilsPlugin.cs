using System.Reflection;
using BepInEx;
using HarmonyLib;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;

namespace LootGoblinsUtils
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class LootGoblinsHeimUtilsPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.lootgoblinsheim.utils";
        public const string PluginName = "LootGoblinsHeimUtils";
        public const string PluginVersion = "0.0.1";
        
        private readonly Harmony _harmony = new(PluginGuid);
        
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            Jotunn.Logger.LogInfo("LootGoblinsHeimUtilsPlugin has landed");
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);

            PrefabManager.OnVanillaPrefabsAvailable += AddBushes;

        }

        private void AddBushes()
        {
            Jotunn.Logger.LogWarning("------------ AddBushes start ------------");
            
            var bushPiece = PieceManager.Instance.GetPiece("RaspberryBush");
            Jotunn.Logger.LogWarning($"bushPiece found = {bushPiece != null}");
            
            var fermentorPiece = PieceManager.Instance.GetPiece("fermentor");
            
            Jotunn.Logger.LogWarning($"fermentorPiece found = {fermentorPiece != null}");

            var pc = new PieceConfig();
            pc.Name = "Малиновый куст";
            pc.PieceTable = PieceTables.Cultivator;
            pc.AddRequirement(new RequirementConfig("Raspberry", 1, 1));

            PieceManager.Instance.AddPiece(new CustomPiece("RaspberryBush_LG", "fermenter", pc));
                
                
            Jotunn.Logger.LogWarning("------------ AddBushes end ------------");

            PrefabManager.OnVanillaPrefabsAvailable -= AddBushes;
        }
    }
}