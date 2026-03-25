using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName = "DungeonKeeperRogue/Map/Config")]
public class MapConfig : ScriptableObject
{
    [SerializeField] private Vector2Int _resolution = new Vector2Int(5, 5);
	[SerializeField][HideInInspector] private MapNode[] _nodes;

	[SerializeField] private MapNodeRewardsConfig _rewardsConfig;
	public MapNodeRewardsConfig RewardsConfig => _rewardsConfig;
	
	public Vector2Int Resolution => _resolution;
	public MapNode[] Nodes => _nodes;

	public MapNode GetNode(int x, int y)
	{
		if (x >= Resolution.x || y >= Resolution.y)
		{
			return null;
		}

		int index = x + (y * Resolution.x);
		return _nodes[index];
	}

	public void SetNode(int x, int y, MapNode newType)
	{
		if (x >= Resolution.x || y >= Resolution.y)
		{
			return;
		}

		int index = x + (y * Resolution.x);
		_nodes[index] = newType;
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (_nodes != null && _nodes.Length == _resolution.x * _resolution.y)
		{
			return;
		}

		MapNode[] oldData = _nodes;
		_nodes = new MapNode[_resolution.x * _resolution.y];

		EditorUtility.SetDirty(this);

		if (oldData == null)
		{
			return;
		}

		// int oldRes = Mathf.RoundToInt(Mathf.Sqrt(oldData.Length));

		// for (int x = 0; x < oldRes && x < _resolution.x; ++x)
		// {
		// 	for (int y = 0; y < oldRes && y < _resolution.y; ++y)
		// 	{
		// 		int oldIndex = x + (y * oldRes);
		// 		int index = x + (y * _resolution.x);
		// 		_nodes[index] = oldData[oldIndex];
		// 	}
		// }
	}
#endif
}