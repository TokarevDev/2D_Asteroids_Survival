using System;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class PlayerHealth : MonoBehaviour, IDamageable
    {
        public event Action<int, int> HealthChanged;
        public event Action Died;

        [SerializeField, Min(1)] private int _maxHealth = 3;

        private readonly Health _health = new();

        public int MaxHealth => _health.MaxHealth;
        public int CurrentHealth => _health.CurrentHealth;
        public bool IsDead => _health.IsDead;

        private void Awake()
        {
            _health.Changed += OnHealthChanged;
            _health.Died += OnDied;

            _health.Initialize(_maxHealth);
        }

        private void OnDestroy()
        {
            _health.Changed -= OnHealthChanged;
            _health.Died -= OnDied;
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
        }

        private void OnHealthChanged(int currentHealth, int maxHealth)
        {
            HealthChanged?.Invoke(currentHealth, maxHealth);
        }

        private void OnDied()
        {
            Died?.Invoke();
        }
    }
}
