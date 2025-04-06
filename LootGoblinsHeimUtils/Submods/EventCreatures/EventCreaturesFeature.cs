using System;
using HarmonyLib;
using Jotunn;
using LootGoblinsUtils.Configuration;
using LootGoblinsUtils.Utils;
using UnityEngine;
using Logger = Jotunn.Logger;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace LootGoblinsUtils.Submods.EventCreatures;

public static class EventCreaturesFeature
{
    [HarmonyPatch(typeof(MonsterAI), nameof(MonsterAI.SetEventCreature))]
    private static class EventCreaturesFeaturePostfix
    {
        private static void Postfix(MonsterAI __instance)
        {
            if (!PluginConfiguration.EnableEventCreatureDropsToggle.Value || __instance == null) return;

            var drop = __instance.GetComponent<CharacterDrop>();

            if (drop != null)
            {
                drop.m_drops.Clear();
                drop.m_dropsEnabled = false;

                Logger.LogDebug(
                    $"[EventCreaturesFeature]: Event Creature ({__instance.name}) loot removed");
            }
            else
            {
                Logger.LogDebug(
                    $"[EventCreaturesFeature]: Event Creature ({__instance.name}) no ItemDrop found");
            }
        }
    }

    [HarmonyPatch(typeof(CharacterDrop), nameof(CharacterDrop.DropItems))]
    private static class CharacterEventDrop
    {
        [HarmonyPriority(2000)]
        private static bool Prefix(CharacterDrop __instance)
        {
            if (!PluginConfiguration.EnableEventCreatureDropsToggle.Value || __instance == null) return true;

            if (!__instance.m_dropsEnabled)
                return false;

            if (RandEventSystem.instance.m_activeEvent != null &&
                RandEventSystem.instance.IsInsideRandomEventArea(RandEventSystem.instance.m_activeEvent,
                    __instance.transform.position))
                return false;

            return true;
        }
    }

    [HarmonyPatch(typeof(RandomEvent), nameof(RandomEvent.OnDeactivate))]
    private static class EventSpawnLoot
    {
        private static void Postfix(RandomEvent __instance)
        {
            if (!PluginConfiguration.EnableEventReward.Value || __instance == null) return;

            try
            {
                var eventPosition = __instance.m_pos;


                var playersInRange = GameObjectExtensions.GetPlayersInRadius(eventPosition, __instance.m_eventRange);

                if (playersInRange.Count == 0 || !playersInRange.Contains(Player.m_localPlayer))
                {
                    Logger.LogInfo($"[EventSpawnLoot] playersInRange {playersInRange.Count}");
                    Logger.LogInfo(
                        $"[EventSpawnLoot] this player in range: {playersInRange.Contains(Player.m_localPlayer)}");

                    return;
                }

                var spawnPosition = Player.m_localPlayer.transform.position + Random.insideUnitSphere * 5f;

                ZoneSystem.instance.GetGroundData(ref spawnPosition, out var normal, out _, out _, out _);

                const string treasureChestPrefabName = "piece_chest_wood";
                var treasureChestPrefab = ZNetScene.instance.GetPrefab(treasureChestPrefabName);
                var treasureChestObject = Object.Instantiate(treasureChestPrefab, spawnPosition,
                    Quaternion.FromToRotation(Vector3.up, normal));
                var container = treasureChestObject.GetComponent<Container>();

                Logger.LogInfo(
                    $"Spawned container for {Player.m_localPlayer.GetPlayerName()}; position: {spawnPosition}");

                if (container != null)
                {
                    container.m_name = $"Награда события для {Player.m_localPlayer.GetPlayerName()}";
                    container.GetInventory().RemoveAll();

                    container.GetInventory().AddItem("Coins", PluginConfiguration.EventRewardMultiplier.Value, 1, 0, 0,
                        string.Empty);

                    container.m_autoDestroyEmpty = true;
                    container.m_piece.SetCreator(Player.m_localPlayer.GetPlayerID());
                    container.m_privacy = Container.PrivacySetting.Private;


                    container.Save();
                    ZDOMan.instance.ForceSendZDO(container.m_nview.GetZDO().m_uid);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }
}