using UnityEngine;

public class NodeSelectionFilterOptions
{
	public SelectionFilter diggable = SelectionFilter.Any;
	public SelectionFilter controlled = SelectionFilter.Any;
	public SelectionFilter adjacentToControlled = SelectionFilter.Any;


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
		switch (controlled)
		{
			
		}

		return false;
	}

	public bool IsAdjacentToControlledSatisfied(MapNode node)
	{
		return false;
	}
}
