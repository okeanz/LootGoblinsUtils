using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using LootGoblinsUtils.Conquest.Components;
using LootGoblinsUtils.Conquest.Components.Spawners.Boar;
using LootGoblinsUtils.Conquest.Pieces.Core;
using LootGoblinsUtils.Utils;

namespace LootGoblinsUtils.Conquest.Pieces.Ghost;

public static class GhostCoreBoar
{
    public static void Load()
    {
        Loader.RunSafe(InnerSetup, "GhostCoreBoar");
    }
    
    private static void InnerSetup()
    {
        var pc = new PieceConfig
        {
            Name = "GhostCoreBoar",
            PieceTable = "_BlueprintPieceTable",
            Category = "utils",
            CraftingStation = CraftingStations.None,
            Requirements = new[]
            {
                new RequirementConfig("LGH_GreenCore", 1, 0, true)
            }
        };

        var newPiece = new CustomPiece("GhostCoreBoar", "stone_wall_1x1", pc);
        
        newPiece
            .SetWntSupport(false)
            .SetPieceHealth(20000)
            .RemoveColliders()
            .RemoveRenderers();

        newPiece.PiecePrefab.AddComponent<ConquestGhost>();
        newPiece.PiecePrefab.AddComponent<SpawnerBoarsRaid>().enabled = false;
        newPiece.PiecePrefab.AddComponent<SpawnerBoarsT1>().enabled = false;
        
        PieceManager.Instance.AddPiece(newPiece);
    }
}