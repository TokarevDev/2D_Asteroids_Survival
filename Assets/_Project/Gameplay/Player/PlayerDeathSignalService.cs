using System;
using Zenject;

namespace Game.Gameplay
{
    public sealed class PlayerDeathSignalService : IInitializable, IDisposable
    {
        private readonly PlayerHealth _playerHealth;
        private readonly SignalBus _signalBus;

        public PlayerDeathSignalService(PlayerHealth playerHealth, SignalBus signalBus)
        {
            _playerHealth = playerHealth;
            _signalBus = signalBus;
        }

        public void Dispose()
        {
            _playerHealth.Died -= OnPlayerDied;
        }

        public void Initialize()
        {
            _playerHealth.Died += OnPlayerDied;
        }

        private void OnPlayerDied()
        {
            _signalBus.Fire<PlayerDiedSignal>();
        }
    }
}
