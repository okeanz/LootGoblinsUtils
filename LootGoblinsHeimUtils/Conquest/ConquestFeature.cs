using Jotunn.Configs;
using Jotunn.Managers;
using LootGoblinsUtils.Conquest.Items;
using LootGoblinsUtils.Conquest.Locations;
using LootGoblinsUtils.Conquest.Pieces;
using LootGoblinsUtils.Conquest.TerminalCommands;
namespace LootGoblinsUtils.Conquest;

public static class ConquestFeature
{
    public static void Load()
    {
        ItemsLoader.Init();
        PiecesLoader.Init();
        LocationLoader.Init();

        CommandManager.Instance.AddConsoleCommand(new CoresList());
    }
}