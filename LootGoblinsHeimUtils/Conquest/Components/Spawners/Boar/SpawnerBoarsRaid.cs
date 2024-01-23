using Jotunn.Managers;

namespace LootGoblinsUtils.Conquest.Components.Spawners.Boar;

public class SpawnerBoarsRaid: DefendersSpawner
{
    public override SpawnerTypes SpawnerType => SpawnerTypes.Raid;

    private void Awake()
    {
        officerPrefabName = "LG_StormHog";
        guardPrefabName = "LG_StormBoar";
        
        officerPrefab = PrefabManager.Instance.GetPrefab(officerPrefabName);
        guardPrefab = PrefabManager.Instance.GetPrefab(guardPrefabName);

        OfficerCountOverride = 1;
        GuardCountOverride = 2;
    }
}