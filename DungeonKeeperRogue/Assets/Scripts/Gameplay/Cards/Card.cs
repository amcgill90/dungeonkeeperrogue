using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Card : MonoBehaviour
{
	[SerializeField] private CardType _cardType;
	[SerializeField] private GameObject _prefabToSpawn;

	[Header("Card info")] 
	[SerializeField] private string _name;
	[SerializeField] private string _description;
	[SerializeField] private Sprite _icon;
	[SerializeField] private int _goldCost;
	
	[Header("Card detail visuals")] 
	[SerializeField] private TMP_Text _nameTextField;
	[SerializeField] private TMP_Text _descriptionTextField;
	[SerializeField] private SpriteRenderer _iconSpriteRenderer;
	[SerializeField] private TMP_Text _costTextField;

	[Header("Events")]
	[SerializeField] private UnityEvent _onHighlightedEvent;
	[SerializeField] private UnityEvent _onStopHighlightedEvent;

	private bool _isPlaying = false;
	private Player _owner;

	public CardType CardType => _cardType;
	public bool IsPlaying => _isPlaying;


	private void OnEnable()
	{
		SetHighlighted(false);
	}

	public void Init(Player owner)
	{
		_owner = owner;
		
		_nameTextField.SetText(_name);
		_descriptionTextField.SetText(_description);
		_iconSpriteRenderer.sprite = _icon;
		_costTextField.SetText(_goldCost.ToString());
	}

	public IEnumerator PlayCard()
	{
		GameObject go = Instantiate(_prefabToSpawn, transform.position, Quaternion.identity);
		Spell spell = go.GetComponentInChildren<Spell>();
		Room room = go.GetComponentInChildren<Room>();

		_isPlaying = true;

		if (spell != null)
		{
			spell.Init(_owner);
			yield return spell.CastSpell();
		}

		if (room != null)
		{
			yield return room.PlaceRoom();
		}

		_isPlaying = false;
	}

	public void SetHighlighted(bool highlight)
	{
		if (highlight)
		{
			_onHighlightedEvent?.Invoke();
			return;
		}
		
		_onStopHighlightedEvent?.Invoke();
	}
}
