using System.Collections;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public class MapUnitBehaviour_MoveInDirection : MapUnitBehaviour
{
    [SerializeField] private float _moveSeconds;
    [SerializeField] private Vector2 _moveDirection;
    
    public override IEnumerator OnStartOfTurn(MapUnit unit, MapUnitBehaviourContext context)
    {
        Vector2 moveDir = _moveDirection.normalized;
        moveDir.x *= Scenario.Instance.Map.NodeXInc;
        moveDir.y *= Scenario.Instance.Map.NodeYInc;
        
        Vector2 currentPosition = unit.transform.position;
        Vector2 moveTarget = (Vector2)unit.transform.position + moveDir;
        
        float elapsedTime = 0f;
        while (elapsedTime < _moveSeconds)
        {
            elapsedTime += Time.deltaTime;
            unit.SetPosition(Vector2.Lerp(currentPosition, moveTarget, elapsedTime / _moveSeconds));
            yield return null;
        }
    }
}