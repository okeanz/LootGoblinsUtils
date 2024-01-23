using System.Linq;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Managers;

namespace LootGoblinsUtils.Submods.Farming.Pieces.Configurators;

public static class PlantRecipeSwapper
{
    public static void SwapPieceRecipe(string prefabName, RequirementConfig[] requirementConfigs)
    {
        var prefab = PrefabManager.Instance.GetPrefab(prefabName);
        var piece = prefab.GetComponent<Piece>();
        var requirements = requirementConfigs.Select(req => req.GetRequirement()).ToArray();
        foreach (var requirement in requirements)
        {
            requirement.FixReferences();
        }
        piece.m_resources = requirements.ToArray();
    }
}