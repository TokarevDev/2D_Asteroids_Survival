using System;
using Game.Core.Configuration;
using Game.Gameplay.Combat;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerHealth : MonoBehaviour, IDamageable
    {
        public event Action<int, int> HealthChanged;
        public event Action Died;

        private readonly Health _health = new();

        private IGameConfigProvider _configProvider;

        public int MaxHealth => _health.MaxHealth;
        public int CurrentHealth => _health.CurrentHealth;
        public bool IsDead => _health.IsDead;

        [Inject]
        private void Construct(IGameConfigProvider configProvider)
        {
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
        }

        private void Awake()
        {
            _health.Changed += OnHealthChanged;
            _health.Died += OnDied;

            _health.Initialize(_configProvider.Player.MaxHealth);
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
