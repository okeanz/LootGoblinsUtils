using HarmonyLib;
using Jotunn;

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Hooks.Events;

[HarmonyPatch(typeof(CharacterAnimEvent), nameof(CharacterAnimEvent.CustomFixedUpdate))]
public class Hook_AttackSpeed
{
    
    public static void Postfix(CharacterAnimEvent __instance)
    {
        if (__instance.m_character != Player.m_localPlayer || !__instance.m_character.InAttack()) return;

        var mods = EffectManager.GetPlayerModifiers();

        __instance.m_animator.speed = mods.AttackSpeedMult;
    }
}