using System.Collections.Generic;
using System.Linq;
using LootGoblinsUtils.Configuration;
using LootGoblinsUtils.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LootGoblinsUtils.Conquest.Components.Spawners;

public abstract class DefendersSpawner : SlowMono
{
    public virtual SpawnerTypes SpawnerType { get; }
    
    public States state = States.Passive;
    public States prevState = States.Passive;

    public string officerPrefabName;
    public string guardPrefabName;
    public GameObject officerPrefab;
    public GameObject guardPrefab;

    private readonly List<GameObject> _officersAlive = new();
    private readonly List<GameObject> _guardsAlive = new();

    private readonly List<Player> _playersInRangeAlert = new();
    private readonly List<Player> _playersInRangeDefense = new();

    public int? OfficerCountOverride;
    public int? GuardCountOverride;

    private static float SpawnRange => PluginConfiguration.Conquest.SpawnRadius.Value;
    private static float AlertRange => PluginConfiguration.Conquest.PlayerAlertRadius.Value;
    private static float DefendRange => PluginConfiguration.Conquest.DefendRadius.Value;
    private int GuardCountMax => GuardCountOverride ?? PluginConfiguration.Conquest.GuardMaxCount.Value;
    private int OfficerCountMax => OfficerCountOverride ?? PluginConfiguration.Conquest.OfficerMaxCount.Value;
    private static bool FeatureEnabled => PluginConfiguration.Conquest.ConquestFeature.Value;
    private static float SpawnRate => PluginConfiguration.Conquest.SpawnRate.Value;

    private float _lastSpawn;
    private ZNetView _nview;

    public StaticTarget forcedStaticTarget;


    private Minimap.PinData _pinAlert;
    private float _worldSizeMult = 200f / 75;

    private void Start()
    {
        _nview = GetComponent<ZNetView>();

        if (Minimap.instance.m_pins.All(pin => pin.m_pos != transform.position))
        {
            _pinAlert = Minimap.instance.AddPin(transform.position, Minimap.PinType.EventArea, "pinName", false, false);
            _pinAlert.m_worldSize = _worldSizeMult * AlertRange;
        }
    }

    private void OnDestroy()
    {
        if (_pinAlert != null)
            Minimap.instance.RemovePin(_pinAlert);
    }

    public override void SlowUpdate()
    {
        if (!_nview.IsValid() || !_nview.IsOwner()) return;
        if (!FeatureEnabled) return;
        if (officerPrefab == null || guardPrefab == null) return;
        Spawn();

        UpdateState();
        ProcessState();
    }

    private void RefreshPlayersList()
    {
        var position = transform.position;
        _playersInRangeAlert.Clear();
        _playersInRangeDefense.Clear();
        Player.GetPlayersInRange(position, AlertRange, _playersInRangeAlert);
        Player.GetPlayersInRange(position, DefendRange, _playersInRangeDefense);
    }

    private void RefreshGuardLists()
    {
        var position = transform.position;
        _officersAlive.Clear();
        _guardsAlive.Clear();

        var officersAround = BaseAI.m_instances.Where(x =>
            Vector3.Distance(x.transform.position, transform.position) < AlertRange * 2 &&
            x.m_nview.GetZDO().GetVec3("spawnerId", Vector3.zero) == position &&
            x.m_nview.GetPrefabName() == officerPrefabName
        ).Select(x => x.gameObject).ToArray();
        _officersAlive.AddRange(officersAround);

        var guardsAround = BaseAI.m_instances.Where(
            x => Vector3.Distance(x.transform.position, transform.position) < AlertRange * 2 &&
                 x.m_nview.GetZDO().GetVec3("spawnerId", Vector3.zero) == position &&
                 x.m_nview.GetPrefabName() == guardPrefabName &&
                 (x as MonsterAI)?.m_follow == null).Cast<MonsterAI>().ToArray();

        _guardsAlive.AddRange(guardsAround.Select(x => x.gameObject));

        SendGuardsToOfficers();
    }

    private void Spawn()
    {
        if (Time.time - _lastSpawn <= SpawnRate) return;
        if (_officersAlive.Count < OfficerCountMax)
        {
            SpawnOfficer();
            _lastSpawn = Time.time;
        }
    }

    private void SpawnOfficer()
    {
        var position = transform.position;

        var insideUnitCircle = Random.insideUnitCircle;
        var spawnPoint =
            position + new Vector3(insideUnitCircle.x, 0.0f, insideUnitCircle.y) * SpawnRange;

        var officer = Instantiate(officerPrefab, spawnPoint, Quaternion.identity);
        officer.GetComponent<ZNetView>().GetZDO().Set("spawnerId", position);
        Jotunn.Logger.LogInfo(
            $"[{_nview.GetZDO().m_uid}--{position}] Spawned officer[{officer.GetComponent<ZNetView>().GetZDO().m_uid}]");


        for (var i = 0; i < GuardCountMax; i++)
        {
            var randomCircle = Random.insideUnitCircle;
            var guard = Instantiate(guardPrefab, spawnPoint + new Vector3(randomCircle.x, 0.0f, randomCircle.y) * 5f,
                Quaternion.identity);
            var monsterAI = guard.GetComponent<MonsterAI>();
            guard.GetComponent<ZNetView>().GetZDO().Set("spawnerId", position);
            monsterAI.SetFollowTarget(officer);
        }
    }

