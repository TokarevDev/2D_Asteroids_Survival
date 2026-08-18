using System;
using Cysharp.Threading.Tasks;
using Game.Core.Navigation;
using Game.Gameplay.Score;
using Game.Gameplay.Signals;
using Zenject;

namespace Game.UI.GameOver
{
    public sealed class GameOverViewModel : IInitializable, IDisposable
    {
        public event Action<bool> VisibilityChanged;
        public event Action<bool> InteractabilityChanged;

        private readonly SignalBus _signalBus;
        private readonly ScoreCounter _scoreCounter;
        private readonly GameNavigationFacade _navigationFacade;

        public int FinalScore { get; private set; }
        public bool IsVisible { get; private set; }

        public bool IsInteractable => !_navigationFacade.IsTransitionInProgress;

        public GameOverViewModel(SignalBus signalBus, GameNavigationFacade navigationFacade, ScoreCounter scoreCounter)
        {
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _navigationFacade = navigationFacade ?? throw new ArgumentNullException(nameof(navigationFacade));
            _scoreCounter = scoreCounter ?? throw new ArgumentNullException(nameof(scoreCounter));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Initialize()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
            _navigationFacade.TransitionStateChanged += OnTransitionStateChanged;

            OnTransitionStateChanged(_navigationFacade.IsTransitionInProgress);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
            _navigationFacade.TransitionStateChanged -= OnTransitionStateChanged;
        }

        public UniTask RestartAsync()
        {
            return _navigationFacade.RestartGameAsync();
        }

        public UniTask ReturnToMainMenuAsync()
        {
            return _navigationFacade.ReturnToMainMenuAsync();
        }

        private void OnTransitionStateChanged(bool isTransitionInProgress)
        {
            InteractabilityChanged?.Invoke(!isTransitionInProgress);
        }

        private void OnPlayerDied()
        {
            if (IsVisible)
            {
                return;
            }

            FinalScore = _scoreCounter.Score;
            IsVisible = true;

            VisibilityChanged?.Invoke(IsVisible);
        }
    }
}
