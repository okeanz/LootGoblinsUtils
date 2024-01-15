using HarmonyLib;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;

namespace LootGoblinsUtils.Creatures;

public static class CreatureDB
{
    public static void LoadCreatures()
    {
        var stormBoarConfig = new CreatureConfig();
        var stormBoar = new CustomCreature("LG_StormBoar", "Boar", stormBoarConfig);

        var stormBoarPrefab = stormBoar.Prefab;
        stormBoarPrefab.transform.localScale *= 2;

        var lightningFx = FinePrefabs.LightningUnitEffectCloned;
        Object.Destroy(lightningFx.GetComponent<ZNetView>());
        var fx = Object.Instantiate(lightningFx, stormBoarPrefab.transform);
        fx.transform.localPosition = Vector3.zero;
        
        var boarHumanoid = stormBoarPrefab.GetComponent<Humanoid>();
        boarHumanoid.m_name = "Штормовой Боров";
        boarHumanoid.m_health = 500;
        var speedMult = 1.3f;
        boarHumanoid.m_speed *= speedMult;
        boarHumanoid.m_turnSpeed *= speedMult;
        boarHumanoid.m_runSpeed *= speedMult;
        boarHumanoid.m_runTurnSpeed *= speedMult;


        var eikthyrCharge = PrefabManager.Instance.GetPrefab("Eikthyr").GetComponent<Humanoid>().m_defaultItems[1]
            .GetComponent<ItemDrop>();
        var attackItem = boarHumanoid.m_defaultItems[0];
        var sharedData = attackItem.GetComponent<ItemDrop>().m_itemData.m_shared;

        sharedData.m_damages.m_lightning = 20;
        sharedData.m_attackForce = 100;
        sharedData.m_attack.m_hitTerrain = true;
        sharedData.m_attack.m_attackRange = 20;
        sharedData.m_attack.m_attackHeight = 2;
        sharedData.m_attack.m_attackRayWidth = 2;
        sharedData.m_attack.m_hitThroughWalls = true;
        sharedData.m_aiAttackRange = 15;
        sharedData.m_aiAttackInterval = 5;
        sharedData.m_aiAttackMaxAngle = 15;
        sharedData.m_aiPrioritized = true;
        sharedData.m_startEffect = eikthyrCharge.m_itemData.m_shared.m_startEffect;
        sharedData.m_triggerEffect = eikthyrCharge.m_itemData.m_shared.m_triggerEffect;
        
        CreatureManager.Instance.AddCreature(stormBoar);
    }
}