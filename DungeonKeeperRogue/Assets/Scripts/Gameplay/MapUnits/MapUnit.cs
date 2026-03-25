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
		public Health Health => _health;
        public bool IsRunningBehaviour => _activeBehaviour != null;
        public Vector2 Position => transform.position;
        
        public delegate void MapUnitEvent(MapUnit unit);
        public event MapUnitEvent OnDestroyed;

        private void Start()
        {
            _team = _config.Team;

			if (_health != null)
			{
            	_health.Init(_config.BaseHealth, _team);
			}

            _spriteRenderer.sprite = _config.Sprite;
			_behaviour.Init();

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
			if (_health == null)
			{
				return;
			}

            _health.TryDamage(details);
        }

		public IEnumerator RunStartOfTurnBehaviour()
		{
			yield return _behaviour.OnStartOfTurn(this);
		}
        
        public IEnumerator RunEndOfTurnBehaviour()
        {
            yield return _behaviour.OnEndOfTurn(this);
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }
    }
}