using Cysharp.Threading.Tasks;
using Game.Core;
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

        private ISceneLoader _sceneLoader;

        [Inject]
        private void Construct(ISceneLoader sceneLoader, IApplicationQuitService applicationQuitService)
        {
            _applicationQuitService = applicationQuitService;
            _sceneLoader = sceneLoader;
        }

        private void OnEnable()
        {
            _startButton.onClick.AddListener(LoadGameScene);
            _exitButton.onClick.AddListener(ExitGame);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(LoadGameScene);
            _exitButton.onClick.RemoveListener(ExitGame);
        }

        private void LoadGameScene()
        {
            _startButton.interactable = false;
            LoadGameSceneAsync().Forget(Debug.LogException);
        }

        private async UniTask LoadGameSceneAsync()
        {
            try
            {
                await _sceneLoader.LoadGameAsync();
            }
            catch
            {
                if (_startButton != null)
                {
                    _startButton.interactable = true;
                }

                throw;
            }
        }

        private void ExitGame()
        {
            _applicationQuitService.Quit();
        }
    }
}
