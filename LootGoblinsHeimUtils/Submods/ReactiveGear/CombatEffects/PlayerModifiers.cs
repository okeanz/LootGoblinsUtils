namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;

public sealed class PlayerModifiers
{
    // Все по умолчанию = 1
    public float AttackStaminaCostMult = 1f;
    public float StaminaRegenMult = 1f;
    public float AttackSpeedMult = 1f;

    public void Reset()
    {
        AttackStaminaCostMult = 1f;
        StaminaRegenMult = 1f;
        AttackSpeedMult = 1f;
    }

    public override string ToString()
    {
        return
            $"AttackStaminaCostMult: {AttackStaminaCostMult}; StaminaRegenMult: {StaminaRegenMult}; AttackSpeedMult: {AttackSpeedMult}";
    }
}