using System.Collections.Generic;
using PrototypingTools.Utils;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<Card> _cards = new();
	private List<Card> _discardPile = new();

	private System.Random _random = new();

	public int TotalCards => _cards.Count + _discardPile.Count;


	public void Init(Player owner, List<Card> cardPrefabs)
	{
		Clear();

		foreach (Card prefab in cardPrefabs)
		{
			Card card = Instantiate(prefab, transform);
			card.gameObject.SetActive(false);
			card.Init(owner);

			_cards.Add(card);
		}

		_cards.Shuffle();
	}

	public void RefreshFromDiscardPile()
	{
		_cards.AddRange(_discardPile);
		_discardPile.Clear();

		_cards.Shuffle();
	}

	public void DiscardCards(List<Card> cards)
	{
		foreach (Card card in cards)
		{
			_cards.Remove(card);
			_discardPile.Add(card);
		}
	}

	public void Clear()
	{
		RefreshFromDiscardPile();

		foreach (Card card in _cards)
		{
			Destroy(card.gameObject);
		}
		_cards.Clear();
	}

	public List<Card> DrawCards(int number, List<Card> ineligibleCards = null)
	{
		List<Card> cards = new();

		number = Mathf.Min(number, TotalCards);

		for (int i = 0; i < number; ++i)
		{
			if (_cards.Count == 0)
			{
				// no more cards available, refresh from discard pile and immediately discard cards picked (so we don't pick again)
				RefreshFromDiscardPile();
				DiscardCards(cards);

				if (ineligibleCards != null && ineligibleCards.Count > 0)
				{
					DiscardCards(ineligibleCards);
				}

				if (_cards.Count == 0)
				{
					// if we still have nothing, we're out of cards, so we're done
					break;
				}
			}

			int index = _random.Next(_cards.Count);
			Card card = _cards[index];

			_discardPile.Add(card);
			_cards.RemoveAt(index);

			cards.Add(card);
		}

		return cards;
	}
}
