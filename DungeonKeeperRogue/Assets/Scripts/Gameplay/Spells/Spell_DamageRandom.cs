using System.Collections;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public class Spell_DamageRandom : Spell
{
	[SerializeField] private Team _targetTeam;
	[SerializeField] private int _damageToDeal = 3;


	public override IEnumerator CastSpell()
	{
		MapActor ma = Scenario.Instance.GetMapActorForTeam(_targetTeam);
		if (ma == null || ma.UnitCount == 0)
		{
			yield break;
		}

		MapUnit unit = ma.Units[Random.Range(0, ma.Units.Count)];
		if (unit != null && unit.Health != null)
		{
			DamageDetails damageDetails = new(_damageToDeal);
			unit.Health.TryDamage(damageDetails);
		}

		yield return null;

		Destroy(gameObject);
	}
}
