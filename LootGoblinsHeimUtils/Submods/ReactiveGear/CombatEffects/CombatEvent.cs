namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;

public enum CombatEventType
{
    AttackStarted,
    HitLanded,
    DodgeAction,
    Block,
    Parry,
    Tick,
    DodgeSuccess,

    // Middleware result
    HitConfirmed,
    MissConfirmed
}

public readonly struct CombatEvent
{
    public readonly CombatEventType Type;
    public readonly float Time;

    public readonly bool IsAttackSecondary;


    public CombatEvent(CombatEventType type, float time, bool isAttackSecondary = false)
    {
        Type = type;
        Time = time;
        IsAttackSecondary = isAttackSecondary;
    }
}