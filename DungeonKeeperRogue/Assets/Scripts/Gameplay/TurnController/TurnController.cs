using System.Collections;
using UnityEngine;

namespace DungeonKeeperRogue.Gameplay
{
    public class TurnController : MonoBehaviour
    {
        [SerializeField] private Team _firstTeam = Team.Player;

        private bool _isRunning;
        private bool _isInitialised;
        private Team _currentTeam;
        private Scenario _scenario;
        private Coroutine _currentAction;

        public Team CurrentTeam => _currentTeam;
        
        public delegate void TurnDelegate(Team team);
        public static event TurnDelegate OnTurnStart;
        public static event TurnDelegate OnTurnEnd;
        
        public void Init(Scenario scenario)
        {
            _isRunning = true;
            _scenario = scenario;
            _isInitialised = true;
        }
        
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

        private void Update()
        {
            if (_isRunning && _isInitialised && _currentAction == null && IsTurnComplete())
            {
                EndTurnAndStartNext();
            }
        }
        
        public void Stop()
        {
            _isRunning = false;
            
            if (_currentAction != null) 
            {
                StopCoroutine(_currentAction);
            }

            _currentAction = null;
        }
        
        private void EndTurnAndStartNext()
        {
            OnTurnEnd?.Invoke(_currentTeam);
            _currentTeam = _currentTeam == Team.Player ? Team.Enemy : Team.Player;
            StartTurn();
        }

        private void StartTurn()
        {
            Debug.Log($"[TurnController]: starting turn {_currentTeam}");
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
        }

        private bool IsTurnComplete()
        {
            switch (_currentTeam)
            {
                case Team.Player:
                    return _scenario.Player.IsTurnComplete;
                case Team.Enemy:
                    return _scenario.Enemy.IsTurnComplete;
                case Team.Neutral:
                default:
                    return true;
            }
        }
    }
}