using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Logger = Jotunn.Logger;
using Object = UnityEngine.Object;

namespace LootGoblinsUtils.Hooks;

public static class ProfilePatcher
{
    private static HarmonyMethod _prefix = new HarmonyMethod(typeof(ProfilePatcher).GetMethod("Prefix"))
    {
        priority = Priority.First
    };

    private static HarmonyMethod _postfixUpdate = new HarmonyMethod(typeof(ProfilePatcher).GetMethod("PostfixUpdate"))
    {
        priority = Priority.Last
    };

    private static HarmonyMethod _postfixFixedUpdate =
        new HarmonyMethod(typeof(ProfilePatcher).GetMethod("PostfixFixedUpdate"))
        {
            priority = Priority.Last
        };

    private static HarmonyMethod _postfixLateUpdate =
        new HarmonyMethod(typeof(ProfilePatcher).GetMethod("PostfixLateUpdate"))
        {
            priority = Priority.Last
        };

    private static HarmonyMethod _postfixCustomUpdate =
        new HarmonyMethod(typeof(ProfilePatcher).GetMethod("PostfixCustomUpdate"))
        {
            priority = Priority.Last
        };
    
    private static HarmonyMethod _postfixCustomFixedUpdate =
        new HarmonyMethod(typeof(ProfilePatcher).GetMethod("PostfixCustomFixedUpdate"))
        {
            priority = Priority.Last
        };

    public static void Patch(Harmony harmony)
    {
        // PatchBody(harmony, _postfixUpdate, "Update");
        PatchBody(harmony, _postfixFixedUpdate, "FixedUpdate");
        // PatchBody(harmony, _postfixLateUpdate, "LateUpdate");
        // PatchBody(harmony, _postfixCustomUpdate, "CustomUpdate");
        // PatchBody(harmony, _postfixCustomFixedUpdate, "CustomFixedUpdate");
    }

    private static void PatchBody(Harmony harmony, HarmonyMethod postfix, string methodName)
    {
        var updateTypeInfos = Assembly.GetAssembly(typeof(Game)).DefinedTypes
            .Where(x => x.DeclaredMethods.Any(t => t.Name == methodName));
        foreach (var methodInfo in updateTypeInfos.Select(
                     x => x.DeclaredMethods.First(m => m.Name == methodName)))
        {
            try
            {
                harmony.Patch(methodInfo, prefix: _prefix, postfix: postfix);
            }
            catch
            {
                Logger.LogWarning($"Cant patch [{methodInfo.Name}]");
            }
        }
    }


    public static Dictionary<string, List<long>> UpdateTable = new();
    public static Dictionary<string, List<int>> InstanceCounter = new();

    public static void Prefix(object __instance, out Stopwatch __state)
    {
        if (__instance == null)
        {
            __state = null;
        }
        else
        {
            __state = new Stopwatch(); // assign your own state
            __state.Start();
        }
    }

    public static void PostfixUpdate(object __instance, Stopwatch __state)
    {
        PostfixBody(__instance, __state, "Update");
    }

    public static void PostfixFixedUpdate(object __instance, Stopwatch __state)
    {
        PostfixBody(__instance, __state, "FixedUpdate");
    }

    public static void PostfixLateUpdate(object __instance, Stopwatch __state)
    {
        PostfixBody(__instance, __state, "LateUpdate");
    }

    public static void PostfixCustomUpdate(object __instance, Stopwatch __state)
    {
        PostfixBody(__instance, __state, "CustomUpdate");
    }
    
    public static void PostfixCustomFixedUpdate(object __instance, Stopwatch __state)
    {
        PostfixBody(__instance, __state, "CustomFixedUpdate");
    }

    public static void PushUpdateTable(string instanceName, long elapsed, int instanceId)
    {
        if (!LootGoblinsHeimUtilsPlugin.LoggingEnabled) return;
        
        if (InstanceCounter.TryGetValue(instanceName, out var counterList))
        {
            if (!counterList.Contains(instanceId)) counterList.Add(instanceId);
        }
        else
        {
            InstanceCounter[instanceName] = new List<int>
            {
                instanceId
            };
        }

        if (UpdateTable.TryGetValue(instanceName, out var profilerData))
        {
            profilerData.Add(elapsed);
        }
        else
        {
            UpdateTable[instanceName] = new List<long>
            {
                elapsed
            };
        }
    }

    public static void PostfixBody(object __instance, Stopwatch __state, string methodName)
    {
        if (__state != null && __instance != null)
        {
            __state.Stop();

            var instanceName = $"[{__instance.GetType().Name}].{methodName}";
            var elapsed = __state.ElapsedTicks;

            try
            {
                var instanceId  = ((Object) __instance).GetInstanceID();
                PushUpdateTable(instanceName, elapsed, instanceId);
            }
            catch
            {
                Logger.LogDebug($"One of {methodName} cant be pushed");
            }

            
        }
    }

    
    private static float _lastUpdateLate;
    private const float UpdateRate = 1f;

    private static void LogProfileInfo(string methodName)
    {
        var updatesList = UpdateTable[methodName];
        var instanceCount = InstanceCounter[methodName].Count;
        // var min = updatesList.Min();
        var max = updatesList.Max();
        var sum = updatesList.Sum() / UpdateRate;
        var count = updatesList.Count / UpdateRate;
        var sumMs = sum / 10000L;
        var sumPerUpdate = sumMs / (count * instanceCount);

        var headerText = fss($"{methodName}[{instanceCount}]:", 45);
        var maxText = fss($"max: {max};", 11);
        var sumTicksText = fss($"sumTicks:{sum};", 20);
        var sumMsText = fss($"sumMs:{sumMs:F};", 11);
        var sumMsPerUpdateText = fss($"sumMsPerUpdate:{sumPerUpdate:F};", 22);

        Logger.LogMessage(
            $"{headerText}\t {maxText}\t {sumTicksText}\t {sumMsText}\t {sumMsPerUpdateText}\t count: {count:0000}\t"
        );
    }
    

    private static string fss(string target, int requiredLength)
    {
        var requiredSymbols = requiredLength - target.Length;
        return requiredSymbols > 0 ? target + new string(' ', requiredSymbols) : target;
    }
    public static void LateUpdate()
    {
        if (Time.time > _lastUpdateLate + UpdateRate)
        {
            _lastUpdateLate = Time.time;
                
            Logger.LogMessage("------------");
            foreach (var keyValuePair in UpdateTable
                         .OrderByDescending(
                             x => x.Value.Sum() / (x.Value.Count * InstanceCounter[x.Key].Count)
                         )
                         .Take(15))
            {
                LogProfileInfo(keyValuePair.Key);
            }

            UpdateTable.Clear();
        }
    }
}