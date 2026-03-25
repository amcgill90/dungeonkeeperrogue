using System.Collections;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public class Room_DrawCardBuff : MapUnitBehaviour
{
	[SerializeField] private int _amount = 1;
	[SerializeField] private FlyingText _flyingTextFX;
	[SerializeField] private float _duration;


	public override IEnumerator OnStartOfTurn(MapUnit unit, MapUnitBehaviourContext context)
	{
		context.cardsToDraw += _amount;
		var flyingText = Instantiate(_flyingTextFX, transform.position, Quaternion.identity);
		flyingText.Init(_amount.ToString());
		
		yield return new WaitForSeconds(_duration);
	}
}
