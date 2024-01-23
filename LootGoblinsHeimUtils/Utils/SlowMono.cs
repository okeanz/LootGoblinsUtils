using System;
using UnityEngine;

namespace LootGoblinsUtils.Utils;

public abstract class SlowMono: MonoBehaviour
{
    protected float UpdateStep = 1f;
    private float _lastUpdate = Time.time;
    private void Update()
    {
        if (Time.time - _lastUpdate > UpdateStep)
        {
            _lastUpdate = Time.time;
            if(!ZoneSystem.instance.IsActiveAreaLoaded()) return;
            if(ZNetScene.instance.InLoadingScreen()) return;
            SlowUpdate();
        }
    }

    public abstract void SlowUpdate();
}