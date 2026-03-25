using System.Collections;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public class Room_DamageBuff : MapUnitBehaviour
{
	[SerializeField] private Team _targetTeam;
	[SerializeField] private DamageSource[] _buffSources;
	[SerializeField] private int _buffAmount = 1;
	[SerializeField] private FlyingText _flyingTextFX;
	[SerializeField] private float _duration;


	public override void Init()
	{
		base.Init();

		Health.OnDamageAttemptedAll += OnDamageAttempted;
	}

	protected override void OnDestroyInternal()
	{
		base.OnDestroyInternal();

		Health.OnDamageAttemptedAll -= OnDamageAttempted;
	}

	private void OnDamageAttempted(Health health, DamageDetails damageDetails)
	{
		if (health.Team != _targetTeam || System.Array.FindIndex(_buffSources, i => i == damageDetails._sourceType) < 0)
		{
			return;
		}

		damageDetails._damage += _buffAmount;

		var flyingText = Instantiate(_flyingTextFX, transform.position, Quaternion.identity);
		flyingText.Init(_buffAmount.ToString());
	}
}
