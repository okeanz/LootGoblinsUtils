using System;
using System.Collections.Generic;
using System.Linq;
using Jotunn;
using Jotunn.Managers;
using LootGoblinsUtils.Configuration;
using LootGoblinsUtils.Utils;

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
        ItemDrop.ItemData.ItemType.Shield,
    };

    private static List<ItemDrop.ItemData.ItemType> _weapon = new()
    {
        ItemDrop.ItemData.ItemType.OneHandedWeapon,
        ItemDrop.ItemData.ItemType.TwoHandedWeapon,
        ItemDrop.ItemData.ItemType.TwoHandedWeaponLeft
    };

    private static bool IsDbUpdated()
    {
        return ObjectDB.instance.m_items.First(x => x != null && x.name == "ShieldBanded").GetComponent<ItemDrop>()
            .m_itemData.m_shared
            .m_blockPower > 50;
    }

    private static void ItemManagerOnOnItemsRegistered()
    {
        Logger.LogInfo("Initialize FatalProtectionFeature");

        if (IsDbUpdated())
        {
            Logger.LogInfo("FatalProtectionFeature: Db already updated, skipping");
            return;
        }

        foreach (var item in ObjectDB.instance.m_items
                     .Select(instanceMItem => instanceMItem.GetComponent<ItemDrop>())
                     .Where(item => _armor.Contains(item.m_itemData.m_shared.m_itemType) ||
                                    _weapon.Contains(item.m_itemData.m_shared.m_itemType)))
        {
            try
            {
                var itemType = item.m_itemData.m_shared.m_itemType;
                var haveDurability =
                    item.m_itemData.m_shared.m_useDurability && item.m_itemData.m_shared.m_maxDurability > 0;


                if (_armor.Contains(itemType) && haveDurability)
                {
                    var targetArmor = item.m_itemData.m_shared.m_armor *
                                      PluginConfiguration.FPArmorToDurabilityRatio.Value;
                    var targetArmorPerLevel = item.m_itemData.m_shared.m_armorPerLevel *
                                              PluginConfiguration.FPArmorToDurabilityRatio.Value;
                    item.m_itemData.m_shared.m_maxDurability =
                        targetArmor > item.m_itemData.m_shared.m_maxDurability
                            ? item.m_itemData.m_shared.m_maxDurability
                            : targetArmor;
                    item.m_itemData.m_shared.m_durabilityPerLevel =
                        targetArmorPerLevel > item.m_itemData.m_shared.m_durabilityPerLevel
                            ? item.m_itemData.m_shared.m_durabilityPerLevel
                            : targetArmorPerLevel;
                }

                if (itemType == ItemDrop.ItemData.ItemType.Shield || _weapon.Contains(itemType))
                {
                    var blockPower = item.m_itemData.m_shared.m_blockPower;
                    item.m_itemData.m_shared.m_blockPower *= PluginConfiguration.FPBlockPowerMultiplier.Value;
                    item.m_itemData.m_shared.m_blockPowerPerLevel *= PluginConfiguration.FPBlockPowerMultiplier.Value;

                    if (haveDurability)
                    {
                        var targetDurability = blockPower * PluginConfiguration.FPBlockPowerToDurabilityRatio.Value;
                        var resultDurability = targetDurability > 100 ? targetDurability : targetDurability + 100;

                        item.m_itemData.m_shared.m_maxDurability =
                            (resultDurability > item.m_itemData.m_shared.m_maxDurability
                                ? item.m_itemData.m_shared.m_maxDurability
                                : resultDurability).RoundOff();
                    }

                    Logger.LogDebug($"[FatalProtectionFeature]: After: " +
                                    $"item: {item.name}; " +
                                    $"m_blockPower: ${item.m_itemData.m_shared.m_blockPower}" +
                                    $"m_blockPowerPerLevel: ${item.m_itemData.m_shared.m_blockPowerPerLevel}" +
                                    $"m_maxDurability: ${item.m_itemData.m_shared.m_maxDurability}" +
                                    $"m_durabilityPerLevel: ${item.m_itemData.m_shared.m_durabilityPerLevel}");
                }

                Logger.LogDebug(
                    $"[FatalProtectionFeature]: {item.GetHoverName()} new durability: {item.m_itemData.m_shared.m_maxDurability}");
            }
            catch (Exception e)
            {
                Logger.LogError($"{item.name}");
                Logger.LogError(e);
            }
        }

        ItemManager.OnItemsRegistered -= ItemManagerOnOnItemsRegistered;
        Logger.LogInfo("Finish FatalProtectionFeature");
    }
}