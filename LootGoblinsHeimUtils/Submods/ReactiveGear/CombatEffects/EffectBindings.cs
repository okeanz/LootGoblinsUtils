using System.Collections.Generic;
using System.Runtime.Serialization;
using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Effects;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;

[JsonConverter(typeof(StringEnumConverter))]
public enum EffectNames
{
    [EnumMember(Value = "burst_effect")] Burst
}

public static class EffectBindings
{
    public static readonly Dictionary<EffectNames, ICombatEffect[]> EffectsMap = new()
    {
        { EffectNames.Burst, new ICombatEffect[] { new BurstMomentumEffect(), new BurstOverheatEffect() } }
    };
}