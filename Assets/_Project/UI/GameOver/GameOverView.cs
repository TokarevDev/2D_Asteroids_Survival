using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public sealed class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private TMP_Text _finalScoreText;

        private GameOverViewModel _viewModel;

        [Inject]
        private void Construct(GameOverViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
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
            if (isVisible)
            {
                _finalScoreText.SetText("Score: {0}", _viewModel.FinalScore);
            }

            _gameOverPanel.SetActive(isVisible);
        }

        private void OnInteractabilityChanged(bool isInteractable)
        {
            _restartButton.interactable = isInteractable;
            _mainMenuButton.interactable = isInteractable;
        }

        private bool ValidateSerializedReferences()
        {
            bool isValid = true;

            if (_gameOverPanel == null)
            {
                Debug.LogError("Game over panel reference is missing", this);
                isValid = false;
            }

            if (_restartButton == null)
            {
                Debug.LogError("Restart button reference is missing", this);
                isValid = false;
            }

            if (_mainMenuButton == null)
            {
                Debug.LogError("Main menu button reference is missing", this);
                isValid = false;
            }

            if (_finalScoreText == null)
            {
                Debug.LogError("Final score text reference is missing", this);
                isValid = false;
            }

            return isValid;
        }
    }
}
