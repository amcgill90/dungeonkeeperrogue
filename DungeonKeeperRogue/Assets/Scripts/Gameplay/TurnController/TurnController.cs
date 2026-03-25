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
		private bool _isRunningEndOfTurn = false;

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

        private void Start()
        {
            _currentTeam = _firstTeam;
            StartTurn();
        }

        private void Update()
        {
            if (_isRunning && _isInitialised && _isRunningEndOfTurn == false && IsTurnComplete())
            {
                EndTurnAndStartNext();
            }
        }
        
        private void EndTurnAndStartNext()
        {
            StartCoroutine(RunEndTurn());
        }

		private IEnumerator RunEndTurn()
		{
			_isRunningEndOfTurn = true;

			yield return _scenario.GetMapActorForTeam(_currentTeam).OnTurnEnd();

			OnTurnEnd?.Invoke(_currentTeam);

			_isRunningEndOfTurn = false;

			Team winningTeam;
			if (_scenario.IsComplete(out winningTeam))
			{
				_isRunning = false;
				yield break;
			}

			_currentTeam = _currentTeam == Team.Player ? Team.Enemy :  Team.Player;
            StartTurn();
		}

        private void StartTurn()
        {
            StartCoroutine(RunStartTurn());
        }

		private IEnumerator RunStartTurn()
		{
			Debug.Log($"[TurnController]: starting turn {_currentTeam}");
			
			OnTurnStart?.Invoke(_currentTeam);

			yield return _scenario.GetMapActorForTeam(_currentTeam).OnTurnStart();
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
