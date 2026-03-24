using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private List<Card> _defaultDeckCards;

	private Deck _deck;

	public Deck Deck => _deck;


	public void Init()
	{
		GameObject deckGo = new GameObject("Deck");
		deckGo.transform.SetParent(transform);

		_deck = deckGo.AddComponent<Deck>();
		_deck.Init(_defaultDeckCards);
	}
}
