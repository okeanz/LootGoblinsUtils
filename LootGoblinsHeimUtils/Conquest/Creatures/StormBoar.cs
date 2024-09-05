using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using LootGoblinsUtils.Conquest.Creatures.Attacks;
using LootGoblinsUtils.Utils;
using UnityEngine;

namespace LootGoblinsUtils.Conquest.Creatures;

public static class StormBoar
{
    public static void Load()
    {
        Loader.RunSafe(InnerLoad, "StormBoar");
    }

    private static void InnerLoad()
    {
        var stormBoarConfig = new CreatureConfig();
        var stormBoar = new CustomCreature("LG_StormBoar", "Boar", stormBoarConfig);

        var stormBoarPrefab = stormBoar.Prefab;
        stormBoarPrefab.transform.localScale *= 2;

        stormBoar.Prefab.AddLightningEffect(Vector3.one);


        var boarHumanoid = stormBoarPrefab.GetComponent<Humanoid>();
        boarHumanoid.m_name = "Штормовой Кабан";
        boarHumanoid.m_health = 120;

        var speedMult = 1.3f;
        boarHumanoid.m_speed *= speedMult;
        boarHumanoid.m_turnSpeed *= speedMult;
        boarHumanoid.m_runSpeed *= speedMult;
        boarHumanoid.m_runTurnSpeed *= speedMult;


        
        var attackItem = AttackInfo.FromCharacterAttack(stormBoar.Prefab, 0, "boar_attack_2");
        attackItem.OverrideBy(FinePrefabs.EikthyrCharge);
        
        attackItem.GetSharedData().m_aiAttackInterval = 5;
        attackItem.GetSharedData().m_attackForce = 100;
        attackItem.GetSharedData().m_damages.m_lightning = 20;
        attackItem.GetSharedData().m_damages.m_blunt = 10;
        
        attackItem.AddToCharactersAttacks(stormBoar.Prefab);

        CreatureManager.Instance.AddCreature(stormBoar);
    }
}