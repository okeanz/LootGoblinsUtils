using System;

namespace LootGoblinsUtils.Submods.Armor;

public static class ItemOverrides
{
    public static void ApplyModifiers(ItemDrop.ItemData.SharedData item, OverrideModifiers modifiers)
    {
        if (modifiers == null) return;

        item.m_movementModifier = 0;
        item.m_eitrRegenModifier = 0;
        item.m_homeItemsStaminaModifier = 0;
        item.m_heatResistanceModifier = 0;
        item.m_jumpStaminaModifier = 0;
        item.m_attackStaminaModifier = 0;
        item.m_blockStaminaModifier = 0;
        item.m_dodgeStaminaModifier = 0;
        item.m_swimStaminaModifier = 0;
        item.m_sneakStaminaModifier = 0;
        item.m_runStaminaModifier = 0;

        if (modifiers.Movement.HasValue)
        {
            item.m_movementModifier = modifiers.Movement.Value;
        }

        if (modifiers.BlockStamina.HasValue)
        {
            item.m_blockStaminaModifier = modifiers.BlockStamina.Value;
        }

        if (modifiers.Armor.HasValue)
        {
            item.m_armor = modifiers.Armor.Value;
        }

        if (modifiers.ArmorPerLevel.HasValue)
        {
            item.m_armorPerLevel = modifiers.ArmorPerLevel.Value;
        }
    }

    [Serializable]
    public class OverrideModifiers
    {
        public float? Movement;
        public float? BlockStamina;
        public int? Armor;
        public int? ArmorPerLevel;
    }
}