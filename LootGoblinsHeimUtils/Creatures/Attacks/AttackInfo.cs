using HarmonyLib;
using Jotunn.Managers;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Creatures.Attacks;

public class AttackInfo
{
    public GameObject Prefab;

    public AttackInfo(string prefabName)
    {
        Prefab = PrefabManager.Instance.GetPrefab(prefabName);
        if (Prefab == null)
            Logger.LogError($"AttackInfo: Cant find prefab '{prefabName}'");
    }

    public AttackInfo(GameObject prefab)
    {
        Prefab = prefab;
        if (Prefab == null)
            Logger.LogError($"AttackInfo: prefab is null");
    }

    public void AddToCharactersAttacks(GameObject characterPrefab)
    {
        var humanoid = characterPrefab.GetComponent<Humanoid>();
        humanoid.m_defaultItems = humanoid.m_defaultItems.AddToArray(Prefab);
    }

    public void ReplaceCharactersAttacks(GameObject characterPrefab)
    {
        var humanoid = characterPrefab.GetComponent<Humanoid>();
        humanoid.m_defaultItems = new[] { Prefab };
    }

    public void OverrideBy(AttackInfo target)
    {
        var sharedData = GetSharedData();
        var targetSharedData = target.GetSharedData();

        sharedData.m_damages = targetSharedData.m_damages.Clone();
        sharedData.m_attackForce = targetSharedData.m_attackForce;
        sharedData.m_attack.m_hitTerrain = targetSharedData.m_attack.m_hitTerrain;
        sharedData.m_attack.m_attackRange = targetSharedData.m_attack.m_attackRange;
        sharedData.m_attack.m_attackHeight = targetSharedData.m_attack.m_attackHeight;
        sharedData.m_attack.m_attackRayWidth = targetSharedData.m_attack.m_attackRayWidth;
        sharedData.m_attack.m_hitThroughWalls = targetSharedData.m_attack.m_hitThroughWalls;
        sharedData.m_attack.m_projectileBursts = targetSharedData.m_attack.m_projectileBursts;
        sharedData.m_attack.m_attackProjectile = targetSharedData.m_attack.m_attackProjectile;
        sharedData.m_attack.m_attackType = targetSharedData.m_attack.m_attackType;
        sharedData.m_attack.m_burstEffect = targetSharedData.m_attack.m_burstEffect;
        sharedData.m_attack.m_hitEffect = targetSharedData.m_attack.m_hitEffect;
        sharedData.m_attack.m_startEffect = targetSharedData.m_attack.m_startEffect;
        sharedData.m_attack.m_triggerEffect = targetSharedData.m_attack.m_triggerEffect;
        sharedData.m_attack.m_trailStartEffect = targetSharedData.m_attack.m_trailStartEffect;
        sharedData.m_attack.m_trailStartEffect = targetSharedData.m_attack.m_trailStartEffect;
        sharedData.m_aiAttackRange = targetSharedData.m_aiAttackRange;
        sharedData.m_aiAttackInterval = targetSharedData.m_aiAttackInterval;
        sharedData.m_aiAttackMaxAngle = targetSharedData.m_aiAttackMaxAngle;
        sharedData.m_aiPrioritized = targetSharedData.m_aiPrioritized;
        sharedData.m_startEffect = targetSharedData.m_startEffect;
        sharedData.m_triggerEffect = targetSharedData.m_triggerEffect;
        sharedData.m_hitEffect = targetSharedData.m_hitEffect;
        sharedData.m_trailStartEffect = targetSharedData.m_trailStartEffect;
        sharedData.m_attack.m_speedFactorRotation = targetSharedData.m_attack.m_speedFactorRotation;
        sharedData.m_attack.m_attackAngle = targetSharedData.m_attack.m_attackAngle;
        sharedData.m_attack.m_useCharacterFacing = targetSharedData.m_attack.m_useCharacterFacing;
        sharedData.m_attack.m_useCharacterFacingYAim = targetSharedData.m_attack.m_useCharacterFacingYAim;
        sharedData.m_attack.m_burstInterval = targetSharedData.m_attack.m_burstInterval;
        
    }

    public static AttackInfo FromCharacterAttack(GameObject characterPrefab, int index, string newName) =>
        new(PrefabManager.Instance.CreateClonedPrefab(newName,
            characterPrefab.GetComponent<Humanoid>().m_defaultItems[index]));

    public GameObject GetClonedPrefab(string newName) => PrefabManager.Instance.CreateClonedPrefab(newName, Prefab);
    public ItemDrop.ItemData.SharedData GetSharedData() => Prefab.GetComponent<ItemDrop>().m_itemData.m_shared;
}