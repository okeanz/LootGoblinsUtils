using HarmonyLib;
using LootGoblinsUtils.Configuration;

namespace LootGoblinsUtils.Submods.ReliableBlock;

[HarmonyPatch(typeof(Character), nameof(Character.AddStaggerDamage))]
public static class Hook
{
    public static void Postfix(ref bool __result)
    {
        if (PluginConfiguration.ReliableBlockToggle.Value)
            __result = false;
    }
}