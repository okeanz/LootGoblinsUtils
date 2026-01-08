using System.Text;
using HarmonyLib;

namespace LootGoblinsUtils.Submods.ReactiveGear.GearSets.Hooks;

[HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetTooltip), typeof(ItemDrop.ItemData), typeof(int),
    typeof(bool), typeof(float), typeof(int))]
public static class ItemTooltipPatch
{
    private const string UpperLine =
        "\n\n<color=#555555>\u256d\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u256e</color><indent=8%>";

    private const string LowerLine =
        "\n<color=#555555>\u2570\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u256f</color>";

    static void Postfix(ItemDrop.ItemData item, int qualityLevel, bool crafting, float worldLevel, int stackOverride,
        ref string __result)
    {
        if (item?.m_dropPrefab == null) return;

        var prefabName = item.m_dropPrefab.name;

        if (!GearSetRegistry.SetsByPrefab.TryGetValue(prefabName, out var setDefinition))
            return;

        GearSetRegistry.EquippedSetCounterBySetName.TryGetValue(setDefinition.name, out var equipmentCounter);

        if (!GearSetRegistry.EffectDescriptionsByEffectName.TryGetValue(setDefinition.effectBinding,
                out var effectDescription)) return;


        var sb = new StringBuilder();


        __result = "<size=80%>" + __result;

        sb.Append(UpperLine);

        effectDescription.ForEach(line => sb.Append(line));

        if (equipmentCounter != null)
            sb.Append($"\n\nНадето: {equipmentCounter.EquippedCount} из {equipmentCounter.MaxCount}");

        sb.Append("</indent>");
        sb.Append(LowerLine);


        __result += sb.ToString();
    }
}