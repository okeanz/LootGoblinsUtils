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
        public const string PluginVersion = "0.0.1";
        
        private readonly Harmony _harmony = new(PluginGuid);
        
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();
        
        public static bool IsServer => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;

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

            var bushPrefab = PrefabManager.Instance.GetPrefab("RaspberryBush");

            var pc = new PieceConfig
            {
                Name = "Малиновый куст",
                PieceTable = PieceTables.Cultivator,
                Category = PieceCategories.Misc,
                Icon = RenderManager.Instance.Render(bushPrefab, RenderManager.IsometricRotation)
            };
            pc.AddRequirement(new RequirementConfig("Raspberry", 1));

            var newPiece = new CustomPiece("RaspberryBush_LG", Fermenters.Fermenter, pc)
            {
                Piece =
                {
                    m_craftingStation = null,
                    m_cultivatedGroundOnly = true,
                    m_groundOnly = true,
                    m_noClipping = false
                }
            };


            var fermenterComponent = newPiece.PiecePrefab.GetComponent<Fermenter>();
            fermenterComponent.m_conversion.Clear();
            fermenterComponent.m_fermentationDuration = 15f;
            fermenterComponent.m_addedEffects = fermenterComponent.m_spawnEffects;
            fermenterComponent.m_tapEffects.m_effectPrefabs = Array.Empty<EffectList.EffectData>();
            fermenterComponent.m_tapDelay = 0.1f;
            PieceManager.Instance.AddPiece(newPiece);
            
            var model = newPiece.PiecePrefab.FindDeepChild("barrel");
            model.gameObject.SetActive(false);
            
            var top = newPiece.PiecePrefab.FindDeepChild("_top");
            Destroy(top.transform.GetChild(0).gameObject);
            
            var fermentingSFX = newPiece.PiecePrefab.FindDeepChild("_fermenting");
            foreach (Transform sfx in fermentingSFX)
            {
                Destroy(sfx.gameObject);
            }
            
            var readySFX = newPiece.PiecePrefab.FindDeepChild("_ready");
            foreach (Transform sfx in readySFX)
            {
                Destroy(sfx.gameObject);
            }
            
            
            var newModel = Instantiate(bushPrefab.FindDeepChild("model").gameObject, model.transform.parent);
            var berrys = newModel.FindDeepChild("Berrys");
            berrys.gameObject.SetActive(false);
            var collider = newModel.GetComponent<CapsuleCollider>();
            collider.radius = 1.6f;
            collider.isTrigger = true;
            fermenterComponent.m_readyObject = berrys.gameObject;
            

            
            var conversionConfig = new FermenterConversionConfig
            {
                ToItem = "Raspberry",
                FromItem = "BoneFragments",
                Station = "RaspberryBush_LG",
                ProducedItems = 10
            };
            ItemManager.Instance.AddItemConversion(new CustomItemConversion(conversionConfig));
                
                
            Jotunn.Logger.LogWarning("------------ AddBushes end ------------");

            PrefabManager.OnVanillaPrefabsAvailable -= AddBushes;
        }
    }
}