using System;
using Cysharp.Threading.Tasks;
using Game.Core.Application;
using Game.Core.Navigation;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public sealed class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;
        private IApplicationQuitService _applicationQuitService;

        private GameNavigationFacade _navigationFacade;

        [Inject]
        private void Construct(GameNavigationFacade navigationFacade, IApplicationQuitService applicationQuitService)
        {
            _navigationFacade = navigationFacade ?? throw new ArgumentNullException(nameof(navigationFacade));
            _applicationQuitService = applicationQuitService
                                      ?? throw new ArgumentNullException(nameof(applicationQuitService));
        }

        private void OnEnable()
        {
            _navigationFacade.TransitionStateChanged += OnTransitionStateChanged;

            _startButton.onClick.AddListener(LoadGameScene);
            _exitButton.onClick.AddListener(ExitGame);

            OnTransitionStateChanged(_navigationFacade.IsTransitionInProgress);
        }

        private void OnDisable()
        {
            _navigationFacade.TransitionStateChanged -= OnTransitionStateChanged;

            _startButton.onClick.RemoveListener(LoadGameScene);
            _exitButton.onClick.RemoveListener(ExitGame);
        }

        private void OnTransitionStateChanged(bool isTransitionInProgress)
        {
            _startButton.interactable = !isTransitionInProgress;
        }

        private void LoadGameScene()
        {
            _navigationFacade.StartGameAsync().Forget(Debug.LogException);
        }

        private void ExitGame()
        {
            _applicationQuitService.Quit();
        }
    }
}
