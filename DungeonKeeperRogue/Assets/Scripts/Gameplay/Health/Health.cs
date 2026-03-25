using System;
using PrototypingTools;
using UnityEngine;
using UnityEngine.Events;

namespace DungeonKeeperRogue.Gameplay
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private ProgressBar _progressBar;
        [SerializeField] private FlyingText _defaultFlyingTextPrefab;
		[SerializeField] private FlyingText _healFlyingTextPrefab;
        [SerializeField] private Transform _flyingTextSpawnPoint;
        [SerializeField] private float _flyingTextSpawnRadius;
        [SerializeField] private bool _showFlyingTextSpawnGizmo;

        [Header("Events")]
        [SerializeField] private UnityEvent _onTakeDamageEvent;
        [SerializeField] private UnityEvent _onHealEvent;
        
        private Team _team;
        private int _invincibilityStacks;
        private int _maxHealth;
        private int _currentHealth;
        
        public bool IsDead => _currentHealth <= 0;
        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        public bool IsFullHealth => _currentHealth >= _maxHealth;
        public bool IsInvincible => _invincibilityStacks > 0;
        public float CurrentHealthFrac => (float)_currentHealth / _maxHealth;
        public Team Team => _team;

        public event Action<DamageDetails> OnDamageAttempted;
		public static event Action<Health, DamageDetails> OnDamageAttemptedAll;
        public event Action<DamageDetails> OnDamageTaken;
        public event Action<int> OnHeal;
        public event Action OnHealthChanged;
        public event Action OnDeath;

        public void Init(int maxHealth, Team team)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _team = team;

            if (_progressBar)
            {
                _progressBar.Init(maxHealth, _currentHealth);
            }
        }
    
        private void AdjustHealth(int amount)
        {
            if (amount == 0)
            {
                return;
            }
            
            _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);

            if (_progressBar)
            {
                _progressBar.UpdateProgress(_currentHealth);
            }
            
            OnHealthChanged?.Invoke();
        }

		public void AdjustMaxHealth(int amount)
		{
			if (amount == 0)
			{
				return;
			}

			_maxHealth += amount;
			
			if (_progressBar)
			{
				// re-init progress bar to have new max value
				_progressBar.Init(_maxHealth, _currentHealth);
			}

			AdjustHealth(amount); // restore same amount of health
		}

        public virtual int TryDamage(DamageDetails details)
        {
            if (IsInvincible)
            {
                return 0;
            }
        
            OnDamageAttempted?.Invoke(details);
			OnDamageAttemptedAll?.Invoke(this, details);
            
            int baseDamage = (int)details._damage;
            if (baseDamage <= 0)
            {
                return 0;
            }
            
            AdjustHealth(-baseDamage);
			
            OnDamageTaken?.Invoke(details);
            _onTakeDamageEvent?.Invoke();

            if (_flyingTextSpawnPoint && details._showFlyingText)
            {
	            var flyingTextPrefab = details._flyingTextPrefabOverride
		            ? details._flyingTextPrefabOverride
		            : _defaultFlyingTextPrefab;

	            if (flyingTextPrefab)
	            {
		            Vector2 spawnPos = GetFlyingTextSpawnPos();

		            var flyingText = Instantiate(flyingTextPrefab, spawnPos, Quaternion.identity);
		            flyingText.Init(baseDamage.ToString(), details._flyingTextFontSizeMultiplier);
	            }
            }

            if (IsDead)
            {
                OnDeath?.Invoke();
            }
            
            return baseDamage;
        }

        public virtual void TryHeal(int amount)
        {
            var healAmount = Mathf.Min(amount, _maxHealth - _currentHealth);
            
            Heal(healAmount);
        }

        private void Heal(int health)
        {
			if (health == 0)
			{
				return;
			}

			if (_healFlyingTextPrefab)
			{
				Vector2 spawnPos = GetFlyingTextSpawnPos();

				var flyingText = Instantiate(_healFlyingTextPrefab, spawnPos, Quaternion.identity);
				flyingText.Init(health.ToString(), 1f);
			}

            AdjustHealth(health);
            OnHeal?.Invoke(health);
            _onHealEvent?.Invoke();
        }

        public void SetInvincibility(bool setInvincible)
        {
            _invincibilityStacks = Mathf.Max(setInvincible ? _invincibilityStacks + 1 : _invincibilityStacks - 1, 0);
        }

        public void SetDead()
        {
            _currentHealth = 0;
            
            OnDeath?.Invoke();
        }

        private Vector2 GetFlyingTextSpawnPos()
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * _flyingTextSpawnRadius;
            return (Vector2)_flyingTextSpawnPoint.position + offset; 
        }

        private void OnDrawGizmosSelected()
        {
            if (_flyingTextSpawnPoint == false || _showFlyingTextSpawnGizmo == false)
            {
                return;
            }
            
            Gizmos.color = Color.paleVioletRed;
            Gizmos.DrawSphere(_flyingTextSpawnPoint.position, _flyingTextSpawnRadius);
        }
    }
}