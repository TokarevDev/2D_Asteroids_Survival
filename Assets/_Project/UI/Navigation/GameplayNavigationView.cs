using System;
using Cysharp.Threading.Tasks;
using Game.Core.Navigation;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI.Navigation
{
    public sealed class GameplayNavigationView : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;

        private GameNavigationFacade _navigationFacade;

        [Inject]
        private void Construct(GameNavigationFacade navigationFacade)
        {
            _navigationFacade = navigationFacade ?? throw new ArgumentNullException(nameof(navigationFacade));
        }

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
            }
        }

        private void OnEnable()
        {
            _navigationFacade.TransitionStateChanged += OnTransitionStateChanged;
            _mainMenuButton.onClick.AddListener(ReturnToMainMenu);

            OnTransitionStateChanged(_navigationFacade.IsTransitionInProgress);
        }

        private void OnDisable()
        {
            _navigationFacade.TransitionStateChanged -= OnTransitionStateChanged;
            _mainMenuButton.onClick.RemoveListener(ReturnToMainMenu);
        }

        private void OnTransitionStateChanged(bool isTransitionInProgress)
        {
            _mainMenuButton.interactable = !isTransitionInProgress;
        }

        private void ReturnToMainMenu()
        {
            _navigationFacade.ReturnToMainMenuAsync().Forget(Debug.LogException);
        }

        private bool ValidateSerializedReferences()
        {
            if (_mainMenuButton != null)
            {
                return true;
            }

            Debug.LogError("Main menu button reference is missing", this);
            return false;
        }
    }
}
