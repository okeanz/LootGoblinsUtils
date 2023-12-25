using Jotunn;
using Jotunn.Configs;
using LootGoblinsUtils.Pieces.Configurators;

namespace LootGoblinsUtils.Pieces;

public static class Saplings
{
    public static void ReplaceRecipes()
    {
        PlantRecipeSwapper.SwapPieceRecipe("sapling_carrot", new []
        {
            new RequirementConfig("CarrotSeeds", 1),
            new RequirementConfig("BoneFragments", 1)
        });
        PlantRecipeSwapper.SwapPieceRecipe("sapling_seedcarrot", new []
        {
            new RequirementConfig("Carrot", 1),
            new RequirementConfig("BoneFragments", 1)
        });
        
        PlantRecipeSwapper.SwapPieceRecipe("sapling_turnip", new []
        {
            new RequirementConfig("TurnipSeeds", 1),
            new RequirementConfig("BoneFragments", 1)
        });
        PlantRecipeSwapper.SwapPieceRecipe("sapling_seedturnip", new []
        {
            new RequirementConfig("Turnip", 1),
            new RequirementConfig("BoneFragments", 1)
        });
        
        PlantRecipeSwapper.SwapPieceRecipe("sapling_onion", new []
        {
            new RequirementConfig("OnionSeeds", 1),
            new RequirementConfig("Entrails", 1)
        });
        PlantRecipeSwapper.SwapPieceRecipe("sapling_seedonion", new []
        {
            new RequirementConfig("Onion", 1),
            new RequirementConfig("Entrails", 1)
        });
        
        PlantRecipeSwapper.SwapPieceRecipe("sapling_barley", new []
        {
            new RequirementConfig("Barley", 1),
            new RequirementConfig("Ooze", 1)
        });
        
        PlantRecipeSwapper.SwapPieceRecipe("sapling_flax", new []
        {
            new RequirementConfig("Flax", 1),
            new RequirementConfig("Ooze", 1)
        });
        
        PlantRecipeSwapper.SwapPieceRecipe("Beech_Sapling", new []
        {
            new RequirementConfig("BeechSeeds", 1),
            new RequirementConfig("LeatherScraps", 1)
        });
        
        PlantRecipeSwapper.SwapPieceRecipe("Birch_Sapling", new []
        {
            new RequirementConfig("BirchSeeds", 1),
            new RequirementConfig("LeatherScraps", 1)
        });
        
        PlantRecipeSwapper.SwapPieceRecipe("Oak_Sapling", new []
        {
            new RequirementConfig("Acorn", 1),
            new RequirementConfig("LeatherScraps", 1)
        }); 
        
        PlantRecipeSwapper.SwapPieceRecipe("FirTree_Sapling", new []
        {
            new RequirementConfig("FirCone", 1),
            new RequirementConfig("LeatherScraps", 1)
        });
        
        PlantRecipeSwapper.SwapPieceRecipe("PineTree_Sapling", new []
        {
            new RequirementConfig("PineCone", 1),
            new RequirementConfig("LeatherScraps", 1)
        });
        
        Logger.LogInfo("Saplings recipes replaced");
    }
}