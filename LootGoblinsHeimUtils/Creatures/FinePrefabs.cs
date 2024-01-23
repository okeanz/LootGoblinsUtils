using Jotunn.Managers;
using LootGoblinsUtils.Creatures.Attacks;
using UnityEngine;

namespace LootGoblinsUtils.Creatures;

public static class FinePrefabs
{
    public static GameObject FireExplosionProjectile => PrefabManager.Instance.GetPrefab("staff_fireball_projectile");

    public static GameObject LightningUnitEffectCloned =>
        PrefabManager.Instance.CreateClonedPrefab("LG_fx_lightning", "fx_Lightning").transform.Find("Sparcs").gameObject;

    public static AttackInfo EikthyrCharge => new("Eikthyr_charge");
    public static AttackInfo ShamanFireball => new("GoblinShaman_attack_fireball");
    public static AttackInfo GdkingShoot => new("gd_king_shoot");
}