using Jotunn.Configs;
using LootGoblinsUtils.Submods.Farming.Pieces.Configurators;

namespace LootGoblinsUtils.Submods.Farming.Pieces;

public static class MushroomYellow
{
    public const string PieceName = "$piece_mushroom_yellow_LG";

    public static void Configure()
    {
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("Russian", PieceName, "Желтый гриб");
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("English", PieceName, "Yellow Mushroom");
        
        new PlantConfiguration
        {
            Name = PieceName,
            IconItemDropPrefabName = "MushroomYellow",
            PickablePrefabName = "Pickable_Mushroom_yellow",
            PieceRecipe = new[]
            {
                new RequirementConfig("MushroomYellow", 1),
                new RequirementConfig("BoneFragments", 1)
            }

        }.Configure();
    }
}