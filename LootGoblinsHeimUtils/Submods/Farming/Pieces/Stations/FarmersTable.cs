using System;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using Logger = Jotunn.Logger;
using Object = UnityEngine.Object;

namespace LootGoblinsUtils.Submods.Farming.Pieces.Stations;

public static class FarmersTable
{
    public const string TableName = "$piece_farmersTable_LG";
    public const string PieceCategory = "$piececategory_farming";

    public static void Configure()
    {
        try
        {
            InnerSetup();
            Logger.LogInfo($"{TableName} constructing completed");
        }
        catch (Exception e)
        {
            Logger.LogError($"{TableName} constructing failed");
            Logger.LogError(e);
        }
    }

    private static void InnerSetup()
    {
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("Russian", TableName, "Столик фермера");
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("English", TableName, "Farmers Table");
        
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("Russian", PieceCategory, "Фермерство");
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("English", PieceCategory, "Farming");
        
        var pestlePrefab = PrefabManager.Instance.GetPrefab("cauldron_ext5_mortarandpestle");
        var pestleModel = pestlePrefab.FindDeepChild("new").gameObject;
        var farmersTableIcon = pestlePrefab.GetComponent<Piece>().m_icon;

        PieceManager.Instance.AddPieceCategory(PieceTables.Hammer, PieceCategory);


        var pc = new PieceConfig
        {
            Name = Localization.instance.Localize(TableName),
            PieceTable = PieceTables.Hammer,
            Category = Localization.instance.Localize(PieceCategory),
            Icon = farmersTableIcon,
            Requirements = new[]
            {
                new RequirementConfig("FineWood", 15, 0, true),
                new RequirementConfig("Bronze", 2, 0, true)
            }
        };

        var newPiece = new CustomPiece(TableName, CraftingStations.Workbench, pc);
        var newModel = Object.Instantiate(pestleModel, newPiece.PiecePrefab.transform);
        newModel.transform.localScale = new Vector3(1.8f, 1.8f, 1.3f);
        var wearNTear = newPiece.PiecePrefab.GetComponent<WearNTear>();
        wearNTear.m_broken = newModel;
        wearNTear.m_new = newModel;
        wearNTear.m_worn = newModel;

        Object.Destroy(newPiece.PiecePrefab.FindDeepChild("New").gameObject);
        Object.Destroy(newPiece.PiecePrefab.FindDeepChild("Worn").gameObject);
        Object.Destroy(newPiece.PiecePrefab.FindDeepChild("Broken").gameObject);

        var craftingStation = newPiece.PiecePrefab.GetComponent<CraftingStation>();
        craftingStation.m_name = TableName;
        craftingStation.m_showBasicRecipies = false;
        
        PieceManager.Instance.AddPiece(newPiece);
    }
}