using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<Card> _cards = new();


	public void Init(List<Card> cardPrefabs)
	{
		Clear();

		foreach (Card prefab in cardPrefabs)
		{
			Card card = Instantiate(prefab, transform);
			card.gameObject.SetActive(false);
			
			_cards.Add(card);
		}
	}

	public void Clear()
	{
		foreach (Card card in _cards)
		{
			Destroy(card.gameObject);
		}
		_cards.Clear();
	}
}
