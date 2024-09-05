using System.Collections.Generic;
using System.Linq;
using Jotunn;
using Jotunn.Managers;
using LootGoblinsUtils.Configuration;

namespace LootGoblinsUtils.Submods.FatalProtection;

public static class FatalProtectionFeature
{
    public static void Init()
    {
        if (!PluginConfiguration.FatalProtectionToggle.Value) return;

        ItemManager.OnItemsRegistered += ItemManagerOnOnItemsRegistered;
    }

    private static List<ItemDrop.ItemData.ItemType> _armor = new()
    {
        ItemDrop.ItemData.ItemType.Chest,
        ItemDrop.ItemData.ItemType.Helmet,
        ItemDrop.ItemData.ItemType.Legs,
        ItemDrop.ItemData.ItemType.Shoulder,
        ItemDrop.ItemData.ItemType.Shield
    };

    private static void ItemManagerOnOnItemsRegistered()
    {
        Logger.LogInfo("Initialize FatalProtectionFeature");

        foreach (var item in ObjectDB.instance.m_items
                     .Select(instanceMItem => instanceMItem.GetComponent<ItemDrop>())
                     .Where(item => _armor.Contains(item.m_itemData.m_shared.m_itemType))
                )
        {
            if (item.m_itemData.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shield)
            {
                var blockPower = item.m_itemData.GetBaseBlockPower();
                item.m_itemData.m_shared.m_blockPower *= PluginConfiguration.FPBlockPowerMultiplier.Value;
                item.m_itemData.m_shared.m_maxDurability = blockPower * PluginConfiguration.FPBlockPowerToDurabilityRatio.Value;
            }
            else
            {
                var armor = item.m_itemData.GetArmor();
                item.m_itemData.m_shared.m_maxDurability = armor * PluginConfiguration.FPArmorToDurabilityRatio.Value;
            }

            Logger.LogDebug(
                $"[FatalProtectionFeature]: {item.GetHoverName()} new durability: {item.m_itemData.m_shared.m_maxDurability}");
        }

        ItemManager.OnItemsRegistered -= ItemManagerOnOnItemsRegistered;
        Logger.LogInfo("Finish FatalProtectionFeature");
    }
}