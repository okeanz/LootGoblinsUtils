using System;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using LootGoblinsUtils.Conquest.Components;
using LootGoblinsUtils.Utils;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Conquest.Pieces.Core;

public static class CorePiece
{
    public static void Load()
    {
        Loader.RunSafe(InnerSetup, "CorePiece");
    }

    private static void InnerSetup()
    {
        var surtlingCore = PrefabManager.Instance.GetPrefab("SurtlingCore");
        var surtlingCoreModel = surtlingCore.transform.GetChild(0).gameObject;

        var pc = new PieceConfig
        {
            Name = "ConquestCore",
            PieceTable = "_BlueprintPieceTable",
            Category = "utils",
            CraftingStation = null,
            Icon = RenderManager.Instance.Render(surtlingCore),
            Requirements = new[]
            {
                new RequirementConfig("LGH_GreenCore", 1, 0, true)
            }
        };

        var newPiece = new CustomPiece("ConquestCore", "stone_wall_1x1", pc);
        
        newPiece
            .ReplaceWntModel(surtlingCoreModel, new Vector3(5, 5, 5))
            .SetWntSupport(false)
            .SetPieceHealth(200);

        newPiece.PiecePrefab.AddComponent<ConquestCore>();

        PieceManager.Instance.AddPiece(newPiece);
    }
}