using HarmonyLib;
using LootGoblinsUtils.Utils;

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Hooks.Modifiers;

[HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyStaminaRegen))]
public static class Hook_StaminaRegen
{
    public static void Postfix(SEMan __instance, ref float staminaMultiplier)
    {
        if (!__instance.m_character.IsLocalPlayer()) return;

        staminaMultiplier *= EffectManager.GetPlayerModifiers().StaminaRegenMult;
    }
}