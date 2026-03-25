using System.Collections;
using UnityEngine;

public class Spell_Dig : Spell
{
	[SerializeField] private int _digsAllowed = 2;

	private int _digCount = 0;
	private static readonly NodeSelectionFilterOptions nodeSelectOptions = new NodeSelectionFilterOptions()
	{
		diggable = SelectionFilter.True,
		adjacentToControlled = SelectionFilter.True
	};


	public override IEnumerator CastSpell()
	{
		// dig needs to receive more player input to determine location to dig
		while (_digCount < _digsAllowed)
		{
			MapNode selectedNode = _owner.Input.DoMapNodeSelection(nodeSelectOptions);
			
			if (selectedNode != null && selectedNode.Diggable != null)
			{
				// player just selected this node, so dig here
				selectedNode.Diggable.Dig();
				selectedNode.SetHighlighted(false);
				++_digCount;
			}

			yield return null;
		}

		Map.Instance.ShowNodeOptions(null);
	}
}
