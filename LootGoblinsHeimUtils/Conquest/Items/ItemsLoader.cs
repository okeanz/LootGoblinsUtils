using Jotunn.Managers;

namespace LootGoblinsUtils.Conquest.Items;

public static class ItemsLoader
{
    public static void Init()
    {
        PrefabManager.OnVanillaPrefabsAvailable += Load;
    }

    private static void Load()
    {
        GreenCore.Load();
        PrefabManager.OnVanillaPrefabsAvailable -= Load;
    }
}