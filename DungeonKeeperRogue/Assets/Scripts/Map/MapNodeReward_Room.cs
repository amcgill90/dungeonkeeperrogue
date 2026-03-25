using System.Collections;
using System.Collections.Generic;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public class MapNodeReward_Room : MapNodeReward
{
    [SerializeField] private Room _roomPrefab;
	[SerializeField] private float _destroyDelay;

	
    public override void Trigger(MapNode fromNode)
    {
        Room room = Instantiate(_roomPrefab, Vector3.zero, Quaternion.identity);
		fromNode.AddRoom(room);
		
        StartCoroutine(DestroyRoutine());
    }

    private IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(_destroyDelay);
        Destroy(gameObject);
    }
}
