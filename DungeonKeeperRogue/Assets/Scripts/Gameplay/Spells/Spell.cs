using System.Collections;
using UnityEngine;

public class Spell : MonoBehaviour
{
	protected Player _owner;


	public void Init(Player owner)
	{
		_owner = owner;
	}

	public virtual IEnumerator CastSpell()
	{
		// override this function to implement specific spell behaviour
		yield return null;
	}
}
