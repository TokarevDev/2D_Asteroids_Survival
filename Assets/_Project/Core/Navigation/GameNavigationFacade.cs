using System;
using Cysharp.Threading.Tasks;
using Game.Core.Scenes;

namespace Game.Core.Navigation
{
    public sealed class GameNavigationFacade
    {
        public event Action<bool> TransitionStateChanged;
        private readonly ISceneLoader _sceneLoader;

        public bool IsTransitionInProgress { get; private set; }

        public GameNavigationFacade(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
        }

        public UniTask StartGameAsync()
        {
            return NavigateAsync(_sceneLoader.LoadGameAsync);
        }

        public UniTask RestartGameAsync()
        {
            return NavigateAsync(_sceneLoader.LoadGameAsync);
        }

        public UniTask ReturnToMainMenuAsync()
        {
            return NavigateAsync(_sceneLoader.LoadMainMenuAsync);
        }

        private async UniTask NavigateAsync(Func<UniTask> transition)
        {
            if (IsTransitionInProgress)
            {
                return;
            }

            IsTransitionInProgress = true;
            TransitionStateChanged?.Invoke(true);

            try
            {
                await transition();
            }
            finally
            {
                IsTransitionInProgress = false;
                TransitionStateChanged?.Invoke(false);
            }
        }
    }
}
