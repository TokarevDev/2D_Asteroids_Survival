using UnityEngine;
using Zenject;

namespace Game.UI
{
    public sealed class HudVisibilityView : MonoBehaviour
    {
        [SerializeField] private GameObject _hudRoot;

        private GameOverViewModel _gameOverViewModel;

        [Inject]
        private void Construct(GameOverViewModel gameOverViewModel)
        {
            _gameOverViewModel = gameOverViewModel;
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
            _gameOverViewModel.VisibilityChanged += OnGameOverVisibilityChanged;
            OnGameOverVisibilityChanged(_gameOverViewModel.IsVisible);
        }

        private void OnDisable()
        {
            _gameOverViewModel.VisibilityChanged -= OnGameOverVisibilityChanged;
        }

        private void OnGameOverVisibilityChanged(bool isGameOverVisible)
        {
            _hudRoot.SetActive(!isGameOverVisible);
        }

        private bool ValidateSerializedReferences()
        {
            if (_hudRoot != null)
            {
                return true;
            }

            Debug.LogError("HUD root reference is missing", this);
            return false;
        }
    }
}
