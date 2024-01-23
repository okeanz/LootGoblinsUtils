using System;
using System.Linq;
using Jotunn.Entities;
using LootGoblinsUtils.Conquest.Pieces;
using LootGoblinsUtils.Conquest.Pieces.Core;
using Object = UnityEngine.Object;

namespace LootGoblinsUtils.Conquest.TerminalCommands;

public class CoresList: ConsoleCommand
{
    public override string Name => "core_list";

    public override string Help => "List cores nearby";

    public override void Run(string[] args)
    {
        try
        {
            var conquestCores = Object.FindObjectsOfType<ConquestCore>()
                .ToDictionary(x=> x, x=> (x.transform.position - Player.m_localPlayer.transform.position).magnitude)
                .OrderByDescending(kvp => kvp.Value);

            foreach (var conquestCore in conquestCores)
            {
                Console.instance.Print($"Core[{conquestCore.Key.GetInstanceID()}]: {conquestCore.Value}");
            }
        }
        catch (Exception e)
        {
            Console.instance.Print(e.ToString());
            throw;
        }
        
    }
}