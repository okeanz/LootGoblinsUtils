using HarmonyLib;
using LootGoblinsUtils.Utils;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Hooks.Events;

[HarmonyPatch(typeof(Character), nameof(Character.Damage))]
public class Character_HitLanded
{
    public static void Prefix(Character __instance, HitData hit)
    {
        if (hit == null) return;


        var attacker = hit.GetAttacker(); // обычно возвращает Character
        if (!attacker.IsLocalPlayer()) return;

        EffectManager.Handle(new CombatEvent(CombatEventType.HitLanded, Time.time));
    }
}