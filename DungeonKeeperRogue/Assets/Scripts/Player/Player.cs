using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private BoxCollider2D _handArea;
	[SerializeField] private Hand _hand;
	[SerializeField] private PlayerInput _input;
	[SerializeField] private List<Card> _defaultDeckCards;
	[SerializeField] private int _cardsToDrawEachTurn = 5;
	[SerializeField] private int _coinsToAwardEachTurn = 3;

	private Deck _deck;
	private int _coins = 0;

	public Deck Deck => _deck;
	public Hand Hand => _hand;
	public int Coins => _coins;


	public void Init()
	{
		GameObject deckGo = new GameObject("Deck");
		deckGo.transform.SetParent(transform);

		_deck = deckGo.AddComponent<Deck>();
		_deck.Init(_defaultDeckCards);

		_input.Init(this);

		// testing
		StartTurn();
	}

	public void StartTurn()
	{
		_coins += _coinsToAwardEachTurn;
		DrawCardsToHand();
	}

	private void DrawCardsToHand()
	{
		List<Card> cards = _deck.DrawCards(_cardsToDrawEachTurn);
		if (cards.Count == 0)
		{
			return;
		}

		float cardInc = cards.Count > 1 ? _handArea.bounds.size.x / (cards.Count - 1) : 0f;
		float cardOffset = _handArea.bounds.center.x - cardInc * (cards.Count / 2);

		for (int i = 0; i < cards.Count; ++i)
		{
			Card card = cards[i];

			card.gameObject.SetActive(true);
			card.transform.SetParent(_handArea.transform);
			card.transform.localPosition = new Vector3(cardOffset + cardInc * i, 0f, 0f);
		}
	}
}
