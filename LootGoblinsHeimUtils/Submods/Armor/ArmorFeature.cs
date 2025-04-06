using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using LootGoblinsUtils.Utils;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Submods.Armor;

public static class ArmorFeature
{
    public static bool IsItemsRegistered;

    public static void Init()
    {
        if (!ArmorFeatureConfiguration.ArmorFeatureToggle.Value) return;

        ItemManager.OnItemsRegistered += ItemManagerOnOnItemsRegistered;
    }

    private static void ItemManagerOnOnItemsRegistered()
    {
        IsItemsRegistered = true;
        UpdateArmor();
    }

    private static void SetupItemEffects(string prefabName, StatusEffect setEffect, int counter,
        StatusEffect burdenEffect = null, ItemOverrides.OverrideModifiers itemMod = null,
        List<RequirementConfig> recipeMod = null)
    {
        var item = PrefabManager.Cache.GetPrefab<ItemDrop>(prefabName);
        item.m_itemData.m_shared.m_setStatusEffect = setEffect;
        item.m_itemData.m_shared.m_setName = $"{setEffect.m_name} {counter.ToRoman()}";
        item.m_itemData.m_shared.m_setSize = 3;


        if (itemMod != null)
        {
            Logger.LogInfo($"itemMod for {prefabName}: {SimpleJson.SimpleJson.SerializeObject(itemMod)}");
            ItemOverrides.ApplyModifiers(item.m_itemData.m_shared, itemMod);
        }

        Logger.LogInfo($"recipemod for {prefabName}: {SimpleJson.SimpleJson.SerializeObject(recipeMod)}");
        if (recipeMod != null)
        {
            var originalRecipe = ObjectDB.instance.m_recipes.Find(rec => rec?.m_item?.name?.Contains(prefabName) ?? false);

            Logger.LogInfo($"Get recipe for ${prefabName}; found: {originalRecipe?.name}");
            
            if (originalRecipe != null)
            {
                ObjectDB.instance.m_recipes.Remove(originalRecipe);

                var newRecipe = new RecipeConfig();
                newRecipe.CraftingStation = "Armory_TW";
                newRecipe.Item = prefabName;
                newRecipe.RepairStation = "Armory_TW";
                newRecipe.MinStationLevel = originalRecipe.m_minStationLevel;
                newRecipe.Requirements = recipeMod.ToArray();

                var gameRecipe = newRecipe.GetRecipe();
                gameRecipe.FixReferences();
                
                foreach (var gameRecipeMResource in gameRecipe.m_resources)
                {
                    gameRecipeMResource.FixReferences();
                }
                
                Logger.LogInfo($"[${prefabName}] reqs: {gameRecipe.m_resources.Length}");
                foreach (var gameRecipeMResource in gameRecipe.m_resources)
                {
                    Logger.LogInfo($"{gameRecipeMResource.m_resItem?.name}; {gameRecipeMResource?.m_resItem?.m_itemData?.m_shared?.m_name}");
                }
                
            
                ObjectDB.instance.m_recipes.Add(gameRecipe);
            }
        }
        

        if (burdenEffect != null)
        {
            item.m_itemData.m_shared.m_equipStatusEffect = burdenEffect;
        }
    }

    public static void UpdateArmor()
    {
        if (!IsItemsRegistered) return;

        ProcessTier(ArmorFeatureConfiguration.Config.silver, ArmorEffectGenerator.SilverArmor);
        ProcessTier(ArmorFeatureConfiguration.Config.bm, ArmorEffectGenerator.BmArmor);

        foreach (var recipe in ObjectDB.instance.m_recipes.Take(10))
        {
            Logger.LogWarning($"rec: n1: {recipe?.m_item?.name}; n2: {recipe?.m_item?.m_itemData?.m_shared?.m_name}");
        }

        ItemManager.OnItemsRegistered -= UpdateArmor;
    }

    private static void ProcessTier(ArmorTierConfig tier, ArmorEffectGenerator.ArmorEffectsClasses tierClass)
    {
        for (var i = 0; i < tier.setList.Count; i++)
        {
            var armorSetConfig = tier.setList[i];
            try
            {
                Logger.LogInfo($"Processing {SimpleJson.SimpleJson.SerializeObject(armorSetConfig)}");
                var armorEffect = tierClass.GetClassByName(armorSetConfig.type);
                var itemMod = tier.GetItemOverridesFromClass(armorSetConfig.type);
                var recipe = tier.GetRecipeFromConfig(armorSetConfig);
                
                
                
                SetupItemEffects(armorSetConfig.head, armorEffect.SetEffect, i + 1, armorEffect.HeadEffect, itemMod,
                    recipe.HeadOriginal);
                SetupItemEffects(armorSetConfig.chest, armorEffect.SetEffect, i + 1, armorEffect.ChestEffect, itemMod,
                    recipe.ChestOriginal);
                SetupItemEffects(armorSetConfig.legs, armorEffect.SetEffect, i + 1, armorEffect.LegEffect, itemMod,
                    recipe.LegsOriginal);
            }
            catch (Exception e)
            {
                Logger.LogError($"Trying to process {SimpleJson.SimpleJson.SerializeObject(armorSetConfig)}");
                Logger.LogError(e);
            }
        }
    }
}