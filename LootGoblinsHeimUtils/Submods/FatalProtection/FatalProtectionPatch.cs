using System;
using System.Collections.Generic;
using HarmonyLib;
using Jotunn;
using LootGoblinsUtils.Configuration;

namespace LootGoblinsUtils.Submods.FatalProtection;

public class FatalProtectionPatch
{
    [HarmonyPatch(typeof(Character), nameof(Character.SetHealth))]
    public static class SetHealthPatch
    {
        private enum PlayerStance
        {
            NoWeapon,
            WeaponBlocking,
            ShieldEquipped,
            ShieldBlocking
        }

        private static readonly Dictionary<PlayerStance, float> _stanceDamageLimit = new()
        {
            {PlayerStance.NoWeapon, PluginConfiguration.FPNormalDefencePercent.Value},
            {PlayerStance.WeaponBlocking, PluginConfiguration.FPNormalDefenceBlockingPercent.Value},
            {PlayerStance.ShieldEquipped, PluginConfiguration.FPNormalDefenceShieldPercent.Value},
            {PlayerStance.ShieldBlocking, PluginConfiguration.FPNormalDefenceShieldBlockingPercent.Value}
        };
        
        private static bool DoPlayerHaveShield(Player player)
        {
            return DoPlayerHaveShield(player, out var _);
        }

        private static bool DoPlayerHaveShield(Player player, out ItemDrop.ItemData shield)
        {
            if (player.LeftItem != null && player.LeftItem.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shield)
            {
                shield = player.LeftItem;
                return true;
            }

            shield = null;
            return false;
        }

        private static PlayerStance GetCurrentStance(Player player)
        {
            var weapon = player.GetCurrentWeapon();
            if (weapon == null) return PlayerStance.NoWeapon;
            
            if (DoPlayerHaveShield(player))
            {
                return player.IsBlocking() ? PlayerStance.ShieldBlocking : PlayerStance.ShieldEquipped;
            }

            if (player.IsBlocking())
                return PlayerStance.WeaponBlocking;

            return PlayerStance.NoWeapon;
        }

        private static List<ItemDrop.ItemData> GetEquipment(Player player, PlayerStance stance)
        {
            var result = new List<ItemDrop.ItemData>();
            switch (stance)
            {
                case PlayerStance.ShieldBlocking:
                case PlayerStance.ShieldEquipped:
                    if(DoPlayerHaveShield(player, out var shield) && shield.m_shared.m_useDurability) result.Add(shield);
                    break;
                case PlayerStance.WeaponBlocking:
                    if(player.RightItem != null && player.RightItem.m_shared.m_useDurability) result.Add(player.RightItem);
                    break;
            }
            
            if(player.m_chestItem != null && player.m_chestItem.m_shared.m_useDurability) result.Add(player.m_chestItem);
            if(player.m_legItem != null && player.m_legItem.m_shared.m_useDurability) result.Add(player.m_legItem);
            if(player.m_helmetItem != null && player.m_helmetItem.m_shared.m_useDurability) result.Add(player.m_helmetItem);
            if(player.m_shoulderItem != null && player.m_shoulderItem.m_shared.m_useDurability) result.Add(player.m_shoulderItem);

            return result;
        }
        
        private static void DamageEquipment(Player player, PlayerStance stance, float damage)
        {
            Logger.LogDebug($"[FatalProtectionPatch] Damaging equipment: Excess damage: {damage}, stance: {stance}");

            var residualDamage = damage * PluginConfiguration.FPEquipmentDamageMultiplier.Value;

            var equipment = GetEquipment(player, stance);
            
            foreach (var itemData in equipment)
            {
                if (itemData.m_durability > residualDamage)
                {
                    itemData.m_durability -= residualDamage;
                    residualDamage = 0;
                }
                else
                {
                    residualDamage -= itemData.m_durability;
                    itemData.m_durability = 0;
                }
                Logger.LogDebug($"[FatalProtectionPatch] {itemData.m_shared.m_name}.durability: {itemData.m_durability} ");
                if(residualDamage == 0) return;
            }
        }

        public static void Prefix(Character __instance, ref float health)
        {
            if (!PluginConfiguration.FatalProtectionToggle.Value) return;
            if (__instance != Player.m_localPlayer) return;

            var player = Player.m_localPlayer;

            var currentHealth = player.GetHealth();
            if (health > currentHealth) return;


            var damage = currentHealth - health;
            var maxHp = player.GetMaxHealth();
            var ratio = damage / maxHp;


            if (ratio > PluginConfiguration.FPCriticalRatio.Value)
            {
                Logger.LogDebug(
                    $"[FatalProtectionPatch]: Cant save you. ratio: {ratio} > {PluginConfiguration.FPCriticalRatio.Value}");
                return;
            }

            if (ratio <= PluginConfiguration.FPNormalDefencePercent.Value) return;


            var stance = GetCurrentStance(player);
            var limit = _stanceDamageLimit[stance];

            var newDamage = maxHp * limit;
            health = currentHealth - newDamage;

            DamageEquipment(player, stance, damage - newDamage);


            Logger.LogDebug(
                $"[FatalProtectionPatch]: currentHealth: {currentHealth} " +
                $"damage: {damage}; maxHp: {maxHp}; " +
                $"ratio: {ratio}; newDamage: {newDamage}" +
                $"limit: {limit}; stance: {Enum.GetName(typeof(PlayerStance), stance)}");
        }
    }
}