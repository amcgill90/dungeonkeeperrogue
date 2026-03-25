using System.Collections;
using UnityEngine;

namespace DungeonKeeperRogue.Gameplay
{
    public class MapUnit : MonoBehaviour, IDamageable
    {
        [SerializeField] private MapUnitConfig _config;
        [SerializeField] private Health _health;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private MapUnitBehaviour _behaviour;

        private Team _team;
        private Coroutine _activeBehaviour;
        private MapNode _currentMapNode;

        public Team Team => _team;
        public bool IsRunningBehaviour => _activeBehaviour != null;
        
        public delegate void MapUnitEvent(MapUnit unit);
        public event MapUnitEvent OnDestroyed;

        private void Start()
        {
            _team = _config.Team;
            _health.Init(_config.BaseHealth, _team);
            _spriteRenderer.sprite = _config.Sprite;

            if (Scenario.Instance)
            {
                Scenario.Instance.RegisterUnit(this);  
            }
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
        }

        public void ProcessDamage(DamageDetails details)
        {
            _health.TryDamage(details);
        }
        
        public void RunBehavior()
        {
            _activeBehaviour ??= StartCoroutine(RunBehaviorRoutine());
        }
        
        private IEnumerator RunBehaviorRoutine()
        {
            yield return _behaviour.Run(this);
            _activeBehaviour = null;
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }
    }
}