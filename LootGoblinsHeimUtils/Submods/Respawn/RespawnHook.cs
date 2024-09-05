using HarmonyLib;
using LootGoblinsUtils.Configuration;

namespace LootGoblinsUtils.Submods.Respawn;

public class RespawnHook
{
    [HarmonyPatch(typeof(PlayerProfile), nameof(PlayerProfile.SetCustomSpawnPoint))]
    public static class SetCustomSpawnPoint
    {
        public static bool Prefix()
        {
            
            return !PluginConfiguration.DisableBedRespawnToggle.Value;
        }
    }
}