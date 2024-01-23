using System;
using System.Collections.Generic;
using System.Linq;
using Jotunn.Managers;
using LootGoblinsUtils.Configuration;
using LootGoblinsUtils.Conquest.Components.Spawners;
using LootGoblinsUtils.Conquest.Locations;
using LootGoblinsUtils.Conquest.Pieces.Core;
using LootGoblinsUtils.Conquest.Pieces.DefenderTotem;
using LootGoblinsUtils.Utils;
using UnityEngine;

namespace LootGoblinsUtils.Conquest.Pieces.Ghost;

public class ConquestGhost : SlowMono
{
    public Location location;

    private States _state;
    private States _prevState;

    private DefendersSpawner _spawnerRaid;
    private DefendersSpawner _spawnerT1;
    private bool _coreExists;
    private bool _defenderExists;
    private Piece _defender;

    private ZNetView _nview;

    private float _awakeTime;
    private float _defenderSetupTime;

    private static float DefenderRaidTime => PluginConfiguration.Conquest.DefenderRaidTime.Value;
    private static float NoBuildRadius => PluginConfiguration.Conquest.PlayerAlertRadius.Value;

    private void Awake()
    {
        var spawners = GetComponents<DefendersSpawner>();
        _spawnerRaid = spawners.FirstOrDefault(x => x.SpawnerType == DefendersSpawner.SpawnerTypes.Raid);
        SetRaidSpawnerParams(false, null);

        _spawnerT1 = spawners.FirstOrDefault(x => x.SpawnerType == DefendersSpawner.SpawnerTypes.Common);
        SetCommonSpawnerParams(false, null);

        UpdateStep = 3f;
        _awakeTime = Time.time;

        location = Location.m_allLocations.FirstOrDefault(x =>
            Vector3.Distance(transform.position, x.transform.position) < 20f);
        if (location != null)
        {
            location.m_noBuild = true;
            location.m_noBuildRadiusOverride = NoBuildRadius;
        }
    }

    private void Start()
    {
        _nview = GetComponent<ZNetView>();
        if (_nview != null && _nview.IsValid())
        {
            var disabled = _nview.GetZDO().GetBool("GhostDisabled");
            if (disabled)
            {
                gameObject.SetActive(false);
                enabled = false;
            }

            _state = (States) _nview.GetZDO().GetInt("ghost_state");
            Jotunn.Logger.LogInfo($"Ghost[{GetInstanceID()}]: Loaded state {_state}");
        }
    }

    public override void SlowUpdate()
    {
        if (Time.time - _awakeTime < 5f) return;
        location = Location.m_allLocations.FirstOrDefault(x =>
            Vector3.Distance(transform.position, x.transform.position) < 20f);
        UpdateState();
        ProcessStates();
    }

    private void UpdateState()
    {
        SearchCore();
        SearchDefenderTotem();
        switch (_state)
        {
            case States.CoreExists:
                if (!_coreExists)
                {
                    SetState(States.CoreDestroyed);
                }

                break;
            case States.CoreDestroyed:
                if (_defenderExists)
                {
                    MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "Защищайте тотем!");
                    _defenderSetupTime = Time.time;
                    SetState(States.DefenderExists);
                }

                break;
            case States.DefenderExists:
                if (!_defenderExists)
                {
                    MessageHud.instance.ShowMessage(MessageHud.MessageType.Center,
                        "Тотем уничтожен! Защитник восстановлен!");
                    RestoreCore();
                    return;
                }

                if (Time.time - _defenderSetupTime >= DefenderRaidTime)
                {
                    MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "Область захвачена!");
                    SetState(States.DefenderProtected);
                }

                break;
            case States.DefenderProtected:
                if (!_defenderExists)
                {
                    MessageHud.instance.ShowMessage(MessageHud.MessageType.Center,
                        "Тотем уничтожен! Защитник восстановлен!");
                    RestoreCore();
                    return;
                }

                break;
        }
    }

    private void RestoreCore()
    {
        location.m_noBuild = false;
        location.m_noBuildRadiusOverride = 0f;
        gameObject.SetActive(false);
        enabled = false;
        _nview.GetZDO().Set("GhostDisabled", true);
        ZNetScene.instance.Destroy(location.transform.parent.gameObject);


        var zoneLocation = LocationLoader.GetRandomT1CoreLocation();
        ZoneSystem.instance.SpawnLocation(
            zoneLocation,
            0,
            transform.position,
            Quaternion.identity,
            ZoneSystem.SpawnMode.Full,
            new List<GameObject>()
        );
    }

    private void ProcessStates()
    {
        switch (_state)
        {
            case States.CoreExists:
                SetCommonSpawnerParams(true, null);
                SetRaidSpawnerParams(false, null);
                location.m_noBuild = true;
                location.m_noBuildRadiusOverride = NoBuildRadius;
                break;
            case States.CoreDestroyed:
                SetCommonSpawnerParams(true, null);
                SetRaidSpawnerParams(false, null);
                location.m_noBuild = false;
                location.m_noBuildRadiusOverride = 0;
                break;
            case States.DefenderExists:
                SetCommonSpawnerParams(true, _defender);
                SetRaidSpawnerParams(true, _defender);
                location.m_noBuild = false;
                location.m_noBuildRadiusOverride = 0;
                if (Time.time - _defenderSetupTime < DefenderRaidTime)
                {
                    MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft,
                        $"Осталось: {Math.Round(DefenderRaidTime - (Time.time - _defenderSetupTime), 2)}");
                }

                break;
            case States.DefenderProtected:
                SetCommonSpawnerParams(false, null);
                SetRaidSpawnerParams(false, null);
                location.m_noBuild = false;
                location.m_noBuildRadiusOverride = 0;
                break;
        }
    }

    private void SearchCore()
    {
        _coreExists = ConquestCore.ActiveCores
            .Any(x => Vector3.Distance(transform.position, x.transform.position) < 20f);
    }

    private void SearchDefenderTotem()
    {
        _defender = DefenderTotemComponent.ActiveTotems
            .FirstOrDefault(x => Vector3.Distance(transform.position, x.transform.position) < 20f)
            ?.GetComponent<Piece>();
        _defenderExists = _defender != null;
    }

    private void SetState(States state)
    {
        if (state != _state)
        {
            Jotunn.Logger.LogInfo(
                $"Ghost[{Vector3.Distance(transform.position, Player.m_localPlayer.transform.position)}]: {_state} => {state}");
            if (_nview.IsValid() && _nview.IsOwner())
                _nview.GetZDO().Set("ghost_state", (int) state);
        }

        _prevState = _state;
        _state = state;
    }

    private void SetCommonSpawnerParams(bool componentEnabled, StaticTarget forceTargetStatic)
    {
        if (_spawnerT1 != null)
        {
            _spawnerT1.enabled = componentEnabled;
            _spawnerT1.forcedStaticTarget = forceTargetStatic;
        }
    }

    private void SetRaidSpawnerParams(bool componentEnabled, StaticTarget forceTargetStatic)
    {
        if (_spawnerRaid != null)
        {
            _spawnerRaid.enabled = componentEnabled;
            _spawnerRaid.forcedStaticTarget = forceTargetStatic;
        }
    }

    public enum States
    {
        CoreExists,
        CoreDestroyed,
        DefenderExists,
        DefenderProtected
    }
}