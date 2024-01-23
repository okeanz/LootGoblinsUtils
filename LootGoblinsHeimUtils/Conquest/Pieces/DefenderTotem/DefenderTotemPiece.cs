using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using LootGoblinsUtils.Conquest.Pieces.Ghost;
using LootGoblinsUtils.Utils;
using UnityEngine;

namespace LootGoblinsUtils.Conquest.Pieces.DefenderTotem;

public static class DefenderTotemPiece
{
    public static void Load()
    {
        Loader.RunSafe(InnerLoad, "DefenderTotemPiece");
    }

    private static void InnerLoad()
    {
        var guardStone = PrefabManager.Instance.CreateClonedPrefab("guard_stone_tmp", "guard_stone");
        var guardStoneModel = guardStone.transform.Find("new").GetChild(0).gameObject;
        var icon = guardStone.GetComponent<Piece>().m_icon;

        guardStoneModel.AddLightningEffect(Vector3.one * 2);

        var pc = new PieceConfig
        {
            Name = "Защитный Тотем",
            PieceTable = PieceTables.Hammer,
            Category = PieceCategories.Misc,
            CraftingStation = CraftingStations.Workbench,
            Icon = icon,
            Requirements = new[]
            {
                new RequirementConfig("Wood", 20, 0, true),
                new RequirementConfig("TrophyBoar", 5, 0, false),
                new RequirementConfig("TrophyNeck", 5, 0, false),
                new RequirementConfig("TrophyDeer", 5, 0, false),
                new RequirementConfig("Resin", 15, 0, true),
            }
        };

        var newPiece = new CustomPiece("DefenderTotem", "stone_pillar", pc);

        newPiece
            .ReplaceWntModel(guardStoneModel, Vector3.one * 2, new Vector3(0, -1, 0))
            .SetPieceHealth(350);

        newPiece.PiecePrefab.AddComponent<DefenderTotemComponent>();

        PieceManager.Instance.AddPiece(newPiece);
    }
}