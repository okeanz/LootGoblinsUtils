using Jotunn.Managers;

namespace LootGoblinsUtils.Conquest.Components.Spawners.Neck;

public class SpawnerNeckT1: DefendersSpawner
{
    public override SpawnerTypes SpawnerType => SpawnerTypes.Common;
    private void Awake()
    {
        officerPrefabName = "LG_ShamanNeck";
        guardPrefabName = "Neck";
        
        officerPrefab = PrefabManager.Instance.GetPrefab(officerPrefabName);
        guardPrefab = PrefabManager.Instance.GetPrefab(guardPrefabName);
    }
}