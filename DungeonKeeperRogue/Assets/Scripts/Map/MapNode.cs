using System.Collections;
using UnityEngine;

public class MapNode : MonoBehaviour
{
	[SerializeField] private GameObject _prefabToInstantiate;


	private Vector2Int _coordinates;
	private GameObject _instancedObject;


	public Vector2Int Coordinates => _coordinates;
	public GameObject InstancedObject => _instancedObject;


	public virtual void Init(int x, int y)
	{
		_coordinates = new Vector2Int(x, y);

		if (_prefabToInstantiate != null)
		{
			_instancedObject = Instantiate(_prefabToInstantiate, transform);
		}
	}

	public void SetHighlighted(bool highlight)
	{
		
	}
}
