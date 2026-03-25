using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapNodeRewardsConfig", menuName = "DungeonKeeperRogue/Reward/Config")]
public class MapNodeRewardsConfig : ScriptableObject
{
	public float RewardChance = 0.5f;
	public int RewardCount = 5;
	
	public List<MapNodeReward> Rewards;

	public MapNodeReward GetRandomReward()
	{
		return Rewards[Random.Range(0, Rewards.Count)];
	}
	
	public bool TryGetRandomNodeReward(out MapNodeReward nodeReward)
	{
		if (RewardChance <= 0 || Rewards.Count <= 0 || Random.value > RewardChance)
		{
			return nodeReward = null;
		}
		
		return nodeReward = Rewards[Random.Range(0, Rewards.Count)];
	}
}