using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace LootGoblinsUtils.Hooks;

[HarmonyPatch(typeof(MonoUpdaters), nameof(MonoUpdaters.Update))]
public class MonoUpdaterPatch
{
    public static bool Prefix(MonoUpdaters __instance)
    {
        if (!LootGoblinsHeimUtilsPlugin.OptimizationsEnabled) return true;

        var watch = Stopwatch.StartNew();
        ++MonoUpdaters.s_updateCount;
        var deltaTime = Time.deltaTime;
        __instance.m_waterVolumeInstances.AddRange(WaterVolume.Instances);
        __instance.m_smokeInstances.AddRange(Smoke.Instances);
        __instance.m_zsfxInstances.AddRange(ZSFX.Instances);
        __instance.m_heightmapInstances.AddRange(Heightmap.Instances);
        __instance.m_visEquipmentInstances.AddRange(VisEquipment.Instances);
        __instance.m_footStepInstances.AddRange(FootStep.Instances);
        __instance.m_instanceRendererInstances.AddRange(InstanceRenderer.Instances);
        __instance.m_waterTriggerInstances.AddRange(WaterTrigger.Instances);
        watch.Stop();
        //ProfilePatcher.PushUpdateTable("MonoUpdater.Lists", watch.ElapsedTicks);
        watch.Restart();
        if (__instance.m_waterVolumeInstances.Count > 0)
        {
            __instance.SafeCall(WaterVolume.StaticUpdate);
            foreach (var waterVolumeInstance in __instance.m_waterVolumeInstances)
                waterVolumeInstance.Update1();
            foreach (var waterVolumeInstance in __instance.m_waterVolumeInstances)
                waterVolumeInstance.Update2();
        }

        watch.Stop();
        //ProfilePatcher.PushUpdateTable("MonoUpdater.Water", watch.ElapsedTicks);
        watch.Restart();

        foreach (var smokeInstance in __instance.m_smokeInstances)
            smokeInstance.CustomUpdate(deltaTime);
        foreach (var zsfxInstance in __instance.m_zsfxInstances)
            zsfxInstance.CustomUpdate(deltaTime);
        foreach (var equipmentInstance in __instance.m_visEquipmentInstances)
            equipmentInstance.CustomUpdate();
        foreach (var footStepInstance in __instance.m_footStepInstances)
            footStepInstance.CustomUpdate(deltaTime);
        foreach (var rendererInstance in __instance.m_instanceRendererInstances)
            rendererInstance.CustomUpdate();
        foreach (var waterTriggerInstance in __instance.m_waterTriggerInstances)
            waterTriggerInstance.CustomUpdate(deltaTime);

        watch.Stop();
        //ProfilePatcher.PushUpdateTable("MonoUpdater.Instances", watch.ElapsedTicks);

        __instance.m_waterVolumeInstances.Clear();
        __instance.m_smokeInstances.Clear();
        __instance.m_zsfxInstances.Clear();
        __instance.m_heightmapInstances.Clear();
        __instance.m_visEquipmentInstances.Clear();
        __instance.m_footStepInstances.Clear();
        __instance.m_instanceRendererInstances.Clear();
        __instance.m_waterTriggerInstances.Clear();
        return false;
    }
}

[HarmonyPatch(typeof(MonoUpdaters), nameof(MonoUpdaters.FixedUpdate))]
public class MonoUpdaterPatchFixed
{
    public static bool Prefix(MonoUpdaters __instance)
    {
        if (!LootGoblinsHeimUtilsPlugin.OptimizationsEnabled) return true;
        var instanceId = __instance.GetInstanceID();
        var localWatch = Stopwatch.StartNew();
        ++MonoUpdaters.s_updateCount;
        var fixedDeltaTime = Time.fixedDeltaTime;
        var time = Time.time;
        if (WaterVolume.Instances.Count > 0)
            WaterVolume.StaticUpdate();
        localWatch.Stop();
        ProfilePatcher.PushUpdateTable("MonoUpdaters.WaterVolume", localWatch.ElapsedTicks, instanceId);
        localWatch.Restart();
        foreach (var transformInstance in ZSyncTransform.Instances)
            transformInstance.CustomFixedUpdate(fixedDeltaTime);
        foreach (var animationInstance in ZSyncAnimation.Instances)
            animationInstance.CustomFixedUpdate();
        foreach (var floatingInstance in Floating.Instances)
            floatingInstance.CustomFixedUpdate(fixedDeltaTime);
        foreach (var shipInstance in Ship.Instances)
            shipInstance.CustomFixedUpdate(fixedDeltaTime, time);
        foreach (var fishInstance in Fish.Instances)
            fishInstance.CustomFixedUpdate();
        foreach (var animEventInstance in CharacterAnimEvent.Instances)
            animEventInstance.CustomFixedUpdate();

        localWatch.Stop();
        ProfilePatcher.PushUpdateTable("MonoUpdaters.Instances", localWatch.ElapsedTicks, instanceId);
        localWatch.Restart();

        __instance.m_updateAITimer += fixedDeltaTime;
        if (__instance.m_updateAITimer >= 0.0500000007450581)
        {
            foreach (var baseAiInstance in BaseAI.Instances)
                baseAiInstance.UpdateAI(0.05f);
            foreach (var monsterAiInstance in MonsterAI.Instances)
                monsterAiInstance.UpdateAI(0.05f);
            foreach (var animalAiInstance in AnimalAI.Instances)
                animalAiInstance.UpdateAI(0.05f);
            __instance.m_updateAITimer -= 0.05f;
        }

        localWatch.Stop();
        ProfilePatcher.PushUpdateTable("MonoUpdaters.AI", localWatch.ElapsedTicks, instanceId);
        localWatch.Restart();

        foreach (var humanoidInstance in Humanoid.Instances)
            humanoidInstance.CustomFixedUpdate();

        localWatch.Stop();
        ProfilePatcher.PushUpdateTable("MonoUpdaters.Humanoid", localWatch.ElapsedTicks, instanceId);
        localWatch.Restart();

        CharacterUpdater.Update(fixedDeltaTime);

        localWatch.Stop();
        ProfilePatcher.PushUpdateTable("MonoUpdaters.Character", localWatch.ElapsedTicks, instanceId);
        return false;
    }

    private static readonly SeparateUpdater<Character> CharacterUpdater = new (
        (character, dt) => character.CustomFixedUpdate(dt),
        Character.Instances
    );
    
    

    public class SeparateUpdater<T>
    {
        private readonly Action<T, float> _updater;
        private readonly List<T> _targets;
        private bool _firstUpdated;
        private float _lastDt;

        public SeparateUpdater(Action<T, float> updater, List<T> targets)
        {
            _updater = updater;
            _targets = targets;
        }
        
        public void Update(float fixedDeltaTime)
        {
            var toUpdate = _firstUpdated
                ? _targets.Skip(Character.Instances.Count / 2)
                : _targets.Take(Character.Instances.Count / 2);
            foreach (var characterInstance in toUpdate)
                _updater(characterInstance, fixedDeltaTime + _lastDt);
            _lastDt = fixedDeltaTime;
            _firstUpdated = !_firstUpdated;
        }
    }
}