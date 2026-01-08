namespace LootGoblinsUtils.Utils;

public static class PlayerExtensions
{
    public static bool IsLocalPlayer(this Player p)
    {
        return p != null && Player.m_localPlayer != null && ReferenceEquals(p, Player.m_localPlayer);
    }
    
    public static bool IsLocalPlayer(this Humanoid p)
    {
        return p != null && Player.m_localPlayer != null && ReferenceEquals(p, Player.m_localPlayer);
    }
    
    public static bool IsLocalPlayer(this Character p)
    {
        return p != null && Player.m_localPlayer != null && ReferenceEquals(p, Player.m_localPlayer);
    }
}