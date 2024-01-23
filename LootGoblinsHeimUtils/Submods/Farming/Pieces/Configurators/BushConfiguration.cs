using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Submods.Farming.Pieces.Configurators;

public class BushConfiguration : Configurable
{
    public string LocalizedBushName;
    public string ReadyObjectName;
    public float ColliderRadius;
    public Sprite FertilizerIcon;
    public RequirementConfig[] BushRequirements;
    public GameObject NewBushModel;
    public FertilizerConfig[] FertilizerConfigs;


    private string BushName => $"piece_{Name}Bush_LG";
    private string FertilizerTierName(int tier) => $"{Name}FertilizerT{tier}_LG";


    protected override void Setup()
    {
        var newPiece = MakeNewPiece();
        var modelParent = BushUtils.CleanupModel(newPiece);
        Logger.LogInfo($"{BushName} CleanupModel complete");
        var newModel = BushUtils.SetupNewModel(NewBushModel, modelParent);
        Logger.LogInfo($"{BushName} SetupNewModel complete");

        var collider = newModel.GetComponent<CapsuleCollider>() ??
                       newModel.AddComponent<CapsuleCollider>();
        collider.radius = ColliderRadius == 0 ? 1.6f : ColliderRadius;
        collider.isTrigger = true;

        var readyObject = newModel.FindDeepChild(ReadyObjectName)?.gameObject;
        if (readyObject == null)
        {
            Logger.LogWarning($"{BushName}.readyObject == null");
        }
        else
        {
            readyObject.SetActive(false);
        }

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
        Logger.LogInfo($"{BushName} newPiece setup complete");
        
        return newPiece;
    }

    private void MakeFertilizers()
    {
        for (var i = 0; i < FertilizerConfigs.Length; i++)
        {
            var config = FertilizerConfigs[i];

            BushUtils.MakeFertilizer(
                localizedName: config.LocalizedName,
                itemName: FertilizerTierName(i + 1),
                description: config.LocalizedDescription,
                requirements: config.RecipeRequirements,
                minStationLevel: i+1
            );

            var conversion = new FermenterConversionConfig
            {
                ToItem = Name,
                FromItem = FertilizerTierName(i + 1),
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