using System;
using UnityEngine;

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Effects;

public class BurstMomentumEffect:ICombatEffect
{
    public string Id => "burst_momentum";
    public bool IsActive { get; set; }
    
    // Конфиг Momentum
    public const int MaxStacks = 3;
    public const int HitsForNextStack = 3;
    public const float MomentumStackDuration = 10.0f;

    // Модификаторы (читаемые числа; бонусы суммируются по стакам)
    public const float AttackSpeedBonusPerStack = 0.40f;       // +20% per stack
    public const float StaminaCostBonusPerStack = 0.15f;       // +15% per stack
    
    public void OnEvent(PlayerRuntime playerRuntime, CombatEvent combatEvent)
    {
        var now = combatEvent.Time;
        var momentumRuntime = playerRuntime.MomentumRuntime;
        
        switch (combatEvent.Type)
        {
            case CombatEventType.HitConfirmed:
                momentumRuntime.MomentumAttackCounter++;

                if (playerRuntime.HasMomentum(now))
                {
                    momentumRuntime.MomentumExpiresAt = now + MomentumStackDuration;
                }
                
                if (playerRuntime.CanGainMomentum(now) && momentumRuntime.MomentumAttackCounter >= HitsForNextStack)
                {
                    AddMomentum(momentumRuntime, now);
                }
                break;
            case CombatEventType.Tick:
                if (momentumRuntime.IsMomentumExpired(now))
                {
                    momentumRuntime.MomentumStacks = 0;
                    momentumRuntime.MomentumExpiresAt = 0;
                    momentumRuntime.MomentumAttackCounter = 0;
                }
                break;
        }
    }
    
    private void AddMomentum(MomentumRuntime rt, float now)
    {
        rt.MomentumStacks = Mathf.Min(MaxStacks, rt.MomentumStacks + 1);
        rt.MomentumExpiresAt = now + MomentumStackDuration;
        rt.MomentumAttackCounter = 0;
    }

    public void ContributeModifiers(PlayerRuntime playerRuntime, PlayerModifiers playerMods, float now)
    {
        var rt = playerRuntime.MomentumRuntime;
        
        if (!rt.HasMomentum(now))
            return;

        var stacks = rt.MomentumStacks;

        playerMods.AttackSpeedMult *= 1f + AttackSpeedBonusPerStack * stacks;
        playerMods.AttackStaminaCostMult *= 1f + StaminaCostBonusPerStack * stacks;
    }

    public class MomentumRuntime
    {
        public int MomentumStacks;
        public int MomentumAttackCounter;
        public float MomentumExpiresAt;
        
        public bool HasMomentum(float now)
            => MomentumStacks > 0 && MomentumExpiresAt > now;

        public bool IsMomentumExpired(float now) => MomentumStacks > 0 && now >= MomentumExpiresAt;

        public void Reset()
        {
            MomentumStacks = 0;
            MomentumExpiresAt = 0;
            MomentumAttackCounter = 0;
        }
    }
}