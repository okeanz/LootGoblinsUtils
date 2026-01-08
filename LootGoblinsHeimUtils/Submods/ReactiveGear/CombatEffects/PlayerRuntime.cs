using System;
using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Effects;
using LootGoblinsUtils.Utils;
using UnityEngine;

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;

public sealed class PlayerRuntime
{
    public readonly PlayerModifiers Mods = new();
    public readonly BurstMomentumEffect.MomentumRuntime MomentumRuntime = new();
    public readonly BurstOverheatEffect.BurstOverheatRuntime OverheatRuntime = new();

    public bool CanGainMomentum(float now)
        => !OverheatRuntime.HasOverheat(now);

    public bool HasMomentum(float now) => MomentumRuntime.HasMomentum(now);


    public override string ToString()
    {
        return
            $"stacks: {MomentumRuntime.MomentumStacks}, " +
            $"timeLeft: {Math.Round(Mathf.Max(0, MomentumRuntime.MomentumExpiresAt - Time.time), 2)}, " +
            $"overheatLeft: {Math.Round(Mathf.Max(0, OverheatRuntime.OverheatExpiresAt - Time.time), 2)}";
    }
}