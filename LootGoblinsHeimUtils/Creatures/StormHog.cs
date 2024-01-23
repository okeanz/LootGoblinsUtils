using HarmonyLib;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using LootGoblinsUtils.Creatures.Attacks;
using LootGoblinsUtils.Utils;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Creatures;

public static class StormHog
{
    public static void Load()
    {
        Loader.RunSafe(InnerLoad, "StormHog");
    }

    private static void InnerLoad()
    {
        var stormBoarConfig = new CreatureConfig();
        var stormHog = new CustomCreature("LG_StormHog", "Boar", stormBoarConfig);

        var stormBoarPrefab = stormHog.Prefab;
        stormBoarPrefab.transform.localScale *= 3;

        stormHog.Prefab.AddLightningEffect(Vector3.one * 3);


        var boarHumanoid = stormBoarPrefab.GetComponent<Humanoid>();
        boarHumanoid.m_name = "Штормовой Боров";
        boarHumanoid.m_health = 220;

        var speedMult = 0.9f;
        boarHumanoid.m_speed *= speedMult;
        boarHumanoid.m_turnSpeed *= speedMult;
        boarHumanoid.m_runSpeed *= speedMult;
        boarHumanoid.m_runTurnSpeed *= speedMult;


        
        var attackItem = AttackInfo.FromCharacterAttack(stormHog.Prefab, 0, "boar_attack_2");
        attackItem.OverrideBy(FinePrefabs.EikthyrCharge);
        
        attackItem.GetSharedData().m_aiAttackInterval = 8;
        attackItem.GetSharedData().m_attackForce = 200;
        attackItem.GetSharedData().m_damages.m_blunt = 30f;
        attackItem.GetSharedData().m_damages.m_lightning = 30f;
        
        attackItem.AddToCharactersAttacks(stormHog.Prefab);
        

        CreatureManager.Instance.AddCreature(stormHog);
    }
}