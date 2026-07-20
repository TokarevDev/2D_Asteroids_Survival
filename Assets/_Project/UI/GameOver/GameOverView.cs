using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;

        private GameOverViewModel _viewModel;

        [Inject]
        private void Construct(GameOverViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Awake()
        {
            if (_gameOverPanel == null)
            {
                Debug.LogError("Game over panel reference is missing", this);
                enabled = false;
            }

            if (_restartButton == null)
            {
                Debug.LogError("Restart button reference is missing", this);
                enabled = false;
                return;
            }

            if (_mainMenuButton == null)
            {
                Debug.LogError("Main menu button reference is missing", this);
                enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            _viewModel.VisibilityChanged += OnVisibilityChanged;
            _viewModel.InteractabilityChanged += OnInteractabilityChanged;

            _restartButton.onClick.AddListener(RestartGame);
            _mainMenuButton.onClick.AddListener(ReturnToMainMenu);

            OnVisibilityChanged(_viewModel.IsVisible);
            OnInteractabilityChanged(_viewModel.IsInteractable);
        }

        private void OnDisable()
        {
            _viewModel.VisibilityChanged -= OnVisibilityChanged;
            _viewModel.InteractabilityChanged -= OnInteractabilityChanged;

            _restartButton.onClick.RemoveListener(RestartGame);
            _mainMenuButton.onClick.RemoveListener(ReturnToMainMenu);
        }

        private void RestartGame()
        {
            _viewModel.RestartAsync().Forget(Debug.LogException);
        }

        private void ReturnToMainMenu()
        {
            _viewModel.ReturnToMainMenuAsync().Forget(Debug.LogException);
        }

        private void OnVisibilityChanged(bool isVisible)
        {
            _gameOverPanel.SetActive(isVisible);
        }

        private void OnInteractabilityChanged(bool isInteractable)
        {
            _restartButton.interactable = isInteractable;
            _mainMenuButton.interactable = isInteractable;
        }
    }
}
