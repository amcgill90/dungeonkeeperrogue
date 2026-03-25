using System.Collections;
using UnityEngine;

public class Room : MonoBehaviour
{
	protected Player _owner;

	private static readonly NodeSelectionFilterOptions nodeSelectOptions = new NodeSelectionFilterOptions()
	{
		controlled = SelectionFilter.True,
		hasRoom = SelectionFilter.False
	};


	public void Init(Player owner)
	{
		_owner = owner;
	}

	public IEnumerator PlaceRoom()
	{
		MapNode placeOnNode = null;

		while (placeOnNode == null)
		{
			placeOnNode = _owner.Input.DoMapNodeSelection(nodeSelectOptions);

			yield return null;
		}

		placeOnNode.AddRoom(this);

		_owner.Input.DoMapNodeSelection(null);
	}
}
