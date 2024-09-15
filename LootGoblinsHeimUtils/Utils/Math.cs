using System;

namespace LootGoblinsUtils.Utils;

public static class ExtensionMethods
{
    public static int RoundOff (this float i)
    {
        return (int)(Math.Ceiling(i / 10.0d)*10);
    }
}