using Jotunn.Managers;

namespace LootGoblinsUtils.Conquest.Components.Spawners.Boar;

public class SpawnerBoarsT1: DefendersSpawner
{
    public override SpawnerTypes SpawnerType => SpawnerTypes.Common;
    private void Awake()
    {
        officerPrefabName = "LG_StormBoar";
        guardPrefabName = "Boar";
        
        officerPrefab = PrefabManager.Instance.GetPrefab(officerPrefabName);
        guardPrefab = PrefabManager.Instance.GetPrefab(guardPrefabName);
    }
}