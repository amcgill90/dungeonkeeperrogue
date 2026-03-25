using UnityEngine;

public abstract class MapNodeReward : MonoBehaviour
{
	[SerializeField] private Sprite _rewardHintSprite;
	
	public Sprite RewardHintSprite => _rewardHintSprite;
	
	public abstract void Trigger(MapNode fromNode);
}