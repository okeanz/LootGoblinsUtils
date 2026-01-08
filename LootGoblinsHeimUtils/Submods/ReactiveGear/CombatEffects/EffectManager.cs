using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Effects;
using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Middlewares;
using LootGoblinsUtils.Submods.ReactiveGear.GearSets;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;

public static class EffectManager
{
    private static readonly List<ICombatEffect> Effects = new();
    private static readonly List<ICombatEventMiddleware> Middlewares = new();
    private static void RegisterEffect(ICombatEffect effect) => Effects.Add(effect);
    private static void RegisterEffects(ICombatEffect[] effects) => Effects.AddRange(effects);
    private static void RegisterMiddleware(ICombatEventMiddleware middleware) => Middlewares.Add(middleware);

    private static readonly PlayerRuntime PlayerRuntime = new();

    public static void Init()
    {
        RegisterEffects(EffectBindings.EffectsMap[EffectNames.Burst]);
        RegisterMiddleware(new HitMissMiddleware());

        EffectUI.InitUI();

        LootGoblinsHeimUtilsPlugin.Instance.StartCoroutine(TickEvent());
    }

    public static void EffectActivationCheck()
    {
        Effects.ForEach(effect => effect.IsActive = false);

        var activeSets = GearSetRegistry.EquippedSetCounterBySetName.Where(pair => pair.Value.IsActive);

        foreach (var set in activeSets)
        {
            var setDef = GearSetRegistry.SetsBySetName[set.Key];
            foreach (var combatEffect in EffectBindings.EffectsMap[setDef.effectBinding])
            {
                combatEffect.IsActive = true;
            }
        }
    }

    private static IEnumerator TickEvent()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (Player.m_localPlayer == null) continue;

            Handle(new CombatEvent(CombatEventType.Tick, Time.time));
        }
    }

    public static void Handle(CombatEvent combatEvent)
    {
        IEnumerable<CombatEvent> stream = new[] { combatEvent };

        foreach (var mw in Middlewares)
            stream = stream.SelectMany(e => mw.Process(e));


        // 1) Обработать событие
        foreach (var e in stream)
        foreach (var combatEffect in Effects.Where(effect => effect.IsActive))
            combatEffect.OnEvent(PlayerRuntime, e);

        // 2) Пересчитать модификаторы 
        var now = combatEvent.Time;
        PlayerRuntime.Mods.Reset();
        foreach (var combatEffect in Effects)
            combatEffect.ContributeModifiers(PlayerRuntime, PlayerRuntime.Mods, now);


        var activeEffects = Effects.Where(x => x.IsActive).ToArray();

        var sb = new StringBuilder();
        sb.Append($"{PlayerRuntime}");
        sb.Append($"\nMods: {PlayerRuntime.Mods}");
        if (activeEffects.Any())
        {
            sb.Append($"\nActive Effects: {string.Join(", ", activeEffects.Select(x => x.Id))}");
        }
        else
        {
            sb.Append($"\nNo Active Effects");
        }

        EffectUI.SetText(sb.ToString());
    }

    public static PlayerModifiers GetPlayerModifiers() => PlayerRuntime.Mods;
}