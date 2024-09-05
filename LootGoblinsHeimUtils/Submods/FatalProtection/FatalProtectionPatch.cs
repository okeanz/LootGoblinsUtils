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
        private static List<Skills.SkillType> _rangedSkills = new()
        {
            Skills.SkillType.Bows,
            Skills.SkillType.BloodMagic,
            Skills.SkillType.Crossbows,
            Skills.SkillType.ElementalMagic
        };

        private enum PlayerStance
        {
            NoWeapon,
            WeaponBlocking,
            ShieldEquipped,
            ShieldBlocking
        }

        private static Dictionary<PlayerStance, float> _stanceDamageLimit = new()
        {
            {PlayerStance.NoWeapon, PluginConfiguration.FPNormalDefencePercent.Value},
            {PlayerStance.WeaponBlocking, PluginConfiguration.FPNormalDefenceBlockingPercent.Value},
            {PlayerStance.ShieldEquipped, PluginConfiguration.FPNormalDefenceShieldPercent.Value},
            {PlayerStance.ShieldBlocking, PluginConfiguration.FPNormalDefenceShieldBlockingPercent.Value}
        };

        private static PlayerStance GetCurrentStance(Player player)
        {
            var weapon = player.GetCurrentWeapon();
            if (weapon == null) return PlayerStance.NoWeapon;

            if (!_rangedSkills.Contains(weapon.m_shared.m_skillType) && player.IsBlocking())
                return PlayerStance.WeaponBlocking;

            if (player.LeftItem != null && player.LeftItem.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shield)
            {
                return player.IsBlocking() ? PlayerStance.ShieldBlocking : PlayerStance.ShieldEquipped;
            }

            return PlayerStance.NoWeapon;
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


            Logger.LogDebug(
                $"[FatalProtectionPatch]: currentHealth: {currentHealth} " +
                $"damage: {damage}; maxHp: {maxHp}; " +
                $"ratio: {ratio}; newDamage: {newDamage}" +
                $"limit: {limit}; stance: {Enum.GetName(typeof(PlayerStance), stance)}");
        }
    }
}