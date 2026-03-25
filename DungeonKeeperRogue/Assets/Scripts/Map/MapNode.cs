using System.Collections;
using UnityEngine;

public class MapNode : MonoBehaviour
{
	public enum HighlightState
	{
		None,
		Option,
		Hovered
	}

	[SerializeField] private bool _isBase;
	[SerializeField] private GameObject _prefabToInstantiate;
	[SerializeField] private SpriteRenderer _highlight;
	[SerializeField] private Color _highlightOptionColour;
	[SerializeField] private GameObject _isControlledHighlight;


	private Vector2Int _coordinates;
	private GameObject _instancedObject;
	private Diggable _diggable;
	private Color _defaultHighlightColour;


	public Vector2Int Coordinates => _coordinates;
	public GameObject InstancedObject => _instancedObject;
	public Diggable Diggable => _diggable;
	public bool IsBase => _isBase;


	private void Awake()
	{
		_defaultHighlightColour = _highlight.color;
	}

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
		}

		_diggable = GetComponentInChildren<Diggable>();

		SetHighlighted(HighlightState.None);
		ShowControlled(false);
	}

	public void SetHighlighted(HighlightState highlight)
	{
		Color colour = highlight == HighlightState.Option ? _highlightOptionColour : _defaultHighlightColour;

		_highlight.color = colour;
		_highlight.gameObject.SetActive(highlight != HighlightState.None);
	}

	public void ShowControlled(bool show)
	{
		_isControlledHighlight.SetActive(show);
	}

	private void OnDig(Diggable diggable)
	{
		if (diggable != _diggable)
		{
			return;
		}

		_diggable = null;
		_instancedObject = null;
		
		ApplyReward();
	}

	private void ApplyReward()
	{
		var mapConfig = Map.Instance.CurrentConfig;
		if (mapConfig == false || mapConfig.RewardsConfig == false) return;

		if (mapConfig.RewardsConfig.TryGetRandomNodeReward(out MapNodeReward reward))
		{
			reward.Trigger();
		}
	}
}
