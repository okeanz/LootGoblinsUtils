namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;

public enum CombatEventType
{
    AttackStarted,
    HitLanded,
    Dodge,
    Block,
    Parry,
    Tick,
    
    // Middleware result
    HitConfirmed,
    MissConfirmed
}

public readonly struct CombatEvent
{
    public readonly CombatEventType Type;
    public readonly float Time;


    public CombatEvent(CombatEventType type, float time)
    {
        Type = type;
        Time = time;
    }
}