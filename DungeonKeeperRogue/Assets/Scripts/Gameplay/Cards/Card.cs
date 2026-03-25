using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
	[SerializeField] private CardType _cardType;
	[SerializeField] private GameObject _highlight;
	[SerializeField] private GameObject _prefabToSpawn;

	private bool _isPlaying = false;
	private Player _owner;

	public CardType CardType => _cardType;
	public bool IsPlaying => _isPlaying;


	private void OnEnable()
	{
		SetHighlighted(false);
	}

	public void Init(Player owner)
	{
		_owner = owner;
	}

	public IEnumerator PlayCard()
	{
		GameObject go = Instantiate(_prefabToSpawn, transform.position, Quaternion.identity);
		Spell spell = go.GetComponentInChildren<Spell>();

		_isPlaying = true;

		if (spell != null)
		{
			spell.Init(_owner);
			yield return spell.CastSpell();
		}

		_isPlaying = false;
	}

	public void SetHighlighted(bool highlight)
	{
		_highlight.SetActive(highlight);
	}
}
