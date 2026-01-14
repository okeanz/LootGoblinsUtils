using HarmonyLib;
using LootGoblinsUtils.Utils;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Hooks.Events;

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.StartAttack), typeof(Character), typeof(bool))]
public static class Hook_HumanoidEvents
{
    public static void Postfix(Humanoid __instance, Character target, bool secondaryAttack, bool __result)
    {
        if (!__result) return;

        if (!__instance.IsLocalPlayer()) return;

        EffectManager.Handle(new CombatEvent(CombatEventType.AttackStarted, Time.time, secondaryAttack));
    }
}

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.IsBlocking))]
public static class Humanoid_IsBlocking
{
    private static bool wasBlocking;
    
    public static void Postfix(Humanoid __instance, bool __result)
    {
        if (!__instance.IsLocalPlayer()) return;
        if(wasBlocking == __result) return;
        

        var isBlocking = __result;

        if (!wasBlocking && isBlocking)
        {
            EffectManager.Handle(new CombatEvent(CombatEventType.Block, Time.time));
        }

        wasBlocking = __result;
    }
}