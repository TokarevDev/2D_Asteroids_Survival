using System;
using Game.Gameplay;
using Zenject;

namespace Game.UI
{
    public sealed class HealthViewModel : IInitializable, IDisposable
    {
        private readonly PlayerHealth _playerHealth;

        public event Action<int, int> HealthChanged;

        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }

        public HealthViewModel(PlayerHealth playerHealth)
        {
            _playerHealth = playerHealth;
        }

        public void Initialize()
        {
            _playerHealth.HealthChanged += OnHealthChanged;

            OnHealthChanged(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
        }

        public void Dispose()
        {
            _playerHealth.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int currentHealth, int maxHealth)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;

            HealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
    }
}
