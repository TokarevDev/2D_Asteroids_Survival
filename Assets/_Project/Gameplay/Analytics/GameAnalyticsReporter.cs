using System;
using Game.Core.Analytics;
using Game.Gameplay.Score;
using Game.Gameplay.Session;
using Game.Gameplay.Signals;
using Zenject;

namespace Game.Gameplay.Analytics
{
    public sealed class GameAnalyticsReporter : IInitializable, IDisposable
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly SignalBus _signalBus;
        private readonly ScoreCounter _scoreCounter;
        private readonly SurvivalTimer _survivalTimer;

        private bool _isGameEnded;

        public GameAnalyticsReporter(IAnalyticsService analyticsService, SignalBus signalBus, ScoreCounter scoreCounter,
            SurvivalTimer survivalTimer)
        {
            _analyticsService = analyticsService ?? throw new ArgumentNullException(nameof(analyticsService));

            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));

            _scoreCounter = scoreCounter ?? throw new ArgumentNullException(nameof(scoreCounter));

            _survivalTimer = survivalTimer ?? throw new ArgumentNullException(nameof(survivalTimer));
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
            _analyticsService.LogGameStarted();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        private void OnPlayerDied()
        {
            if (_isGameEnded)
            {
                return;
            }

            _isGameEnded = true;

            _analyticsService.LogGameEnded(_scoreCounter.Score, _survivalTimer.ElapsedTime);
        }
    }
}
