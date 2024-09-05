using System.Linq;
using Jotunn;
using LootGoblinsUtils.Utils;

namespace LootGoblinsUtils.Submods.Farming;

public static class HoePatch
{
    public static void Patch()
    {
        Loader.RunSafe(RemoveHoeRecipe, "HoePatch");
    }

    private static void RemoveHoeRecipe()
    {
        var recipe = ObjectDB.instance.m_recipes.FirstOrDefault(x => x?.m_item?.name == "Hoe");
        if (recipe != null)
        {
            recipe.m_enabled = false;
        }
    }
}