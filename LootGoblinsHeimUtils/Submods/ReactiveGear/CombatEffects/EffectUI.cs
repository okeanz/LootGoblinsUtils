using Jotunn.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LootGoblinsUtils.Submods.ReactiveGear.CombatEffects;

public static class EffectUI
{
    private static GameObject _panel;
    private static Text _text;

    public static void InitUI()
    {
        if (GUIManager.IsHeadless()) return;
        GUIManager.OnCustomGUIAvailable += GUIManagerOnOnCustomGUIAvailable;
    }

    private static void GUIManagerOnOnCustomGUIAvailable()
    {
        if (SceneManager.GetActiveScene().name != "main") return;

        _panel = GUIManager.Instance.CreateWoodpanel(
            parent: GUIManager.CustomGUIFront.transform,
            anchorMin: new Vector2(0.5f, 0.5f),
            anchorMax: new Vector2(0.5f, 0.5f),
            position: new Vector2(200f, 0f),
            width: 200f,
            height: 200f,
            draggable: true);

        _text = GUIManager.Instance.CreateText(
            text: "Jötunn, the Valheim Lib",
            parent: _panel.transform,
            anchorMin: new Vector2(0.5f, 0.5f),
            anchorMax: new Vector2(0.5f, 0.5f),
            position: new Vector2(0f, 0f),
            font: GUIManager.Instance.AveriaSerifBold,
            fontSize: 15,
            color: GUIManager.Instance.ValheimOrange,
            outline: true,
            outlineColor: Color.black,
            width: 200f,
            height: 200f,
            addContentSizeFitter: false).GetComponent<Text>();
    }

    public static void SetText(string text)
    {
        _text.text = text;
    }
}