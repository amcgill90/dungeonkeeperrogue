using System.Collections;
using UnityEngine;

namespace DungeonKeeperRogue
{
    public class TurnController : MonoBehaviour
    {
        [SerializeField] private Team _firstTeam = Team.Player;

        private Team _currentTeam;
        private Coroutine _currentAction;

        public delegate void TurnDelegate(Team team);
        public static event TurnDelegate OnTurnStart;
        public static event TurnDelegate OnTurnEnd;

        private void Awake()
        {
            TurnAction.OnActionTriggered += OnTurnActionTriggered;
        }

        private void Start()
        {
            _currentTeam = _firstTeam;
            StartTurn();
        }

        private void OnDestroy()
        {
            TurnAction.OnActionTriggered -= OnTurnActionTriggered;
        }
        
        private void OnTurnActionTriggered(TurnAction action)
        {
            StartTurnAction(action);
        }
        
        private void EndTurnAndStartNext()
        {
            OnTurnEnd?.Invoke(_currentTeam);
            _currentTeam = _currentTeam == Team.Player ? Team.Enemy :  Team.Player;
            StartTurn();
        }

        private void StartTurn()
        {
            OnTurnStart?.Invoke(_currentTeam);
        }

        private void StartTurnAction(TurnAction action)
        {
            _currentAction ??= StartCoroutine(RunAction(action));
        }

        private IEnumerator RunAction(TurnAction action)
        {
            yield return action.Run();
            _currentAction = null;
            EndTurnAndStartNext();
        }
    }
}