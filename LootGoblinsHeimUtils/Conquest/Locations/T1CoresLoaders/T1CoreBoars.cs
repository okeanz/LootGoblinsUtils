using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using LootGoblinsUtils.Utils;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Conquest.Locations.T1CoresLoaders;

public static class T1CoreBoars
{
    public static void Load()
    {
        var coreLocationContainer = ZoneManager.Instance.CreateLocationContainer("T1_Core_Boars");
        Blueprints.RavenBP.Instantiate(coreLocationContainer.transform);

        var ghostCore = Object.Instantiate(PieceManager.Instance.GetPiece("GhostCoreBoar").PiecePrefab,
            coreLocationContainer.transform);
        ghostCore.transform.localPosition = Vector3.zero;

        var config = new LocationConfig
        {
            Biome = Heightmap.Biome.Meadows,
            Quantity = 100,
            Priotized = true,
            ExteriorRadius = 20,
            MinDistance = 200f,
            MinDistanceFromSimilar = 100f,
            Group = "T1_Cores",
            ClearArea = true
        };


        var location = new CustomLocation(coreLocationContainer, false, config);
        ZoneManager.Instance.AddCustomLocation(location);
        location.Location.m_noBuild = false;
        location.Location.m_noBuildRadiusOverride = 0f;
        Logger.LogInfo($"Location T1_Core_Boars Loaded");
    }
}