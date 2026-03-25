using UnityEngine;

public class NodeSelectionFilterOptions
{
	public SelectionFilter diggable = SelectionFilter.Any;
	public SelectionFilter controlled = SelectionFilter.Any;
	public SelectionFilter adjacentToControlled = SelectionFilter.Any;
	public SelectionFilter hasRoom = SelectionFilter.Any;


	public bool IsDiggableSatisfied(MapNode node)
	{
		switch (diggable)
		{
			case SelectionFilter.True:
				return node.Diggable != null;

			case SelectionFilter.False:
				return node.Diggable == null;

			case SelectionFilter.Any:
				return true;
		}

		return true;
	}

	public bool IsControlledSatisfied(MapNode node)
	{
		if (controlled == SelectionFilter.Any)
		{
			return true;
		}

		// first get the controlled state of the node
		bool isControlled = Map.Instance.IsNodeControlledByPlayer(node);

		switch (controlled)
		{
			case SelectionFilter.True:
				return isControlled;

			case SelectionFilter.False:
				return isControlled == false;
		}

		return false;
	}

	public bool IsAdjacentToControlledSatisfied(MapNode node)
	{
		if (adjacentToControlled == SelectionFilter.Any)
		{
			return true;
		}

		bool isControlled = Map.Instance.IsAdjacentNodeControlledByPlayer(node);

		switch (adjacentToControlled)
		{
			case SelectionFilter.True:
				return isControlled;

			case SelectionFilter.False:
				return isControlled == false;
		}

		return false;
	}

	public bool IsHasRoomSatisfied(MapNode node)
	{
		switch (hasRoom)
		{
			case SelectionFilter.True:
				return node.HasRoom;

			case SelectionFilter.False:
				return node.HasRoom == false;

			case SelectionFilter.Any:
				return true;
		}

		return false;
	}

	public bool IsValidOption(MapNode node)
	{
		return IsHasRoomSatisfied(node)
			&& IsDiggableSatisfied(node)
			&& IsControlledSatisfied(node)
			&& IsAdjacentToControlledSatisfied(node);
	}
}
