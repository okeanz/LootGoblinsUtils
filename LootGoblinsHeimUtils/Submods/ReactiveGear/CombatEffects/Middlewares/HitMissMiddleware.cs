using System;
using System.Collections.Generic;
using UnityEngine;

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Middlewares;

public class HitMissMiddleware : ICombatEventMiddleware
{
    private float _lastAttackStartTime;
    private float TimeSinceLastAttackStarted(float now) => Mathf.Max(0f, now - _lastAttackStartTime);
    private bool IsAttackAttemptActive => _lastAttackStartTime != 0f;
    
    private const float HitWindow = 1.5f;

    public IEnumerable<CombatEvent> Process(CombatEvent e)
    {
        var stream = new List<CombatEvent> { e };
        var now = e.Time;

        switch (e.Type)
        {
            case CombatEventType.AttackStarted:
                if (IsAttackAttemptActive && TimeSinceLastAttackStarted(now) > HitWindow)
                {
                    stream.Add(new CombatEvent(CombatEventType.MissConfirmed, now));
                }
                
                _lastAttackStartTime = now;
                break;
            case CombatEventType.HitLanded:
                if (IsAttackAttemptActive && TimeSinceLastAttackStarted(now) <= HitWindow)
                {
                    stream.Add(new CombatEvent(CombatEventType.HitConfirmed, now));
                    _lastAttackStartTime = 0f;
                }
                break;
            case CombatEventType.Tick:
                if(IsAttackAttemptActive && TimeSinceLastAttackStarted(now) > HitWindow)
                {
                    stream.Add(new CombatEvent(CombatEventType.MissConfirmed, now));
                    _lastAttackStartTime = 0f;
                }
                break;
        }

        return stream;
    }
}