using Jotunn.Entities;
using UnityEngine;

namespace LootGoblinsUtils.Utils;

public static class CustomPieceExtensions
{
    public static CustomPiece ReplaceWntModel(this CustomPiece piece, GameObject targetModel, Vector3 targetScale)
    {
        return ReplaceWntModel(piece, targetModel, targetScale, Vector3.zero);
    }

    public static CustomPiece ReplaceWntModel(
        this CustomPiece piece, 
        GameObject targetModel, 
        Vector3 targetScale,
        Vector3 offset)
    {
        var newModel = Object.Instantiate(targetModel, piece.PiecePrefab.transform);
        newModel.transform.localPosition += offset;
        newModel.transform.localScale = targetScale;
        var wearNTear = piece.PiecePrefab.GetComponent<WearNTear>();
        if (wearNTear.m_new)
            Object.Destroy(wearNTear.m_new);
        wearNTear.m_new = newModel;

        if (wearNTear.m_broken)
            Object.Destroy(wearNTear.m_broken);
        wearNTear.m_broken = newModel;

        if (wearNTear.m_worn)
            Object.Destroy(wearNTear.m_worn);
        wearNTear.m_worn = newModel;

        return piece;
    }

    public static CustomPiece RemoveExistingWntModels(this CustomPiece piece)
    {
        piece.PiecePrefab.DestroyIfExists("new");
        piece.PiecePrefab.DestroyIfExists("worn");
        piece.PiecePrefab.DestroyIfExists("broken");

        return piece;
    }

    public static CustomPiece SetWntSupport(this CustomPiece piece, bool support)
    {
        var wearNTear = piece.PiecePrefab.GetComponent<WearNTear>();
        wearNTear.m_noSupportWear = support;

        return piece;
    }

    public static CustomPiece SetPieceHealth(this CustomPiece piece, float health)
    {
        var wearNTear = piece.PiecePrefab.GetComponent<WearNTear>();
        wearNTear.m_health = health;

        return piece;
    }

    public static CustomPiece RemoveColliders(this CustomPiece piece)
    {
        var colliders = piece.PiecePrefab.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            Object.Destroy(collider);
        }

        return piece;
    }

    public static CustomPiece RemoveRenderers(this CustomPiece piece)
    {
        var renderers = piece.PiecePrefab.GetComponentsInChildren<Renderer>();
        foreach (var collider in renderers)
        {
            Object.Destroy(collider);
        }

        return piece;
    }
}