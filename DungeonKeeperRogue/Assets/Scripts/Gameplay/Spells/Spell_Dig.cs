using System.Collections;
using UnityEngine;

public class Spell_Dig : Spell
{
	[SerializeField] private int _digsAllowed = 2;
	[SerializeField] private GameObject _digFXPrefab;

	private int _digCount = 0;

	public int DigsRemaining => _digsAllowed - _digCount;

	private static readonly NodeSelectionFilterOptions nodeSelectOptions = new NodeSelectionFilterOptions()
	{
		diggable = SelectionFilter.True,
		adjacentToControlled = SelectionFilter.True
	};


	public override IEnumerator CastSpell()
	{
		HUDExcavationTargeting.Instance.ShowExcavationUI(_digCount + 1, _digsAllowed);
		HUDExcavationTargeting.OnSkip += DigSkipped;

		// dig needs to receive more player input to determine location to dig
		while (_digCount < _digsAllowed)
		{
			MapNode selectedNode = _owner.Input.DoMapNodeSelection(nodeSelectOptions);
			
			if (selectedNode != null && selectedNode.Diggable != null)
			{
				// player just selected this node, so dig here
				selectedNode.Diggable.Dig();
				Instantiate(_digFXPrefab, selectedNode.WorldPos, Quaternion.identity);
				++_digCount;

				HUDExcavationTargeting.Instance.ShowExcavationUI(_digCount + 1, _digsAllowed);
			}

			yield return null;
		}

		_owner.Input.DoMapNodeSelection(null);

		HUDExcavationTargeting.OnSkip -= DigSkipped;
		HUDExcavationTargeting.Instance.CompleteExcavation();

		Destroy(gameObject);
	}

	private void DigSkipped()
	{
		_digCount = _digsAllowed;
	}
}
