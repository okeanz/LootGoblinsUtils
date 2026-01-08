using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;
using UnityEngine;

namespace LootGoblinsUtils.Submods.ReactiveGear.GearSets;

public static class GearSetRegistry
{
    public static readonly Dictionary<string, SetEquipmentCounter> EquippedSetCounterBySetName = new();

    public static readonly Dictionary<string, SetDefinition> SetsByPrefab = new();
    public static readonly Dictionary<string, SetDefinition> SetsBySetName = new();
    public static readonly Dictionary<EffectNames, List<string>> EffectDescriptionsByEffectName = new();

    public static void BuildIndex(GearSetList gearSetList)
    {
        SetsByPrefab.Clear();
        SetsBySetName.Clear();
        EffectDescriptionsByEffectName.Clear();

        var setList = gearSetList.setList;

        foreach (var set in setList)
        {
            foreach (var prefabName in set.parts.Where(prefabName => !string.IsNullOrEmpty(prefabName)))
            {
                if (!SetsByPrefab.ContainsKey(prefabName))
                {
                    SetsByPrefab[prefabName] = set;
                }
            }

            SetsBySetName[set.name] = set;
            EffectDescriptionsByEffectName[set.effectBinding] =
                gearSetList.effectDescriptions.Find(x => x.name == set.effectBinding).description;
        }
    }

    public static void CheckEquippedItems()
    {
        if (!Player.m_localPlayer || Player.m_localPlayer.GetInventory() == null) return;

        EquippedSetCounterBySetName.Clear();

        var equipped = Player.m_localPlayer.GetInventory().GetEquippedItems();

        foreach (var prefabName in equipped.Select(itemData => itemData.m_dropPrefab?.name)
                     .Where(prefabName => prefabName != null))
        {
            if (!SetsByPrefab.TryGetValue(prefabName, out var setPartlyEquipped)) continue;

            if (EquippedSetCounterBySetName.TryGetValue(setPartlyEquipped.name, out var setEquipmentCounter))
            {
                setEquipmentCounter.EquippedCount++;
            }
            else
            {
                EquippedSetCounterBySetName[setPartlyEquipped.name] = new SetEquipmentCounter
                    { EquippedCount = 1, MaxCount = setPartlyEquipped.parts.Count };
            }
        }

        EffectManager.EffectActivationCheck();
    }
}

public class SetEquipmentCounter
{
    public int EquippedCount;
    public int MaxCount;
    public bool IsActive => EquippedCount == MaxCount;
}