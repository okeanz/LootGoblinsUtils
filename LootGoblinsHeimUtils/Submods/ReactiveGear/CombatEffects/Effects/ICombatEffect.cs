namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Effects;

public interface ICombatEffect
{
    string Id { get; } // "burst", "pressure", ...
    
    bool IsActive { get; set; }
    void OnEvent(PlayerRuntime playerRuntime, CombatEvent combatEvent);
    void ContributeModifiers(PlayerRuntime playerRuntime, PlayerModifiers playerMods, float now);
}