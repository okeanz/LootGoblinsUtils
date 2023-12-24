using HarmonyLib;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Hooks;

public static class Hooks
{
    [HarmonyPatch(typeof(Fermenter), nameof(Fermenter.GetHoverText))]
    public static class Bushes
    {
        public static bool Prefix(Fermenter __instance, ref string __result)
        {
            if (!__instance.gameObject.name.Contains("_LG"))
                return true;
            if (!PrivateArea.CheckAccess(__instance.transform.position, flash: false))
            {
                __result = "Доступ запрещён";
                return false;
            }

            var piece = __instance.GetComponent<Piece>();

            __result = __instance.GetStatus() switch
            {
                Fermenter.Status.Empty =>
                    Localization.instance.Localize($"{piece.m_name} ( Пусто )\n[<color=yellow><b>$KEY_Use</b></color>] Добавить удобрение"),
                Fermenter.Status.Fermenting => $"{piece.m_name} ( <color=blue>Рост</color> )",
                Fermenter.Status.Ready => $"{piece.m_name} ( <color=green>Готово</color> )",
                _ => piece.m_name
            };

            return false;

        }
    }

    [HarmonyPatch(typeof(Fermenter), nameof(Fermenter.UpdateCover))]
    public static class FermenterCoverSkip
    {
        static bool Prefix(Fermenter __instance)
        {
            return !__instance.gameObject.name.Contains("_LG");
        }
    }
    
    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.OnPlaced))]
    public static class WearNTearPlacedColliderIsTriggerSwap
    {
        static void Postfix(WearNTear __instance)
        {
            if(!__instance.gameObject.name.Contains("_LG"))
                return;

            // __instance.GetComponentInChildren<CapsuleCollider>().isTrigger = true;
        }
    }
}