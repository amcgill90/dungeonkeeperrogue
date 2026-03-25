using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Dig : Spell
{
	[SerializeField] private int _digsAllowed = 2;
	[SerializeField] private GameObject _digFXPrefab;

	private int _digCount = 0;

	public int DigsRemaining => _digsAllowed - _digCount;

	private static readonly List<System.Func<IEnumerator>> onDigActions = new();

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
			// make sure hand is lowered since we're digging
			_owner.Hand.Show(false);

			MapNode selectedNode = _owner.Input.DoMapNodeSelection(nodeSelectOptions);
			
			if (selectedNode != null && selectedNode.Diggable != null)
			{
				// player just selected this node, so dig here
				selectedNode.Diggable.Dig();
				Instantiate(_digFXPrefab, selectedNode.WorldPos, Quaternion.identity);
				++_digCount;

				HUDExcavationTargeting.Instance.ShowExcavationUI(_digCount + 1, _digsAllowed);

				yield return ProcessOnDigActions();
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

	private static IEnumerator ProcessOnDigActions()
	{
		foreach (System.Func<IEnumerator> action in onDigActions)
		{
			yield return action.Invoke();
		}
	}

	public static void RegisterOnDigAction(System.Func<IEnumerator> action)
	{
		onDigActions.Add(action);
	}

	public static void DeregisterOnDigAction(System.Func<IEnumerator> action)
	{
		onDigActions.Remove(action);
	}
}
