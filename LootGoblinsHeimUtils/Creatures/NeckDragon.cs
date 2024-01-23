using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using LootGoblinsUtils.Creatures.Attacks;
using LootGoblinsUtils.Utils;
using UnityEngine;

namespace LootGoblinsUtils.Creatures;

public class NeckDragon
{
    public static void Load()
    {
        Loader.RunSafe(InnerLoad, "DragonNeck");
    }

    private static void InnerLoad()
    {
        var shamanNeckConfig = new CreatureConfig();
        var shamanNeck = new CustomCreature("LG_DragonNeck", "Neck", shamanNeckConfig);

        var shamanNeckPrefab = shamanNeck.Prefab;
        shamanNeckPrefab.transform.localScale *= 3;

        // shamanNeck.Prefab.AddLightningEffect(Vector3.one * 2);


        var boarHumanoid = shamanNeckPrefab.GetComponent<Humanoid>();
        boarHumanoid.m_name = "Никс-дракон";
        boarHumanoid.m_health = 250;

        var speedMult = 0.7f;
        boarHumanoid.m_speed *= speedMult;
        boarHumanoid.m_turnSpeed *= speedMult;
        boarHumanoid.m_runSpeed *= speedMult;
        boarHumanoid.m_runTurnSpeed *= speedMult;


        
        var attackItem = AttackInfo.FromCharacterAttack(shamanNeck.Prefab, 0, "neck_attack_2");
        attackItem.OverrideBy(FinePrefabs.ShamanFireball);
        
        attackItem.GetSharedData().m_aiAttackInterval = 12;
        attackItem.GetSharedData().m_attackForce = 300;
        attackItem.GetSharedData().m_damages.m_damage = 30f;
        attackItem.GetSharedData().m_damages.m_fire = 30f;
        
        attackItem.AddToCharactersAttacks(shamanNeck.Prefab);
        

        CreatureManager.Instance.AddCreature(shamanNeck);
    }
}