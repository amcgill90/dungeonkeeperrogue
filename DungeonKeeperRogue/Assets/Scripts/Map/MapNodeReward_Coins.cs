using System.Collections;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public class MapNodeReward_Coins : MapNodeReward
{
	[SerializeField] private int _amount = 3;
	[SerializeField] private float _destoyDelay = 1f;
	
	public override void Trigger()
	{
		Scenario.Instance.Player.AddCoins(_amount);
		StartCoroutine(DestroyRoutine());
	}

	private IEnumerator DestroyRoutine()
	{
		yield return new WaitForSeconds(_destoyDelay);
		Destroy(gameObject);
	}
}