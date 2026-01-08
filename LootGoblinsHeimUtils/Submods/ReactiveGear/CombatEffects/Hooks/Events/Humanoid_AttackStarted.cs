using HarmonyLib;
using LootGoblinsUtils.Utils;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Hooks.Events;

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.StartAttack))]
public static class Humanoid_AttackStarted
{
    public static void Postfix(Humanoid __instance, bool __result)
    {
        if (!__result) return;

        if (!__instance.IsLocalPlayer()) return;

        EffectManager.Handle(new CombatEvent(CombatEventType.AttackStarted, Time.time));
    }
}