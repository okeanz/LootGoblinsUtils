using System;
using System.Collections.Generic;
using Jotunn.Managers;
using LootGoblinsUtils.Configuration;
using LootGoblinsUtils.Utils;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Conquest.Pieces.Core;

public class ConquestCore : SlowMono
{
    public static readonly List<ConquestCore> ActiveCores = new();
    private void Awake()
    {
        UpdateStep = 1f;
        
        if (gameObject.PiecePlaced() && !ActiveCores.Contains(this))
        {
            ActiveCores.Add(this);
        }
    }

    private void Start()
    {
        if (gameObject.PiecePlaced() && !ActiveCores.Contains(this))
        {
            ActiveCores.Add(this);
        }
    }

    private void OnDisable()
    {
        ActiveCores.Remove(this);
    }

    private void OnDestroy()
    {
        ActiveCores.Remove(this);
    }

    public override void SlowUpdate()
    {
        if (gameObject.PiecePlaced() && !ActiveCores.Contains(this))
        {
            ActiveCores.Add(this);
        }
    }
}