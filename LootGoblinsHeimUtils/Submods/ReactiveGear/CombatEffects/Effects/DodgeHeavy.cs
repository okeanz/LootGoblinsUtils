namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Effects;

public class DodgeHeavy : ICombatEffect
{
    public string Id => "dodge_heavy";
    public bool IsActive { get; set; }

    public const float SecondaryAttackDamageModifier = 0.15f;
    public const float CounterLasts = 10f;
    public const int MaxDodgeCounter = 3;

    public void OnEvent(PlayerRuntime playerRuntime, CombatEvent combatEvent)
    {
        var now = combatEvent.Time;
        var dodgeRuntime = playerRuntime.DodgeHeavyRuntime;

        switch (combatEvent.Type)
        {
            case CombatEventType.DodgeSuccess:
                if (dodgeRuntime.DodgeSuccessCounter < MaxDodgeCounter)
                    dodgeRuntime.DodgeSuccessCounter++;

                dodgeRuntime.CounterExpiresAt = now + CounterLasts;
                break;
            case CombatEventType.Parry:
            case CombatEventType.Block:
            case CombatEventType.HitConfirmed when combatEvent.IsAttackSecondary:
                dodgeRuntime.DodgeSuccessCounter = 0;
                break;
            case CombatEventType.Tick when dodgeRuntime.DodgeSuccessCounter > 0:
                if (now > dodgeRuntime.CounterExpiresAt)
                    dodgeRuntime.DodgeSuccessCounter = 0;
                break;
        }
    }

    public void ContributeModifiers(PlayerRuntime playerRuntime, PlayerModifiers playerMods, float now)
    {
        playerMods.SecondaryDamageMult *=
            1 + SecondaryAttackDamageModifier * playerRuntime.DodgeHeavyRuntime.DodgeSuccessCounter;
    }

    public class DodgeHeavyRuntime
    {
        public int DodgeSuccessCounter;
        public float CounterExpiresAt;
    }
}