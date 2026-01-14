using HarmonyLib;
using LootGoblinsUtils.Utils;

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Hooks.Modifiers;

[HarmonyPatch(typeof(CharacterAnimEvent), nameof(CharacterAnimEvent.CustomFixedUpdate))]
public class Hook_AttackSpeed
{
    private static bool IsModified;

    public static void Postfix(CharacterAnimEvent __instance)
    {
        if (!__instance.m_character.IsLocalPlayer()) return;
        if (!__instance.m_character.InAttack())
        {
            IsModified = false;
            return;
        }

        if (!IsModified)
        {
            var mods = EffectManager.GetPlayerModifiers();

            __instance.m_animator.speed *= mods.AttackSpeedMult;
            IsModified = true;
        }
    }
}