using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapNodeRewardsConfig", menuName = "DungeonKeeperRogue/Reward/Config")]
public class MapNodeRewardsConfig : ScriptableObject
{
	public List<MapNodeReward> Rewards;
	public float RewardChance = 0.5f;

	public bool TryGetRandomNodeReward(out MapNodeReward nodeReward)
	{
		if (Rewards.Count <= 0 || Random.value > RewardChance)
		{
			nodeReward = null;
			return false;
		}
		
		nodeReward = Rewards[Random.Range(0, Rewards.Count)];
		return true;
	}
}