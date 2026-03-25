using System.Collections.Generic;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public class MapActor : MonoBehaviour
{
    [SerializeField] private Team _team;
    [SerializeField] private bool _autoCompleteTurns;
    
    protected bool _isTurnComplete;
    private readonly List<MapUnit> _units = new();
    
    public List<MapUnit> Units => _units;
    public int UnitCount => _units.Count;
    public bool IsTurnComplete => IsTurnCompleteInternal();

    private void Awake()
    {
        TurnController.OnTurnStart += OnTurnStart;
    }

    private void OnDestroy()
    {
        TurnController.OnTurnStart -= OnTurnStart;
        OnDestroyInternal();
    }

    protected virtual void OnDestroyInternal()
    {
    }

    public virtual void Init() { }
	
    private void OnTurnStart(Team team)
    {
        if (team != _team)
        {
            return;
        }

        OnTurnStartInternal();
        
        _isTurnComplete = false;
        _units.ForEach(u => u.RunBehavior());
    }

    protected virtual void OnTurnStartInternal()
    {
        
    }

    private bool IsTurnCompleteInternal()
    {
        foreach (MapUnit unit in _units)
        {
            if (unit.IsRunningBehaviour)
            {
                return false;
            }
        }
        
        return _autoCompleteTurns || _isTurnComplete;
    }
    
    public void RegisterUnit(MapUnit unit)
    {
        _units.Add(unit);
        unit.OnDestroyed += DeregisterUnit;
    }
	
    private void DeregisterUnit(MapUnit unit)
    {
        unit.OnDestroyed -= DeregisterUnit;
        if (_units.Contains(unit))
        {
            _units.Remove(unit);
        }
    }
}