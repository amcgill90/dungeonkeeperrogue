using System.Collections;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public class Room_DrawCardBuff : MapUnitBehaviour
{
	[SerializeField] private int _amount = 1;


	public override IEnumerator OnStartOfTurn(MapUnit unit, MapUnitBehaviourContext context)
	{
		context.cardsToDraw += _amount;
		yield return null;
	}
}
