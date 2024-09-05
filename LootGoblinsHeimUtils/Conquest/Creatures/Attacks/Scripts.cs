using UnityEngine;

namespace LootGoblinsUtils.Conquest.Creatures.Attacks;

public class Scripts
{
    public void test()
    {
        var target = GameObject.Find("LG_StormHog(Clone)");
        var humanoid = target.GetComponent<Humanoid>();
        var item = humanoid.m_defaultItems[0].GetComponent<ItemDrop>();

        Debug.Log(JsonUtility.ToJson(item));
    }
}