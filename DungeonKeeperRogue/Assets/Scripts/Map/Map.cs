using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoSingleton<Map>
{
	[SerializeField] private MapConfig _defaultConfig;
	[SerializeField] private float _nodeSize;
	[SerializeField] private Transform _nodesRoot;
	

	private MapConfig _currentConfig;
	private MapNode[,] _mapNodes;

	private static List<MapNode> _checkedNodesCache = new();


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
		float xInc = _currentConfig.Resolution.x > 1 ? mapSize.x / (_currentConfig.Resolution.x - 1) : 0f;
		float yInc = _currentConfig.Resolution.y > 1 ? -mapSize.y / (_currentConfig.Resolution.y - 1) : 0f;

		// generate node instances and add to array
		for (int x = 0; x < _currentConfig.Resolution.x; ++x)
		{
			float xPos = x * xInc - halfX;

			for (int y = 0; y < _currentConfig.Resolution.y; ++y)
			{
				float yPos = y * yInc + halfY;
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

	public void ShowNodeOptions(NodeSelectionFilterOptions options)
	{
		foreach (MapNode node in _mapNodes)
		{
			bool isOption = options != null
				&& options.IsDiggableSatisfied(node)
				&& options.IsControlledSatisfied(node)
				&& options.IsAdjacentToControlledSatisfied(node);

			node.SetOption(isOption);
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
}
