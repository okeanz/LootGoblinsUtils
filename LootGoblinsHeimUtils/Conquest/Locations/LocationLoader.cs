using System.Linq;
using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using LootGoblinsUtils.Configuration;
using LootGoblinsUtils.Conquest.Locations.T1CoresLoaders;
using LootGoblinsUtils.Utils;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Conquest.Locations;

public static class LocationLoader
{
    public static string[] PossibleT1Cores = new[]
    {
        "T1_Core_Boars",
        "T1_Core_Necks"
    };

    public static ZoneSystem.ZoneLocation GetRandomT1CoreLocation()
    {
        return ZoneManager.Instance
            .GetCustomLocation(PossibleT1Cores[Random.Range(0, PossibleT1Cores.Length)])
            ?.ZoneLocation;
    }

    public static void Init()
    {
        LocationPatches.Patch();
        ZoneManager.OnVanillaLocationsAvailable += Load;
    }

    private static void Load()
    {
        T1CoreBoars.Load();
        T1CoreNecks.Load();


        ZoneManager.OnVanillaLocationsAvailable -= Load;
    }
}