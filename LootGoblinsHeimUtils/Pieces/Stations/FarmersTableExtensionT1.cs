using System;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using Logger = Jotunn.Logger;
using Object = System.Object;

namespace LootGoblinsUtils.Pieces.Stations;

public static class FarmersTableExtensionT1
{
    public const string TableName = "$piece_farmersTable_ext1_LG";
    public const string TableDescription = "$piece_farmersTable_ext1_description_LG";
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
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("Russian", TableName, "Фермерская сушилка для трав");
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("English", TableName, "Farmer's Spice Rack");
        
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("Russian", TableDescription, "Улучшение столика фермера");
        LootGoblinsHeimUtilsPlugin.Localization.AddTranslation("English", TableDescription, "Farmer's Table improvement");

        
        var pc = new PieceConfig
        {
            Name = Localization.instance.Localize(TableName),
            Description = Localization.instance.Localize(TableDescription),
            PieceTable = PieceTables.Hammer,
            Category = Localization.instance.Localize(PieceCategory),
            Requirements = new[]
            {
                new RequirementConfig("Dandelion", 10, 0, true),
                new RequirementConfig("Carrot", 5, 0, true),
                new RequirementConfig("Mushroom", 5, 0, true),
                new RequirementConfig("Thistle", 5, 0, true),
                new RequirementConfig("Turnip", 3, 0, true),
            }
        };

        var newPiece = new CustomPiece(TableName, "cauldron_ext1_spice", pc);

        var stationExtension = newPiece.PiecePrefab.GetComponent<StationExtension>();
        stationExtension.m_craftingStation = PieceManager.Instance.GetPiece(FarmersTable.TableName).PiecePrefab
            .GetComponent<CraftingStation>();
        
        PieceManager.Instance.AddPiece(newPiece);
    }
}