using HarmonyLib;

// ReSharper disable InconsistentNaming

namespace LootGoblinsUtils.Submods.ReactiveGear.GearSets.Hooks;

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.EquipItem), typeof(ItemDrop.ItemData), typeof(bool))]
public static class Player_EquipItem_Patch
{
    static void Postfix(Humanoid __instance)
    {
        if (__instance == null) return;
        if (__instance != Player.m_localPlayer) return;

        GearSetRegistry.CheckEquippedItems();
    }
}

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UnequipItem), typeof(ItemDrop.ItemData), typeof(bool))]
public static class Player_UnequipItem_Patch
{
    static void Postfix(Humanoid __instance, ItemDrop.ItemData item, bool triggerEquipEffects)
    {
        if (__instance == null) return;
        if (__instance != Player.m_localPlayer) return;

        GearSetRegistry.CheckEquippedItems();
    }
}