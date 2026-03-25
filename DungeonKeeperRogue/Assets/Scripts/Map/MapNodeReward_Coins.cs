using UnityEngine;

public class MapNodeReward_Coins : MapNodeReward
{
	[SerializeField] private int _amount = 3;
	
	public override void Trigger()
	{
		PlayerManager.Instance.Player.AddCoins(_amount);
	}
}