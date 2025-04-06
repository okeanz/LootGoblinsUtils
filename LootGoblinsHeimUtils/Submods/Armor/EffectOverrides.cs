using System;

namespace LootGoblinsUtils.Submods.Armor;

public static class EffectOverrides
{
    public static OverrideModifiers LightSet = new()
    {
        ModifyAttackDamage = 1.15f,
        RunStaminaModifier = -0.05f,
        JumpStaminaModifier = -0.05f,
        RegenStaminaModifier = 1.1f,
        CarryWeightAdd = 40
    };

    public static OverrideModifiers MediumSet = new()
    {
        ModifyAttackDamage = 1.1f,
        RegenStaminaModifier = 1.1f,
        RegenHealthModifier = 1.1f,
        MovementSpeedModifier = 0.1f
    };

    public static OverrideModifiers MediumBurden = new()
    {
        EitrRegenMultiplier = 0.85f,
        SkillLevel1 = Skills.SkillType.Bows,
        SkillLevel1Value = -10,
        NoiseModifier = 0.1f,
        StealthModifier = -0.1f,
        CarryWeightAdd = -10
    };

    public static OverrideModifiers HeavySet = new()
    {
        RegenHealthModifier = 1.2f,
        MovementSpeedModifier = 0.1f,
        SkillLevel1 = Skills.SkillType.Blocking,
        SkillLevel1Value = 20
    };
    
    public static OverrideModifiers HeavyBurden = new()
    {
        EitrRegenMultiplier = 0.7f,
        SkillLevel1 = Skills.SkillType.Bows,
        SkillLevel1Value = -20,
        NoiseModifier = 0.2f,
        StealthModifier = -0.2f,
        CarryWeightAdd = -20
    };

    public static void ApplyStats(SE_Stats effect, OverrideModifiers overrideModifiersData)
    {
        effect.m_healthRegenMultiplier = 1;
        effect.m_staminaRegenMultiplier = 1;
        effect.m_eitrRegenMultiplier = 1;
        effect.m_raiseSkill = Skills.SkillType.None;
        effect.m_raiseSkillModifier = 0;
        effect.m_skillLevel = Skills.SkillType.None;
        effect.m_skillLevel2 = Skills.SkillType.None;
        effect.m_modifyAttackSkill = Skills.SkillType.None;
        effect.m_damageModifier = 1;
        effect.m_skillLevelModifier = 0;
        effect.m_skillLevelModifier2 = 0;
        effect.m_noiseModifier = 0;
        effect.m_stealthModifier = 0;
        effect.m_addMaxCarryWeight = 0;
        effect.m_speedModifier = 0;
        
        if (overrideModifiersData.ModifyAttackDamage.HasValue)
        {
            effect.m_modifyAttackSkill = Skills.SkillType.All;
            effect.m_damageModifier = overrideModifiersData.ModifyAttackDamage.Value;
            effect.m_tooltip = $"Урон: <color=orange>+{(effect.m_damageModifier - 1) * 100}%</color>";
        }

        if (overrideModifiersData.CarryWeightAdd.HasValue)
        {
            effect.m_addMaxCarryWeight = overrideModifiersData.CarryWeightAdd.Value;
        }

        if (overrideModifiersData.RegenStaminaModifier.HasValue)
        {
            effect.m_staminaRegenMultiplier = overrideModifiersData.RegenStaminaModifier.Value;
        }

        if (overrideModifiersData.RunStaminaModifier.HasValue)
        {
            effect.m_runStaminaDrainModifier = overrideModifiersData.RunStaminaModifier.Value;
        }

        if (overrideModifiersData.JumpStaminaModifier.HasValue)
        {
            effect.m_jumpStaminaUseModifier = overrideModifiersData.JumpStaminaModifier.Value;
        }

        if (overrideModifiersData.RegenHealthModifier.HasValue)
        {
            effect.m_healthRegenMultiplier = overrideModifiersData.RegenHealthModifier.Value;
        }

        if (overrideModifiersData.MovementSpeedModifier.HasValue)
        {
            effect.m_speedModifier = overrideModifiersData.MovementSpeedModifier.Value;
        }

        if (overrideModifiersData.EitrRegenMultiplier.HasValue)
        {
            effect.m_eitrRegenMultiplier = overrideModifiersData.EitrRegenMultiplier.Value;
        }

        if (overrideModifiersData.NoiseModifier.HasValue)
        {
            effect.m_noiseModifier = overrideModifiersData.NoiseModifier.Value;
        }

        if (overrideModifiersData.StealthModifier.HasValue)
        {
            effect.m_stealthModifier = overrideModifiersData.StealthModifier.Value;
        }

        if (overrideModifiersData.SkillLevel1.HasValue && overrideModifiersData.SkillLevel1Value.HasValue)
        {
            effect.m_skillLevel = overrideModifiersData.SkillLevel1.Value;
            effect.m_skillLevelModifier = overrideModifiersData.SkillLevel1Value.Value;
        }
    }

    [Serializable]
    public class OverrideModifiers
    {
        public float? ModifyAttackDamage;
        public float? RunStaminaModifier;
        public float? JumpStaminaModifier;
        public float? RegenStaminaModifier;
        public float? RegenHealthModifier;
        public float? MovementSpeedModifier;
        public float? EitrRegenMultiplier;
        public float? NoiseModifier;
        public float? StealthModifier;
        public int? CarryWeightAdd;
        public Skills.SkillType? SkillLevel1;
        public int? SkillLevel1Value;
    }
}