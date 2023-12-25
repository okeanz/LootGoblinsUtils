using Jotunn.Configs;
using LootGoblinsUtils.Pieces.Configurators;

namespace LootGoblinsUtils.Pieces;

public static class Dandelion
{
    public const string PieceName = "$piece_dandelion_LG";

    public static void Configure()
    {
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("Russian", PieceName, "Одуванчик");
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("English", PieceName, "Dandelion");
        
        new PlantConfiguration
        {
            Name = PieceName,
            IconItemDropPrefabName = "Dandelion",
            PickablePrefabName = "Pickable_Dandelion",
            PieceRecipe = new[]
            {
                new RequirementConfig("Dandelion", 1),
                new RequirementConfig("Resin", 1)
            }

        }.Configure();
    }
}