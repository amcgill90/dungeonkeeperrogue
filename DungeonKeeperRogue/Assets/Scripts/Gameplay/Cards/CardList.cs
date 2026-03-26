using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardList", menuName = "DungeonKeeperRogue/Card List")]
public class CardList : ScriptableObject
{
    [SerializeField] private List<Card> _cards;

	public List<Card> Cards => _cards;
}
