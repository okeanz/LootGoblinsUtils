using System;

namespace LootGoblinsUtils.Submods.TgDeath;

[Serializable]
public class DeathInfo
{
    public string playerName;
    public string deathType;
    public string attackerName;
    public int damage;


    public DeathInfo(HitData hitData, string playerName)
    {
        this.playerName = playerName;
        deathType = hitData.m_hitType.ToString();
        attackerName = hitData.GetAttacker()?.m_name != null
            ? Localization.instance.Localize(hitData.GetAttacker()?.m_name)
            : "Unknown";
        damage = (int)hitData.m_damage.GetTotalDamage();
    }

    public string ToJson()
    {
        return SimpleJson.SimpleJson.SerializeObject(this);
    }
}