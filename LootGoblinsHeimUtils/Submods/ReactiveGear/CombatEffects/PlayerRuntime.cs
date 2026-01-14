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
    public readonly DodgeHeavy.DodgeHeavyRuntime DodgeHeavyRuntime = new();

    public bool CanGainMomentum(float now)
        => !OverheatRuntime.HasOverheat(now);

    public bool HasMomentum(float now) => MomentumRuntime.HasMomentum(now);


    public override string ToString()
    {
        return $"dodge: {Newtonsoft.Json.JsonConvert.SerializeObject(DodgeHeavyRuntime)}";
    }
}