using Jotunn.Configs;
using UnityEngine;

namespace LootGoblinsUtils.Utils;

public class Blueprint
{
    public string ID;
    public string Name;
    public string Creator;
    public string Description = string.Empty;
    public string Category = "Blueprints";
    public PieceEntry[] PieceEntries;
    public GameObject Prefab;
    public KeyHintConfig KeyHint;
    public Bounds Bounds;
    public float GhostActiveTime;
    
    public enum Format
    {
        VBuild,
        Blueprint,
    }
}

public class SnapPointEntry
{
    public string line;
    public float posX;
    public float posY;
    public float posZ;
}

public class TerrainModEntry
  {
    public string line;
    public string shape;
    public float posX;
    public float posY;
    public float posZ;
    public float radius;
    public int rotation;
    public float smooth;
    public string paint;
  }