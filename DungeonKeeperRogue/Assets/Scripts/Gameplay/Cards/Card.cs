using System;
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
	[SerializeField] private bool _hasModifiableDamage;
	[SerializeField] private int _baseModifiableDamage;
	[SerializeField] private string _description;
	[SerializeField] private Sprite _icon;
	[SerializeField] private int _goldCost;
	
	[Header("Card detail visuals")] 
	[SerializeField] private TMP_Text _nameTextField;
	[SerializeField] private TMP_Text _descriptionTextField;
	[SerializeField] private SpriteRenderer _iconSpriteRenderer;
	[SerializeField] private TMP_Text _costTextField;
	[SerializeField] private Color _cantPlayColour;

	[Header("Events")]
	[SerializeField] private UnityEvent _onHighlightedEvent;
	[SerializeField] private UnityEvent _onStopHighlightedEvent;

	private bool _isPlaying = false;
	private Player _owner;

	public CardType CardType => _cardType;
	public bool IsPlaying => _isPlaying;
	public int Cost => _goldCost;
	public string Name => _name;
	public Sprite Icon => _icon;

	public static event Action<IntStatProperty> OnModifiableDamageDescriptionGenerated;
	public static event Action OnCardPlayed;

	private void OnEnable()
	{
		SetHighlighted(false);
	}

	public void Init(Player owner)
	{
		_owner = owner;
		
		SetCardInfo();

		OnCardPlayed += SetCardInfo;
	}

	private void SetCardInfo()
	{
		_nameTextField.SetText(_name);
		_descriptionTextField.SetText(GetDescription());
		_iconSpriteRenderer.sprite = _icon;
		_costTextField.SetText(_goldCost.ToString());
	}

	private void OnDestroy()
	{
		OnCardPlayed -= SetCardInfo;
	}

	private void Update()
	{
		UpdatePlayable();
	}

	public string GetDescription()
	{
		if (_hasModifiableDamage == false)
		{
			return _description;
		}

		IntStatProperty damageStat = new IntStatProperty(_baseModifiableDamage);
		OnModifiableDamageDescriptionGenerated?.Invoke(damageStat);

		if (damageStat.GetCurrentValue() > damageStat.GetBaseValue())
		{
			return string.Format(_description, $"<b><color=#0022ff>{damageStat.GetCurrentValue()}</color></b>");
		}
		
		return string.Format(_description, damageStat.GetCurrentValue());
	}

	public bool GetCanPlay()
	{
		if (_owner == null)
		{
			return false;
		}

		bool isRoomPlayable = _cardType != CardType.Room
			|| Map.Instance.CanRoomBePlayed();

		return _owner.Coins >= _goldCost && isRoomPlayable;
	}

	public IEnumerator PlayCard()
	{
		if (GetCanPlay() == false)
		{
			yield break;
		}

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
		
		OnCardPlayed?.Invoke();
	}

	public void SetHighlighted(bool highlight)
	{
		if (GetCanPlay() == false)
		{
			_onStopHighlightedEvent?.Invoke();
			return;
		}

		if (highlight)
		{
			_onHighlightedEvent?.Invoke();
			return;
		}
		
		_onStopHighlightedEvent?.Invoke();
	}

	public void UpdatePlayable()
	{
		_costTextField.color = GetCanPlay() ? Color.white : _cantPlayColour;
	}
}
