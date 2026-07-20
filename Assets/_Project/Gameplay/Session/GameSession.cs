using System;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class GameSession : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;

        private bool _isEnded;

        public GameSession(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Initialize()
        {
            Time.timeScale = 1f;
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
            Time.timeScale = 1f;
        }

        private void OnPlayerDied()
        {
            if (_isEnded)
            {
                return;
            }

            _isEnded = true;
            Time.timeScale = 0f;
        }
    }
}
