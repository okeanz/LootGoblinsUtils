/*using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;
using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Effects;

namespace Units;

[Ignore("")]
public class EffectsUnit
{
    private PlayerRuntime _rt;
    private BurstEffect _effect;

    [SetUp]
    public void SetUp()
    {
        _rt = new PlayerRuntime();
        _effect = new BurstEffect();
    }

    [Test]
    public void MomentumEnablesEffect()
    {
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.AttackStarted, 10f));
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.HitLanded, 11f));

        Assert.Multiple(() =>
        {
            Assert.That(_rt.HasMomentum, Is.False);
            Assert.That(_rt.MomentumAttackCounter, Is.EqualTo(1));
        });

        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.AttackStarted, 12f));
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.HitLanded, 13f));

        Assert.Multiple(() =>
        {
            Assert.That(_rt.HasMomentum, Is.False);
            Assert.That(_rt.HasOverheat, Is.False);
            Assert.That(_rt.MomentumAttackCounter, Is.EqualTo(2));
        });

        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.AttackStarted, 14f));
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.HitLanded, 15f));

        Assert.Multiple(() =>
        {
            Assert.That(_rt.HasMomentum, Is.True);
            Assert.That(_rt.HasOverheat, Is.False);
            Assert.That(_rt.MomentumAttackCounter, Is.EqualTo(3));
            Assert.That(_rt.MomentumStacks, Is.EqualTo(1));
            Assert.That(_rt.MomentumExpiresAt, Is.EqualTo(15 + BurstEffect.MomentumStackDuration));
        });

        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.AttackStarted, 16f));
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.HitLanded, 17f));

        Assert.Multiple(() =>
        {
            Assert.That(_rt.HasMomentum, Is.True);
            Assert.That(_rt.HasOverheat, Is.False);
            Assert.That(_rt.MomentumAttackCounter, Is.EqualTo(4));
            Assert.That(_rt.MomentumStacks, Is.EqualTo(2));
            Assert.That(_rt.MomentumExpiresAt, Is.EqualTo(17f + BurstEffect.MomentumStackDuration));
        });
    }

    private void BuildMomentum()
    {
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.AttackStarted, 10f));
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.HitLanded, 11f));
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.AttackStarted, 12f));
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.HitLanded, 13f));
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.AttackStarted, 14f));
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.HitLanded, 15f));

        Assert.Multiple(() =>
        {
            Assert.That(_rt.HasMomentum, Is.True);
            Assert.That(_rt.HasOverheat, Is.False);
            Assert.That(_rt.MomentumAttackCounter, Is.EqualTo(3));
            Assert.That(_rt.MomentumStacks, Is.EqualTo(1));
            Assert.That(_rt.MomentumExpiresAt, Is.EqualTo(15 + BurstEffect.MomentumStackDuration));
        });
    }

    [Test]
    public void MomentumDisablesEffect()
    {
        BuildMomentum();

        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.AttackStarted, 15.5f));
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.Tick, 16f));

        Assert.Multiple(() =>
        {
            Assert.That(_rt.HasMomentum, Is.True);
            Assert.That(_rt.MomentumAttackCounter, Is.EqualTo(3));
        });

        const float overheatTickTime = 16 + BurstEffect.PauseToOverheat + 2f;
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.Tick, overheatTickTime));

        Assert.Multiple(() =>
        {
            Assert.That(_rt.HasMomentum, Is.False);
            Assert.That(_rt.MomentumAttackCounter, Is.EqualTo(0));
            Assert.That(_rt.HasOverheat, Is.True);
        });
    }

    [Test]
    public void MomentumTimerDeactivation()
    {
        BuildMomentum();

        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.Tick, 15f + BurstEffect.MomentumStackDuration - 1f));

        Assert.That(_rt.HasMomentum, Is.True);

        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.Tick, 15f + BurstEffect.MomentumStackDuration + 1f));

        Assert.That(_rt.HasMomentum, Is.False);
    }

    [Test]
    public void OverheatTimerDeactivation()
    {
        BuildMomentum();

        
        var nextAttackAttempt = _rt.LastHitLandedTime + 1f;
        
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.AttackStarted, nextAttackAttempt));
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.Tick, nextAttackAttempt + BurstEffect.PauseToOverheat));
        Assert.Multiple(() =>
        {
            Assert.That(_rt.HasMomentum, Is.False);
            Assert.That(_rt.HasOverheat, Is.True);
        });


        var afterOverheatDuration = nextAttackAttempt + BurstEffect.PauseToOverheat + BurstEffect.OverheatDuration;
        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.Tick, afterOverheatDuration));
        Assert.Multiple(() =>
        {
            Assert.That(_rt.HasMomentum, Is.False);
            Assert.That(_rt.HasOverheat, Is.False);
        });
    }

    private void DefensiveTriggerByType(CombatEventType eventType)
    {
        BuildMomentum();

        _effect.OnEvent(_rt, new CombatEvent(CombatEventType.Tick, 16f));
        
        Assert.Multiple(() =>
        {
            Assert.That(_rt.HasMomentum, Is.True);
            Assert.That(_rt.HasOverheat, Is.False);
        });
        
        _effect.OnEvent(_rt, new CombatEvent(eventType, 16.2f));
        
        Assert.Multiple(() =>
        {
            Assert.That(_rt.HasMomentum, Is.False);
            Assert.That(_rt.HasOverheat, Is.True);
        });
    }
    
    [Test]
    public void OverheatDefensiveTriggerBlock()
    {
        DefensiveTriggerByType(CombatEventType.Block);
    }
    
    [Test]
    public void OverheatDefensiveTriggerParry()
    {
        DefensiveTriggerByType(CombatEventType.Parry);
    }
    
    [Test]
    public void OverheatDefensiveTriggerDodge()
    {
        DefensiveTriggerByType(CombatEventType.Dodge);
    }
}*/