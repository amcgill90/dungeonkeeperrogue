using System.Collections.Generic;
using UnityEngine;

public class Player : MapActor
{
	[SerializeField] private Hand _hand;
	[SerializeField] private PlayerInput _input;
	[SerializeField] private List<Card> _defaultDeckCards;
	[SerializeField] private int _cardsToDrawEachTurn = 5;
	[SerializeField] private int _coinsToAwardEachTurn = 3;

	private Deck _deck;
	private int _coins = 0;

	public Deck Deck => _deck;
	public Hand Hand => _hand;
	public PlayerInput Input => _input;
	public int Coins => _coins;
	
	public override void Init()
	{
		HUDCombatInfo.OnEndTurn += OnEndTurn;
		
		GameObject deckGo = new GameObject("Deck");
		deckGo.transform.SetParent(transform);

		_deck = deckGo.AddComponent<Deck>();
		_deck.Init(this, _defaultDeckCards);

		_hand.Init(this);

		_input.Init(this);

		// testing
		StartTurn();
	}

	private void SetHandActive(bool active)
	{
		_hand.gameObject.SetActive(active);
	}

	protected override void OnTurnStartInternal()
	{
		SetHandActive(true);
	}

	private void OnEndTurn()
	{
		SetHandActive(false);
		_isTurnComplete = true;
	}

	public void StartTurn()
	{
		_coins += _coinsToAwardEachTurn;
		DrawCardsToHand();
	}

	public void AddCoins(int amount)
	{
		_coins += amount;
	}

	private void DrawCardsToHand()
	{
		List<Card> cards = _deck.DrawCards(_cardsToDrawEachTurn);
		if (cards.Count == 0)
		{
			return;
		}

		for (int i = 0; i < cards.Count; ++i)
		{
			Card card = cards[i];

			_hand.AddCard(card);
		}
	}
}
