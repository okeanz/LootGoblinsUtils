using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;

namespace JotunnModStub
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class LootGoblinsHeimUtilsPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.lootgoblinsheim.utils";
        public const string PluginName = "LootGoblinsHeimUtils";
        public const string PluginVersion = "0.0.1";
        
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            Jotunn.Logger.LogInfo("LootGoblinsHeimUtilsPlugin has landed");
            
            
        }
    }
}