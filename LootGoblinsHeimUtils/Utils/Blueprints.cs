using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Utils;

public static class Blueprints
{
    public static Blueprint RavenBP;

    public static void Init()
    {
        RavenBP = LoadBP("Relicv_Raven_V2.blueprint", "LGH_Raven_BP");
    }

    private static Blueprint LoadBP(string filename, string targetName)
    {
        var text = AssetUtils.LoadTextFromResources(filename);
        if (text == null) return null;
        var strings = text.GetLines(true).ToArray();
        var result = FromArray(targetName, strings, Blueprint.Format.Blueprint);
        Logger.LogInfo($"Loaded BP: ${filename}; value = {result.Name}; pieces = {result.PieceEntries.Length}");
        return result;
    }

    public static void Instantiate(this Blueprint bp, Transform parent)
    {
        if (bp == null)
        {
            Logger.LogError("Blueprints.Instantiate: bp is null");
            return;
        }
        parent.gameObject.SetActive(false);

        foreach (var bpPieceEntry in bp.PieceEntries)
        {
            if (bpPieceEntry?.name == null)
            {
                Logger.LogWarning("Skipping empty bpPieceEntry");
                continue;
            }

            var prefab = PrefabManager.Instance.GetPrefab(bpPieceEntry.name);
            if (prefab == null)
            {
                Logger.LogError($"Blueprint[{bp.Name}]: cant find prefab {bpPieceEntry.name} to instantiate");
                continue;
            }

            Object.Instantiate(prefab, bpPieceEntry.GetPosition(), bpPieceEntry.GetRotation(), parent);
        }
        Logger.LogInfo("Blueprints.Instantiate: done");
        parent.gameObject.SetActive(true);
    }

    public static Blueprint FromArray(string id, string[] lines, Blueprint.Format format)
    {
        var blueprint = new Blueprint
        {
            ID = id
        };
        var pieceEntryList = new List<PieceEntry>();
        var parserState = ParserState.Pieces;
        foreach (var line in lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                if (line.StartsWith("#Name:"))
                    blueprint.Name = line.Substring("#Name:".Length);
                else if (line.StartsWith("#Creator:"))
                    blueprint.Creator = line.Substring("#Creator:".Length);
                else if (line.StartsWith("#Description:"))
                {
                    blueprint.Description = line.Substring("#Description:".Length);
                    if (blueprint.Description.StartsWith("\""))
                        blueprint.Description = SimpleJson.SimpleJson.DeserializeObject<string>(blueprint.Description);
                }
                else if (line.StartsWith("#Category:"))
                {
                    blueprint.Category = line.Substring("#Category:".Length);
                    if (string.IsNullOrEmpty(blueprint.Category))
                        blueprint.Category = "Blueprints";
                }
                else
                    switch (line)
                    {
                        case "#SnapPoints":
                            parserState = ParserState.SnapPoints;
                            break;
                        case "#Terrain":
                            parserState = ParserState.Terrain;
                            break;
                        case "#Pieces":
                            parserState = ParserState.Pieces;
                            break;
                        default:
                            ParseLine(format, line, parserState, pieceEntryList);
                            break;
                    }
            }
        }

        if (string.IsNullOrEmpty(blueprint.Name))
            blueprint.Name = blueprint.ID;
        blueprint.PieceEntries = pieceEntryList.ToArray();
        return blueprint;
    }

    private static void ParseLine(Blueprint.Format format, string line, ParserState parserState,
        List<PieceEntry> pieceEntryList)
    {
        if (!line.StartsWith("#"))
        {
            switch (parserState)
            {
                case ParserState.SnapPoints:
                    return;
                case ParserState.Terrain:
                    return;
                case ParserState.Pieces:
                    switch (format)
                    {
                        case Blueprint.Format.VBuild:
                            pieceEntryList.Add(PieceEntry.FromVBuild(line));
                            return;
                        case Blueprint.Format.Blueprint:
                            pieceEntryList.Add(PieceEntry.FromBlueprint(line));
                            return;
                        default:
                            return;
                    }
                default:
                    return;
            }
        }
    }

    public enum ParserState
    {
        SnapPoints,
        Terrain,
        Pieces,
    }
}