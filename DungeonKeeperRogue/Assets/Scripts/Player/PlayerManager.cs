using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	[SerializeField] private List<Card> _defaultDeckCards;
	
	public static PlayerState PlayerState { get; private set; }
	
	public void CreateNewPlayerState()
	{
		PlayerState = new PlayerState(_defaultDeckCards);
	}
}
