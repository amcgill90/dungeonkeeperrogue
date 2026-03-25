using System.Collections;
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


    private void OnDestroy()
    {
        OnDestroyInternal();
    }

    protected virtual void OnDestroyInternal()
    {
    }

    public virtual void Init() { }
	
    public IEnumerator OnTurnStart()
    {
        _isTurnComplete = false;

		foreach (MapUnit unit in _units)
		{
			yield return unit.RunStartOfTurnBehaviour();
		}

        yield return OnTurnStartInternal();

		if (_autoCompleteTurns)
		{
			_isTurnComplete = true;
		}
    }

	public IEnumerator OnTurnEnd()
	{
		yield return OnTurnEndInternal();

		foreach (MapUnit unit in _units)
		{
			yield return unit.RunEndOfTurnBehaviour();
		}
	}

    protected virtual IEnumerator OnTurnStartInternal()
    {
        yield return null;
    }

	protected virtual IEnumerator OnTurnEndInternal()
    {
        yield return null;
    }

    private bool IsTurnCompleteInternal()
    {
        return _isTurnComplete;
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
