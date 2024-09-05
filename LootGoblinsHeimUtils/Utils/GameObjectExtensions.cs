using System.Collections.Generic;
using System.Linq;
using LootGoblinsUtils.Conquest.Creatures;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Utils;

public static class GameObjectExtensions
{
    public static void SetHuntCharacter(this GameObject monster, Player player)
    {
        if (monster == null)
        {
            Logger.LogWarning($"{monster.name}.SetHuntCharacter: monster == null");
            return;
        }
        var monsterAi = monster.GetComponent<MonsterAI>();
        if (monsterAi == null)
        {
            Logger.LogWarning($"{monster.name}.SetHuntCharacter: monsterAi == null");
            return;
        }
        monsterAi.SetHuntPlayer(true);
        monsterAi.SetTarget(player);
        monsterAi.SetPatrolPoint();
    }

    public static void SetRendererColor(this GameObject target, Color color)
    {
        if (target == null)
        {
            Logger.LogError($"SetRendererColor target == null");
            return;
        }
        var renderer = target.transform.GetComponentInChildren<MeshRenderer>();
        if (renderer == null)
        {
            Logger.LogError($"SetRendererColor renderer == null");
            return;
        }
        renderer.sharedMaterial.color = color;
    }

    public static GameObject AddLightningEffect(this GameObject target, Vector3 fxScale)
    {
        var lightningFx = FinePrefabs.LightningUnitEffectCloned;
        
        var fx = Object.Instantiate(lightningFx, target.transform);

        fx.transform.localPosition = Vector3.zero;
        fx.transform.localScale = fxScale;
        return target;
    }

    public static void DestroyIfExists(this GameObject target, string name)
    {
        var newModel = target.transform.Find(name);
        if(newModel != null) Object.Destroy(newModel.gameObject);
    }

    public static GameObject ClosestToPoint(this IEnumerable<GameObject> list, Vector3 point)
    {
        var gameObjects = list as GameObject[] ?? list.ToArray();
        if (!gameObjects.Any()) return null;
        return gameObjects
            .ToDictionary(x => x, x => Vector3.Distance(x.transform.position, point))
            .OrderBy(x => x.Value)
            .FirstOrDefault().Key;
    }

    public static bool PiecePlaced(this GameObject piece)
    {
        return piece.name.Contains("(Clone)");
    }
}