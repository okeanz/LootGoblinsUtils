using Jotunn.Configs;
using LootGoblinsUtils.Submods.Farming.Pieces.Configurators;

namespace LootGoblinsUtils.Submods.Farming.Pieces;

public static class BlueberryBush
{
    public static void Setup()
    {
        new BushConfiguration
        {
            Name = "Blueberries",
            LocalizedBushName = "Куст черники",
            ReadyObjectName = "Berrys",
            FertilizerIcon = BushUtils.BlueberryBushIcon,
            NewBushModel = BushUtils.BlueberryBushModel,
            BushRequirements = new[]
            {
                new RequirementConfig("Blueberries", 5),
                new RequirementConfig("BoneFragments", 3)
            },
            FertilizerConfigs = new[]
            {
                new BushConfiguration.FertilizerConfig
                {
                    LocalizedName = "Удобрение для черники",
                    LocalizedDescription = "Добавьте в куст черники, чтобы запустить процесс роста",
                    AmountProduced = 5,
                    RecipeRequirements = new[]
                    {
                        new RequirementConfig("BoneFragments", 2)
                    }
                },
                new BushConfiguration.FertilizerConfig
                {
                    LocalizedName = "Удобрение для черники насыщенное",
                    LocalizedDescription =
                        "Добавьте в куст черники, чтобы запустить процесс роста. Увеличивает количество получаемого продукта",
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