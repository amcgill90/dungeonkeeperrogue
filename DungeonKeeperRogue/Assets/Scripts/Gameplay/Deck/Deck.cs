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

	public void Clear()
	{
		RefreshFromDiscardPile();

		foreach (Card card in _cards)
		{
			Destroy(card.gameObject);
		}
		_cards.Clear();
	}

	public List<Card> DrawCards(int number)
	{
		List<Card> cards = new();

		number = Mathf.Min(number, TotalCards);

		for (int i = 0; i < number; ++i)
		{
			if (_cards.Count == 0)
			{
				// no more cards available, refresh from discard pile
				RefreshFromDiscardPile();
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
