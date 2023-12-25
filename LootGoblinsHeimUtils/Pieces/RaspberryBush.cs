﻿using System;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using LootGoblinsUtils.Pieces.Configurators;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LootGoblinsUtils.Pieces;

public static class RaspberryBush
{
    public static void Setup()
    {
        new BushConfiguration
        {
            Name = "Raspberry",
            LocalizedBushName = "Малиновый куст",
            ReadyObjectName = "Berrys",
            FertilizerIcon = BushUtils.RaspberryBushIcon,
            NewBushModel = BushUtils.RaspberryBushModel,
            BushRequirements = new[]
            {
                new RequirementConfig("Raspberry", 5),
                new RequirementConfig("Resin", 3)
            },
            FertilizerConfigs = new[]
            {
                new BushConfiguration.FertilizerConfig
                {
                    LocalizedName = "Удобрение для малины",
                    LocalizedDescription = "Добавьте в малиновый куст, чтобы запустить процесс роста",
                    AmountProduced = 5,
                    RecipeRequirements = new[]
                    {
                        new RequirementConfig("Resin", 3)
                    }
                },
                new BushConfiguration.FertilizerConfig
                {
                    LocalizedName = "Удобрение для малины насыщенное",
                    LocalizedDescription =
                        "Добавьте в малиновый куст, чтобы запустить процесс роста. Увеличивает количество получаемого продукта",
                    AmountProduced = 10,
                    RecipeRequirements = new[]
                    {
                        new RequirementConfig("Resin", 3),
                        new RequirementConfig("BoneFragments", 1)
                    }
                }
            }
        }.Configure();
    }
}