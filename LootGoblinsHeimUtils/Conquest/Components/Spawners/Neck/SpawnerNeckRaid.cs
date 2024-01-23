using Jotunn.Managers;

namespace LootGoblinsUtils.Conquest.Components.Spawners.Neck;

public class SpawnerNeckRaid: DefendersSpawner
{
    public override SpawnerTypes SpawnerType => SpawnerTypes.Raid;
    private void Awake()
    {
        officerPrefabName = "LG_DragonNeck";
        guardPrefabName = "LG_ShamanNeck";
        
        officerPrefab = PrefabManager.Instance.GetPrefab(officerPrefabName);
        guardPrefab = PrefabManager.Instance.GetPrefab(guardPrefabName);

        OfficerCountOverride = 1;
        GuardCountOverride = 2;
    }
}