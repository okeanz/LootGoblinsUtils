using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using Jotunn.Configs;

namespace LootGoblinsUtils.Submods.Armor;

[Serializable]
public class ArmorFeatureConfig
{
    public ArmorTierConfig silver = new();
    public ArmorTierConfig bm = new();
}

[Serializable]
public class ArmorTierConfig
{
    public EffectOverrides.OverrideModifiers lightSetEffect;
    public EffectOverrides.OverrideModifiers mediumSetEffect;
    public EffectOverrides.OverrideModifiers heavySetEffect;
    
    public EffectOverrides.OverrideModifiers lightBurdenEffect;    
    public EffectOverrides.OverrideModifiers mediumBurdenEffect;
    public EffectOverrides.OverrideModifiers heavyBurdenEffect;
    
    
    public ItemOverrides.OverrideModifiers heavyArmorMod;
    public ItemOverrides.OverrideModifiers mediumArmorMod;
    public ItemOverrides.OverrideModifiers lightArmorMod;

    public List<RequirementConfigInner> heavyArmorRecipe = new();
    public List<RequirementConfigInner> mediumArmorRecipe = new();
    public List<RequirementConfigInner> lightArmorRecipe = new();

    public List<ArmorSetConfig> setList = new();
    
    public EffectOverrides.OverrideModifiers GetBurdenEffectFromClass(string type)
    {
        switch (type)
        {
            case "heavy":
                return heavyBurdenEffect;
            case "medium":
                return mediumBurdenEffect;
            default:
                return lightBurdenEffect;
        }
    }
    
    public EffectOverrides.OverrideModifiers GetSetEffectFromClass(string type)
    {
        switch (type)
        {
            case "heavy":
                return heavySetEffect;
            case "medium":
                return mediumSetEffect;
            default:
                return lightSetEffect;
        }
    }

    public ItemOverrides.OverrideModifiers GetItemOverridesFromClass(string type)
    {
        switch (type)
        {
            case "heavy":
                return heavyArmorMod;
            case "medium":
                return mediumArmorMod;
            default:
                return lightArmorMod;
        }
    }

    public ArmorSetRecipe GetRecipeFromConfig(ArmorSetConfig config)
    {
        var baseRecipe = config.type switch
        {
            "heavy" => heavyArmorRecipe,
            "medium" => mediumArmorRecipe,
            _ => lightArmorRecipe
        };

        return new ArmorSetRecipe
        {
            Head = config.headCustomRecipeItem != null
                ? baseRecipe.Append(config.headCustomRecipeItem).ToList()
                : baseRecipe,
            Chest = config.chestCustomRecipeItem != null
                ? baseRecipe.Append(config.chestCustomRecipeItem).ToList()
                : baseRecipe,
            Legs = config.legsCustomRecipeItem != null
                ? baseRecipe.Append(config.legsCustomRecipeItem).ToList()
                : baseRecipe
        };
    }
}

public class ArmorSetRecipe
{
    public List<RequirementConfigInner> Head = new();
    public List<RequirementConfigInner> Chest = new();
    public List<RequirementConfigInner> Legs = new();
    
    public List<RequirementConfig> HeadOriginal => Head.Select(x=> x.ToOriginal()).ToList();
    public List<RequirementConfig> ChestOriginal => Chest.Select(x=> x.ToOriginal()).ToList();
    public List<RequirementConfig> LegsOriginal => Legs.Select(x=> x.ToOriginal()).ToList();
}

[Serializable]
public class ArmorSetConfig
{
    public string head;
    public string chest;
    public string legs;

    public RequirementConfigInner headCustomRecipeItem;
    public RequirementConfigInner chestCustomRecipeItem;
    public RequirementConfigInner legsCustomRecipeItem;

    public string type = ArmorSetTypes.Light;
}

[Serializable]
public class RequirementConfigInner
{
    [Required]
    public string item;
    public int amount = 1;
    public int amountPerLevel;

    public RequirementConfig ToOriginal()
    {
        return new RequirementConfig(item, amount, amountPerLevel);
    }
}

public static class ArmorSetTypes
{
    public const string Light = "light";
    public const string Medium = "medium";
    public const string Heavy = "heavy";
}