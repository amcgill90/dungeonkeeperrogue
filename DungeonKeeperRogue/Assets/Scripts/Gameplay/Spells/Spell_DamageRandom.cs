using System.Collections;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public class Spell_DamageRandom : Spell
{
	[SerializeField] private Team _targetTeam;
	[SerializeField] private int _damageToDeal = 3;
	[SerializeField] private GameObject _fxObject;
	[SerializeField] private Vector2 _spawnPositionDelta;
	[SerializeField] private float _damageDelay;

	public override IEnumerator CastSpell()
	{
		MapActor ma = Scenario.Instance.GetMapActorForTeam(_targetTeam);
		if (ma == false || ma.UnitCount == 0)
		{
			yield break;
		}

		MapUnit unit = ma.Units[Random.Range(0, ma.Units.Count)];
		if (unit == false&& unit.Health == false)
		{
			yield break;
		}

		Vector2 spawnPos = (Vector2) unit.transform.position + _spawnPositionDelta;
		Instantiate(_fxObject, spawnPos, Quaternion.identity);
		
		yield return new WaitForSeconds(_damageDelay);
		
		DamageDetails damageDetails = new(_damageToDeal);
		unit.Health.TryDamage(damageDetails);

		yield return null;

		Destroy(gameObject);
	}
}
