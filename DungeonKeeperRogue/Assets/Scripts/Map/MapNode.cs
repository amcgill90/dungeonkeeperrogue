using UnityEngine;

public class MapNode : MonoBehaviour
{
	public enum HighlightState
	{
		None,
		Option,
		Hovered
	}

	[SerializeField] private NodeTag _tag;
	[SerializeField] private bool _isBase;
	[SerializeField] private bool _canContainReward;
	[SerializeField] private GameObject _prefabToInstantiate;
	[SerializeField] private SpriteRenderer _highlight;
	[SerializeField] private Color _highlightOptionColour;
	[SerializeField] private GameObject _isControlledHighlight;
	
	private Vector2Int _coordinates;
	private GameObject _instancedObject;
	private Diggable _diggable;
	private Color _defaultHighlightColour;
	private Room _room;
	private MapNodeReward _reward;


	public Vector2Int Coordinates => _coordinates;
	public GameObject InstancedObject => _instancedObject;
	public Diggable Diggable => _diggable;
	public bool IsBase => _isBase;
	public bool CanContainReward => _canContainReward;
	public MapNodeReward Reward => _reward;
	public bool HasRoom => _room != null || IsBase;


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

	public void AssignReward(MapNodeReward reward)
	{
		_reward = reward;
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

	public void AddRoom(Room room)
	{
		_room = room;
		room.transform.SetParent(transform, false);
		room.transform.localPosition = new Vector3(0f, 0f, -1f);
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
		if (TryGetReward(out MapNodeReward prefab))
		{
			var reward = Instantiate(prefab, transform);
			reward.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			reward.Trigger();
		}
	}

	private bool TryGetReward(out MapNodeReward reward)
	{
		if (_reward) return reward = _reward;
		
		var mapConfig = Map.Instance.CurrentConfig;
		if (mapConfig == false || mapConfig.RewardsConfig == false) return reward = null;

		return mapConfig.RewardsConfig.TryGetRandomNodeReward(out reward);
	}

	public bool HasTag(NodeTag nodeTag)
	{
		return _tag && _tag == nodeTag;
	}
}