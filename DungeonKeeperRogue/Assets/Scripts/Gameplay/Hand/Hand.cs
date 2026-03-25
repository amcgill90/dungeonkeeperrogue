using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
	[SerializeField] private BoxCollider2D _handArea;
	[SerializeField] private AnimationCurve _showCurve;
	[SerializeField] private Transform _showPosition;
	[SerializeField] private float _showDuration;
	[SerializeField] private AnimationCurve _hideCurve;
	[SerializeField] private Transform _hidePosition;
	[SerializeField] private float _hideDuration;

	private Player _owner;
	private bool _shown = false;
	private float _animTimer = 0f;
	private float _activeAnimDuration = 0f;
	private Vector3 _startPos;
	private List<Card> _cards = new();

	private Vector3 TargetPosition => _shown ? _showPosition.position : _hidePosition.position;

	public List<Card> CardsInHand => new(_cards);


	public void Init(Player owner)
	{
		_owner = owner;
	}

	private void Update()
	{
		if (_activeAnimDuration <= 0f)
		{
			return;
		}

		_animTimer += Time.deltaTime;

		float ratio = _animTimer / _activeAnimDuration;
		if (ratio >= 1f)
		{
			// done
			_activeAnimDuration = 0f;
			transform.position = TargetPosition;
		}
		else
		{
			AnimationCurve curve = _shown ? _showCurve : _hideCurve;
			transform.position = Vector3.Lerp(_startPos, TargetPosition, curve.Evaluate(ratio));
		}
	}

	public void Show(bool show)
	{
		if (show == _shown)
		{
			return;
		}

		_animTimer = 0f;
		_startPos = transform.position;
		_shown = show;

		if (show)
		{
			_activeAnimDuration = _showDuration;
		}
		else
		{
			_activeAnimDuration = _hideDuration;
		}
	}

	public void AddCard(Card card)
	{
		_cards.Add(card);

		card.gameObject.SetActive(true);
		card.transform.SetParent(_handArea.transform);

		UpdateCardsLayout();
	}

	public void UpdateCardsLayout()
	{
		float cardInc = _cards.Count > 1 ? _handArea.bounds.size.x / (_cards.Count - 1) : 0f;
		float cardOffset = _handArea.bounds.center.x - _handArea.bounds.size.x * 0.5f;

		for (int i = 0; i < _cards.Count; ++i)
		{
			Card card = _cards[i];
			card.transform.localPosition = new Vector3(cardOffset + cardInc * i, 0f, 0f);
		}
	}

	public void DiscardCard(Card card)
	{
		card.transform.SetParent(_owner.Deck.transform);
		card.gameObject.SetActive(false);

		_cards.Remove(card);
		UpdateCardsLayout();
	}

	public void DiscardCards()
	{
		// cards are discarded as soon as they are added to the hand, so we just give them back to the deck and turn them off
		foreach (Card card in new List<Card>(_cards))
		{
			DiscardCard(card);
		}
	}
}
