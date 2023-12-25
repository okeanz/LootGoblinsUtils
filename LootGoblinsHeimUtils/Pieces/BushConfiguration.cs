using System;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;

namespace LootGoblinsUtils.Pieces;

public class BushConfiguration
{
    public string TargetItemName;
    public string LocalizedBushName;
    public string ReadyObjectName;
    public Sprite FertilizerIcon;
    public RequirementConfig[] BushRequirements;
    public GameObject NewBushModel;
    public FertilizerConfig[] FertilizerConfigs;


    private string BushName => $"piece_{TargetItemName}Bush_LG";
    private string FertilizerTierName(int tier) => $"{TargetItemName}FertilizerT{tier}_LG";

    public void Configure()
    {
        try
        {
            Jotunn.Logger.LogWarning($"{TargetItemName} constructing...");
            InnerSetup();
            Jotunn.Logger.LogWarning($"{TargetItemName} constructing completed");
        }
        catch (Exception e)
        {
            Jotunn.Logger.LogError($"{TargetItemName} constructing failed");
            Jotunn.Logger.LogError(e);
        }
    }
    
    private void InnerSetup()
    {
        var newPiece = MakeNewPiece();
        var modelParent = BushUtils.CleanupModel(newPiece);
        var newModel = BushUtils.SetupNewModel(NewBushModel, modelParent);
        var readyObject = newModel.FindDeepChild(ReadyObjectName).gameObject;
        readyObject.SetActive(false);

        BushUtils.ConfigureFermenter(newPiece.PiecePrefab.GetComponent<Fermenter>(), readyObject);


        PieceManager.Instance.AddPiece(newPiece);

        MakeFertilizers();
    }
    
    private CustomPiece MakeNewPiece()
    {
        var pc = new PieceConfig
        {
            Name = LocalizedBushName,
            PieceTable = PieceTables.Cultivator,
            Category = PieceCategories.Misc,
            Icon = FertilizerIcon,
            Requirements = BushRequirements
        };

        var newPiece = new CustomPiece(BushName, Fermenters.Fermenter, pc)
        {
            Piece =
            {
                m_craftingStation = null,
                m_cultivatedGroundOnly = true,
                m_groundOnly = true,
                m_noClipping = false
            }
        };
        return newPiece;
    }
    
    private void MakeFertilizers()
    {
        for (var i = 0; i < FertilizerConfigs.Length; i++)
        {
            var config = FertilizerConfigs[i];
            
            BushUtils.MakeFertilizer(
                localizedName: config.LocalizedName,
                itemName: FertilizerTierName(i+1),
                description: config.LocalizedDescription,
                requirements: config.RecipeRequirements
            );
            
            var conversion = new FermenterConversionConfig
            {
                ToItem = TargetItemName,
                FromItem = FertilizerTierName(i+1),
                Station = BushName,
                ProducedItems = config.AmountProduced
            };
            
            ItemManager.Instance.AddItemConversion(new CustomItemConversion(conversion));
        }
    }

    public struct FertilizerConfig
    {
        public string LocalizedName;
        public string LocalizedDescription;
        public RequirementConfig[] RecipeRequirements;
        public int AmountProduced;
    }
}