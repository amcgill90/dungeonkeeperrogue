using System.Collections;
using System.Collections.Generic;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public class MapNodeReward_Spell : MapNodeReward
{
    [SerializeField] private Spell _spellPrefab;
	[SerializeField] private float _destroyDelay;

	
    public override void Trigger(MapNode fromNode)
    {
        Spell spell = Instantiate(_spellPrefab, Vector3.zero, Quaternion.identity);
		spell.Init(Scenario.Instance.Player);
		
        StartCoroutine(Run(spell));
    }

    private IEnumerator Run(Spell spell)
    {
		yield return spell.CastSpell();
        yield return new WaitForSeconds(_destroyDelay);
		
        Destroy(gameObject);
    }
}
