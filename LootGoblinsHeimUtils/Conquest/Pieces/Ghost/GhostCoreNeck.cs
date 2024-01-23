using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using LootGoblinsUtils.Conquest.Components;
using LootGoblinsUtils.Conquest.Components.Spawners.Boar;
using LootGoblinsUtils.Conquest.Components.Spawners.Neck;
using LootGoblinsUtils.Conquest.Pieces.Core;
using LootGoblinsUtils.Utils;

namespace LootGoblinsUtils.Conquest.Pieces.Ghost;

public static class GhostCoreNeck
{
    public static void Load()
    {
        Loader.RunSafe(InnerSetup, "GhostCoreNeck");
    }
    
    private static void InnerSetup()
    {
        var pc = new PieceConfig
        {
            Name = "GhostCoreNeck",
            PieceTable = "_BlueprintPieceTable",
            Category = "utils",
            CraftingStation = CraftingStations.None,
            Requirements = new[]
            {
                new RequirementConfig("LGH_GreenCore", 1, 0, true)
            }
        };

        var newPiece = new CustomPiece("GhostCoreNeck", "stone_wall_1x1", pc);
        
        newPiece
            .SetWntSupport(false)
            .SetPieceHealth(20000)
            .RemoveColliders()
            .RemoveRenderers();

        newPiece.PiecePrefab.AddComponent<ConquestGhost>();
        newPiece.PiecePrefab.AddComponent<SpawnerNeckRaid>().enabled = false;
        newPiece.PiecePrefab.AddComponent<SpawnerNeckT1>().enabled = false;
        
        PieceManager.Instance.AddPiece(newPiece);
    }
}