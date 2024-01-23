using System;
using System.Collections.Generic;
using System.Linq;
using LootGoblinsUtils.Conquest.Pieces.Ghost;
using LootGoblinsUtils.Utils;
using UnityEngine;

namespace LootGoblinsUtils.Conquest.Pieces.DefenderTotem;

public class DefenderTotemComponent : SlowMono
{
    public static readonly List<DefenderTotemComponent> ActiveTotems = new();
    private ZNetView _netView;
    private bool isPlaced;

    private void Awake()
    {
        UpdateStep = 1f;
    }

    private void Start()
    {
        _netView = GetComponent<ZNetView>();
        if (_netView != null && _netView.IsValid())
        {
            isPlaced = _netView.GetZDO().GetBool("isPlaced");
        }
    }

    private void OnDisable()
    {
        ActiveTotems.Remove(this);
    }

    private void OnDestroy()
    {
        ActiveTotems.Remove(this);
    }

    public override void SlowUpdate()
    {
        if (gameObject.PiecePlaced() && !ActiveTotems.Contains(this))
        {
            ActiveTotems.Add(this);
            
            if(isPlaced) return;
            isPlaced = true;
            if (_netView != null && _netView.IsValid())
            {
                _netView.GetZDO().Set("isPlaced", true);
            }

            foreach (var wnt in WearNTear.s_allInstances
                         .Where(wnt => wnt.gameObject != gameObject &&
                                       wnt.GetComponent<ConquestGhost>() == null &&
                                       Vector3.Distance(wnt.transform.position, transform.position) < 20f
                         )
                    )
            {
                wnt.m_piece.m_resources = Array.Empty<Piece.Requirement>();
                wnt.Destroy();
            }
        }
    }
}