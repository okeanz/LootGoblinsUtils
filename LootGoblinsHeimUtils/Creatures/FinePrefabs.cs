using Jotunn.Managers;
using UnityEngine;

namespace LootGoblinsUtils.Creatures;

public static class FinePrefabs
{
    public static GameObject FireExplosionProjectile => PrefabManager.Instance.GetPrefab("staff_fireball_projectile");
    public static GameObject LightningUnitEffect => PrefabManager.Instance.GetPrefab("fx_Lightning");

    public static GameObject LightningUnitEffectCloned =>
        PrefabManager.Instance.CreateClonedPrefab("LG_fx_lightning", "fx_Lightning");
}