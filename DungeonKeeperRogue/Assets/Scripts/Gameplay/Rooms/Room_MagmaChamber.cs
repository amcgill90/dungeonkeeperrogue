using System.Collections;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;
using UnityEngine.Events;

public class Room_MagmaChamber : MapUnitBehaviour
{
    [SerializeField] private Team _targetTeam;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private GameObject _fxObject;
    [SerializeField] private UnityEvent _onFXStart;

    private MapUnit _targetUnit;


	public override void Init()
	{
		base.Init();

		Spell_Dig.RegisterOnDigAction(OnDig);
	}

	protected override void OnDestroyInternal()
	{
		base.OnDestroyInternal();

		Spell_Dig.DeregisterOnDigAction(OnDig);
	}

	private IEnumerator OnDig()
	{
		MapActor ma = Scenario.Instance.GetMapActorForTeam(_targetTeam);
        if (ma == false || ma.UnitCount == 0)
        {
            yield break;
        }

        _targetUnit = ma.Units[Random.Range(0, ma.Units.Count)];

        if (_targetUnit == false || _targetUnit.Health == false)
        {
            yield break;
        }
        
        _onFXStart.Invoke();

        var secondStageTimer = 0f;
        Instantiate(_fxObject, transform.position, Quaternion.identity);

        DamageDetails damageDetails = new(_damageAmount, DamageSource.Room);
        _targetUnit.Health.TryDamage(damageDetails);
	}
}
