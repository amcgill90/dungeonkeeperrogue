using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapConfig))]
public class MapConfigEditor : Editor
{
	public override void OnInspectorGUI()
	{
		if (targets.Length > 1)
		{
			EditorGUILayout.LabelField("Can only edit 1 map config at a time");
			return;
		}

		base.OnInspectorGUI();

		EditorGUILayout.LabelField("Map Nodes");

		MapConfig mapConfig = target as MapConfig;

		EditorGUILayout.BeginVertical();

		for (int y = 0; y < mapConfig.Resolution.y; ++y)
		{
			EditorGUILayout.BeginHorizontal();

			for (int x = 0; x < mapConfig.Resolution.x; ++x)
			{
				MapNode oldType = mapConfig.GetNode(x, y);
				MapNode newType = (MapNode)EditorGUILayout.ObjectField(oldType, typeof(MapNode), false);
				if (oldType != newType)
				{
					Undo.RecordObject(mapConfig, "Changed map node type");
					mapConfig.SetNode(x, y, newType);
					EditorUtility.SetDirty(mapConfig);
				}
			}

			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.EndVertical();
	}
}
