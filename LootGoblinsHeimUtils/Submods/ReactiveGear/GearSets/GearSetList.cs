using System;
using System.Collections.Generic;
using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;

namespace LootGoblinsUtils.Submods.ReactiveGear.GearSets;

[Serializable]
public class GearSetList
{
    public List<SetDefinition> setList = new();
    public List<SetDescription> effectDescriptions = new();
}

[Serializable]
public sealed class SetDefinition
{
    public string name;
    public string displayName;
    public List<string> parts = new();
    public EffectNames effectBinding;
}

[Serializable]
public sealed class SetDescription
{
    public EffectNames name;
    public List<string> description;
}