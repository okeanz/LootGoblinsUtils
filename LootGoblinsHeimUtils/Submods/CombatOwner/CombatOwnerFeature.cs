using HarmonyLib;
using LootGoblinsUtils.Configuration;

namespace LootGoblinsUtils.Submods.CombatOwner;

public static class CombatOwnerFeature
{
    [HarmonyPatch(typeof(MonsterAI), nameof(MonsterAI.SetTarget))]
    private static class SwitchOwnerToTarget
    {
        private static void Postfix(MonsterAI __instance, Character attacker)
        {
            if (!PluginConfiguration.EnableCombatOwnerToggle.Value) return;

            if (attacker is Player player && player != Player.m_localPlayer)
            {
                __instance.m_nview.GetZDO().SetOwner(attacker.GetZDOID().UserID);
                __instance.m_nview.InvokeRPC("CombatOwner Transfer Owner", __instance);
            }
        }
    }

    [HarmonyPatch(typeof(MonsterAI), nameof(MonsterAI.Awake))]
    private static class AddRPCs
    {
        private static void Postfix(MonsterAI __instance)
        {
            if (!PluginConfiguration.EnableCombatOwnerToggle.Value) return;
            __instance.m_nview.Register("CombatOwner Transfer Owner", _ => GetOwnership(__instance));
            
        }

        private static void GetOwnership(MonsterAI creature)
        {
            creature.m_nview.ClaimOwnership();
            creature.SetTarget(Player.m_localPlayer);
        }
    }
}