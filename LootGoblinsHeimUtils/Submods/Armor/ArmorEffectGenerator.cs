using System.IO;
using Jotunn.Utils;
using LootGoblinsUtils.Utils;
using UnityEngine;

namespace LootGoblinsUtils.Submods.Armor;

public static class ArmorEffectGenerator
{
    public static Sprite Icon =
        AssetUtils.LoadSpriteFromFile(Path.Combine(PathUtil.PluginFolder, "Assets", "statusiconwhite.png"));

    private static StatusEffect _baseEffect;

    public static StatusEffect GetBaseEffect
    {
        get { return _baseEffect ??= ObjectDB.instance.m_StatusEffects.Find(x => x.name == "SetEffect_TrollArmor"); }
    }

    public static ArmorEffectsClasses _silverArmor;
    public static ArmorEffectsClasses SilverArmor => _silverArmor ??= GenerateArmorEffectsClasses("Silver", "Серебро");
    
    public static ArmorEffectsClasses _bmArmor;
    public static ArmorEffectsClasses BmArmor => _bmArmor ??= GenerateArmorEffectsClasses("BM", "Чёрный металл");

    private static ArmorEffectsClasses GenerateArmorEffectsClasses(string name, string localizedName)
    {
        return new ArmorEffectsClasses
        {
            Light = new ArmorEffects
            {
                SetEffect = MakeEffect(
                    $"SE_{name}_Light_Set",
                    $"Лёгкая броня ({localizedName})",
                    EffectOverrides.LightSet
                )
            },
            Medium = new ArmorEffects
            {
                SetEffect = MakeEffect(
                    $"SE_{name}_Medium_Set",
                    $"Средняя броня ({localizedName})",
                    EffectOverrides.MediumSet
                ),
                HeadEffect = MakeEffect(
                    $"SE_{name}_Medium_Head",
                    "Средний шлем",
                    EffectOverrides.MediumBurden
                ),
                ChestEffect = MakeEffect(
                    $"SE_{name}_Medium_Chest",
                    "Средний нагрудник",
                    EffectOverrides.MediumBurden
                ),
                LegEffect = MakeEffect(
                    $"SE_{name}_Medium_Legs",
                    "Средние поножи",
                    EffectOverrides.MediumBurden
                ),
            },
            Heavy = new ArmorEffects
            {
                SetEffect = MakeEffect(
                    $"SE_{name}_Heavy_Set",
                    $"Тяжелая броня ({localizedName})",
                    EffectOverrides.HeavySet
                ),
                HeadEffect = MakeEffect(
                    $"SE_{name}_Heavy_Head",
                    "Тяжелый шлем",
                    EffectOverrides.HeavyBurden
                ),
                ChestEffect = MakeEffect(
                    $"SE_{name}_Heavy_Chest",
                    "Тяжелый нагрудник",
                    EffectOverrides.HeavyBurden
                ),
                LegEffect = MakeEffect(
                    $"SE_{name}_Heavy_Legs",
                    "Тяжелые поножи",
                    EffectOverrides.HeavyBurden
                ),
            }
        };
    }


    private static StatusEffect MakeEffect(string name, string localizedName,
        EffectOverrides.OverrideModifiers overrideModifiersData = null)
    {
        var setEffect = ScriptableObject.CreateInstance<SE_Stats>();

        MonoExtensions.CopyFields(GetBaseEffect, setEffect, typeof(SE_Stats));
        setEffect.name = name;
        setEffect.m_name = localizedName;
        setEffect.m_tooltip = "";
        setEffect.m_icon = Icon;

        if (overrideModifiersData != null)
        {
            EffectOverrides.ApplyStats(setEffect, overrideModifiersData);
        }

        ObjectDB.instance.m_StatusEffects.Add(setEffect);

        return setEffect;
    }

    public class ArmorEffects
    {
        public StatusEffect SetEffect;
        public StatusEffect HeadEffect;
        public StatusEffect ChestEffect;
        public StatusEffect LegEffect;
    }

    public class ArmorEffectsClasses
    {
        public ArmorEffects Heavy;
        public ArmorEffects Medium;
        public ArmorEffects Light;

        public ArmorEffects GetClassByName(string name)
        {
            switch (name)
            {
                case "heavy":
                    return Heavy;
                case "medium":
                    return Medium;
                default:
                    return Light;
            }
        }
    }
}