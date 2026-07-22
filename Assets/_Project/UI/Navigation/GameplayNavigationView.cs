using Cysharp.Threading.Tasks;
using Game.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public sealed class GameplayNavigationView : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;

        private ISceneLoader _sceneLoader;
        private bool _isTransitionInProgress;

        [Inject]
        private void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            if (_mainMenuButton != null)
            {
                return;
            }

            Debug.LogError("Main menu button reference is missing", this);
            enabled = false;
        }

        private void OnEnable()
        {
            _mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }

        private void OnDisable()
        {
            _mainMenuButton.onClick.RemoveListener(ReturnToMainMenu);
        }

        private void ReturnToMainMenu()
        {
            if (_isTransitionInProgress)
            {
                return;
            }

            _isTransitionInProgress = true;
            _mainMenuButton.interactable = false;

            ReturnToMainMenuAsync().Forget(Debug.LogException);
        }

        private async UniTask ReturnToMainMenuAsync()
        {
            try
            {
                await _sceneLoader.LoadMainMenuAsync();
            }
            catch
            {
                _isTransitionInProgress = false;
                if (_mainMenuButton != null)
                {
                    _mainMenuButton.interactable = true;
                }

                throw;
            }
        }
    }
}
