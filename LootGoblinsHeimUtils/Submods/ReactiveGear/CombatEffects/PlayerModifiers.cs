namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;

public sealed class PlayerModifiers
{
    // Все по умолчанию = 1
    public float AttackStaminaCostMult = 1f;
    public float StaminaRegenMult = 1f;
    public float AttackSpeedMult = 1f;
    
    public float DamageMult = 1f;
    public float SecondaryDamageMult = 1f;
    public float LightDamageMult = 1f;

    public void Reset()
    {
        AttackStaminaCostMult = 1f;
        StaminaRegenMult = 1f;
        AttackSpeedMult = 1f;

        DamageMult = 1f;
        SecondaryDamageMult = 1f;
        LightDamageMult = 1f;
    }

    public override string ToString()
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(this);
    }
}