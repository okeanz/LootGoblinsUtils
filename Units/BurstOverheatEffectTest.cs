// BurstOverheatEffectTests.cs

using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;
using LootGoblinsUtils.Submods.ReactiveGear.CombatEffects.Effects;

namespace Units
{
    [TestFixture]
    public class BurstOverheatEffectTests
    {
        private PlayerRuntime _rt;
        private BurstOverheatEffect _effect;

        [SetUp]
        public void SetUp()
        {
            _rt = new PlayerRuntime();
            _effect = new BurstOverheatEffect();
        }

        [Test]
        public void MissConfirmed_WhenNoMomentum_DoesNotEnterOverheat_AndDoesNotResetMomentum()
        {
            // Arrange: no momentum
            Assert.That(_rt.HasMomentum(10f), Is.False);

            // Act
            _effect.OnEvent(_rt, Ev(CombatEventType.MissConfirmed, 10f));

            // Assert: overheat not started
            Assert.That(_rt.OverheatRuntime.HasOverheat(10f), Is.False);

            // Assert: momentum still absent (and not "changed" into something)
            Assert.That(_rt.MomentumRuntime.MomentumStacks, Is.EqualTo(0));
            Assert.That(_rt.MomentumRuntime.MomentumExpiresAt, Is.EqualTo(0f));
            Assert.That(_rt.MomentumRuntime.MomentumAttackCounter, Is.EqualTo(0));
        }

        [TestCase(CombatEventType.MissConfirmed)]
        [TestCase(CombatEventType.Dodge)]
        [TestCase(CombatEventType.Block)]
        [TestCase(CombatEventType.Parry)]
        public void TriggerEvents_WhenMomentumActive_StartOverheat_AndResetMomentum(CombatEventType trigger)
        {
            // Arrange: make momentum active at t=10
            GiveMomentum(now: 10f, stacks: 2, duration: 5f);
            Assert.That(_rt.HasMomentum(10f), Is.True);

            // Act: trigger at t=11
            _effect.OnEvent(_rt, Ev(trigger, 11f));

            // Assert: momentum reset
            Assert.That(_rt.MomentumRuntime.MomentumStacks, Is.EqualTo(0));
            Assert.That(_rt.MomentumRuntime.MomentumExpiresAt, Is.EqualTo(0f));
            Assert.That(_rt.MomentumRuntime.MomentumAttackCounter, Is.EqualTo(0));

            // Assert: overheat started
            Assert.That(_rt.OverheatRuntime.OverheatExpiresAt,
                Is.EqualTo(11f + BurstOverheatEffect.OverheatDuration).Within(1e-6));

            Assert.That(_rt.OverheatRuntime.HasOverheat(11f), Is.True);
        }

        [Test]
        public void ContributeModifiers_WhenOverheatActive_AppliesRegenMultiplier()
        {
            // Arrange: start overheat by simulating momentum + miss
            GiveMomentum(now: 10f, stacks: 1, duration: 5f);
            _effect.OnEvent(_rt, Ev(CombatEventType.MissConfirmed, 10f));

            var mods = new PlayerModifiers(); // assumes defaults are 1f
            Assert.That(_rt.OverheatRuntime.HasOverheat(10f), Is.True);

            // Act
            _effect.ContributeModifiers(_rt, mods, 10f);

            // Assert
            Assert.That(mods.StaminaRegenMult,
                Is.EqualTo(1f * BurstOverheatEffect.OverheatRegenMult).Within(1e-6));
        }

        [Test]
        public void ContributeModifiers_WhenOverheatInactive_DoesNotChangeRegenMultiplier()
        {
            // Arrange: overheat inactive
            var mods = new PlayerModifiers();

            // Act
            _effect.ContributeModifiers(_rt, mods, 10f);

            // Assert
            Assert.That(mods.StaminaRegenMult, Is.EqualTo(1f).Within(1e-6));
        }

        [Test]
        public void Tick_AfterOverheatExpires_ClearsOverheatExpiresAt()
        {
            // Arrange: start overheat at t=10 via momentum+miss
            GiveMomentum(now: 10f, stacks: 1, duration: 5f);
            _effect.OnEvent(_rt, Ev(CombatEventType.MissConfirmed, 10f));

            var expiresAt = _rt.OverheatRuntime.OverheatExpiresAt;
            Assert.That(expiresAt, Is.EqualTo(10f + BurstOverheatEffect.OverheatDuration).Within(1e-6));
            Assert.That(_rt.OverheatRuntime.HasOverheat(10f), Is.True);

            // Act: tick just before expiry -> should still be active, and Tick should not clear
            _effect.OnEvent(_rt, Ev(CombatEventType.Tick, expiresAt - 0.001f));
            Assert.That(_rt.OverheatRuntime.HasOverheat(expiresAt - 0.001f), Is.True);
            Assert.That(_rt.OverheatRuntime.OverheatExpiresAt, Is.EqualTo(expiresAt).Within(1e-6));

            // Act: tick after expiry -> should clear to 0
            _effect.OnEvent(_rt, Ev(CombatEventType.Tick, expiresAt + 0.001f));
            Assert.That(_rt.OverheatRuntime.HasOverheat(expiresAt + 0.001f), Is.False);
            Assert.That(_rt.OverheatRuntime.OverheatExpiresAt, Is.EqualTo(0f).Within(1e-6));
        }

        [Test]
        public void TriggerEvent_WhenOverheatAlreadyActive_RefreshesDuration()
        {
            // Arrange: start overheat at t=10
            GiveMomentum(now: 10f, stacks: 1, duration: 5f);
            _effect.OnEvent(_rt, Ev(CombatEventType.MissConfirmed, 10f));

            var firstExpires = _rt.OverheatRuntime.OverheatExpiresAt;

            // Arrange: re-add momentum to allow EnterOverheat again
            // (In real play, CanGainMomentum would likely block this while overheat is active,
            // but this test checks BurstOverheatEffect behavior in isolation.)
            GiveMomentum(now: 11f, stacks: 1, duration: 5f);

            // Act: trigger again at t=12
            _effect.OnEvent(_rt, Ev(CombatEventType.Block, 12f));

            var secondExpires = _rt.OverheatRuntime.OverheatExpiresAt;

            // Assert: refreshed to 12 + duration
            Assert.That(secondExpires, Is.EqualTo(12f + BurstOverheatEffect.OverheatDuration).Within(1e-6));
            Assert.That(secondExpires, Is.GreaterThan(firstExpires));
        }

        // -----------------------
        // Helpers
        // -----------------------

        private static CombatEvent Ev(CombatEventType type, float time)
            => new CombatEvent(type, time);

        private void GiveMomentum(float now, int stacks, float duration)
        {
            _rt.MomentumRuntime.MomentumStacks = stacks;
            _rt.MomentumRuntime.MomentumExpiresAt = now + duration;
            _rt.MomentumRuntime.MomentumAttackCounter = 123; // ensure reset is observable
        }
    }
}
