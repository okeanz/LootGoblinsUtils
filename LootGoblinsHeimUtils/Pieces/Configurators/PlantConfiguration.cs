using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;

namespace LootGoblinsUtils.Pieces.Configurators;

public class PlantConfiguration: Configurable
{
    public string IconItemDropPrefabName;
    public RequirementConfig[] PieceRecipe;
    public string PickablePrefabName;
    
    protected override void Setup()
    {
        var iconPrefab = PrefabManager.Instance.GetPrefab(IconItemDropPrefabName);
        var icon = iconPrefab.GetComponent<ItemDrop>().m_itemData.GetIcon();

        
        var pc = new PieceConfig
        {
            Name = Localization.instance.Localize(Name),
            PieceTable = PieceTables.Cultivator,
            Category = PieceCategories.Misc,
            Icon = icon,
            Requirements = PieceRecipe
        };

        var newPiece = new CustomPiece(Name, "sapling_onion", pc);

        var pickablePrefab = PrefabManager.Instance.CreateClonedPrefab($"{PickablePrefabName}_LG", PickablePrefabName);
        var pickableModel = pickablePrefab.transform.GetChild(0);
        pickablePrefab.GetComponent<Pickable>().m_amount = 5;
        
        var plant = newPiece.PiecePrefab.GetComponent<Plant>();
        plant.m_minScale = 1.2f;
        plant.m_maxScale = 1.7f;
        plant.m_growTime = PluginConfiguration.PlantGrowMinDuration.Value;
        plant.m_growTimeMax = PluginConfiguration.PlantGrowMaxDuration.Value;
        plant.m_name = Localization.instance.Localize(Name);
        plant.m_grownPrefabs = new[]
        {
            pickablePrefab
        };
        
        foreach (Transform children in newPiece.PiecePrefab.transform)
        {
            Object.Destroy(children.gameObject);
        }

        var healthy = Object.Instantiate(pickableModel, newPiece.PiecePrefab.transform).gameObject;
        healthy.name = "healthy";
        healthy.transform.localScale = Vector3.one * 0.5f;
        
        var unhealthy = Object.Instantiate(pickableModel, newPiece.PiecePrefab.transform).gameObject;
        unhealthy.name = "unhealthy";
        unhealthy.transform.localScale = Vector3.one * 0.5f;
        
        var healthyGrown = Object.Instantiate(pickableModel, newPiece.PiecePrefab.transform).gameObject;
        healthyGrown.name = "healthyGrown";
        
        var unhealthyGrown = Object.Instantiate(pickableModel, newPiece.PiecePrefab.transform).gameObject;
        unhealthyGrown.name = "unhealthyGrown";

        plant.m_healthy = healthy;
        plant.m_unhealthy = unhealthy;
        plant.m_healthyGrown = healthyGrown;
        plant.m_unhealthyGrown = unhealthyGrown;
        
        
        
        
        PieceManager.Instance.AddPiece(newPiece);
    }
}