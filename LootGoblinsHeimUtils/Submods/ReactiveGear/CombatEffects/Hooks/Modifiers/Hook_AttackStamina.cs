using HarmonyLib;
using LootGoblinsUtils.Utils;

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Hooks.Modifiers;

[HarmonyPatch(typeof(Attack), nameof(Attack.GetAttackStamina))]
public class Hook_AttackStamina
{
    public static void Postfix(Attack __instance, ref float __result)
    {
        if (!__instance.m_character.IsLocalPlayer()) return;
        
        __result *= EffectManager.GetPlayerModifiers().AttackStaminaCostMult;
    }
}

[HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetDrawStaminaDrain))]
public class Hook_AttackStaminaBow
{
    public static void Postfix(Attack __instance, ref float __result)
    {
        if (!__instance.m_character.IsLocalPlayer()) return;
        
        __result *= EffectManager.GetPlayerModifiers().AttackStaminaCostMult;
    }
}