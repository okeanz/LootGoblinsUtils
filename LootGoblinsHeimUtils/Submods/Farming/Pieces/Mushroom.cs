using Jotunn.Configs;
using LootGoblinsUtils.Submods.Farming.Pieces.Configurators;

namespace LootGoblinsUtils.Submods.Farming.Pieces;

public static class Mushroom
{
    public const string PieceName = "$piece_mushroom_LG";

    public static void Configure()
    {
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("Russian", PieceName, "Гриб");
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("English", PieceName, "Mushroom");
        
        new PlantConfiguration
        {
            Name = PieceName,
            IconItemDropPrefabName = "Mushroom",
            PickablePrefabName = "Pickable_Mushroom",
            PieceRecipe = new[]
            {
                new RequirementConfig("Mushroom", 1),
                new RequirementConfig("Resin", 2)
            }

        }.Configure();
    }
}