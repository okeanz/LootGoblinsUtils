using HarmonyLib;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;

namespace LootGoblinsUtils.Creatures;

public static class CreatureDB
{
    public static void Init()
    {
        CreatureManager.OnVanillaCreaturesAvailable += Load;
    }

    private static void Load()
    {
        StormBoar.Load();
        StormHog.Load();
        NeckShaman.Load();
        NeckDragon.Load();
        CreatureManager.OnVanillaCreaturesAvailable -= Load;
    }
}