using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using LootGoblinsUtils.Utils;
using UnityEngine;

namespace LootGoblinsUtils.Conquest.Items;

public static class GreenCore
{
    public static void Load()
    {
        var blackCore = PrefabManager.Instance.CreateClonedPrefab("LGH_GreenCore2", "BlackCore");
        blackCore.SetRendererColor(Color.green);
        var icon = RenderManager.Instance.Render(new RenderManager.RenderRequest(blackCore)
        {
            Height = 64,
            Width = 64
        });

        var itemConfig = new ItemConfig
        {
            Name = "Зелёное ядро защитника",
            Description = "Источник энергии ядер защиты мира Вальхейм. Используйте, чтобы сразиться с хранителем биома",
            Icons = new[] {icon},
            CraftingStation = CraftingStations.None,
        };

        var customItem = new CustomItem("LGH_GreenCore", "BlackCore", itemConfig);

        customItem.ItemPrefab.SetRendererColor(Color.green);
        customItem.ItemPrefab.transform.GetChild(0).localScale *= 2f;
        customItem.ItemDrop.m_itemData.m_shared.m_weight = 25f;


        ItemManager.Instance.AddItem(customItem);
    }
}