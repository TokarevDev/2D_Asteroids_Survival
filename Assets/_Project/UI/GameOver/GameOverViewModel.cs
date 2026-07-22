using System;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Gameplay;
using Zenject;

namespace Game.UI
{
    public sealed class GameOverViewModel : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly ISceneLoader _sceneLoader;
        private readonly ScoreCounter _scoreCounter;

        private bool _isTransitionInProgress;

        public event Action<bool> VisibilityChanged;
        public event Action<bool> InteractabilityChanged;

        public int FinalScore { get; private set; }

        public bool IsVisible { get; private set; }
        public bool IsInteractable => _isTransitionInProgress == false;

        public GameOverViewModel(SignalBus signalBus, ISceneLoader sceneLoader, ScoreCounter scoreCounter)
        {
            _signalBus = signalBus;
            _sceneLoader = sceneLoader;
            _scoreCounter = scoreCounter;
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

        public UniTask RestartAsync()
        {
            return LoadSceneAsync(_sceneLoader.LoadGameAsync);
        }

        public UniTask ReturnToMainMenuAsync()
        {
            return LoadSceneAsync(_sceneLoader.LoadMainMenuAsync);
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

        private async UniTask LoadSceneAsync(Func<UniTask> loadScene)
        {
            if (_isTransitionInProgress)
            {
                return;
            }

            _isTransitionInProgress = true;
            InteractabilityChanged?.Invoke(false);

            try
            {
                await loadScene();
            }
            catch
            {
                _isTransitionInProgress = false;
                InteractabilityChanged?.Invoke(true);
                throw;
            }
        }
    }
}
