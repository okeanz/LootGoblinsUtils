using Jotunn.Managers;
using LootGoblinsUtils.Conquest.Pieces.Core;
using LootGoblinsUtils.Conquest.Pieces.DefenderTotem;
using LootGoblinsUtils.Conquest.Pieces.Ghost;

namespace LootGoblinsUtils.Conquest.Pieces;

public static class PiecesLoader
{
    public static void Init()
    {
        PrefabManager.OnVanillaPrefabsAvailable += LoadAll;
    }

    private static void LoadAll()
    {
        CorePiece.Load();
        GhostCoreBoar.Load();
        GhostCoreNeck.Load();
        DefenderTotemPiece.Load();
        
        PrefabManager.OnVanillaPrefabsAvailable -= LoadAll;
    }
}