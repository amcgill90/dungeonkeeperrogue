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

        private bool _isScenarioComplete;
        private Player _player;
        private MapActor _enemy;
        private MapNode _playerBaseNode;
        
        public Map Map => _map;
        public Player Player => _player;
        public MapActor Enemy => _enemy;
        public TurnController TurnController => _turnController;

        public delegate void ScenarioEndEvent(Team winner);
        public event ScenarioEndEvent OnScenarioEnd;
        
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
        
        private void LateUpdate()
        {
            UpdateScenarioOutcome();
        }

        private void UpdateScenarioOutcome()
        {
            if (_isScenarioComplete)
            {
                return;
            }
            
            Team winningTeam = Team.Neutral;
            if (_enemy.UnitCount == 0)
            {
                winningTeam = Team.Player;
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
                    
                    winningTeam = Team.Enemy;
                    _isScenarioComplete = true;
                    break;
                }
            }

            if (_isScenarioComplete == false)
            {
                return;
            }
            
            Debug.Log($"[Scenario]: scenario won by {winningTeam}!");
            _turnController.Stop();
            OnScenarioEnd?.Invoke(winningTeam);
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
    }
}