using System;
using Game.Gameplay.Player;
using Zenject;

namespace Game.UI.HUD
{
    public sealed class HealthViewModel : IInitializable, IDisposable
    {
        public event Action<int, int> HealthChanged;

        private readonly PlayerHealth _playerHealth;

        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }
        public bool IsInitialized { get; private set; }

        public HealthViewModel(PlayerHealth playerHealth)
        {
            _playerHealth = playerHealth ?? throw new ArgumentNullException(nameof(playerHealth));
        }

        public void Initialize()
        {
            _playerHealth.HealthChanged += OnHealthChanged;

            IsInitialized = true;
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
