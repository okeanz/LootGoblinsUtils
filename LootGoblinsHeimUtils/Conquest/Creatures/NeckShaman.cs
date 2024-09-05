using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using LootGoblinsUtils.Conquest.Creatures.Attacks;
using LootGoblinsUtils.Utils;

namespace LootGoblinsUtils.Conquest.Creatures;

public class NeckShaman
{
    public static void Load()
    {
        Loader.RunSafe(InnerLoad, "ShamanNeck");
    }

    private static void InnerLoad()
    {
        var shamanNeckConfig = new CreatureConfig();
        var shamanNeck = new CustomCreature("LG_ShamanNeck", "Neck", shamanNeckConfig);

        var shamanNeckPrefab = shamanNeck.Prefab;
        shamanNeckPrefab.transform.localScale *= 2;

        // shamanNeck.Prefab.AddLightningEffect(Vector3.one * 2);


        var boarHumanoid = shamanNeckPrefab.GetComponent<Humanoid>();
        boarHumanoid.m_name = "Никс-шаман";
        boarHumanoid.m_health = 90;

        var speedMult = 0.8f;
        boarHumanoid.m_speed *= speedMult;
        boarHumanoid.m_turnSpeed *= speedMult;
        boarHumanoid.m_runSpeed *= speedMult;
        boarHumanoid.m_runTurnSpeed *= speedMult;


        
        var attackItem = AttackInfo.FromCharacterAttack(shamanNeck.Prefab, 0, "neck_attack_2");
        attackItem.OverrideBy(FinePrefabs.ShamanFireball);
        
        attackItem.GetSharedData().m_aiAttackInterval = 8;
        attackItem.GetSharedData().m_attackForce = 100;
        attackItem.GetSharedData().m_damages.m_damage = 10f;
        attackItem.GetSharedData().m_damages.m_fire = 20f;
        
        attackItem.AddToCharactersAttacks(shamanNeck.Prefab);
        

        CreatureManager.Instance.AddCreature(shamanNeck);
    }
}