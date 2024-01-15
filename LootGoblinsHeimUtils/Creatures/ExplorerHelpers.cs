using UnityEngine;

namespace LootGoblinsUtils.Creatures;

public static class ExplorerHelpers
{
    public static void AttackSharedData()
    {
        JsonUtility.ToJson(GameObject.Find("LG_StormBoar(Clone)").GetComponent<Humanoid>().m_defaultItems[0]
            .GetComponent<ItemDrop>().m_itemData.m_shared);
    }
}