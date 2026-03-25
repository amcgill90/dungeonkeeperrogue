using System.Collections;
using System.Collections.Generic;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public class MapNodeReward_Cards : MapNodeReward
{
    [SerializeField] private int _amount = 3;
    [SerializeField] private float _destroyDelay = 1f;
    [SerializeField] private FlyingText _flyingTextPrefab;
	
    public override void Trigger()
    {
        Scenario.Instance.Player.DrawCardsToHand(_amount);
        var flyingText = Instantiate(_flyingTextPrefab, transform.position, Quaternion.identity);
        flyingText.Init(_amount.ToString());
        StartCoroutine(DestroyRoutine());
    }

    private IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(_destroyDelay);
        Destroy(gameObject);
    }
}
