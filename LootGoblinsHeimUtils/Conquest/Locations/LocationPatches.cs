using Jotunn.Managers;

namespace LootGoblinsUtils.Conquest.Locations;

public static class LocationPatches
{
    public static void Patch()
    {
        ZoneManager.OnVanillaLocationsAvailable += VanillaLocations;
        
    }

    private static void VanillaLocations()
    {
        PatchEikthyrnir();
        ZoneManager.OnVanillaLocationsAvailable -= VanillaLocations;
    }

    private static void PatchEikthyrnir()
    {
        var location = ZoneManager.Instance.GetZoneLocation("Eikthyrnir");
        location.m_minDistance = 900f;
    }
}