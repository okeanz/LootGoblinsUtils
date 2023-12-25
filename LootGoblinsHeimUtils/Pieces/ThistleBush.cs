using Jotunn.Configs;

namespace LootGoblinsUtils.Pieces;

public static class ThistleBush
{
    public static void Setup()
    {
        new BushConfiguration
        {
            TargetItemName = "Thistle",
            LocalizedBushName = "Чертополох",
            ReadyObjectName = "bees",
            FertilizerIcon = BushUtils.thistleIcon,
            NewBushModel = BushUtils.thistleModel,
            BushRequirements = new[]
            {
                new RequirementConfig("Thistle", 5),
                new RequirementConfig("BoneFragments", 3)
            },
            FertilizerConfigs = new[]
            {
                new BushConfiguration.FertilizerConfig
                {
                    LocalizedName = "Удобрение для чертополоха",
                    LocalizedDescription = "Добавьте в чертополох, чтобы запустить процесс роста",
                    AmountProduced = 5,
                    RecipeRequirements = new[]
                    {
                        new RequirementConfig("BoneFragments", 2)
                    }
                },
                new BushConfiguration.FertilizerConfig
                {
                    LocalizedName = "Удобрение для чертополоха насыщенное",
                    LocalizedDescription =
                        "Добавьте в чертополох, чтобы запустить процесс роста. Увеличивает количество получаемого продукта",
                    AmountProduced = 10,
                    RecipeRequirements = new[]
                    {
                        new RequirementConfig("BoneFragments", 2),
                        new RequirementConfig("Entrails", 1)
                    }
                }
            }
        }.Configure();
    }
}