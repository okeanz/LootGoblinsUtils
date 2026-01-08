using System.Collections.Generic;

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Middlewares;

public interface ICombatEventMiddleware
{
    IEnumerable<CombatEvent> Process(CombatEvent input);
}