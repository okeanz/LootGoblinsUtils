using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Pieces;

public static class Mushroom
{
    public const string PieceName = "$piece_mushroom_LG";
    public static void Configure()
    {
        var mushroomItemPrefab = PrefabManager.Instance.GetPrefab("Mushroom");
        var icon = mushroomItemPrefab.GetComponent<ItemDrop>().m_itemData.GetIcon();
        
        var pc = new PieceConfig
        {
            Name = Localization.instance.Localize(PieceName),
            PieceTable = PieceTables.Cultivator,
            Category = PieceCategories.Misc,
            Icon = icon,
            Requirements = new []
            {
                new RequirementConfig("Mushroom", 1)
            }
        };

        var mushroomPrefab = PrefabManager.Instance.CreateClonedPrefab(PieceName, "Pickable_Carrot");
        mushroomPrefab.AddComponent<Piece>();


        var pickable = mushroomPrefab.GetComponent<Pickable>();
        Logger.LogWarning($"LOGSHIT {pickable.m_respawnTimeMinutes}");

        var newPiece = new CustomPiece(mushroomPrefab, false, pc);


        PieceManager.Instance.AddPiece(newPiece);
    }
}