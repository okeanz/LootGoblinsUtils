using HarmonyLib;
using LootGoblinsUtils.Utils;
using UnityEngine;
using Logger = Jotunn.Logger;

// ReSharper disable InconsistentNaming

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Hooks.Events;

[HarmonyPatch(typeof(Character), nameof(Character.Damage))]
public class Character_Damage
{
    public static void Prefix(Character __instance, HitData hit)
    {
        if (hit == null) return;


        var attacker = hit.GetAttacker(); // обычно возвращает Character
        if (attacker is Player player && player.IsLocalPlayer())
        {
            Logger.LogInfo($"before: {hit.GetTotalDamage()}");
            var mods = EffectManager.GetPlayerModifiers();
            if (player.m_currentAttackIsSecondary)
            {
                hit.m_damage.Modify(mods.SecondaryDamageMult);
            }
            else
            {
                hit.m_damage.Modify(mods.LightDamageMult);
            }

            hit.m_damage.Modify(mods.DamageMult);
            
            Logger.LogInfo($"after: {hit.GetTotalDamage()}");
        }


        if (!attacker.IsLocalPlayer()) return;

        EffectManager.Handle(new CombatEvent(CombatEventType.HitLanded, Time.time));
    }
}