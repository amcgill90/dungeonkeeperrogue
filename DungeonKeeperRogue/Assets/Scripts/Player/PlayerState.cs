using System.Collections.Generic;

public class PlayerState
{
    public PlayerState(List<Card> starterCards)
    {
        _deck = new List<Card>();
        _deck.AddRange(starterCards);
    }
	
    private List<Card> _deck;
	
    public List<Card> Deck => _deck;
}