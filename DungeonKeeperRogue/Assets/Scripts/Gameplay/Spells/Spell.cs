using System.Collections;
using UnityEngine;

public class Spell : MonoBehaviour
{
	public IEnumerator CastSpell(MapNode fromNode)
	{
		yield return CastSpellInternal(fromNode);
	}

	protected virtual IEnumerator CastSpellInternal(MapNode fromNode)
	{
		// override this function to implement specific spell behaviour / visuals
		yield return null;
	}
}
