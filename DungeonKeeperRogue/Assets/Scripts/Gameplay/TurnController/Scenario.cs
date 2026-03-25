using UnityEngine;

namespace DungeonKeeperRogue.Gameplay
{
    public class Scenario : MonoSingleton<Scenario>
    {
        [Header("Actors")]
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private MapActor _enemyPrefab;
        
        [Header("Scenario Components")]
        [SerializeField] private Map _map;
        [SerializeField] private TurnController _turnController;

        private Player _player;
        private MapActor _enemy;
        
        public Player Player => _player;
        public MapActor Enemy => _enemy;
        public Map Map => _map;
        public TurnController TurnController => _turnController;
        
        protected override void OnInitialized()
        {
            _player = SpawnMapActor(_playerPrefab);
            _enemy = SpawnMapActor(_enemyPrefab);
            _turnController.Init(this);
            
            TurnController.OnTurnEnd += OnTurnEnd;
        }

        private void OnDestroy()
        {
            TurnController.OnTurnEnd -= OnTurnEnd;
        }

        private void OnTurnEnd(Team team)
        {
            //Check if someone has won?
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