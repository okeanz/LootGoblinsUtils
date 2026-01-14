using System;

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Effects;

public class BurstOverheatEffect : ICombatEffect
{
    public string Id => "burst_overheat";
    public bool IsActive { get; set; }

    public const float OverheatDuration = 3.0f;

    public const float OverheatRegenMult = 0.01f;

    public void OnEvent(PlayerRuntime playerRuntime, CombatEvent combatEvent)
    {
        var now = combatEvent.Time;
        var rt = playerRuntime.OverheatRuntime;

        switch (combatEvent.Type)
        {
            case CombatEventType.MissConfirmed:
            case CombatEventType.DodgeAction:
            case CombatEventType.Block:
            case CombatEventType.Parry:
                if (playerRuntime.HasMomentum(now))
                {
                    EnterOverheat(playerRuntime, now);
                }

                break;
            case CombatEventType.Tick:
                if (rt.IsOverheatExpired(now))
                {
                    rt.OverheatExpiresAt = 0;
                }

                break;
        }
    }

    private void EnterOverheat(PlayerRuntime playerRuntime, float now)
    {
        playerRuntime.MomentumRuntime.Reset();
        playerRuntime.OverheatRuntime.OverheatExpiresAt = now + OverheatDuration;
    }

    public void ContributeModifiers(PlayerRuntime playerRuntime, PlayerModifiers playerMods, float now)
    {
        if (playerRuntime.OverheatRuntime.HasOverheat(now))
        {
            playerMods.StaminaRegenMult *= OverheatRegenMult;
        }
    }

    public class BurstOverheatRuntime
    {
        public float OverheatExpiresAt;

        public bool HasOverheat(float now)
            => OverheatExpiresAt > now;

        public bool IsOverheatExpired(float now) => OverheatExpiresAt > 0 && now >= OverheatExpiresAt;
    }
}