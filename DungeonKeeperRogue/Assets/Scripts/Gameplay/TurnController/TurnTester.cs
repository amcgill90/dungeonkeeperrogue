using DungeonKeeperRogue;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnTester : MonoBehaviour
{
    private bool _waitingForInput;
    private WaitTurnAction _turnAction;
    
    private void Awake()
    {
        _turnAction = new WaitTurnAction(5f);
        
        TurnController.OnTurnStart += OnTurnStart;
        TurnController.OnTurnEnd += OnTurnEnd;
    }

    private void OnDestroy()
    {
        TurnController.OnTurnStart -= OnTurnStart;
        TurnController.OnTurnEnd -= OnTurnEnd;
    }
    
    private void OnTurnEnd(Team team)
    {
        Debug.Log($"[TurnTester]: ending {team}'s turn");
    }

    private void OnTurnStart(Team team)
    {
        Debug.Log($"[TurnTester]: starting {team}'s turn");

        _waitingForInput = team == Team.Player;
        
        if (_waitingForInput == false)
        {
            _turnAction.Trigger();
        }
    }

    private void Update()
    {
        if (_waitingForInput && Keyboard.current[Key.Space].wasPressedThisFrame)
        {
             _turnAction.Trigger();
            _waitingForInput = false;
        }
    }
}
