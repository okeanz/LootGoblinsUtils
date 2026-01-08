using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;
using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Middlewares;

namespace Units;

[TestFixture]
public class HitMissMiddlewareTests
{
    private HitMissMiddleware _mw;

    [SetUp]
    public void Setup()
    {
        _mw = new HitMissMiddleware();
    }

    [Test]
    public void AttackStarted_EmitsOnlyOriginalEvent()
    {
        var outEvents = _mw.Process(new CombatEvent(CombatEventType.AttackStarted, 10f)).ToList();

        Assert.That(outEvents.Select(x => x.Type), Is.EquivalentTo(new[]
        {
            CombatEventType.AttackStarted
        }));
    }

    [Test]
    public void HitLanded_WithoutAttackStarted_DoesNotEmitHitConfirmed()
    {
        var outEvents = _mw.Process(new CombatEvent(CombatEventType.HitLanded, 10f)).ToList();

        Assert.That(outEvents.Select(x => x.Type), Is.EquivalentTo(new[]
        {
            CombatEventType.HitLanded
        }));
    }

    [Test]
    public void HitLanded_WithinWindow_EmitsHitConfirmed_AndClosesAttempt()
    {
        _mw.Process(new CombatEvent(CombatEventType.AttackStarted, 10f));

        var outEvents = _mw.Process(new CombatEvent(CombatEventType.HitLanded, 11.4f)).ToList(); // 1.4s <= 1.5

        Assert.That(outEvents.Select(x => x.Type), Is.EquivalentTo(new[]
        {
            CombatEventType.HitLanded,
            CombatEventType.HitConfirmed
        }));

        // Attempt should be closed: another HitLanded alone should not produce HitConfirmed
        var outEvents2 = _mw.Process(new CombatEvent(CombatEventType.HitLanded, 11.5f)).ToList();
        Assert.That(outEvents2.Any(e => e.Type == CombatEventType.HitConfirmed), Is.False);
    }

    [Test]
    public void HitLanded_AtBoundary_EmitsHitConfirmed()
    {
        _mw.Process(new CombatEvent(CombatEventType.AttackStarted, 10f));

        var outEvents = _mw.Process(new CombatEvent(CombatEventType.HitLanded, 11.5f)).ToList(); // exactly 1.5

        Assert.That(outEvents.Any(e => e.Type == CombatEventType.HitConfirmed), Is.True);
    }

    [Test]
    public void Tick_AfterWindow_EmitsMissConfirmed_AndClosesAttempt()
    {
        _mw.Process(new CombatEvent(CombatEventType.AttackStarted, 10f));

        var outEvents = _mw.Process(new CombatEvent(CombatEventType.Tick, 11.6f)).ToList(); // 1.6 > 1.5

        Assert.That(outEvents.Select(x => x.Type), Is.EquivalentTo(new[]
        {
            CombatEventType.Tick,
            CombatEventType.MissConfirmed
        }));

        // Attempt should be closed: subsequent Tick should not emit another MissConfirmed
        var outEvents2 = _mw.Process(new CombatEvent(CombatEventType.Tick, 12.0f)).ToList();
        Assert.That(outEvents2.Any(e => e.Type == CombatEventType.MissConfirmed), Is.False);
    }

    [Test]
    public void Tick_WithinWindow_DoesNotEmitMissConfirmed()
    {
        _mw.Process(new CombatEvent(CombatEventType.AttackStarted, 10f));

        var outEvents = _mw.Process(new CombatEvent(CombatEventType.Tick, 11.0f)).ToList(); // 1.0 <= 1.5

        Assert.That(outEvents.Select(x => x.Type), Is.EquivalentTo(new[]
        {
            CombatEventType.Tick
        }));
    }

    [Test]
    public void AttackStarted_WhenPreviousAttemptOverdue_EmitsMissConfirmed_ThenStartsNewAttempt()
    {
        // Open attempt at t=10
        _mw.Process(new CombatEvent(CombatEventType.AttackStarted, 10f));

        // Start a new attack at t=11.6 -> previous is overdue (>1.5), so emit MissConfirmed
        var outEvents = _mw.Process(new CombatEvent(CombatEventType.AttackStarted, 11.6f)).ToList();

        Assert.That(outEvents.Select(x => x.Type), Is.EquivalentTo(new[]
        {
            CombatEventType.AttackStarted,
            CombatEventType.MissConfirmed
        }));

        // The new attempt should now be active; a hit in-window from 11.6 should confirm
        var outEvents2 = _mw.Process(new CombatEvent(CombatEventType.HitLanded, 12.5f)).ToList(); // 0.9 <= 1.5
        Assert.That(outEvents2.Any(e => e.Type == CombatEventType.HitConfirmed), Is.True);
    }

    [Test]
    public void LateHit_AfterWindow_DoesNotEmitHitConfirmed()
    {
        _mw.Process(new CombatEvent(CombatEventType.AttackStarted, 10f));

        var outEvents = _mw.Process(new CombatEvent(CombatEventType.HitLanded, 12.0f)).ToList(); // 2.0 > 1.5

        Assert.That(outEvents.Any(e => e.Type == CombatEventType.HitConfirmed), Is.False);
    }
}