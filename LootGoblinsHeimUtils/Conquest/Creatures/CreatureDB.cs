using Jotunn.Managers;

namespace LootGoblinsUtils.Conquest.Creatures;

public static class CreatureDB
{
    public static void Init()
    {
        CreatureManager.OnVanillaCreaturesAvailable += Load;
    }

    private static void Load()
    {
        StormBoar.Load();
        StormHog.Load();
        NeckShaman.Load();
        NeckDragon.Load();
        CreatureManager.OnVanillaCreaturesAvailable -= Load;
    }
}