    private void UpdateState()
    {
        RefreshPlayersList();
        RefreshGuardLists();
        switch (state)
        {
            case States.Passive:
                if (_playersInRangeAlert.Any())
                {
                    MessageHud.instance.ShowMessage(MessageHud.MessageType.Center,
                        "Защитник пробуждается ...");
                    SetState(States.Alert);
                    SendAlertPatrol();
                }

                break;
            case States.Alert:
                if (_playersInRangeDefense.Any())
                {
                    MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "Защитник пробудился !");
                    SetState(States.Defend);
                }

                if (!_playersInRangeAlert.Any())
                {
                    SetState(States.Passive);
                }

                break;
            case States.Defend:
                if (!_playersInRangeAlert.Any())
                {
                    SetState(States.Passive);
                }

                break;
        }
    }


    private void ProcessState()
    {
        switch (state)
        {
            case States.Passive:
                CalmDownGuards();
                break;
            case States.Alert:
                break;
            case States.Defend:
                SendAllToDefend();
                break;
        }
    }

    private void SendAllToDefend()
    {
        foreach (var guardAi in _guardsAlive.Select(x => x.GetComponent<MonsterAI>()))
        {
            guardAi.SetHuntPlayer(true);
        }

        if (forcedStaticTarget)
        {
            foreach (var officer in _officersAlive.Select(x => x.GetComponent<MonsterAI>()))
            {
                officer.m_targetCreature = null;
                officer.m_targetStatic = forcedStaticTarget;
                officer.m_attackPlayerObjects = true;
            }

            return;
        }

        var freeOfficers = _officersAlive.Select(x => x.GetComponent<MonsterAI>()).Where(x => !x.m_huntPlayer).ToList();
        if (!freeOfficers.Any()) return;


        var officersPerPlayer = Mathf.CeilToInt((float) _officersAlive.Count / _playersInRangeAlert.Count);
        foreach (var player in _playersInRangeAlert)
        {
            if (!freeOfficers.Any()) return;

            foreach (var officer in freeOfficers.Take(officersPerPlayer).ToArray())
            {
                var monsterAI = officer.GetComponent<MonsterAI>();
                monsterAI.SetHuntPlayer(true);
                monsterAI.SetTarget(player);

                freeOfficers.Remove(officer);
            }
        }
    }

    private void SendAlertPatrol()
    {
        var availableOfficers = new List<GameObject>(_officersAlive);
        if (availableOfficers.Count == 0)
        {
            Jotunn.Logger.LogMessage("No officers to patrol");
            return;
        }

        foreach (var player in _playersInRangeAlert)
        {
            if (availableOfficers.Count == 0) break;
            var position = player.transform.position;
            var officer = availableOfficers.ClosestToPoint(position);
            availableOfficers.Remove(officer);

            var monsterAI = officer.GetComponent<MonsterAI>();
            monsterAI.SetPatrolPoint(position);
            monsterAI.SetAlerted(true);
        }
    }

    private void CalmDownGuards()
    {
        foreach (var officerAi in _officersAlive.Select(x => x.GetComponent<MonsterAI>()))
        {
            officerAi.SetHuntPlayer(false);
            officerAi.SetAlerted(false);
            if (!officerAi.m_patrol)
                officerAi.SetPatrolPoint(RandomPointInsideSpawnArea());
        }

        foreach (var guardAi in _guardsAlive.Select(x => x.GetComponent<MonsterAI>()))
        {
            guardAi.SetHuntPlayer(false);
            guardAi.SetAlerted(false);
        }

        SendGuardsToOfficers();
    }

    private Vector3 RandomPointInsideSpawnArea()
    {
        var insideUnitCircle = Random.insideUnitCircle;
        return transform.position + new Vector3(insideUnitCircle.x, 0.0f, insideUnitCircle.y) * SpawnRange;
    }

    private void SetState(States target)
    {
        if (state != target)
            Jotunn.Logger.LogMessage($"spawner[{GetInstanceID()}]: {state.ToString()} => {target.ToString()}");
        prevState = state;
        state = target;
    }

    private void SendGuardsToOfficers()
    {
        if (!_officersAlive.Any() || !_guardsAlive.Any()) return;

        var freeGuards = _guardsAlive
            .Select(x => x.GetComponent<MonsterAI>())
            .Where(x => x.m_follow == null)
            .ToList();

        if (!freeGuards.Any()) return;

        foreach (var officer in _officersAlive)
        {
            if (!freeGuards.Any()) break;
            var guardToOfficerRatio = Mathf.CeilToInt((float) freeGuards.Count / _officersAlive.Count);
            foreach (var guardAi in freeGuards.Take(guardToOfficerRatio).ToArray())
            {
                guardAi.SetFollowTarget(officer);
                freeGuards.Remove(guardAi);
            }
        }
    }

    public enum States
    {
        Passive,
        Alert,
        Defend
    }

    public enum SpawnerTypes
    {
        Common,
        Raid
    }
}