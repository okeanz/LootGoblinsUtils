using Jotunn.Managers;
using LootGoblinsUtils.Submods.Farming.Pieces;
using LootGoblinsUtils.Submods.Farming.Pieces.Configurators;
using LootGoblinsUtils.Submods.Farming.Pieces.Stations;

namespace LootGoblinsUtils.Submods.Farming;

public static class FarmingSetup
{
    public static void Init()
    {
        PrefabManager.OnVanillaPrefabsAvailable += Setup;
    }

    private static void Setup()
    {
        Jotunn.Logger.LogInfo("------------ LootGoblinsHeimUtilsPlugin start ------------");
        BushUtils.CacheDependencies();

        FarmersTable.Configure();
        FarmersTableExtensionT1.Configure();

        RaspberryBush.Setup();
        BlueberryBush.Setup();
        CloudberryBush.Setup();
        ThistleBush.Setup();

        Mushroom.Configure();
        MushroomYellow.Configure();
        Dandelion.Configure();

        Saplings.ReplaceRecipes();

        Jotunn.Logger.LogInfo("------------ LootGoblinsHeimUtilsPlugin end ------------");

        PrefabManager.OnVanillaPrefabsAvailable -= Setup;
    }
}