using System.Collections.Generic;
using DungeonKeeperRogue.UI;
using PrototypingTools.Utils;
using UnityEngine;

namespace DungeonKeeperRogue.Gameplay
{
    public class Scenario : MonoSingleton<Scenario>
    {
        [SerializeField] private NodeTag _playerBaseNodeTag;
        
        [Header("Actors")]
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private MapActor _enemyPrefab;
        
        [Header("Scenario Components")]
        [SerializeField] private Map _map;
        [SerializeField] private TurnController _turnController;
		[SerializeField] private CardList _winRewardOptions;
		[SerializeField] private int _winRewardCount = 3;
        [SerializeField] private UIScenarioOutcomePopup _outcomePopup;

        private bool _isScenarioComplete;
        private Team _cachedWinner;
        private Player _player;
        private MapActor _enemy;
        private MapNode _playerBaseNode;
        
        public Map Map => _map;
        public Player Player => _player;
        public MapActor Enemy => _enemy;
        public TurnController TurnController => _turnController;

        public delegate void ScenarioEndEvent(Team winner);
        public static event ScenarioEndEvent OnScenarioEnd;
        
        protected override void OnInitialized()
        {
            _player = SpawnMapActor(_playerPrefab);
            _enemy = SpawnMapActor(_enemyPrefab);
            _turnController.Init(this);
        }

        private void Start()
        {
            _playerBaseNode = _map.FindNodeWithTag(_playerBaseNodeTag);
        }

        private void Update()
        {
            IsComplete();
        }
        
        public bool IsComplete()
        {
            if (_isScenarioComplete)
            {
                return true;
            }

            Team winner = Team.Neutral;
            if (_enemy.UnitCount == 0)
            {
                winner = Team.Player;
                _isScenarioComplete = true;
            }
            else
            {
                foreach (MapUnit unit in _enemy.Units)
                {
                    Vector2 dist = _playerBaseNode.WorldPos - unit.Position;
                    if (Mathf.Approximately(dist.magnitude, 0f) == false)
                    {
                        continue;
                    }
                    
                    winner = Team.Enemy;
                    _isScenarioComplete = true;
                    break;
                }
            }

            if (_isScenarioComplete == false)
            {
                return false;
            }
            
            //Debug.Log($"[Scenario]: scenario won by {winner}!");
            
            _cachedWinner = winner;

			List<Card> rewardOptions = new(_winRewardOptions.Cards);
			rewardOptions.Shuffle();
			while (rewardOptions.Count > _winRewardCount)
			{
				rewardOptions.RemoveAt(rewardOptions.Count - 1);
			}
			
            _outcomePopup.Open(winner, rewardOptions, (Card card) => {
				if (card != null)
				{
					PlayerManager.PlayerState.Deck.Add(card);
				}
				OnScenarioEnd?.Invoke(_cachedWinner);
			});
            _isScenarioComplete = true;
			return true;
        }

        private T SpawnMapActor<T>(T prefab) where T : MapActor
        {
            var mapActor = Instantiate(prefab, transform);
            mapActor.Init();
            return mapActor;
        }

        public void RegisterUnit(MapUnit unit)
        {
            (unit.Team == Team.Player ? _player : _enemy).RegisterUnit(unit);
        }

		public MapActor GetMapActorForTeam(Team team)
		{
			return team == Team.Player ? _player : _enemy;
		}
    }
}
