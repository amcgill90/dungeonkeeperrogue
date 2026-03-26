using System;
using System.Collections;
using System.Collections.Generic;
using PrototypingTools.Utils;
using Unity.Mathematics;
using UnityEngine;

public class Map : MonoSingleton<Map>
{
	[SerializeField] private MapConfig _defaultConfig;
	[SerializeField] private float _nodeSize;
	[SerializeField] private Transform _nodesRoot;
	

	private MapConfig _currentConfig;
	private MapNode[,] _mapNodes;

	private static List<MapNode> _checkedNodesCache = new();
	
	public MapConfig CurrentConfig => _currentConfig;
	public float NodeXInc { get; private set; }
	public float NodeYInc { get; private set; }


	private void OnEnable()
	{
		if (_defaultConfig != null && _currentConfig == null)
		{
			Init(_defaultConfig);
		}
	}

	public void Init(MapConfig config)
	{
		_currentConfig = config;

		InstantiateNodes();
		AssignRewards();
		ShowNodeHighlights(null, null);
	}

	public void RemoveNodes()
	{
		if (_mapNodes != null)
		{
			foreach (MapNode node in _mapNodes)
			{
				Destroy(node.gameObject);
			}
			_mapNodes = null;
		}
	}

	private void InstantiateNodes()
	{
		RemoveNodes();

		// prepare array for easy access and relationships
		_mapNodes = new MapNode[_currentConfig.Resolution.x, _currentConfig.Resolution.y];

		Vector2 mapSize = new Vector2(_nodeSize * _currentConfig.Resolution.x, _nodeSize * _currentConfig.Resolution.y);
		float halfX = mapSize.x * 0.5f;
		float halfY = mapSize.y * 0.5f;
		
		NodeXInc = _currentConfig.Resolution.x > 1 ? mapSize.x / (_currentConfig.Resolution.x - 1) : 0f;
		NodeYInc = _currentConfig.Resolution.y > 1 ? -mapSize.y / (_currentConfig.Resolution.y - 1) : 0f;

		// generate node instances and add to array
		for (int x = 0; x < _currentConfig.Resolution.x; ++x)
		{
			float xPos = x * NodeXInc - halfX;

			for (int y = 0; y < _currentConfig.Resolution.y; ++y)
			{
				float yPos = y * NodeYInc + halfY;
				MapNode prefab = _currentConfig.GetNode(x, y);
				if (prefab == null)
				{
					continue;
				}

				MapNode newNode = Instantiate(prefab, _nodesRoot);
				newNode.Init(x, y);
				newNode.transform.localPosition = new Vector3(xPos, yPos, 0f);

				_mapNodes[x, y] = newNode;
			}
		}
	}
	
	private void AssignRewards()
	{
		var nodes = new List<int2>(_currentConfig.Resolution.x * _currentConfig.Resolution.y);
		for (int x = 0; x < _currentConfig.Resolution.x; ++x)
		{
			for (int y = 0; y < _currentConfig.Resolution.y; ++y)
			{
				var index = new int2(x, y);
				nodes.Add(index);
			}
		}
		
		nodes.Shuffle();
		var assignedRewards = 0;
		List<MapNodeRewardsConfig.ForcedReward> forcedRewardConfig = new(_currentConfig.RewardsConfig.ForcedRewards);
		
		// break forced rewards into a flat list, then shuffle
		List<MapNodeReward> forcedRewards = new();
		foreach (MapNodeRewardsConfig.ForcedReward forcedReward in forcedRewardConfig)
		{
			for (int i = 0; i < forcedReward.forceCount; ++i)
			{
				forcedRewards.Add(forcedReward.reward);
			}
		}
		forcedRewards.Shuffle();

		for (int i = 0; i < nodes.Count; i++)
		{
			var mapNode = _mapNodes[nodes[i].x, nodes[i].y];
			if (mapNode == false || mapNode.CanContainReward == false || mapNode.Reward) continue;

			if (forcedRewards.Count > 0)
			{
				// pick forced rewards first
				mapNode.AssignReward(forcedRewards[0]);
				forcedRewards.RemoveAt(0);
			}
			else
			{
				// no more forced rewards, so assign random one now
				mapNode.AssignReward(_currentConfig.RewardsConfig.GetRandomReward());
				assignedRewards++;
			}
			
			if (assignedRewards >= _currentConfig.RewardsConfig.RewardCount) break;
		}
	}

	public void ShowNodeHighlights(NodeSelectionFilterOptions options, MapNode hovered)
	{
		foreach (MapNode node in _mapNodes)
		{
			bool isControlled = IsNodeControlledByPlayer(node);
			bool isAdjacentToControlled = IsAdjacentNodeControlledByPlayer(node);
			bool isOption = options != null && options.IsValidOption(node);

			node.ShowControlled(isControlled);
			node.ShowRewardHint(isAdjacentToControlled);
			node.SetHighlighted(isOption ? (hovered == node ? MapNode.HighlightState.Hovered : MapNode.HighlightState.Option) : MapNode.HighlightState.None);
		}
	}

	public bool IsNodeControlledByPlayer(MapNode node, List<MapNode> checkedNodes = null)
	{
		// node is controlled by the player if there is a dug out path from it, back to the player base node
		if (checkedNodes == null)
		{
			checkedNodes = _checkedNodesCache;
			checkedNodes.Clear();
		}

		if (checkedNodes.Contains(node))
		{
			return false;
		}

		checkedNodes.Add(node);

		// check that this node is dug out
		if (node.Diggable != null)
		{
			return false;
		}

		// check if this is the player base node
		if (node.IsBase)
		{
			return true;
		}

		// check that adjacent nodes
		return IsAdjacentNodeControlledByPlayer(node, checkedNodes);
	}

	public bool IsAdjacentNodeControlledByPlayer(MapNode node, List<MapNode> checkedNodes = null)
	{
		if (checkedNodes == null)
		{
			checkedNodes = _checkedNodesCache;
			checkedNodes.Clear();
		}

		// left
		if (node.Coordinates.x > 0 && IsNodeControlledByPlayer(_mapNodes[node.Coordinates.x - 1, node.Coordinates.y], checkedNodes))
		{
			return true;
		}

		// right
		if (node.Coordinates.x < _currentConfig.Resolution.x - 1 && IsNodeControlledByPlayer(_mapNodes[node.Coordinates.x + 1, node.Coordinates.y], checkedNodes))
		{
			return true;
		}

		// down
		if (node.Coordinates.y > 0 && IsNodeControlledByPlayer(_mapNodes[node.Coordinates.x, node.Coordinates.y - 1], checkedNodes))
		{
			return true;
		}

		// up
		if (node.Coordinates.y < _currentConfig.Resolution.y - 1 && IsNodeControlledByPlayer(_mapNodes[node.Coordinates.x, node.Coordinates.y + 1], checkedNodes))
		{
			return true;
		}

		return false;
	}

	public bool CanRoomBePlayed()
	{
		foreach (MapNode node in _mapNodes)
		{
			if (IsNodeControlledByPlayer(node) && node.HasRoom == false)
			{
				return true;
			}
		}

		return false;
	}

	public MapNode FindNodeWithTag(NodeTag nodeTag)
	{
		int rows = _mapNodes.GetLength(0);
		int columns = _mapNodes.GetLength(1);

		for (int x = 0; x < rows; ++x)
		{
			for (int y = 0; y < columns; ++y)
			{
				MapNode node = _mapNodes[y, x];
				if (node.HasTag(nodeTag))
				{
					return node;
				}
			}
		}

		return null;
	}
}
