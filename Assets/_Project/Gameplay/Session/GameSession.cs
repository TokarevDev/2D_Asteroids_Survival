using System;
using Game.Gameplay.Signals;
using Zenject;

namespace Game.Gameplay.Session
{
    public sealed class GameSession : IInitializable, IDisposable
    {
        private readonly GamePauseService _gamePauseService;
        private readonly SignalBus _signalBus;

        private bool _isEnded;

        public GameSession(SignalBus signalBus, GamePauseService gamePauseService)
        {
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _gamePauseService = gamePauseService ?? throw new ArgumentNullException(nameof(gamePauseService));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Initialize()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        private void OnPlayerDied()
        {
            if (_isEnded)
            {
                return;
            }

            _isEnded = true;
            _gamePauseService.Pause();
        }
    }
}
