using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
	[SerializeField] private CardType _cardType;
	[SerializeField] private GameObject _prefabToSpawn;

	private bool _isPlaying = false;

	public CardType CardType => _cardType;
	public bool IsPlaying => _isPlaying;


	public IEnumerator PlayCard(MapNode toNode)
	{
		if (toNode == null)
		{
			yield break;
		}

		GameObject go = Instantiate(_prefabToSpawn, toNode.transform);
		Spell spell = go.GetComponentInChildren<Spell>();

		_isPlaying = true;

		if (spell != null)
		{
			yield return spell.CastSpell(toNode);
		}

		_isPlaying = false;
	}
}
