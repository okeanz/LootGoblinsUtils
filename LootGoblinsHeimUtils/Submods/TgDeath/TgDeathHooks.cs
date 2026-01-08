using HarmonyLib;
using Jotunn;
using Jotunn.Managers;

// ReSharper disable InconsistentNaming

namespace LootGoblinsUtils.Submods.TgDeath;

[HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
public static class DeathHook
{
    static void Prefix(Player __instance)
    {
        if (__instance == null) return;
        if (__instance.m_lastHit == null) return;
        var deathInfo = new DeathInfo(__instance.m_lastHit, __instance.GetPlayerName());

        Logger.LogInfo($"Player {__instance.name} isDead");
        Logger.LogInfo(deathInfo.ToJson());
        

        
        
        TgDeathMono.Instance.CaptureAndSend(deathInfo.ToJson());
    }
}
