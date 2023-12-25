﻿using System;
using JetBrains.Annotations;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LootGoblinsUtils.Pieces;

public static class BushUtils
{
    private static GameObject _raspberryBushPrefab;
    public static GameObject RaspberryBushModel;
    public static Sprite RaspberryBushIcon;

    private static GameObject _blueberryBushPrefab;
    public static GameObject BlueberryBushModel;
    public static Sprite BlueberryBushIcon;

    private static GameObject _thistlePrefab;
    public static GameObject thistleModel;
    public static Sprite thistleIcon;

    public static Sprite MeadIcon;

    public static void CacheDependencies()
    {
        _raspberryBushPrefab = PrefabManager.Instance.GetPrefab("RaspberryBush");
        RaspberryBushModel = _raspberryBushPrefab.FindDeepChild("model").gameObject;
        RaspberryBushIcon = RenderManager.Instance.Render(_raspberryBushPrefab, RenderManager.IsometricRotation);

        _blueberryBushPrefab = PrefabManager.Instance.GetPrefab("BlueberryBush");
        BlueberryBushModel = _blueberryBushPrefab.FindDeepChild("model").gameObject;
        BlueberryBushIcon = RenderManager.Instance.Render(_blueberryBushPrefab, RenderManager.IsometricRotation);


        _thistlePrefab = PrefabManager.Instance.GetPrefab("Pickable_Thistle");
        thistleModel = _thistlePrefab.FindDeepChild("visual").gameObject;
        thistleIcon = RenderManager.Instance.Render(_thistlePrefab, RenderManager.IsometricRotation);


        MeadIcon = PrefabManager.Instance.GetPrefab("MeadBaseTasty").GetComponent<ItemDrop>().m_itemData.GetIcon();
    }

    public static GameObject SetupNewModel(GameObject targetModel, Transform targetParentTransform)
    {
        var newModel = Object.Instantiate(targetModel, targetParentTransform);
        newModel.layer = LayerMask.NameToLayer("Default_small");

        var collider = newModel.GetComponent<CapsuleCollider>() ??
                       newModel.AddComponent<CapsuleCollider>();
        collider.radius = 1.6f;
        collider.isTrigger = true;

        return newModel;
    }

    public static Transform CleanupModel(CustomPiece newPiece)
    {
        var model = newPiece.PiecePrefab.FindDeepChild("barrel");
        model.gameObject.SetActive(false);

        var top = newPiece.PiecePrefab.FindDeepChild("_top");
        Object.Destroy(top.transform.GetChild(0).gameObject);

        var fermentingSFX = newPiece.PiecePrefab.FindDeepChild("_fermenting");
        foreach (Transform sfx in fermentingSFX)
        {
            Object.Destroy(sfx.gameObject);
        }

        var readySFX = newPiece.PiecePrefab.FindDeepChild("_ready");
        foreach (Transform sfx in readySFX)
        {
            Object.Destroy(sfx.gameObject);
        }

        return model.parent;
    }

    public static void ConfigureFermenter(Fermenter fermenterComponent, [CanBeNull] GameObject readyObject)
    {
        fermenterComponent.m_conversion.Clear();
        fermenterComponent.m_fermentationDuration = PluginConfiguration.FertilizingDuration.Value;
        fermenterComponent.m_addedEffects = fermenterComponent.m_spawnEffects;
        fermenterComponent.m_tapEffects.m_effectPrefabs = Array.Empty<EffectList.EffectData>();
        fermenterComponent.m_tapDelay = 0.1f;
        fermenterComponent.m_readyObject = readyObject;
    }

    public static void MakeFertilizer(string localizedName, string itemName, string description,
        RequirementConfig[] requirements)
    {
        var config = new ItemConfig
        {
            Name = localizedName,
            Description = description,
            CraftingStation = FarmersTable.TableName,
            Icons = new[] {MeadIcon}
        };
        config.Requirements = requirements;

        var customItem = new CustomItem(itemName, "Stone", config);
        Object.Destroy(customItem.ItemPrefab.transform.GetChild(0).gameObject);
        Object.Instantiate(PrefabManager.Instance.GetPrefab("MeadBaseTasty").transform.GetChild(0).gameObject,
            customItem.ItemPrefab.transform);
        ItemManager.Instance.AddItem(customItem);
    }
}