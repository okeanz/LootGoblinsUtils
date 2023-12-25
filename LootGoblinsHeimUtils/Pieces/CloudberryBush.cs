using Jotunn.Configs;
using LootGoblinsUtils.Pieces.Configurators;

namespace LootGoblinsUtils.Pieces;

public static class CloudberryBush
{
    public static void Setup()
    {
        new BushConfiguration
        {
            Name = "Cloudberry",
            LocalizedBushName = "Куст морошки",
            ReadyObjectName = "Berrys",
            ColliderRadius = 0.7f,
            FertilizerIcon = BushUtils.CloudberryVisuals.Icon,
            NewBushModel = BushUtils.CloudberryVisuals.Model,
            BushRequirements = new[]
            {
                new RequirementConfig("Cloudberry", 5),
                new RequirementConfig("Bloodbag", 2)
            },
            FertilizerConfigs = new[]
            {
                new BushConfiguration.FertilizerConfig
                {
                    LocalizedName = "Удобрение для морошки",
                    LocalizedDescription = "Добавьте в куст морошки, чтобы запустить процесс роста",
                    AmountProduced = 5,
                    RecipeRequirements = new[]
                    {
                        new RequirementConfig("Bloodbag", 3)
                    }
                },
                new BushConfiguration.FertilizerConfig
                {
                    LocalizedName = "Удобрение для морошки насыщенное",
                    LocalizedDescription =
                        "Добавьте в куст морошки, чтобы запустить процесс роста. Увеличивает количество получаемого продукта",
                    AmountProduced = 10,
                    RecipeRequirements = new[]
                    {
                        new RequirementConfig("Bloodbag", 3),
                        new RequirementConfig("Ooze", 1)
                    }
                }
            }
        }.Configure();
    }
}