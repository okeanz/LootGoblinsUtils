using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;
using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Effects;

namespace Units;

[TestFixture]
public class BurstMomentumEffectTests
{
    private BurstMomentumEffect _effect;
    private PlayerModifiers _mods;
    private PlayerRuntime _playerRuntime;

    private BurstMomentumEffect.MomentumRuntime MomentumRuntime => _playerRuntime.MomentumRuntime;

    [SetUp]
    public void SetUp()
    {
        _effect = new BurstMomentumEffect();
        _playerRuntime = new PlayerRuntime();
        _mods = new PlayerModifiers();
    }

    private void Send(CombatEventType type, float t)
        => _effect.OnEvent(_playerRuntime, new CombatEvent(type, t));

    [Test]
    public void HitConfirmed_TwoHits_DoesNotGrantMomentum()
    {
        Send(CombatEventType.HitConfirmed, 10f);
        Send(CombatEventType.HitConfirmed, 10.1f);

        Assert.That(MomentumRuntime.MomentumStacks, Is.EqualTo(0));
        Assert.That(MomentumRuntime.HasMomentum(10.1f), Is.False);
        Assert.That(MomentumRuntime.MomentumAttackCounter, Is.EqualTo(2));
    }

    [Test]
    public void HitConfirmed_ThirdHit_GrantsMomentum_AndResetsCounter()
    {
        Send(CombatEventType.HitConfirmed, 10f);
        Send(CombatEventType.HitConfirmed, 10.1f);
        Send(CombatEventType.HitConfirmed, 10.2f);

        Assert.That(MomentumRuntime.MomentumStacks, Is.EqualTo(1));
        Assert.That(MomentumRuntime.HasMomentum(10.2f), Is.True);
        Assert.That(MomentumRuntime.MomentumExpiresAt,
            Is.EqualTo(10.2f + BurstMomentumEffect.MomentumStackDuration).Within(1e-6));
        Assert.That(MomentumRuntime.MomentumAttackCounter, Is.EqualTo(0),
            "Counter should reset after granting a stack.");
    }

    [Test]
    public void HitConfirmed_GrantsAdditionalStacks_EveryThreeHits()
    {
        // 6 hits -> 2 stacks
        for (int i = 0; i < 6; i++)
            Send(CombatEventType.HitConfirmed, 10f + 0.1f * i);

        Assert.That(MomentumRuntime.MomentumStacks, Is.EqualTo(2));
        Assert.That(MomentumRuntime.MomentumAttackCounter, Is.EqualTo(0));
    }

    [Test]
    public void HitConfirmed_StacksAreCapped()
    {
        // 12 hits -> would be 4 stacks, but cap is 3
        for (int i = 0; i < 12; i++)
            Send(CombatEventType.HitConfirmed, 10f + 0.1f * i);

        Assert.That(MomentumRuntime.MomentumStacks, Is.EqualTo(BurstMomentumEffect.MaxStacks));
        Assert.That(MomentumRuntime.MomentumAttackCounter, Is.EqualTo(0));
    }

    [Test]
    public void HitConfirmed_RefreshesExpiry_WhenAddingStack()
    {
        // First stack at t=10.2
        Send(CombatEventType.HitConfirmed, 10.0f);
        Send(CombatEventType.HitConfirmed, 10.1f);
        Send(CombatEventType.HitConfirmed, 10.2f);

        var firstExpiry = MomentumRuntime.MomentumExpiresAt;
        Assert.That(firstExpiry, Is.EqualTo(10.2f + BurstMomentumEffect.MomentumStackDuration).Within(1e-6));

        // Next stack at t=11.2
        Send(CombatEventType.HitConfirmed, 11.0f);
        Send(CombatEventType.HitConfirmed, 11.1f);
        Send(CombatEventType.HitConfirmed, 11.2f);

        var secondExpiry = MomentumRuntime.MomentumExpiresAt;
        Assert.That(secondExpiry, Is.EqualTo(11.2f + BurstMomentumEffect.MomentumStackDuration).Within(1e-6));
        Assert.That(secondExpiry, Is.GreaterThan(firstExpiry));
        Assert.That(MomentumRuntime.MomentumStacks, Is.EqualTo(2));
    }

    [Test]
    public void Tick_AfterExpiry_ClearsMomentumAndCounter()
    {
        // Activate at t=10.2, expires at 12.2 if duration is 2.0
        Send(CombatEventType.HitConfirmed, 10.0f);
        Send(CombatEventType.HitConfirmed, 10.1f);
        Send(CombatEventType.HitConfirmed, 10.2f);

        var expiry = MomentumRuntime.MomentumExpiresAt;

        // Tick just before expiry: still active
        Send(CombatEventType.Tick, expiry - 0.001f);
        Assert.That(MomentumRuntime.HasMomentum(expiry - 0.001f), Is.True);

        // Tick after expiry: should clear
        Send(CombatEventType.Tick, expiry + 0.001f);
        Assert.That(MomentumRuntime.HasMomentum(expiry + 0.001f), Is.False);
        Assert.That(MomentumRuntime.MomentumStacks, Is.EqualTo(0));
        Assert.That(MomentumRuntime.MomentumExpiresAt, Is.EqualTo(0f));
        Assert.That(MomentumRuntime.MomentumAttackCounter, Is.EqualTo(0));
    }

    [Test]
    public void ContributeModifiers_AppliesOnlyWhileMomentumActive()
    {
        // Activate momentum at t=10.2
        Send(CombatEventType.HitConfirmed, 10.0f);
        Send(CombatEventType.HitConfirmed, 10.1f);
        Send(CombatEventType.HitConfirmed, 10.2f);

        // Apply modifiers at t=10.2 (active)
        _mods = new PlayerModifiers();
        _effect.ContributeModifiers(_playerRuntime, _mods, 10.2f);

        var expectedAtkSpeed = 1f + BurstMomentumEffect.AttackSpeedBonusPerStack * MomentumRuntime.MomentumStacks;
        var expectedStamCost = 1f + BurstMomentumEffect.StaminaCostBonusPerStack * MomentumRuntime.MomentumStacks;

        Assert.That(_mods.AttackSpeedMult, Is.EqualTo(expectedAtkSpeed).Within(1e-6));
        Assert.That(_mods.AttackStaminaCostMult, Is.EqualTo(expectedStamCost).Within(1e-6));

        // After expiry, modifiers should not be applied
        var after = MomentumRuntime.MomentumExpiresAt + 0.01f;

        _mods = new PlayerModifiers();
        _effect.ContributeModifiers(_playerRuntime, _mods, after);

        Assert.That(_mods.AttackSpeedMult, Is.EqualTo(1f).Within(1e-6));
        Assert.That(_mods.AttackStaminaCostMult, Is.EqualTo(1f).Within(1e-6));
    }
}