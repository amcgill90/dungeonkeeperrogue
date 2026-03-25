using System.Collections;
using UnityEngine;

public class MapNode : MonoBehaviour
{
	[SerializeField] private GameObject _prefabToInstantiate;
	[SerializeField] private GameObject _highlight;
	[SerializeField] private GameObject _option;


	private Vector2Int _coordinates;
	private GameObject _instancedObject;
	private Diggable _diggable;


	public Vector2Int Coordinates => _coordinates;
	public GameObject InstancedObject => _instancedObject;
	public Diggable Diggable => _diggable;


	private void OnEnable()
	{
		Diggable.OnDig += OnDig;
	}

	private void OnDisable()
	{
		Diggable.OnDig -= OnDig;
	}

	public virtual void Init(int x, int y)
	{
		_coordinates = new Vector2Int(x, y);

		if (_prefabToInstantiate != null)
		{
			_instancedObject = Instantiate(_prefabToInstantiate, transform);
			_diggable = _instancedObject.GetComponent<Diggable>();
		}

		SetHighlighted(false);
		SetOption(false);
	}

	public void SetHighlighted(bool highlight)
	{
		_highlight.SetActive(highlight);
	}

	public void SetOption(bool option)
	{
		_option.SetActive(option);
	}

	private void OnDig(Diggable diggable)
	{
		if (diggable != _diggable)
		{
			return;
		}

		_diggable = null;
		_instancedObject = null;
	}
}
