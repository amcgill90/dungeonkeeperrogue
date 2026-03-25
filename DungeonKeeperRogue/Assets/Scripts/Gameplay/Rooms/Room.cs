using System.Collections;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public abstract class Room : MapUnitBehaviour
{
	private static readonly NodeSelectionFilterOptions nodeSelectOptions = new NodeSelectionFilterOptions()
	{
		controlled = SelectionFilter.True,
		hasRoom = SelectionFilter.False
	};


	public IEnumerator PlaceRoom()
	{
		MapNode placeOnNode = null;
		PlayerInput input = Scenario.Instance.Player.Input;

		while (placeOnNode == null)
		{
			placeOnNode = input.DoMapNodeSelection(nodeSelectOptions);

			yield return null;
		}

		placeOnNode.AddRoom(this);

		input.DoMapNodeSelection(null);
	}
}
