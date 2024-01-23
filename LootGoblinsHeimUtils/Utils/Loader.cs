using System;
using System.Runtime.CompilerServices;
using Jotunn;

namespace LootGoblinsUtils.Utils;

public static class Loader
{
    public static void RunSafe(Action target, string name = null, [CallerMemberName] string caller = "")
    {
        try
        {
            Logger.LogInfo($"{name ?? caller} start loading ...");
            target();
            Logger.LogInfo($"{name ?? caller} loading completed");
        }
        catch (Exception e)
        {
            Logger.LogError($"{name ?? caller} loading failed");
            Logger.LogError(e);
        }
    }
}