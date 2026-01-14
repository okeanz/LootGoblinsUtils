using HarmonyLib;
using LootGoblinsUtils.Utils;
using UnityEngine;

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Hooks.Events;

[HarmonyPatch(typeof(Player), nameof(Player.UpdateDodge))]
public static class Hook_PlayerUpdateDodge
{
    private static bool wasInDodge;
    public static void Prefix(Player __instance)
    {
        if (!__instance.IsLocalPlayer()) return;

        if (!wasInDodge && __instance.m_inDodge)
        {
            EffectManager.Handle(new CombatEvent(CombatEventType.DodgeAction, Time.time));
            wasInDodge = true;
        }

        if (wasInDodge && !__instance.m_inDodge)
        {
            wasInDodge = false;
            Player_HitWhileDodging_Patch.IsExecuted = false;
        }
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.HitWhileDodging))]
public class Player_HitWhileDodging_Patch
{
    public static bool IsExecuted;
    static void Postfix(Player __instance)
    {
        if (!__instance.IsLocalPlayer())
            return;

        if(IsExecuted) return;
        
        EffectManager.Handle(new CombatEvent(CombatEventType.DodgeSuccess, Time.time));
        IsExecuted = true;
    }
}